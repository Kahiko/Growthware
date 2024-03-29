﻿using System;
using System.Data;
using GrowthWare.Framework.Models.Base;
using GrowthWare.Framework.Models.UI;

namespace GrowthWare.Framework.Models
{
    /// <summary>
    /// Model object representing a Role.
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MRole : AbstractBaseModel
    {
#region "Member Properties"
        private string m_Description = string.Empty;
        private bool m_IsSystem = false;
        private bool m_IsSystemOnly = false;
        private int m_SecurityEntityID = 1;
#endregion


#region "Protected Methods"
        /// <summary>
        /// Initializes with the specified DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow.</param>
        protected new void Initialize(DataRow dataRow)
        {
            Initialize();
            base.Initialize(dataRow);
            m_Description = base.GetString(dataRow, "DESCRIPTION");
            m_IsSystem = base.GetBool(dataRow, "IS_SYSTEM");
            m_IsSystemOnly = base.GetBool(dataRow, "IS_SYSTEM_ONLY");
        }

        private void Initialize()
        {
            base.NameColumnName = "NAME";
            base.IdColumnName = "ROLE_SEQ_ID";
        }
#endregion

#region "Public Methods"

        /// <summary>
        /// Initializes a new instance of the <see cref="MRole" /> class.
        /// </summary>
        public MRole()
        {
        }

        public MRole(MRole roleProfile) 
        {
            this.AddedBy = roleProfile.AddedBy;
            this.AddedDate = roleProfile.AddedDate;
            this.Description = roleProfile.Description;
            this.Id = roleProfile.Id;
            this.IsSystem = roleProfile.IsSystem;
            if(!roleProfile.IsSystemOnly)
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
            this.Description = uiRole.Description;
            this.Id = uiRole.Id;
            this.IsSystem = uiRole.IsSystem;
            if(!uiRole.IsSystemOnly)
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
            this.Initialize(dataRow);
        }
#endregion

#region "Public Properties"
        /// <summary>
        /// Gets or sets the security entity ID.
        /// </summary>
        /// <value>The security entity ID.</value>
        public int SecurityEntityID
        {
            get { return m_SecurityEntityID; }
            set { m_SecurityEntityID = value; }
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
