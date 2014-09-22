using GrowthWare.Framework.Model.Profiles.Base;
using GrowthWare.Framework.Model.Profiles.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Model.Profiles
{
	/// <summary>
	/// Class MMessageProfile
	/// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MMessageProfile : MProfile, IMessageProfile
    {
        #region "Member Properties"
        private int m_SE_SEQ_ID = 1;
        private string m_Description = string.Empty;
        private string m_Title = string.Empty;
        private bool m_FormatAsHTML = false;
        private string m_Body = string.Empty;
        #endregion

        #region "Private Methods"
        /// <summary>
        /// Initializes values given a DataRow
        /// </summary>
        /// <param name="datarow">datarow</param>
        /// <remarks>Does not set ID or Name .. ColumnName should be unique to
        /// each inheriting class.</remarks>
        protected new void Initialize(DataRow datarow)
        {
            base.NameColumnName = "Name";
            base.IdColumnName = "Message_SeqID";
            base.Initialize(datarow);
            m_SE_SEQ_ID = base.GetInt(datarow, "SE_SEQ_ID");
            m_Title = base.GetString( datarow, "TITLE");
            m_Description = base.GetString(datarow, "DESCRIPTION");
            m_FormatAsHTML = base.GetBool(datarow, "FORMAT_AS_HTML");
            m_Body = base.GetString(datarow, "BODY");
        }
        #endregion

        #region "Public Methods"
        /// <summary>
        /// Will return a message profile with the default values
        /// </summary>
        /// <remarks></remarks>
        public MMessageProfile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MMessageProfile" /> class.
		/// </summary>
		/// <param name="profile">MMessageProfile</param>
        public MMessageProfile(MMessageProfile profile)
		{
			if (profile != null) 
			{
				this.AddedBy = profile.AddedBy;
				this.AddedDate = profile.AddedDate;
				this.Body = profile.Body;
				this.Description = profile.Description;
				this.FormatAsHTML = profile.FormatAsHTML;
				this.Id = profile.Id;
				this.Name = profile.Name;
				this.SE_SEQ_ID = profile.SE_SEQ_ID;
				this.Title = profile.Title;
				this.UpdatedBy = profile.UpdatedBy;
				this.UpdatedDate = profile.UpdatedDate;
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="MMessageProfile" /> class.
        /// </summary>
        /// <param name="dr">The dr.</param>
        public MMessageProfile(DataRow dr)
        {
            this.Initialize(dr);
        }

        /// <summary>
        /// Formats the body.
        /// </summary>
        void IMessageProfile.FormatBody()
        {
            PropertyInfo[] myPropertyInfo = this.GetType().GetProperties();
            PropertyInfo myPropertyItem = null;
            foreach (PropertyInfo myPropertyItem_loopVariable in myPropertyInfo)
            {
                myPropertyItem = myPropertyItem_loopVariable;
                object pValue = myPropertyItem.GetValue(this, null);
                if (pValue != null)
                {
                    this.m_Body = this.m_Body.Replace("<" + myPropertyItem.Name + ">", pValue.ToString());
                }
            }
        }

        /// <summary>
        /// Returns all properties encapsulated by angle brackets seporated by the Seporator parameter
        /// </summary>
        /// <param name="seporator">string</param>
        /// <returns>string</returns>
        public string GetTags(string seporator)
        {
            string retVal = string.Empty;
            PropertyInfo[] mPropertyInfo = this.GetType().GetProperties();
            foreach (PropertyInfo mPropertyItem in mPropertyInfo)
            {
                retVal = retVal + "<" + mPropertyItem.Name + ">" + seporator;
            }
            return retVal;
        }
        #endregion

        #region "Public Properties"
        /// <summary>
        /// Gets or sets the S e_ SE q_ ID.
        /// </summary>
        /// <value>The Security Entity ID.</value>
        public int SE_SEQ_ID
        {
            get { return m_SE_SEQ_ID; }
            set { m_SE_SEQ_ID = value; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value.Trim(); }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value.Trim(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [format as HTML].
        /// </summary>
        /// <value><c>true</c> if [format as HTML]; otherwise, <c>false</c>.</value>
        public bool FormatAsHTML
        {
            get { return m_FormatAsHTML; }
            set { m_FormatAsHTML = value; }
        }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public string Body
        {
            get { return m_Body; }
            set { m_Body = value.Trim(); }
        }
        #endregion
    }
}
