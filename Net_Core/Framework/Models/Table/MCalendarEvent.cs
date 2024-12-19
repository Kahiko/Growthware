using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;
/// <summary>
/// Properties for a calendar event.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MCalendarEvent : AbstractBaseModel
{
    #region Public Properties
        public bool AllDay {get; set;}

        public string Color {get; set;}

        public string Description {get; set;}

        /// <summary>
        /// The expected format is ISO 8601.  Example '2024-04-18T14:00:00.000Z'
        /// </summary>
        public string End {get; set;}

        public string Link {get; set;}

        public string Location {get; set;}

        public string Owner {get; set;}

        /// <summary>
        /// The expected format is ISO 8601.  Example '2024-04-18T14:00:00.000Z'
        /// </summary>
        public string Start {get; set;}

        public string Title {get; set;}
    #endregion

    public MCalendarEvent()
    {
        this.IdColumnName = "CalendarEventSeqId";
        this.NameColumnName = "Name";
    }

    public MCalendarEvent(DataRow detailRow)
    {
        this.IdColumnName = "CalendarEventSeqId";
        this.NameColumnName = "Name";
        this.Initialize(detailRow);
    }

    protected new void Initialize(DataRow dataRow)
    {
        base.Initialize(dataRow);
        this.AllDay = base.GetBool(dataRow, "AllDay");
        this.Color = base.GetString(dataRow, "Color");
        this.Description = base.GetString(dataRow, "Description");
        this.End = base.GetDateTime(dataRow, "End", DateTime.Now).ToString();
        this.Link = base.GetString(dataRow, "Link");
        this.Location = base.GetString(dataRow, "Location");
        this.Owner = base.GetString(dataRow, "Owner");
        this.Start = base.GetDateTime(dataRow, "Start", DateTime.Now).ToString();
        this.Title = base.GetString(dataRow, "Title");
    }
}