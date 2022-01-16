Public Class MUIGroupProfile

#Region "Public Properties"

    ''' <summary>
    ''' Roles that are associated with this group but are associated with the SecurityEntityId
    ''' </summary>
    Public RolesInGroup() As String

    ''' <summary>
    ''' Roles that are not associated with this group but are associated with the SecurityEntityId
    ''' </summary>
    Public RolesNotInGroup() As String

    Public Property Id As Integer = -1

    Public Property Name() As String

    ''' <summary>
    ''' Gets or sets the description.
    ''' </summary>
    ''' <value>The description.</value>
    Public Property Description() As String
#End Region
End Class
