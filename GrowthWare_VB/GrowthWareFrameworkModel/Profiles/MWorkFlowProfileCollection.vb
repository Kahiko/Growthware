Imports System.Runtime.Serialization
Imports GrowthWare.Framework.ModelObjects.Base

Namespace ModelObjects
	<Serializable(), CLSCompliant(True)>
	Public NotInheritable Class MWorkFlowProfileCollection
		Inherits MProfileCollection

		Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
			MyBase.New(info, context)
		End Sub
	End Class
End Namespace