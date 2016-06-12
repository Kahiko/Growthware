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
        private int m_SecurityEntity_Seq_Id = 1;
        private string m_Description = string.Empty;
        private string m_Title = string.Empty;
        private bool m_FormatAsHTML = false;
        private string m_Body = string.Empty;
#endregion

#region "Private Methods"
        /// <summary>
        /// Initializes values given a DataRow
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <remarks>Does not set ID or Name .. ColumnName should be unique to
        /// each inheriting class.</remarks>
        protected new void Initialize(DataRow dataRow)
        {
            base.NameColumnName = "Name";
            base.IdColumnName = "MESSAGE_SEQ_ID";
            base.Initialize(dataRow);
            m_SecurityEntity_Seq_Id = base.GetInt(dataRow, "SE_SEQ_ID");
            m_Title = base.GetString(dataRow, "TITLE");
            m_Description = base.GetString(dataRow, "DESCRIPTION");
            m_FormatAsHTML = base.GetBool(dataRow, "FORMAT_AS_HTML");
            m_Body = base.GetString(dataRow, "BODY");
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
                this.FormatAsHtml = profile.FormatAsHtml;
                this.Id = profile.Id;
                this.Name = profile.Name;
                this.SecurityEntitySeqId = profile.SecurityEntitySeqId;
                this.Title = profile.Title;
                this.UpdatedBy = profile.UpdatedBy;
                this.UpdatedDate = profile.UpdatedDate;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MMessageProfile" /> class.
        /// </summary>
        /// <param name="dataRow">The DataRow.</param>
        public MMessageProfile(DataRow dataRow)
        {
            this.Initialize(dataRow);
        }

        /// <summary>
        /// Sets or gets the body property
        /// </summary>
        /// <value>Sets the value</value>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public string Body
        {
            get { return m_Body; }
            set { if (value != null) m_Body = value.Trim(); }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return m_Description; }
            set 
            {
                if(value != null) m_Description = value.Trim(); 
            }
        }

        /// <summary>
        /// Sets or gets the Title property
        /// </summary>
        /// <value>Sets the value</value>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public string Title
        {
            get { return m_Title; }
            set { if(value != null) m_Title = value.Trim(); }
        }
        /// <summary>
        /// Gets or sets the S e_ SE q_ ID.
        /// </summary>
        /// <value>The Security Entity ID.</value>
        public int SecurityEntitySeqId
        {
            get { return m_SecurityEntity_Seq_Id; }
            set { m_SecurityEntity_Seq_Id = value; }
        }

        /// <summary>
        /// Sets or gets the FormatAsHtml property
        /// </summary>
        /// <value>Sets the value</value>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public bool FormatAsHtml
        {
            get { return m_FormatAsHTML; }
            set { m_FormatAsHTML = value; }
        }

        /// <summary>
        /// Formats the body and replaces property names within angle brackes with the appropriate property value.
        /// </summary>
        public void FormatBody()
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
        /// <param name="separator">string</param>
        /// <returns>string</returns>
        public string GetTags(string separator)
        {
            string retVal = string.Empty;
            PropertyInfo[] mPropertyInfo = this.GetType().GetProperties();
            foreach (PropertyInfo mPropertyItem in mPropertyInfo)
            {
                retVal = retVal + "<" + mPropertyItem.Name + ">" + separator;
            }
            return retVal;
        }
    }
#endregion

}
