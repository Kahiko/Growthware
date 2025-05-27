using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.SQLServer;

/// <summary>
/// DFunctions provides all database interaction to SQL Server 2008
/// </summary>
/// <remarks>
///		The Profile and SecurityEntitySeqID properties must be set
///		before using any methods.
///		Properties where chosen instead of parameters because all
///		methods will need one or both to perform their work.
///	</remarks>
public class DFunctions : AbstractDBInteraction, IFunction
{

    #region Member Fields
    private MFunctionProfile m_Profile = null;
    private int m_SecurityEntitySeqId = -2;
    #endregion

    #region Public Properties
    async Task<DataRow> IFunction.GetFunction()
    {
        checkValid();
        SqlParameter[] mParameters = [
            new("@P_FunctionSeqId", m_Profile.Id)
        ];
        String mStoreProcedure = "[ZGWSecurity].[Get_Function]";
        return await base.GetDataRowAsync(mStoreProcedure, mParameters);
    }

    async Task<DataSet> IFunction.GetFunctions(int securityEntitySeqId)
    {
        DataSet mDSFunctions = null;
        checkValid();
        SqlParameter[] mParameters = [
              new("@P_FunctionSeqId", m_Profile.Id)
            , new("@P_SecurityEntitySeqId", securityEntitySeqId)
        ];
        try
        {
            string mStoredProcedure = "[ZGWSecurity].[Get_Function]";
            mDSFunctions = await this.GetDataSetAsync(mStoredProcedure, mParameters);
            mDSFunctions.Tables[(int)FunctionSecurityTables.DerivedRoles].TableName = FunctionSecurityTableNames.DERIVED_ROLES;
            mDSFunctions.Tables[(int)FunctionSecurityTables.AssignedRoles].TableName = FunctionSecurityTableNames.ASSIGNED_ROLES;
            mDSFunctions.Tables[(int)FunctionSecurityTables.AssignedGroups].TableName = FunctionSecurityTableNames.ASSIGNED_GROUPS;
            mDSFunctions.Tables[(int)FunctionSecurityTables.Functions].TableName = FunctionSecurityTableNames.FUNCTIONS;


            bool mHasAssingedRoles = false;
            bool mHasGroups = false;
            if (mDSFunctions.Tables[FunctionSecurityTableNames.ASSIGNED_ROLES].Rows.Count > 0) mHasAssingedRoles = true;
            if (mDSFunctions.Tables[FunctionSecurityTableNames.ASSIGNED_GROUPS].Rows.Count > 0) mHasGroups = true;

            DataRelation mRelation = new DataRelation(FunctionSecurityTableNames.DERIVED_ROLES, mDSFunctions.Tables[FunctionSecurityTableNames.FUNCTIONS].Columns["Function_Seq_ID"], mDSFunctions.Tables[FunctionSecurityTableNames.DERIVED_ROLES].Columns["Function_Seq_ID"]);
            mDSFunctions.Relations.Add(mRelation);
            if (mHasAssingedRoles)
            {
                mRelation = new DataRelation(FunctionSecurityTableNames.ASSIGNED_ROLES, mDSFunctions.Tables[FunctionSecurityTableNames.FUNCTIONS].Columns["Function_Seq_ID"], mDSFunctions.Tables[FunctionSecurityTableNames.ASSIGNED_GROUPS].Columns["Function_Seq_ID"]);
                mDSFunctions.Relations.Add(mRelation);
            }
            if (mHasGroups)
            {
                mRelation = new DataRelation(FunctionSecurityTableNames.ASSIGNED_GROUPS, mDSFunctions.Tables[FunctionSecurityTableNames.FUNCTIONS].Columns["Function_Seq_ID"], mDSFunctions.Tables[FunctionSecurityTableNames.ASSIGNED_GROUPS].Columns["Function_Seq_ID"]);
                mDSFunctions.Relations.Add(mRelation);
            }

        }
        catch (Exception)
        {

            throw;
        }
        return mDSFunctions;
    }

    async Task<DataTable> IFunction.MenuTypes()
    {
        string mStoreProcedure = "[ZGWSecurity].[Get_Menu_Types]";
        SqlParameter[] mParameters = [new("@P_FunctionTypeSeqId", -1)];
        return await base.GetDataTableAsync(mStoreProcedure, mParameters);
    }

    MFunctionProfile IFunction.Profile
    {
        get
        {
            return m_Profile;
        }
        set
        {
            m_Profile = value;
        }
    }

    int IFunction.SecurityEntitySeqId
    {
        get
        {
            return m_SecurityEntitySeqId;
        }
        set
        {
            m_SecurityEntitySeqId = value;
        }
    }
    #endregion

    #region Constructors
    public DFunctions(string connectionString, int securityEntitySeqId) : base()
    {
        this.ConnectionString = connectionString;
        this.m_SecurityEntitySeqId = securityEntitySeqId;
    }
    #endregion

    async Task IFunction.CopyFunctionSecurity(int source, int target, int added_Updated_By)
    {
        SqlParameter[] mParameters = [
            new("@P_Source", source),
            new("@P_Target", target),
            new("@P_Added_Updated_By", added_Updated_By)
        ];
        String mStoreProcedure = "[ZGWSecurity].[Copy_Function_Security]";
        await base.ExecuteNonQueryAsync(mStoreProcedure, mParameters);
    }
    async Task IFunction.Delete(int functionSeqId)
    {
        SqlParameter[] mParameters = [
            new("@P_FunctionSeqId", functionSeqId),
            GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
        ];
        String mStoreProcedure = "[ZGWSecurity].[Delete_Function]";
        await base.ExecuteNonQueryAsync(mStoreProcedure, mParameters);
    }

    async Task<DataTable> IFunction.FunctionTypes()
    {
        string mStoreProcedure = "[ZGWSecurity].[Get_Function_Types]";
        SqlParameter[] mParameters = [new("@P_FunctionTypeSeqId", -1)];
        return await base.GetDataTableAsync(mStoreProcedure, mParameters);
    }

    async Task<DataTable> IFunction.GetMenuOrder(MFunctionProfile Profile)
    {
        string mStoreProcedure = "[ZGWSecurity].[Get_Function_Sort]";
        SqlParameter[] mParameters = [new("@P_FunctionSeqId", Profile.Id)];
        return await base.GetDataTableAsync(mStoreProcedure, mParameters);
    }

    async Task<int> IFunction.Save()
    {
        SqlParameter[] mParameters = [
            GetSqlParameter("@P_FunctionSeqId", m_Profile.Id, ParameterDirection.InputOutput),
            new("@P_Name", m_Profile.Name),
            new("@P_Description", m_Profile.Description ?? ""),
            new("@P_FunctionTypeSeqId", m_Profile.FunctionTypeSeqId),
            new("@P_Source", m_Profile.Source ?? ""),
            new("@P_Controller", m_Profile.Controller ?? ""),
            new("@P_Enable_View_State", m_Profile.EnableViewState),
            new("@P_Enable_Notifications", m_Profile.EnableNotifications),
            new("@P_Redirect_On_Timeout", m_Profile.RedirectOnTimeout),
            new("@P_IS_NAV", m_Profile.IsNavigable),
            new("@P_Link_Behavior", m_Profile.LinkBehavior),
            new("@P_NO_UI", m_Profile.NoUI),
            new("@P_NAV_TYPE_ID", m_Profile.NavigationTypeSeqId),
            new("@P_Action", m_Profile.Action),
            new("@P_Meta_Key_Words", m_Profile.MetaKeywords ?? ""),
            new("@P_ParentSeqId", m_Profile.ParentId),
            new("@P_Notes", m_Profile.Notes ?? ""),
            new("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id))
        ];
        String mStoreProc = "[ZGWSecurity].[Set_Function]";
        await base.ExecuteNonQueryAsync(mStoreProc, mParameters);
        return int.Parse(GetParameterValue("@P_FunctionSeqId", mParameters), CultureInfo.InvariantCulture);
    }

    async Task IFunction.SaveGroups(PermissionType permission, int securityEntitySeqId)
    {
        checkValid();
        String mCommaSeporatedString = m_Profile.GetCommaSeparatedGroups(permission);
        string mStoreProcedure = "[ZGWSecurity].[Set_Function_Groups]";
        SqlParameter[] mParameters = [
            new("@P_FunctionSeqId", m_Profile.Id),
            new("@P_SecurityEntitySeqId", securityEntitySeqId),
            new("@P_Groups", mCommaSeporatedString),
            new("@P_PermissionsNVPDetailSeqId", permission),
            new("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id))
        ];
        await base.ExecuteNonQueryAsync(mStoreProcedure, mParameters);
    }

    async Task IFunction.SaveRoles(PermissionType permission, int securityEntitySeqId)
    {
        checkValid();
        String mCommaSeporatedString = m_Profile.GetCommaSeparatedAssignedRoles(permission);
        string mStoreProcedure = "[ZGWSecurity].[Set_Function_Roles]";
        SqlParameter[] mParameters = [
            new("@P_FunctionSeqId", m_Profile.Id),
            new("@P_SecurityEntitySeqId", securityEntitySeqId),
            new("@P_Roles", mCommaSeporatedString),
            new("@P_PermissionsNVPDetailSeqId", permission),
            new("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id))
        ];
        await base.ExecuteNonQueryAsync(mStoreProcedure, mParameters);
    }

    async Task IFunction.UpdateMenuOrder(string commaSeparated_Ids, MFunctionProfile profile)
    {
        string mStoreProcedure = "[ZGWSecurity].[Set_Function_Sort]";
        SqlParameter[] mParameters = [
            new("@P_Commaseparated_Ids", commaSeparated_Ids),
            new("@P_Added_Updated_By", GetAddedUpdatedBy(profile, m_Profile.Id)),
            GetSqlParameter("@P_Primary_Key", "", ParameterDirection.Output)
        ];
        await base.ExecuteNonQueryAsync(mStoreProcedure, mParameters);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "SecurityEntitySeqId")]
    private void checkValid()
    {
        base.IsValid();
        if (m_Profile == null)
        {
            throw new InvalidOperationException("The Profile property must set before using any methods from this class.");
        }
        if (m_SecurityEntitySeqId == 0)
        {
            throw new InvalidOperationException("The SecurityEntitySeqId property must set before using any methods from this class.");
        }
    }

    private async Task<DataSet> getSecurity()
    {
        string mStoreProcedure = "[ZGWSecurity].[Get_Function_Security]";
        SqlParameter[] mParameters = [new("@P_SecurityEntitySeqId", m_SecurityEntitySeqId)];
        return await base.GetDataSetAsync(mStoreProcedure, mParameters);
    }
}
