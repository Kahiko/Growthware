using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Model object representing the GroupProfile
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MGroupProfile : AAddedUpdated
{

    #region Member Fields
        private string m_Description = string.Empty;
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

        [DBPrimaryKey]
        [DBColumnName("GroupSeqId")]
        public int Id { get; set; }

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
        /// Will return a message profile with the default value's
        /// </summary>
        /// <remarks></remarks>
        public MGroupProfile()
        {
            SetupClass();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MGroupProfile" /> class.
        /// </summary>
        /// <param name="profileDataRow">The profile data row.</param>
        public MGroupProfile(DataRow profileDataRow)
        {
            SetupClass();
            this.Initialize(profileDataRow);
        }
    #endregion

    protected new void Initialize(DataRow dataRow)
    {
        base.Initialize(dataRow);
        this.Id = base.GetInt(dataRow, "GROUP_SEQ_ID");
        this.Name = base.GetString(dataRow, "NAME");
        m_Description = base.GetString(dataRow, "DESCRIPTION");
    }

    protected override void SetupClass()
    {
        base.m_ForeignKeyName = "NOT_USED";
        m_TableName = "[ZGWSecurity].[Groups]";
    }
}
