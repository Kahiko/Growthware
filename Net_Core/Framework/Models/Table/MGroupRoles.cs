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
     * Though this class inherits from the AAddedUpdated class, it does not make use of any of the 
     * functions provided by that class only the AddedBy, AddedDate, UpdatedBy, and 
     * UpdatedDate properties.
     */

    #region Member Fields

        private int m_SecurityEntityID = -1;

        private int m_GroupSeqId = -1;

        private string m_Roles;
    #endregion

    #region Public Properties
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

        /// <summary>
        /// Gets or sets the Security Entity Id.
        /// </summary>
        /// <value>The Security Entity Id.</value>
        public int SecurityEntityID
        {
            get { return m_SecurityEntityID; }
            set { m_SecurityEntityID = value; }
        }

    #endregion

    #region Constructors
        public MGroupRoles()
        {
            this.SetupClass();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MGroupRoles"/> class.
        /// </summary>
        /// <param name="roles">comma separated roles</param>
        /// <param name="securityEntityID"></param>
        public MGroupRoles(string roles, int securityEntityID)
        {
            this.SetupClass();
            Roles = roles;
            SecurityEntityID = securityEntityID;
        }

        public MGroupRoles(int groupSeqId, int securityEntityID)
        {
            this.SetupClass();
            GroupSeqId = groupSeqId;
            SecurityEntityID = securityEntityID;
        }
    #endregion

    protected override void SetupClass()
    {
        GroupSeqId = -1;
        base.m_ForeignKeyName = "NOT_USED";
        m_TableName = "[ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]";
    }
}
