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
    ' Title Class
    '
    ' Represents a page title
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class Title
        Inherits WebControl


        Private _text As String = String.Empty



        '*********************************************************************
        '
        ' Title Constructor
        '
        ' Get the title from the PageInfo object
        '
        '********************************************************************
        Public Sub New()
            ' assign default css class
            CssClass = "title"

            ' Get PageInfo object
            If Not (Context Is Nothing) Then
                'Dim objPageInfo As PageInfo = CType(Context.Items("PageInfo"), PageInfo)
                '_text = objPageInfo.Title

                Dim objModuleProfileInfo As MModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)
                _text = objModuleProfileInfo.Name
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
        ' Display title. Notice that we are HTML Encoding
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            ' we need the section for the transformations
            'Dim objSectionInfo As SectionInfo = CType(Context.Items("SectionInfo"), SectionInfo)

            ' display the content
            'writer.Write(CommunityGlobals.FormatText(AllowHtml.None, objSectionInfo.ID, _text))

            Dim objModuleProfileInfo As MModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)

            writer.Write(BaseHelpers.FormatText(AllowHtml.None, objModuleProfileInfo.MODULE_SEQ_ID, CStr(ViewState("Title"))))

        End Sub 'RenderContents
    End Class 'Title 

End Namespace