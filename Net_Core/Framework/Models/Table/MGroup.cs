using System;
using System.Data;
using GrowthWare.Framework.Models.Base;
using GrowthWare.Framework.Models.UI;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Model object representing the GroupProfile
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MGroupProfile : AAddedUpdated
{

#region Member Fields
    private string m_Description = string.Empty;
    private int m_GroupSeqId = -1;
    private int m_SecurityEntityID = 1;
#endregion

#region Public Properties
    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string Description
    {
        get { return m_Description; }
        set { if (value != null) m_Description = value.Trim(); }
    }

    [DBIgnoreProperty]
    public override string ForeignKeyName => "NOT USED";

    [DBIgnoreProperty]
    public int GroupSeqId {get{ return m_GroupSeqId; } set{ m_GroupSeqId = value; } }

    [DBPrimaryKey]
    [DBColumnName("GroupSeqId")]
    public int Id { get{ return m_GroupSeqId; } set{ m_GroupSeqId = value; } }

    [DBIgnoreProperty]
    public override bool IsForeignKeyNumeric => false;

    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the security entity ID.
    /// </summary>
    /// <value>The security entity ID.</value>
    [DBIgnoreProperty]
    public int SecurityEntityID
    {
        get { return m_SecurityEntityID; }
        set { m_SecurityEntityID = value; }
    }

    [DBIgnoreProperty]
    public override string TableName => "[ZGWSecurity].[Groups]";
#endregion

#region Constructors
    /// <summary>
    /// Will return a message profile with the default value's
    /// </summary>
    /// <remarks></remarks>
    public MGroupProfile()
    {
        this.setDefaults();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MGroupProfile" /> class.
    /// </summary>
    /// <param name="profileDataRow">The profile data row.</param>
    public MGroupProfile(DataRow profileDataRow)
    {
        this.setDefaults();
        this.Initialize(profileDataRow);
    }

    public MGroupProfile(UIGroupProfile uIGroupProfile)
    {
        setDefaults();
        Name = uIGroupProfile.Name;
        Description = uIGroupProfile.Description;
        Id = uIGroupProfile.Id;
    }
#endregion

    /// <summary>
    /// Initializes the specified DataRow.
    /// </summary>
    /// <param name="dataRow">The DataRow.</param>
    protected new void Initialize(DataRow dataRow)
    {
        base.Initialize(dataRow);
        this.m_Description = base.GetString(dataRow, "DESCRIPTION");
        this.m_GroupSeqId = base.GetInt(dataRow, "GROUP_SEQ_ID");
        this.Name = base.GetString(dataRow, "Name");
    }


    /// <summary>
    /// Sets up the common and specific class properties
    /// </summary>
    protected override void setDefaults()
    {
        // base.NameColumnName = "NAME";
        // base.IdColumnName = "GROUP_SEQ_ID";

        // this.Id = -1;
        // base.m_ForeignKeyName = "NOT_USED";
        // m_TableName = "[ZGWSecurity].[Groups]";
    }
}
