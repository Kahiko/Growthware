using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base;
using GrowthWare.Framework.Model.Profiles;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.V2008
{
    /// <summary>
    /// Class DRoles.
    /// </summary>
    public class DRoles : DDBInteraction, IDRoles
    {
        private int m_SE_SEQ_ID;

        private MRoleProfile m_Profile = new MRoleProfile();

        int IDRoles.SecurityEntitySeqID
        {
            get { return m_SE_SEQ_ID; }
            set { m_SE_SEQ_ID = value; }
        }

        MRoleProfile IDRoles.Profile
        {
            get { return m_Profile; }
            set { m_Profile = value; }
        }

        void IDRoles.DeleteRole()
        {
            SqlParameter[] mParameters = { new SqlParameter("@P_Name", m_Profile.Name), new SqlParameter("@P_Security_Entity_SeqID", m_SE_SEQ_ID) };
            String mStoreProc = "ZGWSecurity.Delete_Role";
            base.ExecuteNonQuery( mStoreProc,  mParameters);
        }

        void IDRoles.Save()
        {
            SqlParameter[] myParameters = getInsertUpdateParameters();
            string myStoreProcedure = "ZGWSecurity.Set_Role";
            base.ExecuteNonQuery( myStoreProcedure,  myParameters);
        }

        DataTable IDRoles.Search( MSearchCriteria searchCriteria)
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
			  new SqlParameter("@P_TableOrView", "ZGWSecurity.vwSearchRoles"),
			  new SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
			 };
            mRetVal = base.GetDataTable( mStoredProcedure,  mParameters);
            return mRetVal;
        }

        DataTable IDRoles.RolesBySecurityEntity()
        {
            SqlParameter[] myParameters = { new SqlParameter("@P_Role_SeqID", -1), new SqlParameter("@P_Security_Entity_SeqID", m_SE_SEQ_ID) };
            String myStoreProc = "ZGWSecurity.Get_Role";
            return base.GetDataTable( myStoreProc,  myParameters);
        }

        DataRow IDRoles.ProfileData()
        {
            SqlParameter[] myParameters = { new SqlParameter("@P_Role_SeqID", m_Profile.Id), new SqlParameter("@P_Security_Entity_SeqID", -1) };
            String myStoreProc = "ZGWSecurity.Get_Role";
            return base.GetDataRow( myStoreProc,  myParameters);
        }

        DataTable IDRoles.AccountsInRole()
        {
            SqlParameter[] myParameters = { new SqlParameter("@P_Security_Entity_SeqID", m_SE_SEQ_ID), new SqlParameter("@P_Role_SeqID", m_Profile.Id) };
            string myStoreProcedure = "ZGWSecurity.Get_Accounts_In_Role";
            return base.GetDataTable( myStoreProcedure,  myParameters);
        }

        DataTable IDRoles.AccountsNotInRole()
        {
            SqlParameter[] myParameters = { new SqlParameter("@P_Security_Entity_SeqID", m_SE_SEQ_ID), new SqlParameter("@P_Role_SeqID", m_Profile.Id) };
            string myStoreProcedure = "ZGWSecurity.Get_Accounts_Not_In_Role";
            return base.GetDataTable( myStoreProcedure,  myParameters);
        }

        bool IDRoles.UpdateAllAccountsForRole(int roleSeqID, int securityEntityID, string[] accounts, int accountSeqID)
        {
            bool success = false;
            SqlConnection dbConn = null;
            SqlTransaction trans = null;
            string account = null;
            try
            {
                dbConn = new SqlConnection(ConnectionString);
                dbConn.Open();
                trans = dbConn.BeginTransaction(IsolationLevel.Serializable);
                SqlDatabase db = new SqlDatabase(base.ConnectionString);
                // delete all the accounts for this role/SecurityEntity
                System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand("ZGWSecurity.Delete_Roles_Accounts");
                SqlParameter myParameter = new SqlParameter("@P_ROLE_SEQ_ID", roleSeqID);
                dbCommand.Parameters.Add(myParameter);
                myParameter = new SqlParameter("@P_Security_Entity_SeqID", securityEntityID);
                dbCommand.Parameters.Add(myParameter);
                db.ExecuteNonQuery(dbCommand, trans);

                foreach (string account_loopVariable in accounts)
                {
                    account = account_loopVariable;
                    dbCommand.Parameters.Clear();
                    dbCommand = db.GetStoredProcCommand("ZGWSecurity.Set_Role_Accounts");
                    myParameter = new SqlParameter("@P_Role_SeqID", roleSeqID);
                    dbCommand.Parameters.Add(myParameter);
                    myParameter = new SqlParameter("@P_Security_Entity_SeqID", securityEntityID);
                    dbCommand.Parameters.Add(myParameter);
                    myParameter = new SqlParameter("@P_Account", account);
                    dbCommand.Parameters.Add(myParameter);
                    myParameter = new SqlParameter("@P_Added_Updated_By", accountSeqID);
                    dbCommand.Parameters.Add(myParameter);
                    db.ExecuteNonQuery(dbCommand, trans);
                }

                trans.Commit();
                success = true;
            }
            catch (Exception)
            {
                if ((trans != null)) trans.Rollback();
                throw;
            }
            finally
            {
                if ((trans != null))
                {
                    trans.Dispose();
                    trans = null;
                }
                if ((dbConn != null))
                {
                    dbConn.Dispose();
                    dbConn = null;
                }
            }
            return success;
        }

        private SqlParameter[] getInsertUpdateParameters()
        {
            SqlParameter[] myParameters = 
			{ 
				new SqlParameter("@P_Role_SeqID", m_Profile.Id), 
				new SqlParameter("@P_Name", m_Profile.Name), 
				new SqlParameter("@P_Description", m_Profile.Description), 
				new SqlParameter("@P_Is_System", m_Profile.IsSystem), 
				new SqlParameter("@P_Is_System_Only", m_Profile.IsSystemOnly), 
				new SqlParameter("@P_Security_Entity_SeqID", m_SE_SEQ_ID), 
				new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile)), 
				GetSqlParameter("@P_Primary_Key", m_Profile.Id, ParameterDirection.Output) 
			};
            return myParameters;
        }

    }
}
