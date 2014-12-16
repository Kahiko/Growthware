using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// Class NameValuePairUtility
    /// </summary>
    public static class NameValuePairUtility
    {
        /// <summary>
        /// Constant for the NVP Cached table
        /// </summary>
        public const string CachedNameValuePairDetailsTableName = "CachedNVPDetailsTable";

        /// <summary>
        /// Name for the cache table
        /// </summary>
        public const string CachedNameValuePairTableName = "CachedNVPTable";

        /// <summary>
        /// Deletes the detail.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static void DeleteDetail(MNameValuePairDetail profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!");
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs mBNameValuePairs = new BNameValuePairs(mSecurityProfile);
            mBNameValuePairs.DeleteNameValuePairDetail(profile);
            CacheController.RemoveFromCache(CachedNameValuePairDetailsTableName);
        }

        /// <summary>
        /// GetNVPs will return all Name Value Pairs avalible for a given account
        /// </summary>
        /// <param name="accountId">The account the list of NVPs are for</param>
        /// <returns>Returns a data table of name value pairs for a given account</returns>
        /// <remarks></remarks>
        public static DataTable GetNameValuePairs(int accountId)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            return myNameValuePair.GetAllNameValuePair(accountId);
        }

        /// <summary>
        /// Gets the NVPID.
        /// </summary>
        /// <param name="staticName">Name of the static.</param>
        /// <returns>System.Int32.</returns>
        public static int GetNameValuePairId(string staticName)
        {
            DataView mDataView = null;
            DataTable mDataTable = null;
            int mRetValue = 0;
            try
            {
                int mDefault = -1;
                mDataTable = GetNameValuePairs(mDefault);
                mDataTable.Locale = CultureInfo.InvariantCulture;
                mDataView = mDataTable.DefaultView;
                mDataView.RowFilter = "TABLE_NAME = '" + staticName + "'";
                DataRowView mDataViewRow = mDataView[0];
                mRetValue = int.Parse(mDataViewRow["NVP_SEQ_ID"].ToString(), CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                if (mDataTable != null) 
                {
                    mDataTable.Dispose();
                    mDataTable = null;
                }
                if (mDataView != null) 
                {
                    mDataView.Dispose();
                    mDataView = null;
                }
            }
            return mRetValue;
        }

        /// <summary>
        /// Gets the name of the NVP.
        /// </summary>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        /// <returns>System.String.</returns>
        public static string GetNameValuePairName(int nameValuePairSeqId)
        {
            DataView mDataView = null;
            DataTable mDataTable = null;
            string mRetValue = string.Empty;
            try
            {
                int mDefault = -1;
                mDataTable = GetNameValuePairs(mDefault);
                mDataTable.Locale = CultureInfo.InvariantCulture;
                mDataView = mDataTable.DefaultView;
                mDataView.RowFilter = "NVP_SEQ_ID = " + nameValuePairSeqId;
                DataRowView mDataViewRow = mDataView[0];
                mRetValue = mDataViewRow["NVP_SEQ_ID"].ToString();

            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                if (mDataTable != null) 
                {
                    mDataTable.Dispose();
                    mDataTable = null;
                }
                if (mDataView != null)
                {
                    mDataView.Dispose();
                    mDataView = null;
                }
            }
            return mRetValue;
        }

        /// <summary>
        /// GetNVPs will return all Name Value Pairs reguardless of security
        /// </summary>
        /// <param name="yourDataTable">An instance of a data table you would like populated</param>
        /// <remarks></remarks>
        public static void GetNameValuePairs(DataTable yourDataTable)
        {
            yourDataTable = (DataTable)HttpContext.Current.Cache[CachedNameValuePairTableName];
            if (yourDataTable == null)
            {
                MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
                BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
                yourDataTable = myNameValuePair.GetAllNameValuePair();
                CacheController.AddToCacheDependency(CachedNameValuePairTableName, yourDataTable);
            }
        }

        /// <summary>
        /// Gets the NVP.
        /// </summary>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        /// <returns>MNameValuePair.</returns>
        public static MNameValuePair GetNameValuePair(int nameValuePairSeqId)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            return new MNameValuePair(myNameValuePair.GetNameValuePair(nameValuePairSeqId));
        }

        /// <summary>
        /// Gets the NVP detail.
        /// </summary>
        /// <param name="nameValuePairSeqDetailId">The NVP seq det ID.</param>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        /// <returns>MNameValuePairDetail.</returns>
        public static MNameValuePairDetail GetNameValuePairDetail(int nameValuePairSeqDetailId, int nameValuePairSeqId)
        {
            DataView mDataView = null;
            DataTable mDataTable = null;
            DataTable mImportTable = null;
            MNameValuePairDetail mRetVal = null;
            try
            {
                GetNameValuePairDetails(ref mDataTable, nameValuePairSeqId);
                mDataTable.Locale = CultureInfo.InvariantCulture;
                mDataView = mDataTable.DefaultView;
                mImportTable = mDataTable.Clone();
                mDataView.RowFilter = "NVP_SEQ_DET_ID = " + nameValuePairSeqDetailId;
                foreach (DataRowView drv in mDataView)
                {
                    mImportTable.ImportRow(drv.Row);
                }
                mRetVal = new MNameValuePairDetail(mImportTable.Rows[0]);
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                if (mDataTable != null) 
                {
                    mDataTable.Dispose();
                    mDataTable = null;
                }
                if (mDataView != null)
                {
                    mDataView.Dispose();
                    mDataView = null;
                }
                if (mImportTable != null)
                {
                    mImportTable.Dispose();
                    mImportTable = null;
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the NVP details.
        /// </summary>
        /// <param name="yourDataTable">Your data table.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public static void GetNameValuePairDetails(ref DataTable yourDataTable)
        {
            yourDataTable = (DataTable)HttpContext.Current.Cache[CachedNameValuePairDetailsTableName];
            if (yourDataTable == null)
            {
                MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
                BNameValuePairs myNameValuePairDetails = new BNameValuePairs(mSecurityProfile);
                yourDataTable = myNameValuePairDetails.GetAllNameValuePairDetail();
                CacheController.AddToCacheDependency(CachedNameValuePairDetailsTableName, yourDataTable);
            }
        }

        /// <summary>
        /// Gets the NVP details.
        /// </summary>
        /// <param name="yourDataTable">Your data table.</param>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#"), 
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public static void GetNameValuePairDetails(ref DataTable yourDataTable, int nameValuePairSeqId)
        {
            DataView mDataView = null;
            DataTable mDataTable = null;
            try
            {
                GetNameValuePairDetails(ref mDataTable);
                mDataTable.Locale = CultureInfo.InvariantCulture;
                mDataTable.DefaultView.RowFilter = String.Empty;
                mDataView = new DataView(mDataTable);
                yourDataTable = mDataView.Table.Clone();
                mDataView.RowFilter = "NVP_SEQ_ID = " + nameValuePairSeqId;
                foreach (DataRowView drv in mDataView)
                {
                    yourDataTable.ImportRow(drv.Row);
                }
            }
            catch (Exception ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(ex);
                throw;
            }
            finally 
            {
                if (mDataTable != null) 
                {
                    mDataTable.Dispose();
                    mDataTable = null;
                }
                if (mDataView != null)
                {
                    mDataView.Dispose();
                    mDataView = null;
                }
            }
        }

        /// <summary>
        /// Gets the NVP details.
        /// </summary>
        /// <param name="staticName">Name of the static.</param>
        /// <returns>DataTable.</returns>
        public static DataTable GetNameValuePairDetails(string staticName)
        {
            DataView mDataView = null;
            DataTable mDataTable = null;
            DataTable mReturnTable = null;
            try
            {
                GetNameValuePairDetails(ref mDataTable);
                mDataTable.Locale = CultureInfo.InvariantCulture;
                mDataView = mDataTable.DefaultView;
                mReturnTable = mDataView.Table.Clone();
                mDataView.RowFilter = "TABLE_NAME = '" + staticName + "'";
                foreach (DataRowView drv in mDataView)
                {
                    mReturnTable.ImportRow(drv.Row);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                if (mDataTable != null)
                {
                    mDataTable.Dispose();
                    mDataTable = null;
                }
                if (mDataView != null)
                {
                    mDataView.Dispose();
                    mDataView = null;
                }            
            }
            return mReturnTable;
        }

        /// <summary>
        /// Gets the NVP details.
        /// </summary>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        /// <returns>DataTable.</returns>
        public static DataTable GetNameValuePairDetails(int nameValuePairSeqId)
        {
            DataView mDataView = null;
            DataTable mDataTable = null;
            DataTable mReturnTable = null;
            try
            {
                GetNameValuePairDetails(ref mDataTable);
                mDataTable.Locale = CultureInfo.InvariantCulture;
                mDataView = mDataTable.DefaultView;
                mReturnTable = mDataView.Table.Clone();
                mDataView.RowFilter = "NVP_SEQ_ID = " + nameValuePairSeqId;
                foreach (DataRowView drv in mDataView)
                {
                    mReturnTable.ImportRow(drv.Row);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                if (mDataTable != null)
                {
                    mDataTable.Dispose();
                    mDataTable = null;
                }
                if (mDataView != null)
                {
                    mDataView.Dispose();
                    mDataView = null;
                }                 
            }
            return mReturnTable;
        }

        /// <summary>
        /// Retrieves the selected roles for the name value pair
        /// </summary>
        /// <param name="nameValuePairSeqId">The primary key of the name value pair</param>
        /// <returns>String array list of roles</returns>
        /// <remarks></remarks>
        public static string[] GetSelectedRoles(int nameValuePairSeqId)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            DataTable myDataTable = myNameValuePair.GetNameValuePairRoles(nameValuePairSeqId);
            return GetStringArrayList(myDataTable, "Roles");
        }

        /// <summary>
        /// Retrieves the selected groups for the name value pair
        /// </summary>
        /// <param name="nameValuePairSeqId">The primary key of the name value pair</param>
        /// <returns>String array list of groups</returns>
        /// <remarks></remarks>
        public static string[] GetSelectedGroups(int nameValuePairSeqId)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            DataTable myDataTable = myNameValuePair.GetNameValuePairGroups(nameValuePairSeqId);
            return GetStringArrayList(myDataTable, "Groups");
        }

        /// <summary>
        /// Add all the roles or groups to a string array list to bind to the List picker control.
        /// </summary>
        /// <param name="yourDataTable">The DataTable containing the data to bind</param>
        /// <param name="rowName">The name of the row to add to the array list</param>
        /// <returns>String array list</returns>
        /// <remarks>See the GrowthWare.CustomWebControls.ListPicker object</remarks>
        private static string[] GetStringArrayList(DataTable yourDataTable, string rowName)
        {
            ArrayList mRetrunArrayList = new ArrayList();
            DataRow myDR = null;
            foreach (DataRow myDR_loopVariable in yourDataTable.Rows)
            {
                myDR = myDR_loopVariable;
                mRetrunArrayList.Add(myDR[rowName].ToString());
            }
            return (string[])mRetrunArrayList.ToArray(typeof(string));
        }

        /// <summary>
        /// Inserts or updates a name value pair given a NameValuePair profile
        /// </summary>
        /// <param name="profile">Details of the profile to added or updated</param>
        /// <returns>The Primary sequence ID</returns>
        /// <remarks></remarks>
        public static int Save(MNameValuePair profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!");
            int mRetVal = -1;
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            CacheController.RemoveFromCache(CachedNameValuePairTableName);
            CacheController.RemoveFromCache(CachedNameValuePairDetailsTableName);
            mRetVal = myNameValuePair.Save(profile);
            return mRetVal;
        }

        /// <summary>
        /// Saves the detail.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>System.Int32.</returns>
        public static void SaveDetail(MNameValuePairDetail profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!");
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs mBNameValuePairs = new BNameValuePairs(mSecurityProfile);
            CacheController.RemoveFromCache(CachedNameValuePairDetailsTableName);
            mBNameValuePairs.SaveNameValuePairDetail(profile);
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public static DataTable Search(MSearchCriteria searchCriteria)
        {
            if (searchCriteria == null) throw new ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!");
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs mBNameValuePairs = new BNameValuePairs(mSecurityProfile);
            return mBNameValuePairs.Search(searchCriteria);
        }

        /// <summary>
        /// SetDropSelection will set the selected index given the drop down list and the desired value
        /// </summary>
        /// <param name="theDropDown">The drop down list to set</param>
        /// <param name="selectedValue">The value to set the drop box to</param>
        /// <remarks></remarks>
        public static void SetDropSelection(ListControl theDropDown, string selectedValue)
        {
            if (theDropDown == null) throw new ArgumentNullException("theDropDown", "theDropDown cannot be a null reference (Nothing in Visual Basic)!");
            if (string.IsNullOrEmpty(selectedValue)) throw new ArgumentNullException("selectedValue", "selectedValue cannot be a null reference (Nothing in Visual Basic)!");
            try
            {
                int X = 0;
                for (X = 0; X <= theDropDown.Items.Count - 1; X++)
                {
                    if (theDropDown.Items[X].Value == selectedValue)
                    {
                        theDropDown.SelectedIndex = X;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the groups.
        /// </summary>
        /// <param name="nameValuePairId">The NV p_ ID.</param>
        /// <param name="securityEntityId">The security entity ID.</param>
        /// <param name="commaSeparatedGroups">The comma separated groups.</param>
        /// <param name="nameValuePairProfile">MNameValuePair.</param>
        public static void UpdateGroups(int nameValuePairId, int securityEntityId, string commaSeparatedGroups, MNameValuePair nameValuePairProfile)
        {
            if (nameValuePairProfile == null) throw new ArgumentNullException("nameValuePairProfile", "nameValuePairProfile cannot be a null reference (Nothing in Visual Basic)!");
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs mNameValuePair = new BNameValuePairs(mSecurityProfile);
            mNameValuePair.UpdateGroups(nameValuePairId, securityEntityId, commaSeparatedGroups, nameValuePairProfile);
        }

        /// <summary>
        /// Updates the roles.
        /// </summary>
        /// <param name="nameValuePairId">The NV p_ ID.</param>
        /// <param name="securityEntityId">The security entity ID.</param>
        /// <param name="commaSeparatedRoles">The comma separated roles.</param>
        /// <param name="nameValuePairProfile">MNameValuePair.</param>
        public static void UpdateRoles(int nameValuePairId, int securityEntityId, string commaSeparatedRoles, MNameValuePair nameValuePairProfile)
        {
            if (nameValuePairProfile == null) throw new ArgumentNullException("nameValuePairProfile", "nameValuePairProfile cannot be a null reference (Nothing in Visual Basic)!");
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            myNameValuePair.UpdateRoles(nameValuePairId, securityEntityId, commaSeparatedRoles, nameValuePairProfile);
        }

    }
}
