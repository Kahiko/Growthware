using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.ModelObjects.Base;

namespace GrowthWare.Framework.ModelObjects
{
	/// <summary>
	/// Base properties a name value pair
	/// </summary>
	/// <remarks>
	/// Corresponds to table ZFC_NVP and 
	/// Store procedures: 
	/// ZFP_SET_NVP, ZFP_GET_NVP
	/// </remarks>
	[Serializable(), CLSCompliant(true)]
	public class MNameValuePairDetail : MProfile
	{

		#region "Member Properties"
		private int m_NVPSeqID = -1;
		private string m_Text = string.Empty;
		private string m_Value = string.Empty;
		private int m_SortOrder = 1;
			#endregion
		private int m_Status = 1;

		#region "Protected Methods"
		protected new void init(DataRow dr)
		{
			base.init(dr);
			base.setInt(ref base.m_ID , ref dr, "NVP_SEQ_DET_ID");
			base.setInt(ref m_NVPSeqID, ref dr, "NVP_SEQ_ID");
			base.setString(ref m_Text, ref dr, "NVP_DET_TEXT");
			base.m_Name = m_Text;
			base.setString(ref m_Value, ref dr, "NVP_DET_VALUE");
			base.setInt(ref m_Status, ref dr, "STATUS_SEQ_ID");
			base.setInt(ref m_SortOrder, ref dr, "SORT_ORDER");
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
		/// <param name="dr">Datarow containing base values</param>
		/// <remarks>
		/// Class should be inherited to extend to your project specific properties
		/// </remarks>
		public MNameValuePairDetail(DataRow dr)
		{
			init(dr);
		}
		#endregion

		#region "Public Properties"
		public int NVP_Seq_ID {
			get { return m_NVPSeqID; }
			set { m_NVPSeqID = value; }
		}

		public int Status {
			get { return m_Status; }
			set { m_Status = value; }
		}

		public int SortOrder {
			get { return m_SortOrder; }
			set { m_SortOrder = value; }
		}

		public string Text {
			get { return m_Text; }
			set { m_Text = value.Trim(); }
		}

		public string Value {
			get { return m_Value; }
			set { m_Value = value.Trim(); }
		}
		#endregion
	}
}
