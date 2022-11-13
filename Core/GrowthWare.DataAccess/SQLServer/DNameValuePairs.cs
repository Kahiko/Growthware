using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace GrowthWare.DataAccess.SQLServer
{
    /// <summary>
    /// Class DNameValuePairs
    /// </summary>
    public class DNameValuePairs : AbstractDBInteraction, INameValuePairs
    {
        private MNameValuePair m_Profile = new MNameValuePair();
        private int m_SecurityEntitySeqId;
        private int m_AccountId;
        private int m_PermissionSeqId = 1;

        private MNameValuePairDetail m_Detail_Profile = new MNameValuePairDetail();

        MNameValuePair INameValuePairs.NameValuePairProfile
        {
            get { return m_Profile; }
            set { m_Profile = value; }
        }

        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        /// <value>The primary key.</value>
        public int PrimaryKey { get; set; }

        int INameValuePairs.SecurityEntitySeqId
        {
            get { return m_SecurityEntitySeqId; }
            set { m_SecurityEntitySeqId = value; }
        }

        int INameValuePairs.AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }

        MNameValuePairDetail INameValuePairs.DetailProfile
        {
            get { return m_Detail_Profile; }
            set { m_Detail_Profile = value; }
        }

        DataTable INameValuePairs.GetAllNVP()
        {
            String storeProc = "ZGWSystem.Get_Name_Value_Pair";
            SqlParameter[] mParameters = GetSelectParameters();
            return base.GetDataTable(storeProc, mParameters);
        }

        DataRow INameValuePairs.NameValuePair()
        {
            String storeProc = "ZGWSystem.Get_Name_Value_Pair";
            SqlParameter[] mParameters = GetSelectParameters();
            return base.GetDataRow(storeProc, mParameters);
        }

        int INameValuePairs.Save()
        {
            String storeProc = "ZGWSystem.Set_Name_Value_Pair";
            SqlParameter[] mParameters = GetInsertUpdateParameters();
            int mRetVal = -1;
            base.ExecuteNonQuery(storeProc, mParameters);
            mRetVal = int.Parse(GetParameterValue("@P_Primary_Key", mParameters), CultureInfo.InvariantCulture);
            return mRetVal;
        }

        DataTable INameValuePairs.GetRoles(int NameValuePairSeqID)
        {
            SqlParameter[] mParameters = { new SqlParameter("@P_NVPSeqId", NameValuePairSeqID), new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqId) };
            string myStoreProcedure = "ZGWSecurity.Get_Name_Value_Pair_Roles";
            return base.GetDataTable(myStoreProcedure, mParameters);
        }

        DataTable INameValuePairs.GetGroups(int NameValuePairSeqID)
        {
            SqlParameter[] mParameters = { new SqlParameter("@P_NVPSeqId", NameValuePairSeqID), new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqId) };
            string myStoreProcedure = "ZGWSecurity.Get_Name_Value_Pair_Groups";
            return base.GetDataTable(myStoreProcedure, mParameters);
        }

        void INameValuePairs.UpdateGroups(int NVP_ID, int SecurityEntityID, string CommaSeparatedGroups, MNameValuePair nvpProfile)
        {
            string myStoreProcedure = "ZGWSecurity.Set_Name_Value_Pair_Groups";
            SqlParameter[] mParameters = { new SqlParameter("@P_NVPSeqId", NVP_ID), new SqlParameter("@P_SecurityEntitySeqId", SecurityEntityID), new SqlParameter("@P_Groups", CommaSeparatedGroups), new SqlParameter("@P_PermissionsNVPDetailSeqId", m_PermissionSeqId), new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(nvpProfile)) };
            base.ExecuteNonQuery(myStoreProcedure, mParameters);
        }

        void INameValuePairs.UpdateRoles(int NVP_ID, int SecurityEntityID, string CommaSeparatedRoles, MNameValuePair nvpProfile)
        {
            string myStoreProcdure = "ZGWSecurity.Set_Name_Value_Pair_Roles";
            SqlParameter[] mParameters = { new SqlParameter("@P_NVPSeqId", NVP_ID), new SqlParameter("@P_SecurityEntitySeqId", SecurityEntityID), new SqlParameter("@P_Role", CommaSeparatedRoles), new SqlParameter("@P_PermissionsNVPDetailSeqId", m_PermissionSeqId), new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(nvpProfile)) };
            base.ExecuteNonQuery(myStoreProcdure, mParameters);
        }

        DataRow INameValuePairs.NameValuePairDetail()
        {
            string myStoreProcedure = "ZGWSystem.Get_Name_Value_Pair_Detail";
            SqlParameter[] mParameters = { 
											 new SqlParameter("@P_NVP_DetailSeqId", m_Detail_Profile.Id), 
											 new SqlParameter("@P_NVPSeqId", m_Detail_Profile.NameValuePairSeqId)
										 };
            return base.GetDataRow(myStoreProcedure, mParameters);
        }

        DataRow INameValuePairs.NameValuePairDetails(int NVPSeqDetID, int NVPSeqID)
        {
            string myStoreProcedure = "ZGWSystem.Get_Name_Value_Pair_Details";
            SqlParameter[] mParameters = { new SqlParameter("@P_NVPSeqId", NVPSeqID) };
            return base.GetDataRow(myStoreProcedure, mParameters);
        }

        bool INameValuePairs.DeleteNVPDetail(MNameValuePairDetail profile)
        {
            string myStoreProcedure = "ZGWSystem.Delete_Name_Value_Pair_Detail";
            Boolean mRetVal = false;
            SqlParameter[] mParameters = { new SqlParameter("@P_NVPSeqId", profile.NameValuePairSeqId), new SqlParameter("@P_NVP_DetailSeqId", profile.Id) };
            try
            {
                base.ExecuteNonQuery(myStoreProcedure, mParameters);
                mRetVal = true;
            }
            catch (Exception)
            {
                throw;
            }
            return mRetVal;
        }

        DataTable INameValuePairs.AllNameValuePairDetail()
        {
            string myStoreProcedure = "ZGWSystem.Get_Name_Value_Pair_Details";
            SqlParameter[] mParameters = { new SqlParameter("@P_NVPSeqId", -1) };
            return base.GetDataTable(myStoreProcedure, mParameters);
        }

        DataTable INameValuePairs.GetAllNVPDetail(int NVPSeqID)
        {
            string myStoreProcedure = "ZGWSystem.Get_Name_Value_Pair_Details";
            SqlParameter[] mParameters = { new SqlParameter("@P_NVPSeqId", NVPSeqID) };
            return base.GetDataTable(myStoreProcedure, mParameters);
        }

        void INameValuePairs.SaveNVPDetail(MNameValuePairDetail profile)
        {
            string myStoreProcedure = "ZGWSystem.Set_Name_Value_Pair_Detail";
            SqlParameter[] mParameters = { new SqlParameter("@P_NVP_DetailSeqId", profile.Id), new SqlParameter("@P_NVPSeqId", profile.NameValuePairSeqId), new SqlParameter("@P_NVP_Detail_Name", profile.Value), new SqlParameter("@P_NVP_Detail_Value", profile.Text), new SqlParameter("@P_StatusSeqId", profile.Status), new SqlParameter("@P_Sort_Order", profile.SortOrder), new SqlParameter("@P_Added_Updated_BY", GetAddedUpdatedBy(profile)), 
			GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output), GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output) };
            base.ExecuteNonQuery(myStoreProcedure, mParameters);
        }

        private SqlParameter[] GetInsertUpdateParameters()
        {
            SqlParameter[] mParameters = { 
				new SqlParameter("@P_NVPSeqId", m_Profile.Id), 
				new SqlParameter("@P_Schema_Name", m_Profile.SchemaName), 
				new SqlParameter("@P_Static_Name", m_Profile.StaticName), 
				new SqlParameter("@P_Display", m_Profile.Display), 
				new SqlParameter("@P_Description", m_Profile.Description), 
				new SqlParameter("@P_StatusSeqId", m_Profile.Status), 
				new SqlParameter("@P_Added_Updated_BY", GetAddedUpdatedBy(m_Profile)), 
				GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output), 
				GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output) 
			};
            return mParameters;
        }

        private SqlParameter[] GetSelectParameters()
        {
            SqlParameter[] mParameters = { new SqlParameter("@P_NVPSeqId", m_Profile.Id), new SqlParameter("@P_AccountSeqId", m_AccountId), new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqId) };
            return mParameters;
        }
    }
}
