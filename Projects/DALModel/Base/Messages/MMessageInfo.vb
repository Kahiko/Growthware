Imports System
Namespace Base.Messages
    '*********************************************************************
    '
    ' MessageInfo Class
    '
    ' Represents an individual message. 
    '
    '*********************************************************************
    Public Class MMessageInfo
        Private _MESSAGE_SEQ_ID As Integer = 0
        Private _BUSINESS_UNIT_SEQ_ID As Integer = 0
        Private _NAME As String = String.Empty
        Private _TITLE As String = String.Empty
        Private _DESCRIPTION As String = String.Empty
        Private _BODY As String = String.Empty
        Private _account_Seq_id As Integer = 0



        Public Property MESSAGE_SEQ_ID() As Integer
            Get
                Return _MESSAGE_SEQ_ID
            End Get
            Set(ByVal Value As Integer)
                _MESSAGE_SEQ_ID = Value
            End Set
        End Property

        Public Property BUSINESS_UNIT_SEQ_ID() As Integer
            Get
                Return _BUSINESS_UNIT_SEQ_ID
            End Get
            Set(ByVal Value As Integer)
                _BUSINESS_UNIT_SEQ_ID = Value
            End Set
        End Property
        '*********************************************************************
        '
        ' Name Property
        ' Specifies the name of the message. The name is used
        ' to uniquely identify the message.
        '
        '*********************************************************************
        Public Property Name() As String
            Get
                Return _NAME
            End Get
            Set(ByVal Value As String)
                _NAME = Value.Trim
            End Set
        End Property

        '*********************************************************************
        '
        ' Description Property
        ' Specifies the message description. The description
        ' explains the purpose of the message.
        '
        '*********************************************************************
        Public Property Description() As String
            Get
                Return _DESCRIPTION
            End Get
            Set(ByVal Value As String)
                _DESCRIPTION = Value.Trim
            End Set
        End Property

        '*********************************************************************
        '
        ' Title Property
        ' Specifies the title of the message. The title
        ' appears in the browser title bar and as the subject line
        ' for email messages.
        '
        '*********************************************************************
        Public Property Title() As String
            Get
                Return _TITLE
            End Get
            Set(ByVal Value As String)
                _TITLE = Value.Trim
            End Set
        End Property

        '*********************************************************************
        '
        ' Body Property
        ' Specifies the body of the message. This is the text
        ' that appears in the body of the page or the body of
        ' an email message.
        '
        '*********************************************************************
        Public Property Body() As String
            Get
                Return _BODY
            End Get
            Set(ByVal Value As String)
                _BODY = Value.Trim
            End Set
        End Property


        Public Property Account_seq_id() As Integer
            Get
                Return _account_Seq_id
            End Get
            Set(ByVal Value As Integer)
                _account_Seq_id = Value
            End Set
        End Property
        '*********************************************************************
        '
        ' MessageInfo Constructor
        ' Initializes a new instance of the MessageInfo object.
        '
        '*********************************************************************
        Public Sub New()
        End Sub 'New
    End Class 'MessageInfo
End Namespace