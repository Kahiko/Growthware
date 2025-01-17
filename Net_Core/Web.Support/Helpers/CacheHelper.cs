using GrowthWare.Framework;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Threading;

namespace GrowthWare.Web.Support.Helpers;

/// <summary>
/// A Facade for Microsoft.Extensions.Caching.Memory and relys on some sort of directory/file management
/// that syncronizes the cache directory across multiple servers.
/// </summary>
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
     *      a.) Create a PhysicalFileProvider object
     *      b.) Create a IChangeToken object using the PhysicalFileProvider.Watch method
     *      c.) Register the changeCallback with the change token to remove the 
     *          item from the cache when the file changes
     *      d.) Create a MemoryCacheEntryOptions with the change token
     *      e.) Add or update the value to the m_MemoryCache using the MemoryCacheEntryOptions object
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
    private static readonly object m_CacheLock = new object();
    private string s_CacheDirectory = string.Empty;
    private static readonly Mutex m_Mutex = new Mutex();
    private IMemoryCache m_MemoryCache;

    /// <summary>
    /// Prevent any other instances of this class from being created
    /// </summary>
    private CacheHelper()
    {
        this.s_CacheDirectory = Path.Combine(System.Environment.CurrentDirectory, "CacheDependency");
        this.m_MemoryCache = new MemoryCache(new MemoryCacheOptions());
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
            string mFileName = cacheName + ".txt";
            // Create the file if it does not exist
            if (prepDirecotry())
            {
                if (prepFile(cacheName))
                {
                    // Create a PhysicalFileProvider
                    PhysicalFileProvider mPhysicalFileProvider = new PhysicalFileProvider(s_CacheDirectory);
                    // Create the change token using the PhysicalFileProvider
                    IChangeToken mChangeToken = mPhysicalFileProvider.Watch(mFileName);
                    // Register the change callback to remove the item from the cache
                    mChangeToken.RegisterChangeCallback(changeCallback, cacheName);
                    // Create entry options with the change token and add the value to the cache
                    MemoryCacheEntryOptions mMemoryCacheEntryOptions = new MemoryCacheEntryOptions().AddExpirationToken(mChangeToken);
                    // Add the value to the cache
                    lock (m_CacheLock)
                    {
                        m_MemoryCache.Set(cacheName, value, mMemoryCacheEntryOptions);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Converts state to a string and then removes the item from m_MemoryCache.
    /// </summary>
    /// <param name="state"></param>
    private void changeCallback(object state)
    {
        if (state != default)
        {
            string mCacheName = (string)state;
            lock (m_CacheLock) 
            {
                m_MemoryCache.Remove(mCacheName);
            }
        }
    }

    public T GetFromCache<T>(string cacheName)
    {
        lock (m_CacheLock) 
        {
            m_MemoryCache.TryGetValue(cacheName, out T mRetVal);
            return mRetVal;
        }
        
    }

    /// <summary>
    /// Removes all cache by deleting all the files in the cache directory thus triggering the changeCallback.
    /// </summary>
    public void RemoveAll()
    {
        DirectoryInfo mDirectoryInfo = new DirectoryInfo(this.s_CacheDirectory);
        lock (m_CacheLock) 
        {
            foreach (FileInfo mFileInfo in mDirectoryInfo.GetFiles())
            {
                mFileInfo.Delete();
            }
        }
        lock (m_CacheLock) 
        {
            m_MemoryCache.Dispose();
            this.m_MemoryCache = new MemoryCache(new MemoryCacheOptions());
        }
    }

    /// <summary>
    /// Deletes the file it exists, causing the changeCallback method to be called.
    /// </summary>
    /// <param name="cacheName"></param>
    public void RemoveFromCache(string cacheName)
    {
        string mFileNameAndPath = Path.Combine(s_CacheDirectory, cacheName + ".txt");
        if (File.Exists(mFileNameAndPath))
        {
            File.Delete(mFileNameAndPath);
        }
    }

    /// <summary>
    /// Prepares the cache directory.
    /// </summary>
    /// <returns>False if unable to create cache directory</returns>
    private bool prepDirecotry()
    {
        bool mRetVal = true;
        try
        {
            if (!Directory.Exists(s_CacheDirectory))
            {
                Directory.CreateDirectory(s_CacheDirectory);
            }
        }
        catch (System.Exception ex)
        {
            Logger mLogger = Logger.Instance();
            mLogger.Error("Unable to create cache directory");
            mLogger.Error(ex.Message);
            mRetVal = false;
        }
        return mRetVal;
    }

    /// <summary>
    /// Prepares the cache file.
    /// </summary>
    /// <param name="cacheName"></param>
    /// <returns>False if unable to create cache file</returns>
    private bool prepFile(string cacheName)
    {
        bool mRetVal = true;
        try
        {
            string mFileNameAndPath = Path.Combine(s_CacheDirectory, cacheName + ".txt");
            using (var mFile = File.Open(mFileNameAndPath, FileMode.OpenOrCreate))
            {
                // mFile.Seek(0, SeekOrigin.End);
                using (var stream = new StreamWriter(mFile))
                {
                    stream.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }     
        }
        catch (System.Exception ex)
        {
            Logger mLogger = Logger.Instance();
            mLogger.Error("Unable to create cache file");
            mLogger.Error(ex.Message);
            mRetVal = false;
        }
        return mRetVal;
    }
}