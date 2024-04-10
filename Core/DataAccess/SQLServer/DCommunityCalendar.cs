using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.DataAccess.SQLServer
{
     public class DCommunityCalendar : AbstractDBInteraction, ICommunityCalendar
     {
        private int m_CalendarSeqId = -2;
        private int m_SecurityEntitySeqId = -2;

        int ICommunityCalendar.CalendarSeqId { get{return this.m_CalendarSeqId;} set{this.m_CalendarSeqId = value;} }
        int ICommunityCalendar.SecurityEntitySeqId { get{return this.m_SecurityEntitySeqId;} set{this.m_SecurityEntitySeqId = value;} }

        bool ICommunityCalendar.DeleteCalendar(int calendarSeqId) 
        {
            this.checkValid();
            return true;
        }

		bool ICommunityCalendar.DeleteEvent(int calendarEventSeqId) 
        {
            this.checkValid();
            return true;
        }

        bool ICommunityCalendar.GetCalendar(int calendarSeqId) 
        {
            this.checkValid();
            return true;
        }

        DataRow ICommunityCalendar.GetEvent(int calendarEventSeqId)
        {
            this.checkValid();
            string mStoredProcedure = "[ZGWOptional].[Get_Calendar_Event]";
            SqlParameter[] mParameters = 
            {
                new ("@P_CalendarEventSeqId", calendarEventSeqId),
            };
            try
            {
                return base.GetDataRow(mStoredProcedure, mParameters);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

		DataTable ICommunityCalendar.GetEvents(int functionSeqId, DateTime startDate, DateTime endDate) 
        {
            this.checkValid();
            string mStoredProcedure = "[ZGWOptional].[Get_Calendar_Events]";
            SqlParameter[] mParameters =
			 {
                    new ("@P_FunctionSeqId", functionSeqId),
                    new ("@P_Start_Date", startDate),
                    new ("@P_End_Date", endDate),
			 };
            try
            {
                return base.GetDataTable(mStoredProcedure, mParameters);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        bool ICommunityCalendar.SaveCalendar(MCalendar calendar) 
        {
            this.checkValid();
            return true;
        }

		DataRow ICommunityCalendar.SaveCalendarEvent(int functionSeqId, MCalendarEvent calendarEvent) 
        {
            this.checkValid();
            string mStoredProcedure = "[ZGWOptional].[Set_Calendar_Event]";
            SqlParameter[] mParameters =
			 {
                    new ("@P_CalendarEventSeqId", calendarEvent.Id),
                    new ("@P_FunctionSeqId", functionSeqId),
                    new ("@P_Title", calendarEvent.Title),
                    new ("@P_Start", DateTime.Parse(calendarEvent.Start)),
                    new ("@P_End", DateTime.Parse(calendarEvent.End)),
                    new ("@P_AllDay", calendarEvent.AllDay),
                    new ("@P_Description", calendarEvent.Description),
                    new ("@P_Color", calendarEvent.Color),
                    new ("@P_Link", calendarEvent.Link),
                    new ("@P_Location", calendarEvent.Location),
                    new ("@P_Added_Updated_By", calendarEvent.AddedBy),
			 };
            try
            {
                return base.GetDataRow(mStoredProcedure, mParameters);
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
            if(this.m_CalendarSeqId == -2)
            {
                throw new DataAccessLayerException("CalendarSeqId property must be set before calling methods from this class");
            }
            if(this.m_SecurityEntitySeqId == -2)
            {
                throw new DataAccessLayerException("SecurityEntitySeqID property must be set before calling methods from this class");
            }
        }
     }
#endregion
}