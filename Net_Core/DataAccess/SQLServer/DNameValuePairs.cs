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
/// Class DNameValuePairs
/// </summary>
public class DNameValuePairs : AbstractDBInteraction, INameValuePairs
{

#region Member Fields
    private MNameValuePair m_Profile = new MNameValuePair();
    private int m_SecurityEntitySeqId;
    private int m_AccountId;
    private int m_PermissionSeqId = 1;

    private MNameValuePairDetail m_Detail_Profile = new MNameValuePairDetail();
#endregion

#region Constructors
    public DNameValuePairs(string connectionString): base() 
    {
        this.ConnectionString = connectionString;
    }
#endregion

    MNameValuePair INameValuePairs.NameValuePairProfile
    {
        get { return m_Profile; }
        set { m_Profile = value; }
    }

    /// <summary>
    /// Gets or sets the primary key.
    /// </summary>
    /// <value>The primary key.</value>
    public int PrimaryKey { get; set; }

    int INameValuePairs.SecurityEntitySeqId
    {
        get { return m_SecurityEntitySeqId; }
        set { m_SecurityEntitySeqId = value; }
    }

    int INameValuePairs.AccountId
    {
        get { return m_AccountId; }
        set { m_AccountId = value; }
    }

    MNameValuePairDetail INameValuePairs.DetailProfile
    {
        get { return m_Detail_Profile; }
        set { m_Detail_Profile = value; }
    }

    async Task<DataTable> INameValuePairs.GetAllNVP()
    {
        String storeProc = "[ZGWSystem].[Get_Name_Value_Pair]";
        SqlParameter[] mParameters = getSelectParameters();
        return await base.GetDataTableAsync(storeProc, mParameters);
    }

    async Task<DataRow> INameValuePairs.NameValuePair()
    {
        String storeProc = "[ZGWSystem].[Get_Name_Value_Pair]";
        SqlParameter[] mParameters = getSelectParameters();
        return await base.GetDataRowAsync(storeProc, mParameters);
    }

    async Task<DataRow> INameValuePairs.Save()
    {
        String storeProc = "[ZGWSystem].[Set_Name_Value_Pair]";
        SqlParameter[] mParameters = getInsertUpdateParameters();
        return await base.GetDataRowAsync(storeProc, mParameters);
    }

    async Task<DataTable> INameValuePairs.GetRoles(int NameValuePairSeqID)
    {
        SqlParameter[] mParameters = [ new("@P_NVPSeqId", NameValuePairSeqID), new("@P_SecurityEntitySeqId", m_SecurityEntitySeqId) ];
        string myStoreProcedure = "[ZGWSecurity].[Get_Name_Value_Pair_Roles]";
        return await base.GetDataTableAsync(myStoreProcedure, mParameters);
    }

    async Task<DataTable> INameValuePairs.GetGroups(int NameValuePairSeqID)
    {
        SqlParameter[] mParameters = [ new("@P_NVPSeqId", NameValuePairSeqID), new("@P_SecurityEntitySeqId", m_SecurityEntitySeqId) ];
        string myStoreProcedure = "[ZGWSecurity].[Get_Name_Value_Pair_Groups]";
        return await base.GetDataTableAsync(myStoreProcedure, mParameters);
    }

    async Task INameValuePairs.UpdateGroups(int NVP_ID, int SecurityEntityID, string CommaSeparatedGroups, MNameValuePair nvpProfile)
    {
        string myStoreProcedure = "[ZGWSecurity].[Set_Name_Value_Pair_Groups]";
        SqlParameter[] mParameters = [
            new("@P_NVPSeqId", NVP_ID), 
            new("@P_SecurityEntitySeqId", SecurityEntityID), 
            new("@P_Groups", CommaSeparatedGroups), 
            new("@P_PermissionsNVPDetailSeqId", m_PermissionSeqId), 
            new("@P_Added_Updated_By", GetAddedUpdatedBy(nvpProfile, nvpProfile.Id)) 
        ];
        await base.ExecuteNonQueryAsync(myStoreProcedure, mParameters);
    }

    async Task INameValuePairs.UpdateRoles(int NVP_ID, int SecurityEntityID, string CommaSeparatedRoles, MNameValuePair nvpProfile)
    {
        string myStoreProcdure = "[ZGWSecurity].[Set_Name_Value_Pair_Roles]";
        SqlParameter[] mParameters = [
            new("@P_NVPSeqId", NVP_ID), 
            new("@P_SecurityEntitySeqId", SecurityEntityID), 
            new("@P_Role", CommaSeparatedRoles), 
            new("@P_PermissionsNVPDetailSeqId", m_PermissionSeqId), 
            new("@P_Added_Updated_By", GetAddedUpdatedBy(nvpProfile, nvpProfile.Id)) 
        ];
        await base.ExecuteNonQueryAsync(myStoreProcdure, mParameters);
    }

    async Task<DataRow> INameValuePairs.NameValuePairDetail()
    {
        string myStoreProcedure = "[ZGWSystem].[Get_Name_Value_Pair_Detail]";
        SqlParameter[] mParameters = [
            new("@P_NVPSeqId", m_Detail_Profile.NameValuePairSeqId),
            new("@P_NVP_DetailSeqId", m_Detail_Profile.Id)
        ];
        return await base.GetDataRowAsync(myStoreProcedure, mParameters);
    }

    async Task<DataRow> INameValuePairs.NameValuePairDetails(int NVPSeqDetID, int NVPSeqID)
    {
        string myStoreProcedure = "[ZGWSystem].[Get_Name_Value_Pair_Details]";
        SqlParameter[] mParameters = { new("@P_NVPSeqId", NVPSeqID) };
        return await base.GetDataRowAsync(myStoreProcedure, mParameters);
    }

    async Task<bool> INameValuePairs.DeleteNVPDetail(MNameValuePairDetail profile)
    {
        string myStoreProcedure = "[ZGWSystem].[Delete_Name_Value_Pair_Detail]";
        Boolean mRetVal = false;
        SqlParameter[] mParameters = { new("@P_NVPSeqId", profile.NameValuePairSeqId), new("@P_NVP_DetailSeqId", profile.Id) };
        try
        {
            await base.ExecuteNonQueryAsync(myStoreProcedure, mParameters);
            mRetVal = true;
        }
        catch (Exception)
        {
            throw;
        }
        return mRetVal;
    }

    async Task<DataTable> INameValuePairs.AllNameValuePairDetail()
    {
        string myStoreProcedure = "[ZGWSystem].[Get_Name_Value_Pair_Details]";
        SqlParameter[] mParameters = { new("@P_NVPSeqId", -1) };
        return await base.GetDataTableAsync(myStoreProcedure, mParameters);
    }

    async Task<DataTable> INameValuePairs.GetAllNVPDetail(int NVPSeqID)
    {
        string myStoreProcedure = "[ZGWSystem].[Get_Name_Value_Pair_Details]";
        SqlParameter[] mParameters = [ new("@P_NVPSeqId", NVPSeqID) ];
        return await base.GetDataTableAsync(myStoreProcedure, mParameters);
    }

    async Task<DataRow> INameValuePairs.SaveNVPDetail(MNameValuePairDetail profile)
    {
        string myStoreProcedure = "[ZGWSystem].[Set_Name_Value_Pair_Detail]";
        SqlParameter[] mParameters = [ 
              new("@P_NVP_DetailSeqId", profile.Id)
            , new("@P_NVPSeqId", profile.NameValuePairSeqId)
            , new("@P_NVP_Detail_Name", profile.Text)
            , new("@P_NVP_Detail_Value", profile.Value)
            , new("@P_StatusSeqId", profile.Status)
            , new("@P_Sort_Order", profile.SortOrder)
            , new("@P_Added_Updated_BY", GetAddedUpdatedBy(profile, profile.Id))
            , GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output) ];
        DataRow mRetVal = await base.GetDataRowAsync(myStoreProcedure, mParameters);
        return mRetVal;
    }

    private SqlParameter[] getInsertUpdateParameters()
    {
        SqlParameter[] mParameters = [ 
            new("@P_NVPSeqId", m_Profile.Id), 
            new("@P_Schema_Name", m_Profile.SchemaName), 
            new("@P_Static_Name", m_Profile.StaticName), 
            new("@P_Display", m_Profile.Display), 
            new("@P_Description", m_Profile.Description), 
            new("@P_StatusSeqId", m_Profile.Status), 
            new("@P_Added_Updated_BY", GetAddedUpdatedBy(m_Profile, m_Profile.Id)), 
            GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output) 
        ];
        return mParameters;
    }

    private SqlParameter[] getSelectParameters()
    {
        SqlParameter[] mParameters = { new("@P_NVPSeqId", m_Profile.Id), new("@P_AccountSeqId", m_AccountId), new("@P_SecurityEntitySeqId", m_SecurityEntitySeqId) };
        return mParameters;
    }
}
