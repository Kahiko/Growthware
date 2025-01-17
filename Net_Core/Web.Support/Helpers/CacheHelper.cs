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
/// in order to syncronize the cache across multiple servers.
/// A file is created for each cache entry when RemoveFromCache or RemoveAllCache is called
/// the corresponding file is deleted. A ChangeToken is associated with the file and the
/// value for the cache entry is remove.
/// Any code using this class should call GetFromCache<T>(string cacheName) and if the returned value
/// is null it's because the cache item was not found, at that point data should be retrieved from
/// the database and added to the cache using AddToCache(string cacheName)
/// </summary>
[CLSCompliant(false)]
public class CacheHelper
{
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
    /// Adds a value to the cache be sure to check for the existence of the cache item before adding
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
                    // Get the file provider and create the change token
                    PhysicalFileProvider mPhysicalFileProvider = new PhysicalFileProvider(s_CacheDirectory);
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
    /// Handles the change callback created in AddToCache
    /// </summary>
    /// <param name="state"></param>
    private void changeCallback(object state)
    {
        if (state != default)
        {
            string mCacheName = (string)state;
            string mFileNameAndPath = Path.Combine(s_CacheDirectory, mCacheName + ".txt");
            int mWaitCount = 0;
            do
            {
                try
                {
                    if (File.Exists(mFileNameAndPath))
                    {
                        File.Delete(mFileNameAndPath);
                    }
                }
                catch (System.Exception)
                {
                    mWaitCount++;
                }
            } while (mWaitCount < 4 && File.Exists(mFileNameAndPath));
            if (mWaitCount < 4) 
            {
                lock (m_CacheLock) 
                {
                    m_MemoryCache.Remove(mCacheName);
                }
            } 
            else 
            {
                throw new Exception($"Unable to remove the { mCacheName } item from cache");
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
    /// Removes all cache by deleting all the files.
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
    /// Removes a single item from cache by deleting the associated file.
    /// </summary>
    /// <param name="cacheName"></param>
    public void RemoveFromCache(string cacheName)
    {
        string mFileNameAndPath = Path.Combine(s_CacheDirectory, cacheName + ".txt");
        if (File.Exists(mFileNameAndPath))
        {
            File.Delete(mFileNameAndPath);
        }
        else
        {
            lock (m_CacheLock) 
            {
                m_MemoryCache.Remove(cacheName);
            }
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
            if (!File.Exists(mFileNameAndPath))
            {
                File.Create(mFileNameAndPath).Close();
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