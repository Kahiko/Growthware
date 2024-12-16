using System;
using GrowthWare.Framework.Interfaces;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Class MTestDatabaseTable is used to transport data to the Data Access Layer for storage in the data store.
/// </summary>
public class MTestDatabaseTable : ADatabaseTable
{
    protected override void SetupClass()
    {
        base.m_ForeignKeyName = "NOT_USED";
        base.m_IsForeignKeyNumeric = false;
        m_TableName = "[ZGWOptional].[Feedbacks]";
        // m_UseBrackets = false;
    }

    public MTestDatabaseTable() 
    {
        SetupClass();
    }

    [PrimaryKey]
    public int FeedbackId { get; set; }
    public int AssigneeId { get; set; }
    public DateTime DateClosed { get; set; }
    public DateTime DateOpened { get; set; }
    public readonly DateTime DefaultDateTime;
    public string Details { get; set; }
    [ColumnName("Action")]
    public string FoundInVersion { get; set; }
    public int FunctionSeqId { get; set; }
    public bool IsDeleted { get; set; }
    public string Notes { get; set; }
    public string Severity { get; set; }
    public string Status { get; set; }
    public int SubmittedById { get; set; }
    public string TargetVersion { get; set; }
    public string Type { get; set; }
    public int UpdatedById { get; set; }
    [ColumnName("VerifiedBy_Id")]
    public int VerifiedById { get; set; }
}