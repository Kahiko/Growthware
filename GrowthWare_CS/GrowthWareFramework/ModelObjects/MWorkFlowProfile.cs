
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
	public class MWorkFlowProfile : MProfile
	{

		/// <summary>
		/// Will return a workflow profile with the default vaules
		/// </summary>
		/// <remarks></remarks>

		public MWorkFlowProfile()
		{
		}

		public MWorkFlowProfile(DataRow dr)
		{
			base.init(dr);
		}
	}
}
