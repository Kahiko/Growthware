using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// FunctionTypeUtility serves as the focal point for any web application needing to utilize the GrowthWare framework.
    /// Web needs such as caching are handled here.
    /// </summary>
    public static class FunctionTypeUtility
    {
        /// <summary>
        /// Read only property for cache ame for the collection
        /// </summary>
        private static string s_FunctionTypeCachedCollectionName = "FunctionTypeCollection";

        ///// <summary>
        ///// Read only property for cache name for the DataView
        ///// </summary>
        //private static string s_FunctionTypeCachedDVFunctions = "dvTypeFunctions";

        /// <summary>
        /// Gets the name of the function type by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>MFunctionTypeProfile.</returns>
        public static MFunctionTypeProfile GetProfile(string name)
        {
            var mResult = from mProfile in FunctionTypeCollection()
                          where mProfile.Name.ToLower(CultureInfo.CurrentCulture) == name.ToLower(CultureInfo.CurrentCulture)
                          select mProfile;
            MFunctionTypeProfile mRetVal = null;
            try
            {
                mRetVal = mResult.First();
            }
            catch (NullReferenceException)
            {
                mRetVal = null;
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the function type by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>MFunctionTypeProfile.</returns>
        public static MFunctionTypeProfile GetProfile(int id)
        {
            var mResult = from mProfile in FunctionTypeCollection()
                          where mProfile.Id == id
                          select mProfile;
            MFunctionTypeProfile mRetVal = null;
            try
            {
                mRetVal = mResult.First();
            }
            catch (NullReferenceException)
            {
                mRetVal = null;
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the function type collection.
        /// </summary>
        /// <returns>Collection{MFunctionTypeProfile}.</returns>
        public static Collection<MFunctionTypeProfile> FunctionTypeCollection()
        {
            Collection<MFunctionTypeProfile> mFunctionTypeCollection = (Collection<MFunctionTypeProfile>)HttpContext.Current.Cache[s_FunctionTypeCachedCollectionName];
            if (mFunctionTypeCollection == null)
            {
                mFunctionTypeCollection = new Collection<MFunctionTypeProfile>();
                foreach (DataRow mDataRow in FunctionTypes().Rows)
                {
                    if ((mDataRow["Function_Type_Seq_ID"] != null))
                    {
                        // Add function types to the collection
                        MFunctionTypeProfile mProfile = new MFunctionTypeProfile(mDataRow);
                        mFunctionTypeCollection.Add(mProfile);
                    }
                }
                CacheController.AddToCacheDependency(s_FunctionTypeCachedCollectionName, mFunctionTypeCollection);
            }
            return mFunctionTypeCollection;
        }

        ///// <summary>
        ///// Removes the cached function types.
        ///// </summary>
        //public static void RemoveCachedFunctionTypes()
        //{
        //    CacheController.RemoveFromCache(s_FunctionTypeCachedCollectionName);
        //    CacheController.RemoveFromCache(s_FunctionTypeCachedDVFunctions);
        //}

        ///// <summary>
        ///// Res the build function collection.
        ///// </summary>
        //public static void RebuildFunctionCollection()
        //{
        //    RemoveCachedFunctionTypes();
        //    MFunctionTypeProfile mFunctionProfileInfo = new MFunctionTypeProfile();
        //    mFunctionProfileInfo = GetProfile(1);
        //}

        /// <summary>
        /// Gets the function types.
        /// </summary>
        /// <returns>DataTable.</returns>
        public static DataTable FunctionTypes()
        {
            BFunctions functionTypes = new BFunctions(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            return functionTypes.FunctionTypes();
        }
    }
}
