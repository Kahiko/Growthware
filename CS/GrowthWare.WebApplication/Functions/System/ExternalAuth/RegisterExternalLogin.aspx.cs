using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.ExternalAuth
{
    public partial class RegisterExternalLogin : ClientChoicesPage
    {
        protected string ProviderName
        {
            get { return (string)ViewState["ProviderName"] ?? String.Empty; }
            private set { ViewState["ProviderName"] = value; }
        }

        protected string ProviderAccountKey
        {
            get { return (string)ViewState["ProviderAccountKey"] ?? String.Empty; }
            private set { ViewState["ProviderAccountKey"] = value; }
        }

        private void RedirectOnFail()
        {
            Response.Redirect((User.Identity.IsAuthenticated) ? "~/Account/Manage" : "~/Account/Login");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Process the result from an auth provider in the request
            ProviderName = IdentityHelper.GetProviderNameFromRequest(Request);
            if (String.IsNullOrEmpty(ProviderName))
            {
                RedirectOnFail();
                return;
            }
            if (!IsPostBack)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
                if (loginInfo == null)
                {
                    RedirectOnFail();
                    return;
                }
                //var user = manager.Find(loginInfo.Login);
                MAccountProfile mAccountProfile = AccountUtility.GetProfile(loginInfo.Email);
                if (mAccountProfile != null)
                {
                    //IdentityHelper.SignIn(manager, user, isPersistent: false);
                    //IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    AccountUtility.SetPrincipal(mAccountProfile);
                    //string mAction = ClientChoicesState(MClientChoices.Action);
                    string mAction = "Home";
                    string mScript = "<script type='text/javascript' language='javascript'>window.location.hash = '?Action=" + mAction + "'; location.reload();</script>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", mScript);
                }
                else if (User.Identity.IsAuthenticated)
                {
                    // Apply Xsrf check when linking
                    var verifiedloginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo(IdentityHelper.XsrfKey, User.Identity.GetUserId());
                    if (verifiedloginInfo == null)
                    {
                        //RedirectOnFail();
                        return;
                    }

                    //var result = manager.AddLogin(User.Identity.GetUserId(), verifiedloginInfo.Login);
                    //if (result.Succeeded)
                    //{
                    //    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    //}
                    //else
                    //{
                    //    AddErrors(result);
                    //    return;
                    //}
                }
                else
                {
                    TextBox mEmail = (TextBox)AddEditAccount.FindControl("txtEmail");
                    if (mEmail != null) 
                    {
                        mEmail.Text = loginInfo.Email;
                    }
                }
            }
        }

        protected void LogIn_Click(object sender, EventArgs e)
        {
            CreateAndLoginUser();
        }

        private void CreateAndLoginUser()
        {
            //if (!IsValid)
            //{
            //    return;
            //}
            //var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var user = new ApplicationUser() { UserName = email.Text, Email = email.Text };
            //IdentityResult result = manager.Create(user);
            //if (result.Succeeded)
            //{
            //    var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
            //    if (loginInfo == null)
            //    {
            //        RedirectOnFail();
            //        return;
            //    }
            //    result = manager.AddLogin(user.Id, loginInfo.Login);
            //    if (result.Succeeded)
            //    {
            //        IdentityHelper.SignIn(manager, user, isPersistent: false);

            //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            //        // var code = manager.GenerateEmailConfirmationToken(user.Id);
            //        // Send this link via email: IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id)

            //        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            //        return;
            //    }
            //}
            //AddErrors(result);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}