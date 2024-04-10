using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using GrowthWare.Framework;
using System.Collections.Generic;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Framework.Models.UI;

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

    /// <summary>
    /// Retrieves a single event from the database.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("GetEvent")]
    public ActionResult<MCalendarEvent> GetEvent(string action, int id)
    {
        /** 
          * This method is not strickly necessary for the frontend.  The main
          * purpose is to set the EditId in the session, to be used when
          * enforcing security during the save.
          */
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        if (mFunctionProfile != null)
        {
            MAccountProfile mAccountProfile = AccountUtility.CurrentProfile;
            MSecurityInfo mSecurityInfo = new(mFunctionProfile, mAccountProfile);
            if (mSecurityInfo.MayView)
            {
                MCalendarEvent mRetVal = CalendarUtility.GetEvent(SecurityEntityUtility.CurrentProfile(), id);
                HttpContext.Session.SetInt32("EditId", id);
                return Ok(mRetVal);
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    /// <summary>
    /// Gets a list of events from the database for the given calendar and date range.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("GetEvents")]
    public ActionResult<List<MCalendarEvent>> GetEvents(string action, string startDate, string endDate)
    {
        /** 
          * The startDate and endDate parameters are needed because the data for a calendar is
          * is presented in the UI as whole weeks so some day's for the first week will be from
          * the previous month.  The same is true for the last week.
          */
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        if (mFunctionProfile != null)
        {
            MAccountProfile mAccountProfile = AccountUtility.CurrentProfile;
            MSecurityInfo mSecurityInfo = new(mFunctionProfile, mAccountProfile);
            if (mSecurityInfo.MayView)
            {
                DateTime mStartDate = DateTime.Parse(startDate);
                DateTime mEndtDate = DateTime.Parse(endDate);
                List<MCalendarEvent> mRetVal = CalendarUtility.GetEvents(SecurityEntityUtility.CurrentProfile(), mFunctionProfile.Id, mStartDate, mEndtDate);
                return Ok(mRetVal);
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    /// <summary>
    /// Saves the event to the database.
    /// </summary>
    /// <param name="parameters">UISaveEventParameters</param>
    /// <returns>ActionResult<bool></returns>
    /// <remarks>
    /// The parameters.calendarEvent.Start and parameters.calendarEvent.End are expected to be in ISO 8601 format.
    /// Example '2024-04-18T14:00:00.000Z'
    /// Typescript example: calendarEvent.end = new Date(calendarEvent.end).toISOString();
    /// </remarks>
    [AllowAnonymous]
    [HttpPost("SaveEvent")]
    public ActionResult<MCalendarEvent> SaveEvent(UISaveEventParameters parameters)
    {
        MAccountProfile mAccountProfile = AccountUtility.CurrentProfile;
        if (getEventSecurity(parameters.calendarEvent.Id, parameters.action) && parameters.calendarEvent.AddedBy == mAccountProfile.Id)
        {
            MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(parameters.action);
            parameters.calendarEvent.AddedBy = mAccountProfile.Id;
            if (parameters.calendarEvent.Id < 1) 
            {
                parameters.calendarEvent.AddedBy = mAccountProfile.Id;
                parameters.calendarEvent.AddedDate = DateTime.Now;
            } else 
            {
                parameters.calendarEvent.UpdatedBy = mAccountProfile.Id;
                parameters.calendarEvent.UpdatedDate = DateTime.Now;
            }
            MCalendarEvent mRetVal = CalendarUtility.SaveCalendarEvent(SecurityEntityUtility.CurrentProfile(), mFunctionProfile.Id, parameters.calendarEvent);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetEventSecurity")]
    public ActionResult<Boolean> GetEventSecurity(int calendarEventSeqId)
    {
        return Ok(getEventSecurity(calendarEventSeqId));
    }

    private bool getEventSecurity(int calendarEventSeqId, string action = null)
    {
        bool mRetVal = false;
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
        if (mSecurityEntity != null)
        {
            MCalendarEvent mCalendarEvent = CalendarUtility.GetEvent(mSecurityEntity, calendarEventSeqId);
            if (action == null)
            {
                if (mCalendarEvent != null)
                {
                    if (AccountUtility.CurrentProfile.Id == mCalendarEvent.AddedBy)
                    {
                        mRetVal = true;
                    }
                }
            }
            else
            {
                MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
                if (mFunctionProfile != null)
                {
                    MAccountProfile mAccountProfile = AccountUtility.CurrentProfile;
                    MSecurityInfo mSecurityInfo = new(mFunctionProfile, mAccountProfile);
                    if (mSecurityInfo.MayView)
                    {
                        mRetVal = true;
                    }
                }

            }
        }
        return mRetVal;
    }
}