using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System;
using System.Collections;
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
/// Dim myBll as new BGroups(mySecurityEntityProfile)
/// ]]>
/// </code>
/// </example>
public class BGroups : AbstractBusinessLogic
{

    #region Member Fields
    private IGroups m_DGroups;
    #endregion

    #region Constructors
    /// <summary>
    /// Private BGroups() to ensure only new instances with passed parameters is used.
    /// </summary>
    /// <remarks></remarks>
    private BGroups()
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
    /// MSecurityEntity.Id = ConfigSettings.DefaultSecurityEntityID;
    /// MSecurityEntity.DAL = ConfigSettings.DAL;
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
    /// 
    /// BGroupss mBGroups = BGroupss = New BGroupss(MSecurityEntity);
    /// ]]>
    /// </code>
    /// <code language="VB.NET">
    /// <![CDATA[
    /// Dim MSecurityEntity As MSecurityEntity = New MSecurityEntity()
    /// MSecurityEntity.Id = ConfigSettings.DefaultSecurityEntityID
    /// MSecurityEntity.DAL = ConfigSettings.DAL
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL)
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL)
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString
    /// 
    /// Dim mBGroups As BGroupss = New BGroupss(MSecurityEntity)
    /// ]]>
    /// </code>
    /// </example>
    public BGroups(MSecurityEntity securityEntityProfile)
    {
        if (securityEntityProfile == null) throw new ArgumentNullException(nameof(securityEntityProfile), "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!");
        if (m_DGroups == null)
        {
            this.m_DGroups = (IGroups)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DGroups", securityEntityProfile.ConnectionString, securityEntityProfile.Id);
            if (this.m_DGroups == null)
            {
                throw new InvalidOperationException("Failed to create an instance of DGroups.");
            }
        }
    }
    #endregion

    /// <summary>
    /// Gets the groups by security entity.
    /// </summary>
    /// <param name="securityEntityId">The security entity ID.</param>
    /// <returns>DataTable.</returns>
    public async Task<DataTable> GetGroupsBySecurityEntity(int securityEntityId)
    {
        MGroupProfile myProfile = new();
        m_DGroups.Profile = myProfile;
        return await m_DGroups.GroupsBySecurityEntity();
    }

    /// <summary>
    /// Gets the profile.
    /// </summary>
    /// <param name="groupId">The group ID.</param>
    /// <returns>MGroupProfile.</returns>
    public async Task<MGroupProfile> GetProfile(int groupId)
    {
        MGroupProfile retProfile = new MGroupProfile();
        retProfile.Id = groupId;
        m_DGroups.Profile = retProfile;
        retProfile = new MGroupProfile(await m_DGroups.ProfileData());
        return retProfile;
    }

    /// <summary>
    /// Deletes the group.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
    public async Task<bool> DeleteGroup(MGroupProfile profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        bool retVal = false;
        m_DGroups.Profile = profile;
        if (DatabaseIsOnline()) retVal = await m_DGroups.DeleteGroup();
        return retVal;
    }

    /// <summary>
    /// Updates the group.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
    public async Task<int> Save(MGroupProfile profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        int mRetVal = 0;
        m_DGroups.Profile = profile;
        if (DatabaseIsOnline())
        {
            mRetVal = await m_DGroups.Save();
        }
        return mRetVal;
    }

    /// <summary>
    /// Gets the selected roles.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <returns>System.String[][].</returns>
    public async Task<string[]> GetSelectedRoles(MGroupRoles profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        ArrayList mRolesArray = new ArrayList();
        m_DGroups.GroupRolesProfile = profile;
        DataTable myDataTable = null;
        if (DatabaseIsOnline())
        {
            try
            {
                myDataTable = await m_DGroups.GroupRoles();
                foreach (DataRow item in myDataTable.Rows)
                {
                    mRolesArray.Add(item["Role"].ToString());
                }
            }
            catch (BusinessLogicLayerException)
            {
                throw;
            }
        }
        return (string[])mRolesArray.ToArray(typeof(string));
    }

    /// <summary>
    /// Updates the group roles.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
    public async Task<bool> UpdateGroupRoles(MGroupRoles profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        bool mRetVal = false;
        m_DGroups.GroupRolesProfile = profile;
        if (DatabaseIsOnline()) mRetVal = await m_DGroups.UpdateGroupRoles();
        return mRetVal;
    }
}
