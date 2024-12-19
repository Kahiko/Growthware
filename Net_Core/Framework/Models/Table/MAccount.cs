using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Interfaces;
using GrowthWare.Framework.Models.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace GrowthWare.Framework.Models;
/// <summary>
/// Properties for an account.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MAccountProfile : AAddedUpdated, IGroupRoleSecurity
{

    #region "Member fields"
    private Collection<string> m_AssignedRoles = new Collection<string>();
    private Collection<string> m_AssignedGroups = new Collection<string>();
    private Collection<string> m_DerivedRoles = new Collection<string>();
    private List<MRefreshToken> m_RefreshTokens = new List<MRefreshToken>();
    private static String s_RoleColumn = "Roles";
    private static String s_GroupColumn = "Groups";
    #endregion

    #region Private Methods
    private static void setRefreshTokens(ref List<MRefreshToken> refreshTokens, DataTable refreshTokenData)
    {
        refreshTokens = new List<MRefreshToken>();
        foreach (DataRow row in refreshTokenData.Rows)
        {
            MRefreshToken mRefreshToken = new MRefreshToken(row);
            refreshTokens.Add(mRefreshToken);
        }
    }

    /// <summary>
    /// Sets the assigned roles or groups.
    /// </summary>
    /// <param name="collectionOfStrings">The collection of roles or groups that need to be set</param>
    /// <param name="groupsOrRoles">The DataRowCollection that represents either roles or groups</param>
    /// <param name="columnName">The column name to retrieve the data from</param>
    private static void setRolesOrGroups(ref Collection<string> collectionOfStrings, DataRowCollection groupsOrRoles, String columnName)
    {
        foreach (DataRow mRow in groupsOrRoles)
        {
            if (!Convert.IsDBNull(mRow[columnName]))
            {
                collectionOfStrings.Add(mRow[columnName].ToString());
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

    private static string getCommaSeparatedString(Collection<string> CollectionOfStrings)
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
        // base.NameColumnName = "ACCT";
        // base.IdColumnName = "ACCT_SEQ_ID";
        base.Initialize(dataRow);
        Account = base.GetString(dataRow, "ACCT");
        this.Email = base.GetString(dataRow, "EMAIL");
        this.EnableNotifications = base.GetBool(dataRow, "ENABLE_NOTIFICATIONS");
        this.Id = base.GetInt(dataRow, "ACCT_SEQ_ID");
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
        this.Token = base.GetString(dataRow, "Token");
        this.TimeZone = base.GetInt(dataRow, "TIME_ZONE");
        this.VerificationToken = base.GetString(dataRow, "VerificationToken");
    }

    protected override void SetupClass()
    {
        base.m_ForeignKeyName = "NOT_USED";
        base.m_IsForeignKeyNumeric = true;
        m_TableName = "[ZGWSecurity].[Accounts]";
    }
    #endregion

    #region "Public Methods"

    /// <summary>
    /// Provides a new account profile with the default values
    /// </summary>
    /// <remarks></remarks>
    public MAccountProfile()
    {
        SetupClass();
    }

    public MAccountProfile(int requestingAccountId)
    {
        SetupClass();
        Collection<string> mDefaultRoles = new Collection<string>(){"Authenticated"};
        this.AddedBy = requestingAccountId;
        this.AddedDate = DateTime.Now;
        this.AssignedRoles = mDefaultRoles;
        this.PasswordLastSet = DateTime.Now.AddYears(-22);
        this.LastLogOn = DateTime.Now.AddYears(-22);
        this.UpdatedDate = DateTime.Now.AddYears(-22);
        this.Status = (int)SystemStatus.ChangePassword;
        this.Location = "";
        this.TimeZone = -8;
    }

    /// <summary>
    /// Provides a new account profile with the same values as the account profile
    /// </summary>
    /// <param name="accountProfile">MAccountProfile</param>
    /// <remarks>Does not set AssignedRoles, AssignedGroups or IsSystemAdmin</remarks>
    public MAccountProfile(MAccountProfile accountProfile)
    {
        SetupClass();
        // base.NameColumnName = "ACCT";
        // base.IdColumnName = "ACCT_SEQ_ID";
        this.Id = -1;
        this.Account = accountProfile.Account;
        this.Email = accountProfile.Email;
        this.EnableNotifications = accountProfile.EnableNotifications;
        this.FirstName = accountProfile.FirstName;
        // this.IsSystemAdmin needs to be set by the caller
        this.LastName = accountProfile.LastName;
        this.Location = accountProfile.Location;
        this.MiddleName = accountProfile.MiddleName;
        this.Name = accountProfile.Account;
        this.PreferredName = accountProfile.PreferredName;
        this.Status = accountProfile.Status;
        this.TimeZone = accountProfile.TimeZone;
    }

    /// <summary>
    /// Will populate values based on the contents of the data row.
    /// </summary>
    /// <param name="detailRow">DataRow containing base values</param>
    /// <remarks>
    /// Class should be inherited to extend to your project specific properties
    /// </remarks>
    public MAccountProfile(DataRow detailRow)
    {
        SetupClass();
        this.Initialize(detailRow);
    }

    /// <summary>
    /// Will populate values based on the contents of the data row.
    /// Also populates the roles and groups properties.
    /// </summary>
    /// <param name="detailRow">DataRow containing base values</param>
    /// <param name="assignedRolesData">DataRow containing Role data</param>
    /// <param name="assignedGroupsData">DataRow containing Group data</param>
    /// <param name="derivedRolesData">DataRow containing Role data derived from both assigned roles and groups.></param>
    /// <remarks>
    /// Class should be inherited to extend to your project specific properties
    /// </remarks>
    public MAccountProfile(DataRow detailRow, DataTable refreshTokens, DataTable assignedRolesData, DataTable assignedGroupsData, DataTable derivedRolesData)
    {
        SetupClass();
        if (detailRow != null)
        {
            this.Initialize(detailRow);
            if (refreshTokens != null) setRefreshTokens(ref m_RefreshTokens, refreshTokens);
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
    [DBIgnoreProperty]
    public Collection<String> AssignedRoles
    {
        get
        {
            return m_AssignedRoles;
        }
        set
        {
           if(value != null) { this.m_AssignedRoles = value; }
        }
    }

    /// <summary>
    /// Represents the groups that have been directly assigned to the account.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<String> Groups
    {
        get
        {
            return m_AssignedGroups;
        }
        set
        {
            this.m_AssignedGroups = value;
        }
    }

    /// <summary>
    /// Represents the roles that have been assigned either directly or through association of a role to a group.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<String> DerivedRoles
    {
        get
        {
            return m_DerivedRoles;
        }
        set 
        {
            m_DerivedRoles = value;   
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
    /// Used to determine if the client would like to receive notifications.
    /// </summary>
    [DBColumnName("Enable_Notifications")]
    public bool EnableNotifications { get; set; }

    [DBPrimaryKey]
    [DBColumnName("AccountSeqId")]
    public int Id { get; set; }

    /// <summary>
    /// Represents the status of the account
    /// </summary>
    [DBColumnName("StatusSeqId")]
    public int Status { get; set; }

    /// <summary>
    /// Indicates the last time the account password was changed
    /// </summary>
    [DBColumnName("Password_Last_Set")]
    public DateTime PasswordLastSet { get; set; }

    /// <summary>
    /// Indicates if the account owns the token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public bool OwnsToken(string token) 
    {
        return this.RefreshTokens?.Find(x => x.Token == token) != null;
    }

    /// <summary>
    /// The password for the account
    /// </summary>
    public String Password { get; set; }

    /// <summary>
    /// The number of failed logon attempts
    /// </summary>
    [DBColumnName("Failed_Attempts")]
    public int FailedAttempts { get; set; }

    /// <summary>
    /// First name of the person for the account
    /// </summary>
    [DBColumnName("First_Name")]
    public String FirstName { get; set; }

    /// <summary>
    /// Indicates if the account is a system administrator ... used to
    /// prevent complete lockout when the roles have been
    /// damaged.
    /// </summary>
    [DBColumnName("Is_System_Admin")]
    public bool IsSystemAdmin { get; set; }

    /// <summary>
    /// Last name of the person for the account
    /// </summary>
    [DBColumnName("Last_Name")]
    public String LastName { get; set; }

    /// <summary>
    /// Middle name of the person for the account
    /// </summary>
    [DBColumnName("Middle_Name")]
    public String MiddleName { get; set; }

    [DBIgnoreProperty]
    [DBColumnName("Account")]
    public string Name { get; set; }

    /// <summary>
    /// Preferred or nick name of the person for the account
    /// </summary>
    [DBColumnName("Preferred_Name")]
    public String PreferredName { get; set; }

    /// <summary>
    /// A list of refresh tokens
    /// </summary>
    /// <value></value>
    [DBIgnoreProperty]
    public List<MRefreshToken> RefreshTokens 
    {
        get{ return m_RefreshTokens; }
        set{ m_RefreshTokens = value; }
    }

    /// <summary>
    /// The reset token
    /// </summary>
    /// <value></value>
    public string ResetToken { get; set; }
    
    /// <summary>
    /// Indicates when the reset token expires
    /// </summary>
    /// <value></value>
    public DateTime? ResetTokenExpires { get; set; }

    /// <summary>
    /// The timezone for the account
    /// </summary>
    [DBColumnName("Time_Zone")]
    public int TimeZone { get; set; }

    /// <summary>
    /// Used in new AuthenticationResponse to set the JwtToken
    /// </summary>
    [DBIgnoreProperty]
    public string Token { get; set; }

    /// <summary>
    /// The verification token
    /// </summary>
    /// <value></value>
    public string VerificationToken{ get; set ;}

    /// <summary>
    /// The location of the account
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// The date and time the account was last logged on
    /// </summary>
    [DBColumnName("Last_Login")]
    public DateTime LastLogOn { get; set; }

    /// <summary>
    /// Converts the collection of AssignedRoles to a comma Separated string.
    /// </summary>
    /// <returns>String</returns>
    [DBIgnoreProperty]
    public String GetCommaSeparatedAssignedRoles
    {
        get { return getCommaSeparatedString(m_AssignedRoles); }
    }

    /// <summary>
    /// Converts the collection of AssignedGroups to a comma Separated string.
    /// </summary>
    /// <returns>String</returns>
    [DBIgnoreProperty]
    public String GetCommaSeparatedAssignedGroups
    {
        get { return getCommaSeparatedString(m_AssignedGroups); }
    }

    /// <summary>
    /// Converts the collection of DerivedRoles to a comma Separated string.
    /// </summary>
    /// <returns>String</returns>
    [DBIgnoreProperty]
    public String GetCommaSeparatedDerivedRoles
    {
        get { return getCommaSeparatedString(m_DerivedRoles); }
    }
    #endregion

}

