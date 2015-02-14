Imports GrowthWare.Framework.Model.Profiles.Base

Namespace Model.Profiles
    ''' <summary>
    ''' Class MDBInformation.
    ''' </summary>
    <Serializable(), CLSCompliant(True)> _
    Public Class MDBInformation
        Inherits MProfile

#Region "Constructors"
        ''' <summary>
        ''' Provides a new account profile with the default vaules
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="MDBInformation"/> class.
        ''' </summary>
        ''' <param name="dataRow">The data row.</param>
        Public Sub New(ByVal dataRow As DataRow)
            If Not dataRow Is Nothing Then
                Me.Initialize(dataRow)
            End If
        End Sub
#End Region

#Region "Member Properties"
        Protected mInformationSeqId As Integer = 1
        Protected mVersion As String = String.Empty
        Protected mEnableInheritance As Integer = 1
#End Region


#Region "Public Properties"
        Public Property InformationSeqId() As Integer
            Get
                Return mInformationSeqId
            End Get
            Set(ByVal value As Integer)
                mInformationSeqId = value
            End Set
        End Property

        Public Property Version() As String
            Get
                Return mVersion.Trim
            End Get
            Set(ByVal value As String)
                mVersion = value.Trim
            End Set
        End Property

        Public Property EnableInheritance() As Integer
            Get
                Return mEnableInheritance
            End Get
            Set(ByVal value As Integer)
                mEnableInheritance = value
            End Set
        End Property
#End Region

#Region "Protected Methods"
        Protected Shadows Sub Initialize(ByVal dataRow As DataRow)
            MyBase.IdColumnName = "Information_SEQ_ID"
            MyBase.NameColumnName = "VERSION"
            MyBase.Initialize(dataRow)
            mInformationSeqId = MyBase.GetInt(dataRow, "Information_SEQ_ID")
            mVersion = MyBase.GetString(dataRow, "VERSION")
            mEnableInheritance = MyBase.GetInt(dataRow, "ENABLE_INHERITANCE")
        End Sub

#End Region
    End Class
End Namespace
