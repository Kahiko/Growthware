Imports GrowthWare.Framework.Model.Profiles

Namespace Base
    ''' <summary>
    ''' Used as the base class for user control objected requiring ClientChoicesState
    ''' </summary>
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