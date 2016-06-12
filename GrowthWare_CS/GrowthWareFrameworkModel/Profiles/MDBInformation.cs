using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using GrowthWare.Framework.ModelObjects.Base;

namespace GrowthWare.Framework.ModelObjects
{
	/// <summary>
	/// Base properties an DB Information Profile
	/// </summary>
	/// <remarks>
	/// Corresponds to table ZF_INFORMATION and 
	/// Store procedures: 
	/// ZFP_SET_INFORMATION, ZFP_GET_INFORMATION
	/// </remarks>
	[Serializable(), CLSCompliant(true)]
	public sealed class MDBInformation : MProfile
	{
#region "Member Properties"
		    private int mInformation_SEQ_ID = 1;
			private string mVersion = string.Empty;
			private int mEnableInheritance = 1;
#endregion

#region "Public Properties"
		public int Information_SEQ_ID {
			get { return mInformation_SEQ_ID; }
			set { mInformation_SEQ_ID = value; }
		}

		public string Version {
			get { return mVersion.Trim(); }
			set { mVersion = value.Trim(); }
		}

		public int EnableInheritance {
			get { return mEnableInheritance; }
			set { mEnableInheritance = value; }
		}
#endregion

#region "Private Methods"
		private void Init(DataRow Datarow)
		{
			base.init(Datarow);
			base.setInt(ref mInformation_SEQ_ID, ref Datarow, "Information_SEQ_ID");
			base.setString(ref mVersion, ref Datarow, "VERSION");
			base.setInt(ref mEnableInheritance, ref Datarow, "ENABLE_INHERITANCE");
		}
#endregion

#region "Public Methods"

		public MDBInformation()
		{
		}

		public MDBInformation(DataRow dr)
		{
			this.Init(dr);
		}
#endregion
	}
}
