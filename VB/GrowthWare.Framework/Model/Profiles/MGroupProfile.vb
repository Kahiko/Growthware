Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.Framework.Model.Profiles.Interfaces

Namespace Model.Profiles
    ''' <summary>
    ''' Model object representing the GroupProfile
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable(), CLSCompliant(True)>
    Public Class MGroupProfile
        Inherits MProfile

#Region "Member Properties"
        Private m_Description As String = String.Empty
        Private m_SecurityEntityId As Integer = 1
#End Region

#Region "Protected Methods"
        ''' <summary>
        ''' Initializes the properties using the specified DataRow.
        ''' </summary>
        ''' <param name="dataRow">The dr.</param>
        Protected Overloads Sub Initialize(ByVal dataRow As DataRow)
            Me.IdColumnName = "GROUP_SEQ_ID"
            Me.NameColumnName = "NAME"
            MyBase.Initialize(dataRow)
            m_Description = MyBase.GetString(dataRow, "DESCRIPTION")
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
        ''' Initializes a new instance of the <see cref="MGroupProfile" /> class.
        ''' </summary>
        ''' <param name="dataRow">The DataRow</param>
        Public Sub New(ByVal dataRow As DataRow)
            Me.Initialize(dataRow)
        End Sub
#End Region

#Region "Public Properties"
        ''' <summary>
        ''' Gets or sets the security entity ID.
        ''' </summary>
        ''' <value>The security entity ID.</value>
        Public Property SecurityEntityId() As Integer
            Get
                Return m_SecurityEntityId
            End Get
            Set(ByVal value As Integer)
                m_SecurityEntityId = value
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
            Set(ByVal value As String)
                If Not String.IsNullOrEmpty(value) Then m_Description = value.Trim
            End Set
        End Property
#End Region
    End Class
End Namespace