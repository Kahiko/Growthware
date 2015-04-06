using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Data;
using System.IO;
using System.Web;

namespace GrowthWare.WebApplication.Functions.System.FileManagement
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {
		//http://osman.it/aspnet/jquery-ajax-file-upload/
		public void ProcessRequest(HttpContext context)
		{
			string mResponse = "Success";
			try
			{
				string mAction = GWWebHelper.GetQueryValue(context.Request, "Action");
				string mCurrentDirectory = GWWebHelper.GetQueryValue(context.Request, "CurrentDirectory");
				MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(mAction);
				MDirectoryProfile mDirectoryInfo = DirectoryUtility.GetProfileByFunction(mFunctionProfile.Id);
				string mDirectory = mDirectoryInfo.Directory;
				if (!string.IsNullOrEmpty(mCurrentDirectory)) 
				{
					if (mDirectory.LastIndexOf(Path.DirectorySeparatorChar) != mDirectory.Length) 
					{
						mDirectory += Path.DirectorySeparatorChar + mCurrentDirectory;
					}
					
				}
				string mUploadDirectory = "";
				try
				{
					mUploadDirectory = context.Request.Files.AllKeys[0].ToString().Substring(0, context.Request.Files.AllKeys[0].LastIndexOf("."));
				}
				catch (Exception)
				{
					
					mUploadDirectory = context.Request["fileName"].ToString();
					mUploadDirectory = mUploadDirectory.Substring(0, mUploadDirectory.LastIndexOf("."));
					mUploadDirectory = Path.DirectorySeparatorChar + mUploadDirectory;
				}
				if (!Directory.Exists(mUploadDirectory)) FileUtility.CreateDirectory(mDirectory, mUploadDirectory, mDirectoryInfo);
				mUploadDirectory = mDirectory + Path.DirectorySeparatorChar + mUploadDirectory;
				if (context.Request["completed"] == null)
				{
					if (mDirectoryInfo != null)
					{
						FileUtility.DoUpload(context.Request.Files.AllKeys[0], context.Request.Files[0], mUploadDirectory, mDirectoryInfo);
					}
				}
				else 
				{
					if (context.Request["completed"].ToString().ToLowerInvariant() == "true") 
					{
						string mFileName = context.Request["fileName"].ToString();
						string mPath = mUploadDirectory;
						string mNewpath = Path.Combine(mPath, mFileName);
						char mDirectorySeparatorChar = Path.DirectorySeparatorChar;
						if (mDirectoryInfo != null)
						{
							DataTable mFileTable = FileUtility.GetDirectoryTableData(mUploadDirectory, mDirectoryInfo, true);
							SortTable mSorter = new Framework.Common.SortTable();
							String mColName = "Name";
							mSorter.Sort(mFileTable, mColName, "ASC");

							DataView mFiles = mFileTable.DefaultView;
							mFiles.RowFilter = "Name like '" + mFileName.Substring(0, mFileName.Length - 4) + "%'";
							foreach (DataRowView rowView in mFiles)
							{
								DataRow row = rowView.Row;
								string mPartialFileName = mUploadDirectory + mDirectorySeparatorChar.ToString() + row["Name"].ToString();
								if (mPartialFileName.EndsWith("_UploadNumber_1"))
								{
									if (File.Exists(mNewpath))
									{
										File.Delete(mNewpath);
									}
								}
								if (mPartialFileName != mNewpath) 
								{
									mergeFiles(mNewpath, mPartialFileName);								
								}
							}
							FileUtility.RenameFile(mNewpath, mDirectory + mFileName, mDirectoryInfo);
							FileUtility.DeleteDirectory(mUploadDirectory,mDirectoryInfo);
						}
					
					}
				}
			}
			catch (Exception ex)
			{
				Logger mLog = Logger.Instance();
				mLog.Error(ex);
				mResponse = "Error";
			}
			context.Response.ContentType = "text/plain";
			context.Response.Write(mResponse);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private static void mergeFiles(string file1, string file2)
        {
            FileStream fs1 = null;
            FileStream fs2 = null;
            try
            {
                fs1 = File.Open(file1, FileMode.Append);
                fs2 = File.Open(file2, FileMode.Open);
                byte[] fs2Content = new byte[fs2.Length];
                fs2.Read(fs2Content, 0, (int)fs2.Length);
                fs1.Write(fs2Content, 0, (int)fs2.Length);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                fs1.Close();
                fs2.Close();
                File.Delete(file2);
            }
        }
    }
}