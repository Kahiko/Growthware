using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.FileManagement
{
    public partial class FileManager : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MFunctionProfile mFunctionProfile = FunctionUtility.CurrentProfile();
            String mScript = "<script type='text/javascript' language='javascript'>GW.FileManager.currentDirectory = '/'; GW.FileManager.currentFunctionSeqID=" + mFunctionProfile.Id.ToString() + "</script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", mScript);
        }
    }
}