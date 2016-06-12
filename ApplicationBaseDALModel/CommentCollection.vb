Imports System
Imports System.Collections

'*********************************************************************
'
' CommentCollection Class
'
' This class represents a collection of comments.
' It inherits from an ArrayList and uses a Hashtable internally. 
'
'*********************************************************************
Public Class CommentCollection
    Inherits ArrayList

    Private hashComments As New Hashtable()

    '*********************************************************************
    '
    ' CommentCollection Constructor
    '
    ' Initializes a new instance of the CommentCollection class. 
    '
    '*********************************************************************
    Public Sub New()
    End Sub 'New 

    '*********************************************************************
    '
    ' this indexer
    '
    ' Adds a new comment to the ArrayList and Hashtable. 
    '
    '*********************************************************************

    Default Public Overrides Property Item(ByVal index As Integer) As Object
        Get
            Return CType(MyBase.Item(index), CommentInfo)
        End Get
        Set(ByVal value As Object)
            hashComments(CType(value, CommentInfo).ContentPageID) = value
            MyBase.Item(index) = value
        End Set
    End Property

    '*********************************************************************
    '
    ' Add Method
    '
    ' Adds a new comment to the ArrayList and Hashtable. 
    '
    '*********************************************************************
    Public Overrides Function Add(ByVal value As Object) As Integer
        hashComments(CType(value, CommentInfo).ContentPageID) = value
        Return MyBase.Add(value)
    End Function 'Add

    '*********************************************************************
    '
    ' GetChildren Method
    '
    ' Gets all the immediate child comments by iterating through
    ' the ArrayList. 
    '
    '*********************************************************************
    Public Function GetChildren(ByVal replyID As Integer) As CommentCollection
        Dim colChildren As New CommentCollection()

        Dim e As IEnumerator = Me.GetEnumerator()
        While e.MoveNext()
            If CType(e.Current, CommentInfo).ReplyID = replyID Then
                colChildren.Add(e.Current)
            End If
        End While
        Return colChildren
    End Function 'GetChildren

    '*********************************************************************
    '
    ' GetReplyLevel Method
    '
    ' Gets the nesting level of a comment. 
    '
    '*********************************************************************
    Public Function GetReplyLevel(ByVal comment As CommentInfo) As Integer
        Dim level As Integer = -1

        While Not (comment Is Nothing)
            level += 1
            comment = CType(hashComments(comment.ReplyID), CommentInfo)

            If level > 50 Then
                Exit While
            End If
        End While
        Return level
    End Function 'GetReplyLevel
End Class 'CommentCollection 