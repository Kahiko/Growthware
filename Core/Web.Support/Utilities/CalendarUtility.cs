using System;
using System.Collections.Generic;
using System.Data;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Utilities;

public static class CalendarUtility
{
    public static List<MCalendarEvent> GetEvents(MSecurityEntity securityEntityProfile, int functionSeqId, DateTime startDate, DateTime endDate) 
    {
        List<MCalendarEvent> mRetVal = [];
        BCommunityCalendar mCommunityCalendar = new(securityEntityProfile, ConfigSettings.CentralManagement);
        DataTable mDataTable = mCommunityCalendar.GetEvents(functionSeqId, startDate, endDate);
        foreach (DataRow item in mDataTable.Rows)
        {
            MCalendarEvent mCalendarEvent = new MCalendarEvent(item);
            mRetVal.Add(mCalendarEvent);
        }
        return mRetVal;
    }

    public static MCalendarEvent GetEvent(MSecurityEntity securityEntityProfile, int calendarEventSeqId) 
    {
        BCommunityCalendar mCommunityCalendar = new(securityEntityProfile, ConfigSettings.CentralManagement);
        return mCommunityCalendar.GetEvent(calendarEventSeqId);
    }

    public static MCalendarEvent SaveCalendarEvent(MSecurityEntity securityEntityProfile, int functionSeqId, MCalendarEvent calendarEvent)
    {
        BCommunityCalendar mCommunityCalendar = new(securityEntityProfile, ConfigSettings.CentralManagement);
        return mCommunityCalendar.SaveCalendarEvent(functionSeqId, calendarEvent);
    }
}