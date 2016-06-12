Imports BLL.Base.ClientChoices
Imports DALModel.Special.ClientChoices
Imports System
Imports System.Web
Imports System.Collections
Imports System.Web.UI
Imports System.Web.UI.HtmlControls

#Region " Notes "
' The only differance between ClientChoicesPageModule and ClientChoicesControlModule
' is:
' ClientChoicesControlModule Inherits UserControl and 
' ClientChoicesPageModule Inherits Page
' Both will return the ClientChoicesState from ClientChoices
#End Region

Namespace ClientChoices
    Public Class ClientChoicesPage
        Inherits BasePage

        Public ReadOnly Property ClientChoicesState() As BClientChoicesState
            Get
                Dim myState As BClientChoicesState = CType(Context.Items(MClientChoices.sessionName), BClientChoicesState)
                If (myState Is Nothing) Then
                    Dim Account As String = context.Current.User.Identity.Name
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