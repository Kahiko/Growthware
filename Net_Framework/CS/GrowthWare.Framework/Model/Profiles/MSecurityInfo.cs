﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GrowthWare.Framework.Model.Profiles.Interfaces;

namespace GrowthWare.Framework.Model.Profiles
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
        /// Initializes a new HSecurityInfo object given an object that implements ISecurityInfo.
        ///  All client permissions are calculated relative to the object and the client roles.
        /// </summary>
        /// <param name="groupRolePermissionSecurity">A profile(Function) that implements IMGroupRolePermissionSecurity.</param>
        /// <param name="profileWithDerivedRoles">A profile(Account) that implements IMGroupRoleSecurity.</param>
        public MSecurityInfo(IMGroupRolePermissionSecurity groupRolePermissionSecurity, IMGroupRoleSecurity profileWithDerivedRoles)
        {
            if (groupRolePermissionSecurity == null) throw new ArgumentNullException("groupRolePermissionSecurity", "groupRolePermissionSecurity cannot be a null reference (Nothing in Visual Basic)!");
            if (profileWithDerivedRoles == null) throw new ArgumentNullException("profileWithDerivedRoles", "profileWithDerivedRoles cannot be a null reference (Nothing in Visual Basic)!");
            // Check View Permissions
            m_MayView = CheckAuthenticatedPermission(groupRolePermissionSecurity.DerivedViewRoles, profileWithDerivedRoles.DerivedRoles);
            // Check Add Permissions
            m_MayAdd = CheckAuthenticatedPermission(groupRolePermissionSecurity.DerivedAddRoles, profileWithDerivedRoles.DerivedRoles);
            // Check Edit Permissions
            m_MayEdit = CheckAuthenticatedPermission(groupRolePermissionSecurity.DerivedEditRoles, profileWithDerivedRoles.DerivedRoles);
            // Check Delete Permissions
            m_MayDelete = CheckAuthenticatedPermission(groupRolePermissionSecurity.DerivedDeleteRoles, profileWithDerivedRoles.DerivedRoles);
        }

        /// <summary>
        /// Checks whether an account is in the necessary role for the 4 permissions given an objects roles
        /// </summary>
        /// <param name="objRoles">The obj roles.</param>
        /// <param name="profileDerivedRoles">The derived roles.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        protected static bool CheckAuthenticatedPermission(Collection<String> objRoles, Collection<String> profileDerivedRoles)
        {
            if (objRoles == null) throw new ArgumentNullException("objRoles", "objRoles cannot be a null reference (Nothing in Visual Basic)!");
            if (profileDerivedRoles == null) throw new ArgumentNullException("profileDerivedRoles", "profileDerivedRoles cannot be a null reference (Nothing in Visual Basic)!");
            // If page/module contains the role "Anonymous" the don't bother running the rest of code just return true
            if (objRoles.Contains("Anonymous")) return true;
            if (profileDerivedRoles.Contains("SysAdmin")) return true;
            foreach (string role in objRoles)
            {
                if (profileDerivedRoles.Contains(role))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
