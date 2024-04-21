using GrowthWare.WebSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Globalization;

namespace GrowthWare.WebApplication.Functions.System.ExternalAuth
{
    public partial class OpenAuthProviderLogon : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Response.SuppressFormsAuthenticationRedirect = true;
            //var provider = Request.Form["provider"];
            var provider = GWWebHelper.GetQueryValue(Request, "provider");
            if (provider == null)
            {
                return;
            }
            // Request a redirect to the external login provider
            string redirectUrl = ResolveUrl(String.Format(CultureInfo.InvariantCulture, "~/#?Action=RegisterExternalLogin&{0}={1}&returnUrl={2}", IdentityHelper.ProviderNameKey, provider, ReturnUrl));
            var properties = new AuthenticationProperties() { RedirectUri = redirectUrl };
            // Add xsrf verification when linking accounts
            if (Context.User.Identity.IsAuthenticated)
            {
                properties.Dictionary[IdentityHelper.XsrfKey] = Context.User.Identity.GetUserId();
            }
            Context.GetOwinContext().Authentication.Challenge(properties, provider);
            Response.StatusCode = 401;
            Response.End();
        }

        public string ReturnUrl { get; set; }

        public IEnumerable<string> GetProviderNames()
        {
            return Context.GetOwinContext().Authentication.GetExternalAuthenticationTypes().Select(t => t.AuthenticationType);
        }
    }
}