Imports System
Imports System.Data.SqlClient

Namespace Discuss
    '*********************************************************************
    '
    ' PostInfo Class
    '
    ' Represents all information about a particular post. 
    '
    '*********************************************************************
    Public Class PostInfo
        Inherits ContentInfo

        Private _bodyText As String = String.Empty
        Private _lastCommentUsername As String = String.Empty
        Private _isPinned As Boolean = False
        Private _isAnnouncement As Boolean = False
        Private _isLocked As Boolean = False

        '*********************************************************************
        '
        ' PostInfo Constructor
        '
        ' Initializes post information from a SqlDataReader. 
        '
        '*********************************************************************
        Public Sub New(ByVal dr As SqlDataReader)
            MyBase.New(dr)
            _isPinned = CBool(dr("Discuss_IsPinned"))
            _isAnnouncement = CBool(dr("Discuss_IsAnnouncement"))
            _isLocked = CBool(dr("Discuss_IsLocked"))

            If Not IsDBNull(dr("LastCommentUsername")) Then
                _lastCommentUsername = CStr(dr("LastCommentUsername"))
            End If
            If Not IsDBNull(dr("Discuss_BodyText")) Then
                _bodyText = CStr(dr("Discuss_BodyText"))
            End If
        End Sub 'New 

        '*********************************************************************
        '
        ' IsPinned Property
        '
        ' Pinned posts always appear at top.
        '
        '*********************************************************************
        Public Property IsPinned() As Boolean
            Get
                Return _isPinned
            End Get
            Set(ByVal value As Boolean)
                _isPinned = value
            End Set
        End Property

        '*********************************************************************
        '
        ' IsAnnouncement Property
        '
        ' Announcements appear with a !.
        '
        '*********************************************************************
        Public Property IsAnnouncement() As Boolean
            Get
                Return _isAnnouncement
            End Get
            Set(ByVal value As Boolean)
                _isAnnouncement = value
            End Set
        End Property

        '*********************************************************************
        '
        ' IsLocked Property
        '
        ' Locked posts cannot have replies.
        '
        '*********************************************************************
        Public Property IsLocked() As Boolean
            Get
                Return _isLocked
            End Get
            Set(ByVal value As Boolean)
                _isLocked = value
            End Set
        End Property

        '*********************************************************************
        '
        ' LastCommentUsername Property
        '
        ' Represents the username of the last person to add a comment.
        '
        '*********************************************************************
        Public Property LastCommentUsername() As String
            Get
                Return _lastCommentUsername
            End Get
            Set(ByVal value As String)
                _lastCommentUsername = value
            End Set
        End Property

        '*********************************************************************
        '
        ' BodyText Property
        '
        ' Represents the content of an post.
        '
        '*********************************************************************
        Public Property BodyText() As String
            Get
                Return _bodyText
            End Get
            Set(ByVal value As String)
                _bodyText = value
            End Set
        End Property
    End Class 'PostInfo
End Namespace