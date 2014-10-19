using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Utilities;
using GrowthWare.WebSupport;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles.Base;

namespace GrowthWare.WebApplication.Controllers
{
public class AccountsController : ApiController
{

	[HttpGet()]
	public MUIAccountChoices GetPreferences()
	{
		MClientChoicesState mClientChoicesState = ClientChoicesUtility.GetClientChoicesState(AccountUtility.CurrentProfile().Account.ToString(CultureInfo.InvariantCulture));
		MUIAccountChoices mRetVal = new MUIAccountChoices(mClientChoicesState);
		mRetVal.Environment = GWWebHelper.DisplayEnvironment;
		mRetVal.Version = GWWebHelper.Version;
		return mRetVal;
	}

	[HttpPost()]
	public IHttpActionResult Logon(LogonInfo jsonData)
	{
		if (jsonData == null)
            throw new ArgumentNullException("jsonData", "jsonData cannot be a null reference (Nothing in Visual Basic)!");
		if (string.IsNullOrEmpty(jsonData.Account))
			throw new NullReferenceException("jsonData.Account cannot be a null reference (Nothing in Visual Basic)!");
		if (string.IsNullOrEmpty(jsonData.Password))
			throw new NullReferenceException("jsonData.Password cannot be a null reference (Nothing in Visual Basic)!");
		string mRetVal = "false";
		bool mDomainPassed = false;
		if (jsonData.Account.Contains("\\")) {
			mDomainPassed = true;
		}
		if (ConfigSettings.AuthenticationType.ToUpper() == "LDAP" & !mDomainPassed) {
			jsonData.Account = ConfigSettings.LdapDomain + "\\" + jsonData.Account;
		}
		if (AccountUtility.Authenticated(jsonData.Account, jsonData.Password)) {
			MAccountProfile mAccountProfile = AccountUtility.GetProfile(jsonData.Account);
			AccountUtility.SetPrincipal(mAccountProfile);
			mRetVal = "true";
		} else {
			MAccountProfile mAccountProfile = AccountUtility.GetProfile(jsonData.Account);
			if (mAccountProfile != null) {
				if (mAccountProfile.Account.ToUpper(new CultureInfo("en-US", false)) == jsonData.Account.ToUpper(new CultureInfo("en-US", false))) {
					if (ConfigSettings.AuthenticationType.ToUpper() == "INTERNAL") {
						mRetVal = "Request";
					} else {
						MMessageProfile mMessageProfile = MessageUtility.GetProfile("Logon Error");
						if (mMessageProfile != null) {
							mRetVal = mMessageProfile.Body;
						}
					}
				}
			} else {
				MMessageProfile mMessageProfile = MessageUtility.GetProfile("Logon Error");
				if (mMessageProfile != null) {
					mRetVal = mMessageProfile.Body;
				}
			}
		}

		return Ok(mRetVal);
	}

	//Public Function ChangePassword(<FromBody()> ByVal oldPassword As String, <FromBody()> ByVal newPassword As String) As IHttpActionResult

	[HttpPost()]
	public IHttpActionResult ChangePassword(MChangePassword mChangePassword)
	{
		if (mChangePassword == null)
			throw new ArgumentNullException("mChangePassword", "mChangePassword cannot be a null reference (Nothing in Visual Basic)!");
		MMessageProfile mMessageProfile = new MMessageProfile();
		MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
		MAccountProfile mAccountProfile = AccountUtility.CurrentProfile();
		dynamic mCurrentPassword = "";
		mMessageProfile = MessageUtility.GetProfile("SuccessChangePassword");
		try {
			mCurrentPassword = CryptoUtility.Decrypt(mAccountProfile.Password, mSecurityEntityProfile.EncryptionType);
		} catch (Exception ex) {
			mCurrentPassword = mAccountProfile.Password;
		}
		if (mAccountProfile.Status != (int)SystemStatus.ChangePassword) {
			if (mChangePassword.OldPassword != mCurrentPassword) {
				mMessageProfile = MessageUtility.GetProfile("PasswordNotMatched");
			} else {
				var _with1 = mAccountProfile;
				_with1.PasswordLastSet = System.DateTime.Now;
				_with1.Status = (int)SystemStatus.Active;
				_with1.FailedAttempts = 0;
				_with1.Password = CryptoUtility.Encrypt(mChangePassword.NewPassword.Trim(), mSecurityEntityProfile.EncryptionType);
				try {
					AccountUtility.Save(mAccountProfile, false, false);
				} catch (Exception ex) {
					mMessageProfile = MessageUtility.GetProfile("UnSuccessChangePassword");
				}
			}
		} else {
			try {
				var _with2 = mAccountProfile;
				_with2.PasswordLastSet = System.DateTime.Now;
				_with2.Status = (int)SystemStatus.Active;
				_with2.FailedAttempts = 0;
				_with2.Password = CryptoUtility.Encrypt(mChangePassword.NewPassword.Trim(), mSecurityEntityProfile.EncryptionType);
				AccountUtility.Save(mAccountProfile, false, false);
			} catch (Exception ex) {
				mMessageProfile = MessageUtility.GetProfile("UnSuccessChangePassword");
			}
		}
		AccountUtility.RemoveInMemoryInformation(true);
		return Ok(mMessageProfile.Body);
	}

}

public class MChangePassword
{
	public string OldPassword { get; set; }
	public string NewPassword { get; set; }
}

public class LogonInfo
{
	public string Account { get; set; }
	public string Password { get; set; }
}

/// <summary>
/// Class MUIAccountChoices
/// </summary>
public class MUIAccountChoices : MProfile
{

    /// <summary>
    /// Initializes a new instance of the <see cref="MUIAccountChoices"/> class.
    /// </summary>

    public MUIAccountChoices()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MUIAccountChoices"/> class.
    /// </summary>
    /// <param name="clientChoicesState">State of the client choices.</param>
    public MUIAccountChoices(MClientChoicesState clientChoicesState)
    {
        if (clientChoicesState[MClientChoices.AccountName] != null)
            AccountName = clientChoicesState[MClientChoices.AccountName].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.Action] != null)
            Action = clientChoicesState[MClientChoices.Action].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.BackColor] != null)
            BackColor = clientChoicesState[MClientChoices.BackColor].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.ColorScheme] != null)
            ColorScheme = clientChoicesState[MClientChoices.ColorScheme].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.HeadColor] != null)
            HeadColor = clientChoicesState[MClientChoices.HeadColor].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.LeftColor] != null)
            LeftColor = clientChoicesState[MClientChoices.LeftColor].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.RecordsPerPage] != null)
            RecordsPerPage = int.Parse(clientChoicesState[MClientChoices.RecordsPerPage].ToString(CultureInfo.InvariantCulture));
        if (clientChoicesState[MClientChoices.SecurityEntityId] != null)
            SecurityEntityID = int.Parse(clientChoicesState[MClientChoices.SecurityEntityId].ToString(CultureInfo.InvariantCulture));
        if (clientChoicesState[MClientChoices.SecurityEntityName] != null)
            SecurityEntityName = clientChoicesState[MClientChoices.SecurityEntityName].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.SubheadColor] != null)
            SubheadColor = clientChoicesState[MClientChoices.SubheadColor].ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets or sets the name of the account.
    /// </summary>
    /// <value>The name of the account.</value>
    public string AccountName { get; set; }

    /// <summary>
    /// Gets or sets the action.
    /// </summary>
    /// <value>The action.</value>
    public string Action { get; set; }

    /// <summary>
    /// Gets or sets the color of the back.
    /// </summary>
    /// <value>The color of the back.</value>
    public string BackColor { get; set; }

    /// <summary>
    /// Gets or sets the color scheme.
    /// </summary>
    /// <value>The color scheme.</value>
    public string ColorScheme { get; set; }

    /// <summary>
    /// Gets or sets the environment.
    /// </summary>
    /// <value>The environment.</value>
    public string Environment { get; set; }

    /// <summary>
    /// Gets or sets the color of the head.
    /// </summary>
    /// <value>The color of the head.</value>
    public string HeadColor { get; set; }

    /// <summary>
    /// Gets or sets the color of the header foreground color.
    /// </summary>
    /// <value>The color of the header foreground color.</value>
    public string HeaderForeColor { get; set; }

    /// <summary>
    /// Gets or sets the color of the left.
    /// </summary>
    /// <value>The color of the left.</value>
    public string LeftColor { get; set; }

    /// <summary>
    /// Gets or sets the records per page.
    /// </summary>
    /// <value>The records per page.</value>
    public int RecordsPerPage { get; set; }

    /// <summary>
    /// Gets or sets the account.
    /// </summary>
    /// <value>The account.</value>
    public string Account { get; set; }

    /// <summary>
    /// Gets or sets the security entity ID.
    /// </summary>
    /// <value>The security entity ID.</value>
    public int SecurityEntityID { get; set; }

    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>The version.</value>
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the name of the security entity.
    /// </summary>
    /// <value>The name of the security entity.</value>
    public string SecurityEntityName { get; set; }

    /// <summary>
    /// Gets or sets the color of the subhead.
    /// </summary>
    /// <value>The color of the subhead.</value>
    public string SubheadColor { get; set; }

    /// <summary>
    /// Gets or sets the color of the row background color.
    /// </summary>
    /// <value>The color of the row background color.</value>
    public string RowBackColor { get; set; }

    /// <summary>
    /// Gets or sets the color of the alternating row background color.
    /// </summary>
    /// <value>The color of the alternating row background color.</value>
    public string AlternatingRowBackColor { get; set; }
}

    public class UIAccountProfile
    {
	    public string Account;
	    public int Id;
	    public bool EnableNotifications;
	    public string EMail;
	    public int Status;
	    public string FirstName;
	    public string MiddleName;
	    public string LastName;
	    public string PreferredName;
	    public bool IsSystemAdmin;
	    public int TimeZone;
	    public string Location;
    }

    public class UIAccountRoles
    {
	    public string[] Roles;
    }

    public class UIAccountGroups
    {
	    public string[] Groups;
    }
}