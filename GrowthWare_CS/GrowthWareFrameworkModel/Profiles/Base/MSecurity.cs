using System;
using System.Collections;
using System.Data;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles.Base.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GrowthWare.Framework.Model.Profiles.Base
{
	/// <summary>
	/// The MBaseSecurity is a abstract class that when inherited will add 4 types of roles
	/// to your class/object.  After you have inherited the class pass a data row to the SecurityInit sub 
	/// to populate the roles.
	/// </summary>
	/// <remarks>
	/// Currently there are 4 permission roles and they are Add, Edit, View, Delete.  
	/// If you would like to extend this class do so by inheriting this class and adding your
	/// own types of roles say Moderate if your writing some sort of formum type of class.
	/// Any of the objects you create should now inherit your class and will now have all of the 
	/// roles from this class as well as the ones for yours.
	///</remarks>
	public abstract class MSecurity : MProfile, ISecurityInfo
	{
		private Collection<string> m_AssignedAddRoles = new Collection<string>();
		private Collection<string> m_AssignedDeleteRoles = new Collection<string>();
		private Collection<string> m_AssignedEditRoles = new Collection<string>();
		private Collection<string> m_AssignedViewRoles = new Collection<string>();

		private Collection<string> m_DerivedAddRoles = new Collection<string>();
		private Collection<string> m_DerivedDeleteRoles = new Collection<string>();
		private Collection<string> m_DerivedEditRoles = new Collection<string>();
		private Collection<string> m_DerivedViewRoles = new Collection<string>();

		private Collection<string> m_AddGroups = new Collection<string>();
		private Collection<string> m_DeleteGroups = new Collection<string>();
		private Collection<string> m_EditGroups = new Collection<string>();
		private Collection<string> m_ViewGroups = new Collection<string>();
		private string m_PermissionColumn = "PERMISSIONS_SEQ_ID";

		private string m_RoleColumn = "ROLE";
		private string m_GroupColumn = "Group";

		/// <summary>
		/// Retruns a comma seporated string given a Collection strings
		/// </summary>
		/// <param name="collectionOfStrings">Collecion</param>
		/// <returns>comma seportated string.</returns>
		private string getCommaSeportatedString(Collection<string> collectionOfStrings) 
		{ 
			String mRetValue = String.Empty;
			if (collectionOfStrings != null) 
			{
				if (collectionOfStrings.Count > 0) 
				{
					foreach (var item in collectionOfStrings)
					{
						mRetValue += item.ToString() + ",";
					}
				}
			}
			if (mRetValue.Length > 0) 
			{
				mRetValue = mRetValue.Substring(0, mRetValue.Length - 1);
			}
			return mRetValue;
		}

		/// <summary>
		/// Initialize orverloads and calles mybase.init to will populate the Add, Delete, Edit, and View role properties.
		/// </summary>
		/// <param name="detailDatarow">A data row that contains base information</param>
		/// <param name="derivedRoles">An array of data rows that must contain two columns ("PERMISSIONS_SEQ_ID","ROLE")</param>
		/// <param name="assignedRoles">An array of data rows that must contain two columns ("PERMISSIONS_SEQ_ID","ROLE")</param>
		/// <param name="groups">An array of data rows that must contain two columns ("PERMISSIONS_SEQ_ID","ROLE")</param>
		/// <remarks></remarks>
		virtual protected void Initialize(ref DataRow DetailRow, ref DataRow[] derivedRoles, ref DataRow[] assignedRoles, ref DataRow[] groups)
		{
			base.Initialize(ref DetailRow);
			setRoleOrGroup(ref m_DerivedAddRoles, derivedRoles, PermissionType.Add, m_RoleColumn);
			setRoleOrGroup(ref m_DerivedDeleteRoles, derivedRoles, PermissionType.Delete, m_RoleColumn);
			setRoleOrGroup(ref m_DerivedEditRoles, derivedRoles, PermissionType.Edit, m_RoleColumn);
			setRoleOrGroup(ref m_DerivedViewRoles, derivedRoles, PermissionType.View, m_RoleColumn);
			if (assignedRoles != null) 
			{
				setRoleOrGroup(ref m_AssignedAddRoles, assignedRoles, PermissionType.Add, m_RoleColumn);
				setRoleOrGroup(ref m_AssignedDeleteRoles, assignedRoles, PermissionType.Delete, m_RoleColumn);
				setRoleOrGroup(ref m_AssignedEditRoles, assignedRoles, PermissionType.Edit, m_RoleColumn);
				setRoleOrGroup(ref m_AssignedViewRoles, assignedRoles, PermissionType.View, m_RoleColumn);		
			}
			if (groups != null) 
			{
				setRoleOrGroup(ref m_AddGroups, groups, PermissionType.Add, m_GroupColumn);
				setRoleOrGroup(ref m_DeleteGroups, groups, PermissionType.Delete, m_GroupColumn);
				setRoleOrGroup(ref m_EditGroups, groups, PermissionType.Edit, m_GroupColumn);
				setRoleOrGroup(ref m_ViewGroups, groups, PermissionType.View, m_GroupColumn);		
			}

		}

		/// <summary>
		/// Return assigned roles associated with the "Add" permission.
		/// </summary>
		public Collection<string> AssignedAddRoles
		{
			get { return m_AssignedAddRoles; }
		}

		/// <summary>
		/// Return roles associated with the "Add" permission.
		/// </summary>
		public Collection<string> DerivedAddRoles
		{
			get { return m_DerivedAddRoles; }
		}

		/// <summary>
		/// Return assigned roles associated with the "Delete" permission.
		/// </summary>
		public Collection<string> AssignedDeleteRoles
		{
			get { return m_AssignedDeleteRoles; }
		}

		/// <summary>
		/// Return roles associated with the "Delete" permission.
		/// </summary>
		public Collection<string> DerivedDeleteRoles
		{
			get { return m_DerivedDeleteRoles; }
		}

		/// <summary>
		/// Return roles associated with the "Edit" permission.
		/// </summary>
		public Collection<string> AssignedEditRoles
		{
			get { return m_AssignedEditRoles; }
		}

		/// <summary>
		/// Return roles associated with the "Edit" permission.
		/// </summary>
		public Collection<string> DerivedEditRoles
		{
			get { return m_DerivedEditRoles; }
		}

		/// <summary>
		/// Return assigned roles associated with the "View" permission.
		/// </summary>
		public Collection<string> AssignedViewRoles
		{
			get { return m_AssignedViewRoles; }
		}

		/// <summary>
		/// Return roles associated with the "View" permission.
		/// </summary>
		public Collection<string> DerivedViewRoles
		{
			get { return m_DerivedViewRoles; }
		}

		/// <summary>
		/// Converts the collection of AssignedGroups to a comma seporated string.
		/// </summary>
		/// <returns>String</returns>
		public string GetCommaSeporatedGroups(PermissionType permission) 
		{
			switch (permission)
			{
				case PermissionType.Add:
					return this.getCommaSeportatedString(m_AddGroups);
				case PermissionType.Delete:
					return this.getCommaSeportatedString(m_DeleteGroups);
				case PermissionType.Edit:
					return this.getCommaSeportatedString(m_EditGroups);
				case PermissionType.View:
					return this.getCommaSeportatedString(m_ViewGroups);
				default:
					return null;
			}
		}

		/// <summary>
		/// Converts the collection of AssignedGroups to a comma seporated string.
		/// </summary>
		/// <returns>String</returns>
		public string GetCommaSeporatedRoles(PermissionType permission)
		{
			switch (permission)
			{
				case PermissionType.Add:
					return this.getCommaSeportatedString(m_AssignedAddRoles);
				case PermissionType.Delete:
					return this.getCommaSeportatedString(m_AssignedDeleteRoles);
				case PermissionType.Edit:
					return this.getCommaSeportatedString(m_AssignedEditRoles);
				case PermissionType.View:
					return this.getCommaSeportatedString(m_AssignedViewRoles);
				default:
					return null;
			}
		}

		/// <summary>
		/// Represents the permission column name.
		/// </summary>
		public string PermissionColumn {
			get { return m_PermissionColumn; }
			set { m_PermissionColumn = value.Trim(); }
		}

		/// <summary>
		/// Represents the role column name.
		/// </summary>
		public string RoleColumn {
			get { return m_RoleColumn; }
			set { m_RoleColumn = value.Trim(); }
		}

		/// <summary>
		/// Populates the given permissions roles.
		/// </summary>
		/// <param name="refCollection">reference to the role or group colletion</param>
		/// <param name="roleOrGroups">An array of rows for the role or group</param>
		/// <param name="permissionType">the type of role or group (View, Add, Edit, Delete)</param>
		/// <param name="dataColumnName">Name of the column containg the data... will be different for roles and groups.</param>
		/// <remarks></remarks>
		private void setRoleOrGroup(ref Collection<String> refCollection, DataRow[] roleOrGroups, PermissionType permissionType, String dataColumnName)
		{
			refCollection = new Collection<String>();
			foreach(DataRow row in roleOrGroups)
			{
				if(!Convert.IsDBNull(row[m_PermissionColumn]))
				{
					if (!Convert.IsDBNull(row[dataColumnName])) 
					{
						if (int.Parse(row[m_PermissionColumn].ToString()) == (int)permissionType) 
						{
							refCollection.Add(row[dataColumnName].ToString());
						}
					}
				}
			}
		}

		/// <summary>
		/// Populates all permissions.
		/// </summary>
		/// <param name="SecurityRows">DataRowCollection containing all derived roles for all permissions</param>
		protected void PopulatePermissions(DataRowCollection SecurityRows)
		{
			throw new NotImplementedException();
		}
	}
}
