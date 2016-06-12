Imports GrowthWare.Framework.Model.Profiles

Namespace Base
    ''' <summary>
    ''' used by pages needed access to ClientChoicesState.
    ''' Also inerits from GrowthWare.Framework.Web.Base.Page that
    ''' stored session on the server keeping the view state returned
    ''' to the browser down to a minimum.
    ''' </summary>
    Public Class ClientChoicesPage
        Inherits BaseWebpage

        ''' <summary>
        ''' Gets the state of the client choices.
        ''' </summary>
        ''' <value>The state of the client choices.</value>
        Public ReadOnly Property ClientChoicesState() As MClientChoicesState
            Get
                Return CType(Context.Items(MClientChoices.SessionName), MClientChoicesState)
            End Get
        End Property

        ''' <summary>
        ''' Handles the PreInit event of the Page control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Protected Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            MyBase.Page_PreInit(sender, e)
        End Sub
    End Class

End Namespace
