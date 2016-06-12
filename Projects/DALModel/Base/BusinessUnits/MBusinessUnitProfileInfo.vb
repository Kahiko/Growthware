Namespace Base.BusinessUnits
    Public Class MBusinessUnitProfileInfo
        Private _Business_Unit_Seq_ID As Integer = 1
        Private _Name As String = String.Empty
        Private _Description As String = String.Empty
        Private _Skin As String = "Default"
        Private _Parent_Business_Unit_Seq_ID As Integer = 0
        Private _ConnectionString As String = String.Empty
        Private _STATUS_SEQ_ID As Integer = 3
        Private _DAL As String = String.Empty

        Public Sub New()

        End Sub

        Public Sub New(ByVal drowDirectory As DataRow)
            On Error Resume Next    ' ignore db null
            _Business_Unit_Seq_ID = CInt(drowDirectory("Business_Unit_Seq_ID"))
            _Name = CStr(drowDirectory("Name"))
            _Description = CStr(drowDirectory("Description"))
            _Skin = CStr(drowDirectory("Skin"))
            _Parent_Business_Unit_Seq_ID = CInt(drowDirectory("PARENT_BUSINESS_UNIT_SEQ_ID"))
            _ConnectionString = CStr(drowDirectory("CONNECTION_STRING"))
            _STATUS_SEQ_ID = CInt(drowDirectory("STATUS_SEQ_ID"))
        End Sub 'New

        Public Property BUSINESS_UNIT_SEQ_ID() As Integer
            Get
                Return _Business_Unit_Seq_ID
            End Get
            Set(ByVal Value As Integer)
                _Business_Unit_Seq_ID = Value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal Value As String)
                _Name = Value.Trim
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                _Description = Value.Trim
            End Set
        End Property

        Public Property Skin() As String
            Get
                Return _Skin
            End Get
            Set(ByVal Value As String)
                _Skin = Value.Trim
            End Set
        End Property

        Public Property Parent_Business_Unit_Seq_ID() As Integer
            Get
                Return _Parent_Business_Unit_Seq_ID
            End Get
            Set(ByVal Value As Integer)
                _Parent_Business_Unit_Seq_ID = Value
            End Set
        End Property

        Public Property ConnctionString() As String
            Get
                Return _ConnectionString
            End Get
            Set(ByVal Value As String)
                _ConnectionString = Value.Trim
            End Set
        End Property

        Public Property STATUS_SEQ_ID() As Integer
            Get
                Return _STATUS_SEQ_ID
            End Get
            Set(ByVal Value As Integer)
                _STATUS_SEQ_ID = Value
            End Set
        End Property

        Public Property DAL() As String
            Get
                Return _DAL
            End Get
            Set(ByVal Value As String)
                _DAL = Value
            End Set
        End Property
    End Class
End Namespace