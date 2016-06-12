using System;
using System.Runtime.Serialization;
using GrowthWare.Framework.ModelObjects.Base;

namespace GrowthWare.Framework.ModelObjects
{
	[Serializable(), CLSCompliant(true)]
	public sealed class MFunctionTypeProfileCollection : MProfileCollection
	{

		MFunctionTypeProfileCollection(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			// do nothing
		}
	}
}
