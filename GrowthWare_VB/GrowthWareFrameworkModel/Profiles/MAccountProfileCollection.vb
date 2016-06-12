Imports GrowthWare.Framework.Model.Profiles.Base
Imports System.Runtime.Serialization

Namespace Profiles
	<Serializable(), CLSCompliant(True)> _
	Public NotInheritable Class MAccountProfileCollection
		Inherits MProfileCollection

		Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
			MyBase.New(info, context)
		End Sub
	End Class
End Namespace