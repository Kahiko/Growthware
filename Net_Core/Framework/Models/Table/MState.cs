using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Properties that represent the table [ZGWOptional].[States]
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MState : AbstractBaseModel
{

#region Public Properties
    public string Description { get; set; }

    [DBPrimaryKey]
    public string State { get; set; }

    [DBColumnName("StatusSeqId")]
    public int StatusId { get; set; }
#endregion

#region Constructors
    public MState() 
    { 
        this.SetupClass();
    }

    public MState(DataRow detailRow)
    {
        this.SetupClass();
        this.Description = this.GetString(detailRow, "Description");
        this.State = this.GetString(detailRow, "State");
        this.StatusId = this.GetInt(detailRow, "StatusSeqId");
        base.Initialize(detailRow);
    }
#endregion

    private void SetupClass()
    {
        base.NameColumnName = "State";
        // base.IdColumnName = "State";
    }

}
