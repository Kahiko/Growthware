﻿using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

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
    DataRow IFunction.GetFunction
    {
        get
        {
            checkValid();
            SqlParameter[] mParameters = 
            { 
                new SqlParameter("@P_FunctionSeqId", m_Profile.Id)
            };
            String mStoreProcedure = "ZGWSecurity.Get_Function";
            return base.GetDataRow(mStoreProcedure, mParameters);
        }
    }

    DataSet IFunction.GetFunctions
    {
        get
        {
            DataSet mDSFunctions = null;
            checkValid();
            SqlParameter[] mParameters = [
                new SqlParameter("@P_FunctionSeqId", m_Profile.Id)
            ];
            try
            {
                string mStoredProcedure = "ZGWSecurity.Get_Function";
                DataTable mFunctions = base.GetDataTable(mStoredProcedure, mParameters);
                mDSFunctions = this.getSecurity();
                mDSFunctions.Tables[0].TableName = "DerivedRoles";
                mDSFunctions.Tables[1].TableName = "AssignedRoles";
                mDSFunctions.Tables[2].TableName = "Groups";


                bool mHasAssingedRoles = false;
                bool mHasGroups = false;
                mFunctions.TableName = "Functions";
                if (mDSFunctions.Tables["AssignedRoles"].Rows.Count > 0) mHasAssingedRoles = true;
                if (mDSFunctions.Tables["Groups"].Rows.Count > 0) mHasGroups = true;
                mDSFunctions.Tables.Add(mFunctions);

                DataRelation mRelation = new DataRelation("DerivedRoles", mDSFunctions.Tables["Functions"].Columns["Function_Seq_ID"], mDSFunctions.Tables["DerivedRoles"].Columns["Function_Seq_ID"]);
                mDSFunctions.Relations.Add(mRelation);
                if (mHasAssingedRoles)
                {
                    mRelation = new DataRelation("AssignedRoles", mDSFunctions.Tables["Functions"].Columns["Function_Seq_ID"], mDSFunctions.Tables["AssignedRoles"].Columns["Function_Seq_ID"]);
                    mDSFunctions.Relations.Add(mRelation);
                }
                if (mHasGroups)
                {
                    mRelation = new DataRelation("Groups", mDSFunctions.Tables["Functions"].Columns["Function_Seq_ID"], mDSFunctions.Tables["Groups"].Columns["Function_Seq_ID"]);
                    mDSFunctions.Relations.Add(mRelation);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return mDSFunctions;
        }
    }

    DataTable IFunction.MenuTypes()
    {
        string mStoreProcedure = "ZGWSecurity.Get_Menu_Types";
        SqlParameter[] mParameters = { new SqlParameter("@P_FunctionTypeSeqId", -1) };
        return base.GetDataTable(mStoreProcedure, mParameters);
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

    void IFunction.CopyFunctionSecurity(int source, int target, int added_Updated_By)
    {
        SqlParameter[] mParameters = 
        {
            new SqlParameter("@P_Source", source),
            new SqlParameter("@P_Target", target),
            new SqlParameter("@P_Added_Updated_By", added_Updated_By)
        };
        String mStoreProcedure = "ZGWSecurity.Copy_Function_Security";
        base.ExecuteNonQuery(mStoreProcedure, mParameters);            
    }
    void IFunction.Delete(int functionSeqId)
    {
        SqlParameter[] mParameters = 
        {
            new SqlParameter("@P_FunctionSeqId", functionSeqId),
            GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
        };
        String mStoreProcedure = "ZGWSecurity.Delete_Function";
        base.ExecuteNonQuery(mStoreProcedure, mParameters);
    }

    DataTable IFunction.FunctionTypes()
    {
        string mStoreProcedure = "ZGWSecurity.Get_Function_Types";
        SqlParameter[] mParameters = { new SqlParameter("@P_FunctionTypeSeqId", -1) };
        return base.GetDataTable(mStoreProcedure, mParameters);
    }

    DataTable IFunction.GetMenuOrder(MFunctionProfile Profile)
    {
        string mStoreProcedure = "ZGWSecurity.Get_Function_Sort";
        SqlParameter[] mParameters = { new SqlParameter("@P_FunctionSeqId", Profile.Id) };
        return base.GetDataTable(mStoreProcedure, mParameters);
    }

    int IFunction.Save()
    {
        SqlParameter[] mParameters = [ 
            GetSqlParameter("@P_FunctionSeqId", m_Profile.Id, ParameterDirection.InputOutput), 
            new SqlParameter("@P_Name", m_Profile.Name), 
            new SqlParameter("@P_Description", m_Profile.Description ?? ""), 
            new SqlParameter("@P_FunctionTypeSeqId", m_Profile.FunctionTypeSeqId), 
            new SqlParameter("@P_Source", m_Profile.Source ?? ""), 
            new SqlParameter("@P_Controller", m_Profile.Controller ?? ""), 
            new SqlParameter("@P_Enable_View_State", m_Profile.EnableViewState), 
            new SqlParameter("@P_Enable_Notifications", m_Profile.EnableNotifications), 
            new SqlParameter("@P_Redirect_On_Timeout", m_Profile.RedirectOnTimeout), 
            new SqlParameter("@P_IS_NAV", m_Profile.IsNavigable), 
            new SqlParameter("@P_Link_Behavior", m_Profile.LinkBehavior), 
            new SqlParameter("@P_NO_UI", m_Profile.NoUI), 
            new SqlParameter("@P_NAV_TYPE_ID", m_Profile.NavigationTypeSeqId), 
            new SqlParameter("@P_Action", m_Profile.Action), 
            new SqlParameter("@P_Meta_Key_Words", m_Profile.MetaKeywords ?? ""), 
            new SqlParameter("@P_ParentSeqId", m_Profile.ParentId), 
            new SqlParameter("@P_Notes", m_Profile.Notes ?? ""), 
            new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id))
        ];
        String mStoreProc = "ZGWSecurity.Set_Function";
        base.ExecuteNonQuery(mStoreProc, mParameters);
        return int.Parse(GetParameterValue("@P_FunctionSeqId", mParameters), CultureInfo.InvariantCulture);
    }

    void IFunction.SaveGroups(PermissionType permission)
    {
        checkValid();
        String mCommaSeporatedString = m_Profile.GetCommaSeparatedGroups(permission);
        string mStoreProcedure = "ZGWSecurity.Set_Function_Groups";
        SqlParameter[] mParameters = [
            new SqlParameter("@P_FunctionSeqId", m_Profile.Id), 
            new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqId), 
            new SqlParameter("@P_Groups", mCommaSeporatedString), 
            new SqlParameter("@P_PermissionsNVPDetailSeqId", permission), 
            new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id))
        ];
        base.ExecuteNonQuery(mStoreProcedure, mParameters);
    }

    void IFunction.SaveRoles(PermissionType permission)
    {
        checkValid();
        String mCommaSeporatedString = m_Profile.GetCommaSeparatedAssignedRoles(permission);
        string mStoreProcedure = "ZGWSecurity.Set_Function_Roles";
        SqlParameter[] mParameters = [ 
            new SqlParameter("@P_FunctionSeqId", m_Profile.Id), 
            new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqId), 
            new SqlParameter("@P_Roles", mCommaSeporatedString), 
            new SqlParameter("@P_PermissionsNVPDetailSeqId", permission), 
            new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id))
        ];
        base.ExecuteNonQuery(mStoreProcedure, mParameters);
    }

    void IFunction.UpdateMenuOrder(string commaSeparated_Ids, MFunctionProfile profile)
    {
        string mStoreProcedure = "ZGWSecurity.Set_Function_Sort";
        SqlParameter[] mParameters = [ 
            new SqlParameter("@P_Commaseparated_Ids", commaSeparated_Ids), 
            new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(profile, m_Profile.Id)),
            GetSqlParameter("@P_Primary_Key", "", ParameterDirection.Output)
        ];
        base.ExecuteNonQuery(mStoreProcedure, mParameters);
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

    private DataSet getSecurity()
    {
        string mStoreProcedure = "ZGWSecurity.Get_Function_Security";
        SqlParameter[] mParameters = { new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqId) };
        return base.GetDataSet(mStoreProcedure, mParameters);
    }
}
