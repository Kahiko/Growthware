using System;
using System.ComponentModel;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Properties for an account.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MNameValuePair : AAddedUpdated
{

#region Member Fields
    private int m_NVPSeqId = -1;
    private string m_SchemaName = "dbo";
    private string m_StaticName = "NEW";
    private string m_Display = string.Empty;
    private string m_Description = string.Empty;
    private int m_Status = 1;
#endregion

#region Public Properties
     
    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string Description
    {
        get { return m_Description; }
        set { if (!String.IsNullOrEmpty(value)) m_Description = value.Trim(); }
    }

    /// <summary>
    /// Gets or sets the display.
    /// </summary>
    /// <value>The display.</value>
    public string Display
    {
        get { return m_Display; }
        set { if (!String.IsNullOrEmpty(value)) m_Display = value.Trim(); }
    }

    [DBIgnoreProperty]
    public override string ForeignKeyName => "NOT USED";

    [DBIgnoreProperty]
    public override bool IsForeignKeyNumeric => true;

    [DBPrimaryKey]
    [DBColumnName("NVPSeqId")]
    public int Id { get{ return m_NVPSeqId; } set{ m_NVPSeqId = value; } }

    /// <summary>
    /// Gets or sets the name of the schema.
    /// </summary>
    /// <value>The name of the schema.</value>
    /// <remarks>Default value is dbo</remarks>
    [DBColumnName("Schema_Name")]
    public string SchemaName
    {
        get { return m_SchemaName; }
        set { if (!String.IsNullOrEmpty(value)) m_SchemaName = value.Trim(); }
    }

    /// <summary>
    /// Gets or sets the name of the static.
    /// </summary>
    /// <value>The name of the static.</value>
    /// <remarks>Default value is new</remarks>
    [DBColumnName("Static_Name")]
    public string StaticName
    {
        get { return m_StaticName; }
        set { if (!String.IsNullOrEmpty(value)) m_StaticName = value.Trim(); }
    }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>The status.</value>
    [DBColumnName("StatusSeqId")]
    public int Status
    {
        get { return m_Status; }
        set { m_Status = value; }
    }

    [DBIgnoreProperty]
    public override string TableName => "[ZGWSystem].[Name_Value_Pairs]";

#endregion

#region Constructors
    /// <summary>
    /// Provides a new account profile with the default vaules
    /// </summary>
    /// <remarks></remarks>
    public MNameValuePair()
    {
        this.setDefaults();
    }

    /// <summary>
    /// Will populate values based on the contents of the data row.
    /// </summary>
    /// <param name="dataRow">DataRow containing base values</param>
    /// <remarks>
    /// Class should be inherited to extend to your project specific properties
    /// </remarks>
    public MNameValuePair(DataRow dataRow)
    {
        this.setDefaults();
        this.Initialize(dataRow);
    }
#endregion

    /// <summary>
    /// Initializes the specified DataRow.
    /// </summary>
    /// <param name="dataRow">The DataRow.</param>
    protected new void Initialize(DataRow dataRow)
    {
        base.Initialize(dataRow);
        this.m_NVPSeqId = base.GetInt(dataRow, "NVP_SEQ_ID");
        this.m_SchemaName = base.GetString(dataRow, "SCHEMA_NAME");
        this.m_StaticName = base.GetString(dataRow, "STATIC_NAME");
        this.m_Display = base.GetString(dataRow, "DISPLAY");
        this.m_Description = base.GetString(dataRow, "DESCRIPTION");
        this.m_Status = base.GetInt(dataRow, "STATUS_SEQ_ID");
    }

    /// <summary>
    /// Sets up the class default properties
    /// </summary>
    protected override void setDefaults()
    {
        this.Id = -1;
    }
}
