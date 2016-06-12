Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.Framework.Model.Profiles.Interfaces

Namespace Model.Profiles
    ''' <summary>
    ''' Base properties an account Profile you can inherit this class and add
    ''' any properties or methods that suit your project needs.
    ''' </summary>
    ''' <remarks>
    ''' Corresponds to table [ZGWSystem].[Name_Value_Pairs]
    ''' </remarks>
    <Serializable(), CLSCompliant(True)> _
    Public Class MNameValuePair
        Inherits MProfile

#Region "Member Properties"
        Private m_SchemaName As String = "dbo"
        Private m_StaticName As String = "new"
        Private m_Display As String = String.Empty
        Private m_Description As String = String.Empty
        Private m_Status As Integer = -1
#End Region

#Region "Protected Methods"
        Public Shadows Sub Initialize(ByVal dataRow As DataRow)
            MyBase.IdColumnName = "NVP_SEQ_ID"
            MyBase.NameColumnName = "STATIC_NAME"
            MyBase.Initialize(dataRow)
            m_StaticName = MyBase.Name
            SchemaName = MyBase.GetString(dataRow, "Schema_Name")
            m_Display = MyBase.GetString(dataRow, "DISPLAY")
            m_Description = MyBase.GetString(dataRow, "DESCRIPTION")
            m_Status = MyBase.GetInt(dataRow, "STATUS_SEQ_ID")
        End Sub
#End Region

#Region "Public Methods"
        ''' <summary>
        ''' Provides a new account profile with the default vaules
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Will populate values based on the contents of the data row.
        ''' </summary>
        ''' <param name="dataRow">Datarow containing base values</param>
        ''' <remarks>
        ''' Class should be inherited to extend to your project specific properties
        ''' </remarks>
        Public Sub New(ByVal dataRow As DataRow)
            Initialize(dataRow)
        End Sub
#End Region

#Region "Public Properties"
        ''' <summary>
        ''' Gets or sets the status.
        ''' </summary>
        ''' <value>The status.</value>
        Public Property Status() As Integer
            Get
                Return m_Status
            End Get
            Set(ByVal value As Integer)
                m_Status = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the name of the schema.
        ''' </summary>
        ''' <value>The name of the schema.</value>
        ''' <remarks>Default value is dbo</remarks>
        Public Property SchemaName() As String
            Get
                Return m_SchemaName
            End Get
            Set(ByVal Value As String)
                If Not Value Is Nothing Then m_SchemaName = Value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the name of the static.
        ''' </summary>
        ''' <value>The name of the static.</value>
        ''' <remarks>Default value is new</remarks>
        Public Property StaticName() As String
            Get
                Return m_StaticName
            End Get
            Set(ByVal Value As String)
                If Not Value Is Nothing Then m_StaticName = Value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the display.
        ''' </summary>
        ''' <value>The display.</value>
        Public Property Display() As String
            Get
                Return m_Display
            End Get
            Set(ByVal Value As String)
                If Not Value Is Nothing Then m_Display = Value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the description.
        ''' </summary>
        ''' <value>The description.</value>
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                If Not Value Is Nothing Then m_Description = Value.Trim
            End Set
        End Property
#End Region
    End Class
End Namespace