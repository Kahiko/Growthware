Imports GrowthWare.Framework.ModelObjects.Base.Interfaces
Imports GrowthWare.Framework.ModelObjects.Base
Imports System.Reflection

Namespace ModelObjects
    <Serializable(), CLSCompliant(True)> _
    Public Class MMessageProfile
		Inherits MFormatter
        Implements IProfile, IMessageProfile

#Region "Member Properties"
		Private m_SE_SEQ_ID As Integer = 1
		Private m_Description As String = String.Empty
		Private m_ErrorCode As Integer = -1
		Private m_Title As String = String.Empty
		Private m_FormatAsHTML As Boolean = False
#End Region

#Region "Protected Methods"
		Private Overloads Sub Init(ByVal dr As DataRow)
			On Error Resume Next
			MyBase.Init(dr)
			m_ID = CInt(dr("MESSAGE_SEQ_ID"))
			m_SE_SEQ_ID = CInt(dr("SE_SEQ_ID"))
			m_Title = CStr(dr("TITLE")).Trim
			m_Description = CStr(dr("DESCRIPTION")).Trim
			m_FormatAsHTML = CBool(dr("FORMAT_AS_HTML"))
			m_Body = CStr(dr("BODY"))
		End Sub
#End Region

#Region "Public Methods"
		''' <summary>
		''' Will return a message profile with the default vaules
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()

		End Sub

		Public Sub New(ByVal dr As DataRow)
			Init(dr)
		End Sub

		Public Overridable Sub FormatBody() Implements IMessageProfile.FormatBody
			Dim myPropertyInfo As PropertyInfo() = Me.GetType.GetProperties()
			Dim myPropertyItem As PropertyInfo
			For Each myPropertyItem In myPropertyInfo
				Dim pValue As Object = myPropertyItem.GetValue(Me, Nothing)
				m_Body = m_Body.Replace("<" & myPropertyItem.Name & ">", pValue.ToString)
			Next
		End Sub
#End Region

#Region "Public Properties"
		Public Property SE_SEQ_ID() As Integer
			Get
				Return m_SE_SEQ_ID
			End Get
			Set(ByVal value As Integer)
				m_SE_SEQ_ID = value
			End Set
		End Property

		Public Property Title() As String Implements IMessageProfile.Title
			Get
				Return m_Title
			End Get
			Set(ByVal value As String)
				m_Title = value.Trim
			End Set
		End Property

		Public Property Description() As String
			Get
				Return m_Description
			End Get
			Set(ByVal value As String)
				m_Description = value.Trim
			End Set
		End Property

		Public Property FormatAsHTML() As Boolean Implements IMessageProfile.FormatAsHTML
			Get
				Return m_FormatAsHTML
			End Get
			Set(ByVal value As Boolean)
				m_FormatAsHTML = value
			End Set
		End Property

		Public Property BODY() As String Implements IMessageProfile.Body
			Get
				Return m_Body
			End Get
			Set(ByVal value As String)
				m_Body = value
			End Set
		End Property
#End Region
    End Class
End Namespace