
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
	[Serializable(), CLSCompliant(true)]
	public class MNotificationsProfile
	{

#region "Member Properties"
		private int m_SE_SEQ_ID = -1;
		private bool m_Status = true;
		private string m_Account = string.Empty;
		private int m_Function_Seq_ID = -1;
#endregion

#region "Public Properties"
		public int SecurityEntityID {
			get { return m_SE_SEQ_ID; }
			set { m_SE_SEQ_ID = value; }
		}

		public bool Status {
			get { return m_Status; }
			set { m_Status = value; }
		}

		public string Account {
			get { return m_Account; }
			set { m_Account = value.Trim(); }
		}

		public int FunctionID {
			get { return m_Function_Seq_ID; }
			set { m_Function_Seq_ID = value; }
		}
#endregion

#region "Public Methods"
		/// <summary>
		/// Provides a new account profile with the default vaules
		/// </summary>
		/// <remarks></remarks>

		public MNotificationsProfile()
		{
		}
#endregion
	}
}
