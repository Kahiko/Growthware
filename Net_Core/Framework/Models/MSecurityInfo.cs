using System;
using System.Collections.ObjectModel;
using GrowthWare.Framework.Interfaces;

namespace GrowthWare.Framework.Models
{
    /// <summary>
    /// MSecurityInfo takes an implementation of ISecurityInfo and a collection of
    /// strings to set the May properties.
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MSecurityInfo
    {

        private bool m_MayView = false;
        private bool m_MayAdd = false;
        private bool m_MayEdit = false;
        private bool m_MayDelete = false;

        /// <summary>
        /// MayView()--
        /// This property is calculated relative to the current object that 
        /// implements ISecurityInfo.  
        /// When true, user can view the module.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool MayView
        {
            get { return m_MayView; }
        }

        /// <summary>
        /// MayAdd()--
        /// This property is calculated relative to the current object that 
        /// implements ISecurityInfo.  
        /// When true, user can view the module.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool MayAdd
        {
            get { return m_MayAdd; }
        }

        /// <summary>
        /// MayEdit()--
        /// This property is calculated relative to the current object that 
        /// implements ISecurityInfo.  
        /// When true, user can view the module.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool MayEdit
        {
            get { return m_MayEdit; }
        }

        /// <summary>
        /// MayDelete()--
        /// This property is calculated relative to the current object that 
        /// implements ISecurityInfo.  
        /// When true, user can view the module.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool MayDelete
        {
            get { return m_MayDelete; }
        }

        /// <summary>
        /// Creates a new instance of MSecurityInfo
        /// </summary>
        /// <remarks></remarks>
        public MSecurityInfo()
        {
        }

        /// <summary>
        /// Creates a new instance of MSecurityInfo checking the groups and derived roles for each permission against the account's groups and derived roles
        /// </summary>
        /// <param name="permissionSecurity"></param>
        /// <param name="accountProfile"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MSecurityInfo(IGroupRolePermissionSecurity groupRolePermissionSecurity, IGroupRoleSecurity groupRoleSecurity)
        {
            if (groupRolePermissionSecurity == null) throw new ArgumentNullException(nameof(groupRolePermissionSecurity), "groupRolePermissionSecurity cannot be a null reference (Nothing in Visual Basic)!");
            if (groupRoleSecurity == null) throw new ArgumentNullException(nameof(groupRoleSecurity), "profileWithDerivedRoles cannot be a null reference (Nothing in Visual Basic)!");
            // not all object will be a type of MAccountProfile
            if(groupRoleSecurity is MAccountProfile profile)
            {
                /*
                    If the account profile is a system admin, then all permissions are granted
                    This handles the special case when the selected SecurityEntity has no parent and
                    groups/roles have not been setup and the UI is needed to "Fix" secutiry.
                */
                if(!profile.IsSystemAdmin)
                {
                    SetMemberFields(groupRolePermissionSecurity, groupRoleSecurity);
                }
                else
                {
                    m_MayView = true;
                    m_MayAdd = true;
                    m_MayEdit = true;
                    m_MayDelete = true;
                }
            }
            else
            {
                SetMemberFields(groupRolePermissionSecurity, groupRoleSecurity);
            }

        }

        /// <summary>
        /// Sets the member fields for each permission using the groups and roles from the groupRolePermissionSecurity and the groups and roles from the groupRoleSecurity
        /// </summary>
        /// <param name="groupRolePermissionSecurity"></param>
        /// <param name="groupRoleSecurity"></param>
        protected void SetMemberFields(IGroupRolePermissionSecurity groupRolePermissionSecurity, IGroupRoleSecurity groupRoleSecurity)
        {
            // Check View Permissions
            m_MayView = CheckGroups(groupRolePermissionSecurity.ViewGroups, groupRoleSecurity.Groups);
            if (m_MayView == false && groupRolePermissionSecurity.DerivedViewRoles != null)
            {
                m_MayView = CheckRoles(groupRolePermissionSecurity.DerivedViewRoles, groupRoleSecurity.DerivedRoles);
            }
            // Check Add Permissions
            m_MayAdd = CheckGroups(groupRolePermissionSecurity.AddGroups, groupRoleSecurity.Groups);
            if (m_MayAdd == false && groupRolePermissionSecurity.DerivedAddRoles != null)
            {
                m_MayAdd = CheckRoles(groupRolePermissionSecurity.DerivedAddRoles, groupRoleSecurity.DerivedRoles);
            }
            // Check Edit Permissions
            m_MayEdit = CheckGroups(groupRolePermissionSecurity.EditGroups, groupRoleSecurity.Groups);
            if (m_MayEdit == false && groupRolePermissionSecurity.DerivedEditRoles != null)
            {
                m_MayEdit = CheckRoles(groupRolePermissionSecurity.DerivedEditRoles, groupRoleSecurity.DerivedRoles);
            }
            // Check Delete Permissions
            m_MayDelete = CheckGroups(groupRolePermissionSecurity.DeleteGroups, groupRoleSecurity.Groups);
            if (m_MayDelete == false && groupRolePermissionSecurity.DerivedDeleteRoles != null)
            {
                m_MayDelete = CheckRoles(groupRolePermissionSecurity.DerivedDeleteRoles, groupRoleSecurity.DerivedRoles);
            }
        }

        /// <summary>
        /// Checks whether the permission roles contain the profile's roles
        /// </summary>
        /// <param name="permissionRoles"></param>
        /// <param name="profileDerivedRoles"></param>
        /// <returns></returns>
        protected static bool CheckRoles(Collection<String> permissionRoles, Collection<String> profileDerivedRoles)
        {
            // if there are no permission or profile roles then return false
            if (permissionRoles.Count == 0 || profileDerivedRoles.Count == 0)
            {
                return false;
            }
            // If permissionRoles contains the role "Anonymous" the don't bother running the rest of code just return true
            if (permissionRoles.Contains("Anonymous")) return true;
            // If profileDerivedRoles contains the role "SysAdmin" the don't bother running the rest of code just return true
            if (profileDerivedRoles.Contains("SysAdmin")) return true;
            foreach (string role in permissionRoles)
            {
                if (profileDerivedRoles.Contains(role))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks whether the permission groups contain the profile's groups
        /// </summary>
        /// <param name="permissionGroups"></param>
        /// <param name="profileGroups"></param>
        /// <returns></returns>
        protected static bool CheckGroups(Collection<String> permissionGroups, Collection<String> profileGroups)
        {
            // if there are no permission or profile groups then return false
            if (permissionGroups.Count == 0 || profileGroups.Count == 0)
            {
                return false;
            }
            foreach (string mGroup in profileGroups)
            {
                if (permissionGroups.Contains(mGroup))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
