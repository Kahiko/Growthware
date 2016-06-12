Imports System.Reflection
Imports GrowthWare.Framework.ModelObjects.Base

Namespace ModelObjects.Base
	Public MustInherit Class MFormatter
		Inherits MProfile

		Protected m_Body As String

		Protected Overloads Sub Init(ByVal dr As DataRow)
			MyBase.Init(dr)
		End Sub
	End Class
End Namespace