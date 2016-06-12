Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Discuss
Imports ApplicationBase.Model.Modules

Namespace CustomWebControls
    '*********************************************************************
    '
    ' PostBodyText Class
    '
    ' Represents the body text of a post
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class PostBodyText
        Inherits WebControl


        Private _text As String


        Public Sub New()
            CssClass = "postBodyText"
            Style("margin") = "0px"

            ' Get ContentInfo object
            If Not (Context Is Nothing) Then
                Dim objPostInfo As [Object] = Context.Items("ContentInfo")
                If Not (objPostInfo Is Nothing) Then
                    _text = CType(objPostInfo, PostInfo).BodyText
                End If
            End If
        End Sub 'New 


        '*********************************************************************
        '
        ' Text Property
        '
        ' Allows the text to be assigned 
        ' This property is used in previewing on the add or edit page
        '
        '********************************************************************

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
        ' RenderContents Method
        '
        ' Display content by retrieving content from context
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            ' we need the section for the transformations
            'Dim objSectionInfo As SectionInfo = CType(Context.Items("SectionInfo"), SectionInfo)
            Dim objModuleProfileInfo As MModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)

            ' display the content
            'writer.Write(CommunityGlobals.FormatText(objSectionInfo.AllowHtmlInput, objSectionInfo.ID, _text))
            writer.Write(BaseHelpers.FormatText(AllowHtml.Full, objModuleProfileInfo.MODULE_SEQ_ID, _text))
        End Sub 'RenderContents
    End Class 'PostBodyText 
End Namespace
