Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Model
Imports ApplicationBase.Common.Globals

Namespace CustomWebControls
    '*********************************************************************
    '
    ' Author Class
    '
    ' Represents a content author
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class Author
        Inherits WebControl

        Private _externalImage As String = BaseSettings.imagePath & "external.gif"
        Private objContentInfo As ContentInfo

        '*********************************************************************
        '
        ' Author Constructor
        '
        ' Assign a default css class and retrieve contentInfo from context
        '
        '*********************************************************************
        Public Sub New()
            CssClass = "author"
            ' Get ContentInfo object
            If Not (Context Is Nothing) Then
                objContentInfo = CType(Context.Items("ContentInfo"), ContentInfo)
            End If
        End Sub 'New 

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
        ' TagKey Property
        '
        ' If local user display a link, otherwise display a span 
        '
        '*********************************************************************
        Protected Overrides ReadOnly Property TagKey() As HtmlTextWriterTag
            Get
                If objContentInfo.RemoteAuthor = String.Empty Then
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
            If objContentInfo Is Nothing Then
                objContentInfo = CType(Context.Items("ContentInfo"), ContentInfo)
            End If
            If objContentInfo.RemoteAuthor = String.Empty Then
                Dim encodedAuthor As String = String.Format("Users_ShowProfile.aspx?user={0}", Context.Server.UrlEncode(objContentInfo.Author))
                writer.AddAttribute(HtmlTextWriterAttribute.Href, encodedAuthor)
                MyBase.AddAttributesToRender(writer)
            End If
        End Sub 'AddAttributesToRender

        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Display author (we HTML encode here)
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            If objContentInfo.RemoteAuthor = String.Empty Then
                writer.Write(Context.Server.HtmlEncode(objContentInfo.Author))
            Else
                writer.Write(String.Format("<img src=""{0}"" />", Page.ResolveUrl(_externalImage)))
                writer.Write(Context.Server.HtmlEncode(objContentInfo.RemoteAuthor))
            End If
        End Sub 'RenderContents 
    End Class 'Author
End Namespace