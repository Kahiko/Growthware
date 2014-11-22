Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.Framework.Model.Profiles.Interfaces
Imports System.Reflection

Namespace Model.Profiles
    <Serializable(), CLSCompliant(True)> _
    Public Class MMessageProfile
        Inherits MProfile
        Implements IMessageProfile

#Region "Member Properties"
        Private m_SecurityEntity_Seq_Id As Integer = 1
        Private m_Description As String = String.Empty
        'Private m_ErrorCode As Integer = -1
        Private m_Title As String = String.Empty
        Private m_FormatAsHTML As Boolean = False
        Private m_Body As String = String.Empty
#End Region

#Region "Protected Methods"
        ''' <summary>
        ''' Initializes the specified data row.
        ''' </summary>
        ''' <param name="dataRow">The data row.</param>
        Private Shadows Sub Initialize(ByVal dataRow As DataRow)
            MyBase.IdColumnName = "MESSAGE_SEQ_ID"
            MyBase.NameColumnName = "Name"
            MyBase.Initialize(dataRow)
            m_SecurityEntity_Seq_Id = MyBase.GetInt(dataRow, "SE_SEQ_ID")
            m_Title = MyBase.GetString(dataRow, "TITLE")
            m_Description = MyBase.GetString(dataRow, "DESCRIPTION")
            m_FormatAsHTML = MyBase.GetBool(dataRow, "FORMAT_AS_HTML")
            m_Body = MyBase.GetString(dataRow, "BODY")
        End Sub
#End Region

#Region "Public Methods"
        ''' <summary>
        ''' Will return a message profile with the default vaules
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="MMessageProfile" /> class.
        ''' </summary>
        ''' <param name="dataRow">The data row.</param>
        Public Sub New(ByVal dataRow As DataRow)
            initialize(dataRow)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="MMessageProfile" /> class.
        ''' </summary>
        ''' <param name="profile">MMessageProfile</param>
        Public Sub New(ByVal profile As MMessageProfile)
            If profile IsNot Nothing Then
                With Me
                    .AddedBy = profile.AddedBy
                    .AddedDate = profile.AddedDate
                    .Body = profile.Body
                    .Description = profile.Description
                    .FormatAsHtml = profile.FormatAsHtml
                    .Id = profile.Id
                    .Name = profile.Name
                    .SecurityEntitySeqId = profile.SecurityEntitySeqId
                    .Title = profile.Title
                    .UpdatedBy = profile.UpdatedBy
                    .UpdatedDate = profile.UpdatedDate
                End With
            End If
        End Sub

        ''' <summary>
        ''' Formats the body and replaces < and > with the appropriate property value.
        ''' </summary>
        Public Overridable Sub FormatBody() Implements IMessageProfile.FormatBody
            Dim myPropertyInfo As PropertyInfo() = Me.GetType.GetProperties()
            Dim myPropertyItem As PropertyInfo
            For Each myPropertyItem In myPropertyInfo
                Dim pValue As Object = myPropertyItem.GetValue(Me, Nothing)
                If pValue IsNot Nothing Then
                    m_Body = m_Body.Replace("<" & myPropertyItem.Name & ">", pValue.ToString)
                Else
                    m_Body = m_Body.Replace("<" & myPropertyItem.Name & ">", "")
                End If
            Next
        End Sub

        ''' <summary>
        ''' Returns all properties encapsulated by angle brackets seporated by the Seporator parameter
        ''' </summary>
        ''' <param name="separator">string</param>
        ''' <returns>string</returns>
        Public Function GetTags(ByVal separator As String) As String Implements IMessageProfile.GetTags
            Dim mRetVal As String = String.Empty
            Dim mPropertyInfo As PropertyInfo() = Me.GetType.GetProperties()
            For Each mPropertyItem As PropertyInfo In mPropertyInfo
                mRetVal = mRetVal & "<" & mPropertyItem.Name & ">" & separator
            Next
            Return mRetVal
        End Function
#End Region

#Region "Public Properties"
        ''' <summary>
        ''' Sets or gets the SecurityEntitySeqId property
        ''' </summary>
        ''' <value>Sets the value</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        Public Property SecurityEntitySeqId() As Integer
            Get
                Return m_SecurityEntity_Seq_Id
            End Get
            Set(ByVal value As Integer)
                m_SecurityEntity_Seq_Id = value
            End Set
        End Property

        ''' <summary>
        ''' Sets or gets the Title property
        ''' </summary>
        ''' <value>Sets the value</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        Public Property Title() As String Implements IMessageProfile.Title
            Get
                Return m_Title
            End Get
            Set(ByVal value As String)
                If Not value Is Nothing Then m_Title = value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Sets or gets the Description property
        ''' </summary>
        ''' <value>Sets the value</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal value As String)
                If Not value Is Nothing Then m_Description = value.Trim()
            End Set
        End Property

        ''' <summary>
        ''' Sets or gets the FormatAsHtml property
        ''' </summary>
        ''' <value>Sets the value</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        Public Property FormatAsHtml() As Boolean Implements IMessageProfile.FormatAsHtml
            Get
                Return m_FormatAsHTML
            End Get
            Set(ByVal value As Boolean)
                m_FormatAsHTML = value
            End Set
        End Property

        ''' <summary>
        ''' Sets or gets the body property
        ''' </summary>
        ''' <value>Sets the value</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        Public Property Body() As String Implements IMessageProfile.Body
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
