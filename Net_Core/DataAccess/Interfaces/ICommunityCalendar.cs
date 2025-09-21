using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.Interfaces;

public interface ICommunityCalendar : IDBInteraction
{
    int CalendarSeqId {get; set;}
    int SecurityEntitySeqId {get; set;}

    
    Task<bool> DeleteEvent(int calendarEventSeqId);
    
    Task<DataTable> GetEvents(int functionSeqId, DateTime startDate, DateTime endDate);
    
    Task<DataRow> GetEvent(int calendarEventSeqId);
    
    Task<DataRow> SaveCalendarEvent(int functionSeqId, MCalendarEvent calendarEvent);
}