using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using GrowthWare.Framework;
using System.Collections.Generic;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]

public abstract class AbstractCalendarController : ControllerBase
{
    private Logger m_Logger = Logger.Instance();

    [AllowAnonymous]
    [HttpGet("DeleteEvent")]
    public ActionResult<bool> DeleteEvent(int calendarEventSeqId) 
    { 
        if (getEventSecurity(calendarEventSeqId))
        {
            return Ok(true); 
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetEvents")]
    public ActionResult<List<MCalendarEvent>> GetEvents(string action, string startDate, string endDate)
    {
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        if(mFunctionProfile != null)
        {
            MAccountProfile mAccountProfile = AccountUtility.CurrentProfile;
            MSecurityInfo mSecurityInfo = new(mFunctionProfile, mAccountProfile);
            if(mSecurityInfo.MayView) 
            {
                DateTime mStartDate = DateTime.Parse(startDate);
                DateTime mEndtDate = DateTime.Parse(endDate);
                List<MCalendarEvent> mRetVal = CalendarUtility.GetEvents(SecurityEntityUtility.CurrentProfile(), mFunctionProfile.FunctionTypeSeqId, mStartDate, mEndtDate);
                return Ok(mRetVal);
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    } 

    [AllowAnonymous]
    [HttpGet("GetEventSecurity")]
    public ActionResult<Boolean> GetEventSecurity(int calendarEventSeqId)
    {
        return Ok(getEventSecurity(calendarEventSeqId));
    }

    private bool getEventSecurity(int calendarEventSeqId)
    {
        bool mRetVal = false;
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
        if (mSecurityEntity != null) 
        {
            MCalendarEvent mCalendarEvent = CalendarUtility.GetEvent(mSecurityEntity, calendarEventSeqId);
            if(mCalendarEvent != null)
            {
                if (AccountUtility.CurrentProfile.Id == mCalendarEvent.AddedBy) 
                {
                    mRetVal = true;
                }
            }
        }
        return mRetVal;
    }
}