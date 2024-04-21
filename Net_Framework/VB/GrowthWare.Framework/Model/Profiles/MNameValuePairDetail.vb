Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.Framework.Model.Profiles.Interfaces

Namespace Model.Profiles
    ''' <summary>
    ''' Base properties an account Profile you can inherit this class and add
    ''' any properties or methods that suit your project needs.
    ''' </summary>
    ''' <remarks>
    ''' Corresponds to table [ZGWSystem].[Name_Value_Pairs] and the table created to store the
    ''' detail information example [ZGWSystem].[Statuses]
    ''' </remarks>
    <Serializable(), CLSCompliant(True)> _
    Public Class MNameValuePairDetail
        Inherits MProfile

#Region "Member Properties"
        Private m_NameValuePairSeqId As Integer = -1
        Private m_Text As String = String.Empty
        Private m_Value As String = String.Empty
        Private m_SortOrder As Integer = 0
        Private m_Status As Integer = 1
#End Region

#Region "Protected Methods"
        Public Shadows Sub Init(ByVal dataRow As DataRow)
            MyBase.IdColumnName = "NVP_SEQ_DET_ID"
            MyBase.NameColumnName = "NVP_DET_TEXT"
            MyBase.Initialize(dataRow)
            m_NameValuePairSeqId = MyBase.GetInt(dataRow, "NVP_SEQ_ID")
            m_Text = MyBase.Name
            m_Value = MyBase.GetString(dataRow, "NVP_DET_VALUE")
            m_Status = MyBase.GetInt(dataRow, "STATUS_SEQ_ID")
            m_SortOrder = MyBase.GetInt(dataRow, "SORT_ORDER")
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
            Init(dataRow)
        End Sub
#End Region

#Region "Public Properties"
        ''' <summary>
        ''' Gets or sets the Name value pair sequence identifier.
        ''' </summary>
        ''' <value>The Name value pair sequence identifier.</value>
        Public Property NameValuePairSeqId() As Integer
            Get
                Return m_NameValuePairSeqId
            End Get
            Set(ByVal value As Integer)
                m_NameValuePairSeqId = value
            End Set
        End Property

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
        ''' Gets or sets the sort order.
        ''' </summary>
        ''' <value>The sort order.</value>
        ''' <remarks>Default value is 0</remarks>
        Public Property SortOrder() As Integer
            Get
                Return m_SortOrder
            End Get
            Set(ByVal value As Integer)
                m_SortOrder = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the text.
        ''' </summary>
        ''' <value>The text.</value>
        Public Property Text() As String
            Get
                Return m_Text
            End Get
            Set(ByVal Value As String)
                If Not Value Is Nothing Then m_Text = Value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the value.
        ''' </summary>
        ''' <value>The value.</value>
        Public Property Value() As String
            Get
                Return m_Value
            End Get
            Set(ByVal Value As String)
                If Not Value Is Nothing Then m_Value = Value.Trim
            End Set
        End Property
#End Region
    End Class
End Namespace
