Namespace Model.Profiles
    ''' <summary>
    ''' Model object representing GroupRoles
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable(), CLSCompliant(True)>
    Public Class MGroupRoles

        Private m_AddedUpdatedBy As Integer
        Private m_SecurityEntityId As Integer = -1
        Private m_GroupSeqId As Integer = -1
        Private m_Roles As String

        ''' <summary>
        ''' Gets or sets the ADD_UP_BY.
        ''' </summary>
        ''' <value>Account added or updated by.</value>
        Public Property AddedUpdatedBy() As Integer
            Get
                Return m_AddedUpdatedBy
            End Get
            Set(ByVal value As Integer)
                m_AddedUpdatedBy = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the SE_SEQ_ID.
        ''' </summary>
        ''' <value>The Security Entity ID.</value>
        Public Property SecurityEntityId() As Integer
            Get
                Return m_SecurityEntityId
            End Get
            Set(ByVal value As Integer)
                m_SecurityEntityId = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the GROUP_SEQ_ID.
        ''' </summary>
        ''' <value>The Group Sequence ID.</value>
        Public Property GroupSeqId() As Integer
            Get
                Return m_GroupSeqId
            End Get
            Set(ByVal value As Integer)
                m_GroupSeqId = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the roles.
        ''' </summary>
        ''' <value>The roles.</value>
        Public Property Roles() As String
            Get
                Return m_Roles
            End Get
            Set(ByVal value As String)
                If Not String.IsNullOrEmpty(value) Then m_Roles = value.Trim
            End Set
        End Property
    End Class
End Namespace
