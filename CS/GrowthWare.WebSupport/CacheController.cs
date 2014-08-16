using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace GrowthWare.WebSupport
{
    /// <summary>
    /// Facade for System.Web.Caching
    /// </summary>
    public class CacheController
    {
        /// <summary>
        /// Returns the CacheDirectory path
        /// </summary>
        private static string s_CacheDirectory = HttpContext.Current.Server.MapPath("~\\") + "CacheDependency\\";

        /// <summary>
        ///	AddToCacheDependency function Adds an object to the
        /// cache as well as creates/re-writes the cacheDependency file
        /// based on the appropriate application variable
        /// For each cache dependency file a corresponding application variable
        /// is created, this is done to better track when a file needs
        /// to be changed.
        /// 
        /// The core of the cachecontroler relys on the application
        /// running within a web farm and gives the ability
        /// to keep cached objects syncronized between the servers.
        /// When there is a change to one of the cache dependency objects
        /// the cache object is re-created in the servers memory.
        /// Should part of the application remove a cache object from memory
        /// the corresponding file is altered, file replication occures
        /// and the others servers will then update their in memory cache
        /// objects the next time the cache objected is requested.
        /// </summary>
        /// <param name="key">
        ///		String representation of the cached object as
        /// the corresponding cache file name "myKey.txt".
        /// </param>
        /// <param name="value">
        ///		Object being placed into cache.
        /// </param>
        /// <returns>
        ///		Boolean
        /// </returns>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// 	[ReganM1]	12/15/2006	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static bool AddToCacheDependency(string key, object value)
        {
            bool retVal = false;
            if (!ConfigSettings.CentralManagement & ConfigSettings.EnableCache)
            {
                FileStream fileStream = null;
                StreamWriter writer = null;
                string fileName = null;
                fileName = s_CacheDirectory + key + ".txt";
                // ensure the file exists if not then create one
                if (!File.Exists(fileName))
                {
                    try
                    {
                        File.Create(fileName).Close();
                    }
                    catch
                    {
                        MDirectoryProfile DirectoryProfile = new MDirectoryProfile();
                        FileUtility.CreateDirectory(HttpContext.Current.Server.MapPath("~\\"), "CacheDependency", DirectoryProfile);
                        File.Create(fileName).Close();
                    }
                    HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application[key + "WriteCache"] = true;
                    HttpContext.Current.Application.UnLock();
                }
                // re-write the dependancy file based on the application variable
                // file replication will cause the other servers to remove their cache item
                if (HttpContext.Current.Application[key + "WriteCache"] == null) { HttpContext.Current.Application[key + "WriteCache"] = "true"; }
                if (Convert.ToBoolean(HttpContext.Current.Application[key + "WriteCache"].ToString(), CultureInfo.InvariantCulture))
                {
                    fileStream = new FileStream(fileName, FileMode.Truncate);
                    writer = new StreamWriter(fileStream);
                    writer.WriteLine(DateTime.Now.TimeOfDay);
                    writer.Close();
                    HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application[key + "WriteCache"] = false;
                    HttpContext.Current.Application.UnLock();
                }
                // cache it for future use
                CacheItemRemovedCallback onCacheRemove = null;
                onCacheRemove = new CacheItemRemovedCallback(CheckCallback);
                if ((value != null)) HttpContext.Current.Cache.Add(key, value, new CacheDependency(fileName), System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheRemove);
                // used in the orginal vb code and no eq. for the Err object exists in c#
                // assume that if no exception has happened the set retVal=true
                //if (Err().Number == 0) retVal = true;
                retVal = true;
            }
            else
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Remove a cache item from the servers memory.
        /// </summary>
        /// <param name="cacheName"></param>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// 	[ReganM1]	12/15/2006	Created
        /// </history>
        public static void RemoveFromCache(string cacheName)
        {
            string fileName = null;
            fileName = s_CacheDirectory + cacheName + ".txt";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            HttpContext.Current.Cache.Remove(cacheName);
        }

        /// <summary>
        /// Catches when a cache item has been removed from the servers memory then, 
        /// sets the corresponding application "WriteCache" variable to true, 
        /// forcing the server to write to the associated cache file upon the next request 
        /// for the cached item.
        /// </summary>
        /// <param name="key">Name of the cached item.</param>
        /// <param name="value">Generally a object being placed into cache.</param>
        /// <param name="reason">The reason the item was removed from cache.</param>
        /// <remarks></remarks>
        public static void CheckCallback(string key, object value, CacheItemRemovedReason reason)
        {
            //rebuild cache and file to sync cached objects if necessary
            if ((key != null))
            {
                if (reason == CacheItemRemovedReason.Removed)
                {
                    Logger log = Logger.Instance();
                    try
                    {
                        if (HttpContext.Current.Application != null)
                        {
                            HttpContext.Current.Application[key + "WriteCache"] = true;
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        log.Info("CheckCallback() :: NullReferenceException was encountered." + Environment.NewLine + ex.Message.ToString());
                    }
                    catch (Exception ex)
                    {
                        log.Error("CheckCallback() :: Unexpected was encountered." + Environment.NewLine + ex.Message.ToString());
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Removes all cache items from member by removing all files from the cache dependency directory.
        /// </summary>
        public static void RemoveAllCache()
        {
            DirectoryInfo DirectoryFiles = new DirectoryInfo(s_CacheDirectory);
            foreach (FileInfo directoryFile in DirectoryFiles.GetFiles("*.*"))
            {
                File.Delete(DirectoryFiles.FullName + directoryFile.Name);
            }
        }

    }
}
