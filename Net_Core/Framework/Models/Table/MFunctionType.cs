using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Class MFunctionTypeProfile
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MFunctionTypeProfile : AAddedUpdated
{

#region Member Fields
    private int m_FunctionTypeSeqId = -1;
    private string m_Description = string.Empty;
    private string m_Template = string.Empty;
    private bool m_IsContent;
#endregion

#region Public Properties
    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string Description
    {
        get { return m_Description; }
        set { if (!string.IsNullOrEmpty(value)) m_Description = value.Trim(); }
    }

    [DBIgnoreProperty]
    public override string ForeignKeyName => "NOT_USED";

    /// <summary>
    /// Gets or sets the function_ type_ seq_ ID.
    /// </summary>
    /// <value>The FunctionTypeSeqId.</value>
    [DBPrimaryKey]
    [DBColumnName("FunctionTypeSeqId")]
    public int FunctionTypeSeqId
    {
        get { return m_FunctionTypeSeqId; }
        set { m_FunctionTypeSeqId = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [I s_ CONTENT].
    /// </summary>
    /// <value><c>true</c> if [I s_ CONTENT]; otherwise, <c>false</c>.</value>
    [DBColumnName("Is_Content")]
    public bool IsContent
    {
        get { return m_IsContent; }
        set { m_IsContent = value; }
    }

    [DBIgnoreProperty]
    public int Id
    {
        get
        {
            return m_FunctionTypeSeqId;
        }
        set
        {
            m_FunctionTypeSeqId = value;
        }
    }

    [DBIgnoreProperty]
    public override bool IsForeignKeyNumeric => false;

    public string Name {get; set;}

    [DBIgnoreProperty]
    public override string TableName => "[ZGWSecurity].[Function_Types]";

    /// <summary>
    /// Gets or sets the TEMPLATE.
    /// </summary>
    /// <value>The TEMPLATE.</value>
    public string Template
    {
        get { return m_Template; }
        set { if (!string.IsNullOrEmpty(value)) m_Template = value.Trim(); }
    }
#endregion

#region Constructors
    /// <summary>
    /// Will return a Function profile with the default values
    /// </summary>
    /// <remarks></remarks>
    public MFunctionTypeProfile()
    {
        this.setDefaults();
    }

    /// <summary>
    /// Will return a fully populated Function type profile.
    /// </summary>
    /// <param name="dataRow">A data row containing the Function type information</param>
    /// <remarks></remarks>
    public MFunctionTypeProfile(DataRow dataRow)
    {
        this.setDefaults();
        Initialize(dataRow);
    }
#endregion

    /// <summary>
    /// Initializes the specified datarow.
    /// </summary>
    /// <param name="detailRow">The datarow.</param>
    protected new void Initialize(DataRow detailRow)
    {
        if (detailRow != null)
        {
            base.Initialize(detailRow);
            m_FunctionTypeSeqId = base.GetInt(detailRow, "FUNCTION_TYPE_SEQ_ID");
            this.Name = base.GetString(detailRow, "Name");
            m_Description = base.GetString(detailRow, "Description");
            m_Template = base.GetString(detailRow, "Template");
            m_IsContent = base.GetBool(detailRow, "Is_Content");
        }
    }

    protected override void setDefaults()
    {
        this.m_FunctionTypeSeqId = -1;
    }
}
