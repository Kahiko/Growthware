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
        BCommunityCalendar mCommunityCalendar = new BCommunityCalendar(securityEntityProfile, ConfigSettings.CentralManagement);
        DataTable mDataTable = mCommunityCalendar.GetEvents(functionSeqId, startDate, endDate);
        foreach (DataRow item in mDataTable.Rows)
        {
            MCalendarEvent mCalendarEvent = new MCalendarEvent(item);
            mRetVal.Add(mCalendarEvent);
        }
        return mRetVal;
    }
}