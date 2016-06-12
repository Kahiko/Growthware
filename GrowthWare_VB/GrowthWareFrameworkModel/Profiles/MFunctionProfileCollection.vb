Imports System.Runtime.Serialization
Imports GrowthWare.Framework.Model.Profiles.Base

Namespace Profiles
	<Serializable(), CLSCompliant(True)> _
	Public NotInheritable Class MFunctionProfileCollection
		Inherits MProfileCollection

		Public Sub New()

		End Sub

		Private Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
			MyBase.New(info, context)
		End Sub
	End Class
End Namespace