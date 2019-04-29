Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Model.Profiles.Base

''' <summary>
''' Class MUIAccountChoices
''' </summary>
Public Class MUIAccountChoices
    Inherits MProfile

    ''' <summary>
    ''' Initializes a new instance of the <see cref="MUIAccountChoices"/> class.
    ''' </summary>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="MUIAccountChoices"/> class.
    ''' </summary>
    ''' <param name="clientChoicesState">State of the client choices.</param>
    Public Sub New(ByVal clientChoicesState As MClientChoicesState)
        If clientChoicesState(MClientChoices.AccountName) <> Nothing Then AccountName = clientChoicesState(MClientChoices.AccountName).ToString()
        If clientChoicesState(MClientChoices.Action) <> Nothing Then Action = clientChoicesState(MClientChoices.Action).ToString()
        If clientChoicesState(MClientChoices.AlternatingRowBackColor) <> Nothing Then AlternatingRowBackColor = clientChoicesState(MClientChoices.AlternatingRowBackColor).ToString()
        If clientChoicesState(MClientChoices.BackColor) <> Nothing Then BackColor = clientChoicesState(MClientChoices.BackColor).ToString()
        If clientChoicesState(MClientChoices.ColorScheme) <> Nothing Then ColorScheme = clientChoicesState(MClientChoices.ColorScheme).ToString()
        If clientChoicesState(MClientChoices.HeadColor) <> Nothing Then HeadColor = clientChoicesState(MClientChoices.HeadColor).ToString()
        If clientChoicesState(MClientChoices.HeaderForeColor) <> Nothing Then HeaderForeColor = clientChoicesState(MClientChoices.HeaderForeColor).ToString()
        If clientChoicesState(MClientChoices.LeftColor) <> Nothing Then LeftColor = clientChoicesState(MClientChoices.LeftColor).ToString()
        If clientChoicesState(MClientChoices.RecordsPerPage) <> Nothing Then RecordsPerPage = Integer.Parse(clientChoicesState(MClientChoices.RecordsPerPage).ToString())
        If clientChoicesState(MClientChoices.RowBackColor) <> Nothing Then RowBackColor = clientChoicesState(MClientChoices.RowBackColor).ToString()
        If clientChoicesState(MClientChoices.SecurityEntityId) <> Nothing Then SecurityEntityID = Integer.Parse(clientChoicesState(MClientChoices.SecurityEntityId).ToString())
        If clientChoicesState(MClientChoices.SecurityEntityName) <> Nothing Then SecurityEntityName = clientChoicesState(MClientChoices.SecurityEntityName).ToString()
        If clientChoicesState(MClientChoices.SubheadColor) <> Nothing Then SubheadColor = clientChoicesState(MClientChoices.SubheadColor).ToString()
    End Sub

    ''' <summary>
    ''' Gets or sets the name of the account.
    ''' </summary>
    ''' <value>The name of the account.</value>
    Public Property AccountName As String

    ''' <summary>
    ''' Gets or sets the action.
    ''' </summary>
    ''' <value>The action.</value>
    Public Property Action As String

    ''' <summary>
    ''' Gets or sets the color of the back.
    ''' </summary>
    ''' <value>The color of the back.</value>
    Public Property BackColor As String

    ''' <summary>
    ''' Gets or sets the color scheme.
    ''' </summary>
    ''' <value>The color scheme.</value>
    Public Property ColorScheme As String

    ''' <summary>
    ''' Gets or sets the environment.
    ''' </summary>
    ''' <value>The environment.</value>
    Public Property Environment As String

    ''' <summary>
    ''' Gets or sets the color of the head.
    ''' </summary>
    ''' <value>The color of the head.</value>
    Public Property HeadColor As String

    ''' <summary>
    ''' Gets or sets the color of the header foreground color.
    ''' </summary>
    ''' <value>The color of the header foreground color.</value>
    Public Property HeaderForeColor As String

    ''' <summary>
    ''' Gets or sets the color of the left.
    ''' </summary>
    ''' <value>The color of the left.</value>
    Public Property LeftColor As String

    ''' <summary>
    ''' Gets or sets the records per page.
    ''' </summary>
    ''' <value>The records per page.</value>
    Public Property RecordsPerPage As Integer

    ''' <summary>
    ''' Gets or sets the account.
    ''' </summary>
    ''' <value>The account.</value>
    Public Property Account As String

    ''' <summary>
    ''' Gets or sets the security entity ID.
    ''' </summary>
    ''' <value>The security entity ID.</value>
    Public Property SecurityEntityID As Integer

    ''' <summary>
    ''' Gets or sets the version.
    ''' </summary>
    ''' <value>The version.</value>
    Public Property Version As String

    ''' <summary>
    ''' Gets or sets the framework version.
    ''' </summary>
    ''' <value>The version.</value>
    Public Property FrameworkVersion As String

    ''' <summary>
    ''' Gets or sets the name of the security entity.
    ''' </summary>
    ''' <value>The name of the security entity.</value>
    Public Property SecurityEntityName As String

    ''' <summary>
    ''' Gets or sets the color of the subhead.
    ''' </summary>
    ''' <value>The color of the subhead.</value>
    Public Property SubheadColor As String

    ''' <summary>
    ''' Gets or sets the color of the row background color.
    ''' </summary>
    ''' <value>The color of the row background color.</value>
    Public Property RowBackColor As String

    ''' <summary>
    ''' Gets or sets the color of the alternating row background color.
    ''' </summary>
    ''' <value>The color of the alternating row background color.</value>
    Public Property AlternatingRowBackColor As String
End Class
