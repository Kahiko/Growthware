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
    public MDBInformation GetDBInformation()
    {
        MDBInformation mRetVal = DBInformationUtility.DBInformation();
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
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
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

    [HttpPost("UpdateProfile")]
    public ActionResult<bool> UpdateProfile(int enableInheritance)
    {
        bool mRetVal = false;
        MDBInformation mProfile = DBInformationUtility.DBInformation();
        mProfile.EnableInheritance = enableInheritance;
        mRetVal = DBInformationUtility.UpdateProfile(mProfile);
        return Ok(mRetVal);
    }
}