using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Utilities;

public static class CalendarUtility
{

    private static BCommunityCalendar m_BusinessLogic = null;

    public static async Task<List<MCalendarEvent>> GetEvents(MSecurityEntity securityEntityProfile, int functionSeqId, DateTime startDate, DateTime endDate) 
    {
        List<MCalendarEvent> mRetVal = [];
        DataTable mDataTable = await (await getBusinessLogic()).GetEvents(functionSeqId, startDate, endDate);
        foreach (DataRow item in mDataTable.Rows)
        {
            MCalendarEvent mCalendarEvent = new MCalendarEvent(item);
            mRetVal.Add(mCalendarEvent);
        }
        return mRetVal;
    }

    public static async Task<bool> DeleteEvent(MSecurityEntity securityEntityProfile, int calendarEventSeqId)
    {
        return await (await getBusinessLogic()).DeleteEvent(calendarEventSeqId);
    }

    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static async Task<BCommunityCalendar> getBusinessLogic()
    {
        if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
        {
            m_BusinessLogic = new(await SecurityEntityUtility.CurrentProfile());
        }
        return m_BusinessLogic;
    }

    public static async Task<MCalendarEvent> GetEvent(MSecurityEntity securityEntityProfile, int calendarEventSeqId) 
    {
        return await (await getBusinessLogic()).GetEvent(calendarEventSeqId);
    }

    public static async Task<MCalendarEvent> SaveCalendarEvent(MSecurityEntity securityEntityProfile, int functionSeqId, MCalendarEvent calendarEvent)
    {
        return await (await getBusinessLogic()).SaveCalendarEvent(functionSeqId, calendarEvent);
    }
}