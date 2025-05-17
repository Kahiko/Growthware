using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.SQLServer;

/// <summary>
/// Class DDirectories.
/// </summary>
public class DDirectories : AbstractDBInteraction, IDirectories
{

#region Constructors
    public DDirectories(string connectionString, int securityEntitySeqId): base() 
    {
        this.ConnectionString = connectionString;
        this.SecurityEntitySeqId = securityEntitySeqId;
    }
#endregion

    async Task<DataTable> IDirectories.Directories()
    {
        String mStoredProcedure = "ZGWOptional.Get_Directory";
        SqlParameter[] mParameters = [
            new("@P_FunctionSeqId", -1)
        ];
        return await base.GetDataTableAsync(mStoredProcedure, mParameters);
    }

    async Task IDirectories.Save(MDirectoryProfile profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!");
        String mStoredProcedure = "[ZGWOptional].[Set_Directory]";
        String mImpersonateAccount = profile.ImpersonateAccount;
        String mImpersonatePassword = profile.ImpersonatePassword;
        if (!profile.Impersonate || string.IsNullOrWhiteSpace(profile.Directory))
        {
            mImpersonateAccount = string.Empty;
            mImpersonatePassword = string.Empty;
        }
        SqlParameter[] mParameters = [
            new("@P_FunctionSeqId", profile.Id),
            new("@P_Directory", profile.Directory),
            new("@P_Impersonate", profile.Impersonate),
            new("@P_Impersonating_Account", mImpersonateAccount),
            new("@P_Impersonating_Password", mImpersonatePassword),
            new("@P_Added_Updated_By", GetAddedUpdatedBy(profile, profile.Id)),
            GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output)			
        ];
        await base.ExecuteNonQueryAsync(mStoredProcedure, mParameters);
    }

    public int SecurityEntitySeqId { get; set; }
}
