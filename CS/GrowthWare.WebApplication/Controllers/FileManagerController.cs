using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GrowthWare.WebApplication.Controllers
{
    public class FileManagerController : ApiController
    {
        [HttpPost()]
        public IHttpActionResult CreateDirectory(string currentDirectoryString, int functionSeqId, string newDirectory) 
        {
            string mRetVal = "Unable to create directory";
            if (String.IsNullOrEmpty(currentDirectoryString) || String.IsNullOrEmpty(newDirectory))
            {
                mRetVal = "All parameters must be passed!";
                ArgumentException ex = new ArgumentException(mRetVal);
                Logger mLog = Logger.Instance();
                mLog.Error(mRetVal);
                throw (ex);
            }
            HttpServerUtility mServer = HttpContext.Current.Server;
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetProfile(functionSeqId);
            String mCurrentDirectory = mDirectoryProfile.Directory;
            if (currentDirectoryString.Length > 0)
            {
                mCurrentDirectory += @"\" + currentDirectoryString;
            }
            mRetVal = FileUtility.CreateDirectory(mServer.UrlDecode(mCurrentDirectory), mServer.UrlDecode(newDirectory), mDirectoryProfile);
            return this.Ok(mRetVal);
        }

        [HttpPost()]
        public IHttpActionResult DeleteFiles(List<UIFileInfo> filesToDelete) 
        {
            HttpServerUtility mServer = HttpContext.Current.Server;
            string mRetVal = "Done";
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetProfile(filesToDelete[0].FunctionSeqId);
            bool mExitLoop = false;
            foreach (UIFileInfo item in filesToDelete)
            {
                string mCurrentDirectory = mServer.UrlDecode(item.CurrentDirectory);
                if (mCurrentDirectory.Length == 0)
                {
                    mCurrentDirectory = mDirectoryProfile.Directory;
                }
                else
                {
                    mCurrentDirectory = mDirectoryProfile.Directory + mCurrentDirectory;
                }
                switch (item.FileType)
                {
                    case "File":
                        string mFileName = mCurrentDirectory += "/" + mServer.UrlDecode(item.FileName);
                        mRetVal = FileUtility.DeleteFile(mFileName, mDirectoryProfile);
                        if (mRetVal.IndexOf("Successfully") == -1) mExitLoop = true;
                        break;
                    case "Folder":
                        mCurrentDirectory += "/" + mServer.UrlDecode(item.FileName);
                        mRetVal = FileUtility.DeleteDirectory(mCurrentDirectory, mDirectoryProfile);
                        if (mRetVal.IndexOf("Successfully") == -1) mExitLoop = true;
                        break;
                    default:
                        break;
                }
                if (mExitLoop) break;
            }
            return this.Ok(mRetVal);
        }

        [HttpPost()]
        public IHttpActionResult GetDirectoryLinks(RequestDirectoryLinksInfo requestDirectoryInfo) 
        {
            string mRetVal = FileUtility.GetDirectoryLinks(requestDirectoryInfo.CurrentDirectoryString, requestDirectoryInfo.FunctionSeqId);
            return Ok(mRetVal);
        }
    }

    public class RequestDirectoryLinksInfo
    {
        public string CurrentDirectoryString { get; set; }
        public int FunctionSeqId { get; set; }
    }

    public class UIFileInfo
    {
        public string CurrentDirectory;
        public string FileName;
        public string FileType;
        public int FunctionSeqId;
    }
}
