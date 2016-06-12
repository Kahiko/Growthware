Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Model.Discuss

Namespace CustomWebControls
    '*********************************************************************
    '
    ' ItemDiscussPinnedPost Class
    '
    ' Displays a message when a post is pinned
    '
    '*********************************************************************

    Public Class ItemDiscussPinnedPost
        Inherits Label

        Private _text As String = "Pinned Post"

        '*********************************************************************
        '
        ' ItemDiscussPinnedPost Constructor
        '
        ' Set the default css class
        '
        '*********************************************************************
        Public Sub New()
            CssClass = "itemDiscussPinnedPost"
            Font.Bold = True
            EnableViewState = False
        End Sub 'New

        '*********************************************************************
        '
        ' OnDataBinding Method
        '
        ' Grab the author from the container's DataItem property
        '
        '*********************************************************************
        Protected Overrides Sub OnDataBinding(ByVal e As EventArgs)
            Dim item As ContentItem

            If TypeOf NamingContainer Is ContentItem Then
                item = CType(NamingContainer, ContentItem)
            Else
                item = CType(NamingContainer.NamingContainer, ContentItem)
            End If

            Dim objPostInfo As PostInfo = CType(item.DataItem, PostInfo)
            ViewState("PinnedPost") = objPostInfo.IsPinned
        End Sub 'OnDataBinding

        '*********************************************************************
        '
        ' RenderContents method
        '
        ' Display the text when post is pinned
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            If CBool(ViewState("PinnedPost")) Then
                writer.Write(_text)
            End If
        End Sub 'RenderContents
    End Class 'ItemDiscussPinnedPost 
End Namespace