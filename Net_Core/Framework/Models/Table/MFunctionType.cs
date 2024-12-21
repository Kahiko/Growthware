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
    #region Member Properties
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

        /// <summary>
        /// Gets or sets the function_ type_ seq_ ID.
        /// </summary>
        /// <value>The FunctionTypeSeqId.</value>
        [DBPrimaryKey]
        [DBColumnName("FunctionTypeSeqId")]
        public int Id
        {
            get { return m_FunctionTypeSeqId; }
            set { m_FunctionTypeSeqId = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is Content.
        /// </summary>
        [DBColumnName("Is_Content")]
        public bool IsContent
        {
            get { return m_IsContent; }
            set { m_IsContent = value; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

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
            SetupClass();
        }

        /// <summary>
        /// Will return a fully populated Function type profile.
        /// </summary>
        /// <param name="dataRow">A data row containing the Function type information</param>
        /// <remarks></remarks>
        public MFunctionTypeProfile(DataRow dataRow)
        {
            SetupClass();
            Initialize(dataRow);
        }
    #endregion

    /// <summary>
    /// Initializes the specified datarow.
    /// </summary>
    /// <param name="detailRow">The datarow.</param>
    protected new void Initialize(DataRow detailRow)
    {
        // base.NameColumnName = "NAME";
        // base.IdColumnName = "Function_Type_Seq_ID";
        if (detailRow != null)
        {
            base.Initialize(detailRow);
            m_FunctionTypeSeqId = base.GetInt(detailRow, "Function_Type_Seq_ID");
            m_Description = base.GetString(detailRow, "DESCRIPTION");
            Name = base.GetString(detailRow, "NAME");
            m_Template = base.GetString(detailRow, "TEMPLATE");
            m_IsContent = base.GetBool(detailRow, "IS_CONTENT");
        }
    }

    protected override void SetupClass()
    {
        base.m_ForeignKeyName = "NOT_USED";
        m_TableName = "[ZGWSecurity].[Function_Types]";
    }
}
