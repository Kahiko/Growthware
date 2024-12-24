using System;
using System.Data;
using GrowthWare.Framework.Models.Base;
using GrowthWare.Framework.Models.UI;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Model object representing a Role.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MRole : AAddedUpdated
{

    #region Member Fields
        private string m_Description = string.Empty;
        private bool m_IsSystem = false;
        private bool m_IsSystemOnly = false;
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
            set { if (!String.IsNullOrEmpty(value)) m_Description = value.Trim(); }
        }

        [DBPrimaryKey]
        [DBColumnName("RoleSeqId")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the is system.
        /// </summary>
        /// <value>The is system.</value>
        [DBColumnName("Is_System")]
        public bool IsSystem
        {
            get { return m_IsSystem; }
            set { m_IsSystem = value; }
        }

        /// <summary>
        /// Gets or sets the is system only.
        /// </summary>
        /// <value>The is system only.</value>
        [DBColumnName("Is_System_Only")]
        public bool IsSystemOnly
        {
            get { return m_IsSystemOnly; }
            set { m_IsSystemOnly = value; }
        }

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
    #endregion

    #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MRole" /> class.
        /// </summary>
        public MRole()
        {
            this.SetupClass();
        }

        public MRole(MRole roleProfile)
        {
            this.SetupClass();
            this.AddedBy = roleProfile.AddedBy;
            this.AddedDate = roleProfile.AddedDate;
            this.Description = roleProfile.Description;
            this.Id = roleProfile.Id;
            this.IsSystem = roleProfile.IsSystem;
            if (!roleProfile.IsSystemOnly)
            {
                this.IsSystemOnly = roleProfile.IsSystemOnly;
            }
            this.Name = roleProfile.Name;
            this.SecurityEntityID = roleProfile.SecurityEntityID;
            this.UpdatedBy = roleProfile.UpdatedBy;
            this.UpdatedDate = roleProfile.UpdatedDate;
        }

        public MRole(UIRole uiRole)
        {
            this.SetupClass();
            this.Description = uiRole.Description;
            this.Id = uiRole.Id;
            this.IsSystem = uiRole.IsSystem;
            if (!uiRole.IsSystemOnly)
            {
                this.m_IsSystemOnly = uiRole.IsSystemOnly;
            }
            this.Name = uiRole.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MRole" /> class.
        /// </summary>
        /// <param name="dataRow">The dataRow.</param>
        public MRole(DataRow dataRow)
        {
            this.SetupClass();
            this.Initialize(dataRow);
        }
    #endregion

    /// <summary>
    /// Initializes with the specified DataRow.
    /// </summary>
    /// <param name="dataRow">The DataRow.</param>
    protected new void Initialize(DataRow dataRow)
    {
        base.Initialize(dataRow);
        this.Id = base.GetInt(dataRow, "ROLE_SEQ_ID");
        this.Name = base.GetString(dataRow, "NAME");
        m_Description = base.GetString(dataRow, "DESCRIPTION");
        m_IsSystem = base.GetBool(dataRow, "IS_SYSTEM");
        m_IsSystemOnly = base.GetBool(dataRow, "IS_SYSTEM_ONLY");
    }

    protected override void SetupClass()
    {
        this.Id = -1;
        base.m_ForeignKeyName = "NOT_USED";
        m_TableName = "[ZGWSecurity].[Roles]";
        // base.NameColumnName = "NAME";
        // base.IdColumnName = "ROLE_SEQ_ID";
    }
}
