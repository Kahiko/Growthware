using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System;
using System.Collections;
using System.Data;

namespace GrowthWare.BusinessLogic
{
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
    public class BGroups : BaseBusinessLogic
    {
        private IGroups m_DGroups;

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
        /// <code language="VB.NET">
        /// <![CDATA[
        /// MSecurityEntity MSecurityEntity = MSecurityEntity = New MSecurityEntity();
        /// MSecurityEntity.Id = ConfigSettings.DefaultSecurityEntityID;
        /// MSecurityEntity.DAL = ConfigSettings.DAL;
        /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
        /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
        /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
        /// 
        /// BGroupss mBGroups = BGroupss = New BGroupss(MSecurityEntity, ConfigSettings.CentralManagement);
        /// ]]>
        /// </code>
        /// <code language="C#">
        /// <![CDATA[
        /// Dim MSecurityEntity As MSecurityEntity = New MSecurityEntity()
        /// MSecurityEntity.Id = ConfigSettings.DefaultSecurityEntityID
        /// MSecurityEntity.DAL = ConfigSettings.DAL
        /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL)
        /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL)
        /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString
        /// 
        /// Dim mBGroups As BGroupss = New BGroupss(MSecurityEntity, ConfigSettings.CentralManagement)
        /// ]]>
        /// </code>
        /// </example>
        public BGroups(MSecurityEntity securityEntityProfile, bool centralManagement)
        {
            if (securityEntityProfile == null)
            {
                throw new ArgumentNullException("securityEntityProfile", "The securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!!");
            }
            if (centralManagement)
            {
                if (m_DGroups == null)
                {
                    m_DGroups = (IGroups)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DGroups");
                }
            }
            else
            {
                m_DGroups = (IGroups)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DGroups");
            }

            m_DGroups.ConnectionString = securityEntityProfile.ConnectionString;
            m_DGroups.SecurityEntitySeqID = securityEntityProfile.Id;
        }

        /// <summary>
        /// Gets the groups by security entity.
        /// </summary>
        /// <param name="securityEntityId">The security entity ID.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetGroupsBySecurityEntity(int securityEntityId)
        {
            MGroupProfile myProfile = new MGroupProfile();
            myProfile.SecurityEntityId = securityEntityId;
            m_DGroups.Profile = myProfile;
            return m_DGroups.GroupsBySecurityEntity();
        }

        /// <summary>
        /// Adds the group.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>System.Int32.</returns>
        public void AddGroup(MGroupProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!");
            m_DGroups.Profile = profile;
            if(DatabaseIsOnline()) m_DGroups.AddGroup();
        }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <param name="groupId">The group ID.</param>
        /// <returns>MGroupProfile.</returns>
        public MGroupProfile GetProfile(int groupId)
        {
            MGroupProfile retProfile = new MGroupProfile();
            retProfile.Id = groupId;
            m_DGroups.Profile = retProfile;
            retProfile = new MGroupProfile(m_DGroups.ProfileData());
            return retProfile;
        }

        /// <summary>
        /// Deletes the group.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool DeleteGroup(MGroupProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!");
            bool retVal = false;
            m_DGroups.Profile = profile;
            if (DatabaseIsOnline()) retVal = m_DGroups.DeleteGroup();
            return retVal;
        }

        /// <summary>
        /// Updates the group.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public void Save(MGroupProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!");
            m_DGroups.Profile = profile;
            if (DatabaseIsOnline()) m_DGroups.Save();
        }

        /// <summary>
        /// Gets the selected roles.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>System.String[][].</returns>
        public string[] GetSelectedRoles(MGroupRoles profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!");
            ArrayList ClientRoles = new ArrayList();
            m_DGroups.GroupRolesProfile = profile;
            DataTable myDataTable = null;
            if (DatabaseIsOnline()) 
            {
                try
                {
                    myDataTable = m_DGroups.GroupRoles();
                    DataRow myDR = null;
                    foreach (DataRow myDR_loopVariable in myDataTable.Rows)
                    {
                        myDR = myDR_loopVariable;
                        ClientRoles.Add(myDR["Role"].ToString());
                    }
                }
                catch (BusinessLogicLayerException)
                {
                    throw;
                }
            }
            return (string[])ClientRoles.ToArray(typeof(string));
        }

        /// <summary>
        /// Searches groups using the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>DataTable.</returns>
        public DataTable Search( MSearchCriteria searchCriteria)
        {
            if (searchCriteria == null) throw new ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!!");
            DataTable mRetVal = null;
            if (DatabaseIsOnline()) mRetVal = m_DGroups.Search(searchCriteria);
            return mRetVal;
        }

        /// <summary>
        /// Updates the group roles.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool UpdateGroupRoles(MGroupRoles profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!");
            bool mRetVal = false;
            m_DGroups.GroupRolesProfile = profile;
            if (DatabaseIsOnline()) mRetVal = m_DGroups.UpdateGroupRoles();
            return mRetVal;
        }
    }
}
