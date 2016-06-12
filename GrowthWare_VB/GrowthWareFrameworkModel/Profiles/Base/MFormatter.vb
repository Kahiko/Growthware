Imports System.Reflection
Imports GrowthWare.Framework.Model.Profiles.Base

Namespace ModelObjects.Base
	Public MustInherit Class MFormatter
		Inherits MProfile

		Protected m_Body As String

		Protected Overloads Sub Init(ByVal dr As DataRow)
			MyBase.Initialize(dr)
		End Sub
	End Class
End Namespace