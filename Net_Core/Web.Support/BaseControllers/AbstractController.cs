using Microsoft.AspNetCore.Mvc;
using System;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Utilities;
using System.Collections.Generic;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Framework.Enumerations;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractController : ControllerBase
{

    private string m_ApplicationName = string.Empty;
    private string m_ChunkSize = string.Empty;
    private string m_Environment = string.Empty;
    private string m_Version = string.Empty;
    private Logger m_Logger = Logger.Instance();
    private string m_LogPriority = string.Empty;
    private string m_SecurityEntityTranslation = string.Empty;
    private Random m_Random = new Random(System.DateTime.Now.Millisecond);
    private string m_TempDownloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Temp");

    [Authorize("/sys_admin/searchDBLogs")]
    [HttpGet("CreateSystemLogs")]
    public async Task<ActionResult<string>> CreateSystemLogs()
    {
        try
        {
            // Generate unique ID for the zip file
            var mFileId = Guid.NewGuid().ToString();

            // Ensure temp directory exists
            if (!Directory.Exists(m_TempDownloadDirectory))
            {
                Directory.CreateDirectory(m_TempDownloadDirectory);
            }
            FileUtility.DeleteOlderFiles(m_TempDownloadDirectory, 1);

            var mZipFilePath = Path.Combine(m_TempDownloadDirectory, $"{mFileId}.zip");

            // Create the zip file with database logs and example text file
            await LoggingUtility.CreateSystemLogsZipAsync(mZipFilePath);

            // Return the file ID so the client can download it
            return Ok(mFileId);
        }
        catch (Exception ex)
        {
            Exception mException = new("Error creating log zip file", ex);
            m_Logger.Error(mException);
            return StatusCode(500, "Error creating log zip file");
        }
    }

    [Authorize("/sys_admin/searchDBLogs")]
    [HttpDelete("CleanupSystemLogs")]
    public ActionResult CleanupSystemLogs(string fileId)
    {
        var mFilePath = Path.Combine(m_TempDownloadDirectory, $"{fileId}.zip");
        if (!System.IO.File.Exists(mFilePath))
        {
            return NotFound();
        }
        System.IO.File.Delete(mFilePath);
        return Ok();
    }

    [Authorize("/sys_admin/searchDBLogs")]
    [HttpGet("DownloadSystemLogs")]
    public ActionResult DownloadSystemLogs(string fileId)
    {
        var mFilePath = Path.Combine(m_TempDownloadDirectory, $"{fileId}.zip");
        if (!System.IO.File.Exists(mFilePath))
        {
            return NotFound();
        }
        return File(System.IO.File.ReadAllBytes(mFilePath), "application/zip", "SystemLogs.zip");
    }

    [HttpGet("GetAppSettings")]
    public UIAppSettings GetAppSettings()
    {
        UIAppSettings mRetVal = new UIAppSettings();
        if (this.m_ChunkSize == string.Empty)
        {
            this.m_ChunkSize = ConfigSettings.RequestBodySize.ToString();
        }
        if(this.m_Environment == string.Empty)
        {
            this.m_Environment = ConfigSettings.Environment;
            this.m_Environment = this.m_Environment.Substring(0, this.m_Environment.Length - 1);
        }
        if (this.m_LogPriority == string.Empty)
        {
            this.m_LogPriority = ConfigSettings.LogPriority;
        }
        if (this.m_ApplicationName == string.Empty)
        {
            this.m_ApplicationName = ConfigSettings.AppDisplayedName;
        }
        if(this.m_SecurityEntityTranslation == string.Empty)
        {
            this.m_SecurityEntityTranslation = ConfigSettings.SecurityEntityTranslation;
        }
        if (this.m_Version == string.Empty)
        {
            this.m_Version = ConfigSettings.Version;
        }
        mRetVal.Environment = this.m_Environment;
        mRetVal.ChunkSize = this.m_ChunkSize;
        mRetVal.LogPriority = this.m_LogPriority;
        mRetVal.Name = this.m_ApplicationName;
        mRetVal.SecurityEntityTranslation = this.m_SecurityEntityTranslation;
        mRetVal.Version = this.m_Version;
        return mRetVal;
    }

    [HttpGet("GetDBInformation")]
    public async Task<MDBInformation> GetDBInformation()
    {
        MDBInformation mRetVal = await DBInformationUtility.DBInformation();
        return mRetVal;
    }

    [HttpGet("GetGUID")]
    public ActionResult<string> GetGUID()
    {
        string mRetVal = Guid.NewGuid().ToString();
        return Ok(mRetVal);
    }

    [HttpGet("GetLogLevel")]
    public ActionResult<int> GetLogLevel()
    {
        int mRetVal = this.m_Logger.CurrentLogLevel;
        return Ok(mRetVal);
    }

    [HttpGet("GetRandomNumbers")]
    public ActionResult<List<int>> GetRandomNumbers(int amountOfNumbers, int maxNumber, int minNumber)
    {
        List<int> mRetVal = new List<int>();
        for (int i = 0; i < amountOfNumbers; i++)
        {
            mRetVal.Add(this.m_Random.Next(minNumber, maxNumber));
        }
        return Ok(mRetVal);
    }

    [HttpGet("GetSecurityInfo")]
    public ActionResult<MSecurityInfo> GetSecurityInfo(string action)
    {
        MSecurityInfo mSecurityInfo = new MSecurityInfo();
        if (action == null || string.IsNullOrEmpty(action)) throw new ArgumentNullException(nameof(action), " can not be null or blank!");
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile();
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        if (mFunctionProfile != null)
        {
            mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        }
        return Ok(mSecurityInfo);
    }

    [Authorize("SetLogLevel")]
    [HttpPost("SetLogLevel")]
    public ActionResult<bool> SetLogLevel(int logLevel)
    {
        bool mRetVal = false;
        m_Logger.SetThreshold((LogPriority)logLevel);
        mRetVal = true;
        return Ok(mRetVal);
    }

    [HttpPost("Log")]
    public bool Log(MLoggingProfile profile)
    {
        if (profile.Destination != null) {
            if (profile.Destination.Contains(LogDestination.DB)) 
            {
                LoggingUtility.Save(profile);
            }
            if (profile.Destination.Contains(LogDestination.File)) 
            {
                this.m_Logger.Log(profile.Msg, (LogPriority)Enum.Parse(typeof(LogPriority), profile.Level));
            }
        } 
        else 
        {
            this.m_Logger.Log(profile.Msg, (LogPriority)Enum.Parse(typeof(LogPriority), profile.Level));
        }
        return true;
    }

    /// <summary>
    /// Performs a search for the [ZGWSystem].[Logging] based on the provided search criteria.
    /// </summary>
    /// <param name="searchCriteria">The criteria used to filter the search</param>
    /// <returns></returns>
    [Authorize("/sys_admin/searchDBLogs")]
    [HttpPost("SearchDBLogs")]
    public async Task<String> SearchDBLogs(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[Account], [Component], [ClassName], [Level], [LogDate], [LogSeqId], [MethodName], [Msg]";
        if (searchCriteria.sortColumns.Length > 0)
        {
            Tuple<string, string> mOrderByAndWhere = SearchUtility.GetOrderByAndWhere(mColumns, searchCriteria.searchColumns, searchCriteria.sortColumns, searchCriteria.searchText);
            string mOrderByClause = mOrderByAndWhere.Item1;
            string mWhereClause = mOrderByAndWhere.Item2;
            MSearchCriteria mSearchCriteria = new()
            {
                Columns = mColumns,
                OrderByClause = mOrderByClause,
                PageSize = searchCriteria.pageSize,
                SelectedPage = searchCriteria.selectedPage,
                TableOrView = "[ZGWSystem].[Logging]",
                WhereClause = mWhereClause
            };

            mRetVal = await SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;
    }

    [HttpPost("UpdateProfile")]
    public async Task<ActionResult<bool>> UpdateProfile(int enableInheritance)
    {
        bool mRetVal = false;
        MDBInformation mProfile = await DBInformationUtility.DBInformation();
        mProfile.EnableInheritance = enableInheritance;
        mRetVal = await DBInformationUtility.UpdateProfile(mProfile);
        return Ok(mRetVal);
    }
}