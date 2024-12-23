using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Properties for an Name Value Pair Detail.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MWorkFlows : MNameValuePairDetail
{

    #region Member Fields
        private int m_NameValuePairSeqId = -1;
        private string m_Text = string.Empty;
        private string m_Value = string.Empty;
        private int m_SortOrder = 0;
        private int m_Status = 1;
    #endregion

    #region Public Properties

        [DBPrimaryKey]
        [DBColumnName("NVP_DetailSeqId")]
        public int Id { get; set; }

        public string Name
        {
            get { return m_Text; }
            set { if (!String.IsNullOrEmpty(value)) m_Text = value.Trim(); }
        }

        /// <summary>
        /// Gets or sets the Name Value Pair SeqId.
        /// </summary>
        /// <value>The Name Value Pair SeqId.</value>
        [DBColumnName("NVPSeqId")]
        public int NameValuePairSeqId
        {
            get { return m_NameValuePairSeqId; }
            set { m_NameValuePairSeqId = value; }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [DBColumnName("StatusSeqId")]
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>The sort order.</value>
        [DBColumnName("Sort_Order")]
        public int SortOrder
        {
            get { return m_SortOrder; }
            set { m_SortOrder = value; }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [DBColumnName("NVP_Detail_Name")]
        public string Text
        {
            get { return m_Text; }
            set { if (!String.IsNullOrEmpty(value)) m_Text = value.Trim(); }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [DBColumnName("NVP_Detail_Value")]
        public string Value
        {
            get { return m_Value; }
            set { if (!String.IsNullOrEmpty(value)) m_Value = value.Trim(); }
        }
    #endregion

    #region Constructors
        /// <summary>
        /// Provides a new account profile with the default vaules
        /// </summary>
        /// <remarks></remarks>
        public MWorkFlows()
        {
            this.SetupClass();
        }

        /// <summary>
        /// Will populate values based on the contents of the data row.
        /// </summary>
        /// <param name="dataRow">Datarow containing base values</param>
        /// <remarks>
        /// Class should be inherited to extend to your project specific properties
        /// </remarks>
        public MWorkFlows(DataRow dataRow)
        {
            this.SetupClass();
            this.Initialize(dataRow);
        }
    #endregion

    /// <summary>
    /// Initializes the specified dr.
    /// </summary>
    /// <param name="dataRow">The dr.</param>
    protected new void Initialize(DataRow dataRow)
    {
        // base.IdColumnName = "NVP_SEQ_DET_ID";
        // base.NameColumnName = "NVP_DET_TEXT";
        base.Initialize(dataRow);
        this.Id = base.GetInt(dataRow, "NVP_SEQ_DET_ID");
        m_NameValuePairSeqId = base.GetInt(dataRow, "NVP_SEQ_ID");
        m_Text = base.GetString(dataRow, "NVP_DET_TEXT");
        m_Value = base.GetString(dataRow, "NVP_DET_VALUE");
        m_Status = base.GetInt(dataRow, "STATUS_SEQ_ID");
        m_SortOrder = base.GetInt(dataRow, "SORT_ORDER");
    }

    protected override void SetupClass()
    {
        this.Id = -1;
        base.m_ForeignKeyName = "NOT_USED";
        m_TableName = "[ZGWCoreWeb].[Work_Flows]";
    }

}
