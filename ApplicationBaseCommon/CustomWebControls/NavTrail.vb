Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace CustomWebControls
    'replace next two lines for conversion to framework 2.0
    '<ControlBuilderAttribute(GetType(NavTrailControlBuilder)), ParseChildrenAttribute(False)> _
    '<DefaultProperty("Text"), ToolboxData("<{0}:NavTrail runat=server></{0}:NavTrail>")> _


    <ControlBuilderAttribute(GetType(NavTrailControlBuilder)), _
    ParseChildrenAttribute(False), _
    Designer(GetType(CustomDesigner)), _
    Bindable(True), _
    Localizable(True), _
    Category("Data"), _
    DefaultValue(""), _
    Description("Used to display a line type menu or a trail for navigation.")> _
    Public Class NavTrail
        Inherits WebControl
        Implements INamingContainer

        Private _dataSource As IEnumerable
        Private _NavTrailTab As New ArrayList

        '*********************************************************************
        ' DataSource is the source of data used with the bind
        '*********************************************************************
        Public Overridable Property DataSource() As IEnumerable
            Get
                Return _dataSource
            End Get
            Set(ByVal value As IEnumerable)
                If TypeOf value Is IEnumerable OrElse value Is Nothing Then
                    _dataSource = value
                Else
                    Throw New ArgumentException
                End If
            End Set
        End Property

        '*********************************************************************
        ' Text is the text property of a navtrailtab
        '*********************************************************************
        Property Text() As String
            Get
                Dim s As String = CStr(ViewState("Text"))
                If s Is Nothing Then
                    Return String.Empty
                Else
                    Return s
                End If
            End Get

            Set(ByVal Value As String)
                ViewState("Text") = Value
            End Set
        End Property

        '*********************************************************************
        ' NavTrail Constructor
        '*********************************************************************
        Public Sub New()

        End Sub ' New

        '*********************************************************************
        ' GetDataSource function
        '*********************************************************************
        Protected Overridable Function GetDataSource() As IEnumerable
            If _dataSource Is Nothing Then
                Return Nothing
            End If
            Dim resolvedDataSource As IEnumerable
            resolvedDataSource = _dataSource
            Return resolvedDataSource
        End Function 'GetDataSource

        '*********************************************************************
        ' CreateMyControlHeirarchy sub
        ' Creates a navtrailtab for each item in a dataview
        '*********************************************************************
        Protected Overridable Sub CreateMyControlHeirarchy(ByVal useViewState As Boolean)
            Dim resolvedDataSource As IEnumerable = Nothing
            If useViewState Then
                If Not (ViewState("RowCount") Is Nothing) Then
                    resolvedDataSource = New Object(Fix(ViewState("RowCount"))) {}
                Else
                    Throw New Exception("Unable to retrieve expected data from ViewState")
                End If
            Else
                resolvedDataSource = GetDataSource()
            End If

            If Not (resolvedDataSource Is Nothing) Then
                Dim dataItem As Object
                Dim row As New TableRow
                For Each dataItem In resolvedDataSource
                    Dim myNavTrailTab As New NavTrailTab
                    myNavTrailTab.Text = dataItem(0)
                    myNavTrailTab.Action = dataItem(1)
                    _NavTrailTab.Add(myNavTrailTab)
                Next dataItem
            End If
        End Sub 'CreateMyControlHeirarchy

        Protected Overrides Sub CreateChildControls()
            Controls.Clear()

            If Not (ViewState("RowCount") Is Nothing) Then
                Dim useViewState As Boolean = True
                CreateMyControlHeirarchy(useViewState)
            End If

        End Sub 'CreateChildControls

        Public Overrides Sub DataBind()
            MyBase.OnDataBinding(EventArgs.Empty)
            Controls.Clear()
            ClearChildViewState()
            TrackViewState()
            Dim useViewState As Boolean = False
            CreateMyControlHeirarchy(useViewState)
            ChildControlsCreated = True
        End Sub 'DataBind


        '*********************************************************************
        '
        ' AddParsedSubObject Method
        '
        ' Only add NavTrailTab to the _NavTrailTab collection.
        '
        '*********************************************************************
        Protected Overrides Sub AddParsedSubObject(ByVal obj As [Object])

            If TypeOf obj Is NavTrailTab Then
                _NavTrailTab.Add(obj)
            End If
        End Sub 'AddParsedSubObject

        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Display Navigation Trail.
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            ' Display the tabs
            Dim i As Integer
            For i = 0 To _NavTrailTab.Count - 1
                Dim objTab As NavTrailTab = CType(_NavTrailTab(i), NavTrailTab)
                Dim hyperLink As New HyperLink
                hyperLink.RenderBeginTag(writer)
                hyperLink.CssClass = "NavTrail"
                hyperLink.Text = objTab.Text
                hyperLink.NavigateUrl = "?Action=" & objTab.Action
                hyperLink.RenderEndTag(writer)
                hyperLink.RenderControl(writer)
                If i < _NavTrailTab.Count - 1 Then
                    writer.Write("&nbsp;|&nbsp;")
                End If
            Next i
        End Sub    'RenderContents 
    End Class   ' NavTrail 

    '*********************************************************************
    ' Navigation Trail Control Builder Class
    ' Only parse NavTrailTab in the NavTrail.
    '*********************************************************************
    <Designer(GetType(CustomDesigner))> _
    Public Class NavTrailControlBuilder
        Inherits ControlBuilder

        Public Overrides Function GetChildControlType(ByVal tagName As String, ByVal attributes As IDictionary) As Type

            If String.Compare(tagName, "NavTrailTab", True) = 0 Then
                Return GetType(NavTrailTab)
            End If

            Return Nothing
        End Function    'GetChildControlType
    End Class ' NavTrailControlBuilder

    '*********************************************************************
    '
    ' NavTrailTab Class
    '
    ' Represents individual links in the NavTrail.
    '
    '*********************************************************************
    <Designer(GetType(CustomDesigner))> _
    Public Class NavTrailTab
        Inherits Control

        Private _text As String
        Private _action As String

        Public Property [Text]() As String
            Get
                Return _text
            End Get
            Set(ByVal Value As String)
                _text = Value
            End Set
        End Property

        Public Property Action() As String
            Get
                Return _action
            End Get
            Set(ByVal Value As String)
                _action = Value
            End Set
        End Property
    End Class ' NavTrail
End Namespace