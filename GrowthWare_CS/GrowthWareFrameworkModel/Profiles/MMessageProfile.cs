using System;
using System.Data;
using System.Reflection;
using GrowthWare.Framework.Interfaces;
using GrowthWare.Framework.ModelObjects.Base;
using GrowthWare.Framework.ModelObjects.Base.Interfaces;

namespace GrowthWare.Framework.ModelObjects
{
	[Serializable(), CLSCompliant(true)]
	public class MMessageProfile : MFormatter
	{
#region "Member Properties"
		private int m_SE_SEQ_ID = 1;
		private string m_Description = string.Empty;
		private string m_Title = string.Empty;
		private bool m_FormatAsHTML = false;
#endregion

#region "Protected Methods"
		private new void init(DataRow Datarow)
		{
			base.init(Datarow);
			base.setInt(ref m_ID, ref Datarow, "MESSAGE_SEQ_ID");
			base.setInt(ref m_SE_SEQ_ID, ref Datarow, "SE_SEQ_ID");
			base.setString(ref m_Title, ref Datarow, "TITLE");
			base.setString(ref m_Description, ref Datarow, "DESCRIPTION");
			base.setBool(ref m_FormatAsHTML, ref Datarow, "FORMAT_AS_HTML");
		}
#endregion

#region "Public Methods"
		/// <summary>
		/// Will return a message profile with the default vaules
		/// </summary>
		/// <remarks></remarks>

		public MMessageProfile()
		{
		}

		public MMessageProfile(DataRow dr)
		{
			init(dr);
		}

		public virtual void FormatBody()
		{
			PropertyInfo[] myPropertyInfo = this.GetType().GetProperties();
			PropertyInfo myPropertyItem = null;
			foreach (PropertyInfo myPropertyItem_loopVariable in myPropertyInfo) {
				myPropertyItem = myPropertyItem_loopVariable;
				object pValue = myPropertyItem.GetValue(this, null);
				base.m_Body = base.m_Body.Replace("<" + myPropertyItem.Name + ">", pValue.ToString());
			}
		}
#endregion

#region "Public Properties"
		public int SE_SEQ_ID {
			get { return m_SE_SEQ_ID; }
			set { m_SE_SEQ_ID = value; }
		}

		public string Title {
			get { return m_Title; }
			set { m_Title = value.Trim(); }
		}

		public string Description {
			get { return m_Description; }
			set { m_Description = value.Trim(); }
		}

		public bool FormatAsHTML {
			get { return m_FormatAsHTML; }
			set { m_FormatAsHTML = value; }
		}

		public string Body {
			get { return m_Body; }
			set { m_Body = value.Trim(); }
		}

#endregion
	}
}
