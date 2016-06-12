using System;

namespace GrowthWare.Framework.Model.Profiles
{
	/// <summary>
	/// All propertyies represent the column names in the ZF_ACCT_CHOICES table.
	/// </summary>
	/// <remarks></remarks>
	[Serializable(), CLSCompliant(true)]
	public sealed class MClientChoices
	{

		private MClientChoices()
		{
		}

		public static string RecordsPerPage {
			get { return "RECORDS_PER_PAGE"; }
		}

		public static string AnonymousClientChoicesState {
			get { return "AnonymousClientChoicesState"; }
		}

		public static string SessionName {
			get { return "ClientChoicesState"; }
		}

		public static string AccountName {
			get { return "ACCT"; }
		}

		public static string SecurityEntityID {
			get { return "SE_SEQ_ID"; }
		}

		public static string SecurityEntityName {
			get { return "SE_NAME"; }
		}

		public static string BackColor {
			get { return "BACK_COLOR"; }
		}

		public static string LeftColor {
			get { return "LEFT_COLOR"; }
		}

		public static string HeadColor {
			get { return "HEAD_COLOR"; }
		}

		public static string SubheadColor {
			get { return "SUB_HEAD_COLOR"; }
		}

		public static string ColorScheme {
			get { return "COLOR_SCHEME"; }
		}

		public static string Action {
			get { return "FAVORIATE_ACTION"; }
		}
	}
}
