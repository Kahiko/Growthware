Imports System
Imports System.Data.SqlClient

'*********************************************************************
'
' CommentInfo Class
'
' Represents all information about a particular comment. 
'
'*********************************************************************
Public Class CommentInfo
    Inherits ContentInfo

    Private _text As String
    Private _replyID As Integer
    Private _parentID As Integer

    '*********************************************************************
    '
    ' CommentInfo Constructor
    '
    ' Initializes comment information. 
    '
    '*********************************************************************
    Public Sub New(ByVal dr As SqlDataReader)
        MyBase.New(dr)
        _text = CStr(dr("Comment_Text"))
        _parentID = Fix(dr("ContentPage_ParentID"))
        _replyID = Fix(dr("ContentPage_ReplyID"))
    End Sub 'New

    '*********************************************************************
    '
    ' Text Property
    '
    ' Represents the body text of the comment.
    '
    '*********************************************************************
    Public Property [Text]() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property

    '*********************************************************************
    '
    ' ReplyID Property
    '
    ' Represents the immediate parent of the comment.
    '
    '*********************************************************************
    Public Property ReplyID() As Integer
        Get
            Return _replyID
        End Get
        Set(ByVal value As Integer)
            _replyID = value
        End Set
    End Property

    Public Property ParentID() As Integer
        Get
            Return _parentID
        End Get
        Set(ByVal value As Integer)
            _parentID = value
        End Set
    End Property
End Class 'CommentInfo 