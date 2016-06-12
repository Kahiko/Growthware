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
	public class MRoleProfile : MProfile
	{

#region "Member Properties"
		    private string m_DESCRIPTION = string.Empty;
		    private int m_IsSystem = 0;
		    private int m_IsSystemOnly = 0;
			private int m_SE_SEQ_ID = 1;
#endregion


#region "Protected Methods"
		protected new void init(DataRow Datarow)
		{
			base.init(Datarow);
			base.setString(ref m_DESCRIPTION, ref Datarow, "DESCRIPTION");
			base.setInt(ref m_IsSystem, ref Datarow, "IS_SYSTEM");
			base.setInt(ref m_IsSystemOnly, ref Datarow, "IS_SYSTEM_ONLY");
			base.setInt(ref m_ID, ref Datarow, "ROLE_SEQ_ID");
			base.setString(ref m_Name, ref Datarow, "NAME");
		}
#endregion

		#region "Public Methods"
		/// <summary>
		/// Will return a message profile with the default vaules
		/// </summary>
		/// <remarks></remarks>

		public MRoleProfile()
		{
		}

		public MRoleProfile(DataRow dr)
		{
			init(dr);
		}
		#endregion

		#region "Public Properties"
		public int SecurityEntityID {
			get { return m_SE_SEQ_ID; }
			set { m_SE_SEQ_ID = value; }
		}
		public string Description {
			get { return m_DESCRIPTION; }
			set { m_DESCRIPTION = value.Trim(); }
		}

		public int IsSystem {
			get { return m_IsSystem; }
			set { m_IsSystem = value; }
		}

		public int IsSystemOnly {
			get { return m_IsSystemOnly; }
			set { m_IsSystemOnly = value; }
		}
		#endregion
	}
}
