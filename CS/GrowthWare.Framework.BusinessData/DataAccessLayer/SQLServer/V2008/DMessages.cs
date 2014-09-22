using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.V2008
{
    public class DMessages : DDBInteraction, IDMessages
    {
        private MMessageProfile mProfile = new MMessageProfile();

        int IDMessages.SecurityEntitySeqId { get; set; }

        MMessageProfile IDMessages.Profile
        {
            get { return mProfile; }
            set { mProfile = value; }
        }

        DataTable IDMessages.GetAllMessages()
        {
            String storeProc = "ZGWCoreWeb.Get_Messages";
            SqlParameter[] myParameters = GetSelectParameters();
            return GetDataTable(storeProc, myParameters);
        }

        DataRow IDMessages.GetMessage()
        {
            String storeProc = "ZGWCoreWeb.Get_Messages";
            SqlParameter[] myParameters = GetSelectParameters();
            return GetDataRow(storeProc, myParameters);
        }

        void IDMessages.Save()
        {
            String storeProc = "ZGWCoreWeb.Set_Message";
            SqlParameter[] mParameters = GetInsertUpdateParameters();
            ExecuteNonQuery(storeProc, mParameters);
        }

        private SqlParameter[] GetInsertUpdateParameters()
        {
            SqlParameter[] myParameters = { 
				new SqlParameter("@P_Message_SeqID", mProfile.Id), new SqlParameter("@P_Security_Entity_SeqID", mProfile.SecurityEntitySeqId), 
				new SqlParameter("@P_Name", mProfile.Name), new SqlParameter("@P_Title", mProfile.Title), 
				new SqlParameter("@P_Description", mProfile.Description), new SqlParameter("@P_BODY", mProfile.Body), 
				new SqlParameter("@P_Format_As_HTML", mProfile.FormatAsHtml), new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(mProfile)), 
				GetSqlParameter("@P_PRIMARY_KEY", -1, ParameterDirection.Output) 
			};
            return myParameters;
        }

        private SqlParameter[] GetSelectParameters()
        {
            SqlParameter[] myParamaters = { 
				new SqlParameter("@P_Message_SeqID", mProfile.Id), 
				new SqlParameter("@P_Security_Entity_SeqID", mProfile.SecurityEntitySeqId)
			};
            return myParamaters;
        }

        DataTable IDMessages.Search(MSearchCriteria searchCriteria)
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
			  new SqlParameter("@P_TableOrView", "ZGWCoreWeb.vwSearchMessages"),
			  new SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
			 };
            mRetVal = GetDataTable(mStoredProcedure, mParameters);
            return mRetVal;
        }
    }
}
