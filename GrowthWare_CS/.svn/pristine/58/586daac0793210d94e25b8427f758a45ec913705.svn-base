using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using GrowthWare.Framework.Model.Profiles.Base;

namespace GrowthWare.Framework.Model.Profiles
{
	/// <summary>
	/// Properties for an account.
	/// </summary>
	[Serializable(), CLSCompliant(true)]
	public class MAccountProfile : MProfile
	{

#region "Member fields"
		private Collection<string> m_AssignedRoles = new Collection<string>();
		private Collection<string> m_AssignedGroups = new Collection<string>();
		private Collection<string> m_DerivedRoles = new Collection<string>();
		private String m_RoleColumn = "Roles";
		private String m_GroupColumn = "Groups";
#endregion

#region Private Methods
		/// <summary>
		/// Sets the assigned roles or groups.
		/// </summary>
		/// <param name="StringCollectionObject">The collection of roles or groups that need to be set</param>
		/// <param name="GroupsOrRoles">The DataRowCollection that represents either roles or groups</param>
		/// <param name="ColumnName">The column name to retrieve the data from</param>
		private void setRolesOrGroups(ref Collection<string> StringCollectionObject, DataRowCollection GroupsOrRoles, String ColumnName)
		{
			foreach(DataRow mRow in GroupsOrRoles)
			{
				if(!Convert.IsDBNull(mRow[ColumnName]))
				{
					StringCollectionObject.Add(mRow[ColumnName].ToString());
				}
			}
		}

		/// <summary>
		/// Sets the assigned roles or groups.
		/// </summary>
		/// <param name="StringCollectionObject">The collection of roles or groups that need to be set</param>
		/// <param name="CommaSeporatedString">A comma seporated list of roles or groups 'you, me' as an example</param>
		private void setRolesOrGroups(ref Collection<String> StringCollectionObject, ref String CommaSeporatedString)
		{
			StringCollectionObject = new Collection<string>();
			string[] mRoles = CommaSeporatedString.Split(',');
			foreach(var mRole in mRoles)
			{
				StringCollectionObject.Add(mRole.ToString());
			}
		}

		private string getCommaSeportatedString(Collection<string> CollectionOfStrings)
		{
			String mRetValue = String.Empty;
			if(CollectionOfStrings != null)
			{
				if(CollectionOfStrings.Count > 0)
				{
					foreach(var item in CollectionOfStrings)
					{
						mRetValue += item.ToString() + ",";
					}
				}
			}
			if(mRetValue.Length > 0)
			{
				mRetValue = mRetValue.Substring(0, mRetValue.Length - 1);
			}
			return mRetValue;
		}
#endregion

#region "Protected Methods"
		/// <summary>
		/// Populates direct properties as well as passing the DataRow to the abstract class
		/// for the population of the base properties.
		/// </summary>
		/// <param name="Datarow">DataRow</param>
		new private void Initialize(ref DataRow Datarow)
		{
			base.Initialize(ref Datarow);
			base.Id = base.GetInt(ref Datarow, "ACCT_SEQ_ID");
			Account = base.GetString(ref Datarow, "ACCT");
			base.Name = Account;
			this.EMail = base.GetString(ref Datarow, "EMAIL");
			this.EnableNotifications = base.GetBool(ref Datarow, "ENABLE_NOTIFICATIONS");
			this.IsSystemAdmin = base.GetBool(ref Datarow, "IS_SYSTEM_ADMIN");
			this.Status = base.GetInt(ref Datarow, "STATUS_SEQ_ID");
			this.Password = base.GetString(ref Datarow, "PWD");
			this.FailedAttempts = base.GetInt(ref Datarow, "FAILED_ATTEMPTS");
			this.FirstName = base.GetString(ref Datarow, "FIRST_NAME");
			this.LastLogin = base.GetDateTime(ref Datarow, "LAST_LOGIN", DateTime.Now);
			this.LastName = base.GetString(ref Datarow, "LAST_NAME");
			this.Location = base.GetString(ref Datarow, "LOCATION");
			this.PasswordLastSet = base.GetDateTime(ref Datarow, "PASSWORD_LAST_SET", DateTime.Now);
			this.MiddleName = base.GetString(ref Datarow, "MIDDLE_NAME");
			this.PreferedName = base.GetString(ref Datarow, "PREFERED_NAME");
			this.TimeZone = base.GetInt(ref Datarow, "TIME_ZONE");
		}
#endregion

#region "Public Methods"

		/// <summary>
		/// Provides a new account profile with the default vaules
		/// </summary>
		/// <remarks></remarks>
		public MAccountProfile()
		{
		}

		/// <summary>
		/// Will populate values based on the contents of the data row.
		/// </summary>
		/// <param name="DetailRow">Datarow containing base values</param>
		/// <remarks>
		/// Class should be inherited to extend to your project specific properties
		/// </remarks>
		public MAccountProfile(DataRow DetailRow)
		{
			this.Initialize(ref DetailRow);
		}

		/// <summary>
		/// Will populate values based on the contents of the data row.
		/// Also populates the roles and gropus properties.
		/// </summary>
		/// <param name="DetailRow">DataRow containing base values</param>
		/// <param name="AssignedRolesData">DataRow containing Role data</param>
		/// <param name="AssignedGroupsData">DataRow containing Group data</param>
		/// <param name="DerivedRolesData">DataRow containing Role data derived from both assigned roles and groups.></param>
		/// <remarks>
		/// Class should be inherited to extend to your project specific properties
		/// </remarks>
		public MAccountProfile(DataRow DetailRow, DataTable AssignedRolesData, DataTable AssignedGroupsData, DataTable DerivedRolesData)
		{
			this.Initialize(ref DetailRow);
			setRolesOrGroups(ref m_AssignedRoles, AssignedRolesData.Rows, m_RoleColumn);
			setRolesOrGroups(ref m_AssignedGroups, AssignedGroupsData.Rows, m_GroupColumn);
			setRolesOrGroups(ref m_DerivedRoles, DerivedRolesData.Rows, m_RoleColumn);
		}

		/// <summary>
		/// Will set the collection of roles given a comma seporated string of roles.
		/// </summary>
		/// <param name="CommaSeporatedRoles">String of comma seporated roles</param>
		public void SetRoles(string CommaSeporatedRoles)
		{
			setRolesOrGroups(ref m_AssignedRoles, ref CommaSeporatedRoles);
		}

		/// <summary>
		/// Will set the collection of groups given a comma seporated string of groups.
		/// </summary>
		/// <param name="CommaSeporatedGroups">String of comma seporated groups</param>
		public void SetGroups(string CommaSeporatedGroups)
		{
			setRolesOrGroups(ref m_AssignedGroups, ref CommaSeporatedGroups);
		}

		/// <summary>
		/// Converts the collection of AssignedRoles to a comma seporated string.
		/// </summary>
		/// <returns>String</returns>
		public string GetCommaSeporatedAssingedRoles()
		{
			return this.getCommaSeportatedString(m_AssignedRoles);
		}

		/// <summary>
		/// Converts the collection of AssignedGroups to a comma seporated string.
		/// </summary>
		/// <returns>String</returns>
		public string GetCommaSeporatedAssignedGroups()
		{
			return this.getCommaSeportatedString(m_AssignedGroups);
		}

		/// <summary>
		/// Converts the collection of DerivedRoles to a comma seporated string.
		/// </summary>
		/// <returns>String</returns>
		public string GetCommaSeporatedDerivedRoles()
		{
			return this.getCommaSeportatedString(m_DerivedRoles);
		}
#endregion

#region "Public Properties"

		/// <summary>
		/// Represents the roles that have been directly assigned to the account.
		/// </summary>
		public Collection<String> AssignedRoles
		{
			get
			{
				return m_AssignedRoles;
			}
		}

		/// <summary>
		/// Represents the groups that have been directly assigned to the account.
		/// </summary>
		public Collection<String> AssignedGroups
		{
			get
			{
				return m_AssignedGroups;
			}
		}

		/// <summary>
		/// Represents the roles that have been assigned either directly or through assoication of a role to a group.
		/// </summary>
		public Collection<String> DerivedRoles
		{
			get
			{
				return m_DerivedRoles;
			}
		}

		/// <summary>
		/// Represents the account
		/// </summary>
		public string Account { get; set; }

		/// <summary>
		/// Represents the email address
		/// </summary>
		public string EMail { get; set; }

		/// <summary>
		/// Used to determine if the client would like to recieve notifications.
		/// </summary>
		public bool EnableNotifications{ get; set; }

		/// <summary>
		/// Represents the status of the account
		/// </summary>
		public int Status { get; set; }

		/// <summary>
		/// Indicates the last time the account password was changed
		/// </summary>
		public DateTime PasswordLastSet { get; set; }

		/// <summary>
		/// The password for the account
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// The number of failed logon attemps
		/// </summary>
		public int FailedAttempts { get; set; }

		/// <summary>
		/// First name of the person for the account
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Indicates if the account is a system administrator ... used to
		/// prevent complete lockout when the roles have been
		/// damaged.
		/// </summary>
		public bool IsSystemAdmin { get; set; }

		/// <summary>
		/// Last name of the person for the account
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Middle name of the person for the account
		/// </summary>
		public string MiddleName { get; set; }

		/// <summary>
		/// Prefered or nick name of the person for the account
		/// </summary>
		public string PreferedName { get; set; }

		/// <summary>
		/// The timezone for the account
		/// </summary>
		public int TimeZone { get; set; }

		/// <summary>
		/// The location of the account
		/// </summary>
		public string Location { get; set; }

		/// <summary>
		/// The date and time the account was last loged on
		/// </summary>
		public DateTime LastLogin { get; set; }

		/// <summary>
		/// Used to determine if the client would like to recieve notifications.
		/// </summary>
		public bool Enable_Notifications { get; set; }
#endregion

#region Private Methods
		private string[] splitRoles(DataRow[] SecurityRoles)
		{
			ArrayList colRoles = new ArrayList();
			DataRow row = null;
			foreach (DataRow row_loopVariable in SecurityRoles)
			{
				row = row_loopVariable;
				if (!Convert.IsDBNull(row[m_RoleColumn]))
				{
					colRoles.Add(row[m_RoleColumn]);
				}
			}
			return (string[])colRoles.ToArray(typeof(string));
		}
#endregion
	}
}
