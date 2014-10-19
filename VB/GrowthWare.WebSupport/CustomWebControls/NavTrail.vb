Imports System.ComponentModel
Imports System.Globalization
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports GrowthWare.WebSupport.CustomWebControls.Designers

Namespace CustomWebControls
    <ControlBuilderAttribute(GetType(NavigationTrailControlBuilder)), _
    ParseChildrenAttribute(False), _
    Designer(GetType(CustomDesigner)), _
    Bindable(True), _
    Localizable(True), _
    Category("Data"), _
    DefaultValue(""), _
    Description("Used to display a line type menu or a trail for navigation.")> _
    Public Class NavigationTrail
        Inherits WebControl
        Implements INamingContainer

        Private mDataSource As IEnumerable
        Private mNavigationTrailTab As New ArrayList
        Private m_Orientation As Orientation = Orientation.Horizontal

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
        Public Overridable Property Orientation As Orientation
            Get
                Return m_Orientation
            End Get
            Set(value As Orientation)
                m_Orientation = value
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
        ''' Creates a NavigationTrailtab for each item in a dataview
        ''' </summary>
        ''' <param name="useViewState"></param>
        ''' <remarks>
        ''' Please note that there must at least 4 data items and 
        ''' data item 1 and 3 are used for text and URL respectively
        ''' </remarks>
        Protected Overridable Sub CreateMyControlHierarchy(ByVal useViewState As Boolean)
            Dim resolvedDataSource As IEnumerable = Nothing
            If useViewState Then
                If Not (ViewState("RowCount") Is Nothing) Then
                    resolvedDataSource = New Object(Fix(ViewState("RowCount"))) {}
                Else
                    Throw New CustomWebControlException("Unable to retrieve expected data from View State")
                End If
            Else
                resolvedDataSource = DataSource()
            End If

            If Not (resolvedDataSource Is Nothing) Then
                Dim dataItem As DataRowView
                For Each dataItem In resolvedDataSource
                    Using myNavigationTrailTab As New NavigationTrailTab
                        myNavigationTrailTab.Action = dataItem(3)
                        myNavigationTrailTab.Text = dataItem(1)
                        myNavigationTrailTab.ToolTip = dataItem(2)
                        mNavigationTrailTab.Add(myNavigationTrailTab)
                    End Using
                Next dataItem
            End If
        End Sub

        Protected Overrides Sub CreateChildControls()
            Controls.Clear()

            If Not (ViewState("RowCount") Is Nothing) Then
                Dim useViewState As Boolean = True
                CreateMyControlHierarchy(useViewState)
            End If

        End Sub

        Public Overrides Sub DataBind()
            MyBase.OnDataBinding(EventArgs.Empty)
            Controls.Clear()
            ClearChildViewState()
            TrackViewState()
            Dim useViewState As Boolean = False
            CreateMyControlHierarchy(useViewState)
            ChildControlsCreated = True
        End Sub 'DataBind

        ''' <summary>
        ''' Only add NavigationTrailTab to the mNavigationTrailTab collection.
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddParsedSubObject(ByVal obj As [Object])

            If TypeOf obj Is NavigationTrailTab Then
                mNavigationTrailTab.Add(obj)
            End If
        End Sub

        ''' <summary>
        ''' Display Navigation Trail.
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            If writer Is Nothing Then Throw New ArgumentNullException("writer", "writer cannot be a null reference (Nothing in Visual Basic)")
            ' Display the tabs
            Dim i As Integer
            For i = 0 To mNavigationTrailTab.Count - 1
                Dim objTab As NavigationTrailTab = CType(mNavigationTrailTab(i), NavigationTrailTab)
                Using hyperLink As New HyperLink
                    hyperLink.RenderBeginTag(writer)
                    hyperLink.Text = objTab.Text
                    hyperLink.NavigateUrl = objTab.Action
                    hyperLink.ToolTip = objTab.ToolTip
                    hyperLink.RenderEndTag(writer)
                    hyperLink.RenderControl(writer)
                End Using
                If m_Orientation = Orientation.Horizontal Then
                    If i < mNavigationTrailTab.Count - 1 Then
                        writer.Write("&nbsp;|&nbsp;")
                    End If
                Else
                    If i < mNavigationTrailTab.Count - 1 Then
                        writer.Write("<br/>")
                    End If
                End If
            Next i
        End Sub
    End Class

    '*********************************************************************
    ' Navigation Trail Control Builder Class
    ' Only parse NavigationTrailTab in the NavigationTrail.
    '*********************************************************************

    <Designer(GetType(CustomDesigner))> _
    Public Class NavigationTrailControlBuilder
        Inherits ControlBuilder

        ''' <summary>
        ''' Gets the type of the child control.
        ''' </summary>
        ''' <param name="tagName">Name of the tag.</param>
        ''' <param name="attribs">The attributes.</param>
        ''' <returns>Type.</returns>
        Public Overrides Function GetChildControlType(ByVal tagName As String, ByVal attribs As IDictionary) As Type
            If attribs Is Nothing Then Throw New ArgumentNullException("attribs", "attribs cannot be a null reference (Nothing in Visual Basic)")
            If String.Compare(tagName, "NavigationTrailTab", StringComparison.OrdinalIgnoreCase) = 0 Then
                Return GetType(NavigationTrailTab)
            End If

            Return Nothing
        End Function    'GetChildControlType
    End Class

    ''' <summary>
    ''' Represents individual links in the NavigationTrail.
    ''' </summary>
    ''' <remarks></remarks>
    <Designer(GetType(CustomDesigner))> _
    Public Class NavigationTrailTab
        Inherits Control

        Public Property Action As String

        Public Property [Text] As String

        Public Property ToolTip As String

    End Class

    Public Enum Orientation
        Horizontal = 0
        Vertical = 1
    End Enum

End Namespace
