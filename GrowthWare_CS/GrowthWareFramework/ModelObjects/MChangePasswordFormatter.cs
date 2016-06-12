
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using GrowthWare.Framework.ModelObjects;
using GrowthWare.Framework.ModelObjects.Base;

namespace GrowthWare.Framework.ModelObjects
{
	[Serializable(), CLSCompliant(true)]
	public class MChangePasswordFormatter : MMessageProfile
	{
#region "Member Properties"
		private string m_AccountName = string.Empty;
		private string m_FullName = string.Empty;
		private string m_Password = string.Empty;
		private string m_Server = string.Empty;
#endregion

#region "Public Properties"
		public string AccountName {
			get { return m_AccountName; }
			set { m_AccountName = value.Trim(); }
		}

		public string FullName {
			get { return m_FullName; }
			set { m_FullName = value.Trim(); }
		}

		public string Password {
			get { return m_Password; }
			set { m_Password = value.Trim(); }
		}

		public string Server {
			get { return m_Server; }
			set { m_Server = value.Trim(); }
		}
#endregion
	}
}
