using GrowthWare.Framework;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GrowthWare.Web.Support.Helpers;

[CLSCompliant(false)]
public class CacheHelper
{
    /*
     * The CacheHelper relys on the filing system to syncronize the cache directory 
     * in order to syncronize cache across multiple servers.
     *
     * Any code that uses the CacheHelper should be aware that GetFromCache will return null
     * if the cache item does not exist. At that point the calling code should do what it needs
     * to get the data then call AddToCache.
     *
     * AddToCache - Adds a value to the cache and creates the corresponding file.
     *  1.) Prepare the cache directory by creating it if need be
     *  2.) Prepare the cache file by creating it if need be it doesn't always need to be created
     *      because the file could have been created by another server
     *  3.) Once the file has been dealt with the following will happen
     *      a.) Calculate the full path of the cache file
     *      b.) Use a per-file change token (isolated per cache entry)
     *      d.) Create a MemoryCacheEntryOptions with the change token
     *      e.) Set the value in the cache
     *
     * changeCallback - Removes the item from m_MemoryCache when the file it represents changes.
     *
     * GetFromCache - Returns the value from the cache or null. Null indicating the item is not in the cache.
     *
     * RemoveAll - Deletes all the files in the cache directory, causing the changeCallback method to be called.
     *
     * RemoveFromCache - Deletes the file it exists, causing the changeCallback method to be called.
     *
     */

    private static CacheHelper m_CacheHelper;
    private static readonly object m_CacheLock = new();
    private string s_CacheDirectory = string.Empty;
    private Logger m_Logger = Logger.Instance();
    private static readonly Mutex m_Mutex = new();
    private MemoryCache m_MemoryCache;
    private int m_NumberOfTimesCacheCallBackWasCalled = 0;
    private PhysicalFileProvider m_FileProvider;

    /// <summary>
    /// Prevent any other instances of this class from being created
    /// </summary>
    private CacheHelper()
    {
        this.s_CacheDirectory = Path.Combine(Environment.CurrentDirectory, "CacheDependency");
        this.prepDirectory();
        this.m_MemoryCache = new MemoryCache(new MemoryCacheOptions());
        this.m_FileProvider = new PhysicalFileProvider(s_CacheDirectory);  // Use a single instance
    }

    /// <summary>
    /// Returns the instance of the CacheHelper class.
    /// </summary>
    /// <returns></returns>
    public static CacheHelper Instance()
    {
        try
        {
            m_Mutex.WaitOne();
            if (m_CacheHelper == null)
            {
                m_CacheHelper = new CacheHelper();
            }
        }
        catch
        {
            throw;
        }
        finally
        {
            m_Mutex.ReleaseMutex();
        }
        return m_CacheHelper;
    }

    /// <summary>
    /// Creates or updates the cache file in order to syncronoize the cache across multiple servers then
    /// adds or updates a value in cache.
    /// </summary>
    /// <param name="cacheName"></param>
    /// <param name="value"></param>
    /// <code language="c#">
    ///     String mCacheName = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture) + "_Functions";
    ///     Collection<MFunctionProfile> mRetVal = m_CacheHelper.GetFromCache<Collection<YourObjectType>>(mCacheName);;
    ///     if (mRetVal == null)
    ///     {
    ///         mRetVal = <get your data from the database>;
    ///         m_CacheHelper.AddToCache(mCacheName, mRetVal);
    ///     }
    ///     return mRetVal;
    /// </code>
    public void AddToCache(string cacheName, object value)
    {
        if (!ConfigSettings.CentralManagement & ConfigSettings.EnableCache)
        {
            lock (m_CacheLock)
            {
                if (prepDirectory() && prepFile(cacheName))
                {
                    // Calculate the full path of the cache file
                    string mFileNameAndPath = getFullFileAndPath(cacheName);
                    // Use a per-file change token (isolated per cache entry)
                    IChangeToken mChangeToken = m_FileProvider.Watch(Path.GetFileName(mFileNameAndPath));
                    // Create memory cache entry options with the change token and Register the change callback
                    MemoryCacheEntryOptions mMemoryCacheEntryOptions = new MemoryCacheEntryOptions()
                        .AddExpirationToken(mChangeToken)
                        .RegisterPostEvictionCallback((key, value, reason, state) => changeCallback(key, value, reason, state), cacheName);
                    // Set the value in the cache
                    m_MemoryCache.Set(cacheName, value, mMemoryCacheEntryOptions);  // Store object, not file content
                }
            }
        }
    }

    /// <summary>
    /// Callback method that is invoked when a cache item is removed from the memory cache.
    /// </summary>
    /// <param name="key">The key of the cache entry that was removed.</param>
    /// <param name="value">The value of the cache entry that was removed.</param>
    /// <param name="reason">The reason for the eviction of the cache entry.</param>
    /// <param name="state">Additional state information passed to the callback.</param>
    private void changeCallback(object key, object value, EvictionReason reason, object state)
    {
        try
        {
            m_NumberOfTimesCacheCallBackWasCalled++;
            m_Logger.Debug($"Call Number: {m_NumberOfTimesCacheCallBackWasCalled} Cache item {state} was removed from the cache because {reason}.");

            // Ensure the cache entry is explicitly removed
            lock (m_CacheLock)
            {
                m_MemoryCache.Remove(key);
            }
        }
        catch (Exception ex)
        {
            m_Logger.Error(ex.Message);
        }
    }

    /// <summary>
    /// Returns the value from the cache or null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cacheName"></param>
    /// <returns></returns>
    public T GetFromCache<T>(string cacheName)
    {
        lock (m_CacheLock)
        {
            m_MemoryCache.TryGetValue(cacheName, out T mRetVal);
            return mRetVal;
        }
    }

    /// <summary>
    /// Constructs a full file path for the cache file associated with the given cache name.
    /// </summary>
    /// <param name="cacheName">The name of the cache for which the file path is to be calculated.</param>
    /// <returns>The full file path of the cache file.</returns>
    private string getFullFileAndPath(string cacheName)
    {
        return Path.Combine(s_CacheDirectory, cacheName + ".txt");
    }

    /// <summary>
    /// Removes all cache by deleting all the files in the cache directory thus triggering the changeCallback.
    /// </summary>
    public void RemoveAll()
    {
        // Delete all the files in the cache directory triggering the changeCallback
        // Hopefully there is some mechanism to syncronize the directory across multiple servers
        DirectoryInfo mDirectoryInfo = new DirectoryInfo(this.s_CacheDirectory);
        lock (m_CacheLock)
        {
            foreach (FileInfo mFileInfo in mDirectoryInfo.GetFiles())
            {
                mFileInfo.Delete();
            }
        }
    }

    /// <summary>
    /// Deletes the file it exists, causing the changeCallback method to be called.
    /// </summary>
    /// <param name="cacheName"></param>
    public void RemoveFromCache(string cacheName)
    {
        string mFileNameAndPath = getFullFileAndPath(cacheName);
        if (File.Exists(mFileNameAndPath))
        {
            File.Delete(mFileNameAndPath);
        }
    }

    /// <summary>
    /// Prepares the cache directory.
    /// </summary>
    /// <returns>False if unable to create cache directory</returns>
    private bool prepDirectory()
    {
        try
        {
            if (!Directory.Exists(s_CacheDirectory))
            {
                Directory.CreateDirectory(s_CacheDirectory);
            }
            return true;
        }
        catch (Exception ex)
        {
            Logger mLogger = Logger.Instance();
            mLogger.Error("Unable to create cache directory");
            mLogger.Error(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Prepares the cache file.
    /// </summary>
    /// <param name="cacheName"></param>
    /// <returns>False if unable to create cache file</returns>
    private bool prepFile(string cacheName)
    {
        try
        {
            string mFileNameAndPath = getFullFileAndPath(cacheName);

            // Only create if it does not exist
            if (!File.Exists(mFileNameAndPath))
            {
                using (var stream = new StreamWriter(mFileNameAndPath))
                {
                    stream.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Logger mLogger = Logger.Instance();
            mLogger.Error("Unable to create cache file");
            mLogger.Error(ex.Message);
            return false;
        }
    }
}

