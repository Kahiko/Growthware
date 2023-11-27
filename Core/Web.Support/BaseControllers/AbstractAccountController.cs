using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Framework.Enumerations;
using System.Data;
using System.Linq;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractAccountController : ControllerBase
{
    private Logger m_Logger = Logger.Instance();
    private string s_AnonymousAccount = "Anonymous";


    [AllowAnonymous]
    [HttpPost("Authenticate")]
    public ActionResult<AuthenticationResponse> Authenticate(string account, string password)
    {
        MAccountProfile mAccountProfile = AccountUtility.Authenticate(account, password, ipAddress());
        if (mAccountProfile == null)
        {
            AccountUtility.GetAccount("Anonymous");
            return StatusCode(403, "Incorrect account or password");
        }
        AuthenticationResponse mAuthenticationResponse = new AuthenticationResponse(mAccountProfile);
        setTokenCookie(mAuthenticationResponse.RefreshToken);
        return Ok(mAuthenticationResponse);
    }

    [Authorize("/accounts/change-password")]
    [HttpPost("ChangePassword")]
    public ActionResult ChangePassword(string oldPassword, string newPassword)
    {
        UIChangePassword mChangePassword = new UIChangePassword();
        mChangePassword.OldPassword = oldPassword;
        mChangePassword.NewPassword = newPassword;
        // if (mChangePassword. <= 0) throw new ArgumentNullException("accountSeqId", " must be a positive number!");
        if (mChangePassword.NewPassword.Length == 0) throw new ArgumentNullException("NewPassword", " can not be blank");
        if (mChangePassword.OldPassword.Length == 0) throw new ArgumentNullException("OldPassword", " can not be blank");
        string mRetVal = AccountUtility.ChangePassword(mChangePassword);
        return Ok(mRetVal);
    }

    /// <summary>
    /// Example of how to delete an account
    /// </summary>
    /// <param name="accountSeqId"></param>
    /// <returns>ActionResult<bool></returns>
    [HttpDelete("DeleteAccount")]
    public ActionResult<bool> DeleteAccount(int accountSeqId)
    {
        // TODO: Remove/comment out the HttpDelete and change the access modifier to private!!!

        // This is here only for example it is this developers view that deleting accounts
        // is extremely risky and should be left to say a backend developer (DBA if you like)
        // it is possible for data to be associated with an account outside the realms of this
        // application and deleting it here could be quite an issue

        if (accountSeqId < 1) throw new ArgumentNullException("accountSeqId", " must be a positive number!");
        MAccountProfile mRequestingProfile = getCurrentAccount();
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

    // [HttpGet("/accounts/edit-my-account")]
    [HttpGet("EditAccount")]
    public ActionResult<MAccountProfile> EditAccount(string account)
    {

        MAccountProfile mRequestingProfile = getCurrentAccount();
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditAccount);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        HttpContext.Session.Remove("EditId");
        MAccountProfile mAccountProfile = new MAccountProfile(mRequestingProfile.Id);
        if (account != "new") // Populate from the DB
        {
            mAccountProfile = AccountUtility.GetAccount(account);
        }
        if (mSecurityInfo.MayEdit)
        {
            HttpContext.Session.SetInt32("EditId", mAccountProfile.Id);
            return Ok(mAccountProfile);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }


    [HttpGet("EditProfile")]
    public ActionResult<MAccountProfile> EditProfile(string account)
    {
        MAccountProfile mRequestingProfile = getCurrentAccount();
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
    public ContentResult EncryptDecrypt(string txtValue, int encryptionType, bool encrypt)
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
        return Content(mRetVal);
    }

    /// <summary>
    /// Returns the current account from HttpContext, if Context is not available then "Anonymous" will be returned
    /// </summary>
    /// <returns>MAccountProfile</returns>
    private MAccountProfile getCurrentAccount()
    {
        MAccountProfile mRetVal = AccountUtility.CurrentProfile;
        mRetVal.Password = string.Empty;
        return mRetVal;
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
            // mRetVal = AccountUtility.GetAccount("Anonymous");
            mRetVal = new MAccountProfile();
        }
        mRetVal.Password = string.Empty;
        return mRetVal;
    }

    [HttpGet("GetMenuData")]
    public ActionResult<string> GetMenuData(int menuType)
    {
        MAccountProfile mAccountProfile = getCurrentAccount();
        string mRetVal = null;
        MenuType mMenuType = (MenuType)menuType;
        if (mAccountProfile != null && mAccountProfile.Account.ToLowerInvariant() != this.s_AnonymousAccount.ToLowerInvariant())
        {
            mRetVal = AccountUtility.GetMenuData(mAccountProfile.Account, mMenuType);
        }
        else
        {
            mRetVal = AccountUtility.GetMenuData(this.s_AnonymousAccount, mMenuType);
        }
        return Ok(mRetVal);
    }

    [HttpGet("GetMenuItems")]
    public ActionResult<IList<MMenuTree>> GetMenuItems(int menuType)
    {
        MAccountProfile mAccountProfile = getCurrentAccount();
        IList<MMenuTree> mRetVal = null;
        MenuType mMenuType = (MenuType)menuType;
        if (mAccountProfile != null && mAccountProfile.Account.ToLowerInvariant() != this.s_AnonymousAccount.ToLowerInvariant())
        {
            mRetVal = AccountUtility.GetMenuItems(mAccountProfile.Account, mMenuType);
        }
        else
        {
            mRetVal = AccountUtility.GetMenuItems(this.s_AnonymousAccount, mMenuType);
        }
        return Ok(mRetVal);
    }

    [HttpGet("GetPreferences")]
    public UIAccountChoices GetPreferences()
    {
        UIAccountChoices mRetVal = new UIAccountChoices(ClientChoicesUtility.CurrentState);
        return mRetVal;
    }

    [HttpGet("GetSelectableActions")]
    public List<UISelectedableAction> GetSelectableActions()
    {
        List<string> mExcludedActions = new List<string>() { "favorite", "logoff", "logon" };
        List<UISelectedableAction> mRetVal = new List<UISelectedableAction>();
        IList<MMenuTree> mMenuItems = AccountUtility.GetMenuItems(getCurrentAccount().Account, MenuType.Hierarchical);
        addSelectedActions(mMenuItems, ref mRetVal);
        mMenuItems = AccountUtility.GetMenuItems(getCurrentAccount().Account, MenuType.Horizontal);
        addSelectedActions(mMenuItems, ref mRetVal);
        mMenuItems = AccountUtility.GetMenuItems(getCurrentAccount().Account, MenuType.Vertical);
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

    private string ipAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        else
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }

    [HttpGet("Logoff")]
    public ActionResult<AuthenticationResponse> Logoff()
    {
        AccountUtility.RemoveInMemoryInformation(this.getCurrentAccount().Account);
        ClientChoicesUtility.ClearSession();
        AuthenticationResponse mAuthenticationResponse = new(AccountUtility.GetAccount(AccountUtility.AnonymousAccount));
        setTokenCookie(mAuthenticationResponse.RefreshToken);
        return mAuthenticationResponse;
    }

    [HttpPost("RefreshToken")]
    public ActionResult<AuthenticationResponse> RefreshToken()
    {
        try
        {
            var mRefreshToken = Request.Cookies["refreshToken"];
            AuthenticationResponse mAuthenticationResponse = TokenUtility.RefreshToken(mRefreshToken, ipAddress());
            setTokenCookie(mAuthenticationResponse.RefreshToken);
            MAccountProfile mAccountProfile = AccountUtility.GetAccount(mAuthenticationResponse.Account);
            EncryptionType mEncryptionType = (EncryptionType)SecurityEntityUtility.CurrentProfile().EncryptionType;
            string mPassword = string.Empty;
            CryptoUtility.TryDecrypt(mAccountProfile.Password, out mPassword, mEncryptionType);
            return Authenticate(mAccountProfile.Account, mPassword);
        }
        catch (System.Exception ex)
        {
            if (ex.Message.Contains("token does not exist"))
            {
                return NotFound();
                // throw;
            }
            else
            {
                this.m_Logger.Error(ex);
                throw new Exception("token does not exist, unable to get account");
            }
        }
    }

    [AllowAnonymous]
    [HttpPost("SaveAccount")]
    public ActionResult<bool> SaveAccount(MAccountProfile accountProfile)
    {
        // requesting profile same as 
        bool mRetVal = false;
        MAccountProfile mRequestingProfile = getCurrentAccount();
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile("SaveAccount");
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        MSecurityInfo mSecurityInfo_View_Account_Group = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.View_Account_Group_Tab), mRequestingProfile);
        MSecurityInfo mSecurityInfo_View_Account_Role = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.View_Account_Role_Tab), mRequestingProfile);
        var mEditId = HttpContext.Session.GetInt32("EditId");
        if (mEditId != null && (mSecurityInfo.MayAdd || mSecurityInfo.MayEdit))
        {
            // we don't want to save the of the properties from the UI so we get the profile from the DB
            MAccountProfile mExistingAccount = AccountUtility.GetAccount(accountProfile.Account);
            if (mExistingAccount == null)
            {
                mExistingAccount = new MAccountProfile(mRequestingProfile.Id);
                mExistingAccount.Password = ""; // should be auto generated and 
            }
            mExistingAccount.Account = accountProfile.Account;
            mExistingAccount.AssignedRoles = accountProfile.AssignedRoles;
            mExistingAccount.Email = accountProfile.Email;
            mExistingAccount.EnableNotifications = accountProfile.EnableNotifications;
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
            mExistingAccount.Status = accountProfile.Status;
            mExistingAccount.TimeZone = accountProfile.TimeZone;
            mExistingAccount.UpdatedBy = mRequestingProfile.Id;
            mExistingAccount.UpdatedDate = DateTime.Now;
            AccountUtility.Save(mExistingAccount, false, mSecurityInfo_View_Account_Role.MayView, mSecurityInfo_View_Account_Role.MayView);
            mRetVal = true;
        }
        else
        {
            this.m_Logger.Error(mRequestingProfile.Account + " does not have permissions to 'SaveAccount'");
        }
        return Ok(mRetVal);
    }

    [HttpPost("SaveClientChoices")]
    public ActionResult<bool> SaveClientChoices(UIAccountChoices accountChoices)
    {
        if (accountChoices == null) throw new ArgumentNullException("accountChoices", "accountChoices cannot be a null reference (Nothing in Visual Basic)!");
        bool mRetVal = false;
        if (accountChoices.Account.ToLower() != this.s_AnonymousAccount.ToLower())
        {
            MSecurityEntity mSecurityEntity = SecurityEntityUtility.GetProfile(accountChoices.SecurityEntityID);
            MClientChoicesState mDefaultClientChoicesState = ClientChoicesUtility.AnonymousState;
            MClientChoicesState mClientChoicesState = ClientChoicesUtility.CurrentState;
            mClientChoicesState[MClientChoices.AccountName] = accountChoices.Account;
            mClientChoicesState[MClientChoices.Action] = accountChoices.Action ?? mDefaultClientChoicesState[MClientChoices.Action];
            mClientChoicesState[MClientChoices.AlternatingRowBackColor] = accountChoices.AlternatingRowBackColor ?? mDefaultClientChoicesState[MClientChoices.AlternatingRowBackColor];
            mClientChoicesState[MClientChoices.BackColor] = accountChoices.BackColor ?? mDefaultClientChoicesState[MClientChoices.BackColor];
            mClientChoicesState[MClientChoices.ColorScheme] = accountChoices.ColorScheme ?? mDefaultClientChoicesState[MClientChoices.ColorScheme];
            mClientChoicesState[MClientChoices.HeadColor] = accountChoices.HeadColor ?? mDefaultClientChoicesState[MClientChoices.HeadColor];
            mClientChoicesState[MClientChoices.HeaderForeColor] = accountChoices.HeaderForeColor ?? mDefaultClientChoicesState[MClientChoices.HeaderForeColor];
            mClientChoicesState[MClientChoices.LeftColor] = accountChoices.LeftColor ?? mDefaultClientChoicesState[MClientChoices.LeftColor];
            mClientChoicesState[MClientChoices.RecordsPerPage] = accountChoices.RecordsPerPage.ToString() ?? mDefaultClientChoicesState[MClientChoices.RecordsPerPage];
            mClientChoicesState[MClientChoices.RowBackColor] = accountChoices.RowBackColor ?? mDefaultClientChoicesState[MClientChoices.RowBackColor];
            mClientChoicesState[MClientChoices.SecurityEntityID] = mSecurityEntity.Id.ToString();
            mClientChoicesState[MClientChoices.SecurityEntityName] = mSecurityEntity.Name;
            mClientChoicesState[MClientChoices.SubheadColor] = accountChoices.SubheadColor ?? mDefaultClientChoicesState[MClientChoices.SubheadColor];
            ClientChoicesUtility.Save(mClientChoicesState);
            mRetVal = true;
        }
        return mRetVal;
    }

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
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", mToken, cookieOptions);
    }
}