using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Properties for a calendar event.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MCalendar : AAddedUpdated
{

    [DBColumnName("CalendarSeqId")]
    public int Id {get; set;}

	public int SecurityEntitySeqId {get; set;}

	public int FunctionSeqId {get; set;}

	public string Comment {get; set;}

	public bool Active {get; set;}

    [DBIgnoreProperty]
    public override string ForeignKeyName => "FunctionSeqId";

    [DBIgnoreProperty]
    public override bool IsForeignKeyNumeric => true;

    [DBIgnoreProperty]
    public override string TableName => "[ZGWOptional].[Calendars]";

    public MCalendar()
    {
    }

    public MCalendar(DataRow detailRow)
    {
        this.Initialize(detailRow);
        this.Active = base.GetBool(detailRow, "Active");
        this.Comment = base.GetString(detailRow, "Comment");
        this.FunctionSeqId = base.GetInt(detailRow, "FunctionSeqId");
        this.SecurityEntitySeqId = base.GetInt(detailRow, "SecurityEntitySeqId");
    }

    protected override void setDefaults()
    {
        throw new NotImplementedException();
    }
}
