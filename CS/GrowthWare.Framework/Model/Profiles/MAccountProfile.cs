using GrowthWare.Framework.Model.Profiles.Base;
using GrowthWare.Framework.Model.Profiles.Interfaces;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;


namespace GrowthWare.Framework.Model.Profiles
{
    /// <summary>
    /// Properties for an account.
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MAccountProfile : MProfile, IMGroupRoleSecurity
    {

        #region "Member fields"
        private Collection<string> m_AssignedRoles = new Collection<string>();
        private Collection<string> m_AssignedGroups = new Collection<string>();
        private Collection<string> m_DerivedRoles = new Collection<string>();
        private static String s_RoleColumn = "Roles";
        private static String s_GroupColumn = "Groups";
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the assigned roles or groups.
        /// </summary>
        /// <param name="StringCollectionObject">The collection of roles or groups that need to be set</param>
        /// <param name="GroupsOrRoles">The DataRowCollection that represents either roles or groups</param>
        /// <param name="ColumnName">The column name to retrieve the data from</param>
        private static void setRolesOrGroups(ref Collection<string> StringCollectionObject, DataRowCollection GroupsOrRoles, String ColumnName)
        {
            foreach (DataRow mRow in GroupsOrRoles)
            {
                if (!Convert.IsDBNull(mRow[ColumnName]))
                {
                    StringCollectionObject.Add(mRow[ColumnName].ToString());
                }
            }
        }

        /// <summary>
        /// Sets the assigned roles or groups.
        /// </summary>
        /// <param name="StringCollectionObject">The collection of roles or groups that need to be set</param>
        /// <param name="CommaSeparatedString">A comma Separated list of roles or groups 'you, me' as an example</param>
        private static void setRolesOrGroups(ref Collection<String> StringCollectionObject, ref String CommaSeparatedString)
        {
            StringCollectionObject = new Collection<string>();
            string[] mRoles = CommaSeparatedString.Split(',');
            foreach (var mRole in mRoles)
            {
                StringCollectionObject.Add(mRole.ToString());
            }
        }

        private static string getCommaSeportatedString(Collection<string> CollectionOfStrings)
        {
            String mRetValue = String.Empty;
            if (CollectionOfStrings != null)
            {
                if (CollectionOfStrings.Count > 0)
                {
                    foreach (var item in CollectionOfStrings)
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
        #endregion

        #region "Protected Methods"
        /// <summary>
        /// Populates direct properties as well as passing the DataRow to the abstract class
        /// for the population of the base properties.
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        protected new void Initialize(DataRow dataRow)
        {
            base.NameColumnName = "ACCT";
            base.IdColumnName = "ACCT_SEQ_ID";
            base.Initialize(dataRow);
            Account = base.Name;
            this.Email = base.GetString(dataRow, "EMAIL");
            this.EnableNotifications = base.GetBool(dataRow, "ENABLE_NOTIFICATIONS");
            this.IsSystemAdmin = base.GetBool(dataRow, "IS_SYSTEM_ADMIN");
            this.Status = base.GetInt(dataRow, "STATUS_SEQ_ID");
            this.Password = base.GetString(dataRow, "PWD");
            this.FailedAttempts = base.GetInt(dataRow, "FAILED_ATTEMPTS");
            this.FirstName = base.GetString(dataRow, "FIRST_NAME");
            this.LastLogOn = base.GetDateTime(dataRow, "LAST_LOGIN", DateTime.Now);
            this.LastName = base.GetString(dataRow, "LAST_NAME");
            this.Location = base.GetString(dataRow, "LOCATION");
            this.PasswordLastSet = base.GetDateTime(dataRow, "PASSWORD_LAST_SET", DateTime.Now);
            this.MiddleName = base.GetString(dataRow, "MIDDLE_NAME");
            this.PreferredName = base.GetString(dataRow, "PREFERRED_NAME");
            this.TimeZone = base.GetInt(dataRow, "TIME_ZONE");
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
        /// <param name="detailRow">Datarow containing base values</param>
        /// <remarks>
        /// Class should be inherited to extend to your project specific properties
        /// </remarks>
        public MAccountProfile(DataRow detailRow)
        {
            this.Initialize(detailRow);
        }

        /// <summary>
        /// Will populate values based on the contents of the data row.
        /// Also populates the roles and gropus properties.
        /// </summary>
        /// <param name="detailRow">DataRow containing base values</param>
        /// <param name="assignedRolesData">DataRow containing Role data</param>
        /// <param name="assignedGroupsData">DataRow containing Group data</param>
        /// <param name="derivedRolesData">DataRow containing Role data derived from both assigned roles and groups.></param>
        /// <remarks>
        /// Class should be inherited to extend to your project specific properties
        /// </remarks>
        public MAccountProfile(DataRow detailRow, DataTable assignedRolesData, DataTable assignedGroupsData, DataTable derivedRolesData)
        {
            if (detailRow != null) 
            {
                this.Initialize(detailRow);
                if (assignedRolesData != null) setRolesOrGroups(ref m_AssignedRoles, assignedRolesData.Rows, s_RoleColumn);
                if (assignedGroupsData != null) setRolesOrGroups(ref m_AssignedGroups, assignedGroupsData.Rows, s_GroupColumn);
                if (derivedRolesData != null) setRolesOrGroups(ref m_DerivedRoles, derivedRolesData.Rows, s_RoleColumn);
            }
        }

        /// <summary>
        /// Will set the collection of roles given a comma Separated string of roles.
        /// </summary>
        /// <param name="commaSeparatedRoles">String of comma Separated roles</param>
        public void SetRoles(string commaSeparatedRoles)
        {
            setRolesOrGroups(ref m_AssignedRoles, ref commaSeparatedRoles);
        }

        /// <summary>
        /// Will set the collection of groups given a comma Separated string of groups.
        /// </summary>
        /// <param name="commaSeparatedGroups">String of comma Separated groups</param>
        public void SetGroups(string commaSeparatedGroups)
        {
            setRolesOrGroups(ref m_AssignedGroups, ref commaSeparatedGroups);
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
        public Collection<String> Groups
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
        public String Account { get; set; }

        /// <summary>
        /// Represents the email address
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// Used to determine if the client would like to recieve notifications.
        /// </summary>
        public bool EnableNotifications { get; set; }

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
        public String Password { get; set; }

        /// <summary>
        /// The number of failed logon attemps
        /// </summary>
        public int FailedAttempts { get; set; }

        /// <summary>
        /// First name of the person for the account
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// Indicates if the account is a system administrator ... used to
        /// prevent complete lockout when the roles have been
        /// damaged.
        /// </summary>
        public bool IsSystemAdmin { get; set; }

        /// <summary>
        /// Last name of the person for the account
        /// </summary>
        public String LastName { get; set; }

        /// <summary>
        /// Middle name of the person for the account
        /// </summary>
        public String MiddleName { get; set; }

        /// <summary>
        /// Prefered or nick name of the person for the account
        /// </summary>
        public String PreferredName { get; set; }

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
        public DateTime LastLogOn { get; set; }

        /// <summary>
        /// Converts the collection of AssignedRoles to a comma Separated string.
        /// </summary>
        /// <returns>String</returns>
        public String GetCommaSeparatedAssignedRoles
        {
            get { return getCommaSeportatedString(m_AssignedRoles); }
        }

        /// <summary>
        /// Converts the collection of AssignedGroups to a comma Separated string.
        /// </summary>
        /// <returns>String</returns>
        public String GetCommaSeparatedAssignedGroups
        {
            get { return getCommaSeportatedString(m_AssignedGroups); }
        }

        /// <summary>
        /// Converts the collection of DerivedRoles to a comma Separated string.
        /// </summary>
        /// <returns>String</returns>
        public String GetCommaSeparatedDerivedRoles
        {
            get { return getCommaSeportatedString(m_DerivedRoles); }
        }
        #endregion

    }
}
