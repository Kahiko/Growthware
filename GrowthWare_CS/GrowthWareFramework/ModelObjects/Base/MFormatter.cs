using System;
using System.Data;

namespace GrowthWare.Framework.ModelObjects.Base
{
	public abstract class MFormatter : MProfile
	{
		protected string m_Body;

		protected override void init(DataRow dr)
		{
			base.init(dr);

			base.setString(ref m_Body, ref dr, "BODY");
		}
	}
}
