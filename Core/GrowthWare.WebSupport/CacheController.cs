using Microsoft.AspNetCore.Http;
using System;

namespace GrowthWare.WebSupport;

/// <summary>
/// Facade for System.Web.Caching
/// </summary>
[CLSCompliant(false)]
public class CacheController
{
    static HttpContext m_HttpContext = null;

    public CacheController(HttpContext httpContext)
    {
        m_HttpContext = httpContext ?? throw new System.ArgumentNullException(nameof(httpContext));
    }

    public static bool AddToCacheDependency(string key, object value)
    {
        bool mRetVal = false;

        return mRetVal;
    }

    // public static void CheckCallback(string key, object value, CacheItemRemovedReason reason)
    // {
    //     m_HttpContext.Cache.Add();
    // }
}