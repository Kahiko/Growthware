using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Interfaces;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.SQLServer;
/// <summary>
/// DAccounts provides all database interaction to SQL Server 2008
/// </summary>
/// <remarks>
///		The Profile and SecurityEntitySeqID properties must be set
///		before using any methods.
///		Properties where chosen instead of parameters because all
///		methods will need one or both to perform their work.
///	</remarks>    
public class DAccounts : AbstractDBInteraction, IAccount
{

#region Member Fields
    private MAccountProfile m_Profile = null;
    private int m_SecurityEntitySeqID = -2;
#endregion

#region Public Properties
    MAccountProfile IAccount.Profile
    {
        get { return this.m_Profile; }
        set { this.m_Profile = value; }
    }

    int IAccount.SecurityEntitySeqId
    {
        get { return m_SecurityEntitySeqID; }
        set { m_SecurityEntitySeqID = value; }
    }

    async Task<DataRow> IAccount.GetAccount()
    {
        String mStoredProcedure = "[ZGWSecurity].[Get_Account]";
        SqlParameter[] mParameters = [
                GetSqlParameter("@P_Is_System_Admin", m_Profile.IsSystemAdmin, ParameterDirection.Input),
                GetSqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqID, ParameterDirection.Input),
                GetSqlParameter("@P_Account", this.Cleanup(m_Profile.Account), ParameterDirection.Input)
            ];
        return await base.GetDataRowAsync(mStoredProcedure, mParameters);
    }

    async Task<DataRow> IAccount.GetAccountByRefreshToken()
    {
        String mStoredProcedure = "[ZGWSecurity].[Get_Account_By_Refresh_Token]";
        SqlParameter[] mParameters = [
            GetSqlParameter("@P_Token", this.Cleanup(m_Profile.Token), ParameterDirection.Input)
        ];
        return await base.GetDataRowAsync(mStoredProcedure, mParameters);
    }

    async Task<DataRow> IAccount.GetAccountByResetToken()
    {
        String mStoredProcedure = "[ZGWSecurity].[Get_Account_By_Reset_Token]";
        SqlParameter[] mParameters = [
            GetSqlParameter("@P_ResetToken", this.Cleanup(m_Profile.ResetToken), ParameterDirection.Input)
        ];
        return await base.GetDataRowAsync(mStoredProcedure, mParameters);
    }

    async Task<DataRow> IAccount.GetAccountByVerificationToken()
    {
        String mStoredProcedure = "[ZGWSecurity].[Get_Account_By_Verification_Token]";
        SqlParameter[] mParameters = [
            GetSqlParameter("@P_VerificationToken", this.Cleanup(m_Profile.VerificationToken), ParameterDirection.Input)
        ];
        return await base.GetDataRowAsync(mStoredProcedure, mParameters);
    }

    async Task<DataTable> IAccount.GetAccounts()
    {
        checkValid();
        String mStoredProcedure = "[ZGWSecurity].[Get_Account]";
        SqlParameter[] mParameters = [
            new("@P_Is_System_Admin", m_Profile.IsSystemAdmin),
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID),
            new("@P_Account", "")
        ];
        return await base.GetDataTableAsync(mStoredProcedure, mParameters);
    }
#endregion

#region Constructors
    public DAccounts(string connectionString, int securityEntitySeqID) : base() 
    { 
        this.ConnectionString = connectionString;
        this.m_SecurityEntitySeqID = securityEntitySeqID;
    }
#endregion

#region Public Methods
    bool IAccount.RefreshTokenExists(string refreshToken)
    {
        bool mRetVal = false;
        Int32 mCount = 0;
        string mCleanedValue = this.Cleanup(refreshToken);
        string mCommandText = "SELECT COUNT(*) FROM [ZGWSecurity].[RefreshTokens] WHERE [Token] = @P_Token";
        SqlParameter[] mParameters = {
                new("@P_Token", mCleanedValue),
            };
        var mDbValue = base.ExecuteScalar(mCommandText, mParameters, true);
        if (mDbValue != null)
        {
            mCount = (Int32)mDbValue;
            if (mCount == 0)
            {
                mRetVal = true;
            }
        }
        return mRetVal;
    }

    async Task<DataTable> IAccount.RefreshTokens()
    {
        string mCommandText = "SELECT [RefreshTokenId], RT.[AccountSeqId], [Token], [Expires], [Created], [CreatedByIp], [Revoked], [RevokedByIp], [ReplacedByToken], [ReasonRevoked] ";
        mCommandText += "FROM [ZGWSecurity].[RefreshTokens] RT ";
        mCommandText += "INNER JOIN [ZGWSecurity].[Accounts] ACCT ON ACCT.[Account] = @P_Account AND RT.AccountSeqId = ACCT.[AccountSeqId] ";
        mCommandText += "ORDER BY [Created] ASC;";
        SqlParameter[] mParameters = [
            new("@P_Account", m_Profile.Account),
        ];
        return await base.GetDataTableAsync(mCommandText, mParameters, true);
    }

    bool IAccount.ResetTokenExists(string resetToken)
    {
        bool mRetVal = false;
        Int32 mCount = 0;
        string mCleanedValue = this.Cleanup(resetToken);
        string mCommandText = "SELECT COUNT(*) FROM [ZGWSecurity].[Accounts] WHERE [ResetToken] = @P_ResetToken";
        SqlParameter[] mParameters = {
                new("@P_ResetToken", mCleanedValue),
            };
        var mDbValue = base.ExecuteScalar(mCommandText, mParameters, true);
        if (mDbValue != null)
        {
            mCount = (Int32)mDbValue;
            if (mCount == 0)
            {
                mRetVal = true;
            }
        }
        return mRetVal;
    }

    async Task<DataTable> IAccount.Roles()
    {
        checkValid();
        String mStoredProcedure = "[ZGWSecurity].[Get_Account_Roles]";
        SqlParameter[] mParameters = [
            new("@P_Account", this.Cleanup(m_Profile.Account)),
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID)
        ];
        return await base.GetDataTableAsync(mStoredProcedure, mParameters);
    }

    async Task<DataTable> IAccount.GetMenu(string account, MenuType menuType)
    {
        String mStoredProcedure = "[ZGWSecurity].[Get_Menu_Data]";
        SqlParameter[] mParameters = [
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID),
            new("@P_Navigation_Types_NVP_DetailSeqId", (int)menuType),
            new("@P_Account", this.Cleanup(account))
        ];
        return await base.GetDataTableAsync(mStoredProcedure, mParameters);
    }

    async Task<DataTable> IAccount.Groups()
    {
        checkValid();
        String mStoredProcedure = "[ZGWSecurity].[Get_Account_Groups]";
        SqlParameter[] mParameters = [
            new("@P_Account", this.Cleanup(m_Profile.Account)),
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID)
        ];
        return await base.GetDataTableAsync(mStoredProcedure, mParameters);
    }

    async Task<DataTable> IAccount.Security()
    {
        String mStoredProcedure = "[ZGWSecurity].[Get_Account_Security]";
        SqlParameter[] mParameters = [
            new("@P_Account", this.Cleanup(m_Profile.Account)),
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID)
        ];
        return await base.GetDataTableAsync(mStoredProcedure, mParameters);
    }

    int IAccount.Save()
    {
        checkValid();
        int mRetInt;
        String mStoredProcedure = "ZGWSecurity.Set_Account";
        // 1/1/1753 12:00:00 AM
        DateTime mTestDate = new(0001, 1, 1, 0, 0, 0);
        if (m_Profile.LastLogOn == mTestDate)
        {
            m_Profile.LastLogOn = new(1753, 1, 1, 0, 0, 0);
        }
        if (m_Profile.PasswordLastSet == mTestDate)
        {
            m_Profile.PasswordLastSet = new(1753, 1, 1, 0, 0, 0);
        }
        SqlParameter[] mParameters = [
            GetSqlParameter("@P_AccountSeqId", m_Profile.Id, ParameterDirection.InputOutput),
            new("@P_StatusSeqId", m_Profile.Status),
            new("@P_Account", this.Cleanup(m_Profile.Account)),
            new("@P_First_Name", this.Cleanup(m_Profile.FirstName)),
            new("@P_Last_Name", this.Cleanup(m_Profile.LastName)),
            new("@P_Middle_Name", !string.IsNullOrWhiteSpace(m_Profile.MiddleName) ? m_Profile.MiddleName : DBNull.Value),
            new("@P_Preferred_Name", !string.IsNullOrWhiteSpace(m_Profile.PreferredName) ? m_Profile.PreferredName : DBNull.Value),
            new("@P_Email", m_Profile.Email),
            new("@P_Password", m_Profile.Password),
            new("@P_Password_Last_Set", m_Profile.PasswordLastSet),
            new("@P_ResetToken", !string.IsNullOrWhiteSpace(m_Profile.ResetToken) ? m_Profile.ResetToken : DBNull.Value),
            new("@P_ResetTokenExpires", !string.IsNullOrWhiteSpace(m_Profile.ResetTokenExpires.ToString()) ? m_Profile.ResetTokenExpires : DBNull.Value),
            new("@P_Failed_Attempts", m_Profile.FailedAttempts),
            new("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id)),
            new("@P_Last_Login", m_Profile.LastLogOn),
            new("@P_Time_Zone", m_Profile.TimeZone),
            new("@P_Location", m_Profile.Location),
            new("@P_Enable_Notifications", m_Profile.EnableNotifications),
            new("@P_Is_System_Admin", m_Profile.IsSystemAdmin),
            new("@P_VerificationToken", !string.IsNullOrWhiteSpace(m_Profile.VerificationToken) ? m_Profile.VerificationToken : DBNull.Value)
        ];
        base.ExecuteNonQuery(mStoredProcedure, mParameters);
        mRetInt = int.Parse(GetParameterValue("@P_AccountSeqId", mParameters), CultureInfo.InvariantCulture);
        return mRetInt;
    }

    void IAccount.SaveGroups()
    {
        checkValid();
        String mStoredProcedure = "ZGWSecurity.Set_Account_Groups";
        SqlParameter[] mParameters = {
            new("@P_Account", this.Cleanup(m_Profile.Account)),
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID),
            new("@P_Groups", m_Profile.GetCommaSeparatedAssignedGroups),
            new("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id))
            };
        base.ExecuteNonQuery(mStoredProcedure, mParameters);
    }

    void IAccount.SaveRefreshTokens()
    {
        MRefreshToken[] mRefreshTokens = this.m_Profile.RefreshTokens.ToArray();
        IDatabaseTable mFirstObj = (IDatabaseTable)mRefreshTokens.First();
        string mTempTableName = "[" + Guid.NewGuid().ToString() + "]";
        bool mIncludePrimaryKey = false;
        string mPrimaryKeyName = mFirstObj.PrimaryKeyName;

        DTO_BulkInsert_Parameters mBulkInsertParameters = new()
        {
            DestinationTableName = mFirstObj.TableName,
            DoDelete = true,
            ForeignKeyName = mFirstObj.ForeignKeyName,
            IncludePrimaryKey = mIncludePrimaryKey,
            ListOfProfiles = mRefreshTokens,
            NumberOfProfiles = mRefreshTokens.Count(),
            TempTableName = mTempTableName,
            PrimaryKeyName = mPrimaryKeyName
        };

        base.BulkInsert(mBulkInsertParameters);
    }

    void IAccount.SaveRoles()
    {
        checkValid();
        String mStoredProcedure = "ZGWSecurity.Set_Account_Roles";
        SqlParameter[] mParameters = {
            new("@P_Account", this.Cleanup(m_Profile.Account)),
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID),
            new("@P_Roles", m_Profile.GetCommaSeparatedAssignedRoles),
            new("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id))
            };
        base.ExecuteNonQuery(mStoredProcedure, mParameters);
    }

    void IAccount.Delete()
    {
        string myStoreProcedure = "ZGWSecurity.Delete_Account";
        SqlParameter[] myParameters = { new("@P_AccountSeqId", m_Profile.Id) };
        base.ExecuteNonQuery(myStoreProcedure, myParameters);
    }

    bool IAccount.VerificationTokenExists(string token)
    {
        bool mRetVal = true;
        Int32 mCount = 0;
        string mCleanedValue = this.Cleanup(token);
        string mCommandText = "SELECT TOP(1) * FROM [ZGWSecurity].[Accounts] WHERE [VerificationToken] = @P_Token";
        SqlParameter[] mParameters = {
                new("@P_Token", mCleanedValue),
            };
        var mDbValue = base.ExecuteScalar(mCommandText, mParameters, true);
        if (mDbValue != null)
        {
            mCount = (Int32)mDbValue;
        }
        if (mCount == 0)
        {
            mRetVal = false;
        }
        return mRetVal;
    }
#endregion

#region Private Methods
    private void checkValid()
    {
        base.IsValid();
        if (m_Profile == null)
        {
            throw new DataAccessLayerException("The Profile property must set before using any methods from this class.");
        }
        if (m_SecurityEntitySeqID == -2)
        {
            throw new DataAccessLayerException("The SecurityEntityID property must set before using any methods from this class.");
        }
    }
#endregion
}
