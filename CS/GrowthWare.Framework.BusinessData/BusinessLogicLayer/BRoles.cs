using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.BusinessData.BusinessLogicLayer
{
    /// <summary>
    /// Process business logic for accounts
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// MSecurityEntityProfile can be found in the GrowthWare.Framework.ModelObjects namespace.  
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
    public class BRoles
    {
        private IDRoles m_BRoles;

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
        /// <code language="VB.NET">
        /// <![CDATA[
        /// MSecurityEntityProfile mSecurityEntityProfile = MSecurityEntityProfile = New MSecurityEntityProfile();
        /// mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID;
        /// mSecurityEntityProfile.DAL = ConfigSettings.DAL;
        /// mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL);
        /// mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL);
        /// mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString;
        /// 
        /// BRoless mBRoles = BRoless = New BRoless(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        /// ]]>
        /// </code>
        /// <code language="C#">
        /// <![CDATA[
        /// Dim mSecurityEntityProfile As MSecurityEntityProfile = New MSecurityEntityProfile()
        /// mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID
        /// mSecurityEntityProfile.DAL = ConfigSettings.DAL
        /// mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL)
        /// mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL)
        /// mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString
        /// 
        /// Dim mBRoles As BRoless = New BRoless(mSecurityEntityProfile, ConfigSettings.CentralManagement)
        /// ]]>
        /// </code>
        /// </example>
        public BRoles(MSecurityEntityProfile securityEntityProfile, bool centralManagement)
        {
            if (securityEntityProfile == null)
            {
                throw new ArgumentException("The securityEntityProfile and not be null!");
            }
            if (centralManagement)
            {
                if (m_BRoles == null)
                {
                    m_BRoles = (IDRoles)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DRoles");
                }
            }
            else
            {
                m_BRoles = (IDRoles)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DRoles");
            }

            m_BRoles.ConnectionString = securityEntityProfile.ConnectionString;
            m_BRoles.SecurityEntitySeqID = securityEntityProfile.Id;
        }

        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void Save(MRoleProfile profile)
        {
            m_BRoles.Profile = profile;
            m_BRoles.Save();
        }

        /// <summary>
        /// Searches the specified search critera.
        /// </summary>
        /// <param name="searchCriteria">The search critera.</param>
        /// <returns>DataTable.</returns>
        public DataTable Search(MSearchCriteria searchCriteria)
        {
            return m_BRoles.Search(searchCriteria);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public void DeleteRole(MRoleProfile profile)
        {
            m_BRoles.Profile = profile;
            m_BRoles.DeleteRole();
        }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void GetProfile(MRoleProfile profile)
        {
            m_BRoles.Profile = profile;
            profile = new MRoleProfile(m_BRoles.ProfileData());
        }

        /// <summary>
        /// Gets the roles by BU.
        /// </summary>
        /// <param name="securityEntityId">The security entity ID.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetRolesBySecurityEntity(int securityEntityId)
        {
            m_BRoles.SecurityEntitySeqID = securityEntityId;
            return m_BRoles.RolesBySecurityEntity();
        }

        /// <summary>
        /// Gets the accounts in role.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetAccountsInRole(MRoleProfile profile)
        {
            m_BRoles.Profile = profile;
            return m_BRoles.AccountsInRole();
        }

        /// <summary>
        /// Gets the accounts not in role.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetAccountsNotInRole(MRoleProfile profile)
        {
            m_BRoles.Profile = profile;
            return m_BRoles.AccountsNotInRole();
        }

        /// <summary>
        /// Updates all accounts for role.
        /// </summary>
        /// <param name="roleSeqId">The role seq ID.</param>
        /// <param name="securityEntityId">The security entity ID.</param>
        /// <param name="accounts">The accounts.</param>
        /// <param name="accountSeqId">The account seq ID.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool UpdateAllAccountsForRole(int roleSeqId, int securityEntityId, string[] accounts, int accountSeqId)
        {
            return m_BRoles.UpdateAllAccountsForRole(roleSeqId, securityEntityId, accounts, accountSeqId);
        }
    }
}
