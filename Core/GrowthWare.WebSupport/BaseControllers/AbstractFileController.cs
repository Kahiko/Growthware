using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.WebSupport.Utilities;

namespace GrowthWare.WebSupport.BaseControllers;

public class FileTreeManager
{
    private DirectoryInfo mDirectoryInfo;

    public FileTreeManager(string aEntryPoint)
    {
        SetEntryPoint(aEntryPoint);
    }

    public void SetEntryPoint(string aEntryPoint)
    {
        this.mDirectoryInfo = new DirectoryInfo(aEntryPoint);
    }

    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        foreach (DirectoryInfo dir in this.mDirectoryInfo.EnumerateDirectories())
        {
            result.Append("|-- " + dir.Name + Environment.NewLine);
        }
        return result.ToString();
    }
}

[CLSCompliant(false)]
public abstract class AbstractFileController : ControllerBase
{
    string m_TempUploadDirectory = "tempUpload" + Path.DirectorySeparatorChar;
    private static Logger m_Logger = Logger.Instance();

    // [Authorize("GetFiles")]
    // [HttpGet("GetFiles")]
    // public ActionResult<FileInfo[]> GetFiles()
    // {
    //     return Ok();
    // }

    [HttpGet("GetDirectories")]
    public ActionResult<MDirectoryTree> GetDirectories(string action, string selectedPath)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if(mSecurityInfo.MayView)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            // https://stackoverflow.com/questions/24725775/converting-a-directory-structure-and-parsing-to-json-format-in-c-sharp
            MDirectoryTree mDirTree = new MDirectoryTree(new DirectoryInfo(mDirectoryProfile.Directory), mDirectoryProfile.Directory);
            string result =  mDirTree.ToJson();
            return Ok(mDirTree);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [HttpGet("GetFiles")]
    public ActionResult<List<FileInfoLight>> GetFiles(string action, string selectedPath)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if(mSecurityInfo.MayView)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            string mPath = this.calculatePath(mDirectoryProfile.Directory, selectedPath);
            DirectoryInfo mDirectoryInto = new DirectoryInfo(mPath);
            List<FileInfoLight> mRetVal = new List<FileInfoLight>();
            if(mDirectoryInto.GetFiles() != null)
            {
                foreach (FileInfo item in mDirectoryInto.GetFiles())
                {
                    mRetVal.Add(new FileInfoLight(item));
                }
            }
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    private string calculatePath(string directory, string selectedPath)
    {
        string mRetVal = string.Empty;
        if(selectedPath != null) { mRetVal = selectedPath; }
        if(!mRetVal.StartsWith(Path.DirectorySeparatorChar) && !directory.EndsWith(Path.DirectorySeparatorChar)) { mRetVal = Path.DirectorySeparatorChar.ToString() + mRetVal; }
        mRetVal = directory + mRetVal;
        if(mRetVal.LastIndexOf(Path.DirectorySeparatorChar) == 0) { mRetVal = directory; }
        return mRetVal;
    }

    private static void mergeFiles(string file1, string file2)
    {
        FileStream mFileStream1 = null;
        FileStream mFileStream2 = null;
        try
        {
            mFileStream1 = System.IO.File.Open(file1, FileMode.Append);
            mFileStream2 = System.IO.File.Open(file2, FileMode.Open);
            byte[] fs2Content = new byte[mFileStream2.Length];
            mFileStream2.Read(fs2Content, 0, (int)mFileStream2.Length);
            mFileStream1.Write(fs2Content, 0, (int)mFileStream2.Length);
        }
        catch (Exception ex)
        {
            m_Logger.Error(ex.Message);
        }
        finally
        {
            if (mFileStream1 != null) mFileStream1.Close();
            if (mFileStream2 != null) mFileStream2.Close();
            System.IO.File.Delete(file2);
        }
    }

    [HttpPost("RenameFile")]
    public ActionResult RenameFile(string action, string oldName, string newName)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile); 
        if(mSecurityInfo.MayEdit)
        {
            return Ok();
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [HttpPost("UploadFile")]
    public async Task<IActionResult> UploadFile()
    {
        string mAction = Request.Form["action"].ToString(); // Set in file-manager.service.ts - multiPartFileUpload or singleFileUpload
        string mSelectedPath = Request.Form["selectedPath"].ToString(); // Set in file-manager.service.ts - multiPartFileUpload or singleFileUpload
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(mAction);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile); 
        if(mSecurityInfo.MayAdd)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            if(mDirectoryProfile != null)
            {
                UploadResponse mRetVal = new UploadResponse();
                mRetVal.IsSuccess = false;
                try
                {
                    string mStartingDirectory = mDirectoryProfile.Directory;
                    string mUploadDirectory = "";
                    string mFullPath = string.Empty;
                    DirectoryInfo mDirectoryInfo = null;
                    if (mSelectedPath.Contains(":"))
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "The current path parameter can not contain a colon.");
                    }
                    mUploadDirectory = this.calculatePath(mDirectoryProfile.Directory, mSelectedPath);
                    if (!Directory.Exists(mUploadDirectory))
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "UploadFile is not intended to create directories.");
                    }
                    mUploadDirectory += this.m_TempUploadDirectory;
                    if (Request.Form.Files.Count() > 0)
                    {
                        // do the upload
                        IFormFile mFormFile = Request.Form.Files[0]; // we will only ever have one file
                        mRetVal.FileName = mFormFile.FileName;
                        if (mFormFile.FileName == "blob")
                        {
                            if (mFormFile.Name.EndsWith("_UploadNumber_1"))
                            {
                                mDirectoryInfo = new DirectoryInfo(mUploadDirectory);
                                if(mDirectoryInfo.Exists)
                                {
                                    mDirectoryInfo.Delete(true);
                                    mDirectoryInfo.Refresh();
                                    while (mDirectoryInfo.Exists)
                                    {
                                        System.Threading.Thread.Sleep(100);
                                        mDirectoryInfo.Refresh();
                                    }
                                }
                                Task mCreateDirTask = Task.Run(() => mDirectoryInfo.Create());
                                await Task.WhenAll(mCreateDirTask);
                            }
                            mFullPath = mUploadDirectory + mFormFile.Name;
                        }
                        else
                        {
                            mFullPath = mUploadDirectory.Replace(this.m_TempUploadDirectory, "") + mFormFile.FileName;
                        }
                        if (System.IO.File.Exists(mFullPath)) System.IO.File.Delete(mFullPath);
                        using (var stream = new FileStream(mFullPath, FileMode.Create))
                        {
                            await mFormFile.CopyToAsync(stream);
                        }
                        mRetVal.Data = "Successfully uploaded";
                        mRetVal.IsSuccess = true;
                    }
                    if (!String.IsNullOrWhiteSpace(Request.Form["completed"]) && Request.Form["completed"].ToString().ToLowerInvariant() == "true")
                    {
                        mDirectoryInfo = new DirectoryInfo(mUploadDirectory);
                        string mFileName = Request.Form["fileName"].ToString(); // Set in file-manager.service.ts - multiPartFileUpload or singleFileUpload
                        // merge all of the together
                        FileInfo[] mSortedFiles = mDirectoryInfo.GetFiles().OrderBy(x => x.Name).ToArray();
                        if (mSortedFiles.Count() > 0)
                        {
                            // Determine the new file name using the first full name in the sorted files
                            string mNewFileName = mSortedFiles[0].FullName.Replace(m_TempUploadDirectory, ""); // The original intended directory
                            mNewFileName = mNewFileName.Replace("_UploadNumber_1", ""); // strip off the _UploadNumber_1
                            if (System.IO.File.Exists(mNewFileName)) System.IO.File.Delete(mNewFileName);
                            for (int i = 0; i < mSortedFiles.Count(); i++)
                            {
                                mergeFiles(mNewFileName, mSortedFiles[i].FullName);
                            }
                            // delete the temp upload directory if no more files need to be merged
                            mDirectoryInfo = new DirectoryInfo(mUploadDirectory);
                            if (mDirectoryInfo.GetFiles().Count() == 0)
                            {
                                mDirectoryInfo.Delete();
                            }
                            mRetVal.Data = "Successfully uploaded";
                            mRetVal.FileName = mNewFileName.Replace(mStartingDirectory + Path.DirectorySeparatorChar + mSelectedPath, "");
                            mRetVal.IsSuccess = true;
                        }
                        else
                        {
                            System.IO.File.Move(mFullPath, mFullPath.Replace(m_TempUploadDirectory, ""));
                            mRetVal.Data = "Successfully uploaded";
                            mRetVal.FileName = mFileName.Replace(mStartingDirectory, "");
                            mRetVal.IsSuccess = true;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    m_Logger.Error(ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, "UploadFile failed.");
                }
                return Ok(mRetVal);
            }
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }
}
