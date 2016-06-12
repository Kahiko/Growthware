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
    ' ItemTitle Class
    '
    ' Represents a title displayed in a template
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class ItemTitle
        Inherits WebControl

        '*********************************************************************
        '
        ' ItemTitle Constructor
        '
        ' Assign a default css style (the user can override)
        '
        '*********************************************************************
        Public Sub New()
            CssClass = "itemTitle"
            EnableViewState = False
        End Sub 'New

        '*********************************************************************
        '
        ' OnDataBinding Method
        '
        ' Get the title from the container's DataItem property
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
            ViewState("Title") = objContentInfo.Title
        End Sub 'OnDataBinding

        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Display the title (No HTML allowed)
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            ' we need the section for the transformations
            Dim objModuleProfileInfo As MModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)
            'Dim objSectionInfo As SectionInfo = CType(Context.Items("ModuleProfileInfo"), SectionInfo)

            ' display the content
            'writer.Write(CommunityGlobals.FormatText(AllowHtml.None, objSectionInfo.ID, CStr(ViewState("Title"))))

            writer.Write(BaseHelpers.FormatText(AllowHtml.None, objModuleProfileInfo.MODULE_SEQ_ID, CStr(ViewState("Title"))))
        End Sub 'RenderContents 
    End Class 'ItemTitle
End Namespace