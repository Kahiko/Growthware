Imports ApplicationBase.BusinessLogic.ClientChoices
Imports ApplicationBase.Model.Special.ClientChoices
Imports System
Imports System.Web
Imports System.Web.UI

#Region " Notes "
' The only differance between ClientChoicesPageModule and ClientChoicesControlModule
' is:
' ClientChoicesControlModule Inherits UserControl and 
' ClientChoicesPageModule Inherits Page
' Both will return the ClientChoicesState from ClientChoices#End Region
#End Region

Namespace ClientChoices
    Public Class ClientChoicesUserControl
        Inherits UserControl

        Public ReadOnly Property ClientChoicesState() As BClientChoicesState
            Get
                Dim myState As BClientChoicesState = CType(Context.Items(MClientChoices.sessionName), BClientChoicesState)
                If (myState Is Nothing) Then
					Dim Account As String = HttpContext.Current.User.Identity.Name
                    If Account.Trim.Length = 0 Then Account = "Anonymous"
                    myState = New BClientChoicesState(Account)
                    If (myState Is Nothing) Then
                        Throw New Exception("No Client Choices State Loaded!!!")
                    End If
                End If
                Return myState
            End Get
        End Property
    End Class
End Namespace