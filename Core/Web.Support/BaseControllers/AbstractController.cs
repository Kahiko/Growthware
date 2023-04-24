using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Utilities;
using System.Collections.Generic;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractController : ControllerBase
{

    private string m_ApplicationName = string.Empty;
    private string m_Version = string.Empty;
    
    private Logger m_Logger = Logger.Instance();
    
    private string m_LogPriority = string.Empty;

    private Random m_Random = new Random(System.DateTime.Now.Millisecond);

    // // returns the current authenticated account (null if not logged in)
    // public MAccountProfile m_AccountProfile => (MAccountProfile)HttpContext.Items["AccountProfile"];
    // // returns the current authenticated accounts client choices (null if not logged in)
    // public MClientChoices m_ClientChoices => (MClientChoices)HttpContext.Items["ClientChoices"];
    // // returns the current security entity (default as defined in GrowthWare.json)
    // public MSecurityEntity m_SecurityEntity => (MSecurityEntity)HttpContext.Items["SecurityEntity"];

    [HttpGet("GetAppSettings")]
    public UIAppSettings GetAppSettings()
    {
        UIAppSettings mRetVal = new UIAppSettings();
        if(this.m_LogPriority == string.Empty)
        {
            this.m_LogPriority = ConfigSettings.LogPriority;
        }
        if(this.m_ApplicationName == string.Empty)
        {
            this.m_ApplicationName = ConfigSettings.AppDisplayedName;
        }
        if(this.m_Version == string.Empty)
        {
            this.m_Version = ConfigSettings.Version;
        }
        mRetVal.LogPriority = this.m_LogPriority;
        mRetVal.Name = this.m_ApplicationName;
        mRetVal.Version = this.m_Version;
        return mRetVal;
    }

    [HttpGet("GetGUID")]
    public ActionResult<string> GetGUID() 
    {
        string mRetVal = Guid.NewGuid().ToString();
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
        if (action == null || string.IsNullOrEmpty(action)) throw new ArgumentNullException("action", " can not be null or blank!");
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        if(mFunctionProfile != null) 
        {
            mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        }
        return Ok(mSecurityInfo);
    }


    [HttpPost("Log")]
    public bool Log(MLoggingProfile profile)
    {
        // MLoggingProfile mProfile = new MLoggingProfile(profile);
        LoggingUtility.Save(profile);
        return true;
    }
}