using System;
using System.Data;
using GrowthWare.Framework.Interfaces;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Class MFeedback is used to transport data to the Data Access Layer for storage in the data store.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MFeedback : AbstractDatabaseFunctions, IDatabaseFunctions
{

    public MFeedback()
    {
        setKeyAndTable();
        this.DefaultDateTime = new(1900, 1, 1);
    }

    public MFeedback(UIFeedback feedback) 
    { 
        setKeyAndTable();
        this.DefaultDateTime = new(1900, 1, 1);
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
        setKeyAndTable();
        this.Initialize(detailRow);
    }

    #region "Protected Methods"
        private void Initialize(DataRow dataRow)
        {
            DateTime mNow = DateTime.Now;
            this.FeedbackId = base.GetInt(dataRow, "FeedbackId");
            this.AssigneeId = base.GetInt(dataRow, "AssigneeId");
            this.DateClosed = base.GetDateTime(dataRow, "Date_Closed", this.DefaultDateTime);
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
        }
    #endregion

    #region "Private Methods"
        private void setKeyAndTable()
        {
            this.m_ForeignKeyIsNumber = true;
            this.m_ForeignKeyName = "";
            this.m_PrimaryKeyName = "[FeedbackId]";
            this.m_TableName = "[ZGWOptional].[Feedbacks]";
        }
    #endregion

    #region Public Properties
        public int FeedbackId { get; set; }
        public int AssigneeId { get; set; }
        public DateTime DateClosed { get; set; }
        public DateTime DateOpened { get; set; }
		public readonly DateTime DefaultDateTime;
        public string Details { get; set; }
        public string FoundInVersion { get; set; }
        public int FunctionSeqId { get; set; }
        public string Notes { get; set; }
        public string Severity { get; set; }
        public string Status { get; set; }
        public int SubmittedById { get; set; }
        public string TargetVersion { get; set; }
        public string Type { get; set; }
        public int UpdatedById { get; set; }
        public int VerifiedById { get; set; }
    #endregion    
}