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
    /// MSecurityEntityProfile can be found in the GrowthWare.Framework.Model.Profiles namespace.  
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
    public class BRoles : BaseBusinessLogic
    {
        private IDRoles m_DRoles;

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
        /// BRoles mBRoles = BRoles = New BRoles(mSecurityEntityProfile, ConfigSettings.CentralManagement);
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
        /// Dim mBRoles As BRoles = New BRoles(mSecurityEntityProfile, ConfigSettings.CentralManagement)
        /// ]]>
        /// </code>
        /// </example>
        public BRoles(MSecurityEntityProfile securityEntityProfile, bool centralManagement)
        {
            if (securityEntityProfile == null)
            {
                throw new ArgumentNullException("securityEntityProfile", "The securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!!");
            }
            if (centralManagement)
            {
                if (m_DRoles == null)
                {
                    m_DRoles = (IDRoles)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DRoles");
                }
            }
            else
            {
                m_DRoles = (IDRoles)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DRoles");
            }

            m_DRoles.ConnectionString = securityEntityProfile.ConnectionString;
            m_DRoles.SecurityEntitySeqID = securityEntityProfile.Id;
        }

        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void Save(MRoleProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!");
            m_DRoles.Profile = profile;
            if (DatabaseIsOnline()) m_DRoles.Save();
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>DataTable.</returns>
        public DataTable Search(MSearchCriteria searchCriteria)
        {
            if (searchCriteria == null) throw new ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!!");
            DataTable mRetVal = null;
            if (DatabaseIsOnline()) mRetVal = m_DRoles.Search(searchCriteria);
            return mRetVal;
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public void DeleteRole(MRoleProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!");
            m_DRoles.Profile = profile;
            if (DatabaseIsOnline()) m_DRoles.DeleteRole();
        }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public MRoleProfile GetProfile(MRoleProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!");
            MRoleProfile mRetVal = null;
            m_DRoles.Profile = profile;
            if (DatabaseIsOnline()) mRetVal = new MRoleProfile(m_DRoles.ProfileData());
            return mRetVal;
        }

        /// <summary>
        /// Gets the roles by BU.
        /// </summary>
        /// <param name="securityEntityId">The security entity ID.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetRolesBySecurityEntity(int securityEntityId)
        {
            m_DRoles.SecurityEntitySeqID = securityEntityId;
            return m_DRoles.RolesBySecurityEntity();
        }

        /// <summary>
        /// Gets the accounts in role.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetAccountsInRole(MRoleProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!");
            DataTable mRetVal = null;
            m_DRoles.Profile = profile;
            if (DatabaseIsOnline()) mRetVal = m_DRoles.AccountsInRole();
            return mRetVal;
        }

        /// <summary>
        /// Gets the accounts not in role.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetAccountsNotInRole(MRoleProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!");
            DataTable mRetVal = null;
            m_DRoles.Profile = profile;
            if (DatabaseIsOnline()) mRetVal = m_DRoles.AccountsNotInRole();
            return mRetVal;
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
            if (accounts == null) throw new ArgumentNullException("accounts", "accounts cannot be a null reference (Nothing in Visual Basic)!!");
            bool mRetVal = false;
            if (DatabaseIsOnline()) mRetVal = m_DRoles.UpdateAllAccountsForRole(roleSeqId, securityEntityId, accounts, accountSeqId);
            return mRetVal;
        }
    }
}
