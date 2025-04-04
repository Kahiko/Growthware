using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Properties for an Feedback.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class UIFeedback
{

#region Public Properties
    public string Action { get; set; }
    
    public string Assignee { get; set; }
    
    public int AssigneeId { get; set; }
    
    [DBColumnName("Date_Closed")]
    public DateTime DateClosed { get; set; }
    
    [DBColumnName("Date_Opened")]
    public DateTime DateOpened { get; set; }
    
    public string Details { get; set; }
    
    public int FeedbackId { get; set; }
    
    [DBColumnName("Found_In_Version")]
    public string FoundInVersion { get; set; }

    /// <summary>
    /// The "FunctionSeqId" should be looked up using the "Action" value.
    /// </summary>
    public int FunctionSeqId { get; set; }

    public string Notes { get; set; }

    public string Severity { get; set; }

    public string Status { get; set; }

    /// <summary>
    /// The account who submitted the feedback.
    /// </summary>
    /// <remarks>Used by the UI</remarks>
    [DBIgnoreProperty]
    public string SubmittedBy { get; set; }

    public int SubmittedById { get; set; }

    public string TargetVersion { get; set; }

    public string Type { get; set; }

    /// <summary>
    /// The account who updated the feedback.
    /// </summary>
    /// <remarks>Used by the UI</remarks>
    [DBIgnoreProperty]
    public string UpdatedBy { get; set; }

    public int UpdatedById { get; set; }

    /// <summary>
    /// The account who verified the feedback was resolved.
    /// </summary>
    /// <remarks>Used by the UI</remarks>
    [DBIgnoreProperty]
    public string VerifiedBy { get; set; }

    public int VerifiedById { get; set; }
#endregion

#region Constructors
    public UIFeedback()
    {
        // do nothing
    }

    public UIFeedback(DataRow dataRow)
    {
        this.Initialize(dataRow);
    }
#endregion

    /// <summary>
    /// Populates direct properties as well as passing the DataRow to the abstract class
    /// for the population of the base properties.
    /// </summary>
    /// <param name="dataRow">DataRow</param>
    protected void Initialize(DataRow dataRow)
    {
        this.FeedbackId = DataRowHelper.GetInt(dataRow, "FeedbackId");
        this.Action = DataRowHelper.GetString(dataRow, "Action");
        this.Assignee = DataRowHelper.GetString(dataRow, "Assignee");
        this.AssigneeId = DataRowHelper.GetInt(dataRow, "AssigneeId");
        this.DateClosed = DataRowHelper.GetDateTime(dataRow, "Date_Closed", DateTime.Now);
        this.DateOpened = DataRowHelper.GetDateTime(dataRow, "Date_Opened", DateTime.Now);
        this.Details = DataRowHelper.GetString(dataRow, "Details");
        this.FoundInVersion = DataRowHelper.GetString(dataRow, "Found_In_Version");
        this.FunctionSeqId = DataRowHelper.GetInt(dataRow, "FunctionSeqId");
        this.Notes = DataRowHelper.GetString(dataRow, "Notes");
        this.Severity = DataRowHelper.GetString(dataRow, "Severity");
        this.Status = DataRowHelper.GetString(dataRow, "Status");
        this.SubmittedBy = DataRowHelper.GetString(dataRow, "SubmittedBy");
        this.SubmittedById = DataRowHelper.GetInt(dataRow, "SubmittedById");
        this.TargetVersion = DataRowHelper.GetString(dataRow, "TargetVersion");
        this.Type = DataRowHelper.GetString(dataRow, "Type");
        this.UpdatedBy = DataRowHelper.GetString(dataRow, "UpdatedBy");
        this.UpdatedById = DataRowHelper.GetInt(dataRow, "UpdatedById");
        this.VerifiedBy = DataRowHelper.GetString(dataRow, "VerifiedBy");
        this.VerifiedById = DataRowHelper.GetInt(dataRow, "VerifiedById");
    }
}
