namespace GrowthWare.Framework.Enumerations;

public enum CacheItemRemovedReason
{
    //
    // Summary:
    //     The item is removed from the cache by a System.Web.Caching.Cache.Remove(System.String)
    //     method call or by an System.Web.Caching.Cache.Insert(System.String,System.Object)
    //     method call that specified the same key.
    Removed = 1,
    //
    // Summary:
    //     The item is removed from the cache because it expired.
    Expired,
    //
    // Summary:
    //     The item is removed from the cache because the system removed it to free memory.
    Underused,
    //
    // Summary:
    //     The item is removed from the cache because the cache dependency associated with
    //     it changed.
    DependencyChanged
}