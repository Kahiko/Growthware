Imports System.Runtime.Serialization
Imports GrowthWare.Framework.Model.Profiles.Base

Namespace Profiles
	<Serializable(), CLSCompliant(True)> _
	Public NotInheritable Class MFunctionTypeProfileCollection
		Inherits MProfileCollection

		Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
			MyBase.New(info, context)
		End Sub
	End Class
End Namespace