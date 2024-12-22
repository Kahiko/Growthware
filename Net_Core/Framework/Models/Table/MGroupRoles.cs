using System;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Model object representing GroupRoles
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MGroupRoles : AbstractBaseModel
{
    #region Member Fields
        private int m_AddedUpdatedBy;

        private int m_SecurityEntityID = -1;

        private int m_GroupSeqId = -1;

        private string m_Roles;
    #endregion

    #region Public Properties
        /// <summary>
        /// Gets or Added Updated By id.
        /// </summary>
        /// <value>The Added Updated By id.</value>
        public int AddedUpdatedBy
        {
            get { return m_AddedUpdatedBy; }
            set { m_AddedUpdatedBy = value; }
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
}
