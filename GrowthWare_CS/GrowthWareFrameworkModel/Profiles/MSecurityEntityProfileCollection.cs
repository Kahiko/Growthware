using System;
using System.Runtime.Serialization;
using GrowthWare.Framework.Model.Profiles.Base;

namespace GrowthWare.Framework.Model.Profiles
{
	[Serializable(), CLSCompliant(true)]
	public sealed class MSecurityEntityProfileCollection : MProfileCollection
	{

		MSecurityEntityProfileCollection(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			// do nothing
		}
	}
}
