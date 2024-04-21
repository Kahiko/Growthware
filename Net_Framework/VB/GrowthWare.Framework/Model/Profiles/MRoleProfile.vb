Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.Framework.Model.Profiles.Interfaces

Namespace Model.Profiles
    ''' <summary>
    ''' Model object representing a Role.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable(), CLSCompliant(True)> _
    Public Class MRoleProfile
        Inherits MProfile

#Region "Member Properties"
        Private m_Description As String = String.Empty
        Private m_IsSystem As Boolean = False
        Private m_IsSystemOnly As Boolean = False
        Private m_SecurityEntitySeqId As Integer = 1
#End Region

#Region "Protected Methods"
        ''' <summary>
        ''' Initializes with the specified DataRow.
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <remarks></remarks>
        Protected Overloads Sub Initialize(ByVal dataRow As DataRow)
            MyBase.IdColumnName = "ROLE_SEQ_ID"
            MyBase.NameColumnName = "NAME"
            MyBase.Initialize(dataRow)
            m_Description = MyBase.GetString(dataRow, "DESCRIPTION")
            m_IsSystem = MyBase.GetBool(dataRow, "IS_SYSTEM")
            m_IsSystemOnly = MyBase.GetBool(dataRow, "IS_SYSTEM_ONLY")
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
        ''' Initializes a new instance of the <see cref="MRoleProfile" /> class.
        ''' </summary>
        ''' <param name="dataRow">The DataRow</param>
        Public Sub New(ByVal dataRow As DataRow)
            Initialize(dataRow)
        End Sub
#End Region

#Region "Public Properties"
        ''' <summary>
        ''' Gets or sets the security entity ID.
        ''' </summary>
        ''' <value>The security entity ID.</value>
        Public Property SecurityEntityId() As Integer
            Get
                Return m_SecurityEntitySeqId
            End Get
            Set(ByVal value As Integer)
                m_SecurityEntitySeqId = value
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
                If Not value Is Nothing Then m_Description = value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the is system.
        ''' </summary>
        ''' <value>The is system.</value>
        Public Property IsSystem() As Boolean
            Get
                Return Me.m_IsSystem
            End Get
            Set(value As Boolean)
                Me.m_IsSystem = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the is system only.
        ''' </summary>
        ''' <value>The is system only.</value>
        Public Property IsSystemOnly() As Boolean
            Get
                Return Me.m_IsSystemOnly
            End Get
            Set(value As Boolean)
                Me.m_IsSystemOnly = value
            End Set
        End Property
#End Region
    End Class

End Namespace
