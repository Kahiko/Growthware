Imports Common
Imports BLL.Base.ClientChoices
Imports DALModel.Special.ClientChoices
#Region " Notes "
' The ClientChoicesUtility gives access to the choices a client has made.
#End Region
Public Class ClientChoicesUtility
	Private Shared CachedAnonymousClientChoicesInfo As String = "AnonymousClientChoicesInfo"

	Private Shared Function getClientState() As BClientChoicesState
		Dim myClientChoices As BClientChoicesState
		Try
			myClientChoices = HttpContext.Current.Items(MClientChoices.sessionName)
		Catch ex As Exception
			Dim mike As String = String.Empty
		End Try
		Return myClientChoices
	End Function

	Public Shared Function GetSelectedBusinessUnit() As Integer
		Dim myBusinessUnitID As Integer = 1
		If Not HttpContext.Current Is Nothing Then
			If Not HttpContext.Current.User Is Nothing Then
				If HttpContext.Current.User.Identity.IsAuthenticated OrElse HttpContext.Current.User.Identity.Name = "Anonymous" Then
					Dim myClientChoices As BClientChoicesState = getClientState()
					If Not myClientChoices Is Nothing Then
						myBusinessUnitID = myClientChoices(MClientChoices.BusinessUnitID)
					Else
						myClientChoices = New BClientChoicesState(HttpContext.Current.User.Identity.Name)
						If Not myClientChoices Is Nothing Then
							myBusinessUnitID = myClientChoices(MClientChoices.BusinessUnitID)
						End If
					End If
				End If
			End If
		End If
		Return myBusinessUnitID
	End Function
End Class