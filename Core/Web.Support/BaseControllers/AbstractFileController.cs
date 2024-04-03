using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Web.Support.Jwt;
using Microsoft.AspNetCore.StaticFiles;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractFileController : ControllerBase
{
    string m_TempUploadDirectory = "tempUpload" + Path.DirectorySeparatorChar;

    private static Logger m_Logger = Logger.Instance();

    /// <summary>
    /// Calculates the path given the directory and the selected (or desired) path
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="selectedPath"></param>
    /// <returns>string</returns>
    private string calculatePath(string directory, string selectedPath)
    {
        string mRetVal = string.Empty;
        if (selectedPath != null) { mRetVal = selectedPath; }
        string mDirectory = directory.Replace(@"\", @"/");
        mDirectory = mDirectory.Replace(@"/", Path.DirectorySeparatorChar.ToString());
        mDirectory = mDirectory.TrimEnd(Path.DirectorySeparatorChar);
        mDirectory = mDirectory.TrimStart(Path.DirectorySeparatorChar);
        if (!mDirectory.EndsWith(Path.DirectorySeparatorChar)) { mDirectory = mDirectory + Path.DirectorySeparatorChar.ToString(); }

        string mSelectedPath = string.Empty;
        if (!string.IsNullOrEmpty(selectedPath))
        {
            mSelectedPath = selectedPath.Replace(@"\", @"/");
            mSelectedPath = mSelectedPath.Replace(@"/", Path.DirectorySeparatorChar.ToString());
            mSelectedPath = mSelectedPath.TrimEnd(Path.DirectorySeparatorChar);
            mSelectedPath = mSelectedPath.TrimStart(Path.DirectorySeparatorChar);
        }
        mRetVal = mDirectory + mSelectedPath;
        if (mRetVal.LastIndexOf(Path.DirectorySeparatorChar) == 0) { mRetVal = mDirectory; }
        if (!mRetVal.EndsWith(Path.DirectorySeparatorChar)) { mRetVal = mRetVal + Path.DirectorySeparatorChar.ToString(); }
        return mRetVal;
    }

    /// <summary>
    /// Creates a new directory.
    /// </summary>
    /// <param name="action">The action to perform.</param>
    /// <param name="selectedPath">The selected path.</param>
    /// <param name="newPath">The new path.</param>
    /// <returns>An ActionResult indicating the success or failure of the operation.</returns>
    [HttpPost("CreateDirectory")]
    public ActionResult CreateDirectory(string action, string selectedPath, string newPath)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayAdd)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            string mCurrentPath = this.calculatePath(mDirectoryProfile.Directory, selectedPath);
            string mNewDirectoryName = Path.Combine(mCurrentPath, newPath);
            DirectoryInfo mDirectoryInfo = new DirectoryInfo(mNewDirectoryName);
            if (!mDirectoryInfo.Exists)
            {
                mDirectoryInfo.Create();
            }
            return Ok();
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    /// <summary>
    /// Deletes a directory.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="selectedPath">The selected path.</param>
    /// <returns>An ActionResult<bool> indicating the success of the operation.</returns>
    [HttpDelete("DeleteDirectory")]
    public ActionResult<bool> DeleteDirectory(string action, string selectedPath)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayDelete)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            string mFullPath = this.calculatePath(mDirectoryProfile.Directory, selectedPath);
            if (mFullPath != mDirectoryProfile.Directory)
            {
                DirectoryInfo mDirectoryInfo = new DirectoryInfo(mFullPath);
                recursiveDelete(mDirectoryInfo);
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
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayDelete)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            string mFileName = Path.Combine(this.calculatePath(mDirectoryProfile.Directory, selectedPath), fileName);
            if (System.IO.File.Exists(mFileName))
            {
                System.IO.File.Delete(mFileName);
                return Ok(true);
            }
            return StatusCode(StatusCodes.Status404NotFound, String.Format("The file '{0}' does not exists", mFileName));
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    /// <summary>
    /// Retrieves the content type of a file based on its full file name.
    /// </summary>
    /// <param name="fullFileName">The full file name of the file including the path.</param>
    /// <returns>The content type of the file.</returns>
    private string getContentType(string fullFileName)
    {
        FileExtensionContentTypeProvider mFileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
        string mContentType;
        if (!mFileExtensionContentTypeProvider.TryGetContentType(fullFileName, out mContentType))
        {
            mContentType = "application/octet-stream";
        }
        return mContentType;
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
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayView)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            DirectoryInfo mDirectoryInfo = new DirectoryInfo(mDirectoryProfile.Directory);
            try
            {
                if (mDirectoryInfo == null || !mDirectoryInfo.Exists)
                {
                    mDirectoryInfo.Create();
                }
                // https://stackoverflow.com/questions/24725775/converting-a-directory-structure-and-parsing-to-json-format-in-c-sharp
                MDirectoryTree mDirTree = new MDirectoryTree(mDirectoryInfo, mDirectoryProfile.Directory);
                string result = mDirTree.ToJson();
                return Ok(mDirTree);
            }
            catch (System.Exception ex)
            {
                Logger.Instance().Error(ex);
                return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
            }
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    /// <summary>
    /// Retrieves a file from the specified path and returns it as an IActionResult.
    /// </summary>
    /// <param name="action">The action of the function.</param>
    /// <param name="selectedPath">The selected path.</param>
    /// <param name="fileName">The name of the file to retrieve.</param>
    /// <returns>An IActionResult representing the retrieved file.</returns>
    [AllowAnonymous]
    [HttpGet("GetFile")]
    public IActionResult GetFile(string action, string selectedPath, string fileName)
    {
        if (action == null) throw new ArgumentNullException("action", "action cannot be a null reference (Nothing in Visual Basic)!");
        if (selectedPath == null) throw new ArgumentNullException("selectedPath", "selectedPath cannot be a null reference (Nothing in Visual Basic)!");
        if (fileName == null) throw new ArgumentNullException("fileName", "fileName cannot be a null reference (Nothing in Visual Basic)!");

        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayView)
        {
            // get the directory information from the directory profile
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            if (mDirectoryProfile != null)
            {
                string mPath = this.calculatePath(mDirectoryProfile.Directory, selectedPath);
                string mFilePath = Path.Combine(mPath, fileName);

                // Check if the file exists
                if (System.IO.File.Exists(mFilePath))
                {
                    // Set the appropriate content type based on the file extension
                    string mContentType = getContentType(mFilePath);
                    FileStream mFileStream = new(mFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    return File(mFileStream, mContentType, fileName);
                }
                else
                {
                    // File not found
                    return NotFound();
                }
            }
            return StatusCode(StatusCodes.Status404NotFound, "Could not determine the directory information");
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    /// <summary>
    /// Retrieves a list of files from the specified directory.
    /// </summary>
    /// <param name="action">The action to perform.</param>
    /// <param name="selectedPath">The selected path.</param>
    /// <returns>An ActionResult containing a list of FileInfoLight objects.</returns>
    [HttpGet("GetFiles")]
    public ActionResult<List<FileInfoLight>> GetFiles(string action, string selectedPath)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayView)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            string mPath = this.calculatePath(mDirectoryProfile.Directory, selectedPath);
            DirectoryInfo mDirectoryInto = new DirectoryInfo(mPath);
            List<FileInfoLight> mRetVal = new List<FileInfoLight>();
            if (mDirectoryInto.GetFiles() != null)
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

    /// <summary>
    /// This method is an HTTP POST endpoint that gets the line count of files in a directory.
    /// </summary>
    /// <param name="countInfo">An object of type UICountInfo that contains information about the directory, files to include, and files to exclude.</param>
    /// <returns>An ActionResult of type string that contains the line count of the files in the directory.</returns>
    [HttpPost("GetLineCount")]
    public ActionResult<string> GetLineCount(UICountInfo countInfo)
    {
        string mRetVal = string.Empty;
        int mDirectoryLineCount = 0;
        int mTotalLinesOfCode = 0;
        StringBuilder mSB = new StringBuilder();
        DirectoryInfo mCurrentDirectory = new DirectoryInfo(countInfo.TheDirectory);
        String[] mFileArray = countInfo.IncludeFiles.Split(',');
        String[] mExclusionArray = countInfo.ExcludePattern.Split(',');
        List<String> mExcludeList = new List<String>();
        foreach (string item in mExclusionArray)
        {
            mExcludeList.Add(item.ToString().Trim().ToUpper());
        }
        mRetVal = FileUtility.GetLineCount(mCurrentDirectory, 0, mSB, mExcludeList, mDirectoryLineCount, ref mTotalLinesOfCode, mFileArray);
        return Ok(mRetVal);
    }

    /// <summary>
    /// Retrieves result for natural sorting.
    /// </summary>
    /// <param name="sortDirection">The direction to sort the data in.</param>
    /// <returns>An ActionResult containing the test result.</returns>
    /// <note>
    /// Used for testing
    /// </note>
    [HttpGet("GetTestNaturalSort")]
    public ActionResult<UITestNaturalSort> GetTestNaturalSort(string sortDirection)
    {
        UITestNaturalSort mRetVal = new UITestNaturalSort();
        DataTable mDataTable = new DataTable("MyTable");
        mDataTable.Columns.Add("col1", System.Type.GetType("System.String"));
        mDataTable.Columns.Add("col2", System.Type.GetType("System.String"));
        DataRow mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "Chapter(10)";
        mDataRow["col2"] = "Chapter(10)";
        mDataTable.Rows.Add(mDataRow);
        mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "Chapter 2 Ep 2-3";
        mDataRow["col2"] = "Chapter 2 Ep 2-3";
        mDataTable.Rows.Add(mDataRow);
        mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "Chapter 2 Ep 1-2";
        mDataRow["col2"] = "Chapter 2 Ep 1-2";
        mDataTable.Rows.Add(mDataRow);
        mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "";
        mDataRow["col2"] = "";
        mDataTable.Rows.Add(mDataRow);
        mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "Rocky(IV)";
        mDataRow["col2"] = "Rocky(IV)";
        mDataTable.Rows.Add(mDataRow);
        mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "Chapter(1)";
        mDataRow["col2"] = "Chapter(1)";
        mDataTable.Rows.Add(mDataRow);
        mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "Chapter(11)";
        mDataRow["col2"] = "Chapter(11)";
        mDataTable.Rows.Add(mDataRow);
        mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "Rocky(I)";
        mDataRow["col2"] = "Rocky(I)";
        mDataTable.Rows.Add(mDataRow);
        mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "Rocky(II)";
        mDataRow["col2"] = "Rocky(II)";
        mDataTable.Rows.Add(mDataRow);
        mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "Rocky(IX)";
        mDataRow["col2"] = "Rocky(IX)";
        mDataTable.Rows.Add(mDataRow);
        mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "Rocky(X)";
        mDataRow["col2"] = "Rocky(X)";
        mDataTable.Rows.Add(mDataRow);
        mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "Chapter(2)";
        mDataRow["col2"] = "Chapter(2)";
        mDataTable.Rows.Add(mDataRow);
        mDataRow = mDataTable.NewRow();
        mDataRow["col1"] = "Chapter 1 Ep 2-3";
        mDataRow["col2"] = "Chapter 1 Ep 2-3";
        mDataTable.Rows.Add(mDataRow);
        DataView mDataView = mDataTable.DefaultView;
        SortTable mSortTable = new Framework.SortTable();
        mSortTable.Sort(mDataTable, "col1", sortDirection);

        mRetVal.StartTime = mSortTable.StartTime.ToString();
        mRetVal.StopTime = mSortTable.StopTime.ToString();
        TimeSpan mSpan = mSortTable.StopTime.Subtract(mSortTable.StartTime);
        mRetVal.TotalMilliseconds = mSpan.Milliseconds.ToString();
        mDataView.Sort = "col1 " + sortDirection;

        List<MColumns> mDataTableList = mDataTable.AsEnumerable().Select(item => new MColumns
        {
            col1 = item["col1"].ToString(),
            col2 = item["col2"].ToString()
        }).ToList();
        mDataTableList.RemoveAt(0);
        List<MColumns> mDataViewList = mDataView.Table.AsEnumerable().Select(item => new MColumns
        {
            col1 = item["col1"].ToString(),
            col2 = item["col2"].ToString()
        }).ToList();

        mDataViewList.RemoveAt(0);
        mRetVal.DataTable = mDataTableList;
        mRetVal.DataView = mDataViewList;
        mDataTable.Dispose();
        mDataView.Dispose();
        return Ok(mRetVal);
    }

    /// <summary>
    /// Merges the contents of two files.
    /// </summary>
    /// <param name="file1">The first file.</param>
    /// <param name="file2">The second file.</param>
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

    /// <summary>
    /// Deletes a directory and all subdirectories and files
    /// </summary>
    /// <param name="directoryInfo"></param>
    private static void recursiveDelete(DirectoryInfo directoryInfo)
    {
        if (!directoryInfo.Exists)
        {
            return;
        }

        foreach (DirectoryInfo mDirectoryInfo in directoryInfo.EnumerateDirectories())
        {
            recursiveDelete(mDirectoryInfo);
        }
        FileInfo[] mFiles = directoryInfo.GetFiles();
        foreach (var mFileInfo in mFiles)
        {
            mFileInfo.IsReadOnly = false;
            mFileInfo.Delete();
        }
        directoryInfo.Delete();
    }

    /// <summary>
    /// Renames a directory.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="selectedPath">The selected path.</param>
    /// <param name="newName">The new name.</param>
    /// <returns>An ActionResult.</returns>
    [HttpPost("RenameDirectory")]
    public ActionResult RenameDirectory(string action, string selectedPath, string newName)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayEdit)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            if (mDirectoryProfile != null)
            {
                string[] mSelectedPathParts = selectedPath.Split(@"\");
                string mSelectedPath = "";
                if (mSelectedPathParts.Count() == 0) { mSelectedPathParts = selectedPath.Split(@"/"); }
                if (mSelectedPathParts.Count() > 2)
                {
                    mSelectedPath = selectedPath.Replace(mSelectedPathParts[mSelectedPathParts.Count() - 1], "");
                }
                string mOldDirectoryName = this.calculatePath(mDirectoryProfile.Directory, selectedPath);
                string mNewDirectoryName = this.calculatePath(mDirectoryProfile.Directory, mSelectedPath);
                mNewDirectoryName = Path.Combine(mNewDirectoryName, newName);
                if (mOldDirectoryName != mDirectoryProfile.Directory)
                {
                    Directory.Move(mOldDirectoryName, mNewDirectoryName);
                    return Ok();
                }
                return StatusCode(StatusCodes.Status403Forbidden, "Not allowed to change the root directory");
            }
            return StatusCode(StatusCodes.Status404NotFound, "Could not determine the directory information");
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    /// <summary>
    /// Renames a file.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="selectedPath">The selected path.</param>
    /// <param name="oldName">The old name.</param>
    /// <param name="newName">The new name.</param>
    /// <returns>An ActionResult.</returns>
    [HttpPost("RenameFile")]
    public ActionResult RenameFile(string action, string selectedPath, string oldName, string newName)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayEdit)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            if (mDirectoryProfile != null)
            {
                string mPath = this.calculatePath(mDirectoryProfile.Directory, selectedPath);
                string mOldFileName = mPath + oldName;
                string mNewFileName = mPath + newName;
                if (!System.IO.File.Exists(mOldFileName))
                {
                    return StatusCode(StatusCodes.Status404NotFound, String.Format("The file '{0}' does not exists", oldName));
                }
                if (System.IO.File.Exists(mNewFileName))
                {
                    return StatusCode(StatusCodes.Status409Conflict, String.Format("The file '{0}' already exists please delete it first", oldName));
                }
                System.IO.File.Move(mOldFileName, mNewFileName);
                return Ok();
            }
            return StatusCode(StatusCodes.Status404NotFound, "Could not determine the directory information");
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    /// <summary>
    /// An asynchronous action to facilitate Uploading a file.
    /// </summary>
    /// <returns>An IActionResult representing the result of the upload.</returns>
    [HttpPost("UploadFile")]
    public async Task<IActionResult> UploadFile()
    {
        string mAction = Request.Form["action"].ToString(); // Set in file-manager.service.ts - multiPartFileUpload or singleFileUpload
        string mSelectedPath = Request.Form["selectedPath"].ToString(); // Set in file-manager.service.ts - multiPartFileUpload or singleFileUpload
        if (string.IsNullOrEmpty(mAction)) { return StatusCode(StatusCodes.Status500InternalServerError, "Missing the 'action' property."); }
        if (mSelectedPath.Contains(":"))
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "The current path parameter can not contain a colon.");
        }
        try
        {
            MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
            MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(mAction);
            MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
            if (mSecurityInfo.MayAdd)
            {
                MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
                if (mDirectoryProfile != null)
                {
                    UploadResponse mRetVal = new UploadResponse();
                    mRetVal.IsSuccess = false;
                    string mUploadDirectory = this.calculatePath(mDirectoryProfile.Directory, mSelectedPath) + Path.DirectorySeparatorChar + this.m_TempUploadDirectory;
                    // create the upload directory if one doest exist
                    DirectoryInfo mDirectoryInfo = new DirectoryInfo(mUploadDirectory);
                    string mCompleted = Request.Form["completed"];
                    if (!mDirectoryInfo.Exists) { mDirectoryInfo.Create(); }
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
                        if (mSortedFiles != null && mSortedFiles.Count() > 0)
                        {
                            string mNewFileName = mSortedFiles[0].FullName.Replace(m_TempUploadDirectory, ""); // The original intended directory
                            mNewFileName = mNewFileName.Replace("_UploadNumber_1", ""); // strip off the _UploadNumber_1
                            if (System.IO.File.Exists(mNewFileName)) System.IO.File.Delete(mNewFileName);
                            for (int i = 0; i < mSortedFiles.Count(); i++)
                            {
                                mergeFiles(mNewFileName, mSortedFiles[i].FullName);
                            }
                            if (mDirectoryInfo.GetFiles().Count() == 0)
                            {
                                mDirectoryInfo.Delete();
                            }
                            mRetVal.FileName = mNewFileName.Replace(mDirectoryInfo.FullName.Replace(this.m_TempUploadDirectory, ""), "");
                            mRetVal.IsSuccess = true;
                            return Ok(mRetVal);
                        }
                    }
                    if (mRetVal.IsSuccess)
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
