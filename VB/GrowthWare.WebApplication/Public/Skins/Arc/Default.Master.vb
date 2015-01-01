Imports GrowthWare.Framework.Model.Profiles

Public Class _Default6
    Inherits System.Web.UI.MasterPage

    ''' <summary>
    ''' Gets the state of the client choices.
    ''' </summary>
    ''' <value>The state of the client choices.</value>
    Public ReadOnly Property ClientChoicesState() As MClientChoicesState
        Get
            Return CType(Context.Items(MClientChoices.SessionName), MClientChoicesState)
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class