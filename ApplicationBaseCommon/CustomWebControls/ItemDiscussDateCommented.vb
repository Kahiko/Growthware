Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Discuss

Namespace CustomWebControls
    '*********************************************************************
    '
    ' ItemDiscussDateCommented Class
    '
    ' Represents the date that a post was last commented 
    ' displayed in a template
    '
    '*********************************************************************

    Public Class ItemDiscussDateCommented
        Inherits ItemDate

        Private _isPinned As Boolean = False

        '*********************************************************************
        '
        ' ItemDiscussDateCommented Constructor
        '
        ' Assign a default css class.
        '
        '*********************************************************************
        Public Sub New()
            CssClass = "itemDiscussDateCommented"
            EnableViewState = False
        End Sub 'New

        '*********************************************************************
        '
        ' Render Method
        '
        ' Don't render for pinned post.
        '
        '*********************************************************************
        Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
            If _isPinned = False Then
                MyBase.Render(writer)
            End If
        End Sub 'Render

        '*********************************************************************
        '
        ' AssignContentItem Method
        '
        ' Assigns the correct content item to the date.
        '
        '*********************************************************************
        Protected Overrides Sub AssignContentItem(ByVal contentInfo As ContentInfo)
            [Date] = contentInfo.DateCommented
            _isPinned = CType(contentInfo, PostInfo).IsPinned
        End Sub 'AssignContentItem
    End Class 'ItemDiscussDateCommented 
End Namespace