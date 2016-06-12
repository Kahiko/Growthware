
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
namespace GrowthWare.Framework.ModelObjects
{
	[Serializable(), CLSCompliant(true)]
	public class MGroupRoles
	{
		private int m_ADD_UP_BY;
		private int m_SE_SEQ_ID = -1;
		private int m_GROUP_SEQ_ID = -1;

		private string mRoles;
		public int ADD_UP_BY {
			get { return m_ADD_UP_BY; }
			set { m_ADD_UP_BY = value; }
		}

		public int SE_SEQ_ID {
			get { return m_SE_SEQ_ID; }
			set { m_SE_SEQ_ID = value; }
		}

		public int GROUP_SEQ_ID {
			get { return m_GROUP_SEQ_ID; }
			set { m_GROUP_SEQ_ID = value; }
		}

		public string Roles {
			get { return mRoles; }
			set { mRoles = value.Trim(); }
		}
	}
}
