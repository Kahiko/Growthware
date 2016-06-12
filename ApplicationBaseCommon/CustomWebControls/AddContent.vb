Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Modules

Namespace CustomWebControls
    '*********************************************************************
    '
    ' AddContent Class
    '
    ' Represents a link to a page to add new content.
    ' This was originally implemented by overriding the HyperLink control.
    ' Unfortunately, the HyperLink control does not resolve paths 
    ' correctly when used in skins, so we have to do some extra work.
    ' This control will also check add permissions before displaying the link.
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class AddContent
        Inherits WebControl

        Private _text As String = String.Empty
        Private _navigateUrl As String = String.Empty
        Private _imageUrl As String = String.Empty

        '*********************************************************************
        '
        ' Text Property
        '
        ' The text to display in the Hyperlink
        '
        '*********************************************************************
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
        ' NavigateUrl Property
        '
        ' The path to the addcontent page (this should always be a link
        ' to a page in the same section).
        '
        '*********************************************************************
        Public Property NavigateUrl() As String
            Get
                Return _navigateUrl
            End Get
            Set(ByVal value As String)
                _navigateUrl = value
            End Set
        End Property

        '*********************************************************************
        '
        ' ImageUrl Property
        '
        ' The path for the (optional) picture to display in the hyperlink.
        '
        '*********************************************************************
        Public Property ImageUrl() As String
            Get
                Return _imageUrl
            End Get
            Set(ByVal value As String)
                _imageUrl = value
            End Set
        End Property

        '*********************************************************************
        '
        ' AddAttributesToRender Method
        '
        ' Add the HRef that links to the AddContent page
        '
        '*********************************************************************
        Protected Overrides Sub AddAttributesToRender(ByVal writer As HtmlTextWriter)
            'writer.AddAttribute(HtmlTextWriterAttribute.Href, CommunityGlobals.CalculatePath(_navigateUrl))
            writer.AddAttribute(HtmlTextWriterAttribute.Href, BaseSettings.FQDNPage & BaseSettings.getURL)
            MyBase.AddAttributesToRender(writer)
        End Sub 'AddAttributesToRender

        '*********************************************************************
        '
        ' Render Method
        '
        ' Here's where we check add permissions
        '
        '*********************************************************************
        Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
            Dim objModuleProfileInfo As MModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)
            Dim objAccountProfileInfo As MAccountSecurityInfo = New MAccountSecurityInfo(objModuleProfileInfo)

            If objAccountProfileInfo.MayAdd OrElse Array.IndexOf(objModuleProfileInfo.AddRoles, "Authenticated") <> -1 Then
                MyBase.Render(writer)
            End If
        End Sub 'Render

        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Display the contents of the link
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            If _imageUrl = String.Empty Then
                writer.Write(_text)
            Else
                writer.Write(String.Format("<img src=""{0}"" alt=""{1}"" border=""0"" />", Page.ResolveUrl(_imageUrl), _text))
            End If
        End Sub 'RenderContents 

        '*********************************************************************
        '
        ' TagKey Property
        '
        ' We want to create an A tag around the content of this control
        '
        '*********************************************************************
        Protected Overrides ReadOnly Property TagKey() As HtmlTextWriterTag
            Get
                Return HtmlTextWriterTag.A
            End Get
        End Property
    End Class 'AddContent 
End Namespace
