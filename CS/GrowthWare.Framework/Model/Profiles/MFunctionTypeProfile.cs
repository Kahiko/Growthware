using GrowthWare.Framework.Model.Profiles.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Model.Profiles
{
    /// <summary>
    /// Class MFunctionTypeProfile
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MFunctionTypeProfile : MProfile
    {
        #region "Member Properties"
        private int m_FunctionTypeSeqId = -1;
        private string m_Description = string.Empty;
        private string m_Template = string.Empty;
        private bool m_IsContent;
        #endregion

        /// <summary>
        /// Will return a Function profile with the default values
        /// </summary>
        /// <remarks></remarks>
        public MFunctionTypeProfile()
        {
        }

        /// <summary>
        /// Will return a fully populated Function type profile.
        /// </summary>
        /// <param name="dataRow">A data row containing the Function type information</param>
        /// <remarks></remarks>
        public MFunctionTypeProfile(DataRow dataRow)
        {
            Initialize(dataRow);
        }

        /// <summary>
        /// Initializes the specified datarow.
        /// </summary>
        /// <param name="Datarow">The datarow.</param>
        protected new void Initialize(DataRow detailRow)
        {
            base.NameColumnName = "NAME";
            base.IdColumnName = "Function_Type_Seq_ID";
            if (detailRow != null) 
            {
                base.Initialize(detailRow);
                m_FunctionTypeSeqId = Id;
                m_Description = base.GetString(detailRow, "DESCRIPTION");
                m_Template = base.GetString(detailRow, "TEMPLATE");
                m_IsContent = base.GetBool(detailRow, "IS_CONTENT");
            }
        }

        /// <summary>
        /// Gets or sets the function_ type_ seq_ ID.
        /// </summary>
        /// <value>The FunctionTypeSeqId.</value>
        public int FunctionTypeSeqId
        {
            get { return m_FunctionTypeSeqId; }
            set { m_FunctionTypeSeqId = value; }
        }

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
        /// Gets or sets the TEMPLATE.
        /// </summary>
        /// <value>The TEMPLATE.</value>
        public string Template
        {
            get { return m_Template; }
            set { if(!string.IsNullOrEmpty(value)) m_Template = value.Trim(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [I s_ CONTENT].
        /// </summary>
        /// <value><c>true</c> if [I s_ CONTENT]; otherwise, <c>false</c>.</value>
        public bool IsContent
        {
            get { return m_IsContent; }
            set { m_IsContent = value; }
        }
    }
}