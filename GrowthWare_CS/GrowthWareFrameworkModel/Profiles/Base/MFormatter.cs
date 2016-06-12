using System;
using System.Data;

namespace GrowthWare.Framework.Model.Profiles.Base
{
	public abstract class MFormatter : MProfile
	{
		protected string m_Body;

		protected override void Initialize(ref DataRow dr)
		{
			base.Initialize(ref dr);

			this.m_Body = base.GetString(ref dr, "Body");
		}
	}
}
