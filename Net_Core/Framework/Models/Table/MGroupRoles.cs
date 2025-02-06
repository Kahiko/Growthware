using System;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Model object representing GroupRoles
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MGroupRoles : AAddedUpdated
{
    /*
     * Though this class inherits from the AbstractBaseModel class, it does not make use of any of the 
     * functions provided by that class only the AddedBy, AddedDate, UpdatedBy, and 
     * UpdatedDate properties.
     */

#region Member Fields
    private int m_AddedUpdatedBy;
    private int m_SecurityEntityID = -1;
    private int m_GroupSeqId = -1;

    private string m_Roles;
#endregion

#region Public Properties

    [DBIgnoreProperty]
    public override string ForeignKeyName => "NOT USED";

    /// <summary>
    /// Gets or Added Updated By id.
    /// </summary>
    /// <value>The Added Updated By id.</value>
    public int AddedUpdatedBy
    {
        get { return m_AddedUpdatedBy; }
        set { m_AddedUpdatedBy = value; }
    }

    [DBIgnoreProperty]
    public override bool IsForeignKeyNumeric => true;

    /// <summary>
    /// Gets or sets the Security Entity Id.
    /// </summary>
    /// <value>The Security Entity Id.</value>
    public int SecurityEntityID
    {
        get { return m_SecurityEntityID; }
        set { m_SecurityEntityID = value; }
    }

    [DBIgnoreProperty]
    public override string TableName => "[ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]";

    /// <summary>
    /// Gets or sets the Group Sequence Id.
    /// </summary>
    /// <value>The Group Sequence Id.</value>
    public int GroupSeqId
    {
        get { return m_GroupSeqId; }
        set { m_GroupSeqId = value; }
    }

    /// <summary>
    /// Gets or sets the roles.
    /// </summary>
    /// <value>The roles.</value>
    public string Roles
    {
        get { return m_Roles; }
        set { if (!String.IsNullOrEmpty(value)) m_Roles = value.Trim(); }
    }
#endregion

#region Constructors
    public MGroupRoles()
    {
        this.setDefaults();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MGroupRoles"/> class.
    /// </summary>
    /// <param name="roles">comma separated roles</param>
    /// <param name="securityEntityID"></param>
    public MGroupRoles(string roles, int securityEntityID)
    {
        this.setDefaults();
        Roles = roles;
        SecurityEntityID = securityEntityID;
    }

    public MGroupRoles(int groupSeqId, int securityEntityID)
    {
        this.setDefaults();
        GroupSeqId = groupSeqId;
        SecurityEntityID = securityEntityID;
    }
#endregion

    protected override void setDefaults()
    {
        GroupSeqId = -1;
        // base.m_ForeignKeyName = "NOT_USED";
        // m_TableName = "[ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]";
    }
}
