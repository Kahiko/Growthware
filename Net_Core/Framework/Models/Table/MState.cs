using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Properties that represent the table [ZGWOptional].[States]
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MState : AAddedUpdated
{

#region Public Properties
    public string Description { get; set; }

    [DBIgnoreProperty]
    public override string ForeignKeyName => "NOT USED";
 
    [DBIgnoreProperty]
    public override bool IsForeignKeyNumeric => true;

    [DBPrimaryKey]
    public string State { get; set; }

    [DBColumnName("StatusSeqId")]
    public int StatusId { get; set; }

    [DBIgnoreProperty]
    public override string TableName => "[ZGWOptional].[States]";

#endregion

#region Constructors
    public MState() 
    { 
        this.setDefaults();
    }

    public MState(DataRow detailRow)
    {
        this.setDefaults();
        this.Description = this.GetString(detailRow, "Description");
        this.State = this.GetString(detailRow, "State");
        this.StatusId = this.GetInt(detailRow, "StatusSeqId");
        base.Initialize(detailRow);
    }
#endregion

    protected override void setDefaults()
    {
        this.State = string.Empty;
    }

}
