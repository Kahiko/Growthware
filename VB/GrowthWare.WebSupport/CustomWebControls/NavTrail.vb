Imports System.ComponentModel
Imports System.Globalization
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports GrowthWare.WebSupport.CustomWebControls.Designers

Namespace CustomWebControls
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

        Private mDataSource As IEnumerable
        Private mNavTrailTab As New ArrayList
        Private m_Orentation As Orentation = Orentation.Horizontal

        ''' <summary>
        ''' DataSource is the source of data used with the bind
        ''' </summary>
        ''' <value></value>
        ''' <returns>IEnumerable</returns>
        ''' <remarks></remarks>
        Public Overridable Property DataSource() As IEnumerable
            Get
                Return mDataSource
            End Get
            Set(ByVal value As IEnumerable)
                If TypeOf value Is IEnumerable OrElse value Is Nothing Then
                    mDataSource = value
                Else
                    Throw New ArgumentNullException("value", "value can not be null (Nothing in VB)!")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Valid settings are Horizontal or Vertical
        ''' </summary>
        ''' <value>Horzontal or Vertical</value>
        ''' <returns>String</returns>
        Public Overridable Property Orentation As Orentation
            Get
                Return m_Orentation
            End Get
            Set(value As Orentation)
                m_Orentation = value
            End Set
        End Property


        ''' <summary>
        ''' Text Property
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
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

        ''' <summary>
        ''' New constructor
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Gets the data source
        ''' </summary>
        ''' <returns>IEnumerable</returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetDataSource() As IEnumerable
            If mDataSource Is Nothing Then
                Return Nothing
            End If
            Dim resolvedDataSource As IEnumerable
            resolvedDataSource = mDataSource
            Return resolvedDataSource
        End Function

        ''' <summary>
        ''' Creates a navtrailtab for each item in a dataview
        ''' </summary>
        ''' <param name="useViewState"></param>
        ''' <remarks>
        ''' Please note that there must at least 4 data items and 
        ''' data item 1 and 3 are used for text and URL respectively
        ''' </remarks>
        Protected Overridable Sub CreateMyControlHeirarchy(ByVal useViewState As Boolean)
            Dim resolvedDataSource As IEnumerable = Nothing
            If useViewState Then
                If Not (ViewState("RowCount") Is Nothing) Then
                    resolvedDataSource = New Object(Fix(ViewState("RowCount"))) {}
                Else
                    Throw New CustomWebControlException("Unable to retrieve expected data from ViewState")
                End If
            Else
                resolvedDataSource = GetDataSource()
            End If

            If Not (resolvedDataSource Is Nothing) Then
                Dim dataItem As DataRowView
                For Each dataItem In resolvedDataSource
                    Dim myNavTrailTab As New NavTrailTab
                    myNavTrailTab.Action = dataItem(3)
                    myNavTrailTab.Text = dataItem(1)
                    myNavTrailTab.ToolTip = dataItem(2)
                    mNavTrailTab.Add(myNavTrailTab)
                Next dataItem
            End If
        End Sub

        Protected Overrides Sub CreateChildControls()
            Controls.Clear()

            If Not (ViewState("RowCount") Is Nothing) Then
                Dim useViewState As Boolean = True
                CreateMyControlHeirarchy(useViewState)
            End If

        End Sub

        Public Overrides Sub DataBind()
            MyBase.OnDataBinding(EventArgs.Empty)
            Controls.Clear()
            ClearChildViewState()
            TrackViewState()
            Dim useViewState As Boolean = False
            CreateMyControlHeirarchy(useViewState)
            ChildControlsCreated = True
        End Sub 'DataBind

        ''' <summary>
        ''' Only add NavTrailTab to the mNavTrailTab collection.
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddParsedSubObject(ByVal obj As [Object])

            If TypeOf obj Is NavTrailTab Then
                mNavTrailTab.Add(obj)
            End If
        End Sub

        ''' <summary>
        ''' Display Navigation Trail.
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            ' Display the tabs
            Dim i As Integer
            For i = 0 To mNavTrailTab.Count - 1
                Dim objTab As NavTrailTab = CType(mNavTrailTab(i), NavTrailTab)
                Dim hyperLink As New HyperLink
                hyperLink.RenderBeginTag(writer)
                hyperLink.Text = objTab.Text
                hyperLink.NavigateUrl = objTab.Action
                hyperLink.ToolTip = objTab.ToolTip
                hyperLink.RenderEndTag(writer)
                hyperLink.RenderControl(writer)
                If m_Orentation = Orentation.Horizontal Then
                    If i < mNavTrailTab.Count - 1 Then
                        writer.Write("&nbsp;|&nbsp;")
                    End If
                Else
                    If i < mNavTrailTab.Count - 1 Then
                        writer.Write("<br/>")
                    End If
                End If
            Next i
        End Sub
    End Class

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

    ''' <summary>
    ''' Represents individual links in the NavTrail.
    ''' </summary>
    ''' <remarks></remarks>
    <Designer(GetType(CustomDesigner))> _
    Public Class NavTrailTab
        Inherits Control

        Public Property Action As String

        Public Property [Text] As String

        Public Property ToolTip As String

    End Class

    Public Enum Orentation
        Horizontal = 0
        Vertical = 1
    End Enum

End Namespace
