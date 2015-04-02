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

        [WebMethod(EnableSession = false)]
        public static string GetDirectoryLinks(string currentDirectoryString, int functionSeqId)
        {
            HttpContext context = null;
            context = HttpContext.Current;
            string mCurrentDirectory = context.Server.UrlDecode(currentDirectoryString);
            StringBuilder mStringBuilder = new StringBuilder();
            StringWriter mStringWriter = new StringWriter(mStringBuilder);
            HtmlTextWriter mWriter = new HtmlTextWriter(mStringWriter);
            string mPath = string.Empty;
            HyperLink mFirstLink = new HyperLink();
            mFirstLink = new HyperLink();
            mFirstLink.Attributes.Add("href", "#");
            mFirstLink.Attributes.Add("onclick", string.Format("javascript:GW.FileManager.changeDirectory('{0}','{1}')", "/", functionSeqId));
            mFirstLink.Text = @"Home\";
            mFirstLink.RenderControl(mWriter);
            if (mCurrentDirectory.Length > 2)
            {

                Array mArray = mCurrentDirectory.Split('/');
                foreach (string item in mArray)
                {
                    if (item.Length > 0)
                    {
                        mPath += "/" + item;
                        HyperLink mLink = new HyperLink();
                        mLink.Attributes.Add("href", "#");
                        mLink.Attributes.Add("onclick", string.Format("javascript:GW.FileManager.changeDirectory('{0}','{1}')", mPath, functionSeqId));
                        mLink.Text = item + @"\";
                        mLink.RenderControl(mWriter);
                    }
                }
            }
            return mStringBuilder.ToString();
        }
    }
}