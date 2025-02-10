using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.DataAccess.SQLServer
{
    /// <summary>
    /// Class DRoles.
    /// </summary>
    public class DRoles : AbstractDBInteraction, IRoles
    {
        private int m_SecurityEntityID;

        private MRole m_Profile = new MRole();

        int IRoles.SecurityEntitySeqID
        {
            get { return m_SecurityEntityID; }
            set { m_SecurityEntityID = value; }
        }

        MRole IRoles.Profile
        {
            get { return m_Profile; }
            set { m_Profile = value; }
        }

        void IRoles.DeleteRole()
        {
            SqlParameter[] mParameters = { new SqlParameter("@P_Name", m_Profile.Name), new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntityID) };
            String mStoreProc = "ZGWSecurity.Delete_Role";
            base.ExecuteNonQuery( mStoreProc,  mParameters);
        }

        int IRoles.Save()
        {
            SqlParameter[] mParameters = getInsertUpdateParameters();
            string myStoreProcedure = "ZGWSecurity.Set_Role";
            base.ExecuteNonQuery( myStoreProcedure,  mParameters);
            int mRetVal = int.Parse(GetParameterValue("@P_Primary_Key", mParameters));
            return mRetVal;
        }

        DataTable IRoles.RolesBySecurityEntity()
        {
            SqlParameter[] myParameters = { new SqlParameter("@P_RoleSeqId", -1), new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntityID) };
            String myStoreProc = "ZGWSecurity.Get_Role";
            return base.GetDataTable( myStoreProc,  myParameters);
        }

        DataRow IRoles.ProfileData()
        {
            SqlParameter[] myParameters = { new SqlParameter("@P_RoleSeqId", m_Profile.Id), new SqlParameter("@P_SecurityEntitySeqId", -1) };
            String myStoreProc = "ZGWSecurity.Get_Role";
            return base.GetDataRow( myStoreProc,  myParameters);
        }

        DataTable IRoles.AccountsInRole()
        {
            SqlParameter[] myParameters = { new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntityID), new SqlParameter("@P_RoleSeqId", m_Profile.Id) };
            string myStoreProcedure = "ZGWSecurity.Get_Accounts_In_Role";
            return base.GetDataTable( myStoreProcedure,  myParameters);
        }

        DataTable IRoles.AccountsNotInRole()
        {
            SqlParameter[] myParameters = { new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntityID), new SqlParameter("@P_RoleSeqId", m_Profile.Id) };
            string myStoreProcedure = "ZGWSecurity.Get_Accounts_Not_In_Role";
            return base.GetDataTable( myStoreProcedure,  myParameters);
        }

        bool IRoles.UpdateAllAccountsForRole(int roleSeqID, int SecurityEntityID, string[] accounts, int accountSeqID)
        {
            bool mRetVal = false;
            SqlConnection mSqlConnection = null;
            SqlTransaction mSqlTransaction = null;
            string mAccount = null;
            try
            {
                mSqlConnection = new SqlConnection(ConnectionString);
                mSqlConnection.Open();
                mSqlTransaction = mSqlConnection.BeginTransaction(IsolationLevel.Serializable);
                // delete all the accounts for this role/SecurityEntity
                SqlCommand mSqlCommand = new SqlCommand("ZGWSecurity.Delete_Roles_Accounts", mSqlConnection);
                mSqlCommand.CommandType = CommandType.StoredProcedure;
                mSqlCommand.Transaction = mSqlTransaction;

                SqlParameter mSqlParameter = new SqlParameter("@P_ROLE_SEQ_ID", roleSeqID);
                mSqlCommand.Parameters.Add(mSqlParameter);
                mSqlParameter = new SqlParameter("@P_SecurityEntitySeqId", SecurityEntityID);
                mSqlCommand.Parameters.Add(mSqlParameter);
                mSqlCommand.ExecuteNonQuery();

                mSqlCommand.CommandText = "ZGWSecurity.Set_Role_Accounts";
                foreach (string account_loopVariable in accounts)
                {
                    mAccount = account_loopVariable;
                    mSqlCommand.Parameters.Clear();
                    mSqlParameter = new SqlParameter("@P_RoleSeqId", roleSeqID);
                    mSqlCommand.Parameters.Add(mSqlParameter);
                    mSqlParameter = new SqlParameter("@P_SecurityEntitySeqId", SecurityEntityID);
                    mSqlCommand.Parameters.Add(mSqlParameter);
                    mSqlParameter = new SqlParameter("@P_Account", mAccount);
                    mSqlCommand.Parameters.Add(mSqlParameter);
                    mSqlParameter = new SqlParameter("@P_Added_Updated_By", accountSeqID);
                    mSqlCommand.Parameters.Add(mSqlParameter);
                    mSqlCommand.ExecuteNonQuery();
                }

                mSqlTransaction.Commit();
                mRetVal = true;
            }
            catch (Exception)
            {
                if ((mSqlTransaction != null)) mSqlTransaction.Rollback();
                throw;
            }
            finally
            {
                if ((mSqlTransaction != null))
                {
                    mSqlTransaction.Dispose();
                    mSqlTransaction = null;
                }
                if ((mSqlConnection != null))
                {
                    mSqlConnection.Dispose();
                    mSqlConnection = null;
                }
            }
            return mRetVal;
        }

        private SqlParameter[] getInsertUpdateParameters()
        {
            SqlParameter[] myParameters = 
			{ 
				new SqlParameter("@P_RoleSeqId", m_Profile.Id), 
				new SqlParameter("@P_Name", m_Profile.Name), 
				new SqlParameter("@P_Description", m_Profile.Description), 
				new SqlParameter("@P_Is_System", m_Profile.IsSystem), 
				new SqlParameter("@P_Is_System_Only", m_Profile.IsSystemOnly), 
				new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntityID), 
				new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id)), 
				GetSqlParameter("@P_Primary_Key", m_Profile.Id, ParameterDirection.Output) 
			};
            return myParameters;
        }

    }
}
