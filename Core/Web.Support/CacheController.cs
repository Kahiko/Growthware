using GrowthWare.Framework;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Threading;

namespace GrowthWare.Web.Support;

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
public class CacheController
{
    private static CacheController m_CacheController;
    private string s_CacheDirectory = string.Empty;
    private static readonly Mutex m_Mutex = new Mutex();
    private readonly IMemoryCache m_MemoryCache;

    private CacheController()
    {
        this.s_CacheDirectory = Path.Combine(System.Environment.CurrentDirectory, "CacheDependency");
        this.m_MemoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    /// <summary>
    /// Returns the instance of the CacheController class.
    /// </summary>
    /// <returns></returns>
    public static CacheController Instance()
    {
        try
        {
            m_Mutex.WaitOne();
            if (m_CacheController == null)
            {
                m_CacheController = new CacheController();
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
        return m_CacheController;
    }

    /// <summary>
    /// Adds a value to the cache be sure to check for the existence of the cache item before adding
    /// </summary>
    /// <param name="cacheName"></param>
    /// <param name="value"></param>
    /// <code language="c#">
    ///     String mCacheName = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture) + "_Functions";
    ///     Collection<MFunctionProfile> mRetVal = m_CacheController.GetFromCache<Collection<YourObjectType>>(mCacheName);;
    ///     if (mRetVal == null)
    ///     {
    ///         mRetVal = <get your data from the database>;
    ///         m_CacheController.AddToCache(mCacheName, mRetVal);
    ///     }
    ///     return mRetVal;
    /// </code>
    public void AddToCache(string cacheName, object value)
    {
        if (!ConfigSettings.CentralManagement & ConfigSettings.EnableCache)
        {
            string mFileName = cacheName + ".txt";
            string mFileNameAndPath = Path.Combine(s_CacheDirectory, cacheName + ".txt");
            // Create the file if it does not exist
            if (!File.Exists(mFileNameAndPath))
            {
                File.Create(mFileNameAndPath).Close();
            }
            // Get the file provider and create the change token
            PhysicalFileProvider mPhysicalFileProvider = new PhysicalFileProvider(s_CacheDirectory);
            IChangeToken mChangeToken = mPhysicalFileProvider.Watch(mFileName);
            // Register the change callback to remove the item from the cache
            mChangeToken.RegisterChangeCallback(ChangeCallback, cacheName);
            // Create entry options with the change token and add the value to the cache
            MemoryCacheEntryOptions mMemoryCacheEntryOptions = new MemoryCacheEntryOptions().AddExpirationToken(mChangeToken);
            // Add the value to the cache
            m_MemoryCache.Set(cacheName, value, mMemoryCacheEntryOptions);
        }
    }

    /// <summary>
    /// Handles the change callback created in AddToCache
    /// </summary>
    /// <param name="state"></param>
    private void ChangeCallback(object state)
    {
        if (state != default)
        {
            string mCacheName = (string)state;
            string mFileNameAndPath = Path.Combine(s_CacheDirectory, mCacheName + ".txt");
            if(File.Exists(mFileNameAndPath))
            {
                File.Delete(mFileNameAndPath);
            }
        }
    }

    public T GetFromCache<T>(string cacheName)
    {
        m_MemoryCache.TryGetValue(cacheName, out T mRetVal);
        return mRetVal;
    }

    /// <summary>
    /// Removes all cache by deleting all the files.
    /// </summary>
    public void RemoveAllCache()
    {
        DirectoryInfo mDirectoryInfo = new DirectoryInfo(this.s_CacheDirectory);
        foreach (FileInfo mFileInfo in mDirectoryInfo.GetFiles())
        {
            mFileInfo.Delete(); 
        }
        m_MemoryCache.Dispose();
    }

    /// <summary>
    /// Removes a single item from cache by deleting the associated file.
    /// </summary>
    /// <param name="cacheName"></param>
    public void RemoveFromCache(string cacheName)
    {
        string mFileNameAndPath = Path.Combine(s_CacheDirectory, cacheName + ".txt");
        if(File.Exists(mFileNameAndPath))
        {
            File.Delete(mFileNameAndPath);
        }
        else
        {
            m_MemoryCache.Remove(cacheName);
        }
    }
}