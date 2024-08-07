using System;
using System.Collections.Generic;
using System.Data;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Utilities;

public static class CalendarUtility
{

    private static BCommunityCalendar m_BusinessLogic = null;

    public static List<MCalendarEvent> GetEvents(MSecurityEntity securityEntityProfile, int functionSeqId, DateTime startDate, DateTime endDate) 
    {
        List<MCalendarEvent> mRetVal = [];
        BCommunityCalendar mCommunityCalendar = getBusinessLogic();
        DataTable mDataTable = mCommunityCalendar.GetEvents(functionSeqId, startDate, endDate);
        foreach (DataRow item in mDataTable.Rows)
        {
            MCalendarEvent mCalendarEvent = new MCalendarEvent(item);
            mRetVal.Add(mCalendarEvent);
        }
        return mRetVal;
    }

    public static bool DeleteEvent(MSecurityEntity securityEntityProfile, int calendarEventSeqId)
    {
        BCommunityCalendar mCommunityCalendar = getBusinessLogic();
        return mCommunityCalendar.DeleteEvent(calendarEventSeqId);
    }

    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static BCommunityCalendar getBusinessLogic()
    {
        if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
        {
            m_BusinessLogic = new(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement);
        }
        return m_BusinessLogic;
    }

    public static MCalendarEvent GetEvent(MSecurityEntity securityEntityProfile, int calendarEventSeqId) 
    {
        BCommunityCalendar mCommunityCalendar = getBusinessLogic();
        return mCommunityCalendar.GetEvent(calendarEventSeqId);
    }

    public static MCalendarEvent SaveCalendarEvent(MSecurityEntity securityEntityProfile, int functionSeqId, MCalendarEvent calendarEvent)
    {
        BCommunityCalendar mCommunityCalendar = getBusinessLogic();
        return mCommunityCalendar.SaveCalendarEvent(functionSeqId, calendarEvent);
    }
}