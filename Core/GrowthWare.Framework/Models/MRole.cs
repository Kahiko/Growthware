using System;
using System.Data;

namespace GrowthWare.Framework.Models
{
    /// <summary>
    /// Model object representing a Role.
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MRole : MBaseModel
    {
#region "Member Properties"
        private string m_Description = string.Empty;
        private bool m_IsSystem = false;
        private bool m_IsSystemOnly = false;
        private int m_SecurityEntityId = 1;
#endregion


#region "Protected Methods"
        /// <summary>
        /// Initializes with the specified DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow.</param>
        protected new void Initialize(DataRow dataRow)
        {
            base.NameColumnName = "NAME";
            base.IdColumnName = "ROLE_SEQ_ID";
            base.Initialize(dataRow);
            m_Description = base.GetString(dataRow, "DESCRIPTION");
            m_IsSystem = base.GetBool(dataRow, "IS_SYSTEM");
            m_IsSystemOnly = base.GetBool(dataRow, "IS_SYSTEM_ONLY");
        }
#endregion

#region "Public Methods"

        /// <summary>
        /// Initializes a new instance of the <see cref="MRole" /> class.
        /// </summary>
        public MRole()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MRole" /> class.
        /// </summary>
        /// <param name="dataRow">The dataRow.</param>
        public MRole(DataRow dataRow)
        {
            this.Initialize(dataRow);
        }
#endregion

#region "Public Properties"
        /// <summary>
        /// Gets or sets the security entity ID.
        /// </summary>
        /// <value>The security entity ID.</value>
        public int SecurityEntityId
        {
            get { return m_SecurityEntityId; }
            set { m_SecurityEntityId = value; }
        }

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
        /// Gets or sets the is system.
        /// </summary>
        /// <value>The is system.</value>
        public bool IsSystem
        {
            get { return m_IsSystem; }
            set { m_IsSystem = value; }
        }

        /// <summary>
        /// Gets or sets the is system only.
        /// </summary>
        /// <value>The is system only.</value>
        public bool IsSystemOnly
        {
            get { return m_IsSystemOnly; }
            set { m_IsSystemOnly = value; }
        }
#endregion
    }
}
