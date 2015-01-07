using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Owin;
using GrowthWare.WebApplication.Models;
using GrowthWare.Framework.Common;

namespace GrowthWare.WebApplication.App_Start
{
    public partial class Startup
    {

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301883
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(20),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Add settings based on the web.config file.
            if (ConfigSettings.GetAppSettingValue("EnableMicrosoftAccountAuthentication", true).ToUpperInvariant() == "TRUE") 
            { 
                app.UseMicrosoftAccountAuthentication(
                    clientId: ConfigSettings.GetAppSettingValue("MicrosoftAccountClientId", true),
                    clientSecret: ConfigSettings.GetAppSettingValue("MicrosoftAccountClientSecret", true));            
            }
            if (ConfigSettings.GetAppSettingValue("EnableTwitterAuthentication", true).ToUpperInvariant() == "TRUE") 
            {
                app.UseTwitterAuthentication(
                   consumerKey: ConfigSettings.GetAppSettingValue("TwitterConsumerKey", true),
                   consumerSecret: ConfigSettings.GetAppSettingValue("TwitterConsumerSecret", true));
            }
            if (ConfigSettings.GetAppSettingValue("EnableFacebookAuthentication", true).ToUpperInvariant() == "TRUE")
            {
                app.UseFacebookAuthentication(
                   appId: ConfigSettings.GetAppSettingValue("FacebookAppId", true),
                   appSecret: ConfigSettings.GetAppSettingValue("FacebookAppSecret", true));
            }
            if (ConfigSettings.GetAppSettingValue("EnableGoogleAuthentication", true).ToUpperInvariant() == "TRUE")
            {
                app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                {
                    ClientId = ConfigSettings.GetAppSettingValue("GoogleClientId", true),
                    ClientSecret = ConfigSettings.GetAppSettingValue("GoogleClientSecret", true)
                });
            }
        }
    }
}