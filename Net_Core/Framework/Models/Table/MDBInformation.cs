using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Base properties an DB Information Profile
/// </summary>
/// <remarks>
/// Corresponds to table ZGWSystem.Database_Information and 
/// Store procedures: 
/// ZGWSystem.Set_DataBase_Information, ZGWSystem.Get_Database_Information
/// </remarks>
public class MDBInformation : AAddedUpdated
{

#region Member Fields
    private int m_InformationSeqId = 1;
    private string m_Version = string.Empty;
    private int m_EnableInheritance = 1;
#endregion

#region Public Properties
    /// <summary>
    /// The database information Sequence Identifier
    /// </summary>
    [DBPrimaryKey]
    [DBColumnName("Database_InformationSeqId")]
    public int DatabaseInformationSeqId
    {
        get { return m_InformationSeqId; }
        set { m_InformationSeqId = value; }
    }

    [DBIgnoreProperty]
    public override string TableName => "[ZGWSystem].[Database_Information]";

    /// <summary>
    /// The version of the database
    /// </summary>
    public string Version
    {
        get { return m_Version.Trim(); }
        set
        {
            if (value != null) m_Version = value.Trim();
        }
    }

    /// <summary>
    /// Determins if the database should use Inheritance when 
    /// calculating roles and or groups.
    /// </summary>
    [DBColumnName("Enable_Inheritance")]
    public int EnableInheritance
    {
        get { return m_EnableInheritance; }
        set { m_EnableInheritance = value; }
    }

    [DBIgnoreProperty]
    public override string ForeignKeyName => throw new System.NotImplementedException();

    [DBPrimaryKey]
    [DBColumnName("Database_InformationSeqId")]
    public int Id { get; set; }

    [DBIgnoreProperty]
    public override bool IsForeignKeyNumeric => throw new System.NotImplementedException();

#endregion

#region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="MDBInformation"/> class.
    /// </summary>
    public MDBInformation()
    {
        this.setDefaults();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MDBInformation"/> class.
    /// </summary>
    /// <param name="dataRow">The dataRow.</param>
    public MDBInformation(DataRow dataRow)
    {
        this.setDefaults();
        this.Initialize(dataRow);
    }
#endregion

    /// <summary>
    /// Populates direct properties as well as passing the DataRow to the abstract class
    /// for the population of the base properties.
    /// </summary>
    /// <param name="dataRow">DataRow</param>
    protected new void Initialize(DataRow dataRow)
    {
        base.Initialize(dataRow);
        this.Id = base.GetInt(dataRow, "Information_SEQ_ID");
        m_InformationSeqId = base.GetInt(dataRow, "Information_SEQ_ID");
        m_Version = base.GetString(dataRow, "VERSION");
        m_EnableInheritance = base.GetInt(dataRow, "ENABLE_INHERITANCE");
    }

    protected override void setDefaults()
    {
        this.Id = -1;
    }

}
