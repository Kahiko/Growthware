using GrowthWare.Framework.Model.Profiles.Base;
using GrowthWare.Framework.Model.Profiles.Interfaces;
using System;
using System.Data;

namespace GrowthWare.Framework.Model.Profiles
{
    /// <summary>
    /// Properties for an Name Value Pair Detail.
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MNameValuePairDetail : MProfile
    {
        #region "Member Properties"
        private int m_NameValuePairSeqId = -1;
        private string m_Text = string.Empty;
        private string m_Value = string.Empty;
        private int m_SortOrder = 0;
        private int m_Status = 1;
        #endregion

        #region "Protected Methods"
        /// <summary>
        /// Initializes the specified dr.
        /// </summary>
        /// <param name="dataRow">The dr.</param>
        protected new void Initialize(DataRow dataRow)
        {
            base.NameColumnName = "STATIC_NAME";
            base.IdColumnName = "NVP_SEQ_DET_ID";
            base.Initialize(dataRow);
            m_NameValuePairSeqId = base.GetInt(dataRow, "NVP_SEQ_ID"); ;
            m_Text = base.Name;
            m_Value = base.GetString(dataRow, "NVP_DET_VALUE");
            m_Status = base.GetInt(dataRow, "STATUS_SEQ_ID");
            m_SortOrder = base.GetInt(dataRow, "SORT_ORDER");
        }
        #endregion

        #region "Public Methods"
        /// <summary>
        /// Provides a new account profile with the default vaules
        /// </summary>
        /// <remarks></remarks>
        public MNameValuePairDetail()
        {
        }

        /// <summary>
        /// Will populate values based on the contents of the data row.
        /// </summary>
        /// <param name="dataRow">Datarow containing base values</param>
        /// <remarks>
        /// Class should be inherited to extend to your project specific properties
        /// </remarks>
        public MNameValuePairDetail(DataRow dataRow)
        {
            this.Initialize(dataRow);
        }
        #endregion

        #region "Public Properties"
        /// <summary>
        /// Gets or sets the Name Value Pair SeqId.
        /// </summary>
        /// <value>The Name Value Pair SeqId.</value>
        public int NameValuePairSeqId
        {
            get { return m_NameValuePairSeqId; }
            set { m_NameValuePairSeqId = value; }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>The sort order.</value>
        public int SortOrder
        {
            get { return m_SortOrder; }
            set { m_SortOrder = value; }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return m_Text; }
            set { if (!String.IsNullOrEmpty(value)) m_Text = value.Trim(); }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return m_Value; }
            set { if (!String.IsNullOrEmpty(value)) m_Value = value.Trim(); }
        }
        #endregion
    }
}
