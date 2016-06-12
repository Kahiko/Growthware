Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Common.Globals

Namespace CustomWebControls
    '*********************************************************************
    '
    ' ItemCommentText Class
    '
    ' Represents the body text of a comment
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class ItemCommentText
        Inherits WebControl

        Public Sub New()
            CssClass = "itemCommentText"
            EnableViewState = False
        End Sub 'New

        Protected Overrides Sub OnDataBinding(ByVal e As EventArgs)
            Dim item As ContentItem

            If TypeOf NamingContainer Is ContentItem Then
                item = CType(NamingContainer, ContentItem)
            Else
                item = CType(NamingContainer.NamingContainer, ContentItem)
            End If

            Dim objCommentInfo As CommentInfo = CType(item.DataItem, CommentInfo)
            ViewState("Text") = objCommentInfo.Text
        End Sub 'OnDataBinding



        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            ' we need the section for the transformations
            'Dim objSectionInfo As SectionInfo = CType(Context.Items("SectionInfo"), SectionInfo)
            Dim objModuleInfo As MModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)
            ' display the content
            writer.Write(BaseHelpers.FormatText(AllowHtml.Full, objModuleInfo.MODULE_SEQ_ID, CStr(ViewState("Text"))))
        End Sub 'RenderContents
    End Class 'ItemCommentText 
End Namespace
