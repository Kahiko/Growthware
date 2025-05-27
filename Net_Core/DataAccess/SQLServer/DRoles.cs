using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.SQLServer;

/// <summary>
/// Class DRoles.
/// </summary>
public class DRoles : AbstractDBInteraction, IRoles
{

    #region Member Fields
    private int m_SecurityEntityID;

    private MRole m_Profile = new();
    #endregion

    #region Constructors
    public DRoles(string connectionString, int securityEntitySeqId) : base()
    {
        this.ConnectionString = connectionString;
        this.m_SecurityEntityID = securityEntitySeqId;
    }
    #endregion

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

    async Task IRoles.DeleteRole(string roleName, int securityEntitySeqId)
    {
        SqlParameter[] mParameters = [new("@P_Name", roleName), new("@P_SecurityEntitySeqId", securityEntitySeqId)];
        String mStoreProc = "[ZGWSecurity].[Delete_Role]";
        await base.ExecuteNonQueryAsync(mStoreProc, mParameters);
    }

    async Task<int> IRoles.Save(MRole profile)
    {
        SqlParameter[] mParameters = getInsertUpdateParameters(profile);
        string myStoreProcedure = "[ZGWSecurity].[Set_Role]";
        await base.ExecuteNonQueryAsync(myStoreProcedure, mParameters);
        int mRetVal = int.Parse(GetParameterValue("@P_Primary_Key", mParameters));
        return mRetVal;
    }

    async Task<DataTable> IRoles.RolesBySecurityEntity(int securityEntitySeqId)
    {
        // a roleSeqId of -1 will return all rows for the given securityEntitySeqId
        SqlParameter[] myParameters = [new("@P_RoleSeqId", -1), new("@P_SecurityEntitySeqId", securityEntitySeqId)];
        String myStoreProc = "[ZGWSecurity].[Get_Role]";
        return await base.GetDataTableAsync(myStoreProc, myParameters);
    }

    async Task<DataRow> IRoles.ProfileData(int roleSeqId)
    {
        // a roleSeqId <> -1 will return a single row for the given roleSeqId and the securityEntitySeqId is ignored
        SqlParameter[] myParameters = [new("@P_RoleSeqId", roleSeqId), new("@P_SecurityEntitySeqId", -1)];
        String myStoreProc = "[ZGWSecurity].[Get_Role]";
        return await base.GetDataRowAsync(myStoreProc, myParameters);
    }

    async Task<DataTable> IRoles.AccountsInRole()
    {
        SqlParameter[] myParameters = [new("@P_SecurityEntitySeqId", m_SecurityEntityID), new("@P_RoleSeqId", m_Profile.Id)];
        string myStoreProcedure = "[ZGWSecurity].[Get_Accounts_In_Role]";
        return await base.GetDataTableAsync(myStoreProcedure, myParameters);
    }

    async Task<DataTable> IRoles.AccountsNotInRole()
    {
        SqlParameter[] myParameters = [new("@P_SecurityEntitySeqId", m_SecurityEntityID), new("@P_RoleSeqId", m_Profile.Id)];
        string myStoreProcedure = "[ZGWSecurity].[Get_Accounts_Not_In_Role]";
        return await base.GetDataTableAsync(myStoreProcedure, myParameters);
    }

    async Task<bool> IRoles.UpdateAllAccountsForRole(int roleSeqID, int SecurityEntityID, string[] accounts, int accountSeqID)
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
            SqlCommand mSqlCommand = new SqlCommand("[ZGWSecurity].[Delete_Roles_Accounts]", mSqlConnection);
            mSqlCommand.CommandType = CommandType.StoredProcedure;
            mSqlCommand.Transaction = mSqlTransaction;

            SqlParameter mSqlParameter = new("@P_ROLE_SEQ_ID", roleSeqID);
            mSqlCommand.Parameters.Add(mSqlParameter);
            mSqlParameter = new("@P_SecurityEntitySeqId", SecurityEntityID);
            mSqlCommand.Parameters.Add(mSqlParameter);
            await mSqlCommand.ExecuteNonQueryAsync();

            mSqlCommand.CommandText = "[ZGWSecurity].[Set_Role_Accounts]";
            foreach (string account_loopVariable in accounts)
            {
                mAccount = account_loopVariable;
                mSqlCommand.Parameters.Clear();
                mSqlParameter = new("@P_RoleSeqId", roleSeqID);
                mSqlCommand.Parameters.Add(mSqlParameter);
                mSqlParameter = new("@P_SecurityEntitySeqId", SecurityEntityID);
                mSqlCommand.Parameters.Add(mSqlParameter);
                mSqlParameter = new("@P_Account", mAccount);
                mSqlCommand.Parameters.Add(mSqlParameter);
                mSqlParameter = new("@P_Added_Updated_By", accountSeqID);
                mSqlCommand.Parameters.Add(mSqlParameter);
                await mSqlCommand.ExecuteNonQueryAsync();
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

    private SqlParameter[] getInsertUpdateParameters(MRole profile)
    {
        SqlParameter[] mParameters =
        {
            new("@P_RoleSeqId", profile.Id),
            new("@P_Name", profile.Name),
            new("@P_Description", profile.Description),
            new("@P_Is_System", profile.IsSystem),
            new("@P_Is_System_Only", profile.IsSystemOnly),
            new("@P_SecurityEntitySeqId", profile.SecurityEntityID),
            new("@P_Added_Updated_By", GetAddedUpdatedBy(profile, profile.Id)),
            GetSqlParameter("@P_Primary_Key", profile.Id, ParameterDirection.Output)
        };
        return mParameters;
    }

}
