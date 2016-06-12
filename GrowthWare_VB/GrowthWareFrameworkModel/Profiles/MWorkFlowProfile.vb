Imports GrowthWare.Framework.ModelObjects.Base.Interfaces
Imports GrowthWare.Framework.ModelObjects.Base

Namespace ModelObjects
	<Serializable(), CLSCompliant(True)> _
	Public Class MWorkFlowProfile
		Inherits MProfile
		Implements IProfile

		''' <summary>
		''' Will return a workflow profile with the default vaules
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()

		End Sub

		Public Sub New(ByVal dr As DataRow)
			MyBase.Init(dr)
		End Sub
	End Class
End Namespace