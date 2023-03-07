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

    /// <summary>
    /// Calculates the path given the directory and the selected (or desired) path
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="selectedPath"></param>
    /// <returns>string</returns>
    private string calculatePath(string directory, string selectedPath)
    {
        string mRetVal = string.Empty;
        if(selectedPath != null) { mRetVal = selectedPath; }
        if(!mRetVal.StartsWith(Path.DirectorySeparatorChar) && !directory.EndsWith(Path.DirectorySeparatorChar)) { mRetVal = Path.DirectorySeparatorChar.ToString() + mRetVal; }
        mRetVal = directory + mRetVal;
        if(mRetVal.LastIndexOf(Path.DirectorySeparatorChar) == 0) { mRetVal = directory; }
        return mRetVal;
    }

    [HttpDelete("DeleteDirectory")]
    public ActionResult<bool> DeleteDirectory(string action, string selectedPath)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if(mSecurityInfo.MayDelete)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            string mFullPath = this.calculatePath(mDirectoryProfile.Directory, selectedPath);
            if(mFullPath != mDirectoryProfile.Directory)
            {
                DirectoryInfo mDirectoryInfo = new DirectoryInfo(mFullPath);
                RecursiveDelete(mDirectoryInfo);
                return Ok(true);
            }
            return StatusCode(StatusCodes.Status403Forbidden, "The root directory can not be deleted");
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    /// <summary>
    /// Deletes a file (fileName) using the action and selected to determine the exact path the file is located in
    /// </summary>
    /// <param name="action">Used to determine both security and the root path</param>
    /// <param name="selectedPath">The path beyond the actions directory</param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [HttpDelete("DeleteFile")]
    public ActionResult<bool> DeleteFile(string action, string selectedPath, string fileName)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if(mSecurityInfo.MayDelete)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            string mFileName = Path.Combine(this.calculatePath(mDirectoryProfile.Directory, selectedPath), fileName);
            if(System.IO.File.Exists(mFileName))
            {
                System.IO.File.Delete(mFileName);
                return Ok(true);
            }
            return StatusCode(StatusCodes.Status404NotFound , String.Format("The file '{0}' does not exists", mFileName));
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    /// <summary>
    /// Returns hierarchical representing a directory structure
    /// </summary>
    /// <param name="action">Used to determine both security and the root path</param>
    /// <param name="selectedPath">The path beyond the actions directory</param>
    /// <returns>a json string representation of the MDirectoryTree object</returns>
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

    public static void RecursiveDelete(DirectoryInfo directoryInfo)
    {
        if (!directoryInfo.Exists)
        {
            return;
        }

        foreach (DirectoryInfo mDirectoryInfo in directoryInfo.EnumerateDirectories())
        {
            RecursiveDelete(mDirectoryInfo);
        }
        FileInfo[] mFiles = directoryInfo.GetFiles();
        foreach (var mFileInfo in mFiles)
        {
            mFileInfo.IsReadOnly = false;
            mFileInfo.Delete();
        }
        directoryInfo.Delete();
    }

    [HttpPost("RenameFile")]
    public ActionResult RenameFile(string action, string selectedPath, string oldName, string newName)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile); 
        if(mSecurityInfo.MayEdit)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            string mPath = this.calculatePath(mDirectoryProfile.Directory, selectedPath);
            string mOldFileName = mPath + oldName;
            string mNewFileName = mPath + newName;
            if(!System.IO.File.Exists(mOldFileName)) 
            {
                return StatusCode(StatusCodes.Status404NotFound , String.Format("The file '{0}' does not exists", oldName));
            }
            if(System.IO.File.Exists(mNewFileName)) 
            {
                return StatusCode(StatusCodes.Status409Conflict , String.Format("The file '{0}' already exists please delete it first", oldName));
            }
            System.IO.File.Move(mOldFileName, mNewFileName);
            return Ok();
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [HttpPost("UploadFile")]
    public async Task<IActionResult> UploadFile()
    {
        string mAction = Request.Form["action"].ToString(); // Set in file-manager.service.ts - multiPartFileUpload or singleFileUpload
        string mSelectedPath = Request.Form["selectedPath"].ToString(); // Set in file-manager.service.ts - multiPartFileUpload or singleFileUpload
        if(string.IsNullOrEmpty(mAction)) { return StatusCode(StatusCodes.Status500InternalServerError, "Missing the 'action' property."); }
        if (mSelectedPath.Contains(":"))
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "The current path parameter can not contain a colon.");
        }
        try
        {
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
                    string mUploadDirectory = this.calculatePath(mDirectoryProfile.Directory, mSelectedPath) + Path.DirectorySeparatorChar + this.m_TempUploadDirectory;
                    // create the upload directory if one doest exist
                    DirectoryInfo mDirectoryInfo = new DirectoryInfo(mUploadDirectory);
                    string mCompleted = Request.Form["completed"];
                    if(!mDirectoryInfo.Exists) { mDirectoryInfo.Create(); }
                    if (Request.Form.Files.Count() > 0)
                    {
                        // attempt the upload
                        IFormFile mFormFile = Request.Form.Files[0]; // there should only ever be one
                        mRetVal.FileName = mFormFile.Name;
                        string mFullPath = mUploadDirectory + mRetVal.FileName;
                        if (System.IO.File.Exists(mFullPath)) System.IO.File.Delete(mFullPath);
                        using (var stream = new FileStream(mFullPath, FileMode.Create))
                        {
                            await mFormFile.CopyToAsync(stream);
                        }
                        mRetVal.IsSuccess = true;
                    }
                    if (!String.IsNullOrWhiteSpace(mCompleted) && mCompleted.ToLowerInvariant() == "true")
                    {
                        // merge logic
                        string mFileName = Request.Form["fileName"];
                        // get file that start with the file name
                        FileInfo[] mSortedFiles = mDirectoryInfo.GetFiles().Where(f => f.Name.StartsWith(mFileName)).OrderBy(x => x.Name).ToArray();
                        if(mSortedFiles != null && mSortedFiles.Count() > 0)
                        {
                            string mNewFileName = mSortedFiles[0].FullName.Replace(m_TempUploadDirectory, ""); // The original intended directory
                            mNewFileName = mNewFileName.Replace("_UploadNumber_1", ""); // strip off the _UploadNumber_1
                            if (System.IO.File.Exists(mNewFileName)) System.IO.File.Delete(mNewFileName);
                            for (int i = 0; i < mSortedFiles.Count(); i++)
                            {
                                mergeFiles(mNewFileName, mSortedFiles[i].FullName);
                            }
                            if(mDirectoryInfo.GetFiles().Count() == 0) 
                            {
                                mDirectoryInfo.Delete();
                            }
                            mRetVal.FileName = mNewFileName.Replace(mDirectoryInfo.FullName.Replace(this.m_TempUploadDirectory, ""), "");
                            mRetVal.IsSuccess = true;
                            return Ok(mRetVal);
                        }
                    }
                    if(mRetVal.IsSuccess)
                    {
                        return Ok(mRetVal);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, "No file found in Request.Form.Files.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "UploadFile is not intended to create directories.");
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");            
        }
        catch (System.Exception ex)
        {
            Logger.Instance().Fatal(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "UploadFile Failed.");
        }
    }
}
