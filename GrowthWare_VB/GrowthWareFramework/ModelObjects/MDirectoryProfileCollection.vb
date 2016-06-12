Imports System.Runtime.Serialization
Imports GrowthWare.Framework.ModelObjects.Base

Namespace ModelObjects
	<Serializable(), CLSCompliant(True)> _
	Public NotInheritable Class MDirectoryProfileCollection
		Inherits MProfileCollection

		Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
			MyBase.New(info, context)
		End Sub
	End Class
End Namespace