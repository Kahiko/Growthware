
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
namespace GrowthWare.Framework.ModelObjects
{
	/// <summary>
	/// Example of using inheritence to extend the base AccountProfile
	/// </summary>
	/// <remarks></remarks>
	[Serializable(), CLSCompliant(true)]
	public class ExtendedAccountProfile : MAccountProfile
	{

		#region "Private Members"
			#endregion
		private string m_ExtraProperty = string.Empty;

		#region "Public Methods"

		public ExtendedAccountProfile()
		{
		}

		public ExtendedAccountProfile(DataRow dr)
		{
			base.init(dr);
			base.setString(ref m_ExtraProperty, ref dr, "ExtraProperty");
		}
		#endregion

		#region "Public Properties"
		public string extraProperty {
			get { return m_ExtraProperty; }
			set { m_ExtraProperty = value.Trim(); }
		}
		#endregion
	}
}
