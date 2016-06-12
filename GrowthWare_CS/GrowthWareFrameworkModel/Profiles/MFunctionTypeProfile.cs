
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using GrowthWare.Framework.ModelObjects.Base.Interfaces;
using GrowthWare.Framework.ModelObjects.Base;

namespace GrowthWare.Framework.ModelObjects
{
	[Serializable(), CLSCompliant(true)]
	public class MFunctionTypeProfile : MProfile
	{
#region "Member Properties"
		private int  m_Function_Type_Seq_ID = -1;
		private string m_Description = string.Empty;
		private string m_TEMPLATE = string.Empty;
		private bool m_IS_CONTENT;
#endregion

		/// <summary>
		/// Will return a Function profile with the default vaules
		/// </summary>
		/// <remarks></remarks>
		public MFunctionTypeProfile()
		{
		}

		/// <summary>
		/// Will return a fully populated Function type profile.
		/// </summary>
		/// <param name="Datarow">A data row containing the Function type information</param>
		/// <remarks></remarks>
		public MFunctionTypeProfile(DataRow Datarow)
		{
			init(Datarow);
		}

		protected new void init(DataRow Datarow)
		{
			base.init(Datarow);
			base.setInt(ref m_Function_Type_Seq_ID, ref Datarow, "Function_Type_Seq_ID");
			m_ID = m_Function_Type_Seq_ID;
			base.setString(ref m_Name, ref Datarow, "NAME");
			base.setString(ref m_Description, ref Datarow, "DESCRIPTION");
			base.setString(ref m_TEMPLATE, ref Datarow, "TEMPLATE");
			base.setBool(ref m_IS_CONTENT, ref Datarow, "IS_CONTENT");
		}

		public int Function_Type_Seq_ID {
			get { return m_Function_Type_Seq_ID; }
			set { m_Function_Type_Seq_ID = value; }
		}

		public string Description {
			get { return m_Description; }
			set { m_Description = value.Trim(); }
		}

		public string TEMPLATE {
			get { return m_TEMPLATE; }
			set { m_TEMPLATE = value.Trim(); }
		}

		public bool IS_CONTENT {
			get { return m_IS_CONTENT; }
			set { m_IS_CONTENT = value; }
		}
	}
}
