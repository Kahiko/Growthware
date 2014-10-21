using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
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
        public const string CACHED_NVP_DETAILS_TABLE_NAME = "CachedNVPDetailsTable";

        /// <summary>
        /// Name for the cache table
        /// </summary>
        public const string CACHED_NVP_TABLE_NAME = "CachedNVPTable";

        /// <summary>
        /// Deletes the detail.
        /// </summary>
        /// <param name="Profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static void DeleteDetail(MNameValuePairDetail Profile)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs mBNameValuePairs = new BNameValuePairs(mSecurityProfile);
            mBNameValuePairs.DeleteNVPDetail(Profile);
            CacheController.RemoveFromCache(CACHED_NVP_DETAILS_TABLE_NAME);
        }

        /// <summary>
        /// GetNVPs will return all Name Value Pairs avalible for a given account
        /// </summary>
        /// <param name="AccountID">The account the list of NVPs are for</param>
        /// <returns>Returns a data table of name value pairs for a given account</returns>
        /// <remarks></remarks>
        public static DataTable GetNVPs(int AccountID)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            return myNameValuePair.GetAllNVP(AccountID);
        }

        /// <summary>
        /// Gets the NVPID.
        /// </summary>
        /// <param name="StaticName">Name of the static.</param>
        /// <returns>System.Int32.</returns>
        public static int GetNVPID(string StaticName)
        {
            DataView mDV = new DataView();
            DataTable mDT = new DataTable();
            int mRetValue = 0;
            int mDefault = -1;
            mDT = GetNVPs(mDefault);
            mDV = mDT.DefaultView;
            mDV.RowFilter = "TABLE_NAME = '" + StaticName + "'";
            DataRowView mDataViewRow = mDV[0];
            mRetValue = int.Parse(mDataViewRow["NVP_SEQ_ID"].ToString());
            return mRetValue;
        }

        /// <summary>
        /// Gets the name of the NVP.
        /// </summary>
        /// <param name="NVPSeqID">The NVP seq ID.</param>
        /// <returns>System.String.</returns>
        public static string GetNVPName(int NVPSeqID)
        {
            DataView mDV = new DataView();
            DataTable mDT = new DataTable();
            string mRetValue = string.Empty;
            int mDefault = -1;
            mDT = GetNVPs(mDefault);
            mDV = mDT.DefaultView;
            mDV.RowFilter = "NVP_SEQ_ID = " + NVPSeqID;
            DataRowView mDataViewRow = mDV[0];
            mRetValue = mDataViewRow["NVP_SEQ_ID"].ToString();
            return mRetValue;
        }

        /// <summary>
        /// GetNVPs will return all Name Value Pairs reguardless of security
        /// </summary>
        /// <param name="YourDataTable">An instance of a data table you would like populated</param>
        /// <remarks></remarks>
        public static void GetNVPs(DataTable YourDataTable)
        {
            YourDataTable = (DataTable)HttpContext.Current.Cache[CACHED_NVP_TABLE_NAME];
            if (YourDataTable == null)
            {
                MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
                BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
                YourDataTable = myNameValuePair.GetAllNVP();
                CacheController.AddToCacheDependency(CACHED_NVP_TABLE_NAME, YourDataTable);
            }
        }

        /// <summary>
        /// Gets the NVP.
        /// </summary>
        /// <param name="NVPSeqID">The NVP seq ID.</param>
        /// <returns>MNameValuePair.</returns>
        public static MNameValuePair GetNVP(int NVPSeqID)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            return new MNameValuePair(myNameValuePair.GetNVP(NVPSeqID));
        }

        /// <summary>
        /// Gets the NVP detail.
        /// </summary>
        /// <param name="NVPSeqDetID">The NVP seq det ID.</param>
        /// <param name="NVPSeqID">The NVP seq ID.</param>
        /// <returns>MNameValuePairDetail.</returns>
        public static MNameValuePairDetail GetNVPDetail(int NVPSeqDetID, int NVPSeqID)
        {
            DataView mDV = new DataView();
            DataTable mDT = new DataTable();
            DataTable mImportTable = new DataTable();
            GetNVPDetails(mDT, NVPSeqID);
            mDV = mDT.DefaultView;
            mImportTable = mDT.Clone();
            mDV.RowFilter = "NVP_SEQ_DET_ID = " + NVPSeqDetID;
            foreach (DataRowView drv in mDV)
            {
                mImportTable.ImportRow(drv.Row);
            }
            return new MNameValuePairDetail(mImportTable.Rows[0]);
        }

        /// <summary>
        /// Gets the NVP details.
        /// </summary>
        /// <param name="YourDataTable">Your data table.</param>
        public static void GetNVPDetails(DataTable YourDataTable)
        {
            YourDataTable = (DataTable)HttpContext.Current.Cache[CACHED_NVP_DETAILS_TABLE_NAME];
            if (YourDataTable == null)
            {
                MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
                BNameValuePairs myNameValuePairDetails = new BNameValuePairs(mSecurityProfile);
                YourDataTable = myNameValuePairDetails.GetAllNVPDetail();
                CacheController.AddToCacheDependency(CACHED_NVP_DETAILS_TABLE_NAME, YourDataTable);
            }
        }

        /// <summary>
        /// Gets the NVP details.
        /// </summary>
        /// <param name="yourDataTable">Your data table.</param>
        /// <param name="NVPSeqID">The NVP seq ID.</param>
        public static void GetNVPDetails(DataTable yourDataTable, int NVPSeqID)
        {
            DataView mDV = new DataView();
            DataTable mDT = new DataTable();
            GetNVPDetails(mDT);
            mDV = mDT.DefaultView;
            yourDataTable = mDV.Table.Clone();
            mDV.RowFilter = "NVP_SEQ_ID = " + NVPSeqID;
            foreach (DataRowView drv in mDV)
            {
                yourDataTable.ImportRow(drv.Row);
            }
        }

        /// <summary>
        /// Gets the NVP details.
        /// </summary>
        /// <param name="staticName">Name of the static.</param>
        /// <returns>DataTable.</returns>
        public static DataTable GetNVPDetails(string staticName)
        {
            DataView mDV = new DataView();
            DataTable mDT = new DataTable();
            DataTable mReturnTable = null;
            GetNVPDetails(mDT);
            mDV = mDT.DefaultView;
            mReturnTable = mDV.Table.Clone();
            mDV.RowFilter = "TABLE_NAME = '" + staticName + "'";
            foreach (DataRowView drv in mDV)
            {
                mReturnTable.ImportRow(drv.Row);
            }
            return mReturnTable;
        }

        /// <summary>
        /// Gets the NVP details.
        /// </summary>
        /// <param name="NVPSeqID">The NVP seq ID.</param>
        /// <returns>DataTable.</returns>
        public static DataTable GetNVPDetails(int NVPSeqID)
        {
            DataView mDV = new DataView();
            DataTable mDT = new DataTable();
            DataTable mReturnTable = null;
            GetNVPDetails(mDT);
            mDV = mDT.DefaultView;
            mReturnTable = mDV.Table.Clone();
            mDV.RowFilter = "NVP_SEQ_ID = " + NVPSeqID;
            foreach (DataRowView drv in mDV)
            {
                mReturnTable.ImportRow(drv.Row);
            }
            return mReturnTable;
        }

        /// <summary>
        /// Retrieves the selected roles for the name value pair
        /// </summary>
        /// <param name="nameValuePairSeqID">The primary key of the name value pair</param>
        /// <returns>String array list of roles</returns>
        /// <remarks></remarks>
        public static string[] GetSelectedRoles(int nameValuePairSeqID)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            DataTable myDataTable = myNameValuePair.GetNVPRoles(nameValuePairSeqID);
            return GetStringArrayList(myDataTable, "Roles");
        }

        /// <summary>
        /// Retrieves the selected groups for the name value pair
        /// </summary>
        /// <param name="nameValuePairSeqID">The primary key of the name value pair</param>
        /// <returns>String array list of groups</returns>
        /// <remarks></remarks>
        public static string[] GetSelectedGroups(int nameValuePairSeqID)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            DataTable myDataTable = myNameValuePair.GetNVPGroups(nameValuePairSeqID);
            return GetStringArrayList(myDataTable, "Groups");
        }

        /// <summary>
        /// Add all the roles or groups to a string array list to bind to the List picker control.
        /// </summary>
        /// <param name="yourDT">The DataTable containing the data to bind</param>
        /// <param name="rowName">The name of the row to add to the array list</param>
        /// <returns>String array list</returns>
        /// <remarks>See the GrowthWare.CustomWebControls.ListPicker object</remarks>
        private static string[] GetStringArrayList(DataTable yourDT, string rowName)
        {
            ArrayList mRetrunArrayList = new ArrayList();
            DataRow myDR = null;
            foreach (DataRow myDR_loopVariable in yourDT.Rows)
            {
                myDR = myDR_loopVariable;
                mRetrunArrayList.Add(myDR[rowName].ToString());
            }
            return (string[])mRetrunArrayList.ToArray(typeof(string));
        }

        /// <summary>
        /// Inserts or updates a name value pair given a NameValuePair profile
        /// </summary>
        /// <param name="Profile">Details of the profile to added or updated</param>
        /// <returns>The Primary sequence ID</returns>
        /// <remarks></remarks>
        public static int Save(MNameValuePair Profile)
        {
            int mRetVal = -1;
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            CacheController.RemoveFromCache(CACHED_NVP_TABLE_NAME);
            CacheController.RemoveFromCache(CACHED_NVP_DETAILS_TABLE_NAME);
            mRetVal = myNameValuePair.Save(Profile);
            return mRetVal;
        }

        /// <summary>
        /// Saves the detail.
        /// </summary>
        /// <param name="Profile">The profile.</param>
        /// <returns>System.Int32.</returns>
        public static void SaveDetail(MNameValuePairDetail Profile)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs mBNameValuePairs = new BNameValuePairs(mSecurityProfile);
            CacheController.RemoveFromCache(CACHED_NVP_DETAILS_TABLE_NAME);
            mBNameValuePairs.SaveNVPDetail(Profile);
        }

        public static DataTable Search(MSearchCriteria searchCriteria)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs mBNameValuePairs = new BNameValuePairs(mSecurityProfile);
            return mBNameValuePairs.Search(searchCriteria);
        }

        /// <summary>
        /// SetDropSelection will set the selected index given the drop down list and the desired value
        /// </summary>
        /// <param name="theDropDown">The drop down list to set</param>
        /// <param name="SelectedValue">The value to set the drop box to</param>
        /// <remarks></remarks>
        public static void SetDropSelection(DropDownList theDropDown, string SelectedValue)
        {
            try
            {
                int X = 0;
                for (X = 0; X <= theDropDown.Items.Count - 1; X++)
                {
                    if (theDropDown.Items[X].Value == SelectedValue)
                    {
                        theDropDown.SelectedIndex = X;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the groups.
        /// </summary>
        /// <param name="NVP_ID">The NV p_ ID.</param>
        /// <param name="SecurityEntityID">The security entity ID.</param>
        /// <param name="CommaSeparatedGroups">The comma separated groups.</param>
        /// <param name="nvpProfile">MNameValuePair.</param>
        public static void UpdateGroups(int NVP_ID, int SecurityEntityID, string CommaSeparatedGroups, MNameValuePair nvpProfile)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            myNameValuePair.UpdateGroups(NVP_ID, SecurityEntityID, CommaSeparatedGroups, nvpProfile);
        }

        /// <summary>
        /// Updates the roles.
        /// </summary>
        /// <param name="NVP_ID">The NV p_ ID.</param>
        /// <param name="SecurityEntityID">The security entity ID.</param>
        /// <param name="CommaSeparatedRoles">The comma separated roles.</param>
        /// <param name="nvpProfile">MNameValuePair.</param>
        public static void UpdateRoles(int NVP_ID, int SecurityEntityID, string CommaSeparatedRoles, MNameValuePair nvpProfile)
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BNameValuePairs myNameValuePair = new BNameValuePairs(mSecurityProfile);
            myNameValuePair.UpdateRoles(NVP_ID, SecurityEntityID, CommaSeparatedRoles, nvpProfile);
        }

    }
}
