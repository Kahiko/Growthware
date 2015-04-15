Imports GrowthWare.Framework.Model.Profiles.Base

Namespace Model.Profiles
    ''' <summary>
    ''' Class MDBInformation.
    ''' </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="MDB")>
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
        Private m_InformationSeqId As Integer = 1
        Private m_Version As String = String.Empty
        Private m_EnableInheritance As Integer = 1
#End Region


#Region "Public Properties"
        Public Property InformationSeqId() As Integer
            Get
                Return m_InformationSeqId
            End Get
            Set(ByVal value As Integer)
                m_InformationSeqId = value
            End Set
        End Property

        Public Property Version() As String
            Get
                Return m_Version.Trim
            End Get
            Set(ByVal value As String)
                If Not value Is Nothing Then m_Version = value.Trim
            End Set
        End Property

        Public Property EnableInheritance() As Integer
            Get
                Return m_EnableInheritance
            End Get
            Set(ByVal value As Integer)
                m_EnableInheritance = value
            End Set
        End Property
#End Region

#Region "Protected Methods"
        Protected Shadows Sub Initialize(ByVal dataRow As DataRow)
            MyBase.IdColumnName = "Information_SEQ_ID"
            MyBase.NameColumnName = "VERSION"
            MyBase.Initialize(dataRow)
            m_InformationSeqId = MyBase.GetInt(dataRow, "Information_SEQ_ID")
            m_Version = MyBase.GetString(dataRow, "VERSION")
            m_EnableInheritance = MyBase.GetInt(dataRow, "ENABLE_INHERITANCE")
        End Sub

#End Region
    End Class
End Namespace
