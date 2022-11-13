using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;

namespace GrowthWare.BusinessLogic
{
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
    /// MSecurityEntity MSecurityEntity = MSecurityEntity = New MSecurityEntity();
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID;
    /// MSecurityEntity.DAL = ConfigSettings.DAL;
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
    /// 
    /// Dim myBll as new BSecurityEntities(mySecurityEntityProfile, ConfigSettings.CentralManagement)
    /// ]]>
    /// </code>
    /// <code language="C#">
    /// <![CDATA[
    /// MSecurityEntity MSecurityEntity = new MSecurityEntity();
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID;
    /// MSecurityEntity.DAL = ConfigSettings.DAL;
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
    /// 
    /// BSecurityEntities mBSecurityEntities = New BSecurityEntities(MSecurityEntity, ConfigSettings.CentralManagement);
    /// ]]>
    /// </code>	/// </example>
    public class BSecurityEntities : AbstractBusinessLogic
    {
        private ISecurityEntities m_DSecurityEntities;

        /// <summary>
        /// Private constructor to ensure only new instances with passed parameters is used.
        /// </summary>
        /// <remarks></remarks>
        private BSecurityEntities() { }

        /// <summary>
        /// Parameters are need to pass along to the factory for correct connection to the desired datastore.
        /// </summary>
        /// <param name="securityEntityProfile">The Security Entity profile used to obtain the DAL name, DAL name space, and the Connection String</param>
        /// <param name="centralManagement">Boolean value indicating if the system is being used to manage multiple database instances.</param>
        /// <remarks></remarks>
        /// <example> This sample shows how to create an instance of the class.
        /// <code language="VB.NET">
        /// <![CDATA[
        /// MSecurityEntity MSecurityEntity = MSecurityEntity = New MSecurityEntity();
        /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID;
        /// MSecurityEntity.DAL = ConfigSettings.DAL;
        /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
        /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
        /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
        /// 
        /// Dim mBClientChoices As BClientChoices = New BClientChoices(MSecurityEntity, ConfigSettings.CentralManagement)
        /// ]]>
        /// </code>
        /// <code language="C#">
        /// <![CDATA[
        /// Dim MSecurityEntity As MSecurityEntity = New MSecurityEntity()
        /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID
        /// MSecurityEntity.DAL = ConfigSettings.DAL
        /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL)
        /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL)
        /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString
        /// 
        /// BClientChoices mBClientChoices = new BClientChoices(MSecurityEntity, ConfigSettings.CentralManagement);
        /// ]]>
        /// </code>
        /// </example>
        public BSecurityEntities(MSecurityEntity securityEntityProfile, bool centralManagement)
        {
            if (securityEntityProfile == null)
            {
                throw new ArgumentNullException("securityEntityProfile", "The securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!!");
            }
            if (centralManagement)
            {
                if (m_DSecurityEntities == null)
                {
                    m_DSecurityEntities = (ISecurityEntities)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DSecurityEntities");
                }
            }
            else
            {
                m_DSecurityEntities = (ISecurityEntities)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DSecurityEntities");
            }

            m_DSecurityEntities.ConnectionString = securityEntityProfile.ConnectionString;
        }

        /// <summary>
        /// Returns a collection of MSecurityEntity objects for the given.
        /// </summary>
        /// <returns>
        ///		Collection of MSecurityEntity
        ///	</returns>
        /// <remarks></remarks>
        public Collection<MSecurityEntity> SecurityEntities()
        {
            Collection<MSecurityEntity> mRetVal = new Collection<MSecurityEntity>();
            DataTable mDataTable = null;
            try
            {
                if (ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture) == "ONLINE")
                {
                    mDataTable = m_DSecurityEntities.GetSecurityEntities();
                    foreach (DataRow item in mDataTable.Rows)
                    {
                        MSecurityEntity mProfile = new MSecurityEntity(item);
                        mRetVal.Add(mProfile);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mDataTable != null)
                {
                    mDataTable.Dispose();
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the valid security entities.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="SecurityEntityID">The security entity id.</param>
        /// <param name="isSystemAdmin">if set to <c>true</c> [is system admin].</param>
        /// <returns>DataTable.</returns>
        public DataTable GetValidSecurityEntities(string account, int SecurityEntityID, bool isSystemAdmin)
        {
            DataTable mRetVal = null;
            if (ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture) == "ONLINE")
            { 
                mRetVal = m_DSecurityEntities.GetValidSecurityEntities(account, SecurityEntityID, isSystemAdmin);
            }
            return mRetVal;
        }

        /// <summary>
        /// Save Function information to the database
        /// </summary>
        /// <param name="profile">MSecurityEntity</param>
        /// <returns>Integer</returns>
        public int Save(MSecurityEntity profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!");
            profile.Id = profile.Id;
            if (ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture) == "ONLINE") m_DSecurityEntities.Save(profile);
            return profile.Id;
        }
    }
}
