
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
	public class MNameValuePair : MProfile
	{

		#region "Member Properties"
			private string m_SchemaName = string.Empty;
			private string m_StaticName = "NEW";
			private string m_Display = string.Empty;
			private string m_Description = string.Empty;
			private int m_Status = -1;
		#endregion

		#region "Protected Methods"
		protected new void init(DataRow Datarow)
		{
			base.init(Datarow);
			base.setInt(ref base.m_ID, ref Datarow, "NVP_SEQ_ID");
			base.setString(ref m_SchemaName, ref Datarow, "SCHEMA_NAME");
			base.setString(ref m_StaticName, ref Datarow, "STATIC_NAME");
			base.m_Name = m_StaticName;
			base.setString(ref m_Display, ref Datarow, "DISPLAY");
			base.setString(ref m_Description, ref Datarow, "DESCRIPTION");
			base.setInt(ref m_Status, ref Datarow, "STATUS_SEQ_ID");
		}
		#endregion

		#region "Public Methods"
		/// <summary>
		/// Provides a new account profile with the default vaules
		/// </summary>
		/// <remarks></remarks>

		public MNameValuePair()
		{
		}

		/// <summary>
		/// Will populate values based on the contents of the data row.
		/// </summary>
		/// <param name="dr">Datarow containing base values</param>
		/// <remarks>
		/// Class should be inherited to extend to your project specific properties
		/// </remarks>
		public MNameValuePair(DataRow dr)
		{
			init(dr);
		}
		#endregion

		#region "Public Properties"
		public int Status {
			get { return m_Status; }
			set { m_Status = value; }
		}

		public string SchemaName
		{
			get{ return m_SchemaName; }
			set{ m_SchemaName = value.Trim(); }
		}

		public string StaticName {
			get { return m_StaticName; }
			set { m_StaticName = value.Trim(); }
		}

		public string Display {
			get { return m_Display; }
			set { m_Display = value.Trim(); }
		}

		public string Description {
			get { return m_Description; }
			set { m_Description = value.Trim(); }
		}
		#endregion
	}
}
