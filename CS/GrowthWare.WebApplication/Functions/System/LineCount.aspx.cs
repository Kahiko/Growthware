using GrowthWare.WebSupport.BasePages;
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

namespace GrowthWare.WebApplication.Functions.System
{
    public partial class LineCount : BaseWebpage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int mDirectoryLineCount = 0;
            int mTotalLinesOfCode = 0;
            txtDirectoryName.Value = Server.MapPath("~\\");
            StringBuilder mSB = new StringBuilder();
            String[] mFileArray = txtFiles.Value.Split(',');
            String[] mExclusionArray = txtExclusionPattern.Value.ToString().Split(',');
            List<String> mExcludeList = new List<String>();
            foreach (string item in mExclusionArray)
            {
                mExcludeList.Add(item.ToString().Trim().ToUpper());
            }
            litLineCount.InnerHtml = "";
            litTotalLines.InnerHtml = "";
            DirectoryInfo currentDirectory = new DirectoryInfo(txtDirectoryName.Value);
            litLineCount.InnerHtml = FileUtility.GetLineCount(currentDirectory, 0, mSB, mExcludeList, mDirectoryLineCount, mTotalLinesOfCode, mFileArray);
        }

        [WebMethod(CacheDuration = 0, EnableSession = false)]
        public static string GetLineCount(CountInfo countInfo)
        {
            int mDirectoryLineCount = 0;
            int mTotalLinesOfCode = 0;
            StringBuilder mSB = new StringBuilder();
            String[] mFileArray = countInfo.IncludeFiles.Split(',');
            String[] mExclusionArray = countInfo.ExcludePattern.Split(',');
            List<String> mExcludeList = new List<String>();
            foreach (string item in mExclusionArray)
            {
                mExcludeList.Add(item.ToString().Trim().ToUpper());
            }
            DirectoryInfo currentDirectory = new DirectoryInfo(countInfo.TheDirectory);
            return FileUtility.GetLineCount(currentDirectory, 0, mSB, mExcludeList, mDirectoryLineCount, mTotalLinesOfCode, mFileArray);
        }
    }
	public class CountInfo
	{
		public string TheDirectory { get; set; }
		public string ExcludePattern { get; set; }
		public string IncludeFiles { get; set; }
	}
}