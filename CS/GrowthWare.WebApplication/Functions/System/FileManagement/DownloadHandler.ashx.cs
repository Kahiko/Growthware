using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Utilities;
using System;
using System.IO;
using System.Web;

namespace GrowthWare.WebApplication.Functions.System.FileManagement
{
    /// <summary>
    /// Summary description for DownloadHandler
    /// </summary>
    public class DownloadHandler : IHttpHandler
    {

        /// <summary>
        /// Enables processing of HTTP Web requests for downloading files that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>

        public void ProcessRequest(HttpContext context)
        {
            String mFilename = GWWebHelper.GetQueryValue(context.Request, "fileName");
            String mPath = GWWebHelper.GetQueryValue(context.Request, "thePath");
            String mFunctionSeqIDString = GWWebHelper.GetQueryValue(context.Request, "functionSeqID");
            if (!String.IsNullOrEmpty(mFilename) && !String.IsNullOrEmpty(mFunctionSeqIDString) && !String.IsNullOrEmpty(mPath))
            {
                int mFunctionSeqID = int.Parse(mFunctionSeqIDString);
                MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetProfile(mFunctionSeqID);
                context.Response.ContentType = "application/octet-stream";
                context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", mFilename));
                using (Stream s = new FileStream(mDirectoryProfile.Directory + mPath + "/" + mFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    s.CopyTo(context.Response.OutputStream);
                }
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Invalid filename");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}