using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Framework.Models.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]

public abstract class AbstractCalendarController : ControllerBase
{
    private Logger m_Logger = Logger.Instance();

    /// <summary>
    /// Deletes an event
    /// </summary>
    /// <param name="calendarEventSeqId"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("DeleteEvent")]
    public async Task<ActionResult<bool>> DeleteEvent(int calendarEventSeqId, string action)
    {
        MCalendarEvent mCalendarEvent = await CalendarUtility.GetEvent(SecurityEntityUtility.CurrentProfile(), calendarEventSeqId);
        if (await getEventSecurity(mCalendarEvent))
        {
            MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
            if (mFunctionProfile != null)
            {
                MAccountProfile mAccountProfile = await AccountUtility.CurrentProfile();
                MSecurityInfo mSecurityInfo = new(mFunctionProfile, mAccountProfile);
                if (mSecurityInfo.MayView)
                {
                    return Ok(await CalendarUtility.DeleteEvent(SecurityEntityUtility.CurrentProfile(), calendarEventSeqId));
                }
            }
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
    public async Task<ActionResult<MCalendarEvent>> GetEvent(string action, int id)
    {
        /** 
          * This method is not strickly necessary for the frontend.  The main
          * purpose is to set the EditId in the session, to be used when
          * enforcing security during the save.
          */
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        if (mFunctionProfile != null)
        {
            MAccountProfile mAccountProfile = await AccountUtility.CurrentProfile();
            MSecurityInfo mSecurityInfo = new(mFunctionProfile, mAccountProfile);
            if (mSecurityInfo.MayView)
            {
                MCalendarEvent mRetVal = await CalendarUtility.GetEvent(SecurityEntityUtility.CurrentProfile(), id);
                HttpContext.Session.SetInt32("EditId", id);
                return Ok(mRetVal);
            }
            // return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
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
    public async Task<ActionResult<List<MCalendarEvent>>> GetEvents(string action, string startDate, string endDate)
    {
        /** 
          * The startDate and endDate parameters are needed because the data for a calendar is
          * is presented in the UI as whole weeks so some day's for the first week will be from
          * the previous month.  The same is true for the last week.
          */
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        if (mFunctionProfile != null)
        {
            MAccountProfile mAccountProfile = await AccountUtility.CurrentProfile();
            MSecurityInfo mSecurityInfo = new(mFunctionProfile, mAccountProfile);
            if (mSecurityInfo.MayView)
            {
                DateTime mStartDate = DateTime.Parse(startDate);
                DateTime mEndtDate = DateTime.Parse(endDate);
                List<MCalendarEvent> mRetVal = await CalendarUtility.GetEvents(SecurityEntityUtility.CurrentProfile(), mFunctionProfile.Id, mStartDate, mEndtDate);
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
    public async Task<ActionResult<MCalendarEvent>> SaveEvent(UISaveEventParameters parameters)
    {
        MAccountProfile mAccountProfile = await AccountUtility.CurrentProfile();
        UISaveEventParameters mParameters = parameters; // Bad practice to alter a parameter in a method.
        if (mParameters.calendarEvent.Id < 1)
        {
            mParameters.calendarEvent.AddedBy = mAccountProfile.Id;
            mParameters.calendarEvent.AddedDate = DateTime.Now;
        }
        else
        {
            mParameters.calendarEvent.UpdatedBy = mAccountProfile.Id;
            mParameters.calendarEvent.UpdatedDate = DateTime.Now;
        }
        MCalendarEvent mCalendarEvent = await CalendarUtility.GetEvent(SecurityEntityUtility.CurrentProfile(), mParameters.calendarEvent.Id);
        if (await getEventSecurity(mCalendarEvent, mParameters.action) && mParameters.calendarEvent.AddedBy == mAccountProfile.Id)
        {
            MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(mParameters.action);
            MCalendarEvent mRetVal = await CalendarUtility.SaveCalendarEvent(SecurityEntityUtility.CurrentProfile(), mFunctionProfile.Id, mParameters.calendarEvent);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetEventSecurity")]
    public async Task<ActionResult<Boolean>> GetEventSecurity(int calendarEventSeqId)
    {
        MCalendarEvent mCalendarEvent = await CalendarUtility.GetEvent(SecurityEntityUtility.CurrentProfile(), calendarEventSeqId);
        return Ok(getEventSecurity(mCalendarEvent));
    }

    private async Task<bool> getEventSecurity(MCalendarEvent calendarEvent, string action = null)
    {
        bool mRetVal = false;
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
        if (mSecurityEntity != null)
        {
            if (action == null)
            {
                if (AccountUtility.CurrentProfile().Id == calendarEvent.AddedBy)
                {
                    mRetVal = true;
                }
            }
            else
            {
                MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
                if (mFunctionProfile != null)
                {
                    MAccountProfile mAccountProfile = await AccountUtility.CurrentProfile();
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