using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using System.Diagnostics;
using GWHelpers = GrowthWare.Web.Support.Helpers;

namespace GrowthWare.Web.Support.Helpers.Tests;

[TestFixture]
public class CacheHelperTests
{
    private GWHelpers.CacheHelper m_CacheHelper;
    private string s_CacheDirectory = string.Empty;

    [SetUp]
    public void SetUp()
    {
        // Access the singleton instance of CacheHelper
        m_CacheHelper = GWHelpers.CacheHelper.Instance();
        s_CacheDirectory = Path.Combine(System.Environment.CurrentDirectory, "CacheDependency");
    }

    private void addToCache(string mCacheName, object value)
    {
        var mCacheItem = m_CacheHelper.GetFromCache<object>(mCacheName);
        if (mCacheItem == null)
        {
            m_CacheHelper.AddToCache(mCacheName, value);
        }
    }

    [Test, Order(1)]
    public void AddToCache_AddsValue_WhenCalled()
    {
        // Arrange
        string mCacheName = "testCache";
        object mValue = new { Data = "Test" };

        // Act
        addToCache(mCacheName, mValue);

        // Assert
        var mResult = m_CacheHelper.GetFromCache<object>(mCacheName);
        Assert.That(mResult, Is.EqualTo(mValue));
    }

    [Test, Order(2)]
    public void GetFromCache_ReturnsValue_WhenExists()
    {
        // Arrange
        string mCacheName = "testCache";
        object mExpectedValue = new { Data = "Test" };

        // Add the expected value to the cache if needed
        addToCache(mCacheName, mExpectedValue);

        // Act
        var mResult = m_CacheHelper.GetFromCache<object>(mCacheName);

        // Assert
        Assert.That(mResult, Is.EqualTo(mExpectedValue));
    }

    [Test, Order(3)]
    public void RemoveFromCache_RemovesCacheEntry_WhenCalled()
    {
        // Arrange
        string mCacheName = "testCache";
        object expectedValue = new { Data = "Test" };
        // Add the expected value to the cache if needed
        addToCache(mCacheName, expectedValue);

        // Act
        m_CacheHelper.RemoveFromCache(mCacheName);
        // Wait for a brief moment to ensure the callback has time to execute
        System.Threading.Thread.Sleep(100);        
        var mResult = m_CacheHelper.GetFromCache<object>(mCacheName);

        // Assert
        Assert.That(mResult, Is.Null, "Cache entry was not removed.");
    }

    [Test, Order(4)]
    public void RemoveAll_RemovesAllCacheEntries_WhenCalled()
    {
        // Arrange
        addToCache("testCache1", new { Data = "Test1" });
        addToCache("testCache2", new { Data = "Test2" });

        // Act
        m_CacheHelper.RemoveAll();

        // Assert
        Assert.That(m_CacheHelper.GetFromCache<object>("testCache1"), Is.Null, "Cache entry testCache1 was not removed.");
        Assert.That(m_CacheHelper.GetFromCache<object>("testCache2"), Is.Null, "Cache entry testCache2 was not removed.");
    }

    [Test, Order(4)]
    public void ChangeCallback_RemovesCacheEntry_WhenFileChanges()
    {
        // Arrange
        string mCacheName = "testCache";
        // Add the expected value to the cache if needed
        addToCache(mCacheName, new { Data = "Test" });
        object mExpectedValue = m_CacheHelper.GetFromCache<object>(mCacheName);
        Assert.That(mExpectedValue, Is.Not.Null, "Cache entry was not Added as expected.");

        // Act
        // Trigger the changeCallback in CacheHelper by deleting the corresponding file
        string filePath = Path.Combine(s_CacheDirectory, mCacheName + ".txt");
        File.Delete(filePath);

        // Wait for a brief moment to ensure the callback has time to execute
        System.Threading.Thread.Sleep(100);

        // Assert
        mExpectedValue = m_CacheHelper.GetFromCache<object>(mCacheName);
        Assert.That(mExpectedValue, Is.Null, "Cache entry was not removed as expected.");
    }
}