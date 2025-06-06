﻿using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.BusinessLogic;

/// <summary>
/// Process business logic for accounts
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
/// Dim myBll as new BRoles(mySecurityEntityProfile)
/// ]]>
/// </code>
/// </example>
public class BRoles : AbstractBusinessLogic
{

    #region Member Fields
    private IRoles m_DRoles;
    #endregion

    #region Constructors
    /// <summary>
    /// Private BRoles() to ensure only new instances with passed parameters is used.
    /// </summary>
    /// <remarks></remarks>
    private BRoles()
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
    /// BRoles mBRoles = BRoles = New BRoles(MSecurityEntity);
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
    /// Dim mBRoles As BRoles = New BRoles(MSecurityEntity)
    /// ]]>
    /// </code>
    /// </example>
    public BRoles(MSecurityEntity securityEntityProfile)
    {
        if (securityEntityProfile == null) throw new ArgumentNullException(nameof(securityEntityProfile), "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!");
        if (m_DRoles == null)
        {
            this.m_DRoles = (IRoles)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DRoles", securityEntityProfile.ConnectionString, securityEntityProfile.Id);
            if (this.m_DRoles == null)
            {
                throw new InvalidOperationException("Failed to create an instance of DRoles.");
            }
        }
    }
    #endregion

    /// <summary>
    /// Saves the specified profile.
    /// </summary>
    /// <param name="profile">The profile.</param>
    public async Task<int> Save(MRole profile)
    {
        int mRetVal = -1;
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        m_DRoles.Profile = profile;
        if (DatabaseIsOnline()) mRetVal = await m_DRoles.Save(profile);
        return mRetVal;
    }

    /// <summary>
    /// Deletes the role.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
    public async Task DeleteRole(MRole profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        m_DRoles.Profile = profile;
        if (DatabaseIsOnline()) await m_DRoles.DeleteRole(profile.Name, profile.SecurityEntityID);
    }

    /// <summary>
    /// Gets the profile.
    /// </summary>
    /// <param name="profile">The profile.</param>
    public async Task<MRole> GetProfile(MRole profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        MRole mRetVal = new();
        if (DatabaseIsOnline()) mRetVal = new(await m_DRoles.ProfileData(profile.Id));
        return mRetVal;
    }

    /// <summary>
    /// Gets the roles by BU.
    /// </summary>
    /// <param name="securityEntitySeqId">The security entity ID.</param>
    /// <returns>DataTable.</returns>
    public async Task<DataTable> GetRolesBySecurityEntity(int securityEntitySeqId)
    {
        return await m_DRoles.RolesBySecurityEntity(securityEntitySeqId);
    }

    /// <summary>
    /// Gets the accounts in role.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <returns>DataTable.</returns>
    public async Task<DataTable> GetAccountsInRole(MRole profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        DataTable mRetVal = null;
        m_DRoles.Profile = profile;
        if (DatabaseIsOnline()) mRetVal = await m_DRoles.AccountsInRole();
        return mRetVal;
    }

    /// <summary>
    /// Gets the accounts not in role.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <returns>DataTable.</returns>
    public async Task<DataTable> GetAccountsNotInRole(MRole profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        DataTable mRetVal = null;
        m_DRoles.Profile = profile;
        if (DatabaseIsOnline()) mRetVal = await m_DRoles.AccountsNotInRole();
        return mRetVal;
    }

    /// <summary>
    /// Updates all accounts for role.
    /// </summary>
    /// <param name="roleSeqId">The role seq ID.</param>
    /// <param name="SecurityEntityID">The security entity ID.</param>
    /// <param name="accounts">The accounts.</param>
    /// <param name="accountSeqId">The account seq ID.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
    public async Task<bool> UpdateAllAccountsForRole(int roleSeqId, int SecurityEntityID, string[] accounts, int accountSeqId)
    {
        if (accounts == null) throw new ArgumentNullException(nameof(accounts), "accounts cannot be a null reference (Nothing in Visual Basic)!!");
        bool mRetVal = false;
        if (DatabaseIsOnline()) mRetVal = await m_DRoles.UpdateAllAccountsForRole(roleSeqId, SecurityEntityID, accounts, accountSeqId);
        return mRetVal;
    }
}
