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
/// BDBInformation is the business implementation for the DB information.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "BDB")]
public class BDBInformation : AbstractBusinessLogic
{

    #region Member Fields
    private IDBInformation m_DDBInformation;
    #endregion

    #region Constructors
    /// <summary>
    /// Private BDBInformation() to ensure only new instances with passed parameters is used.
    /// </summary>
    /// <remarks></remarks>
    private BDBInformation() { }

    /// <summary>
    /// Parameters are need to pass along to the factory for correct connection to the desired data store.
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
    /// BDBInformation mBAccount = BDBInformation = New BDBInformation(MSecurityEntity);
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
    /// Dim mBDBInformation As BDBInformation = New BDBInformation(MSecurityEntity)
    /// ]]>
    /// </code>
    /// </example>
    public BDBInformation(MSecurityEntity securityEntityProfile)
    {
        if (securityEntityProfile == null) throw new ArgumentNullException(nameof(securityEntityProfile), "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!");
        if (m_DDBInformation == null)
        {
            this.m_DDBInformation = (IDBInformation)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDBInformation", securityEntityProfile.ConnectionString);
            if (this.m_DDBInformation == null)
            {
                throw new InvalidOperationException("Failed to create an instance of DDBInformation.");
            }
        }
    }
    #endregion

    /// <summary>
    /// Return a populated MDBInformation profile.
    /// </summary>
    /// <returns>MDBInformation</returns>
    /// <remarks></remarks>
    public async Task<MDBInformation> GetProfile()
    {
        return new MDBInformation(await this.m_DDBInformation.GetProfileRow());
    }

    /// <summary>
    /// Updates the ZF_INFORMATION table using the properties of a given profile.
    /// </summary>
    /// <param name="profile">MDBInformation</param>
    /// <returns>Boolean</returns>
    /// <remarks></remarks>
    public async Task<bool> UpdateProfile(MDBInformation profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile can not be null (Nothing in Visual Basic)");
        bool mRetVal = false;
        this.m_DDBInformation.Profile = profile;
        if (DatabaseIsOnline())
        {
            mRetVal = await this.m_DDBInformation.UpdateProfile();
        }
        return mRetVal;
    }
}
