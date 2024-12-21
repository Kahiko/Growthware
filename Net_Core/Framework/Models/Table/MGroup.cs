using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Model object representing the GroupProfile
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MGroupProfile : AbstractBaseModel
{

    #region Member Fields
        private string m_Description = string.Empty;
        private int m_SecurityEntityID = 1;
    #endregion

    #region Public Properties
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
            set { if (value != null) m_Description = value.Trim(); }
        }
    #endregion

    #region Constructors
        /// <summary>
        /// Will return a message profile with the default value's
        /// </summary>
        /// <remarks></remarks>
        public MGroupProfile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MGroupProfile" /> class.
        /// </summary>
        /// <param name="profileDataRow">The profile data row.</param>
        public MGroupProfile(DataRow profileDataRow)
        {
            this.Initialize(profileDataRow);
        }
    #endregion

    protected new void Initialize(DataRow dataRow)
    {
        base.NameColumnName = "NAME";
        base.IdColumnName = "GROUP_SEQ_ID";
        base.Initialize(dataRow);
        m_Description = base.GetString(dataRow, "DESCRIPTION");
    }

}
