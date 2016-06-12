Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Model

Namespace CustomWebControls
    '*********************************************************************
    '
    ' ItemCommentCount Class
    '
    ' Represents the number of comments 
    '
    '*********************************************************************

    Public Class ItemCommentCount
        Inherits WebControl

        Private _formatText As String = "{0} comments"
        '*********************************************************************
        '
        ' ItemCommentCount Constructor
        '
        ' Assign a default cascading style class
        '
        '*********************************************************************
        Public Sub New()
            CssClass = "itemCommentCount"
            EnableViewState = False
        End Sub 'New


        '*********************************************************************
        '
        ' FormatText Property
        '
        ' Determines the formatting of the comment
        '
        '*********************************************************************

        Public Property FormatText() As String
            Get
                Return _formatText
            End Get
            Set(ByVal value As String)
                _formatText = value
            End Set
        End Property



        '*********************************************************************
        '
        ' OnDataBinding Method
        '
        ' Get the comment info from context
        '
        '*********************************************************************
        Protected Overrides Sub OnDataBinding(ByVal e As EventArgs)
            Dim item As ContentItem

            If TypeOf NamingContainer Is ContentItem Then
                item = CType(NamingContainer, ContentItem)
            Else
                item = CType(NamingContainer.NamingContainer, ContentItem)
            End If

            Dim objContentInfo As ContentInfo = CType(item.DataItem, ContentInfo)
            ViewState("CommentCount") = objContentInfo.CommentCount
        End Sub 'OnDataBinding




        '*********************************************************************
        '
        ' Render Method
        '
        ' Only render if comments are enabled
        '
        '*********************************************************************
        Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
            ' needs help
            'Dim objSectionInfo As SectionInfo = CType(HttpContext.Current.Items("SectionInfo"), SectionInfo)
            'If objSectionInfo.EnableComments Then
            '    MyBase.Render(writer)
            'End If
            MyBase.Render(writer)
        End Sub 'Render


        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Render the comment count
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)

            writer.Write(String.Format(_formatText, ViewState("CommentCount")))
        End Sub 'RenderContents
    End Class 'ItemCommentCount 
End Namespace
