Imports DALModel.Base

Namespace Special.Accounts
    <Serializable()> _
    Public Class MAccountProfileInfo
        Private _ACCOUNT_SEQ_ID As Integer = -1
        Private _SYSTEM_STATUS_ID As Integer = MSystemStatus.value.ChangePassword
        Private _AccountName As String = String.Empty
        Private _PWD As String = "password"
        Private _FailedAttempts As Integer = 0
        Private _FIRST_NAME As String = String.Empty
        Private _LAST_NAME As String = String.Empty
        Private _MIDDLE_NAME As String = String.Empty
        Private _PREFERED_NAME As String = String.Empty
        Private _EMAIL As String = String.Empty
        Private _TIME_ZONE As Integer = -5
        Private _LOCATION As String = String.Empty
        Private _CREATED_BY As Integer = -1
        Private _DATE_CREATED As DateTime
        Private _UPDATED_BY As Integer = -1
        Private _UPDATED_DATE As DateTime
        Private _LAST_LOGIN As DateTime
        Private _EnableNotifications As Boolean = False

        '*********************************************************************
        '
        ' ProfileInfo Constructor
        '
        ' Initializes a new instance of the ProfileInfo class.
        '
        '*********************************************************************
        Public Sub New()
        End Sub 'New

        '*********************************************************************
        '
        ' ID Property
        '
        ' The Account's sequence ID.
        '
        '*********************************************************************
        Public Property ACCOUNT_SEQ_ID() As Integer
            Get
                Return _ACCOUNT_SEQ_ID
            End Get
            Set(ByVal Value As Integer)
                _ACCOUNT_SEQ_ID = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' CREATED_BY Property
        '
        ' The Account's that created this account.
        '
        '*********************************************************************
        Public Property CREATED_BY() As Integer
            Get
                Return _CREATED_BY
            End Get
            Set(ByVal Value As Integer)
                _CREATED_BY = Value
            End Set
        End Property

        Public Property UPDATED_BY() As Integer
            Get
                Return _UPDATED_BY
            End Get
            Set(ByVal Value As Integer)
                _UPDATED_BY = Value
            End Set
        End Property

        Public Property UPDATED_DATE() As DateTime
            Get
                Return _UPDATED_DATE
            End Get
            Set(ByVal Value As DateTime)
                _UPDATED_DATE = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' SYSTEM_STATUS_ID Property
        '
        ' The Account's System status ID.
        '
        '*********************************************************************
        Public Property SYSTEM_STATUS_ID() As Integer
            Get
                Return _SYSTEM_STATUS_ID
            End Get
            Set(ByVal Value As Integer)
                _SYSTEM_STATUS_ID = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' AccountName Property
        '
        ' The client's account name.
        '
        '*********************************************************************

        Public Property ACCOUNT() As String
            Get
                Return _AccountName
            End Get
            Set(ByVal Value As String)
                _AccountName = Value.Trim
            End Set
        End Property

        '*********************************************************************
        '
        ' Password Property
        '
        ' The client's password.
        '
        '*********************************************************************

        Public Property PWD() As String
            Get
                Return _PWD
            End Get
            Set(ByVal Value As String)
                _PWD = Value.Trim
            End Set
        End Property

        '*********************************************************************
        '
        ' FailedAttempts Property
        '
        ' The Account's sequence ID.
        '
        '*********************************************************************
        Public Property FailedAttempts() As Integer
            Get
                Return _FailedAttempts
            End Get
            Set(ByVal Value As Integer)
                _FailedAttempts = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' FirstName Property
        '
        ' The client's first name.
        '
        '*********************************************************************

        Public Property First_Name() As String
            Get
                Return _FIRST_NAME
            End Get
            Set(ByVal Value As String)
                _FIRST_NAME = Value.Trim
            End Set
        End Property

        '*********************************************************************
        '
        ' LastName Property
        '
        ' The client's last name.
        '
        '*********************************************************************

        Public Property Last_Name() As String
            Get
                Return _LAST_NAME
            End Get
            Set(ByVal Value As String)
                _LAST_NAME = Value.Trim
            End Set
        End Property

        '*********************************************************************
        '
        ' Middle_Name Property
        '
        ' The client's last name.
        '
        '*********************************************************************

        Public Property Middle_Name() As String
            Get
                Return _MIDDLE_NAME
            End Get
            Set(ByVal Value As String)
                _MIDDLE_NAME = Value.Trim
            End Set
        End Property

        '*********************************************************************
        '
        ' Prefered_Name Property
        '
        ' The client's first name.
        '
        '*********************************************************************

        Public Property Prefered_Name() As String
            Get
                Return _PREFERED_NAME
            End Get
            Set(ByVal Value As String)
                _PREFERED_NAME = Value.Trim
            End Set
        End Property

        '*********************************************************************
        '
        ' EMAIL Property
        '
        ' The clients email address.
        '
        '*********************************************************************

        Public Property EMAIL() As String
            Get
                Return _EMAIL
            End Get
            Set(ByVal Value As String)
                _EMAIL = Value.Trim
            End Set
        End Property

        '*********************************************************************
        '
        ' TimeZone Property
        '
        ' The client's time zone.
        '
        '*********************************************************************

        Public Property TIME_ZONE() As Integer
            Get
                Return _TIME_ZONE
            End Get
            Set(ByVal Value As Integer)
                _TIME_ZONE = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' Location Property
        '
        ' The client's location.
        '
        '*********************************************************************

        Public Property Location() As String
            Get
                Return _LOCATION
            End Get
            Set(ByVal Value As String)
                _LOCATION = Value.Trim
            End Set
        End Property

        '*********************************************************************
        '
        ' DateCreated Property
        '
        ' The date the account registered.
        '
        '*********************************************************************

        Public Property DATE_CREATED() As DateTime
            Get
                Return _DATE_CREATED
            End Get
            Set(ByVal Value As DateTime)
                _DATE_CREATED = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' LAST_LOGIN Property
        '
        ' The date the client last explicitly logged in.
        '
        '*********************************************************************

        Public Property LAST_LOGIN() As DateTime
            Get
                Return _LAST_LOGIN
            End Get
            Set(ByVal Value As DateTime)
                _LAST_LOGIN = Value
            End Set
        End Property


        '*********************************************************************
        '
        ' EnableNotifications Property
        '
        ' Indicates whether the user wants to receive
        ' email notifications.
        '
        '*********************************************************************

        Public Property EnableNotifications() As Boolean
            Get
                Return _EnableNotifications
            End Get
            Set(ByVal Value As Boolean)
                _EnableNotifications = Value
            End Set
        End Property
    End Class
End Namespace