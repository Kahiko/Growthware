using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.SQLServer;

/// <summary>
/// Class DGroups
/// </summary>
public class DGroups : AbstractDBInteraction, IGroups
{

#region Public Properties
    /// <summary>
    /// GroupProfile
    /// </summary>
    /// <value>The profile.</value>
    public MGroupProfile Profile { get; set; }
    /// <summary>
    /// GroupRoles
    /// </summary>
    /// <value>The group roles profile.</value>
    public MGroupRoles GroupRolesProfile { get; set; }

    /// <summary>
    /// Sets or gets the SecurityEntitySeqID
    /// </summary>
    /// <value>The security entity seq ID.</value>
    public int SecurityEntitySeqId { get; set; }
#endregion

#region Constructors
    public DGroups(string connectionString, int securityEntitySeqId) : base() 
    { 
        this.ConnectionString = connectionString;
        this.SecurityEntitySeqId = securityEntitySeqId;
    }
#endregion

    /// <summary>
    /// Deletes a group in a given Security Entity
    /// </summary>
    /// <returns>bool</returns>
    public async Task<bool> DeleteGroup()
    {
        SqlParameter[] mParameters = [
            new SqlParameter("@P_SecurityEntitySeqId", Profile.SecurityEntityID), 
            new SqlParameter("@P_GroupSeqId", Profile.Id) 
        ];
        String mStoreProc = "ZGWSecurity.Delete_Group";
        await base.ExecuteNonQueryAsync( mStoreProc, mParameters);
        return true;
    }

    /// <summary>
    /// Get's all of the groups for a given Security Entity
    /// </summary>
    /// <returns>DataTable</returns>
    public async Task<DataTable> GroupsBySecurityEntity()
    {
        SqlParameter[] mParameters = [ 
            new SqlParameter("@P_SecurityEntitySeqId", Profile.SecurityEntityID), 
            new SqlParameter("@P_GroupSeqId", -1) 
        ];
        String mStoreProc = "ZGWSecurity.Get_Group";
        return await base.GetDataTableAsync( mStoreProc, mParameters);
    }

    /// <summary>
    /// Returns a data row necessary to populate MGroup
    /// </summary>
    /// <returns>DataRow</returns>
    public async Task<DataRow> ProfileData()
    {
        SqlParameter[] mParameters = [
            new SqlParameter("@P_SecurityEntitySeqId", Profile.SecurityEntityID), 
            new SqlParameter("@P_GroupSeqId", Profile.Id) 
        ];
        String mStoreProc = "ZGWSecurity.Get_Group";
        return await base.GetDataRowAsync( mStoreProc, mParameters);
    }

    /// <summary>
    /// Updates a group effects all Security Entities
    /// </summary>
    /// <returns>bool</returns>
    public async Task<int> Save()
    {
        SqlParameter[] mParameters = getInsertUpdateParameters();
        String mStoreProc = "ZGWSecurity.Set_Group";
        await base.ExecuteNonQueryAsync( mStoreProc, mParameters);
        int mRetVal = int.Parse(GetParameterValue("@P_PRIMARY_KEY", mParameters));
        return mRetVal;
    }

    /// <summary>
    /// Returns a DataTable of Group roles
    /// </summary>
    /// <returns>DataTable</returns>
    /// <exception cref="System.ApplicationException"></exception>
    public async Task<DataTable> GroupRoles()
    {
        if (GroupRolesProfile.GroupSeqId == -1)
        {
            throw new ArgumentException("The GroupRoles Profile must be set.");
        }
        string mymStoreProcedure = "ZGWSecurity.Get_Group_Roles";
        SqlParameter[] mParameters = [ 
            new SqlParameter("@P_SecurityEntitySeqId", GroupRolesProfile.SecurityEntityID), 
            new SqlParameter("@P_GroupSeqId", GroupRolesProfile.GroupSeqId) 
        ];
        return await base.GetDataTableAsync(mymStoreProcedure, mParameters);
    }

    /// <summary>
    /// Updates the Groups roles
    /// </summary>
    /// <returns>bool</returns>
    /// <exception cref="System.ApplicationException"></exception>
    public async Task<bool> UpdateGroupRoles()
    {
        if (GroupRolesProfile.GroupSeqId == -1)
        {
            throw new ArgumentException("The GroupRoles Profile must be set.");
        }
        string mymStoreProcedure = "ZGWSecurity.Set_Group_Roles";
        SqlParameter[] mParameters = [
            new SqlParameter("@P_GroupSeqId", GroupRolesProfile.GroupSeqId), 
            new SqlParameter("@P_SecurityEntitySeqId", GroupRolesProfile.SecurityEntityID), 
            new SqlParameter("@P_Roles", GroupRolesProfile.Roles??""), 
            new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(GroupRolesProfile, GroupRolesProfile.GroupSeqId)) 
        ];
        await base.ExecuteNonQueryAsync(mymStoreProcedure, mParameters);
        return true;
    }

    private SqlParameter[] getInsertUpdateParameters()
    {
        SqlParameter[] mParameters = [
            new SqlParameter("@P_GroupSeqId", Profile.Id), 
            new SqlParameter("@P_Name", Profile.Name), 
            new SqlParameter("@P_Description", Profile.Description), 
            new SqlParameter("@P_SecurityEntitySeqId", Profile.SecurityEntityID), 
            new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(Profile, Profile.Id)), 
            GetSqlParameter("@P_PRIMARY_KEY", Profile.Id, ParameterDirection.Output) 
        ];
        return mParameters;
    }
}
