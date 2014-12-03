Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.Framework.Model.Profiles.Interfaces

Namespace Model.Profiles
    <Serializable(), CLSCompliant(True)> _
    Public Class MFunctionTypeProfile
        Inherits MProfile
        Implements IMProfile

#Region "Member Properties"
        Private m_FunctionTypeSeqId = -1
        Private m_Description As String = String.Empty
        Private m_Template As String = String.Empty
        Private m_IsContent As Boolean
#End Region

        ''' <summary>
        ''' Will return a Function profile with the default vaules
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Will return a fully populated Function type profile.
        ''' </summary>
        ''' <param name="dataRow">A data row containing the Function type information</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal dataRow As DataRow)
            Initialize(dataRow)
            MyBase.Initialize(dataRow)
        End Sub

        Protected Shadows Sub Initialize(ByVal detailRow As DataRow)
            MyBase.IdColumnName = "Function_Type_Seq_ID"
            MyBase.NameColumnName = "NAME"
            If Not detailRow Is Nothing Then
                MyBase.Initialize(detailRow)
                m_FunctionTypeSeqId = Id
                m_Description = CStr(detailRow("DESCRIPTION"))
                m_Template = CStr(detailRow("TEMPLATE"))
                m_IsContent = CStr(detailRow("IS_CONTENT"))
            End If
        End Sub

        Public Property FunctionTypeSeqId() As Integer
            Get
                Return m_FunctionTypeSeqId
            End Get
            Set(ByVal value As Integer)
                m_FunctionTypeSeqId = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then m_Description = Value.Trim()
            End Set
        End Property

        Public Property Template() As String
            Get
                Return m_Template
            End Get
            Set(ByVal value As String)
                If Not String.IsNullOrEmpty(value) Then m_Template = value.Trim()
            End Set
        End Property

        Public Property IsContent() As Boolean
            Get
                Return m_IsContent
            End Get
            Set(ByVal value As Boolean)
                m_IsContent = value
            End Set
        End Property
    End Class
End Namespace