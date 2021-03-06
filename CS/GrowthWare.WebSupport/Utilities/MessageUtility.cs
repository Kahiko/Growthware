﻿using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// Class MessageUtility
    /// </summary>
    public static class MessageUtility
    {
        private static string s_MessagesUnitCachedDVName = "dvMessages";

        private static string s_MessagesUnitCachedCollectionName = "MessagesCollection";

        /// <summary>
        /// Messages the name of the unit cached collection.
        /// </summary>
        /// <param name="securityEntityId">The security entity ID.</param>
        /// <returns>System.String.</returns>
        public static string MessagesUnitCachedCollectionName(int securityEntityId)
        {
            return securityEntityId.ToString(CultureInfo.InvariantCulture) + s_MessagesUnitCachedCollectionName;
        }

        /// <summary>
        /// Messages the name of the unit cached DV.
        /// </summary>
        /// <param name="securityEntityId">The security entity ID.</param>
        /// <returns>System.String.</returns>
        public static string MessagesUnitCachedDVName(int securityEntityId)
        {
            return securityEntityId.ToString(CultureInfo.InvariantCulture) + s_MessagesUnitCachedDVName;
        }

        //private static Collection<MMessageProfile> AllMessages()
        //{
        //    DataTable mDataTable = new DataTable();
        //    Collection<MMessageProfile> mMessagesCollection = null;
        //    BMessages mBMessages = new BMessages(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        //    DataSet myDataSet = new DataSet();
        //    try
        //    {
        //        int mySecurityEntity = ClientChoicesUtility.SelectedSecurityEntity();
        //        mMessagesCollection = mBMessages.GetMessages(mySecurityEntity);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if ((myDataSet != null))
        //        {
        //            myDataSet.Dispose();
        //            myDataSet = null;
        //        }
        //        if ((mDataTable != null))
        //        {
        //            mDataTable.Dispose();
        //            mDataTable = null;
        //        }
        //        if ((mBMessages != null))
        //        {
        //            mBMessages = null;
        //        }
        //    }
        //    return mMessagesCollection;
        //}

        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public static void Save(MMessageProfile profile)
        {
            BMessages mBMessages = new BMessages(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            mBMessages.Save(profile);
            RemoveCachedMessagesCollection();
        }

        /// <summary>
        /// Gets the name of the message profile by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>MMessageProfile.</returns>
        public static MMessageProfile GetProfile(string name)
        {
            var mResult = from mProfile in Messages()
                          where mProfile.Name.ToLower(CultureInfo.CurrentCulture) == name.ToLower(CultureInfo.CurrentCulture)
                          select mProfile;
            MMessageProfile mRetVal = new MMessageProfile();
            try
            {
                mRetVal = mResult.First();
            }
            catch (InvalidOperationException)
            {
                String mMSG = "Count not find function: " + name + " in the database";
                Logger mLog = Logger.Instance();
                mLog.Error(mMSG);
                mRetVal = null;
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the message profile by id.
        /// </summary>
        /// <param name="messageSeqId">The Message Sequence ID.</param>
        /// <returns>MMessageProfile.</returns>
        public static MMessageProfile GetProfile(int messageSeqId)
        {
            var mResult = from mProfile in Messages()
                          where mProfile.Id == messageSeqId
                          select mProfile;
            MMessageProfile mRetVal = null;
            try
            {
                mRetVal = mResult.First();
            }
            catch (InvalidOperationException ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(ex);
                mRetVal = null;
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <returns>Collection{MMessageProfile}.</returns>
        public static Collection<MMessageProfile> Messages()
        {
            MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
            string mCacheName = MessagesUnitCachedCollectionName(mSecurityEntityProfile.Id);
            Collection<MMessageProfile> mMessageCollection = null;
            mMessageCollection = (Collection<MMessageProfile>)HttpContext.Current.Cache[mCacheName];
            if (mMessageCollection == null)
            {
                BMessages mBMessages = new BMessages(mSecurityEntityProfile, ConfigSettings.CentralManagement);
                mMessageCollection = mBMessages.GetMessages(mSecurityEntityProfile.Id);
                CacheController.AddToCacheDependency(mCacheName, mMessageCollection);
            }
            return mMessageCollection;
        }

        /// <summary>
        /// Removes the cached messages DV.
        /// </summary>
        public static void RemoveCachedMessagesDV()
        {
            int mySecurityEntity = ClientChoicesUtility.SelectedSecurityEntity();
            CacheController.RemoveFromCache(MessagesUnitCachedDVName(mySecurityEntity));
        }

        /// <summary>
        /// Removes the cached messages collection.
        /// </summary>
        public static void RemoveCachedMessagesCollection()
        {
            int mySecurityEntity = ClientChoicesUtility.SelectedSecurityEntity();
            CacheController.RemoveFromCache(MessagesUnitCachedCollectionName(mySecurityEntity));
            RemoveCachedMessagesDV();
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>DataTable.</returns>
        public static DataTable Search(MSearchCriteria searchCriteria)
        {
            try
            {
                BMessages mBMessages = new BMessages(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
                return mBMessages.Search(searchCriteria);
            }
            catch (IndexOutOfRangeException ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Debug(ex);
                return null;
            }
        }
    }
}
