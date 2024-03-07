using GrowthWare.DataAccess.Interfaces.Base;
using System;
using System.Data;

namespace GrowthWare.DataAccess.Interfaces
{
    public interface ICommunityCalendar : IDBInteraction
    {
        int CalendarSeqId {get; set;}
        int SecurityEntitySeqId {get; set;}

        bool DeleteCalendar(int calendarSeqId);
		bool DeleteEvent(int calendarEventSeqId);
        bool GetCalendar(int calendarSeqId);
		DataTable GetEvents(DateTime startDate, DateTime endDate);
        bool SaveCalendar(int functionSeqId, String comment, int accountSeqId);
		bool SaveEvent(String comment, DateTime entryDate, int accountSeqId);
    }
}
