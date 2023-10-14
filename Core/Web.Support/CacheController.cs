using Microsoft.AspNetCore.Http;
using System;
using GrowthWare.Framework.Enumerations;
using System.Text.Json;

namespace GrowthWare.Web.Support;
/// <summary>
/// Facade for System.Web.Caching
/// </summary>
[CLSCompliant(false)]
public class CacheController
{
    // TODO: Cache has not been implemented we are just using session atm and should be doing something with
    // "Cache in-memory in ASP.NET Core" (https://learn.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-3.1) perhaps 
    static HttpContext m_HttpContext = null;

    public CacheController(HttpContext httpContext)
    {
        m_HttpContext = httpContext ?? throw new System.ArgumentNullException(nameof(httpContext));
    }

    public static bool AddToCacheDependency(string key, object value)
    {
        bool mRetVal = true;
        string mJsonString = JsonSerializer.Serialize(value);
        m_HttpContext.Session.SetString(key, mJsonString);
        return mRetVal;
    }

    public static void CheckCallback(string key, object value, CacheItemRemovedReason reason)
    {

    }

    public static T GetFromCache<T>(string key)
    {
        string mJsonString = m_HttpContext.Session.GetString(key);
        var mRetVal = JsonSerializer.Deserialize<T>(mJsonString);
        return mRetVal;
    }

    public static void RemoveAllCache()
    {

    }
    public static void RemoveFromCache(string cacheName)
    {

    }
}