using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.SQLServer;

/// <summary>
/// Provides data access to SQL Server 2008
/// </summary>
public class DSecurityEntities : AbstractDBInteraction, ISecurityEntities
{

#region Constructors
    public DSecurityEntities(string connectionString): base() 
    {
        this.ConnectionString = connectionString;
    }
#endregion

    async Task ISecurityEntities.DeleteRegistrationInformation(int securityEntitySeqId)
    {
        string mStoredProcedure = "[ZGWSecurity].[Delete_Registration_Information]";
        SqlParameter[] mParameters = [
            new("@P_SecurityEntitySeqId", securityEntitySeqId)
        ];
        await base.ExecuteNonQueryAsync(mStoredProcedure, mParameters);
    }

    async Task<DataTable> ISecurityEntities.GetRegistrationInformation()
    {
        string mStoredProcedure = "[ZGWSecurity].[Get_Registration_Information]";
        SqlParameter[] mParameters = [
            new("@P_SecurityEntitySeqId", -1)
        ];
        return await base.GetDataTableAsync(mStoredProcedure, mParameters);
    }

    async Task<DataTable> ISecurityEntities.GetSecurityEntities()
    {
        string mStoredProcedure = "[ZGWSecurity].[Get_Security_Entity]";
        SqlParameter[] mParameters = [
            new("@P_SecurityEntitySeqId", -1)
        ];
        return await base.GetDataTableAsync(mStoredProcedure, mParameters);
    }

    async Task<DataTable> ISecurityEntities.GetSecurityEntities(String account, int SecurityEntityID, bool isSecurityEntityAdministrator)
    {
        if (String.IsNullOrEmpty(account)) { throw new ArgumentNullException(nameof(account), "account cannot be a null reference (Nothing in Visual Basic)!"); };
        if (SecurityEntityID == -1) { throw new ArgumentNullException(nameof(SecurityEntityID), "SecurityEntityID cannot be a null reference (Nothing in Visual Basic)!"); };
        string mStoredProcedure = "[ZGWSecurity].[Get_Valid_Security_Entity]";
        SqlParameter[] mParameters = [
            new("@P_ACCT", account),
            new("@P_IS_SE_ADMIN", isSecurityEntityAdministrator),
            new("@P_SecurityEntityID", SecurityEntityID),
            GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
        ];
        return await base.GetDataTableAsync(mStoredProcedure, mParameters);
    }

    async Task<DataTable> ISecurityEntities.GetValidSecurityEntities(string account, int SecurityEntityID, bool isSystemAdmin)
    {
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException(nameof(account), "account cannot be a null reference (Nothing in Visual Basic)!");
        if (SecurityEntityID == -1) throw new ArgumentNullException(nameof(SecurityEntityID), "SecurityEntityID must be greater than -1");

        string mStoreProcedure = "[ZGWSecurity].[Get_Valid_Security_Entity]";
        SqlParameter[] myParameters = [
            new("@P_Account", account),
            new("@P_IS_SE_ADMIN", isSystemAdmin),
            new("@P_SecurityEntitySeqId", SecurityEntityID)
        ];
        return await base.GetDataTableAsync(mStoreProcedure, myParameters);
    }

    async Task<int> ISecurityEntities.Save(MSecurityEntity profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile can not be nothing");
        SqlParameter mPrimaryKey = GetSqlParameter("@P_PRIMARY_KEY", null, ParameterDirection.Output);
        mPrimaryKey.Size = 10;
        string mStoredProcedure = "[ZGWSecurity].[Set_Security_Entity]";
        SqlParameter[] mParameters = [
            new("@P_SecurityEntitySeqId", profile.Id),
            new("@P_NAME", profile.Name),
            new("@P_DESCRIPTION", profile.Description ?? ""),
            new("@P_URL", profile.Url ?? ""),
            new("@P_StatusSeqId", profile.StatusSeqId),
            new("@P_DAL", profile.DataAccessLayer),
            new("@P_DAL_Name", profile.DataAccessLayerAssemblyName),
            new("@P_DAL_NAME_SPACE", profile.DataAccessLayerNamespace),
            new("@P_DAL_STRING", profile.ConnectionString),
            new("@P_SKIN", profile.Skin),
            new("@P_STYLE", profile.Style ?? ""),
            new("@P_ENCRYPTION_TYPE", profile.EncryptionType),
            new("@P_ParentSecurityEntitySeqId", profile.ParentSeqId),
            new("@P_Added_Updated_By", GetAddedUpdatedBy(profile, profile.Id)),
            mPrimaryKey
        ];
        await base.ExecuteNonQueryAsync(mStoredProcedure, mParameters);
        profile.Id = int.Parse(GetParameterValue("@P_PRIMARY_KEY", mParameters).ToString(), CultureInfo.InvariantCulture);
        return profile.Id;
    }

    async Task<DataRow> ISecurityEntities.SaveRegistrationInformation(MRegistrationInformation profile)
    {
        string mStoredProcedure = "[ZGWSecurity].[Set_Registration_Information]";
        SqlParameter[] mParameters = [
            new ("@P_SecurityEntitySeqId", profile.Id),
            new ("@P_SecurityEntitySeqId_Owner", profile.SecurityEntitySeqIdOwner),
            new ("@P_AccountChoices", profile.AccountChoices),
            new ("@P_AddAccount", profile.AddAccount),
            new ("@P_Groups", profile.Groups),
            new ("@P_Roles", profile.Roles),
            new ("@P_Added_Updated_By", GetAddedUpdatedBy(profile, profile.Id))
        ];
        try
        {
            return await base.GetDataRowAsync(mStoredProcedure, mParameters);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
