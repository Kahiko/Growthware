using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;

namespace GrowthWare.DataAccess.Interfaces
{
    public interface ICommunityCalendar : IDBInteraction
    {
        int CalendarSeqId {get; set;}
        int SecurityEntitySeqId {get; set;}
		
        bool DeleteEvent(int calendarEventSeqId);
		
        DataTable GetEvents(int functionSeqId, DateTime startDate, DateTime endDate);
        
        DataRow GetEvent(int calendarEventSeqId);
        
		DataRow SaveCalendarEvent(int functionSeqId, MCalendarEvent calendarEvent);
    }
}
