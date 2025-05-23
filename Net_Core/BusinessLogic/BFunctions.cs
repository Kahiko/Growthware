using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.BusinessLogic;

/// <summary>
/// Process business logic for functions
/// </summary>
/// <remarks>
/// <![CDATA[
/// MSecurityEntity can be found in the GrowthWare.Framework.Model.Profiles namespace.  
/// 
/// The following properties are necessary for correct business logic operation.
/// .ConnectionString
/// .DALName
/// .DALNameSpace
/// ]]>
/// </remarks>
/// <example> This sample shows how to create an instance of the class.
/// <code language="VB.NET">
/// <![CDATA[
/// Dim myBll as new BFunctions(MSecurityEntity, ConfigSettings.CentralManagement)
/// ]]>
/// </code>
/// </example>
public class BFunctions : AbstractBusinessLogic
{

    #region Member Fields
    private IFunction m_DFunctions;
    #endregion

    #region Constructors
    /// <summary>
    /// Private BFunctions() to ensure only new instances with passed parameters is used.
    /// </summary>
    /// <remarks></remarks>
    private BFunctions()
    {
    }

    /// <summary>
    /// Parameters are need to pass along to the factory for correct connection to the desired datastore.
    /// </summary>
    /// <param name="securityEntityProfile">The Security Entity profile used to obtain the DAL name, DAL name space, and the Connection String</param>
    /// <param name="centralManagement">Boolean value indicating if the system is being used to manage multiple database instances.</param>
    /// <remarks></remarks>
    /// <example> This sample shows how to create an instance of the class.
    /// <code language="C#">
    /// <![CDATA[
    /// MSecurityEntity MSecurityEntity = MSecurityEntity = New MSecurityEntity();
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID;
    /// MSecurityEntity.DAL = ConfigSettings.DAL;
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
    /// 
    /// BFunctions mBFunction = New BFunctions(MSecurityEntity);
    /// ]]>
    /// </code>
    /// <code language="VB.NET">
    /// <![CDATA[
    /// Dim MSecurityEntity As MSecurityEntity = New MSecurityEntity()
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID
    /// MSecurityEntity.DAL = ConfigSettings.DAL
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL)
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL)
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString
    /// 
    /// Dim mBFunction As BFunctions = New BFunctions(MSecurityEntity)
    /// ]]>
    /// </code>
    /// </example>
    public BFunctions(MSecurityEntity securityEntityProfile)
    {
        if (securityEntityProfile == null) throw new ArgumentNullException(nameof(securityEntityProfile), "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!");
        if (m_DFunctions == null)
        {
            this.m_DFunctions = (IFunction)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DFunctions", securityEntityProfile.ConnectionString, securityEntityProfile.Id);
            if (this.m_DFunctions == null)
            {
                throw new InvalidOperationException("Failed to create an instance of DFunctions.");
            }
        }
    }
    #endregion

    /// <summary>
    /// Gets the function types.
    /// </summary>
    /// <returns>DataTable.</returns>
    public async Task<Collection<MFunctionTypeProfile>> FunctionTypes()
    {
        Collection<MFunctionTypeProfile> mRetVal = [];
        DataTable mDTFunctionTypes = await m_DFunctions.FunctionTypes();
        foreach (DataRow item in mDTFunctionTypes.Rows)
        {
            mRetVal.Add(new MFunctionTypeProfile(item));
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns a collection of MFunctionProfile objects for the given
    /// security entity.
    /// </summary>
    /// <param name="securityEntitySeqId">Integer</param>
    /// <returns>Collection(of MFunctionProfile)</returns>
    /// <remarks></remarks>
    public async Task<Collection<MFunctionProfile>> GetFunctions(int securityEntitySeqId)
    {
        Collection<MFunctionProfile> mRetVal = new Collection<MFunctionProfile>();
        DataSet mDSFunctions = null;
        if (DatabaseIsOnline())
        {
            try
            {
                m_DFunctions.Profile = new MFunctionProfile();
                m_DFunctions.SecurityEntitySeqId = securityEntitySeqId;
                mDSFunctions = await m_DFunctions.GetFunctions();
                bool mHasAssignedRoles = false;
                bool mHasGroups = false;
                if (mDSFunctions.Tables[1].Rows.Count > 0) mHasAssignedRoles = true;
                if (mDSFunctions.Tables[2].Rows.Count > 0) mHasGroups = true;
                DataRow[] mGroups = null;
                DataRow[] mAssignedRoles = null;
                DataRow[] mDerivedRoles = null;

                foreach (DataRow item in mDSFunctions.Tables["Functions"].Rows)
                {
                    mDerivedRoles = item.GetChildRows("DerivedRoles");
                    mAssignedRoles = null;
                    if (mHasAssignedRoles) mAssignedRoles = item.GetChildRows("AssignedRoles");
                    mGroups = null;
                    if (mHasGroups) mGroups = item.GetChildRows("Groups");
                    MFunctionProfile mProfile = new MFunctionProfile(item, mDerivedRoles, mAssignedRoles, mGroups);
                    mRetVal.Add(mProfile);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        return mRetVal;
    }

    /// <summary>
    /// Gets the menu order.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <returns>DataTable.</returns>
    public async Task<DataTable> GetMenuOrder(MFunctionProfile profile)
    {
        DataTable mRetVal = null;
        if (DatabaseIsOnline()) mRetVal = await m_DFunctions.GetMenuOrder(profile);
        return mRetVal;
    }

    /// <summary>
    /// Saves the specified profile.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <param name="saveGroups">if set to <c>true</c> [save groups].</param>
    /// <param name="saveRoles">if set to <c>true</c> [save roles].</param>
    /// <returns>System.Int32.</returns>
    public async Task<int> Save(MFunctionProfile profile, bool saveGroups, bool saveRoles, int securityEntitySeqId)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        if (DatabaseIsOnline())
        {
            m_DFunctions.Profile = profile;
            profile.Id = await m_DFunctions.Save();
            m_DFunctions.Profile = profile;
            if (saveGroups)
            {
                await m_DFunctions.SaveGroups(PermissionType.Add, securityEntitySeqId);
                await m_DFunctions.SaveGroups(PermissionType.Delete, securityEntitySeqId);
                await m_DFunctions.SaveGroups(PermissionType.Edit, securityEntitySeqId);
                await m_DFunctions.SaveGroups(PermissionType.View, securityEntitySeqId);
            }
            if (saveRoles)
            {
                await m_DFunctions.SaveRoles(PermissionType.Add, securityEntitySeqId);
                await m_DFunctions.SaveRoles(PermissionType.Delete, securityEntitySeqId);
                await m_DFunctions.SaveRoles(PermissionType.Edit, securityEntitySeqId);
                await m_DFunctions.SaveRoles(PermissionType.View, securityEntitySeqId);
            }
        }
        return profile.Id;

    }

    public async Task CopyFunctionSecurity(int source, int target, int added_Updated_By)
    {
        if (DatabaseIsOnline()) await m_DFunctions.CopyFunctionSecurity(source, target, added_Updated_By);
    }

    /// <summary>
    /// Deletes the specified function seq id.
    /// </summary>
    /// <param name="functionSeqId">The function seq id.</param>
    public async Task Delete(int functionSeqId)
    {
        if (DatabaseIsOnline()) await m_DFunctions.Delete(functionSeqId);
    }

    public async Task<DataTable> MenuTypes()
    {
        return await m_DFunctions.MenuTypes();
    }

    /// <summary>
    /// Updates the sort order fot the given functions in the commaSeparated_Ids.
    /// </summary>
    /// <param name="commaSeparated_Ids">A comma separated list of ids</param>
    /// <param name="profile">The profile.</param>
    public async Task UpdateMenuOrder(string commaseparated_Ids, MFunctionProfile profile)
    {
        if (DatabaseIsOnline()) await m_DFunctions.UpdateMenuOrder(commaseparated_Ids, profile);
    }
}
