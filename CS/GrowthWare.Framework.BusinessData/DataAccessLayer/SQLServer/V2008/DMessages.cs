using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.V2008
{
    /// <summary>
    /// Class DMessages
    /// </summary>
    public class DMessages : DDBInteraction, IDMessages
    {
        private MMessageProfile m_Profile = new MMessageProfile();
        private SqlParameter[] GetInsertUpdateParameters()
        {
            SqlParameter[] myParameters = { 
				new SqlParameter("@P_Message_SeqID", m_Profile.Id), new SqlParameter("@P_Security_Entity_SeqID", m_Profile.SecurityEntitySeqId), 
				new SqlParameter("@P_Name", m_Profile.Name), new SqlParameter("@P_Title", m_Profile.Title), 
				new SqlParameter("@P_Description", m_Profile.Description), new SqlParameter("@P_BODY", m_Profile.Body), 
				new SqlParameter("@P_Format_As_HTML", m_Profile.FormatAsHtml), new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile)), 
				GetSqlParameter("@P_PRIMARY_KEY", -1, ParameterDirection.Output) 
			};
            return myParameters;
        }


        DataTable IDMessages.Search(MSearchCriteria searchCriteria)
        {
            if (searchCriteria == null) throw new ArgumentNullException("searchCriteria", "searchCriteria can not be null (Nothing in VB) or empty!");
            string mStoredProcedure = "ZGWSystem.Get_Paginated_Data";
            DataTable mRetVal;
            SqlParameter[] mParameters =
			 {
			  new SqlParameter("@P_Columns", searchCriteria.Columns),
			  new SqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
			  new SqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
			  new SqlParameter("@P_PageSize", searchCriteria.PageSize),
			  new SqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
			  new SqlParameter("@P_TableOrView", "ZGWCoreWeb.vwSearchMessages"),
			  new SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
			 };
            mRetVal = GetDataTable(mStoredProcedure, mParameters);
            return mRetVal;
        }

        MMessageProfile IDMessages.Profile
        {
            get
            {
                return m_Profile;
            }
            set
            {
                m_Profile = value;
            }
        }

        int IDMessages.SecurityEntitySeqId { get; set; }

        DataTable IDMessages.Messages()
        {
            String storeProc = "ZGWCoreWeb.Get_Messages";
            SqlParameter[] mParamaters = { 
				new SqlParameter("@P_Message_SeqID", -1), 
				new SqlParameter("@P_Security_Entity_SeqID", m_Profile.SecurityEntitySeqId)
			};
            return GetDataTable(storeProc, mParamaters);
        }

        DataRow IDMessages.Message(int messageSeqId)
        {
            String storeProc = "ZGWCoreWeb.Get_Messages";
            SqlParameter[] mParamaters = { 
				new SqlParameter("@P_Message_SeqID", messageSeqId), 
				new SqlParameter("@P_Security_Entity_SeqID", m_Profile.SecurityEntitySeqId)
			};
            return GetDataRow(storeProc, mParamaters);
        }

        void IDMessages.Save()
        {
            String storeProc = "ZGWCoreWeb.Set_Message";
            SqlParameter[] mParameters = GetInsertUpdateParameters();
            ExecuteNonQuery(storeProc, mParameters);
        }
    }
}
