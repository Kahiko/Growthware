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
    public static class FunctionTypeUtility
    {
        private static Logger m_LogUtility = Logger.Instance();
        /// <summary>
        /// Read only property for cache ame for the collection
        /// </summary>
        public static readonly string FunctionTypeCachedCollectionName = "FunctionTypeCollection";

        /// <summary>
        /// Read only property for cache name for the DataView
        /// </summary>
        public static readonly string FunctionTypeCachedDVFunctions = "dvTypeFunctions";

        /// <summary>
        /// Gets the name of the function type by.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <returns>MFunctionTypeProfile.</returns>
        public static MFunctionTypeProfile GetProfile(string Name)
        {
            var mResult = from mProfile in GetFunctionTypeCollection()
                          where mProfile.Name.ToLower(CultureInfo.CurrentCulture) == Name.ToLower(CultureInfo.CurrentCulture)
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
            var mResult = from mProfile in GetFunctionTypeCollection()
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
        public static Collection<MFunctionTypeProfile> GetFunctionTypeCollection()
        {
            Collection<MFunctionTypeProfile> mFunctionTypeCollection = (Collection<MFunctionTypeProfile>)HttpContext.Current.Cache[FunctionTypeCachedCollectionName];
            if (mFunctionTypeCollection == null)
            {
                mFunctionTypeCollection = new Collection<MFunctionTypeProfile>();
                foreach (DataRow mDataRow in GetFunctionTypes().Rows)
                {
                    if ((mDataRow["Function_Type_Seq_ID"] != null))
                    {
                        // Add function types to the collection
                        MFunctionTypeProfile mProfile = new MFunctionTypeProfile(mDataRow);
                        mFunctionTypeCollection.Add(mProfile);
                    }
                }

                try
                {
                    CacheController.AddToCacheDependency(FunctionTypeCachedCollectionName, mFunctionTypeCollection);
                }
                catch
                {
                    ApplicationException myAppEx = new ApplicationException("Could not add to cache for Function Type '");
                    throw myAppEx;
                }
            }
            return mFunctionTypeCollection;
        }

        /// <summary>
        /// Removes the cached function types.
        /// </summary>
        public static void RemoveCachedFunctionTypes()
        {
            CacheController.RemoveFromCache(FunctionTypeCachedCollectionName);
            CacheController.RemoveFromCache(FunctionTypeCachedDVFunctions);
        }

        /// <summary>
        /// Res the build function collection.
        /// </summary>
        public static void ReBuildFunctionCollection()
        {
            RemoveCachedFunctionTypes();
            MFunctionTypeProfile myFunctionProfileInfo = new MFunctionTypeProfile();
            myFunctionProfileInfo = GetProfile(1);
        }

        /// <summary>
        /// Gets the function types.
        /// </summary>
        /// <returns>DataTable.</returns>
        public static DataTable GetFunctionTypes()
        {
            BFunctions functionTypes = new BFunctions(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            return functionTypes.FunctionTypes();
        }
    }
}
