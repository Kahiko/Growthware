using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GrowthWare.Web.Support;
/// <summary>
/// Facade for Microsoft.Extensions.Caching.Memory
/// </summary>
[CLSCompliant(false)]
public class CacheController
{

    // TODO: Cache has not been implemented we are just using session atm and should be doing something with
    // "Cache in-memory in ASP.NET Core" (https://learn.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-3.1) perhaps 

    private static CacheController m_CacheController;
    private static readonly Mutex m_Mutex = new Mutex();
    private readonly IMemoryCache m_MemoryCache;
    private string s_CacheDirectory = string.Empty;

    private CacheController()
    {
        this.s_CacheDirectory = Path.Combine(System.Environment.CurrentDirectory, "CacheDependency");
        this.m_MemoryCache = new MemoryCache(new MemoryCacheOptions());
    }

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

    public void AddToCacheDependency<T>(string cacheName, object value)
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
        // var mMemoryCacheEntryOptions = new MemoryCacheEntryOptions()
        //     .AddExpirationToken(mChangeToken)
        //     .RegisterPostEvictionCallback((key, value, reason, state) => {
        //         Console.WriteLine($"Cache item '{key}' was evicted due to {reason}.");
        //         if (reason == EvictionReason.Expired || reason == EvictionReason.TokenExpired)
        //         {
        //             // Remove the item from the cache
        //             RemoveFromCache(cacheName);
        //         }
        //     });
        var mMemoryCacheEntryOptions = new MemoryCacheEntryOptions().AddExpirationToken(mChangeToken);
        m_MemoryCache.Set(cacheName, value, mMemoryCacheEntryOptions);
    }

    public void ChangeCallback(object state)
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

    public void RemoveAllCache()
    {
        DirectoryInfo mDirectoryInfo = new DirectoryInfo(this.s_CacheDirectory);
        foreach (FileInfo mFileInfo in mDirectoryInfo.GetFiles())
        {
            mFileInfo.Delete(); 
        }        
    }

    public void RemoveFromCache(string cacheName)
    {
        // this.m_MemoryCache.Remove(cacheName);
        string mFileNameAndPath = Path.Combine(s_CacheDirectory, cacheName + ".txt");
        File.Delete(mFileNameAndPath);
    }
}