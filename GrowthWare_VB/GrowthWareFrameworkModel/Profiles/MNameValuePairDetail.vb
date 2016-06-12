Imports GrowthWare.Framework.Enumerations
Imports GrowthWare.Framework.ModelObjects.Base

Namespace ModelObjects
    ''' <summary>
    ''' Base properties a name value pair
    ''' </summary>
    ''' <remarks>
    ''' Corresponds to table ZFC_NVP and 
    ''' Store procedures: 
    ''' ZFP_SET_NVP, ZFP_GET_NVP
    ''' </remarks>
    <Serializable(), CLSCompliant(True)> _
    Public Class MNameValuePairDetail
		Inherits MProfile

#Region "Member Properties"
        Private m_NVPSeqID As Integer = -1
        Private m_Text As String = String.Empty
        Private m_Value As String = String.Empty
        Private m_SortOrder As Integer = 1
        Private m_Status As Integer = 1
#End Region

#Region "Protected Methods"
        Public Shadows Sub Init(ByVal dr As DataRow)
            On Error Resume Next
            MyBase.Init(dr)
            MyBase.m_ID = CInt(dr("NVP_SEQ_DET_ID"))
            m_NVPSeqID = CInt(dr("NVP_SEQ_ID"))
            m_Text = CStr(dr("NVP_DET_TEXT"))
            MyBase.m_Name = m_Text
            m_Value = CStr(dr("NVP_DET_VALUE"))
            m_Status = CInt(dr("STATUS_SEQ_ID"))
            m_SortOrder = CInt(dr("SORT_ORDER"))
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
        ''' <param name="dr">Datarow containing base values</param>
        ''' <remarks>
        ''' Class should be inherited to extend to your project specific properties
        ''' </remarks>
        Public Sub New(ByVal dr As DataRow)
            Init(dr)
        End Sub
#End Region

#Region "Public Properties"
        Public Property NVP_Seq_ID() As Integer
            Get
                Return m_NVPSeqID
            End Get
            Set(ByVal value As Integer)
                m_NVPSeqID = value
            End Set
        End Property

        Public Property Status() As Integer
            Get
                Return m_Status
            End Get
            Set(ByVal value As Integer)
                m_Status = value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return m_SortOrder
            End Get
            Set(ByVal value As Integer)
                m_SortOrder = value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return m_Text
            End Get
            Set(ByVal Value As String)
                m_Text = Value.Trim
            End Set
        End Property

        Public Property Value() As String
            Get
                Return m_Value
            End Get
            Set(ByVal Value As String)
                m_Value = Value.Trim
            End Set
        End Property
#End Region
    End Class
End Namespace