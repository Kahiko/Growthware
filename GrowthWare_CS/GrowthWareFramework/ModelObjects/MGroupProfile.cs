
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
	public class MGroupProfile : MProfile
	{

		#region "Member Properties"
		private string mDESCRIPTION = string.Empty;
			#endregion
		private int mSE_SEQ_ID = 1;

		#region "Protected Methods"
		protected new void init(DataRow Datarow)
			{
				base.init(Datarow);
				base.setString(ref mDESCRIPTION, ref Datarow, "DESCRIPTION");
				base.setInt(ref m_ID, ref Datarow, "GROUP_SEQ_ID");
				base.setString(ref m_Name, ref Datarow, "NAME");
			}
		#endregion

		#region "Public Methods"
		/// <summary>
		/// Will return a message profile with the default vaules
		/// </summary>
		/// <remarks></remarks>

		public MGroupProfile()
		{
		}

		public MGroupProfile(DataRow ProfileDataRow)
		{
			init(ProfileDataRow);
		}
		#endregion

		#region "Public Properties"
		public int SecurityEntityID {
			get { return mSE_SEQ_ID; }
			set { mSE_SEQ_ID = value; }
		}

		public string Description {
			get { return mDESCRIPTION; }
			set { mDESCRIPTION = value.Trim(); }
		}
		#endregion
	}
}
