using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.UserControls
{
    public partial class FileManagerControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SearchControl.ShowAddLink = false;
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetProfile(FunctionUtility.CurrentProfile().Id);
            String mLinks = GrowthWare.WebApplication.Functions.System.FileManagement.FileManager.GetDirectoryLinks("/", mDirectoryProfile.FunctionSeqId);
            directorySelector.InnerHtml = mLinks;
            MFunctionProfile mFunctionProfile = FunctionUtility.CurrentProfile();
            MAccountProfile mAccountProfile = AccountUtility.CurrentProfile();
            MSecurityInfo mSI = new MSecurityInfo(mFunctionProfile, mAccountProfile);
            UploadControl.Visible = mSI.MayAdd;
            tdNewDirectory.Visible = mSI.MayAdd;
            SearchControl.ShowDeleteAll = mSI.MayDelete;
            SearchControl.ShowSelect = mSI.MayDelete;
        }
    }
}