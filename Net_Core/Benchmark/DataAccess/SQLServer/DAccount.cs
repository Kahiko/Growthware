using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.Benchmark.DataAccess.SQLServer;

[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class DAccounts : AbstractDBInteraction, IDBInteraction
{

    #region Member Fields
    private string m_ConnectionString = ConfigSettings.ConnectionString;
    private MAccountProfile m_Profile;
    private int m_SecurityEntitySeqID = 1;
    #endregion

    #region Constructors
    public DAccounts() : base()
    {
        this.ConnectionString = ConfigSettings.ConnectionString;
        this.m_SecurityEntitySeqID = 1;
        this.m_Profile = new()
        {
            Account = "Developer"
        };
    }
    #endregion

    [Benchmark(Baseline = true)]
    public MAccountProfile GetProfile()
    {
        string mCommandText = "ZGWSecurity.Get_Account";
        SqlParameter[] mParameters = {
                GetSqlParameter("@P_Is_System_Admin", m_Profile.IsSystemAdmin, ParameterDirection.Input),
                GetSqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqID, ParameterDirection.Input),
                GetSqlParameter("@P_Account", this.Cleanup(m_Profile.Account), ParameterDirection.Input)
            };
        DataRow mDataRow = base.GetDataRow(mCommandText, mParameters);

        mCommandText = "SELECT [RefreshTokenId], RT.[AccountSeqId], [Token], [Expires], [Created], [CreatedByIp], [Revoked], [RevokedByIp], [ReplacedByToken], [ReasonRevoked] ";
        mCommandText += "FROM [ZGWSecurity].[RefreshTokens] RT ";
        mCommandText += "INNER JOIN [ZGWSecurity].[Accounts] ACCT ON ACCT.[Account] = @P_Account AND RT.AccountSeqId = ACCT.[AccountSeqId] ";
        mCommandText += "ORDER BY [Created] ASC;";
        mParameters = [
            new("@P_Account", m_Profile.Account),
        ];
        DataTable mRefreshTokens = base.GetDataTable(mCommandText, mParameters, true);

        mCommandText = "ZGWSecurity.Get_Account_Roles";
        mParameters = [
            new("@P_Account", this.Cleanup(m_Profile.Account)),
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID)
        ];
        DataTable mAssignedRoles = base.GetDataTable(mCommandText, mParameters);

        mCommandText = "ZGWSecurity.Get_Account_Groups";
        mParameters = [
            new("@P_Account", this.Cleanup(m_Profile.Account)),
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID)
        ];
        DataTable mAssignedGroups = base.GetDataTable(mCommandText, mParameters);

        mCommandText = "ZGWSecurity.Get_Account_Security";
        mParameters = [
            new("@P_Account", this.Cleanup(m_Profile.Account)),
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID)
        ];
        DataTable mRoles = base.GetDataTable(mCommandText, mParameters);

        return new MAccountProfile(mDataRow, mRefreshTokens, mAssignedRoles, mAssignedGroups, mRoles);
    }

    [Benchmark]
    public async Task<MAccountProfile> GetProfileAsync()
    {
        string mCommandText = "ZGWSecurity.Get_Account";
        SqlParameter[] mParameters = [
            GetSqlParameter("@P_Is_System_Admin", m_Profile.IsSystemAdmin, ParameterDirection.Input),
            GetSqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqID, ParameterDirection.Input),
            GetSqlParameter("@P_Account", this.Cleanup(m_Profile.Account), ParameterDirection.Input)
        ];
        DataRow mDataRow = await base.GetDataRowAsync(mCommandText, mParameters);

        mCommandText = "SELECT [RefreshTokenId], RT.[AccountSeqId], [Token], [Expires], [Created], [CreatedByIp], [Revoked], [RevokedByIp], [ReplacedByToken], [ReasonRevoked] ";
        mCommandText += "FROM [ZGWSecurity].[RefreshTokens] RT ";
        mCommandText += "INNER JOIN [ZGWSecurity].[Accounts] ACCT ON ACCT.[Account] = @P_Account AND RT.AccountSeqId = ACCT.[AccountSeqId] ";
        mCommandText += "ORDER BY [Created] ASC;";
        mParameters = [
            new("@P_Account", m_Profile.Account),
        ];
        DataTable mRefreshTokens = await base.GetDataTableAsync(mCommandText, mParameters, true);

        mCommandText = "ZGWSecurity.Get_Account_Roles";
        mParameters = [
            new("@P_Account", this.Cleanup(m_Profile.Account)),
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID)
        ];
        DataTable mAssignedRoles = await base.GetDataTableAsync(mCommandText, mParameters);

        mCommandText = "ZGWSecurity.Get_Account_Groups";
        mParameters = [
            new("@P_Account", this.Cleanup(m_Profile.Account)),
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID)
        ];
        DataTable mAssignedGroups = await base.GetDataTableAsync(mCommandText, mParameters);

        mCommandText = "ZGWSecurity.Get_Account_Security";
        mParameters = [
            new("@P_Account", this.Cleanup(m_Profile.Account)),
            new("@P_SecurityEntitySeqId", m_SecurityEntitySeqID)
        ];
        DataTable mRoles = await base.GetDataTableAsync(mCommandText, mParameters);

        return new MAccountProfile(mDataRow, mRefreshTokens, mAssignedRoles, mAssignedGroups, mRoles);
    }

}