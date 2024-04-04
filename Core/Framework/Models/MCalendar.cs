using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Properties for a calendar event.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MCalendar : AbstractBaseModel
{

	public int CalendarSeqId {get; set;}
	public int SecurityEntitySeqId {get; set;}
	public int FunctionSeqId {get; set;}
	public string Comment {get; set;}
	public bool Active {get; set;}

    public MCalendar()
    {
        this.IdColumnName = "CalendarSeqId";
        this.NameColumnName = string.Empty;
    }

    public MCalendar(DataRow detailRow)
    {
        this.IdColumnName = "CalendarSeqId";
        this.NameColumnName = string.Empty;
        this.Initialize(detailRow);
        this.Active = base.GetBool(detailRow, "Active");
        this.Comment = base.GetString(detailRow, "Comment");
        this.FunctionSeqId = base.GetInt(detailRow, "FunctionSeqId");
        this.SecurityEntitySeqId = base.GetInt(detailRow, "SecurityEntitySeqId");
    }
}