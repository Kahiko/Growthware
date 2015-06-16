using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.IO;
using System.Web;

namespace GrowthWare.WebApplication
{
    public partial class _Default : BaseWebpage
    {

        protected new void Page_PreInit(object sender, EventArgs e)
        {
            base.Page_PreInit(sender, e);
            if (ConfigSettings.ForceHttps && HttpContext.Current.Request.Url.Scheme.ToLowerInvariant() != "https")
            {
                Response.Redirect(ConfigSettings.RootSite);
            }
            MSecurityEntityProfile mSecProfile = SecurityEntityUtility.CurrentProfile();
            String mMasterPage = "Content/Skins/" + mSecProfile.Skin + "/" + mSecProfile.Skin + ".master";
            string fileName = null;
            fileName = HttpContext.Current.Server.MapPath("~\\") + mMasterPage;
            if (!File.Exists(fileName))
            {
                mMasterPage = "Content/Skins/Default/Default.Master";
            }
            this.MasterPageFile = mMasterPage;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}