using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Model.Profiles.Base;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Utilities;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Linq;

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
        mRetVal.FrameWorkVersion = GWWebHelper.FrameworkVersion;
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
            mAccountProfile.LastLogOn = DateTime.Now;
            if(mAccountProfile.Status == Convert.ToInt32(SystemStatus.Disabled)) mAccountProfile.Status = Convert.ToInt32(SystemStatus.Active);
            mAccountProfile.FailedAttempts = 0;
            AccountUtility.Save(mAccountProfile, false, false);
			AccountUtility.SetPrincipal(mAccountProfile);
			mRetVal = "true";
		} else {
			MAccountProfile mAccountProfile = AccountUtility.GetProfile(jsonData.Account);
			if (mAccountProfile != null) {
				if (mAccountProfile.Account.ToUpper(new CultureInfo("en-US", false)) == jsonData.Account.ToUpper(new CultureInfo("en-US", false))) {
					if (ConfigSettings.AuthenticationType.ToUpper() == "INTERNAL") {
                        if (mAccountProfile.Status != Convert.ToInt32(SystemStatus.Disabled) || mAccountProfile.Status != Convert.ToInt32(SystemStatus.Inactive))
                        {
                            mRetVal = "Request";
                        }
                        else 
                        {
                            MMessageProfile mMessageProfile = MessageUtility.GetProfile("DisabledAccount");
                            if (mMessageProfile != null)
                            {
                                mRetVal = mMessageProfile.Body;
                            }                      
                        }
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

	[HttpPost()]
	public IHttpActionResult ChangePassword(MChangePassword mChangePassword)
	{
		if (mChangePassword == null) throw new ArgumentNullException("mChangePassword", "mChangePassword cannot be a null reference (Nothing in Visual Basic)!");
		MMessageProfile mMessageProfile = new MMessageProfile();
		MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
		MAccountProfile mAccountProfile = AccountUtility.CurrentProfile();
		string mCurrentPassword = "";
		mMessageProfile = MessageUtility.GetProfile("SuccessChangePassword");
		try {
			mCurrentPassword = CryptoUtility.Decrypt(mAccountProfile.Password, mSecurityEntityProfile.EncryptionType);
		} catch (Exception) {
			mCurrentPassword = mAccountProfile.Password;
		}
		if (mAccountProfile.Status != (int)SystemStatus.ChangePassword) {
			if (mChangePassword.OldPassword != mCurrentPassword) {
				mMessageProfile = MessageUtility.GetProfile("PasswordNotMatched");
			} else {
                mAccountProfile.PasswordLastSet = System.DateTime.Now;
                mAccountProfile.Status = (int)SystemStatus.Active;
                mAccountProfile.FailedAttempts = 0;
                mAccountProfile.Password = CryptoUtility.Encrypt(mChangePassword.NewPassword.Trim(), mSecurityEntityProfile.EncryptionType);
				try {
					AccountUtility.Save(mAccountProfile, false, false);
				} catch (Exception) {
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
			} catch (Exception) {
				mMessageProfile = MessageUtility.GetProfile("UnSuccessChangePassword");
			}
		}
		AccountUtility.RemoveInMemoryInformation(true);
		return Ok(mMessageProfile.Body);
	}

    [HttpPost()]
    public IHttpActionResult Save(UIAccountProfile uiProfile) 
    {
        if (uiProfile == null) throw new ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!");
        string mRetVal = "False";
        bool mSaveGroups = false;
        bool mSaveRoles = false;
        MAccountProfile mCurrentAccountProfile = AccountUtility.CurrentProfile();
        MAccountProfile mAccountProfileToSave = new MAccountProfile();
        Logger mLog = Logger.Instance();
        if (HttpContext.Current.Request.QueryString["Action"].ToString().ToUpper(CultureInfo.InvariantCulture).IndexOf("REGISTER") > -1)
        {
            MAccountProfile mExistingAccount = AccountUtility.GetProfile(uiProfile.Account);
            if (mExistingAccount == null)
            {
                String mGroups = ConfigSettings.RegistrationGroups;
                String mRoles = ConfigSettings.RegistrationRoles;
                mAccountProfileToSave = populateAccountProfile(uiProfile, mAccountProfileToSave);
                mAccountProfileToSave.Id = uiProfile.Id;
                mAccountProfileToSave.AddedBy = mCurrentAccountProfile.Id;
                mAccountProfileToSave.AddedDate = DateTime.Now;
                mAccountProfileToSave.SetGroups(mGroups);
                mAccountProfileToSave.SetRoles(mRoles);
                mAccountProfileToSave.PasswordLastSet = DateTime.Now;
                mAccountProfileToSave.LastLogOn = DateTime.Now;
                mAccountProfileToSave.Password = CryptoUtility.Encrypt(ConfigSettings.RegistrationPassword, ConfigSettings.EncryptionType);
                mAccountProfileToSave.Status = int.Parse(ConfigSettings.RegistrationStatusId);
                if (HttpContext.Current.Request.QueryString["Action"].ToString().ToUpper(CultureInfo.InvariantCulture).IndexOf("REGISTEREXTERNALLOGIN") > -1) mAccountProfileToSave.Status = (int)SystemStatus.Active;
                MClientChoicesState mClientChoiceState = ClientChoicesUtility.GetClientChoicesState(ConfigSettings.RegistrationAccountChoicesAccount, true);
                MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.GetProfile(int.Parse(ConfigSettings.RegistrationSecurityEntityId));

                mClientChoiceState.IsDirty = false;
                mClientChoiceState.AccountName = mAccountProfileToSave.Account;
                mClientChoiceState[MClientChoices.SecurityEntityId] = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture);
                mClientChoiceState[MClientChoices.SecurityEntityName] = mSecurityEntityProfile.Name;
                try
                {
                    AccountUtility.Save(mAccountProfileToSave, mSaveRoles, mSaveGroups, mSecurityEntityProfile);
                    ClientChoicesUtility.Save(mClientChoiceState, false);
                    AccountUtility.SetPrincipal(mAccountProfileToSave);
                    mRetVal = "Your account has been created";
                }
                catch (Exception ex)
                {
                    mLog.Error(ex);
                }
            }
            else 
            {
                mRetVal = "The account '" + uiProfile.Account + "' already exists please choose a different account/email";
            }
        }
        else 
        {
            if (HttpContext.Current.Items["EditId"] != null)
            {
                int mEditId = int.Parse(HttpContext.Current.Items["EditId"].ToString());
                if (mEditId == uiProfile.Id)
                {
                    MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.CurrentProfile(), AccountUtility.CurrentProfile());
                    if (mSecurityInfo != null)
                    {
                        if (mEditId != 1)
                        {
                            if(mCurrentAccountProfile.Id != uiProfile.Id) mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditOtherAccount", true)), mCurrentAccountProfile);
                            if (mSecurityInfo.MayEdit) 
                            {
                                MSecurityInfo mGroupTabSecurity = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_View_Account_Group_Tab", true)), mCurrentAccountProfile);
                                MSecurityInfo mRoleTabSecurity = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_View_Account_Role_Tab", true)), mCurrentAccountProfile);
                                mAccountProfileToSave = AccountUtility.GetProfile(mEditId);
                                mAccountProfileToSave = populateAccountProfile(uiProfile, mAccountProfileToSave);
                                string mGroups = String.Join(",", uiProfile.AccountGroups.Groups);
                                string mRoles = String.Join(",", uiProfile.AccountRoles.Roles);
                                if (mGroupTabSecurity.MayView && FunctionUtility.CurrentProfile().Action.ToLowerInvariant() == ConfigSettings.GetAppSettingValue("Actions_EditOtherAccount", true).ToLower(CultureInfo.InvariantCulture)) 
                                {
                                    if (mAccountProfileToSave.GetCommaSeparatedAssignedGroups != mGroups) 
                                    {
                                        mSaveGroups = true;
                                        mAccountProfileToSave.SetGroups(mGroups);
                                    }
                                }
                                if (mRoleTabSecurity.MayView && FunctionUtility.CurrentProfile().Action.ToLowerInvariant() == ConfigSettings.GetAppSettingValue("Actions_EditOtherAccount", true).ToLower(CultureInfo.InvariantCulture)) 
                                {
                                    if (mAccountProfileToSave.GetCommaSeparatedAssignedRoles != mRoles) 
                                    {
                                        mSaveRoles = true;
                                        mAccountProfileToSave.SetRoles(mRoles);
                                    }
                                }
                                mAccountProfileToSave.AddedBy = mCurrentAccountProfile.Id;
                                mAccountProfileToSave.AddedDate = DateTime.Now;
                                AccountUtility.Save(mAccountProfileToSave, mSaveRoles, mSaveGroups);
                                mLog.Debug("Saved account " + mAccountProfileToSave.Account + " by " + mCurrentAccountProfile.Account);
                                mRetVal = "true";
                            }
                            else
                            {
                                Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to edit");
                                mLog.Error(mError);
                                return this.InternalServerError(mError);
                            }
                        }
                        else 
                        {
                            if (mSecurityInfo.MayAdd)
                            {
                                mSaveRoles = true;
                                mSaveGroups = true;
                                mAccountProfileToSave = populateAccountProfile(uiProfile, mAccountProfileToSave);
                                mAccountProfileToSave.Id = -1;
                                mAccountProfileToSave.AddedBy = mCurrentAccountProfile.Id;
                                mAccountProfileToSave.AddedDate = DateTime.Now;
                                mAccountProfileToSave.UpdatedBy = mAccountProfileToSave.AddedBy;
                                mAccountProfileToSave.UpdatedDate = mAccountProfileToSave.AddedDate;
                                string mGroups = String.Join(",", uiProfile.AccountGroups.Groups);
                                string mRoles = String.Join(",", uiProfile.AccountRoles.Roles);
                                AccountUtility.Save(mAccountProfileToSave, mSaveRoles, mSaveGroups);
                                mLog.Debug("Added account " + mAccountProfileToSave.Account + " by " + mCurrentAccountProfile.Account);
                                mRetVal = "true";
                            }
                            else 
                            {
                                Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to add");
                                mLog.Error(mError);
                                return this.InternalServerError(mError);
                            }
                        }
                    }
                    else 
                    {
                        Exception mError = new Exception("Security Info can not be determined nothing has been saved!!!!");
                        mLog.Error(mError);
                        return this.InternalServerError(mError);            
                    }
                }
                else 
                {
                    Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
                    mLog.Error(mError);
                    return this.InternalServerError(mError);            
                }
            }
        }
        return Ok(mRetVal);
    }

    [HttpPost()]
    public IHttpActionResult SaveClientChoices(MUIAccountChoices choices) 
    {
        if (choices == null) throw new ArgumentNullException("choices", "choices cannot be a null reference (Nothing in Visual Basic)!");
        string mRetVal = "false";
        MClientChoicesState mClientChoicesState = ClientChoicesUtility.GetClientChoicesState(AccountUtility.CurrentProfile().Account);
        mClientChoicesState[MClientChoices.Action] = choices.Action;
        mClientChoicesState[MClientChoices.BackColor] = choices.BackColor;
        mClientChoicesState[MClientChoices.ColorScheme] = choices.ColorScheme;
        mClientChoicesState[MClientChoices.HeadColor] = choices.HeadColor;
        mClientChoicesState[MClientChoices.HeaderForeColor] = choices.HeaderForeColor;
        mClientChoicesState[MClientChoices.RowBackColor] = choices.RowBackColor;
        mClientChoicesState[MClientChoices.AlternatingRowBackColor] = choices.AlternatingRowBackColor;
        mClientChoicesState[MClientChoices.LeftColor] = choices.LeftColor;
        mClientChoicesState[MClientChoices.RecordsPerPage] = choices.RecordsPerPage.ToString();
        mClientChoicesState[MClientChoices.SubheadColor] = choices.SubheadColor;
        ClientChoicesUtility.Save(mClientChoicesState);
        AccountUtility.RemoveInMemoryInformation(true);
        return Ok(mRetVal);
    }

    [HttpPost()]
    public IHttpActionResult Delete([FromUri] int accountSeqId) 
    {
        if (accountSeqId <= 0) throw new ArgumentNullException("accountSeqId", " must be a positive number!");
        string mRetVal = "False";
        Logger mLog = Logger.Instance();
        if (HttpContext.Current.Items["EditId"] != null)
        {
            int mEditId = int.Parse(HttpContext.Current.Items["EditId"].ToString());
            if (mEditId == accountSeqId)
            {
                MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditOtherAccount", true)), AccountUtility.CurrentProfile());
                if (mSecurityInfo != null)
                {
                    if (mSecurityInfo.MayDelete)
                    {
                        try
                        {
                            AccountUtility.Delete(accountSeqId);
                            mRetVal = "True";
                        }
                        catch (Exception ex)
                        {
                            mLog.Error(ex);
                            throw;
                        }
                    }
                    else
                    {
                        Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to delete");
                        mLog.Error(mError);
                        return this.InternalServerError(mError);
                    }
                }
                else
                {
                    Exception mError = new Exception("Security Info can not be determined nothing has been deleted!!!!");
                    mLog.Error(mError);
                    return this.InternalServerError(mError);
                }
            }
            else
            {
                Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
                mLog.Error(mError);
                return this.InternalServerError(mError);
            }
        }
        return Ok(mRetVal);
    }

    [HttpPost()]
    public IHttpActionResult SelectSecurityEntity([FromUri] int selectedSecurityEntityId) 
    {
        MSecurityEntityProfile targetSEProfile = SecurityEntityUtility.GetProfile(selectedSecurityEntityId);
        MSecurityEntityProfile currentSEProfile = SecurityEntityUtility.CurrentProfile();
        MClientChoicesState mClientChoicesState = ClientChoicesUtility.GetClientChoicesState(AccountUtility.CurrentProfile().Account);
        MMessageProfile mMessageProfile = null;
        try
        {
            if (!ConfigSettings.CentralManagement)
            {
                mClientChoicesState[MClientChoices.SecurityEntityId] = targetSEProfile.Id.ToString();
                mClientChoicesState[MClientChoices.SecurityEntityName] = targetSEProfile.Name;
            }
            else
            {
                if (currentSEProfile.ConnectionString == targetSEProfile.ConnectionString)
                {
                    mClientChoicesState[MClientChoices.SecurityEntityId] = targetSEProfile.Id.ToString();
                    mClientChoicesState[MClientChoices.SecurityEntityName] = targetSEProfile.Name;
                }
                else
                {
                    mClientChoicesState[MClientChoices.SecurityEntityId] = ConfigSettings.DefaultSecurityEntityId.ToString();
                    mClientChoicesState[MClientChoices.SecurityEntityName] = "System";
                }
            }
            MMessageProfile myMessageProfile = new MMessageProfile();
            // update all of your in memory information
            AccountUtility.RemoveInMemoryInformation(true);
            ClientChoicesUtility.Save(mClientChoicesState);
            mMessageProfile = MessageUtility.GetProfile("ChangedSelectedSecurityEntity");
        }
        catch (Exception ex)
        {
                Logger mLog= Logger.Instance();
                mMessageProfile = MessageUtility.GetProfile("NoDataFound");
                Exception myEx = new Exception("SelectSecurityEntity:: reported an error.", ex);
                mLog.Error(myEx);
        }
        // refresh the view
        return Ok(mMessageProfile.Body);    
    }

    private MAccountProfile populateAccountProfile(UIAccountProfile uiProfile, MAccountProfile accountProfile) 
    {
        if (accountProfile == null) accountProfile = new MAccountProfile();
        accountProfile.Account = uiProfile.Account;
        accountProfile.Email = uiProfile.EMail;
        accountProfile.EnableNotifications = uiProfile.EnableNotifications;
        accountProfile.FirstName = uiProfile.FirstName;
        if (AccountUtility.CurrentProfile().IsSystemAdmin) 
        {
            accountProfile.IsSystemAdmin = uiProfile.IsSystemAdmin;
        }
        accountProfile.LastName = uiProfile.LastName;
        accountProfile.Location = uiProfile.Location;
        accountProfile.MiddleName = uiProfile.MiddleName;
        accountProfile.Name = uiProfile.Account;
        accountProfile.PreferredName = uiProfile.PreferredName;
        accountProfile.Status = uiProfile.Status;
        accountProfile.TimeZone = uiProfile.TimeZone;
        return accountProfile;
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
    /// Gets or sets the framework version.
    /// </summary>
    /// <value>The version.</value>
    public string FrameWorkVersion { get; set; }
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
        public UIAccountGroups AccountGroups;
        public UIAccountRoles AccountRoles;
        public bool CanSaveRoles;
        public bool CanSaveGroups;
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