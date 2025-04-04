using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;
/// <summary>
/// Properties for a calendar event.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MCalendarEvent : AAddedUpdated
{

#region Public Properties
    public bool AllDay {get; set;}

    public int CalendarSeqId {get; set;}

    public string Color {get; set;}

    public string Description {get; set;}

    /// <summary>
    /// The expected format is ISO 8601.  Example '2024-04-18T14:00:00.000Z'
    /// </summary>
    public string End {get; set;}

    [DBIgnoreProperty]
    public override string ForeignKeyName => "CalendarSeqId";

    [DBPrimaryKey]
    [DBColumnName("CalendarEventSeqId")]
    public int Id {get; set;}

    [DBIgnoreProperty]
    public override bool IsForeignKeyNumeric => true;

    public string Link {get; set;}

    public string Location {get; set;}

    [DBIgnoreProperty]
    public string Owner {get; set;}

    [DBIgnoreProperty]
    public override string TableName => "[ZGWOptional].[Calendar_Events]";

    /// <summary>
    /// The expected format is ISO 8601.  Example '2024-04-18T14:00:00.000Z'
    /// </summary>
    public string Start {get; set;}

    public string Title {get; set;}
#endregion

    #region Constructors
        public MCalendarEvent()
        {
            this.setDefaults();
        }

        public MCalendarEvent(DataRow detailRow)
        {
            this.setDefaults();
            this.Initialize(detailRow);
        }
    #endregion

    protected new void Initialize(DataRow dataRow)
    {
        base.Initialize(dataRow);
        this.Id = base.GetInt(dataRow, "CalendarEventSeqId");
        this.AllDay = base.GetBool(dataRow, "AllDay");
        this.CalendarSeqId = base.GetInt(dataRow, "CalendarSeqId");
        this.Color = base.GetString(dataRow, "Color");
        this.Description = base.GetString(dataRow, "Description");
        this.End = base.GetDateTime(dataRow, "End", DateTime.Now).ToString();
        this.Link = base.GetString(dataRow, "Link");
        this.Location = base.GetString(dataRow, "Location");
        this.Owner = base.GetString(dataRow, "Owner");
        this.Start = base.GetDateTime(dataRow, "Start", DateTime.Now).ToString();
        this.Title = base.GetString(dataRow, "Title");
    }

    protected override void setDefaults()
    {
        this.Id = -1;
    }
}