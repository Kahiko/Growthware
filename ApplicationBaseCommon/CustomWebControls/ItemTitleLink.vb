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
    ' ItemTitleLink Class
    '
    ' Represents a title displayed in a template as a link
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class ItemTitleLink
        Inherits WebControl

        Private objModuleProfileInfo As MModuleProfileInfo



        '*********************************************************************
        '
        ' ItemTitle Constructor
        '
        ' Assign a default css style (the user can override)
        '
        '*********************************************************************
        Public Sub New()
            MyBase.New(HtmlTextWriterTag.A)
            CssClass = "itemTitleLink"
            EnableViewState = False

            If Not (Context Is Nothing) Then
                objModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)
            End If
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
            ViewState("ContentPageID") = objContentInfo.ContentPageID
            ViewState("ContentPageSectionID") = objContentInfo.SectionID
            ViewState("Title") = objContentInfo.Title
        End Sub 'OnDataBinding





        Protected Overrides Sub AddAttributesToRender(ByVal writer As HtmlTextWriter)
            Dim link As String

            Dim contentPageSectionID As Integer = Fix(ViewState("ContentPageSectionID"))

            ' if content in current section, just link
            If BaseSettings.defaultBusinessUnitID = contentPageSectionID Then
                link = String.Format("{0}.aspx", ViewState("ContentPageID"))
            Else
                'link = ContentPageUtility.CalculateContentPath(contentPageSectionID, Fix(ViewState("ContentPageID")))
                link = BaseSettings.FQDNPage & "?Action=discussiondetails&ID=" & Fix(ViewState("ContentPageID"))
            End If
            writer.AddAttribute(HtmlTextWriterAttribute.Href, link)
            MyBase.AddAttributesToRender(writer)
        End Sub 'AddAttributesToRender




        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Display the title
        ' Note: we are going to HTML Encode here to prevent script injections
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)

            writer.Write(Context.Server.HtmlEncode(CStr(ViewState("Title"))))
        End Sub 'RenderContents
    End Class 'ItemTitleLink 
End Namespace