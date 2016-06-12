
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using GrowthWare.Framework.ModelObjects.Base;
using GrowthWare.Framework.Enumerations;

namespace GrowthWare.Framework.ModelObjects
{
	[Serializable(), CLSCompliant(true)]
	public class MStateProfile : MProfile
	{

		#region "Private Methods"
		private new void init(DataRow Datarow)
		{
			base.init(Datarow);
			base.setString(ref mState, ref Datarow, "STATE");
			base.setString(ref mDescription, ref Datarow, "DESCRIPTION");
			base.setInt(ref mSTATUS_SEQ_ID, ref Datarow, "STATUS_SEQ_ID");
		}
		#endregion

		#region "Private Properties"
		    private string mState = "NEG1";
		    private string mDescription = string.Empty;
            private int mSTATUS_SEQ_ID = (int)SystemStatus.Inactive;
        #endregion

		#region "Public Properties"
		public string State {
			get { return mState; }
			set { mState = value.Trim(); }
		}

		public string Description {
			get { return mDescription; }
			set { mDescription = value.Trim(); }
		}

		public int STATUS_SEQ_ID {
			get { return mSTATUS_SEQ_ID; }
			set { mSTATUS_SEQ_ID = value; }
		}
		#endregion

		#region "Public Methods"
		/// <summary>
		/// Will return an instance populated with default values.
		/// </summary>
		/// <remarks></remarks>

		public MStateProfile()
		{
		}

		/// <summary>
		/// Will return an instance populated with information for the data row provided.
		/// </summary>
		/// <param name="dr"></param>
		/// <remarks></remarks>
		public MStateProfile(DataRow dr)
		{
			init(dr);
		}
		#endregion

	}
}
