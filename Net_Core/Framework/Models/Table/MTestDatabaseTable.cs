using System.Data;
using GrowthWare.Framework.Interfaces;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

public class MTestDatabaseTable : AAddedUpdated
{

    #region Member Fields
    #endregion

#region Properties

    public string Description { get; set; }

    [DBIgnoreProperty]
    public override string TableName => "[dbo].[MyTable]";

    [DBIgnoreProperty]
    public override string ForeignKeyName => "not used";

    [DBPrimaryKey]
    [DBColumnName("SecurityEntitySeqId")]
    public int Id { get; set; }

    [DBIgnoreProperty]
    public override bool IsForeignKeyNumeric => false;
#endregion

    #region Constructors
    public MTestDatabaseTable()
    {
        // populate default values ie this.Id = -1
    }

    public MTestDatabaseTable(DataRow dataRow)
    {
        Initialize(dataRow);
    }
    #endregion

    protected override void Initialize(DataRow dataRow)
    {
        base.Initialize(dataRow);
        // populate properties
    }
}