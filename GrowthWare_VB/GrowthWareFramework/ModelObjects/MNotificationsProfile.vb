Imports GrowthWare.Framework.Enumerations
Imports GrowthWare.Framework.ModelObjects.Base

Namespace ModelObjects
    <Serializable(), CLSCompliant(True)> _
    Public Class MNotificationsProfile

#Region "Member Properties"
        Private m_SE_SEQ_ID As Integer = -1
        Private m_Status As Integer = 1
        Private m_Account As String = String.Empty
        Private m_Function_Seq_ID As Integer = -1
#End Region

#Region "Public Properties"
        Public Property SecurityEntityID() As Integer
            Get
                Return m_SE_SEQ_ID
            End Get
            Set(ByVal value As Integer)
                m_SE_SEQ_ID = value
            End Set
        End Property

        Public Property Status() As Boolean
            Get
                Return m_Status
            End Get
            Set(ByVal value As Boolean)
                m_Status = value
            End Set
        End Property

        Public Property Account() As String
            Get
                Return m_Account
            End Get
            Set(ByVal value As String)
                m_Account = value.Trim
            End Set
        End Property

        Public Property FunctionID() As Integer
            Get
                Return m_Function_Seq_ID
            End Get
            Set(ByVal value As Integer)
                m_Function_Seq_ID = value
            End Set
        End Property
#End Region

#Region "Public Methods"
            ''' <summary>
            ''' Provides a new account profile with the default vaules
            ''' </summary>
            ''' <remarks></remarks>
        Public Sub New()

        End Sub
#End Region
    End Class
End Namespace