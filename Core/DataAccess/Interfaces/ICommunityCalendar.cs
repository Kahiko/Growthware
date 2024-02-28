using GrowthWare.DataAccess.Interfaces.Base;
using System;
using System.Data;

namespace GrowthWare.DataAccess.Interfaces
{
    public interface ICommunityCalendar : IDBInteraction
    {
        string CalendarName {get; set;}
        int SecurityEntitySeqId {get; set;}

		bool GetCalendarData(ref DataSet calendarDataSet, DateTime startDate, DateTime endDate);
		bool SaveCalendarData(String comment, DateTime entryDate, int accountSeqId);
		bool DeleteCalendarData(String comment, DateTime entryDate, int accountSeqId);
    }
}
