using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Class MFeedback is used to transport data to the Data Access Layer for storage in the data store.
/// </summary>
/// <remarks>
/// The [ZGWOptional].[Feedbacks] is an "in-line" history table, use only with the stored procedures.
/// </remarks>
[Serializable(), CLSCompliant(true)]
public class MFeedback : ADatabaseTable
{

#region Member Fields
    private DateTime m_DefaultSystemDateTime = new(1753, 1, 1, 0, 0, 0);
#endregion

#region Public Properties

    [DBPrimaryKey]
    public int FeedbackId { get; set; }

    public int AssigneeId { get; set; }

    [DBColumnName("Date_Closed")]

    public DateTime DateClosed { get; set; }
    [DBColumnName("Date_Opened")]

    public DateTime DateOpened { get; set; }

    public string Details { get; set; }
    [DBColumnName("Found_In_Version")]

    public string FoundInVersion { get; set; }

    [DBIgnoreProperty]
    public override string ForeignKeyName => throw new NotImplementedException();

    public int FunctionSeqId { get; set; }

    [DBIgnoreProperty]
    public override bool IsForeignKeyNumeric => throw new NotImplementedException();

    public string Notes { get; set; }

    public string Severity { get; set; }

    public string Status { get; set; }

    public int SubmittedById { get; set; }

    [DBIgnoreProperty]
    public override string TableName => "[ZGWOptional].[Feedbacks]";

    public string TargetVersion { get; set; }

    public string Type { get; set; }

    public int UpdatedById { get; set; }

    public int VerifiedById { get; set; }

    [DBIgnoreProperty]
    [DBColumnName("Start_Date")]
    public DateTime StartDate { get; set; }

    [DBIgnoreProperty]
    [DBColumnName("End_Date")]
    public DateTime EndDate { get; set; }
#endregion    

#region Constructors
    public MFeedback()
    {
        setDefaults();
    }

    public MFeedback(UIFeedback feedback) 
    { 
        setDefaults();
        DateTime mNow = DateTime.Now;
        this.FeedbackId = feedback.FeedbackId;
        this.AssigneeId = feedback.AssigneeId;
        this.DateClosed = feedback.DateClosed;
        this.DateOpened = feedback.DateOpened;
        this.Details = feedback.Details;
        this.FoundInVersion = feedback.FoundInVersion;
        this.Notes = feedback.Notes;
        this.Severity = feedback.Severity;
        this.Status = feedback.Status;
        this.SubmittedById = feedback.SubmittedById;
        this.TargetVersion = feedback.TargetVersion;
        this.Type = feedback.Type;
        this.UpdatedById = feedback.UpdatedById;
        this.VerifiedById = feedback.VerifiedById;
    }

    public MFeedback(DataRow detailRow)
    {
        setDefaults();
        this.Initialize(detailRow);
    }
#endregion

    protected void Initialize(DataRow dataRow)
    {
        DateTime mNow = DateTime.Now;
        this.FeedbackId = base.GetInt(dataRow, "FeedbackId");
        this.AssigneeId = base.GetInt(dataRow, "AssigneeId");
        this.DateClosed = base.GetDateTime(dataRow, "Date_Closed", m_DefaultSystemDateTime);
        this.DateOpened = base.GetDateTime(dataRow, "Date_Opened", mNow);
        this.Details = base.GetString(dataRow, "Details");
        this.FoundInVersion = base.GetString(dataRow, "Found_In_Version");
        this.FunctionSeqId = base.GetInt(dataRow, "FunctionSeqId");
        this.Notes = base.GetString(dataRow, "Notes");
        this.Severity = base.GetString(dataRow, "Severity");
        this.Status = base.GetString(dataRow, "Status");
        this.SubmittedById = base.GetInt(dataRow, "SubmittedById");
        this.TargetVersion = base.GetString(dataRow, "TargetVersion");
        this.Type = base.GetString(dataRow, "Type");
        this.UpdatedById = base.GetInt(dataRow, "UpdatedById");
        this.VerifiedById = base.GetInt(dataRow, "VerifiedById");
        this.StartDate = base.GetDateTime(dataRow, "Start_Date", base.DefaultSystemDateTime);
        this.EndDate = base.GetDateTime(dataRow, "End_Date", base.DefaultSystemDateTime);
    }

    protected override void setDefaults()
    {
        this.FunctionSeqId = -1;
    }
}