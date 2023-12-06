using GrowthWare.DataAccess.Interfaces.Base;
using System;
using System.Data;

namespace GrowthWare.DataAccess.Interfaces
{
    public interface ICalendarData : IDBInteraction
    {
        string CalendarName {get; set;}
        int SecurityEntitySeqID {get; set;}

		bool GetCalendarData(ref DataSet calendarDataSet);
		bool SaveCalendarData(String comment, DateTime entryDate, int accountSeqId);
		bool DeleteCalendarData(String comment, DateTime entryDate, int accountSeqId);
    }
}
