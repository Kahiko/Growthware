using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Properties for an Feedback.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class UIFeedback : AbstractDatabaseFunctions 
{
    public UIFeedback() { }

    public UIFeedback(DataRow dataRow) 
    { 
        this.Initialize(dataRow);
    }

    #region "Protected Methods"
        /// <summary>
        /// Populates direct properties as well as passing the DataRow to the abstract class
        /// for the population of the base properties.
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        protected void Initialize(DataRow dataRow)
        {
            this.FeedbackId = base.GetInt(dataRow, "FeedbackId");
            this.Action = base.GetString(dataRow, "Action");
            this.Assignee = base.GetString(dataRow, "Assignee");
            this.AssigneeId = base.GetInt(dataRow, "AssigneeId");
            this.DateClosed = base.GetDateTime(dataRow, "Date_Closed", DateTime.Now);
            this.DateOpened = base.GetDateTime(dataRow, "Date_Opened", DateTime.Now);
            this.Details = base.GetString(dataRow, "Details");
            this.FoundInVersion = base.GetString(dataRow, "Found_In_Version");
            this.FunctionSeqId = base.GetInt(dataRow, "FunctionSeqId");
            this.Notes = base.GetString(dataRow, "Notes");
            this.Severity = base.GetString(dataRow, "Severity");
            this.Status = base.GetString(dataRow, "Status");
            this.SubmittedBy = base.GetString(dataRow, "SubmittedBy");
            this.SubmittedById = base.GetInt(dataRow, "SubmittedById");
            this.TargetVersion = base.GetString(dataRow, "TargetVersion");
            this.Type = base.GetString(dataRow, "Type");
            this.UpdatedBy = base.GetString(dataRow, "UpdatedBy");
            this.UpdatedById = base.GetInt(dataRow, "UpdatedById");
            this.VerifiedBy = base.GetString(dataRow, "VerifiedBy");
            this.VerifiedById = base.GetInt(dataRow, "VerifiedById");
        }    
    #endregion
    #region Public Properties
        public int FeedbackId { get; set; }
        public string Action { get; set; }
        public string Assignee { get; set; }
        public int AssigneeId { get; set; }
        public DateTime DateClosed { get; set; }
        public DateTime DateOpened { get; set; }
        public string Details { get; set; }
        public string FoundInVersion { get; set; }
        /// <summary>
        /// The "FunctionSeqId" should be looked up using the "Action" value.
        /// </summary>
        public int FunctionSeqId { get; set; }
        public string Notes { get; set; }
        public string Severity { get; set; }
        public string Status { get; set; }
        public string SubmittedBy { get; set; }
        public int SubmittedById { get; set; }
        public string TargetVersion { get; set; }
        public string Type { get; set; }
        public string UpdatedBy { get; set; }
        public int UpdatedById { get; set; }
        public string VerifiedBy { get; set; }
        public int VerifiedById { get; set; }
    #endregion
}
