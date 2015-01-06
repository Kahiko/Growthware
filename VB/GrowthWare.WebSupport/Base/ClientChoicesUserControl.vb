Imports GrowthWare.Framework.Model.Profiles

Namespace Base
    Public Class ClientChoicesUserControl
        Inherits BaseUserControl

        ''' <summary>
        ''' Gets the state of the client choices.
        ''' </summary>
        ''' <value>The state of the client choices.</value>
        Public ReadOnly Property ClientChoicesState() As MClientChoicesState
            Get
                Return CType(Context.Items(MClientChoices.SessionName), MClientChoicesState)
            End Get
        End Property
    End Class
End Namespace