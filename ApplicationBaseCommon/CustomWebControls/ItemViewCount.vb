Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Diagnostics
Imports ApplicationBase.Model

Namespace CustomWebControls
    '*********************************************************************
    '
    ' ItemViewCount Class
    '
    ' Represents a view count displayed in a template
    '
    '*********************************************************************

    Public Class ItemViewCount
        Inherits WebControl



        Public Sub New()
            CssClass = "itemViewCount"
            EnableViewState = False
        End Sub 'New


        Protected Overrides Sub OnDataBinding(ByVal e As EventArgs)
            Dim item As ContentItem

            If TypeOf NamingContainer Is ContentItem Then
                item = CType(NamingContainer, ContentItem)
            Else
                item = CType(NamingContainer.NamingContainer, ContentItem)
            End If

            Dim objContentInfo As ContentInfo = CType(item.DataItem, ContentInfo)
            ViewState("ViewCount") = objContentInfo.ViewCount
        End Sub 'OnDataBinding



        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            writer.Write(ViewState("ViewCount"))
        End Sub 'RenderContents
    End Class 'ItemViewCount 
End Namespace