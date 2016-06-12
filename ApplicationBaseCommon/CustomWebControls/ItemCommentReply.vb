Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
imports ApplicationBase.Model
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Common.Globals

Namespace CustomWebControls
    '*********************************************************************
    '
    ' ItemCommentReply Class
    '
    ' Represents a link to a page to add a comment.
    ' This was originally implemented by overriding the HyperLink control.
    ' Unfortunately, the HyperLink control does not resolve paths 
    ' correctly when used in skins, so we have to do some extra work.
    ' This control will also check comment permissions before displaying the link.
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class ItemCommentReply
        Inherits WebControl

        Private _text As String = String.Empty
        Private _imageUrl As String = String.Empty
        Private _replyUrl As String = "Comments_AddComment.aspx?id={0}&ReturnUrl={1}"




        '*********************************************************************
        '
        ' ItemCommentReply Constructor
        '
        ' Assign a default css class and disable view state
        '
        '*********************************************************************
        Public Sub New()
            CssClass = "itemCommentReply"
            EnableViewState = False
        End Sub 'New




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
        ' ReplyUrl Property
        '
        ' The path for the comment page (defaults to the standard one).
        '
        '*********************************************************************

        Public Property ReplyUrl() As String
            Get
                Return _replyUrl
            End Get
            Set(ByVal value As String)
                _replyUrl = value
            End Set
        End Property


        '*********************************************************************
        '
        ' ContentPageID Property
        '
        ' The contentPageID being commented.
        '
        '*********************************************************************

        Public Property ContentPageID() As Integer
            Get
                If ViewState("ContentPageID") Is Nothing Then
                    Return -1
                Else
                    Return Fix(ViewState("ContentPageID"))
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("ContentPageID") = value
            End Set
        End Property




        '*********************************************************************
        '
        ' OnDataBinding Method
        '
        ' Grab the ContentPageID from the containing control's DataItem
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
            ContentPageID = objContentInfo.ContentPageID
        End Sub 'OnDataBinding





        '*********************************************************************
        '
        ' AddAttributesToRender Method
        '
        ' Add the HRef that links to the AddContent page
        '
        '*********************************************************************
        Protected Overrides Sub AddAttributesToRender(ByVal writer As HtmlTextWriter)
            Dim _parentContentInfo As ContentInfo = CType(Context.Items("ContentInfo"), ContentInfo)
            'Dim replyLink As String = String.Format(_replyUrl, ContentPageID, HttpUtility.UrlEncode(String.Format("{0}.aspx", _parentContentInfo.ContentPageID)))
            Dim replyLink As String = "i don't know"
            'writer.AddAttribute(HtmlTextWriterAttribute.Href, CommunityGlobals.CalculatePath(replyLink))
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
            'Dim objUserInfo As UserInfo = CType(Context.Items("UserInfo"), UserInfo)
            'Dim objSectionInfo As SectionInfo = CType(Context.Items("SectionInfo"), SectionInfo)

            Dim objModuleProfileInfo As MModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)
            Dim objAccountProfileInfo As MAccountSecurityInfo = New MAccountSecurityInfo(objModuleProfileInfo)

            If objAccountProfileInfo.MayEdit Then
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
    End Class 'ItemCommentReply 

End Namespace