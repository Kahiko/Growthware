Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Model

Namespace CustomWebControls
    '*********************************************************************
    '
    ' DisplayTopic Class
    '
    ' This control displays either a topic name or topic image that
    ' links to the Topic page.
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class DisplayTopic
        Inherits WebControl

        Private _name As String = String.Empty
        Private _image As String = String.Empty
        Private _alwaysDisplay As Boolean = False

        Public Sub New()
            ' Set CSS Class
            CssClass = "displayTopic"

            ' Get ContentInfo from context
            If Not (Context Is Nothing) Then
                Dim objContentInfo As [Object] = Context.Items("ContentInfo")
                If Not (objContentInfo Is Nothing) Then
                    _name = CType(objContentInfo, ContentInfo).TopicName
                    _image = CType(objContentInfo, ContentInfo).TopicImage
                End If
            End If
        End Sub 'New

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Image() As String
            Get
                Return _image
            End Get
            Set(ByVal value As String)
                _image = value
            End Set
        End Property

        '*********************************************************************
        '
        ' AlwaysDisplay Property
        '
        ' Enables you to display a topic even when topics are disabled
        ' for a section. This property is set by the skin for the topic menu.
        '
        '*********************************************************************
        Public Property AlwaysDisplay() As Boolean
            Get
                Return _alwaysDisplay
            End Get
            Set(ByVal value As Boolean)
                _alwaysDisplay = value
            End Set
        End Property

        '*********************************************************************
        '
        ' Render Method
        '
        ' Only render if topics are enabled. We need to check for the existence
        ' of context to avoid a Designer error in Visual Studio .NET
        '
        '*********************************************************************
        Protected Overrides Sub Render(ByVal tw As HtmlTextWriter)
            If _alwaysDisplay Then
                MyBase.Render(tw)
                Return
            End If

            'If Not (Context.Items("SectionInfo") Is Nothing) Then
            '    Dim objSectionInfo As SectionInfo = CType(Context.Items("SectionInfo"), SectionInfo)

            '    If objSectionInfo.EnableTopics Then
            '        MyBase.Render(tw)
            '    End If
            'End If

            MyBase.Render(tw)
        End Sub 'Render

        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Renders either the name or image for the topic.
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal tw As HtmlTextWriter)
            Dim imageTag As String
            Dim imagePath As String

            ' Calculate topic path
            Dim topicPath As String = String.Format("~/Topic.aspx?topic={0}", Context.Server.UrlEncode(_name))
            topicPath = Page.ResolveUrl(topicPath)

            tw.AddAttribute(HtmlTextWriterAttribute.Href, topicPath)
            tw.RenderBeginTag(HtmlTextWriterTag.A)
            If Image = String.Empty Then
                tw.Write(_name)
            Else
                imagePath = Page.ResolveUrl("~/" + _image)
                imageTag = String.Format("<img src=""{0}"" ALT=""{1}"" border=""0"">", imagePath, _name)
                tw.Write(imageTag)
            End If
            tw.RenderEndTag()
        End Sub 'RenderContents
    End Class 'DisplayTopic 
End Namespace