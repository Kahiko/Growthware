Public Class MUIRoleProfile

#Region "Public Properties"

    ''' <summary>
    ''' Accounts that are associated with this role but are associated with the SecurityEntityId
    ''' </summary>
    Public AccountsInRole() As String

    ''' <summary>
    ''' Accounts that are not associated with this role but are associated with the SecurityEntityId
    ''' </summary>
    Public AccountsNotInRole() As String

    Public Property Id As Integer = -1

    Public Property Name() As String

    ''' <summary>
    ''' Gets or sets the SecurityEntityId for the current or selected security entity of the current account.
    ''' </summary>
    ''' <value>The SecurityEntityId.</value>
    Public Property SecurityEntityId() As Integer

    ''' <summary>
    ''' Gets or sets the description.
    ''' </summary>
    ''' <value>The description.</value>
    Public Property Description() As String

    ''' <summary>
    ''' Gets or sets the is system.
    ''' </summary>
    ''' <value>The is system.</value>
    Public Property IsSystem() As Boolean

    ''' <summary>
    ''' Gets or sets the is system only.
    ''' </summary>
    ''' <value>The is system only.</value>
    Public Property IsSystemOnly() As Boolean
#End Region
End Class
