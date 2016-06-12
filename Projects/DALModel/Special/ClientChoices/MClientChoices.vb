Namespace Special.ClientChoices
    '*********************************************************************
    '
    ' BaseMClientChoices Class
    '
    ' Use this class to limit any code changes that would be
    ' necessary should the database table structure change.
    ' Each property return the name of the column in the database
    ' allowing developers to use names and to facilliate 
    ' database column changes.
    '
    '*********************************************************************

    Public Class MClientChoices
        Public Shared ReadOnly Property RecordsPerPage() As String
            Get
				Return "RECORDS_PER_PAGE"
            End Get
        End Property

        Public Shared ReadOnly Property AnonymousClientChoicesState() As String
            Get
                Return "AnonymousClientChoicesState"
            End Get
        End Property

        Public Shared ReadOnly Property sessionName() As String
            Get
                Return "ClientChoicesState"
            End Get
        End Property

        Public Shared ReadOnly Property AccountName() As String
            Get
                Return "ACCOUNT"
            End Get
        End Property

        Public Shared ReadOnly Property BusinessUnitID() As String
            Get
                Return "BUSINESS_UNIT_SEQ_ID"
            End Get
        End Property

        Public Shared ReadOnly Property BusinessUnitName() As String
            Get
                Return "BUSINESS_UNIT_NAME"
            End Get
        End Property

        Public Shared ReadOnly Property BackColor() As String
            Get
                Return "BACK_COLOR"
            End Get
        End Property

        Public Shared ReadOnly Property LeftColor() As String
            Get
                Return "LEFT_COLOR"
            End Get
        End Property

        Public Shared ReadOnly Property HeadColor() As String
            Get
                Return "HEAD_COLOR"
            End Get
        End Property

        Public Shared ReadOnly Property SubheadColor() As String
            Get
                Return "SUB_HEAD_COLOR"
            End Get
        End Property

        Public Shared ReadOnly Property ColorScheme() As String
            Get
                Return "COLOR_SCHEME"
            End Get
        End Property

        Public Shared ReadOnly Property Action() As String
            Get
                Return "MODULE_ACTION"
            End Get
        End Property
    End Class
End Namespace