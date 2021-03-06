﻿Namespace Model.Profiles
    ''' <summary>
    ''' All propertyies represent the column names in the ZF_ACCT_CHOICES table.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable(), CLSCompliant(True)> _
    Public NotInheritable Class MClientChoices

        ''' <summary>
        ''' Added to prevent the compiler from creating a default constructor.
        ''' Resolves CA 1053
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()

        End Sub

        ''' <summary>
        ''' Gets the records per page.
        ''' </summary>
        ''' <value>The records per page.</value>
        Public Shared ReadOnly Property RecordsPerPage() As String
            Get
                Return "RECORDS_PER_PAGE"
            End Get
        End Property

        ''' <summary>
        ''' Gets the state of the anonymous client choices.
        ''' </summary>
        ''' <value>The state of the anonymous client choices.</value>
        Public Shared ReadOnly Property AnonymousClientChoicesState() As String
            Get
                Return "AnonymousClientChoicesState"
            End Get
        End Property

        ''' <summary>
        ''' Gets the name of the session.
        ''' </summary>
        ''' <value>The name of the session.</value>
        Public Shared ReadOnly Property SessionName() As String
            Get
                Return "ClientChoicesState"
            End Get
        End Property

        ''' <summary>
        ''' Gets the name of the account.
        ''' </summary>
        ''' <value>The name of the account.</value>
        Public Shared ReadOnly Property AccountName() As String
            Get
                Return "ACCT"
            End Get
        End Property

        ''' <summary>
        ''' Gets the security entity ID.
        ''' </summary>
        ''' <value>The security entity ID.</value>
        Public Shared ReadOnly Property SecurityEntityId() As String
            Get
                Return "SE_SEQ_ID"
            End Get
        End Property

        ''' <summary>
        ''' Gets the name of the security entity.
        ''' </summary>
        ''' <value>The name of the security entity.</value>
        Public Shared ReadOnly Property SecurityEntityName() As String
            Get
                Return "SE_NAME"
            End Get
        End Property

        ''' <summary>
        ''' Gets the color of the back.
        ''' </summary>
        ''' <value>The color of the back.</value>
        Public Shared ReadOnly Property BackColor() As String
            Get
                Return "BACK_COLOR"
            End Get
        End Property

        ''' <summary>
        ''' Gets the color of the left.
        ''' </summary>
        ''' <value>The color of the left.</value>
        Public Shared ReadOnly Property LeftColor() As String
            Get
                Return "LEFT_COLOR"
            End Get
        End Property

        ''' <summary>
        ''' Gets the color of the head.
        ''' </summary>
        ''' <value>The color of the head.</value>
        Public Shared ReadOnly Property HeadColor() As String
            Get
                Return "HEAD_COLOR"
            End Get
        End Property

        ''' <summary>
        ''' Gets the color of the header fore.
        ''' </summary>
        ''' <value>The color of the header fore.</value>
        Public Shared ReadOnly Property HeaderForeColor() As String
            Get
                Return "Header_ForeColor"
            End Get
        End Property

        ''' <summary>
        ''' Gets the color of the subhead.
        ''' </summary>
        ''' <value>The color of the subhead.</value>
        Public Shared ReadOnly Property SubheadColor() As String
            Get
                Return "SUB_HEAD_COLOR"
            End Get
        End Property

        ''' <summary>
        ''' Gets the color of the row back.
        ''' </summary>
        ''' <value>The color of the row back.</value>
        Public Shared ReadOnly Property RowBackColor() As String
            Get
                Return "Row_BackColor"
            End Get
        End Property

        ''' <summary>
        ''' Gets the color of the alternating row back.
        ''' </summary>
        ''' <value>The color of the alternating row back.</value>
        Public Shared ReadOnly Property AlternatingRowBackColor As String
            Get
                Return "AlternatingRow_BackColor"
            End Get
        End Property

        ''' <summary>
        ''' Gets the color scheme.
        ''' </summary>
        ''' <value>The color scheme.</value>
        Public Shared ReadOnly Property ColorScheme() As String
            Get
                Return "COLOR_SCHEME"
            End Get
        End Property

        ''' <summary>
        ''' Gets the action.
        ''' </summary>
        ''' <value>The action.</value>
        Public Shared ReadOnly Property Action() As String
            Get
                Return "Favorite_Action"
            End Get
        End Property

    End Class
End Namespace

