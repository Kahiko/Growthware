using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Interfaces;
using GrowthWare.Framework.Models.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Properties for an Feedback.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class UIFeedbackResult : AbstractDatabaseFunctions 
{
    #region "Protected Methods"
        /// <summary>
        /// Populates direct properties as well as passing the DataRow to the abstract class
        /// for the population of the base properties.
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        protected void Initialize(DataRow dataRow)
        {
            this.FeedbackId = base.GetInt(dataRow, "FeedbackId");
            this.AreaFound = base.GetString(dataRow, "Area_Found");
            this.AssigneeId = base.GetInt(dataRow, "AssigneeId");
            this.Assignee = base.GetString(dataRow, "Assignee");
            this.DateClosed = base.GetDateTime(dataRow, "Date_Closed", DateTime.Now);
            this.DateOpened = base.GetDateTime(dataRow, "Date_Opened", DateTime.Now);
            this.Details = base.GetString(dataRow, "Details");
            this.FoundInVersion = base.GetString(dataRow, "FoundInVersion");
            this.Notes = base.GetString(dataRow, "Notes");
            this.Severity = base.GetString(dataRow, "Severity");
            this.Status = base.GetString(dataRow, "Status");
            this.SubmittedBy = base.GetString(dataRow, "SubmittedBy");
            this.TargetVersion = base.GetString(dataRow, "TargetVersion");
            this.Type = base.GetString(dataRow, "Type");
            this.VerifiedById = base.GetInt(dataRow, "VerifiedById");
            this.VerifiedBy = base.GetString(dataRow, "VerifiedBy");
            this.Start_Date = base.GetDateTime(dataRow, "Start_Date", DateTime.Now);
            this.End_Date = base.GetDateTime(dataRow, "End_Date", DateTime.Now);
        }    
    #endregion
    #region Public Properties
        public int FeedbackId { get; set; }
        public string AreaFound { get; set; }
        public int AssigneeId { get; set; }
        public string Assignee { get; set; }
        public DateTime DateClosed { get; set; }
        public DateTime DateOpened { get; set; }
        public string Details { get; set; }
        public string FoundInVersion { get; set; }
        public string Notes { get; set; }
        public string Severity { get; set; }
        public string Status { get; set; }
        public string SubmittedBy { get; set; }
        public string TargetVersion { get; set; }
        public string Type { get; set; }
        public int VerifiedById { get; set; }
        public string VerifiedBy { get; set; }
		public DateTime Start_Date { get; set; }
		public DateTime End_Date { get; set; }
    #endregion
}
