using System;
using GrowthWare.Framework.ModelObjects.Base.Interfaces;

namespace GrowthWare.Framework.ModelObjects
{
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
		public bool MayView {
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
		public bool MayAdd {
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
		public bool MayEdit {
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
		public bool MayDelete {
			get { return m_MayDelete; }
		}


		public MSecurityInfo()
		{
		}

		/// <summary>
		/// Initializes a new HSecurityInfo object given an object that implements ISecurityInfo.
		///  All client permissions are calculated relative to the object and the client roles.
		/// </summary>
		/// <param name="SecurityInfoObject"></param>
		/// <remarks></remarks>
		public MSecurityInfo(ISecurityInfo SecurityInfoObject, string[] AccountRoles)
		{
			// Check View Permissions
			m_MayView = CheckAuthenticatedPermission(SecurityInfoObject.ViewRoles, AccountRoles);
			// Check Add Permissions
			m_MayAdd = CheckAuthenticatedPermission(SecurityInfoObject.AddRoles, AccountRoles);
			// Check Edit Permissions
			m_MayEdit = CheckAuthenticatedPermission(SecurityInfoObject.EditRoles, AccountRoles);
			// Check Delete Permissions
			m_MayDelete = CheckAuthenticatedPermission(SecurityInfoObject.DeleteRoles, AccountRoles);
		}

		/// <summary>
		/// Checks whether an account is in the necessary role for the 4 permissions given an objects roles
		/// </summary>
		/// <param name="ObjectRoles">String array</param>
		/// <returns>True/False</returns>
		/// <remarks></remarks>
		protected bool CheckAuthenticatedPermission(string[] ObjectRoles, string[] AccountRoles)
		{
			// If page/module contains the role "Anonymous" the don't bother running the rest of code just return true
			if (Array.IndexOf(ObjectRoles, "Anonymous") != -1) return true; 
			if (Array.IndexOf(AccountRoles,"Sysadmin") != -1)  return true;
			foreach (string role in ObjectRoles) {
				if (Array.IndexOf(AccountRoles, role) != -1)
				{
					return true;
				}
			}
			return false;
		}

	}
}
