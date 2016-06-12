Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Discuss

Namespace CustomWebControls
    '*********************************************************************
    '
    ' ItemDiscussLastCommentUsername Class
    '
    ' Represents an author displayed in a template
    '
    '*********************************************************************

    Public Class ItemDiscussLastCommentUsername
        Inherits WebControl


        Private _externalImage As String = BaseSettings.imagePath & "external.gif"



        '*********************************************************************
        '
        ' ExternalImage Property
        '
        ' The relative path to an image displayed for remote users
        '
        '*********************************************************************

        Public Property ExternalImage() As String
            Get
                Return _externalImage
            End Get
            Set(ByVal value As String)
                _externalImage = value
            End Set
        End Property



        '*********************************************************************
        '
        ' ItemDiscussLastCommentUsername Constructor
        '
        ' Set the default css class
        '
        '*********************************************************************
        Public Sub New()
            MyBase.New(HtmlTextWriterTag.A)
            CssClass = "itemDiscussLastCommentUsername"
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
            ViewState("Author") = objPostInfo.LastCommentUsername
        End Sub 'OnDataBinding


        '*********************************************************************
        '
        ' TagKey Property
        '
        ' If local user display a link, otherwise display a span 
        '
        '*********************************************************************

        Protected Overrides ReadOnly Property TagKey() As HtmlTextWriterTag
            Get
                If CStr(ViewState("Author")) <> String.Empty Then
                    Return HtmlTextWriterTag.A
                Else
                    Return HtmlTextWriterTag.Span
                End If
            End Get
        End Property



        '*********************************************************************
        '
        ' AddAttributesToRender Method
        '
        ' Add the HRef that links to the profile page
        '
        '*********************************************************************
        Protected Overrides Sub AddAttributesToRender(ByVal writer As HtmlTextWriter)
            If CStr(ViewState("Author")) <> String.Empty Then
                Dim encodedAuthor As String = String.Format("Users_ShowProfile.aspx?user={0}", Context.Server.UrlEncode(CStr(ViewState("Author"))))
                writer.AddAttribute(HtmlTextWriterAttribute.Href, encodedAuthor)
                MyBase.AddAttributesToRender(writer)
            End If
        End Sub 'AddAttributesToRender




        '*********************************************************************
        '
        ' RenderContents method
        '
        ' Display the link label with author name
        ' Notice that we HtmlEncode (Thanks Andrew D!)
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            If CStr(ViewState("Author")) <> String.Empty Then
                writer.Write(Context.Server.HtmlEncode(CStr(ViewState("Author"))))
            Else
                writer.Write(String.Format("<img src=""{0}"" />", Page.ResolveUrl(_externalImage)))
            End If
        End Sub 'RenderContents 
    End Class 'ItemDiscussLastCommentUsername 
End Namespace