using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using GrowthWare.Framework.Common;

namespace GrowthWare.WebApplication.UserControls
{
    public partial class OpenAuthProviders : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigSettings.GetAppSettingValue("EnableThirdPartyAuthentication", true).ToUpperInvariant() == "TRUE") 
            {
                thirdPartyAuthentication.Visible = true;
                providerDetails.Visible = true;
            }
        }
        public string ReturnUrl { get; set; }

        public IEnumerable<string> GetProviderNames()
        {
            return Context.GetOwinContext().Authentication.GetExternalAuthenticationTypes().Select(t => t.AuthenticationType);
        }
    }
}