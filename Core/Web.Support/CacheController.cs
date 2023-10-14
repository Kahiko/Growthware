using Microsoft.AspNetCore.Http;
using System;
using GrowthWare.Framework.Enumerations;
using System.Text.Json;

namespace GrowthWare.Web.Support;
/// <summary>
/// Facade for System.Web.Caching
/// </summary>
[CLSCompliant(false)]
public static class CacheController
{
    // TODO: Cache has not been implemented we are just using session atm and should be doing something with
    // "Cache in-memory in ASP.NET Core" (https://learn.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-3.1) perhaps 
    static IHttpContextAccessor m_HttpContextAccessor = null;


    public static bool AddToCacheDependency(string key, object value)
    {
        bool mRetVal = true;
        string mJsonString = JsonSerializer.Serialize(value);
        m_HttpContextAccessor.HttpContext.Session.SetString(key, mJsonString);
        return mRetVal;
    }

    public static void CheckCallback(string key, object value, CacheItemRemovedReason reason)
    {

    }

    public static T GetFromCache<T>(string key)
    {
        string mJsonString = m_HttpContextAccessor.HttpContext.Session.GetString(key);
        if(mJsonString != null && !String.IsNullOrEmpty(mJsonString))
        {
            return JsonSerializer.Deserialize<T>(mJsonString);

        }
        return default;
    }

    public static void RemoveAllCache()
    {

    }
    public static void RemoveFromCache(string cacheName)
    {

    }

    /// <summary>
    /// Set the HttpContextAccessor for the application.
    /// </summary>
    /// <param name="httpContextAccessor">An instance of IHttpContextAccessor to be set as the HttpContextAccessor.</param>
    [CLSCompliant(false)]
    public static void SetHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        m_HttpContextAccessor = httpContextAccessor;
    }
}