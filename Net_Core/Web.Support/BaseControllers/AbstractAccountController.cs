using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.Messages;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Framework.Enumerations;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractAccountController : ControllerBase
{
    private Logger m_Logger = Logger.Instance();

    /// <summary>
    /// Performs an account authentication and handles the token cookie.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("Authenticate")]
    public ActionResult<Tuple<AuthenticationResponse, UIAccountChoices>> Authenticate(string account, string password)
    {
        MAccountProfile mAccountProfile = AccountUtility.Authenticate(account, password, ipAddress());
        AuthenticationResponse mRetVal = new AuthenticationResponse(mAccountProfile);
        setTokenCookie(mRetVal.RefreshToken);
        ClientChoicesUtility.SynchronizeContext(mAccountProfile.Account);
        UIAccountChoices mAccountChoice = new UIAccountChoices(ClientChoicesUtility.CurrentState);
        return Ok(Tuple.Create(mRetVal, mAccountChoice));
    }

    /// <summary>
    /// ChangePassword is a function that allows the account to change their password.
    /// </summary>
    /// <param name="oldPassword">The account's current password.</param>
    /// <param name="newPassword">The account's new password.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [Authorize("/accounts/change-password")]
    [HttpPost("ChangePassword")]
    public ActionResult<Tuple<string, AuthenticationResponse>> ChangePassword(string oldPassword, string newPassword)
    {
        UIChangePassword mChangePassword = new()
        {
            OldPassword = oldPassword,
            NewPassword = newPassword
        };
        if (mChangePassword.NewPassword.Length == 0) throw new ArgumentNullException("NewPassword", " can not be blank");
        if (mChangePassword.OldPassword.Length == 0) throw new ArgumentNullException("OldPassword", " can not be blank");
        Tuple<string, MAccountProfile> mChangePasswordResult = AccountUtility.ChangePassword(mChangePassword, ipAddress());
        AuthenticationResponse mAuthenticationResponse = new AuthenticationResponse(mChangePasswordResult.Item2);
        setTokenCookie(mAuthenticationResponse.RefreshToken);
        Tuple<string, AuthenticationResponse> mRetVal = Tuple.Create(mChangePasswordResult.Item1, mAuthenticationResponse);
        return Ok(mRetVal);
    }

    /// <summary>
    /// Example of how to delete an account
    /// </summary>
    /// <param name="accountSeqId"></param>
    /// <returns>ActionResult<bool></returns>
    // [HttpDelete("DeleteAccount")]
    private ActionResult<bool> DeleteAccount(int accountSeqId)
    {
        // This is here only for example it is this developers view that deleting accounts
        // is extremely risky and should be left to say a backend developer (DBA if you like)
        // it is possible for data to be associated with an account outside the realms of this
        // application and deleting it here could be quite an issue

        if (accountSeqId < 1) throw new ArgumentNullException(nameof(accountSeqId), " must be a positive number!");
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditAccount);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        var mEditId = HttpContext.Session.GetInt32("EditId");
        if (mEditId != null)
        {
            if (mSecurityInfo.MayDelete && accountSeqId != AccountUtility.CurrentProfile.Id)
            {
                AccountUtility.Delete(accountSeqId);
                HttpContext.Session.Remove("EditId");
                return Ok(true);
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        return StatusCode(StatusCodes.Status204NoContent, "Could not find the account to delete");
    }

    /// <summary>
    /// Allows for editing an account other than the current account.
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    [HttpGet("EditAccount")]
    public ActionResult<MAccountProfile> EditAccount(string account)
    {
        HttpContext.Session.Remove("EditId");
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditAccount);
        MSecurityInfo mSecurityInfo = new (mFunctionProfile, mRequestingProfile);
        MAccountProfile mAccountProfile = new(mRequestingProfile.Id);
        if (account != "new") // Populate from the DB
        {
            mAccountProfile = AccountUtility.GetAccount(account);
            if (!mSecurityInfo.MayEdit)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
            }
        } 
        HttpContext.Session.SetInt32("EditId", mAccountProfile.Id);
        return Ok(mAccountProfile);
    }


    /// <summary>
    /// Allows for editing the same account as the current account.
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    [HttpGet("EditProfile")]
    public ActionResult<MAccountProfile> EditProfile(string account)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditAccount);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mRequestingProfile.Account.ToLowerInvariant() == account.ToLowerInvariant())
        {
            MAccountProfile mAccountProfile = this.getAccount(account);
            HttpContext.Session.SetInt32("EditId", mAccountProfile.Id);
            return Ok(mAccountProfile);
        }
        else
        {
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
    }

    [HttpGet("EncryptDecrypt")]
    public ActionResult<string> EncryptDecrypt(string txtValue, int encryptionType, bool encrypt)
    {
        string mRetVal = string.Empty;
        EncryptionType mEncryptionType = (EncryptionType)encryptionType;
        if (encrypt)
        {
            CryptoUtility.TryEncrypt(txtValue, out mRetVal, mEncryptionType);
        }
        else
        {
            CryptoUtility.TryDecrypt(txtValue, out mRetVal, mEncryptionType);
        }
        return mRetVal;
    }

    [AllowAnonymous]
    [HttpPost("ForgotPassword")]
    public ActionResult<string> ForgotPassword(string account)
    {
        if (String.IsNullOrWhiteSpace(account)) 
        {
            ArgumentNullException mArgumentNullException = new (nameof(account), " account can not be null or empty");
            m_Logger.Error(mArgumentNullException);
            return StatusCode(StatusCodes.Status400BadRequest, mArgumentNullException.Message);

        }
        MAccountProfile mAccountProfile = AccountUtility.ForgotPassword(account, Request.Headers["origin"]);
        if (mAccountProfile != null)
        {
            MMessage mMessage = MessageUtility.GetProfile("RequestNewPassword");
            MRequestNewPassword mRequestNewPassword = new(mMessage)
            {
                AccountName = Uri.EscapeDataString(mAccountProfile.Account),
                FullName = mAccountProfile.FirstName + " " + mAccountProfile.LastName,
                Password = Uri.EscapeDataString(mAccountProfile.Password),
                ResetToken = mAccountProfile.ResetToken
            };
            string urlRoot = $"{Request.Scheme}://{Request.Host}/";
            mRequestNewPassword.Server = urlRoot;
            mRequestNewPassword.FormatBody();
            // send email
            MessageUtility.SendMail(mRequestNewPassword, mAccountProfile);
            // Get return value
            mMessage = MessageUtility.GetProfile("Request Password Reset UI");
            return Ok(mMessage.Body.Replace("<b>", "").Replace("</b>", ""));
        }
        return StatusCode(StatusCodes.Status204NoContent, "Could not request password change");
    }

    /// <summary>
    /// Returns a MAccountProfile given the account. If the account is not specivied ("" or "_") then a new MAccountProfile will be returned.
    /// </summary>
    /// <param name="account"></param>
    /// <returns>MAccountProfile</returns>
    private MAccountProfile getAccount(string account)
    {
        MAccountProfile mRetVal = new MAccountProfile();
        if (!String.IsNullOrWhiteSpace(account) && account != "_")
        {
            mRetVal = AccountUtility.GetAccount(account);
        }
        if (mRetVal == null)
        {
            // mRetVal = AccountUtility.GetAccount(AccountUtility.AnonymousAccount);
            mRetVal = new MAccountProfile();
        }
        mRetVal.Password = string.Empty;
        return mRetVal;
    }

    /// <summary>
    /// Retrieves menu data based on the specified menu type as a JSON string.
    /// </summary>
    /// <param name="menuType"></param>
    /// <returns></returns>
    [HttpGet("GetMenuData")]
    public ActionResult<string> GetMenuData(int menuType)
    {
        MAccountProfile mAccountProfile = AccountUtility.CurrentProfile;
        string mRetVal = null;
        MenuType mMenuType = (MenuType)menuType;
        if (mAccountProfile != null && mAccountProfile.Account.ToLowerInvariant() != ConfigSettings.Anonymous.ToLowerInvariant())
        {
            mRetVal = AccountUtility.GetMenuData(mAccountProfile.Account, mMenuType);
        }
        else
        {
            mRetVal = AccountUtility.GetMenuData(ConfigSettings.Anonymous, mMenuType);
        }
        return Ok(mRetVal);
    }

    /// <summary>
    /// Retrieves a list of menu items based on the specified menu type for the current account.
    /// </summary>
    /// <param name="menuType"></param>
    /// <returns></returns>
    [HttpGet("GetMenuItems")]
    public ActionResult<IList<MMenuTree>> GetMenuItems(int menuType)
    {
        MAccountProfile mAccountProfile = AccountUtility.CurrentProfile;
        IList<MMenuTree> mRetVal = null;
        MenuType mMenuType = (MenuType)menuType;
        if (mAccountProfile != null && mAccountProfile.Account.ToLowerInvariant() != ConfigSettings.Anonymous.ToLowerInvariant())
        {
            mRetVal = AccountUtility.GetMenuItems(mAccountProfile.Account, mMenuType);
        }
        else
        {
            mRetVal = AccountUtility.GetMenuItems(ConfigSettings.Anonymous, mMenuType);
        }
        return Ok(mRetVal);
    }

    /// <summary>
    /// Returns the current client preferences
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetPreferences")]
    public UIAccountChoices GetPreferences()
    {
        UIAccountChoices mRetVal = new UIAccountChoices(ClientChoicesUtility.CurrentState);
        return mRetVal;
    }

    /// <summary>
    /// Returns a list of selectable actions for the current client.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// This is needed by the UI to allow the client to choose their favorite "action"
    /// </remarks>
    [HttpGet("GetSelectableActions")]
    public List<UISelectedableAction> GetSelectableActions()
    {
        List<string> mExcludedActions = new List<string>() { "api", "favorite", "logoff", "logon" };
        List<UISelectedableAction> mRetVal = new List<UISelectedableAction>();
        IList<MMenuTree> mMenuItems = AccountUtility.GetMenuItems(AccountUtility.CurrentProfile.Account, MenuType.Hierarchical);
        addSelectedActions(mMenuItems, ref mRetVal);
        mMenuItems = AccountUtility.GetMenuItems(AccountUtility.CurrentProfile.Account, MenuType.Horizontal);
        addSelectedActions(mMenuItems, ref mRetVal);
        mMenuItems = AccountUtility.GetMenuItems(AccountUtility.CurrentProfile.Account, MenuType.Vertical);
        addSelectedActions(mMenuItems, ref mRetVal);
        // not the best way b/c this is defined in the DB but it's better than nothing
        foreach (string mAction in mExcludedActions)
        {
            var mItemToRemove = mRetVal.SingleOrDefault(r => r.Title.ToLower().Contains(mAction.ToLower()));
            if (mItemToRemove != null) { mRetVal.Remove(mItemToRemove); }
        }
        mRetVal = mRetVal.OrderBy(o => o.Title).ToList();
        return mRetVal;
    }

    /// <summary>
    /// Recursively adds UISelectedableAction to selectedableActions given a list of MMenuTree
    /// </summary>
    /// <param name="menuTree"></param>
    /// <param name="selectedableActions"></param>
    private void addSelectedActions(IList<MMenuTree> menuTree, ref List<UISelectedableAction> selectedableActions)
    {
        foreach (MMenuTree mMenuTree in menuTree)
        {
            if (mMenuTree.Children == null || mMenuTree.Children.Count == 0)
            {
                UISelectedableAction mSelectedableAction = new UISelectedableAction();
                mSelectedableAction.Action = mMenuTree.Action;
                mSelectedableAction.Title = mMenuTree.Label;
                selectedableActions.Add(mSelectedableAction);
            }
            else
            {
                addSelectedActions(mMenuTree.Children, ref selectedableActions);
            }
        }
    }

    /// <summary>
    /// Returns the IP address of the request.
    /// </summary>
    /// <returns></returns>
    private string ipAddress()
    {
        string mRetVal = string.Empty;
        string mIpAddress = getRemoteHostIpAddressUsingRemoteIpAddress();
        if (!string.IsNullOrEmpty(mIpAddress)) mRetVal = mIpAddress;
        mIpAddress = getRemoteHostIpAddressUsingXForwardedFor();
        if (!string.IsNullOrEmpty(mIpAddress)) mRetVal = mIpAddress;
        mIpAddress = getRemoteHostIpAddressUsingXRealIp();
        if (!string.IsNullOrEmpty(mIpAddress)) mRetVal = mIpAddress;

        return mRetVal;
    }

    private string getRemoteHostIpAddressUsingRemoteIpAddress()
    {
        if(HttpContext.Connection.RemoteIpAddress != null) return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        return string.Empty;
    }

    private string getRemoteHostIpAddressUsingXForwardedFor()
    {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        IPAddress? remoteIpAddress = null;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        var forwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

        if (!string.IsNullOrEmpty(forwardedFor))
        {
            var ips = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(s => s.Trim());

            foreach (var ip in ips)
            {
                if (IPAddress.TryParse(ip, out var address) &&
                    (address.AddressFamily is AddressFamily.InterNetwork
                     or AddressFamily.InterNetworkV6))
                {
                    remoteIpAddress = address;
                    break;
                }
            }
        }
        if(remoteIpAddress != null) return remoteIpAddress.MapToIPv4().ToString();
        return string.Empty;
    }

    private string getRemoteHostIpAddressUsingXRealIp()
    {
        IPAddress remoteIpAddress = null;
        var xRealIpExists = HttpContext.Request.Headers.TryGetValue("X-Real-IP", out var xRealIp);

        if (xRealIpExists)
        {
            if (!IPAddress.TryParse(xRealIp, out IPAddress address))
            {
                return remoteIpAddress.MapToIPv4().ToString();
            }

            var isValidIP = (address.AddressFamily is AddressFamily.InterNetwork
                             or AddressFamily.InterNetworkV6);

            if (isValidIP)
            {
                remoteIpAddress = address;
            }

            return remoteIpAddress.MapToIPv4().ToString();
        }
        if(remoteIpAddress != null) return remoteIpAddress.MapToIPv4().ToString();
        return string.Empty;
    }

    /// <summary>
    /// Removes any in memory authentication information and logs the user out.
    /// </summary>
    /// <returns></returns>
    [HttpPost("Logoff")]
    public ActionResult<Tuple<AuthenticationResponse, UIAccountChoices>> Logoff()
    {
        string mRefreshToken = Request.Cookies["refreshToken"];
        if (mRefreshToken == null)
        {
            mRefreshToken = string.Empty;
        }
        if (AccountUtility.CurrentProfile != null && !string.IsNullOrWhiteSpace(AccountUtility.CurrentProfile.Account)) 
        {
            AccountUtility.Logoff(AccountUtility.CurrentProfile.Account, mRefreshToken, ipAddress());
        }
        MAccountProfile mAccountProfile = AccountUtility.GetAccount(AccountUtility.AnonymousAccount);
        ClientChoicesUtility.SynchronizeContext(mAccountProfile.Account);
        CryptoUtility.TryEncrypt(mAccountProfile.Password, out string mPassword, (EncryptionType)SecurityEntityUtility.CurrentProfile.EncryptionType);
        return Authenticate(mAccountProfile.Account, mPassword);
    }

    /// <summary>
    /// Creates a new refresh token with a Json Web Token.
    /// </summary>
    /// <returns>AuthenticationResponse</returns>
    /// <remarks>
    /// Should be executed a minute before the JWT expires.
    /// The refresh should expire a considerable amout of time before the JWT and
    /// is the mechanism by which the account is authenticated at a later date.
    /// </remarks>
    [HttpPost("RefreshToken")]
    public ActionResult<Tuple<AuthenticationResponse, UIAccountChoices>> RefreshToken()
    {
        string mRefreshToken = Request.Cookies[ConfigSettings.JWT_Refresh_CookieName];
        AuthenticationResponse mAuthenticationResponse = null;
        Tuple<AuthenticationResponse, UIAccountChoices> mRetVal = null;
        if (mRefreshToken != null)
        {
            mAuthenticationResponse = AccountUtility.RefreshToken(mRefreshToken, ipAddress());
            if (!mAuthenticationResponse.Account.Equals(AccountUtility.AnonymousAccount))
            {
                setTokenCookie(mAuthenticationResponse.RefreshToken);
            }
            else
            {
                Response.Cookies.Delete(ConfigSettings.JWT_Refresh_CookieName);
            }
            ClientChoicesUtility.SynchronizeContext(mAuthenticationResponse.Account);
            mRetVal = Tuple.Create(mAuthenticationResponse, new UIAccountChoices(ClientChoicesUtility.CurrentState));
            return Ok(mRetVal);
        }
        MAccountProfile mAccountProfile = AccountUtility.GetAccount(AccountUtility.AnonymousAccount);
        mAuthenticationResponse = new AuthenticationResponse(mAccountProfile);
        UIAccountChoices mAccountChoice = new UIAccountChoices(ClientChoicesUtility.CurrentState);
        Response.Cookies.Delete(ConfigSettings.JWT_Refresh_CookieName);
        mRetVal = Tuple.Create(mAuthenticationResponse, mAccountChoice);
        return Ok(mRetVal);
    }

    /// <summary>
    /// Registers a new account profile.  If the registration is successful, a verification email will be sent to the account.
    /// </summary>
    /// <param name="accountProfile">The account profile to be registered.</param>
    /// <returns>An IActionResult containing a message indicating the success or failure of the registration.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the accountProfile parameter is null or empty.</exception>
    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register(MAccountProfile accountProfile)
    {
        if (accountProfile == null) throw new ArgumentNullException(nameof(accountProfile), " can not be blank");
        if (string.IsNullOrWhiteSpace(accountProfile.Email)) throw new ArgumentNullException("Email", " can not be blank");
        if (string.IsNullOrWhiteSpace(accountProfile.FirstName)) throw new ArgumentNullException("FirstName", " can not be blank");
        if (string.IsNullOrWhiteSpace(accountProfile.LastName)) throw new ArgumentNullException("LastName", " can not be blank");
        MAccountProfile mSavedAccountProfile = AccountUtility.Register(accountProfile, Request.Headers.Origin);
        string mRetunMsg = "Registration successful, please check your email for verification instructions";
        bool mMailSent = false;
        if(mSavedAccountProfile != null)
        {
            MMessage mMessage = MessageUtility.GetProfile("RegistrationSuccess");
            MRegistrationSuccess mRegistrationSuccess = new(mMessage)
            {
                Email = mSavedAccountProfile.Email,
                FullName = mSavedAccountProfile.FirstName + " " + mSavedAccountProfile.LastName,
                VerificationToken = Uri.EscapeDataString(mSavedAccountProfile.VerificationToken)
            };
            string mUrlRoot = string.Format("{0}:/{1}", Request.Scheme, Request.Host);
            mRegistrationSuccess.Server = mUrlRoot;
            mRegistrationSuccess.FormatBody();
            // send email
            mMailSent = MessageUtility.SendMail(mRegistrationSuccess, accountProfile);
            if(!mMailSent)
            {
                mRetunMsg = "Registration failed, could not send mail to '" + accountProfile.Email + "' the account was not created!";
                AccountUtility.Delete(mSavedAccountProfile.Id);
            }
            return Ok(new { message = mRetunMsg });
        }
        else
        {
            if(mSavedAccountProfile != null && mSavedAccountProfile.Id > 0)
            {
                AccountUtility.Delete(mSavedAccountProfile.Id);
            }
            // Send email indicating error
            mRetunMsg = "Registration failed, please try again";
        }
        return Ok(new { message = mRetunMsg });
    }

    [HttpPut("ResetPassword")]
    public ActionResult<Tuple<AuthenticationResponse, UIAccountChoices>> ResetPassword(string resetToken, string newPassword)
    {
        if (String.IsNullOrWhiteSpace(resetToken)) throw new ArgumentNullException(nameof(resetToken), " can not be blank");
        if (String.IsNullOrWhiteSpace(newPassword)) throw new ArgumentNullException(nameof(newPassword), " can not be blank");
        // Get the account from the reset token.  If found continute else fall through and return 406
        MAccountProfile mAccountProfile = AccountUtility.GetProfileByResetToken(resetToken);
        if (mAccountProfile != null)
        {
            AccountUtility.ResetPassword(mAccountProfile, newPassword);
            // Return Authenticate result (should always work we just saved the new password)
            // changing my mind should return the same as Authenticate
            return Authenticate(mAccountProfile.Account, newPassword);
        };
        return StatusCode(StatusCodes.Status406NotAcceptable, "The reset token is no longer invalid");
    }

    /// <summary>
    /// Revokes a refresh token.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("RevokeToken")]
    public IActionResult RevokeToken(string token)
    {
        // TODO: For future use, not currently being used!

        // accept token from request body or cookie
        var mToken = token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(mToken))
            return BadRequest(new { message = "Token is required" });

        // users can revoke their own tokens and admins can revoke any tokens
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile("RevokeToken");
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (!AccountUtility.CurrentProfile.OwnsToken(token) && !mSecurityInfo.MayView)
            return Unauthorized(new { message = "Unauthorized" });

        AccountUtility.RevokeToken(token, ipAddress());
        return Ok(new { message = "Token revoked" });
    }

    /// <summary>
    /// Saves the specified account.
    /// </summary>
    /// <param name="accountProfile"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("SaveAccount")]
    public ActionResult<bool> SaveAccount(MAccountProfile accountProfile)
    {
        // requesting profile same as 
        bool mRetVal = false;
        if (accountProfile == null) throw new ArgumentNullException(nameof(accountProfile), " can not be blank");
        if (string.IsNullOrWhiteSpace(accountProfile.Account)) throw new ArgumentNullException("Account", " can not be blank");
        if (string.IsNullOrWhiteSpace(accountProfile.FirstName)) throw new ArgumentNullException("FirstName", " can not be blank");
        if (string.IsNullOrWhiteSpace(accountProfile.LastName)) throw new ArgumentNullException("LastName", " can not be blank");
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile("SaveAccount");
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        MSecurityInfo mSecurityInfo_View_Account_Group = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.View_Account_Group_Tab), mRequestingProfile);
        MSecurityInfo mSecurityInfo_View_Account_Role = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.View_Account_Role_Tab), mRequestingProfile);
        var mEditId = HttpContext.Session.GetInt32("EditId");
        if (mEditId != null)
        {
            // we don't want to save the of the properties from the UI so we get the profile from the DB
            MAccountProfile mExistingAccount = AccountUtility.GetAccount(accountProfile.Account);
            if(mSecurityInfo.MayAdd || mSecurityInfo.MayEdit || mRequestingProfile.Account == mExistingAccount.Account)
            {
                if (mExistingAccount.Account == null)
                {
                    mExistingAccount = new MAccountProfile(mRequestingProfile.Id);
                    mExistingAccount.Password = ""; // should be auto generated and 
                }
                mExistingAccount.Account = accountProfile.Account;
                mExistingAccount.AssignedRoles = accountProfile.AssignedRoles;
                mExistingAccount.Email = accountProfile.Email;
                mExistingAccount.EnableNotifications = accountProfile.EnableNotifications;
                if (mRequestingProfile.Account != mExistingAccount.Account) 
                {
                    mExistingAccount.FailedAttempts = accountProfile.FailedAttempts;
                    mExistingAccount.Status = accountProfile.Status;
                }

                mExistingAccount.FirstName = accountProfile.FirstName;
                mExistingAccount.Groups = accountProfile.Groups;
                mExistingAccount.Id = accountProfile.Id;
                if (mRequestingProfile.IsSystemAdmin)
                {
                    mExistingAccount.IsSystemAdmin = accountProfile.IsSystemAdmin;
                }
                mExistingAccount.LastName = accountProfile.LastName;
                mExistingAccount.Location = accountProfile.Location;
                mExistingAccount.MiddleName = accountProfile.MiddleName;
                mExistingAccount.Name = accountProfile.Account;
                mExistingAccount.PreferredName = accountProfile.PreferredName;
                mExistingAccount.TimeZone = accountProfile.TimeZone;
                mExistingAccount.UpdatedBy = mRequestingProfile.Id;
                mExistingAccount.UpdatedDate = DateTime.Now;
                AccountUtility.Save(mExistingAccount, false, mSecurityInfo_View_Account_Role.MayView, mSecurityInfo_View_Account_Role.MayView);
                mRetVal = true;
            }
        }
        if (mRetVal == false) 
        {
            this.m_Logger.Error(mRequestingProfile.Account + " does not have permissions to 'SaveAccount'");
        }
        return Ok(mRetVal);
    }

    /// <summary>
    /// Saves the client choices.
    /// </summary>
    /// <param name="accountChoices"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("SaveClientChoices")]
    public ActionResult<UIAccountChoices> SaveClientChoices(UIAccountChoices accountChoices)
    {
        if (accountChoices == null) throw new ArgumentNullException(nameof(accountChoices), "accountChoices cannot be a null reference (Nothing in Visual Basic)!");
        if (accountChoices.Account.ToLower() != ConfigSettings.Anonymous.ToLower())
        {
            MSecurityEntity mSecurityEntity = SecurityEntityUtility.GetProfile(accountChoices.SecurityEntityId);
            MClientChoicesState mDefaultClientChoicesState = ClientChoicesUtility.AnonymousState;
            MClientChoicesState mClientChoicesState = ClientChoicesUtility.CurrentState;

            mClientChoicesState[MClientChoices.Account] = accountChoices.Account;
            mClientChoicesState[MClientChoices.Action] = accountChoices.Action ?? mDefaultClientChoicesState[MClientChoices.Action];
            mClientChoicesState[MClientChoices.SecurityEntityId] = mSecurityEntity.Id.ToString();
            mClientChoicesState[MClientChoices.SecurityEntityName] = mSecurityEntity.Name;
            mClientChoicesState[MClientChoices.RecordsPerPage] = accountChoices.RecordsPerPage.ToString() ?? mDefaultClientChoicesState[MClientChoices.RecordsPerPage];

            mClientChoicesState[MClientChoices.ColorScheme] = accountChoices.ColorScheme ?? mDefaultClientChoicesState[MClientChoices.ColorScheme];
            mClientChoicesState[MClientChoices.EvenRow] = accountChoices.EvenRow ?? mDefaultClientChoicesState[MClientChoices.EvenRow];
            mClientChoicesState[MClientChoices.EvenFont] = accountChoices.EvenFont ?? mDefaultClientChoicesState[MClientChoices.EvenFont];

            mClientChoicesState[MClientChoices.OddRow] = accountChoices.OddRow ?? mDefaultClientChoicesState[MClientChoices.OddRow];
            mClientChoicesState[MClientChoices.OddFont] = accountChoices.OddFont ?? mDefaultClientChoicesState[MClientChoices.OddFont];

            mClientChoicesState[MClientChoices.HeaderRow] = accountChoices.HeaderRow ?? mDefaultClientChoicesState[MClientChoices.HeaderRow];
            mClientChoicesState[MClientChoices.HeaderFont] = accountChoices.HeaderFont ?? mDefaultClientChoicesState[MClientChoices.HeaderFont];

            mClientChoicesState[MClientChoices.Background] = accountChoices.Background ?? mDefaultClientChoicesState[MClientChoices.Background];
            ClientChoicesUtility.Save(mClientChoicesState);
            AccountUtility.RemoveInMemoryInformation(accountChoices.Account);
            UIAccountChoices mRetVal = new(mClientChoicesState);
            return Ok(mRetVal);
        }
        return Ok(false);
    }

    /// <summary>
    /// Performs a search for accounts based on the provided search criteria.
    /// </summary>
    /// <param name="searchCriteria">The criteria used to filter the search</param>
    /// <returns></returns>
    [Authorize("Accounts")]
    [HttpPost("SearchAccounts")]
    public String SearchAccounts(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[AccountSeqId], [Account], [First_Name], [Last_Name], [Email], [Added_Date], [Last_Login]";
        if (searchCriteria.sortColumns.Length > 0)
        {
            Tuple<string, string> mOrderByAndWhere = SearchUtility.GetOrderByAndWhere(mColumns, searchCriteria.searchColumns, searchCriteria.sortColumns, searchCriteria.searchText);
            string mOrderByClause = mOrderByAndWhere.Item1;
            string mWhereClause = mOrderByAndWhere.Item2;
            MSearchCriteria mSearchCriteria = new MSearchCriteria
            {
                Columns = mColumns,
                OrderByClause = mOrderByClause,
                PageSize = searchCriteria.pageSize,
                SelectedPage = searchCriteria.selectedPage,
                TableOrView = "[ZGWSecurity].[Accounts]",
                WhereClause = mWhereClause
            };

            mRetVal = SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;
    }

    /// <summary>
    /// Sets an HttpOnly refresh cookie.
    /// </summary>
    /// <param name="token"></param>
    private void setTokenCookie(string token)
    {
        string mToken = string.Empty;
        if (token != null)
        {
            mToken = token;
        }
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(ConfigSettings.JWT_Refresh_Cookie_TTL_Days)
        };
        Response.Cookies.Append(ConfigSettings.JWT_Refresh_CookieName, mToken, cookieOptions);
    }

    /// <summary>
    /// Verifies an account by looking up the account in the database using the verification token.
    /// </summary>
    /// <param name="verificationToken">The verification token to check.</param>
    /// <param name="email">The email address of the account to verify.</param>
    /// <returns>A tuple containing an AuthenticationResponse object and a UIAccountChoices object.</returns>
    [AllowAnonymous]
    [HttpPost("VerifyAccount")]
    public ActionResult<Tuple<AuthenticationResponse, UIAccountChoices>> VerifyAccount(string verificationToken, string email)
    {
        // Look up the account in the database using the verification token
        // If the account exists and the verification token is valid:
        //      Set the last log on date
        //      Set the status to change password
        //      Set the verification token to null
        //      Save the profile
        //      return the account and account choices
        // If the account does not exist or the verification token is invalid, throw an exception
        MAccountProfile mAccountProfile = AccountUtility.VerifyAccount(verificationToken, email);
        if(mAccountProfile == null)
        {
            return StatusCode(StatusCodes.Status406NotAcceptable, "The verification token is not invalid");
        }
        mAccountProfile.LastLogOn = DateTime.Now;
        mAccountProfile.Status = (int)SystemStatus.ChangePassword;
        mAccountProfile.VerificationToken = null;
        AccountUtility.Save(mAccountProfile, false, false, false);
        AuthenticationResponse mRetVal = new(mAccountProfile);
        ClientChoicesUtility.SynchronizeContext(mAccountProfile.Account);
        UIAccountChoices mAccountChoice = new(ClientChoicesUtility.CurrentState);
        return Ok(Tuple.Create(mRetVal, mAccountChoice));
    }
}