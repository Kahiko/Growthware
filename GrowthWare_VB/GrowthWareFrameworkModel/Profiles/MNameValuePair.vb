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
    Public Class MNameValuePair
		Inherits MProfile

#Region "Member Properties"
        Private mStaticName As String = "NEW"
        Private mDisplay As String = String.Empty
        Private mDescription As String = String.Empty
        Private mStatus As Integer = -1
#End Region

#Region "Protected Methods"
        Public Shadows Sub Init(ByVal dr As DataRow)
            On Error Resume Next
            MyBase.Init(dr)
            MyBase.m_ID = CInt(dr("NVP_SEQ_ID"))
            mStaticName = CStr(dr("STATIC_NAME"))
            MyBase.m_Name = mStaticName
            mDisplay = CStr(dr("DISPLAY"))
            mDescription = CStr(dr("DESCRIPTION"))
            mStatus = CInt(dr("STATUS_SEQ_ID"))
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
        Public Property Status() As Integer
            Get
                Return mStatus
            End Get
            Set(ByVal value As Integer)
                mStatus = value
            End Set
        End Property

        Public Property StaticName() As String
            Get
                Return mStaticName
            End Get
            Set(ByVal Value As String)
                mStaticName = Value.Trim
            End Set
        End Property

        Public Property Display() As String
            Get
                Return mDisplay
            End Get
            Set(ByVal Value As String)
                mDisplay = Value.Trim
            End Set
        End Property

        Public Property Description() As String
            Get
                Return mDescription
            End Get
            Set(ByVal Value As String)
                mDescription = Value.Trim
            End Set
        End Property
#End Region
    End Class
End Namespace