using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.DataAccess.SQLServer
{
    public class DCommunityCalendar : AbstractDBInteraction, ICommunityCalendar
    {
        private string m_CalendarName = string.Empty;
        private int m_SecurityEntitySeqId = -2;

        public string CalendarName { get; set; }
        public int SecurityEntitySeqId { get{return this.m_SecurityEntitySeqId;} set{this.m_SecurityEntitySeqId = value;} }

        public bool DeleteCalendarData(string comment, DateTime entryDate, int accountSeqId)
        {
            this.checkValid();
            string mStoredProcedure = "[ZGWOptional].[Delete_Calendar_Data]";
            SqlParameter[] mParameters =
			 {
                    new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqId),
                    new SqlParameter("@P_Calendar_Name", m_CalendarName),
                    new SqlParameter("@P_Comment", comment),
                    new SqlParameter("@P_EntryDate", entryDate),
                    new SqlParameter("@P_ADDUPD_BY", accountSeqId)
			 };
             try
             {
                  base.ExecuteNonQuery(mStoredProcedure, mParameters);
                  return true;
             }
             catch (System.Exception)
             {
                 throw;
             }
        }

        public bool GetCalendarData(ref DataSet calendarDataSet, DateTime startDate, DateTime endDate)
        {
            this.checkValid();
            string mStoredProcedure = "[ZGWOptional].[Get_Calendar_Data]";
            SqlParameter[] mParameters =
			 {
                    new ("@P_SecurityEntitySeqId", this.m_SecurityEntitySeqId),
                    new ("@P_Calendar_Name", this.m_CalendarName),
                    new ("@P_Start_Date", startDate),
                    new ("@P_End_Date", endDate),
			 };
             try
             {
                  base.ExecuteNonQuery(mStoredProcedure, mParameters);
                  return true;
             }
             catch (System.Exception)
             {
                 throw;
             }
        }

        public bool SaveCalendarData(string comment, DateTime entryDate, int accountSeqId)
        {
            this.checkValid();
            string mStoredProcedure = "[ZGWOptional].[Set_Calendar_Data]";
            SqlParameter[] mParameters =
			 {
                    new ("@P_SecurityEntitySeqId", this.m_SecurityEntitySeqId),
                    new ("@P_Calendar_Name", this.m_CalendarName),
                    new ("@P_Comment", comment),
                    new ("@P_EntryDate", entryDate),
                    new ("@P_ADDUPD_BY", accountSeqId)
			 };
             try
             {
                  base.ExecuteNonQuery(mStoredProcedure, mParameters);
                  return true;
             }
             catch (System.Exception)
             {
                 throw;
             }
        }

#region "Private Methods"
        private void checkValid()
        {
            base.IsValid();
            if(String.IsNullOrEmpty(m_CalendarName) || String.IsNullOrWhiteSpace(m_CalendarName))
            {
                throw new DataAccessLayerException("CalendarName property must be set before calling methods from this class");
            }
            if(m_SecurityEntitySeqId == -2)
            {
                throw new DataAccessLayerException("SecurityEntitySeqID property must be set before calling methods from this class");
            }
        }
    }
#endregion
}