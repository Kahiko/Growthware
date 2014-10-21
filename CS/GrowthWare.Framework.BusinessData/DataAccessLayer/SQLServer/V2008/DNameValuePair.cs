using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.V2008
{
    /// <summary>
    /// Class DNameValuePair
    /// </summary>
    public class DNameValuePair : DDBInteraction, IDNameValuePair
    {
        private MNameValuePair m_Profile = new MNameValuePair();
        private int m_SecurityEntitySeqId;
        private int m_AccountId;
        private int m_PermissionSeqId = 1;

        private MNameValuePairDetail m_Detail_Profile = new MNameValuePairDetail();

        MNameValuePair IDNameValuePair.NameValuePairProfile
        {
            get { return m_Profile; }
            set { m_Profile = value; }
        }

        public int PrimaryKey { get; set; }

        int IDNameValuePair.SecurityEntitySeqId
        {
            get { return m_SecurityEntitySeqId; }
            set { m_SecurityEntitySeqId = value; }
        }

        int IDNameValuePair.AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }

        MNameValuePairDetail IDNameValuePair.DetailProfile
        {
            get { return m_Detail_Profile; }
            set { m_Detail_Profile = value; }
        }

        DataTable IDNameValuePair.GetAllNVP()
        {
            String storeProc = "ZGWSystem.Get_Name_Value_Pair";
            SqlParameter[] mParameters = GetSelectParameters();
            return base.GetDataTable(storeProc, mParameters);
        }

        DataRow IDNameValuePair.GetNVP()
        {
            String storeProc = "ZGWSystem.Get_Name_Value_Pair";
            SqlParameter[] mParameters = GetSelectParameters();
            return base.GetDataRow(storeProc, mParameters);
        }

        int IDNameValuePair.Save()
        {
            String storeProc = "ZGWSystem.Set_Name_Value_Pair";
            SqlParameter[] mParameters = GetInsertUpdateParameters();
            int mRetVal = -1;
            base.ExecuteNonQuery(storeProc, mParameters);
            mRetVal = int.Parse(GetParameterValue("@P_Primary_Key", mParameters));
            return mRetVal;
        }

        DataTable IDNameValuePair.GetRoles(int NameValuePairSeqID)
        {
            SqlParameter[] mParameters = { new SqlParameter("@P_NVP_SeqID", NameValuePairSeqID), new SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqId) };
            string myStoreProcedure = "ZGWSecurity.Get_Name_Value_Pair_Roles";
            return base.GetDataTable(myStoreProcedure, mParameters);
        }

        DataTable IDNameValuePair.GetGroups(int NameValuePairSeqID)
        {
            SqlParameter[] mParameters = { new SqlParameter("@P_NVP_SeqID", NameValuePairSeqID), new SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqId) };
            string myStoreProcedure = "ZGWSecurity.Get_Name_Value_Pair_Groups";
            return base.GetDataTable(myStoreProcedure, mParameters);
        }

        void IDNameValuePair.UpdateGroups(int NVP_ID, int SecurityEntityID, string CommaSeparatedGroups, MNameValuePair nvpProfile)
        {
            string myStoreProcedure = "ZGWSecurity.Set_Name_Value_Pair_Groups";
            SqlParameter[] mParameters = { new SqlParameter("@P_NVP_SeqID", NVP_ID), new SqlParameter("@P_Security_Entity_SeqID", SecurityEntityID), new SqlParameter("@P_Groups", CommaSeparatedGroups), new SqlParameter("@P_Permissions_NVP_Detail_SeqID", m_PermissionSeqId), new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(nvpProfile)) };
            base.ExecuteNonQuery(myStoreProcedure, mParameters);
        }

        void IDNameValuePair.UpdateRoles(int NVP_ID, int SecurityEntityID, string CommaSeparatedRoles, MNameValuePair nvpProfile)
        {
            string myStoreProcdure = "ZGWSecurity.Set_Name_Value_Pair_Roles";
            SqlParameter[] mParameters = { new SqlParameter("@P_NVP_SeqID", NVP_ID), new SqlParameter("@P_Security_Entity_SeqID", SecurityEntityID), new SqlParameter("@P_Role", CommaSeparatedRoles), new SqlParameter("@P_Permissions_NVP_Detail_SeqID", m_PermissionSeqId), new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(nvpProfile)) };
            base.ExecuteNonQuery(myStoreProcdure, mParameters);
        }

        DataRow IDNameValuePair.GetNVPDetail()
        {
            string myStoreProcedure = "ZGWSystem.Get_Name_Value_Pair_Detail";
            SqlParameter[] mParameters = { 
											 new SqlParameter("@P_NVP_Detail_SeqID", m_Detail_Profile.Id), 
											 new SqlParameter("@P_NVP_SeqID", m_Detail_Profile.NameValuePairSeqId)
										 };
            return base.GetDataRow(myStoreProcedure, mParameters);
        }

        DataRow IDNameValuePair.GetNVPDetails(int NVPSeqDetID, int NVPSeqID)
        {
            string myStoreProcedure = "ZGWSystem.Get_Name_Value_Pair_Details";
            SqlParameter[] mParameters = { new SqlParameter("@P_NVP_SeqID", NVPSeqID) };
            return base.GetDataRow(myStoreProcedure, mParameters);
        }

        bool IDNameValuePair.DeleteNVPDetail(MNameValuePairDetail profile)
        {
            string myStoreProcedure = "ZGWSystem.Delete_Name_Value_Pair_Detail";
            Boolean mRetVal = false;
            SqlParameter[] mParameters = { new SqlParameter("@P_NVP_SeqID", profile.NameValuePairSeqId), new SqlParameter("@P_NVP_Detail_SeqID", profile.Id) };
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

        DataTable IDNameValuePair.GetAllNVPDetail()
        {
            string myStoreProcedure = "ZGWSystem.Get_Name_Value_Pair_Details";
            SqlParameter[] mParameters = { new SqlParameter("@P_NVP_SeqID", -1) };
            return base.GetDataTable(myStoreProcedure, mParameters);
        }

        DataTable IDNameValuePair.GetAllNVPDetail(int NVPSeqID)
        {
            string myStoreProcedure = "ZGWSystem.Get_Name_Value_Pair_Details";
            SqlParameter[] mParameters = { new SqlParameter("@P_NVP_SeqID", NVPSeqID) };
            return base.GetDataTable(myStoreProcedure, mParameters);
        }

        void IDNameValuePair.SaveNVPDetail(MNameValuePairDetail profile)
        {
            string myStoreProcedure = "ZGWSystem.Set_Name_Value_Pair_Detail";
            SqlParameter[] mParameters = { new SqlParameter("@P_NVP_Detail_SeqID", profile.Id), new SqlParameter("@P_NVP_SeqID", profile.NameValuePairSeqId), new SqlParameter("@P_NVP_Detail_Name", profile.Value), new SqlParameter("@P_NVP_Detail_Value", profile.Text), new SqlParameter("@P_Status_SeqID", profile.Status), new SqlParameter("@P_Sort_Order", profile.SortOrder), new SqlParameter("@P_Added_Updated_BY", GetAddedUpdatedBy(profile)), 
			GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output), GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output) };
            base.ExecuteNonQuery(myStoreProcedure, mParameters);
        }

        DataTable IDNameValuePair.Search(MSearchCriteria searchCriteria)
        {
            string mStoredProcedure = "ZGWSystem.Get_Paginated_Data";
            DataTable mRetVal;
            SqlParameter[] mParameters =
			 {
			  new SqlParameter("@P_Columns", searchCriteria.Columns),
			  new SqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
			  new SqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
			  new SqlParameter("@P_PageSize", searchCriteria.PageSize),
			  new SqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
			  new SqlParameter("@P_TableOrView", "ZGWSystem.vwSearchNVP"),
			  new SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
			 };
            mRetVal = base.GetDataTable(mStoredProcedure, mParameters);
            return mRetVal;
        }

        private SqlParameter[] GetInsertUpdateParameters()
        {
            SqlParameter[] mParameters = { 
				new SqlParameter("@P_NVP_SeqID", m_Profile.Id), 
				new SqlParameter("@P_Schema_Name", m_Profile.SchemaName), 
				new SqlParameter("@P_Static_Name", m_Profile.StaticName), 
				new SqlParameter("@P_Display", m_Profile.Display), 
				new SqlParameter("@P_Description", m_Profile.Description), 
				new SqlParameter("@P_Status_SeqID", m_Profile.Status), 
				new SqlParameter("@P_Added_Updated_BY", GetAddedUpdatedBy(m_Profile)), 
				GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output), 
				GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output) 
			};
            return mParameters;
        }

        private SqlParameter[] GetSelectParameters()
        {
            SqlParameter[] mParameters = { new SqlParameter("@P_NVP_SeqID", m_Profile.Id), new SqlParameter("@P_Account_SeqID", m_AccountId), new SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqId) };
            return mParameters;
        }
    }
}
