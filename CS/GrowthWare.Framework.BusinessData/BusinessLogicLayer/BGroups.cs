using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections;
using System.Data;
using System.Globalization;

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
    /// Dim myBll as new BGroups(mySecurityEntityProfile)
    /// ]]>
    /// </code>
    /// </example>
    public class BGroups : BaseBusinessLogic
    {
        private IDGroups m_DGroups;

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
        /// MSecurityEntityProfile mSecurityEntityProfile = MSecurityEntityProfile = New MSecurityEntityProfile();
        /// mSecurityEntityProfile.Id = ConfigSettings.DefaultSecurityEntityID;
        /// mSecurityEntityProfile.DAL = ConfigSettings.DAL;
        /// mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL);
        /// mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL);
        /// mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString;
        /// 
        /// BGroupss mBGroups = BGroupss = New BGroupss(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        /// ]]>
        /// </code>
        /// <code language="C#">
        /// <![CDATA[
        /// Dim mSecurityEntityProfile As MSecurityEntityProfile = New MSecurityEntityProfile()
        /// mSecurityEntityProfile.Id = ConfigSettings.DefaultSecurityEntityID
        /// mSecurityEntityProfile.DAL = ConfigSettings.DAL
        /// mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL)
        /// mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL)
        /// mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString
        /// 
        /// Dim mBGroups As BGroupss = New BGroupss(mSecurityEntityProfile, ConfigSettings.CentralManagement)
        /// ]]>
        /// </code>
        /// </example>
        public BGroups(MSecurityEntityProfile securityEntityProfile, bool centralManagement)
        {
            if (securityEntityProfile == null)
            {
                throw new ArgumentException("The securityEntityProfile and not be null!");
            }
            if (centralManagement)
            {
                if (m_DGroups == null)
                {
                    m_DGroups = (IDGroups)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DGroups");
                }
            }
            else
            {
                m_DGroups = (IDGroups)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DGroups");
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
            return m_DGroups.GetGroupsBySecurityEntity();
        }

        /// <summary>
        /// Adds the group.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>System.Int32.</returns>
        public void AddGroup(MGroupProfile profile)
        {
            m_DGroups.Profile = profile;
            m_DGroups.AddGroup();
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
            retProfile = new MGroupProfile(m_DGroups.GetProfileData());
            return retProfile;
        }

        /// <summary>
        /// Deletes the group.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool DeleteGroup(MGroupProfile profile)
        {
            bool retVal = false;
            m_DGroups.Profile = profile;
            retVal = m_DGroups.DeleteGroup();
            return retVal;
        }

        /// <summary>
        /// Updates the group.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public void Save(MGroupProfile profile)
        {
            m_DGroups.Profile = profile;
            m_DGroups.Save();
        }

        /// <summary>
        /// Gets the selected roles.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>System.String[][].</returns>
        public string[] GetSelectedRoles(MGroupRoles profile)
        {
            ArrayList ClientRoles = new ArrayList();
            m_DGroups.GroupRolesProfile = profile;
            DataTable myDataTable = m_DGroups.GetGroupRoles();
            DataRow myDR = null;
            foreach (DataRow myDR_loopVariable in myDataTable.Rows)
            {
                myDR = myDR_loopVariable;
                ClientRoles.Add(myDR["Role"].ToString());
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
            return m_DGroups.Search(searchCriteria);
        }

        /// <summary>
        /// Updates the group roles.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool UpdateGroupRoles(MGroupRoles profile)
        {
            m_DGroups.GroupRolesProfile = profile;
            return m_DGroups.UpdateGroupRoles();
        }
    }
}
