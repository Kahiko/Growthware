Imports System.Text
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Web.UI
Imports System.Web.UI.Design
Imports System.Drawing.Design
Imports System.Drawing
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Xml
Imports System.Resources
Imports System.Configuration
Imports System.Globalization

Namespace CustomWebControls.Menu
    '/ <summary>
    '/ Represents the method that will handle the <see cref="Menu"/> class's <see cref="MenuItemClicked"/>
    '/ event.
    '/ </summary>
    '/ <param name="sender">The source of the event.</param>
    '/ <param name="e">A <see cref="MenuItemClickEventArgs"/> that contains the event data.</param>
    Public Delegate Sub MenuItemClickedEventHandler(ByVal sender As Object, ByVal e As MenuItemClickEventArgs)


    '/ <summary>
    '/ The menu class is an ASP.NET server control that displays a client-side menu utilizing
    '/ CSS and DHTML.  Its contents are bound through an XML file or by programmatically constructing the
    '/ Menu's menu items.  For full documentation, FAQs, examples, and a messageboard, be sure to check out the
    '/ official skmMenu site: <a href="http://skmMenu.com/">skmMenu.com</a>.
    '/ </summary>
    <Serializable(), DefaultProperty("ID"), ToolboxData("<{0}:Menu runat=server></{0}:Menu>"), Designer("skmMenu.Design.MenuDesigner"), ParseChildren(True), PersistChildren(False), DefaultEvent("MenuItemClick")> _
     Public Class Menu
        Inherits WebControl
        Implements INamingContainer, IPostBackEventHandler

#Region "Private Member Variables"
        Private _subItemsIds As New ArrayList   ' the list of submenu ids
        Private _dataSource As Object = Nothing    ' the menu's datasource - used for databinding
        Private _items As New MenuItemCollection   ' the top-level menu
        Private _roles As New RoleCollection   ' the user roles
        Private _imagePreload As StringBuilder
        Private _curzindex As Integer

        ' styles for the Menu, and unselected & selected menuitems...
        Private _unselectedMenuItemStyle As New TableItemStyle
        Private _selectedMenuItemStyle As New TableItemStyle
#End Region

#Region "MenuItemClick Event"
        '/ <summary>
        '/ Occurs when a <see cref="MenuItem"/> associated with a command is clicked.
        '/ </summary>
        Public Event MenuItemClick As MenuItemClickedEventHandler



        '/ <summary>
        '/ Raises the <see cref="MenuItemClick"/> event.  This allows you to provide a custom handler for the event.
        '/ </summary>
        '/ <param name="e">Instance of <see cref="MenuItemClickEventArgs"/> that contains the event data.</param>
        Protected Overridable Sub OnMenuItemClick(ByVal e As MenuItemClickEventArgs)
            If Not (e Is Nothing) Then
                RaiseEvent MenuItemClick(Me, e)
            End If
        End Sub    'OnMenuItemClick
#End Region

#Region "IStateManager Implementation"

        '/ <summary>
        '/ SaveViewState saves the state of the menu into an object (specifically, an object array
        '/ with five indices).  This is required to have the state persisted across postbacks.
        '/ </summary>
        '/ <returns>A five-element object array representing the menu's state.</returns>
        Protected Overrides Function SaveViewState() As Object
            Dim state() As [Object] = New Object(4) {}
            state(0) = MyBase.SaveViewState()
            state(1) = CType(Me._selectedMenuItemStyle, IStateManager).SaveViewState()
            state(2) = CType(Me._unselectedMenuItemStyle, IStateManager).SaveViewState()
            state(3) = CType(Me._items, IStateManager).SaveViewState()
            state(4) = CType(Me._roles, IStateManager).SaveViewState()

            Return state
        End Function    'SaveViewState



        '/ <summary>
        '/ Loads the state from the passed in saveState object.  This method runs during the
        '/ page life-cycle, and is required for the menu to work across postbacks.
        '/ </summary>
        '/ <param name="savedState">The state persisted by SaveViewState() in the previous life-cycle.</param>
        Protected Overrides Sub LoadViewState(ByVal savedState As Object)
            Dim state As Object() = Nothing

            If Not (savedState Is Nothing) Then
                state = CType(savedState, Object())

                MyBase.LoadViewState(state(0))
                CType(Me._selectedMenuItemStyle, IStateManager).LoadViewState(state(1))
                CType(Me._unselectedMenuItemStyle, IStateManager).LoadViewState(state(2))
                CType(Me._items, IStateManager).LoadViewState(state(3))
                CType(Me._roles, IStateManager).LoadViewState(state(4))
            End If
        End Sub    'LoadViewState



        '/ <summary>
        '/ TrackViewState informs all of the menus complex properties that they, too, need to
        '/ track their viewstate changes.
        '/ </summary>
        Protected Overrides Sub TrackViewState()
            MyBase.TrackViewState()

            If Not (Me._items Is Nothing) Then
                CType(Items, IStateManager).TrackViewState()
            End If
            If Not (Me._selectedMenuItemStyle Is Nothing) Then
                CType(Me.SelectedMenuItemStyle, IStateManager).TrackViewState()
            End If
            If Not (Me._unselectedMenuItemStyle Is Nothing) Then
                CType(Me.UnselectedMenuItemStyle, IStateManager).TrackViewState()
            End If
            If Not (Me._roles Is Nothing) Then
                CType(Me._roles, IStateManager).TrackViewState()
            End If
        End Sub    'TrackViewState
#End Region

#Region "Overriden Control Methods"

        '/ <summary>
        '/ The Render method is responsible for generating the HTML markup.
        '/ </summary>
        '/ <param name="writer">HTMLTextWriter instance to write to.</param>
        '/ <remarks><b>Render</b> ensures that the Menu is created in a server-side Web Form.  This check is required
        '/ because the Menu may contain <see cref="MenuItem"/> instances that, when clicked, cause a postback.</remarks>
        Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
            ' Make sure that the menu is inside a Web Form...
            If Not (Page Is Nothing) Then
                Page.VerifyRenderingInServerForm(Me)
            End If
            MyBase.RenderChildren(writer)
        End Sub    'Render



        '/ <summary>
        '/ This method is called from base.Render(), and starts the build menu process.
        '/ </summary>
        Protected Overrides Sub CreateChildControls()
            If Me._dataSource Is Nothing Then
                ' If not databound (i.e dynamic), traverse the entire menu
                ' and set all of the MenuIDs
                Dim i As Integer
                For i = 0 To (Me.Items.Count) - 1
                    BuildMenuID(Me.Items(i), Me.ClientID, i)
                Next i
            End If
            BuildMenu()
        End Sub    'CreateChildControls



        '/ <summary>
        '/ Event handler for the DataBinding event.
        '/ </summary>
        '/ <remarks>This method runs when the DataBind() method is called.  Essentially, it clears out the
        '/ current state and builds up the menu from the specified <see cref="DataSource"/>.
        '/ </remarks>
        Protected Overrides Sub OnDataBinding(ByVal e As EventArgs)
            ' Start by resetting the Control's state
            Me.Controls.Clear()
            If HasChildViewState Then
                ClearChildViewState()
            End If
            ' load the datasource either as a string or XmlDocuemnt
            Dim xmlDoc As New XmlDocument

            If TypeOf (Me.DataSource) Is String Then
                ' Load the XML document specified by DataSource as a filepath
                xmlDoc.Load(CStr(Me.DataSource))
            ElseIf TypeOf (Me.DataSource) Is XmlDocument Then
                xmlDoc = CType(DataSource, XmlDocument)
            Else
                Return    ' exit - nothing to databind
            End If
            ' Clear out the MenuItems and build them according to the XmlDocument
            Me._items.Clear()
            Me._items = GatherMenuItems(xmlDoc.SelectSingleNode("/menu"), Me.ClientID)
            BuildMenu()

            Me.ChildControlsCreated = True

            If Not IsTrackingViewState Then
                TrackViewState()
            End If
        End Sub    'OnDataBinding


        '/ <summary>
        '/ Generates the client-side JavaScript.
        '/ </summary><remarks>
        '/ For non-databound (dynamic) menus, <b>OnPreRender</b> also sets all of the MenuIDs.
        '/ <p />
        '/ For more information on adding client-side script via an ASP.NET server control, refer to:
        '/	<a href="http://msdn.microsoft.com/asp.net/default.aspx?pull=/library/en-us/dnaspp/html/aspnet-injectclientsidesc.asp">Injecting Client-Side Script from an ASP.NET Server Control</a>
        '/ </remarks>
        Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
            MyBase.OnPreRender(e)

            Me.RegisterClientScriptBlock()    ' adds methods and global vars
            Me.RegisterClientStartupScript()    ' adds initialization code for each menu
            Me.RegisterSubmenuArray()    ' adds the skm_SubMenus array
            ' Build Preload command to preload images (if any)
            Me._imagePreload = New StringBuilder
            Dim i As Integer
            For i = 0 To (Me.Items.Count) - 1
                BuildImagePreload(Me.Items(i))
            Next i
            Me.RegisterPreloadCommand()    ' adds the code to preload the images
        End Sub    'OnPreRender


        '/ <summary>
        '/ Determines what images are used by a specified <see cref="MenuItem"/> instance.
        '/ </summary>
        '/ <param name="mi">The <b>MenuItem</b> to examine.</param>
        '/ <remarks>The <see cref="MenuItem"/> class has a number of image properties, like <see cref="MouseOverImage"/>,
        '/ <see cref="MouseDownImage"/> and others.  This method checks these properties to determine if any images
        '/ are used.  If it locates any, it marks the image to be preloaded using client-side JavaScript.</remarks>
        Protected Overridable Sub BuildImagePreload(ByVal mi As MenuItem)
            If mi.Image <> String.Empty Then
                If Me._imagePreload.ToString() <> String.Empty Then
                    Me._imagePreload.Append(",")
                End If
                Me._imagePreload.Append(("'" + mi.Image + "'"))
            End If
            If mi.MouseOverImage <> String.Empty Then
                If Me._imagePreload.ToString() <> String.Empty Then
                    Me._imagePreload.Append(",")
                End If
                Me._imagePreload.Append(("'" + mi.MouseOverImage + "'"))
            End If
            If mi.MouseDownImage <> String.Empty Then
                If Me._imagePreload.ToString() <> String.Empty Then
                    Me._imagePreload.Append(",")
                End If
                Me._imagePreload.Append(("'" + mi.MouseDownImage + "'"))
            End If
            If mi.MouseUpImage <> String.Empty Then
                If Me._imagePreload.ToString() <> String.Empty Then
                    Me._imagePreload.Append(",")
                End If
                Me._imagePreload.Append(("'" + mi.MouseUpImage + "'"))
            End If
            If mi.SubItems.Count > 0 Then
                Dim i As Integer
                For i = 0 To mi.SubItems.Count - 1
                    BuildImagePreload(mi.SubItems(i))
                Next i
            End If
        End Sub    'BuildImagePreload



        '/ <summary>
        '/ Creates a <see cref="MenuID"/> for a <see cref="MenuItem"/>.
        '/ </summary>
        '/ <param name="mi">The <see cref="MenuItem"/> that will have its <b>MenuID</b> set.</param>
        '/ <param name="parentID">The <b>MenuID</b> of the <b>MenuItem</b> <i>mi</i>'s parent.</param>
        '/ <param name="indexValue">If <b>MenuItem</b> <i>mi</i> has a parent, the <b>indexValue</b> indicates
        '/ what index <i>mi</i> is in its parent's set of <b>MenuItem</b> children.</param>
        '/ <remarks><b>BuildMenuID()</b> formats the index as a three-digit number.  This puts an upperbound on the
        '/ number of <b>MenuItems</b> any menu can contain.  Precisely, no single menu may contain more than 1,000
        '/ <b>MenuItem</b>s.</remarks>
        Protected Overridable Sub BuildMenuID(ByVal mi As MenuItem, ByVal parentID As String, ByVal indexValue As Integer)
            mi.MenuID = parentID + "-menuItem" + indexValue.ToString("d3")    ' to allow for more than 1,000 menu items per menu, change "d3" to "d4" ("d4" will allow for 10,000 MenuItems...)
            If mi.SubItems.Count > 0 Then
                Dim i As Integer
                For i = 0 To mi.SubItems.Count - 1
                    BuildMenuID(mi.SubItems(i), mi.MenuID + "-subMenu", i)       ' recurse through the mi's children...
                Next i
            End If
        End Sub    'BuildMenuID
#End Region


#Region "Menu Creation Methods"
#Region "Class and URL Helper Functions"

        '/ <summary>
        '/ Allows for two strings to be passed in - a default CSS class and a specific Menu class.  If no
        '/ Menu class is specifies, the default class is returned.
        '/ </summary>
        Private Overloads Function GetClass(ByVal defaultclass As String, ByVal menuclass As String) As String
            If menuclass <> String.Empty Then
                Return menuclass
            Else
                Return defaultclass
            End If
        End Function    'GetClass

        '/ <summary>
        '/ Allows for three strings to be passed in - a default CSS class, a specific Menu class, and a style class.
        '/ The proper class string is returned.
        '/ </summary>
        Private Overloads Function GetClass(ByVal defaultclass As String, ByVal menuclass As String, ByVal styleclass As String) As String
            If menuclass <> String.Empty Then
                Return menuclass
            ElseIf defaultclass <> String.Empty Then
                Return defaultclass
            Else
                Return styleclass
            End If
        End Function    'GetClass


        '/ <summary>
        '/ This method utilizes <b>ResolveUrl()</b> if needed.  <b>ResolveUrl()</b> resolves any tildes that may
        '/ appear in the URL into the proper path.  For more information see <a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemwebuicontrolclassresolveurltopic.asp">http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemwebuicontrolclassresolveurltopic.asp</a>
        '/ </summary>
        Private Function GetURL(ByVal resolveurl As Boolean, ByVal url As String) As String
            If Me.DefaultResolveURL OrElse resolveurl Then
                Return Me.ResolveUrl(url)
            Else
                Return url
            End If
        End Function    'GetURL
#End Region


        '/ <summary>
        '/ BuildMenu builds the top-level menu.  It is called from the OnDataBinding method as well
        '/ as from <see cref="CreateChildControls"/>.  It has code to check if the top-level menu should be
        '/ laid out horizontally or vertically.
        '/ </summary>
        Protected Overridable Sub BuildMenu()
            Dim image As String = String.Empty
            Dim mouseoverimage As String = String.Empty
            Dim mousedownimage As String = String.Empty
            Dim mouseupimage As String = String.Empty
            Dim enUSCulture As New CultureInfo("en-us", False)

            ' iterate through the Items
            Dim menu As New Table
            menu.Attributes.Add("id", Me.ClientID)
            menu.MergeStyle(Me.ControlStyle)

            menu.CellPadding = ItemPadding
            menu.CellSpacing = ItemSpacing
            menu.GridLines = GridLines

            ' Add the Menu control's STYLE properties to the TABLE
            Dim key As IEnumerator = Me.Style.Keys.GetEnumerator()
            While key.MoveNext()
                Dim k As String = key.Current.ToString()
                menu.Style.Add(k, Me.Style(k))
            End While

            menu.Style.Remove("Z-INDEX")    ' remove z-index added automatically by grid positioning
            ' set the Z-INDEX
            menu.Style.Add("z-index", Me.zIndex.ToString())
            _curzindex = Me.zIndex + 2

            BuildOpacity(menu)

            Dim tr As TableRow = Nothing
            If Layout = MenuLayout.Horizontal Then
                tr = New TableRow
            End If
            ' Iterate through the top-level menu's menuitems, and add a <td> tag for each menuItem
            Dim i As Integer
            For i = 0 To (Me._items.Count) - 1
                Dim mi As MenuItem = Me.Items(i)

                ' only render this MenuItem if it is visible and the user has permissions
                If mi.Visible AndAlso UserHasPermission(mi) Then
                    If Layout = MenuLayout.Vertical Then
                        tr = New TableRow
                    End If
                    Dim td As New TableCell
                    td.ApplyStyle(Me._unselectedMenuItemStyle)
                    ' The style is overwritten by anything specifically set in menuitem
                    If Not mi.BackColor.Equals(Color.Empty) Then
                        td.BackColor = mi.BackColor
                    End If
                    If Not (mi.Font Is Nothing) Then
                        td.Font.CopyFrom(mi.Font)
                    End If
                    If Not mi.ForeColor.Equals(Color.Empty) Then
                        td.ForeColor = mi.ForeColor
                    End If
                    If Not mi.Height.Equals(Unit.Empty) Then
                        td.Height = mi.Height
                    End If
                    If Not mi.Width.Equals(Unit.Empty) Then
                        td.Width = mi.Width
                    End If
                    If mi.CssClass <> String.Empty Then
                        td.CssClass = mi.CssClass
                    ElseIf Me.DefaultCssClass <> String.Empty Then
                        td.CssClass = Me.DefaultCssClass
                    End If
                    If Not mi.BorderColor.Equals(Color.Empty) Then
                        td.BorderColor = mi.BorderColor
                    End If
                    If mi.BorderStyle <> BorderStyle.NotSet Then
                        td.BorderStyle = mi.BorderStyle
                    End If
                    If Not mi.BorderWidth.Equals(Unit.Empty) Then
                        td.BorderWidth = mi.BorderWidth
                    End If
                    If mi.HorizontalAlign <> System.Web.UI.WebControls.HorizontalAlign.NotSet Then
                        td.HorizontalAlign = mi.HorizontalAlign
                    End If
                    If mi.VerticalAlign <> System.Web.UI.WebControls.VerticalAlign.NotSet Then
                        td.VerticalAlign = mi.VerticalAlign
                    End If
                    BuildOpacity(td)
                    If mi.Text <> String.Empty Then
                        td.Text = mi.Text       ' Text
                    ElseIf mi.Image <> String.Empty Then       ' Show Image
                        Dim cellimage As New System.Web.UI.WebControls.Image

                        cellimage.ImageUrl = mi.Image
                        cellimage.AlternateText = mi.ImageAltText
                        td.Controls.Add(cellimage)

                        image = mi.Image
                        mouseoverimage = mi.MouseOverImage
                        mousedownimage = mi.MouseDownImage
                        mouseupimage = mi.MouseUpImage
                    End If
                    td.Attributes.Add("id", mi.MenuID)

                    ' Add in the left or right image as needed
                    If mi.LeftImage <> String.Empty Then
                        Dim leftimage As New System.Web.UI.WebControls.Image
                        Dim leftliteral As New System.Web.UI.WebControls.Literal

                        leftimage.ImageAlign = mi.LeftImageAlign
                        If Not mi.LeftImageRightPadding.Equals(Unit.Empty) Then
                            leftimage.Style.Add("margin-right", mi.LeftImageRightPadding.Value.ToString())
                        End If
                        leftimage.ImageUrl = mi.LeftImage
                        td.Controls.Add(leftimage)

                        leftliteral.Text = td.Text
                        td.Controls.Add(leftliteral)
                    ElseIf mi.RightImage <> String.Empty Then
                        Dim rightimage As New System.Web.UI.WebControls.Image
                        Dim rightliteral As New System.Web.UI.WebControls.Literal

                        rightliteral.Text = td.Text
                        td.Controls.Add(rightliteral)

                        rightimage.ImageAlign = mi.RightImageAlign
                        If Not mi.RightImageLeftPadding.Equals(Unit.Empty) Then
                            rightimage.Style.Add("margin-left", mi.RightImageLeftPadding.Value.ToString())
                        End If
                        rightimage.ImageUrl = mi.RightImage
                        td.Controls.Add(rightimage)
                    End If

                    ' Prepare MouseOverCssClass
                    Dim mouseover As String = String.Empty

                    If Me.DefaultMouseOverCssClass <> String.Empty OrElse mi.MouseOverCssClass <> String.Empty OrElse Me._selectedMenuItemStyle.CssClass <> String.Empty Then
                        mouseover = "this.className='" + GetClass(Me.DefaultMouseOverCssClass, mi.MouseOverCssClass, Me.SelectedMenuItemStyle.CssClass) + "';"
                    End If
                    If mi.Enabled Then       ' if enabled...
                        ' Generate OnClick handler
                        If mi.JavascriptCommand <> String.Empty Then       ' javascript command
                            td.Attributes.Add("onclick", mi.JavascriptCommand + "skm_closeSubMenus(document.getElementById('" + Me.ClientID + "'));")
                        ElseIf mi.Url <> String.Empty Then
                            If mi.Target <> String.Empty Then
                                td.Attributes.Add("onclick", "javascript:skm_closeSubMenus(document.getElementById('" + Me.ClientID + "'));window.open('" + GetURL(mi.ResolveURL, mi.Url) + "','" + mi.Target + "');")
                            ElseIf Me.DefaultTarget <> String.Empty Then
                                td.Attributes.Add("onclick", "javascript:skm_closeSubMenus(document.getElementById('" + Me.ClientID + "'));window.open('" + GetURL(mi.ResolveURL, mi.Url) + "','" + Me.DefaultTarget + "');")
                            Else
                                td.Attributes.Add("onclick", "javascript:skm_closeSubMenus(document.getElementById('" + Me.ClientID + "'));location.href='" + GetURL(mi.ResolveURL, mi.Url) + "';")
                            End If
                        ElseIf mi.CommandName <> String.Empty Then       ' Must be postback action
                            td.Attributes.Add("onclick", Page.GetPostBackClientHyperlink(Me, mi.CommandName))
                        ElseIf Me.ClickToOpen Then       ' Open submenu on click
                            If Layout = MenuLayout.Vertical Then
                                td.Attributes.Add("onclick", "javascript:skm_mousedOverMenu('" + Me.ClientID + "',this, document.getElementById('" + Me.ClientID + "'), true, '" + mouseoverimage + "');" + GenerateShimCall("true", mi.MenuID + "-subMenu") + mouseover)
                            Else
                                td.Attributes.Add("onclick", "javascript:skm_mousedOverMenu('" + Me.ClientID + "',this, document.getElementById('" + Me.ClientID + "'), false, '" + mouseoverimage + "');" + GenerateShimCall("true", mi.MenuID + "-subMenu") + mouseover)
                            End If
                        End If
                    End If
                    If mi.Enabled Then
                        ' Output Tooltip
                        If mi.ToolTip <> String.Empty Then
                            td.ToolTip = mi.ToolTip
                        End If
                        ' Output MouseDownCssClass or MouseDownImage
                        Dim mousedown As String = String.Empty

                        If Me.DefaultMouseDownCssClass <> String.Empty OrElse mi.MouseDownCssClass <> String.Empty Then
                            mousedown = "this.className='" + GetClass(Me.DefaultMouseDownCssClass, mi.MouseDownCssClass) + "';"
                        End If
                        If mousedownimage <> String.Empty Then
                            mousedown += "setimage(this, '" + mousedownimage + "');"
                        End If
                        If mousedown <> String.Empty Then
                            td.Attributes.Add("onmousedown", mousedown)
                        End If
                        ' Output MouseUpCssClass or MouseUpImage
                        Dim mouseup As String = String.Empty

                        If Me.DefaultMouseUpCssClass <> String.Empty OrElse mi.MouseUpCssClass <> String.Empty Then
                            mouseup = "this.className='" + GetClass(Me.DefaultMouseUpCssClass, mi.MouseUpCssClass) + "';"
                        End If
                        If mouseupimage <> String.Empty Then
                            mouseup += "setimage(this, '" + mouseupimage + "');"
                        End If
                        If mouseup <> String.Empty Then
                            td.Attributes.Add("onmouseup", mouseup)
                        End If
                        If Me.ClickToOpen = False Then
                            If Layout = MenuLayout.Vertical Then
                                td.Attributes.Add("onmouseover", "javascript:skm_mousedOverMenu('" + Me.ClientID + "',this, document.getElementById('" + Me.ClientID + "'), true, '" + mouseoverimage + "');" + GenerateShimCall("true", mi.MenuID + "-subMenu") + mouseover)
                            Else
                                td.Attributes.Add("onmouseover", "javascript:skm_mousedOverMenu('" + Me.ClientID + "',this, document.getElementById('" + Me.ClientID + "'), false, '" + mouseoverimage + "');" + GenerateShimCall("true", mi.MenuID + "-subMenu") + mouseover)
                            End If
                        Else
                            td.Attributes.Add("onmouseover", "javascript:skm_mousedOverClickToOpen('" + Me.ClientID + "', this, document.getElementById('" + Me.ClientID + "'), '" + mouseoverimage + "');" + mouseover)
                        End If
                        td.Attributes.Add("onmouseout", "javascript:skm_mousedOutMenu('" + Me.ClientID + "', this, '" + image + "');" + "this.className='" + GetClass(Me.DefaultCssClass, mi.CssClass, Me._unselectedMenuItemStyle.CssClass) + "';")
                        ' disabled...
                    Else
                        td.Attributes.Add("onmouseover", "javascript:skm_mousedOverSpacer('" + Me.ClientID + "', this, document.getElementById('" + Me.ClientID + "'));")
                        td.Attributes.Add("onmouseout", "javascript:skm_mousedOutSpacer('" + Me.ClientID + "', this);")
                    End If

                    If mi.Url <> String.Empty OrElse mi.CommandName <> String.Empty Then
                        If Me.Cursor <> MouseCursor.Default Then
                            If Me.Page.Request.Browser.Browser = "IE" Then
                                td.Style.Add("cursor", "hand")
                            Else
                                td.Style.Add("cursor", "pointer")
                            End If
                        End If
                    End If
                    tr.Cells.Add(td)

                    If Layout = MenuLayout.Vertical Then
                        menu.Rows.Add(tr)
                    End If
                    ' Add the subitems for this menu, if needed
                    If mi.SubItems.Count > 0 Then
                        ' Create an IFrame (IE5.5 or better) to windowed form elements that might
                        ' interfere with display of the menu
                        If Me.Page.Request.Browser.Browser = "IE" AndAlso Convert.ToDouble(Me.Page.Request.Browser.Version, enUSCulture) >= 5.5 Then
                            Dim iframe As New System.Web.UI.HtmlControls.HtmlGenericControl
                            iframe.TagName = "iframe"
                            iframe.Attributes.Add("id", "shim" + mi.MenuID + "-subMenu")
                            iframe.Attributes.Add("src", IFrameSrc)
                            iframe.Attributes.Add("scrolling", "no")
                            iframe.Attributes.Add("frameborder", "no")
                            iframe.Style.Add("position", "absolute")
                            iframe.Style.Add("top", "0px")
                            iframe.Style.Add("left", "0px")
                            iframe.Style.Add("display", "none")
                            BuildOpacity(iframe)
                            Controls.Add(iframe)
                        End If

                        Me._subItemsIds.Add((mi.MenuID + "-subMenu"))
                        AddMenu(mi.MenuID + "-subMenu", mi.SubItems)
                    End If
                End If
            Next i

            If Layout = MenuLayout.Horizontal Then
                menu.Rows.Add(tr)
            End If
            Controls.Add(menu)
        End Sub    'BuildMenu


#Region "Opacity Adding Methods"

        Private Overloads Sub BuildOpacity(ByVal ctrl As WebControl)
            Dim enUSCulture As New CultureInfo("en-us", False)
            If Not (Me.Opacity Is Nothing) AndAlso Me.Opacity <> String.Empty Then
                If Convert.ToInt32(Me.Opacity) <> 100 Then    ' If 100 don't output, 100=solid
                    If Me.Page.Request.Browser.Browser = "IE" AndAlso Convert.ToDouble(Me.Page.Request.Browser.Version, enUSCulture) >= 5.0 Then
                        ctrl.Style.Add("filter", "alpha (opacity=" + Me.Opacity + ")")
                    ElseIf Me.Page.Request.Browser.Browser = "Mozilla" OrElse (Me.Page.Request.Browser.Browser = "Netscape" AndAlso Convert.ToDouble(Me.Page.Request.Browser.Version, enUSCulture) >= 6.0) Then
                        ctrl.Style.Add("-moz-opacity", "." + Me.Opacity)
                    End If
                End If
            End If
        End Sub    'BuildOpacity

        Private Overloads Sub BuildOpacity(ByVal ctrl As System.Web.UI.HtmlControls.HtmlGenericControl)
            Dim enUSCulture As New CultureInfo("en-us", False)
            If Not (Me.Opacity Is Nothing) AndAlso Me.Opacity <> String.Empty Then
                If Convert.ToInt32(Me.Opacity) <> 100 Then    ' If 100 don't output, 100=solid
                    If Me.Page.Request.Browser.Browser = "IE" AndAlso Convert.ToDouble(Me.Page.Request.Browser.Version, enUSCulture) >= 5.0 Then
                        ctrl.Style.Add("filter", "alpha (opacity=" + Me.Opacity + ")")
                    ElseIf Me.Page.Request.Browser.Browser = "Mozilla" OrElse (Me.Page.Request.Browser.Browser = "Netscape" AndAlso Convert.ToDouble(Me.Page.Request.Browser.Version, enUSCulture) >= 6.0) Then
                        ctrl.Style.Add("-moz-opacity", "." + Me.Opacity)
                    End If
                End If
            End If
        End Sub    'BuildOpacity
#End Region


        '/ <summary>
        '/ AddMenu is called recusively, doing a depth-first traversal of the menu hierarchy and building
        '/ up the HTML elements from the object model.
        '/ </summary>
        '/ <param name="menuID">The ID of the parent menu.</param>
        '/ <param name="myItems">The collection of menuitems.</param>
        Protected Overridable Sub AddMenu(ByVal menuID As String, ByVal myItems As MenuItemCollection)
            Dim image As String = String.Empty
            Dim mouseoverimage As String = String.Empty
            Dim mousedownimage As String = String.Empty
            Dim mouseupimage As String = String.Empty
            Dim enUSCulture As New CultureInfo("en-us", False)

            ' iterate through the Items
            Dim menu As New Table

            menu.Attributes.Add("id", menuID)
            menu.Attributes.Add("style", "display: none;")
            ' The style is overwritten by anthing specifically set in menuitem
            If Not Me.BackColor.Equals(Color.Empty) Then
                menu.BackColor = Me.BackColor
            End If
            If Not (Me.Font Is Nothing) Then
                menu.Font.CopyFrom(Me.Font)
            End If
            If Not Me.ForeColor.Equals(Color.Empty) Then
                menu.ForeColor = Me.ForeColor
            End If
            If Me.SubMenuCssClass <> String.Empty Then
                menu.CssClass = Me.SubMenuCssClass
            ElseIf Me.CssClass <> String.Empty Then    ' Use if SubMenuCssClass was blank
                menu.CssClass = Me.CssClass
            End If
            If Not Me.BorderColor.Equals(Color.Empty) Then
                menu.BorderColor = Me.BorderColor
            End If
            If Me.BorderStyle <> BorderStyle.NotSet Then
                menu.BorderStyle = Me.BorderStyle
            End If
            If Not Me.BorderWidth.Equals(Unit.Empty) Then
                menu.BorderWidth = Me.BorderWidth
            End If
            menu.CellPadding = ItemPadding
            menu.CellSpacing = ItemSpacing
            menu.GridLines = GridLines
            menu.Style.Add("z-index", _curzindex.ToString())
            _curzindex += 2

            BuildOpacity(menu)

            ' Iterate through the menuItem's subMenu...
            Dim i As Integer
            For i = 0 To myItems.Count - 1
                Dim mi As MenuItem = myItems(i)

                ' only render this MenuItem if it is visible and the user has permissions
                If mi.Visible AndAlso UserHasPermission(mi) Then
                    Dim tr As New TableRow
                    Dim td As New TableCell
                    td.ApplyStyle(Me._unselectedMenuItemStyle)
                    ' The style is overwritten by anything specifically set in menuitem
                    If Not mi.BackColor.Equals(Color.Empty) Then
                        td.BackColor = mi.BackColor
                    End If
                    If Not (mi.Font Is Nothing) Then
                        td.Font.CopyFrom(mi.Font)
                    End If
                    If Not mi.ForeColor.Equals(Color.Empty) Then
                        td.ForeColor = mi.ForeColor
                    End If
                    If Not mi.Height.Equals(Unit.Empty) Then
                        td.Height = mi.Height
                    End If
                    If Not mi.Width.Equals(Unit.Empty) Then
                        td.Width = mi.Width
                    End If
                    If mi.CssClass <> String.Empty Then
                        td.CssClass = mi.CssClass
                    ElseIf Me.DefaultCssClass <> String.Empty Then
                        td.CssClass = Me.DefaultCssClass
                    End If
                    If Not mi.BorderColor.Equals(Color.Empty) Then
                        td.BorderColor = mi.BorderColor
                    End If
                    If mi.BorderStyle <> BorderStyle.NotSet Then
                        td.BorderStyle = mi.BorderStyle
                    End If
                    If Not mi.BorderWidth.Equals(Unit.Empty) Then
                        td.BorderWidth = mi.BorderWidth
                    End If
                    If mi.HorizontalAlign <> System.Web.UI.WebControls.HorizontalAlign.NotSet Then
                        td.HorizontalAlign = mi.HorizontalAlign
                    End If
                    If mi.VerticalAlign <> System.Web.UI.WebControls.VerticalAlign.NotSet Then
                        td.VerticalAlign = mi.VerticalAlign
                    End If
                    BuildOpacity(td)
                    If mi.Text <> String.Empty Then
                        td.Text = mi.Text       ' Text
                    ElseIf mi.Image <> String.Empty Then       ' Show Image
                        Dim cellimage As New System.Web.UI.WebControls.Image

                        cellimage.ImageUrl = mi.Image
                        cellimage.AlternateText = mi.ImageAltText
                        td.Controls.Add(cellimage)

                        image = mi.Image
                        mouseoverimage = mi.MouseOverImage
                        mousedownimage = mi.MouseDownImage
                        mouseupimage = mi.MouseUpImage
                    End If
                    td.Attributes.Add("id", mi.MenuID)

                    ' Add in the left or right image as needed
                    If mi.LeftImage <> String.Empty Then
                        Dim leftimage As New System.Web.UI.WebControls.Image
                        Dim leftliteral As New System.Web.UI.WebControls.Literal

                        leftimage.ImageAlign = mi.LeftImageAlign
                        If Not mi.LeftImageRightPadding.Equals(Unit.Empty) Then
                            leftimage.Style.Add("margin-right", mi.LeftImageRightPadding.Value.ToString())
                        End If
                        leftimage.ImageUrl = mi.LeftImage
                        td.Controls.Add(leftimage)

                        leftliteral.Text = td.Text
                        td.Controls.Add(leftliteral)
                    ElseIf mi.RightImage <> String.Empty Then
                        Dim rightimage As New System.Web.UI.WebControls.Image
                        Dim rightliteral As New System.Web.UI.WebControls.Literal

                        rightliteral.Text = td.Text
                        td.Controls.Add(rightliteral)

                        rightimage.ImageAlign = mi.RightImageAlign
                        If Not mi.RightImageLeftPadding.Equals(Unit.Empty) Then
                            rightimage.Style.Add("margin-left", mi.RightImageLeftPadding.Value.ToString())
                        End If
                        rightimage.ImageUrl = mi.RightImage
                        td.Controls.Add(rightimage)
                    End If

                    ' Prepare MouseOverCssClass
                    Dim mouseover As String = String.Empty

                    If Me.DefaultMouseOverCssClass <> String.Empty OrElse mi.MouseOverCssClass <> String.Empty OrElse Me._selectedMenuItemStyle.CssClass <> String.Empty Then
                        mouseover = "this.className='" + GetClass(Me.DefaultMouseOverCssClass, mi.MouseOverCssClass, Me._selectedMenuItemStyle.CssClass) + "';"
                    End If
                    If mi.Enabled Then       ' If enabled...
                        ' Generate OnClick handler
                        If mi.JavascriptCommand <> String.Empty Then       ' javascript command
                            td.Attributes.Add("onclick", mi.JavascriptCommand + "skm_closeSubMenus(document.getElementById('" + Me.ClientID + "'));")
                        ElseIf mi.Url <> String.Empty Then
                            If mi.Target <> String.Empty Then
                                td.Attributes.Add("onclick", "javascript:skm_closeSubMenus(document.getElementById('" + Me.ClientID + "'));window.open('" + GetURL(mi.ResolveURL, mi.Url) + "','" + mi.Target + "');")
                            ElseIf Me.DefaultTarget <> String.Empty Then
                                td.Attributes.Add("onclick", "javascript:skm_closeSubMenus(document.getElementById('" + Me.ClientID + "'));window.open('" + GetURL(mi.ResolveURL, mi.Url) + "','" + Me.DefaultTarget + "');")
                            Else
                                td.Attributes.Add("onclick", "javascript:skm_closeSubMenus(document.getElementById('" + Me.ClientID + "'));location.href='" + GetURL(mi.ResolveURL, mi.Url) + "';")
                            End If
                        ElseIf mi.CommandName <> String.Empty Then       ' Must be postback action
                            td.Attributes.Add("onclick", Page.GetPostBackClientHyperlink(Me, mi.CommandName))
                        ElseIf Me.ClickToOpen Then       ' Open submenu on click
                            td.Attributes.Add("onclick", "javascript:skm_mousedOverMenu('" + Me.ClientID + "',this, document.getElementById('" + menuID + "'), true, '" + mouseoverimage + "');" + GenerateShimCall("true", mi.MenuID + "-subMenu"))
                        End If
                    End If
                    If mi.Enabled Then
                        ' Output Tooltip
                        If mi.ToolTip <> String.Empty Then
                            td.ToolTip = mi.ToolTip
                        End If
                    End If
                    ' Is this a enabled menuitem?  (as opposed to a Separator or Header)
                    If mi.MenuType = MenuItemType.MenuItem AndAlso mi.Enabled Then
                        ' Output MouseDownCssClass or MouseDownImage
                        Dim mousedown As String = String.Empty

                        If Me.DefaultMouseDownCssClass <> String.Empty OrElse mi.MouseDownCssClass <> String.Empty Then
                            mousedown = "this.className='" + GetClass(Me.DefaultMouseDownCssClass, mi.MouseDownCssClass) + "';"
                        End If
                        If mousedownimage <> String.Empty Then
                            mousedown += "setimage(this, '" + mousedownimage + "');"
                        End If
                        If mousedown <> String.Empty Then
                            td.Attributes.Add("onmousedown", mousedown)
                        End If
                        ' Output MouseUpCssClass or MouseUpImage
                        Dim mouseup As String = String.Empty

                        If Me.DefaultMouseUpCssClass <> String.Empty OrElse mi.MouseUpCssClass <> String.Empty Then
                            mouseup = "this.className='" + GetClass(Me.DefaultMouseUpCssClass, mi.MouseUpCssClass) + "';"
                        End If
                        If mouseupimage <> String.Empty Then
                            mouseup += "setimage(this, '" + mouseupimage + "');"
                        End If
                        If mouseup <> String.Empty Then
                            td.Attributes.Add("onmouseup", mouseup)
                        End If
                        If Me.ClickToOpen = False Then
                            td.Attributes.Add("onmouseover", "javascript:skm_mousedOverMenu('" + Me.ClientID + "',this, document.getElementById('" + menuID + "'), true, '" + mouseoverimage + "');" + GenerateShimCall("true", mi.MenuID + "-subMenu") + mouseover)
                        Else
                            td.Attributes.Add("onmouseover", "javascript:skm_mousedOverClickToOpen('" + Me.ClientID + "', this, document.getElementById('" + menuID + "'), '" + mouseoverimage + "');" + mouseover)
                        End If
                        td.Attributes.Add("onmouseout", "javascript:skm_mousedOutMenu('" + Me.ClientID + "', this,'" + image + "');" + "this.className='" + GetClass(Me.DefaultCssClass, mi.CssClass, Me._unselectedMenuItemStyle.CssClass) + "';")
                    Else       ' If only a spacer, header or disabled, don't make any style change on mouseover
                        td.Attributes.Add("onmouseover", "javascript:skm_mousedOverSpacer('" + Me.ClientID + "', this, document.getElementById('" + menuID + "'));")
                        td.Attributes.Add("onmouseout", "javascript:skm_mousedOutSpacer('" + Me.ClientID + "', this);")
                    End If

                    If mi.Url <> String.Empty OrElse mi.CommandName <> String.Empty Then
                        If Me.Cursor <> MouseCursor.Default Then
                            If Me.Page.Request.Browser.Browser = "IE" Then
                                td.Style.Add("cursor", "hand")
                            Else
                                td.Style.Add("cursor", "pointer")
                            End If
                        End If
                    End If
                    tr.Cells.Add(td)
                    menu.Rows.Add(tr)

                    ' (Recursively) Add the subitems for this menu, if needed
                    If mi.SubItems.Count > 0 Then
                        ' Create an IFrame (IE5.5 or better) to windowed form elements that might
                        ' interfere with display of the menu
                        If Me.Page.Request.Browser.Browser = "IE" AndAlso Convert.ToDouble(Me.Page.Request.Browser.Version, enUSCulture) >= 5.5 Then
                            Dim iframe As New System.Web.UI.HtmlControls.HtmlGenericControl
                            iframe.TagName = "iframe"
                            iframe.Attributes.Add("id", "shim" + mi.MenuID + "-subMenu")
                            iframe.Attributes.Add("src", IFrameSrc)
                            iframe.Attributes.Add("scrolling", "no")
                            iframe.Attributes.Add("frameborder", "no")
                            iframe.Style.Add("position", "absolute")
                            iframe.Style.Add("top", "0px")
                            iframe.Style.Add("left", "0px")
                            iframe.Style.Add("display", "none")
                            BuildOpacity(iframe)
                            Controls.Add(iframe)
                        End If

                        Me._subItemsIds.Add((mi.MenuID + "-subMenu"))
                        AddMenu(mi.MenuID + "-subMenu", mi.SubItems)
                    End If
                End If
            Next i

            Controls.Add(menu)
        End Sub    'AddMenu


        '/ <summary>
        '/ Determines if a user belongs to a role for a particular <see cref="MenuItem"/>.
        '/ </summary>
        '/ <param name="mi">The MenuItem to check.</param>
        '/ <returns><b>true</b> if the user has the right permissions to view this MenuItem; <b>false</b> otherwise.</returns>
        '/ <remarks>UserHasPermission() works by returning <b>true</b> if there are no roles defined for <b>mi</b>
        '/ or if there is at least one role defined for the user and the <b>mi</b> role collection and user role collection
        '/ are <i>not</i> disjoint.</remarks>
        Protected Overridable Function UserHasPermission(ByVal mi As MenuItem) As Boolean
            If mi.Roles.Count = 0 Then
                Return True    ' no roles needed to access mi, return true;
            End If
            If Me._roles.Count = 0 Then
                Return False    ' there are roles needed for mi, but user has no roles
            End If
            Return Not mi.Roles.Disjoint(_roles)    ' allow access if mi's roles and user's roles are non-disjoint
        End Function    'UserHasPermission


        '/ <summary>
        '/ For Internet Explorer 5.5 and up, IFRAMEs are used.  This method returns the client-side JavaScript to show/hide
        '/ the IFRAME.
        '/ </summary>
        '/ <param name="state">A string vaue containing either the string <b>true</b> or <b>false</b>.  A value of
        '/ <b>true</b> makes the IFRAME visible; <b>false</b> hides it.</param>
        '/ <param name="id">The client-side <b>id</b> of the IFRAME.</param>
        '/ <returns>A client-side JavaScript function call that will show/hide the IFRAME.</returns>
        Protected Overridable Function GenerateShimCall(ByVal state As String, ByVal id As String) As String
            Dim enUSCulture As New CultureInfo("en-us", False)
            If Me.Page.Request.Browser.Browser = "IE" AndAlso Convert.ToDouble(Me.Page.Request.Browser.Version, enUSCulture) >= 5.5 Then
                Return "skm_shimSetVisibility(" + state + ",'" + id + "');"
            Else
                Return String.Empty
            End If
        End Function    'GenerateShimCall


        '/ <summary>
        '/ This method is used from the OnDataBinding method; it traverses the XML document,
        '/ building up the object model.
        '/ </summary>
        '/ <param name="itemsNode">The current menuItem XmlNode</param>
        '/ <param name="parentID">The ID of the parent menuItem XmlNode</param>
        '/ <returns>A set of MenuItems for this menu.</returns>
        Protected Overridable Function GatherMenuItems(ByVal itemsNode As XmlNode, ByVal parentID As String) As MenuItemCollection
            ' Make sure we have an XmlNode instance - it should never be null, else the
            ' XML document does not have the expected structure
            If itemsNode Is Nothing Then
                Throw New ArgumentException("The XML data for the Menu control is in an invalid format.")
            End If
            Dim myItems As New MenuItemCollection
            If IsTrackingViewState Then
                CType(myItems, IStateManager).TrackViewState()
            End If
            ' iterate through each MenuItem
            Dim menuItems As XmlNodeList = itemsNode.ChildNodes
            Dim i As Integer
            For i = 0 To menuItems.Count - 1
                Dim menuItem As XmlNode = menuItems(i)

                ' Create the menuitem
                If menuItem.Name = "menuItem" OrElse menuItem.Name = "menuSpacer" Then
                    myItems.Add(BuildMenuItem(menuItem, parentID, i))
                End If
            Next i
            Return myItems
        End Function    'GatherMenuItems

        '/ <summary>
        '/ This method creates a single <see cref="MenuItem"/> and is called repeatedly from <see cref="GatherMenuItems"/>.
        '/ </summary>
        '/ <param name="menuItem">The MenuItem XmlNode.</param>
        '/ <param name="parentID">The parent MenuItem's <see cref="MenuID"/>.</param>
        '/ <param name="indexValue">The ordinal index of the MenuItem in the set of MenuItems.</param>
        '/ <returns>A new <see cref="MenuItem"/> instance.</returns>
        Protected Overridable Function BuildMenuItem(ByVal menuItem As XmlNode, ByVal parentID As String, ByVal indexValue As Integer) As MenuItem
            Dim mi As New MenuItem
            If IsTrackingViewState Then
                CType(mi, IStateManager).TrackViewState()
            End If
            ' Format the indexValue so its three-digits (allows for 1,000 menuitems per (sub)menu
            mi.MenuID = parentID + "-menuItem" + indexValue.ToString("d3")

            ' Build a spacer
            If menuItem.Name = "menuSpacer" Then
                Return BuildMenuSpacer(menuItem, mi)
            End If

            Dim textTextNode As XmlNode = menuItem.SelectSingleNode("text/text()")
            Dim urlTextNode As XmlNode = menuItem.SelectSingleNode("url/text()")
            Dim targetTextNode As XmlNode = menuItem.SelectSingleNode("target/text()")
            Dim commandNameTextNode As XmlNode = menuItem.SelectSingleNode("commandname/text()")
            Dim javascriptCommandTextNode As XmlNode = menuItem.SelectSingleNode("javascriptcommand/text()")
            Dim tooltipTextNode As XmlNode = menuItem.SelectSingleNode("tooltip/text()")
            Dim cssclassTextNode As XmlNode = menuItem.SelectSingleNode("cssclass/text()")
            Dim mouseovercssclassTextNode As XmlNode = menuItem.SelectSingleNode("mouseovercssclass/text()")
            Dim mouseupcssclassTextNode As XmlNode = menuItem.SelectSingleNode("mouseupcssclass/text()")
            Dim mousedowncssclassTextNode As XmlNode = menuItem.SelectSingleNode("mousedowncssclass/text()")
            Dim imageTextNode As XmlNode = menuItem.SelectSingleNode("image/text()")
            Dim imagealttextTextNode As XmlNode = menuItem.SelectSingleNode("imagealttext/text()")
            Dim mouseoverimageTextNode As XmlNode = menuItem.SelectSingleNode("mouseoverimage/text()")
            Dim mouseupimageTextNode As XmlNode = menuItem.SelectSingleNode("mouseupimage/text()")
            Dim mousedownimageTextNode As XmlNode = menuItem.SelectSingleNode("mousedownimage/text()")
            Dim resolveurlTextNode As XmlNode = menuItem.SelectSingleNode("resolveurl/text()")
            Dim enabledTextNode As XmlNode = menuItem.SelectSingleNode("enabled/text()")
            Dim visibleTextNode As XmlNode = menuItem.SelectSingleNode("visible/text()")
            Dim horizontalalignTextNode As XmlNode = menuItem.SelectSingleNode("horizontalalign/text()")
            Dim verticalalignTextNode As XmlNode = menuItem.SelectSingleNode("verticalalign/text()")
            Dim widthTextNode As XmlNode = menuItem.SelectSingleNode("width/text()")
            Dim heightTextNode As XmlNode = menuItem.SelectSingleNode("height/text()")
            Dim leftImageTextNode As XmlNode = menuItem.SelectSingleNode("leftimage/text()")
            Dim rightImageTextNode As XmlNode = menuItem.SelectSingleNode("rightimage/text()")
            Dim rightImageLeftPaddingTextNode As XmlNode = menuItem.SelectSingleNode("rightimageleftpadding/text()")
            Dim leftImageRightPaddingTextNode As XmlNode = menuItem.SelectSingleNode("leftimagerightpadding/text()")
            Dim rightImageAlignTextNode As XmlNode = menuItem.SelectSingleNode("rightimagealign/text()")
            Dim leftImageAlignTextNode As XmlNode = menuItem.SelectSingleNode("leftimagealign/text()")
            Dim backColorTextNode As XmlNode = menuItem.SelectSingleNode("backcolor/text()")
            Dim borderColorTextNode As XmlNode = menuItem.SelectSingleNode("bordercolor/text()")
            Dim borderWidthTextNode As XmlNode = menuItem.SelectSingleNode("borderwidth/text()")
            Dim rolesTextNode As XmlNode = menuItem.SelectSingleNode("roles/text()")

            If textTextNode Is Nothing AndAlso imageTextNode Is Nothing Then
                ' whoops, the <text> element is required
                Throw New ArgumentException("The XML data for the Menu control is in an invalid format: missing both the <text> and <image> elements for a <menuItem>.  One of these must be specified.")
            End If
            If Not (textTextNode Is Nothing) Then
                mi.Text = textTextNode.Value
            End If
            If Not (urlTextNode Is Nothing) Then
                mi.Url = urlTextNode.Value
            End If
            If Not (targetTextNode Is Nothing) Then
                mi.Target = targetTextNode.Value
            End If
            If Not (commandNameTextNode Is Nothing) Then
                mi.CommandName = commandNameTextNode.Value
            End If
            If Not (javascriptCommandTextNode Is Nothing) Then
                mi.JavascriptCommand = javascriptCommandTextNode.Value
            End If
            If Not (tooltipTextNode Is Nothing) Then
                mi.ToolTip = tooltipTextNode.Value
            End If
            If Not (backColorTextNode Is Nothing) Then
                mi.BackColor = ColorTranslator.FromHtml(backColorTextNode.Value)
            End If
            If Not (borderColorTextNode Is Nothing) Then
                mi.BorderColor = ColorTranslator.FromHtml(borderColorTextNode.Value)
            End If
            If Not (borderWidthTextNode Is Nothing) Then
                mi.BorderWidth = New Unit(borderWidthTextNode.Value)
            End If
            If Not (cssclassTextNode Is Nothing) Then
                mi.CssClass = cssclassTextNode.Value
            End If
            If Not (mouseovercssclassTextNode Is Nothing) Then
                mi.MouseOverCssClass = mouseovercssclassTextNode.Value
            End If
            If Not (mouseupcssclassTextNode Is Nothing) Then
                mi.MouseUpCssClass = mouseupcssclassTextNode.Value
            End If
            If Not (mousedowncssclassTextNode Is Nothing) Then
                mi.MouseDownCssClass = mousedowncssclassTextNode.Value
            End If
            If Not (imageTextNode Is Nothing) Then
                mi.Image = imageTextNode.Value
            End If
            If Not (imagealttextTextNode Is Nothing) Then
                mi.ImageAltText = imagealttextTextNode.Value
            End If
            If Not (mouseoverimageTextNode Is Nothing) Then
                mi.MouseOverImage = mouseoverimageTextNode.Value
            End If
            If Not (mouseupimageTextNode Is Nothing) Then
                mi.MouseUpImage = mouseupimageTextNode.Value
            End If
            If Not (mousedownimageTextNode Is Nothing) Then
                mi.MouseDownImage = mousedownimageTextNode.Value
            End If
            If Not (resolveurlTextNode Is Nothing) Then
                If resolveurlTextNode.Value.ToLower() = "true" Then
                    mi.ResolveURL = True
                ElseIf resolveurlTextNode.Value.ToLower() = "false" Then
                    mi.ResolveURL = False
                End If
            End If
            If Not (enabledTextNode Is Nothing) Then
                If enabledTextNode.Value.ToLower() = "true" Then
                    mi.Enabled = True
                ElseIf enabledTextNode.Value.ToLower() = "false" Then
                    mi.Enabled = False
                End If
            End If
            If Not (visibleTextNode Is Nothing) Then
                If visibleTextNode.Value.ToLower() = "true" Then
                    mi.Visible = True
                ElseIf visibleTextNode.Value.ToLower() = "false" Then
                    mi.Visible = False
                End If
            End If
            If Not (horizontalalignTextNode Is Nothing) Then
                If horizontalalignTextNode.Value.ToLower() = "center" Then
                    mi.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center
                ElseIf horizontalalignTextNode.Value.ToLower() = "justify" Then
                    mi.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Justify
                ElseIf horizontalalignTextNode.Value.ToLower() = "left" Then
                    mi.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left
                ElseIf horizontalalignTextNode.Value.ToLower() = "right" Then
                    mi.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right
                End If
            End If
            If Not (verticalalignTextNode Is Nothing) Then
                If verticalalignTextNode.Value.ToLower() = "bottom" Then
                    mi.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Bottom
                ElseIf verticalalignTextNode.Value.ToLower() = "middle" Then
                    mi.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Middle
                ElseIf verticalalignTextNode.Value.ToLower() = "top" Then
                    mi.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top
                End If
            End If
            If Not (widthTextNode Is Nothing) Then
                mi.Width = Unit.Pixel(Convert.ToInt32(widthTextNode.Value))
            End If
            If Not (heightTextNode Is Nothing) Then
                mi.Height = Unit.Pixel(Convert.ToInt32(heightTextNode.Value))
            End If
            If Not (leftImageTextNode Is Nothing) Then
                mi.LeftImage = leftImageTextNode.Value
                If Not (leftImageRightPaddingTextNode Is Nothing) Then
                    mi.LeftImageRightPadding = Unit.Pixel(Convert.ToInt32(leftImageRightPaddingTextNode.Value))
                End If
                If Not (leftImageAlignTextNode Is Nothing) Then
                    Select Case leftImageAlignTextNode.Value.ToLower()
                        Case "absbottom"
                            mi.LeftImageAlign = ImageAlign.AbsBottom
                        Case "absmiddle"
                            mi.LeftImageAlign = ImageAlign.AbsMiddle
                        Case "baseline"
                            mi.LeftImageAlign = ImageAlign.Baseline
                        Case "bottom"
                            mi.LeftImageAlign = ImageAlign.Bottom
                        Case "left"
                            mi.LeftImageAlign = ImageAlign.Left
                        Case "middle"
                            mi.LeftImageAlign = ImageAlign.Middle
                        Case "right"
                            mi.LeftImageAlign = ImageAlign.Right
                        Case "texttop"
                            mi.LeftImageAlign = ImageAlign.TextTop
                        Case "top"
                            mi.LeftImageAlign = ImageAlign.Top
                        Case Else
                            mi.LeftImageAlign = ImageAlign.NotSet
                    End Select
                End If
            ElseIf Not (rightImageTextNode Is Nothing) Then
                mi.RightImage = rightImageTextNode.Value
                If Not (rightImageLeftPaddingTextNode Is Nothing) Then
                    mi.RightImageLeftPadding = Unit.Pixel(Convert.ToInt32(rightImageLeftPaddingTextNode.Value))
                End If
                If Not (rightImageAlignTextNode Is Nothing) Then
                    Select Case rightImageAlignTextNode.Value.ToLower()
                        Case "absbottom"
                            mi.RightImageAlign = ImageAlign.AbsBottom
                        Case "absmiddle"
                            mi.RightImageAlign = ImageAlign.AbsMiddle
                        Case "baseline"
                            mi.RightImageAlign = ImageAlign.Baseline
                        Case "bottom"
                            mi.RightImageAlign = ImageAlign.Bottom
                        Case "left"
                            mi.RightImageAlign = ImageAlign.Left
                        Case "middle"
                            mi.RightImageAlign = ImageAlign.Middle
                        Case "right"
                            mi.RightImageAlign = ImageAlign.Right
                        Case "texttop"
                            mi.RightImageAlign = ImageAlign.TextTop
                        Case "top"
                            mi.RightImageAlign = ImageAlign.Top
                        Case Else
                            mi.RightImageAlign = ImageAlign.NotSet
                    End Select
                End If
            End If

            If Not (rolesTextNode Is Nothing) Then    ' add the roles
                mi.Roles.AddRange(rolesTextNode.Value.Split(New Char() {","c}))
            End If
            ' see if there is a submenu
            Dim subMenu As XmlNode = menuItem.SelectSingleNode("subMenu")
            If Not (subMenu Is Nothing) Then
                ' Recursively processes the <menuItem>'s <subMenu> node, if present
                mi.SubItems.AddRange(GatherMenuItems(subMenu, mi.MenuID + "-subMenu"))
            End If

            Return mi
        End Function    'BuildMenuItem


        '/ <summary>
        '/ Creates a MenuItem spacer.
        '/ </summary>
        Protected Overridable Function BuildMenuSpacer(ByVal menuItem As XmlNode, ByVal mi As MenuItem) As MenuItem
            Dim children As XmlNodeList = menuItem.ChildNodes

            mi.Text = String.Empty
            mi.MenuType = MenuItemType.MenuSeparator

            Dim i As Integer
            For i = 0 To children.Count - 1
                Dim node As XmlNode = children(i)
                Select Case node.Name
                    Case "cssclass"
                        mi.CssClass = node.InnerText
                    Case "height"
                        mi.Height = Unit.Parse(node.InnerText)
                    Case "spacermarkup"
                        mi.Text = node.InnerText
                End Select
            Next i

            Return mi
        End Function    'BuildMenuSpacer
#End Region


#Region "Client Script Methods"
        '/ <summary>
        '/ A helper method that determines if an external script is being used.
        '/ </summary>
        '/ <remarks><b>UseExternalScript</b> simply returns the result of the check <b>ExternalScriptUrl != null</b>.</remarks>

        Protected Overridable ReadOnly Property UseExternalScript() As Boolean
            Get
                Return Not (ExternalScriptUrl Is Nothing)
            End Get
        End Property


        '/<summary>
        '/Return the configured URL for the external script.
        '/</summary>
        '/<remarks>
        '/ The external JavaScript file maybe specified either in the <b>Web.config</b> file or through the
        '/ Menu control's <see cref="ScriptPath"/> property.  <b>ExternalScriptUrl checks first the <b>Web.config</b>
        '/ file and then the <see cref="ScriptPath"/> property to determine if an external JavaScript file should be used.</b>
        '/</remarks>

        Protected Overridable ReadOnly Property ExternalScriptUrl() As String
            Get
                Dim config As NameValueCollection = ConfigurationSettings.GetConfig("skmMenu")    '
                If Not (config Is Nothing) Then
                    Return config("ExternalScriptUrl")
                ElseIf Me.ScriptPath <> String.Empty Then
                    Return Me.ScriptPath
                Else
                    Return Nothing
                End If
            End Get
        End Property



        '/ <summary>
        '/ Registers the main client script.
        '/ </summary>
        '/ <remarks>RegisterClientScriptBlock() adds the global variables and JavaScript methods.  Depending on
        '/ how skmMenu is configured, this client-side script is either emitted directly to the browser in the
        '/ HTML stream, or references an external JavaScript file.  It is recommended that the external JavaScript
        '/ file approach be used, since it yields better performance on both the client and server.<p />
        '/ For more information on using an external JavaScript file, see the <see cref="ScriptPath"/> property.</remarks>
        Protected Overridable Sub RegisterClientScriptBlock()
            If Not Me.Page.IsClientScriptBlockRegistered("skmMenu") Then
                Dim script As String
                If UseExternalScript Then    ' we have an external JavaScript file reference.
                    Dim scriptBuilder As New StringBuilder
                    scriptBuilder.AppendFormat("<script language=""javascript"" src=""{0}""></script>", Me.ResolveUrl(ExternalScriptUrl))
                    script = scriptBuilder.ToString()
                Else
                    ' Need to load the script from the assembly's resources.
                    ' If you are working on the skmMenu code base, this is in Menu.resx...
                    Dim manager As New ResourceManager(Me.GetType())
                    script = manager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, True, True).GetString("ClientScript")
                End If
                Me.Page.RegisterClientScriptBlock("skmMenu", script)
            End If
        End Sub    'RegisterClientScriptBlock



        '/<summary>
        '/Registers the startup client script.
        '/</summary>
        '/<remarks>The startup script involves calls to the client-side <b>skm_registerMenu()</b> function.
        '/</remarks>
        Protected Overridable Sub RegisterClientStartupScript()
            Dim script As New StringBuilder
            script.Append("<script language=""javascript"">")
            script.Append("skm_registerMenu('")
            script.Append(Me.ClientID)
            script.Append("',")
            script.Append(InstantiateStyleInfoJavascript(Me.SelectedMenuItemStyle))
            script.Append(","c)
            script.Append(InstantiateStyleInfoJavascript(Me.UnselectedMenuItemStyle))
            script.Append(","c)
            script.Append(Me.MenuFadeDelay)
            script.Append(","c)
            script.Append(Me.HighlightTopMenu.ToString().ToLower())
            script.Append(");")
            script.Append("</script>")

            ' It is vital that Page.RegisterStartupScript be used so that we can be guaranteed
            ' that this initialization code appears *AFTER* the methods registerd in the
            ' RegisterClientScriptBlock() method...
            Page.RegisterStartupScript(Me.ClientID, script.ToString())
        End Sub    'RegisterClientStartupScript



        '/<summary>
        '/Registers the SubMenus for the menu with a JavaScript array.
        '/</summary>
        '/<remarks>A client-side array <b>skm_subMenuIDs</b> maintains an array of all of the <b>id</b> values
        '/of the client-side submenus.  This is useful in dynamically determining the client position of a submenu.</remarks>
        Protected Overridable Sub RegisterSubmenuArray()
            Dim i As Integer
            For i = 0 To (Me._subItemsIds.Count) - 1
                Page.RegisterArrayDeclaration("skm_subMenuIDs", "'" + CStr(_subItemsIds(i)) + "'")
            Next i

            'This is a hacky fix for the case where no submenus are specified
            'must do this properly later
            Page.RegisterArrayDeclaration("skm_subMenuIDs", "'" + Me.ClientID + "'")
        End Sub    'RegisterSubmenuArray



        '/<summary>
        '/Registers the Preload command to preload any images.
        '/</summary>
        '/<remarks>Preloading images used in a page improves the user experience by loading an image before it is needed.
        '/If a menu has rollover images and does <b>not</b> preload its images, there will be a short delay when the
        '/user mouses over the menu item for the first time, as the image must then be downloaded.  Preloading the images
        '/removes this delay since the image will have likely been downloaded by the time the user mouses over the menu
        '/image.<p />For more information on preloading JavaScript images refer to <a href="http://javascript.internet.com/miscellaneous/preload-images.html">this article</a>.</remarks>
        Protected Overridable Sub RegisterPreloadCommand()
            If Me._imagePreload.ToString() <> String.Empty Then
                ' Again, this must use RegisterStartupScript since it relies on the preloadimages() JavaScript
                ' method - that is, it must appear *after* preloadimages() is defined...
                Page.RegisterStartupScript(Me.ClientID + "_preload", "<script language=""javascript"">" + "preloadimages(" + Me._imagePreload.ToString() + ");</script>")
            End If
        End Sub    'RegisterPreloadCommand



        '/<summary>
        '/Returns the javascript that instantiates a style info object
        '/that corresponds to the style specified.
        '/</summary>
        Protected Overridable Function InstantiateStyleInfoJavascript(ByVal style As Style) As String
            Dim sb As New StringBuilder
            sb.Append("new skm_styleInfo('")
            sb.Append(ColorTranslator.ToHtml(style.BackColor))
            sb.Append("','")
            sb.Append(ColorTranslator.ToHtml(style.BorderColor))
            sb.Append("','")
            sb.Append(IIf(style.BorderStyle = BorderStyle.NotSet, String.Empty, style.BorderStyle.ToString()))
            sb.Append("','")
            sb.Append(style.BorderWidth.ToString())
            sb.Append("','")
            sb.Append(ColorTranslator.ToHtml(style.ForeColor))
            sb.Append("','")
            sb.Append(String.Join(",", style.Font.Names))
            sb.Append("','")
            sb.Append(style.Font.Size.ToString())
            sb.Append("','")
            sb.Append(IIf(style.Font.Italic, "italic", String.Empty))
            sb.Append("','")
            sb.Append(IIf(style.Font.Bold, "bold", String.Empty))
            sb.Append("','")
            sb.Append(style.CssClass)
            sb.Append("')")

            Return sb.ToString()
        End Function    'InstantiateStyleInfoJavascript

        Sub RaisePostBackEvent(ByVal eventArgument As String) Implements System.Web.UI.IPostBackEventHandler.RaisePostBackEvent
            OnMenuItemClick(New MenuItemClickEventArgs(eventArgument))
        End Sub    'IPostBackEventHandler.RaisePostBackEvent
#End Region

#Region "Menu Properties"
        '/ <summary>
        '/ Sets or gets the name of the XML file or XmlDocument object that is the datasource for the menu.
        '/ </summary>
        '/ <remarks>The <b>DataSource</b> can be assigned either a string filename to an XML file, or an
        '/ XmlDocument object.  Attempting to assign the <b>DataSource</b> to something other than a string or
        '/ XmlDocument will result in an <b>ArgumentException</b> being thrown.</remarks>

        Public Property DataSource() As Object
            Get
                Return Me._dataSource
            End Get
            Set(ByVal Value As Object)
                If TypeOf Value Is String OrElse TypeOf Value Is XmlDocument Then
                    Me._dataSource = Value
                Else
                    Throw New ArgumentException("DataSource must be a string or XmlDocument instance.")
                End If
            End Set
        End Property

        '/ <summary>
        '/ Returns the Menu's top-level <see cref="MenuItem"/>s.
        '/ </summary>
        '/ <remarks>This returns a <see cref="MenuItemCollection"/> instance holding the top-level menu items only.
        '/ If you want to drill down into submenu items, you must programmatically recurse down.  For example,
        '/ imagine we had a menu with two top-level menu items, File and Help, and File had three menu items,
        '/ New, Open, and Save.  To programmatically access the New menu item from the Menu you'd use:
        '/ <code>
        '/  MenuItem FileNewMenuItem = MenuControlID.Items[0].Items[0];
        '/ </code></remarks>

        <Browsable(False), EditorAttribute(GetType(System.ComponentModel.Design.CollectionEditor), GetType(UITypeEditor))> _
        Public ReadOnly Property Items() As MenuItemCollection
            Get
                If Me.IsTrackingViewState Then
                    CType(_items, IStateManager).TrackViewState()
                End If
                Return Me._items
            End Get
        End Property

        '/ <summary>
        '/ Specifies the roles the current user belongs to.  When assigning roles to the MenuItems, the user's roles
        '/ affect what menu items are displayed.
        '/ </summary>
        '/ <remarks>
        '/ Each MenuItem in a skmMenu can be assigned a set of <i>roles</i>.  A <i>role</i> is a name that implies some
        '/ level of access.  Roles are denoted as strings.  Example roles might be: developer, tester, and admin.<p />
        '/ To denote a role for a <see cref="MenuItem"/>, the <see cref="MenuItem.Roles"/> property can be used programmatically,
        '/ <i>or</i> the &lt;roles&gt; XML element can be used if the menu data is supplied in an XML file.  (For more
        '/ information on the &lt;roles&gt; XML element and binding menu data via an XML file, consult
        '/ <a href="http://skmmenu.com/menu/Download/XMLStructure.html">http://skmmenu.com/menu/Download/XMLStructure.html</a>.<p />
        '/ The Menu class's <b>UserRoles</b> property specifies the set of roles the user viewing the page has.  This
        '/ needs to be set programmatically in the first page load (the role information is persisted across postbacks).
        '/ Typically this role assignment will need to be done by looking up the current user's set of roles in a database
        '/ or some other ACL store.<p />
        '/ After the <b>UserRoles</b> property has been set, you can bind the XML data to skmMenu through a call to
        '/ <see cref="DataBind"/>.  Only those <see cref="MenuItem"/>s that either (a) have no roles defined or (b) has a role
        '/ collection that intersects the <b>UserRoles</b> role collection, will be rendered.  For a live demo of
        '/ roles with skmMenu, see <a href="http://skmmenu.com/menu/Examples/Roles.aspx">http://skmmenu.com/menu/Examples/Roles.aspx</a>.
        '/ </remarks>

        <Browsable(False)> _
        Public ReadOnly Property UserRoles() As RoleCollection
            Get
                If Me.IsTrackingViewState Then
                    CType(_items, IStateManager).TrackViewState()
                End If
                Return Me._roles
            End Get
        End Property


        '/ <summary>
        '/ Specifies the style for selected <see cref="MenuItem"/>s.
        '/ </summary>
        '/ <remarks>A MenuItem is <i>selected</i> when the user moves the mouse over the menu item.</remarks>

        <Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies the style for selected menuitems.")> _
        Public ReadOnly Property SelectedMenuItemStyle() As TableItemStyle
            Get
                If Me.IsTrackingViewState Then
                    CType(Me._selectedMenuItemStyle, IStateManager).TrackViewState()
                End If
                Return Me._selectedMenuItemStyle
            End Get
        End Property


        '/ <summary>
        '/ Specifies whether URL's should be resolved before being output.
        '/ </summary>
        '/ <remarks>If this property is marked <b>True</b> then MenuItem URLs can use ~, as in ~/SomeDir/SomePage.htm.  Internally, if this
        '/ property is set to <b>True</b> the <b>ResolveUrl()</b> method is used.</remarks>

        <Category("Behavior"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies whether URL's should be resolved before being output.")> _
        Public Property DefaultResolveURL() As Boolean
            Get
                Dim o As Object = ViewState("MenuDefaultResolveURL")
                If o Is Nothing Then
                    Return False
                Else
                    Return CBool(o)
                End If
            End Get
            Set(ByVal Value As Boolean)
                ViewState("MenuDefaultResolveURL") = Value
            End Set
        End Property


        '/ <summary>
        '/ Specifies whether the top level menu should remain highlighted when the submenu beneath is displayed.
        '/ </summary>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies whether the top level menu should remain highlighted when the submenu beneath is displayed.")> _
        Public Property HighlightTopMenu() As Boolean
            Get
                Dim o As Object = ViewState("MenuHighlightTopMenu")
                If o Is Nothing Then
                    Return False
                Else
                    Return CBool(o)
                End If
            End Get
            Set(ByVal Value As Boolean)
                ViewState("MenuHighlightTopMenu") = Value
            End Set
        End Property


        '/ <summary>
        '/ Specifies whether a submenu is displayed on mouse over or when clicked.
        '/ </summary>
        '/ <value>A value of <b>True</b> indicates that submenus are displayed with a menu item is clicked;
        '/ a value of <b>False</b> indicates that the submenu is displayed when the menu item is moused over.</value>

        <Category("Behavior"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies whether a submenu is displayed on mouse over or when clicked."), DefaultValue("false")> _
        Public Property ClickToOpen() As Boolean
            Get
                Dim o As Object = ViewState("MenuClickToOpen")
                If o Is Nothing Then
                    Return False
                Else
                    Return CBool(o)
                End If
            End Get
            Set(ByVal Value As Boolean)
                ViewState("MenuClickToOpen") = Value
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the z-index style value for the menu.
        '/ </summary>
        '/ <remarks>For Internet Explorer, submenus are displayed using IFRAMEs.  Using IFRAMEs keeps the menu
        '/ above other form elements, such as drop-down lists.  The <b>zIndex</b> specifies the "height" of the IFRAME
        '/ relative to othjer elements on the page.  The default value of 1000 should ensure that the menus are
        '/ always the top-most elements on the page.</remarks>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The z-index property for the menu."), NotifyParentProperty(True), DefaultValue("1000")> _
        Public Property zIndex() As Integer
            Get
                Dim o As Object = ViewState("ItemzIndex")
                If Not (o Is Nothing) Then
                    Return CInt(o)
                Else
                    Return 1000
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("MenuzIndex") = Value
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the opacity style value for the menu.
        '/ </summary>
        '/ <remarks>Opacity values must be between 0 and 100.<p />
        '/ The opacity property only works for select Web browsers: IE 5.0+, Netscape 6+ and Mozilla.</remarks>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The opacity property only works IE 5.0+, Netscape 6+ and Mozilla."), NotifyParentProperty(True), DefaultValue("")> _
        Public Property Opacity() As String
            Get
                Dim o As Object = ViewState("MenuOpacity")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal Value As String)
                ' Conversion in order to cause error if non-numeric
                Dim opacityvalue As Integer = Convert.ToInt32(Value)

                If opacityvalue > 100 OrElse opacityvalue < 0 Then
                    Throw New ArgumentOutOfRangeException("Opacity can not be greater than 100 or less than 0.")
                End If
                ViewState("MenuOpacity") = Value
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the value for css class used for SubMenus.
        '/ </summary>
        '/ <remarks>If <b>SubMenuCssClass</b> is not specified and <see cref="CssClass"/> is specified, CssClass is 
        '/ used instead.</remarks>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The value used for the stylesheet class for SubMenus.  If not specified and CssClass is specified, CssClass is used instead."), NotifyParentProperty(True), DefaultValue("")> _
        Public Property SubMenuCssClass() As String
            Get
                Dim o As Object = ViewState("MenuSubMenuCssClass")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("MenuSubMenuCssClass") = Value
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the default CSS class name for the <see cref="MenuItem"/>'s CSS class.
        '/ </summary>
        '/ <remarks>If a <see cref="MenuItem"/> has no specified CSS class, the <b>DefaultCssClass</b> is used instead.</remarks>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The default for a menuitem's optional stylesheet class if the menuitem doesn't specify a value."), NotifyParentProperty(True), DefaultValue("")> _
        Public Property DefaultCssClass() As String
            Get
                Dim o As Object = ViewState("MenuDefaultCssClass")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("MenuDefaultCssClass") = Value
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the default value for the menuitem's mouse over css class.  Used when a menuitem has no value specified.
        '/ </summary>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The default for a menuitem's mouse over stylesheet class if the menuitem doesn't specify a value."), NotifyParentProperty(True), DefaultValue("")> _
        Public Property DefaultMouseOverCssClass() As String
            Get
                Dim o As Object = ViewState("MenuDefaultMouseOverCssClass")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("MenuDefaultMouseOverCssClass") = Value
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the default value for the menuitem's mouse down css class.  Used when a menuitem has no value specified.
        '/ </summary>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The default for a menuitem's mouse down stylesheet class if the menuitem doesn't specify a value."), NotifyParentProperty(True), DefaultValue("")> _
        Public Property DefaultMouseDownCssClass() As String
            Get
                Dim o As Object = ViewState("MenuDefaultMouseDownCssClass")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("MenuDefaultMouseDownCssClass") = Value
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the default value for the menuitem's mouse up css class.  Used when a menuitem has no value specified.
        '/ </summary>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The default for a menuitem's mouse up stylesheet class if the menuitem doesn't specify a value."), NotifyParentProperty(True), DefaultValue("")> _
        Public Property DefaultMouseUpCssClass() As String
            Get
                Dim o As Object = ViewState("MenuDefaultMouseUpCssClass")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("MenuDefaultMouseUpCssClass") = Value
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the path to an external JavaScript file.  If no external path is specified, the JavaScript is
        '/ rendered by the control directly in the page.
        '/ </summary>
        '/ <remarks>It is highly recommended that you utilize an external JavaScript file, as it promises performance increases
        '/ both for the client and server.  The client can cache an external JavaScript file, thereby reducing the bandwidth
        '/ needed to be downloaded.  On the server side, using an external JavaScript file results in several dozen lines
        '/ of JavaScript code not needing to be rendered.<p />To use an external JavaScript file, place the <b>skmMenu.js</b> file
        '/ that was included in the download in a directory in your Web site.  You can then set this property to the
        '/ path of the <b>skmMenu.js</b> file.</remarks>
        '/ <example>
        '/ [C#] This example assumes that the <b>skmMenu.js</b> file was placed in the /scripts/ directory of your Web site.
        '/ <code>
        '/ protected Menu myMenuControl;
        '/ 
        '/ private void Page_Load(object sender, EventArgs e)
        '/ {
        '/   myMenuControl.ScriptPath = "/scripts/skmMenu.js";
        '/   ...
        '/ }
        '/ </code>
        '/ </example>

        <PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies external path and filename for javascript file."), DefaultValue(""), EditorAttribute(GetType(UrlEditor), GetType(UITypeEditor))> _
        Public Property ScriptPath() As String
            Get
                Dim o As Object = ViewState("MenuScriptPath")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("MenuScriptPath") = Value
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the default target for links in menuitems.  Used if no target property is specified for a menuitem.
        '/ </summary>

        <PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies default target for links in menuitems - if no target property specified for a menuitem."), DefaultValue("")> _
        Public Property DefaultTarget() As String
            Get
                Dim o As Object = ViewState("MenuDefaultTarget")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("MenuDefaultTarget") = Value
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets padding for each menuitem (pixels).
        '/ </summary>
        '/ <remarks>Menus and submenus are rendered as HTML tables.  This property, then, specifies the rendered
        '/ table's <b>CellPadding</b>.</remarks>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), DefaultValue(2), Description("Specifies padding for each menuitem.")> _
        Public Property ItemPadding() As Integer
            Get
                Dim o As Object = ViewState("MenuItemPadding")
                If Not (o Is Nothing) Then
                    Return CInt(o)
                Else
                    Return 2
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("MenuItemPadding") = Value
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the spacing for each menuitem (pixels).
        '/ </summary>
        '/ <remarks>Menus and submenus are rendered as HTML tables.  This property, then, specifies the rendered
        '/ table's <b>CellSpacing</b>.</remarks>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), DefaultValue(2), Description("Specifies spacing for each menuitem.")> _
        Public Property ItemSpacing() As Integer
            Get
                Dim o As Object = ViewState("MenuItemSpacing")
                If Not (o Is Nothing) Then
                    Return CInt(o)
                Else
                    Return 2
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("MenuItemSpacing") = Value
            End Set
        End Property


        '/ <summary>
        '/ Specifies the style for unselected <see cref="MenuItem"/>s.
        '/ </summary>
        '/ <remarks>An <i>unselected</i> MenuItem is one that does <b>not</b> have the mouse cursor hovering over it.</remarks>

        <Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies the style for unselected menuitems.")> _
        Public ReadOnly Property UnselectedMenuItemStyle() As TableItemStyle
            Get
                If Me.IsTrackingViewState Then
                    CType(Me._unselectedMenuItemStyle, IStateManager).TrackViewState()
                End If
                Return Me._unselectedMenuItemStyle
            End Get
        End Property


        '/ <summary>
        '/ Sets or Gets the menu's layout direction.
        '/ </summary>
        '/ <value>A <see cref="MenuLayout"/> enumeration value.  The default is <b>Vertical</b></value>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Specifies the menu layout direction."), DefaultValue(MenuLayout.Vertical)> _
        Public Property Layout() As MenuLayout
            Get
                Dim o As Object = ViewState("MenuLayout")
                If o Is Nothing Then
                    Return MenuLayout.Vertical
                Else
                    Return CType(o, MenuLayout)
                End If
            End Get
            Set(ByVal Value As MenuLayout)
                ViewState("MenuLayout") = Value
            End Set
        End Property


        '/ <summary>
        '/ Sets or Gets the menu's mouse cursor.
        '/ </summary>
        '/ <value>A <see cref="MouseCursor"/> enumeration value.  The default is <b>MouseCursor.Default</b>.</value>
        '/ <remarks>Setting the <b>Cursor</b> property to <b>MouseCursor.Pointer</b> will have the browser display
        '/ a mouse pointer when the mouse is above a clickable menu item.</remarks>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Specifies the menu mouse cursor for items with an associated URL or command.")> _
        Public Property Cursor() As MouseCursor
            Get
                Dim o As Object = ViewState("MenuMouseCursor")
                If o Is Nothing Then
                    Return MouseCursor.Default
                Else
                    Return CType(o, MouseCursor)
                End If
            End Get
            Set(ByVal Value As MouseCursor)
                ViewState("MenuMouseCursor") = Value
            End Set
        End Property


        '/ <summary>
        '/ Sets or Gets the menu's gridline property.
        '/ </summary>
        '/ <value>This property is set with one of the <b>GridLines</b> enumeration values.  The default is <b>None</b>.</value>
        '/ <remarks>
        '/ Menus and submenus are rendered as HTML tables.  This property, then, specifies the rendered
        '/ table's <b>GridLines</b> property value.
        '/ The following table lists the possible values:
        '/ <list type="table">
        '/		<listheader><term>Value</term><term>Description</term></listheader>
        '/		<item><term>None</term><description>No cell border is displayed.</description></item>
        '/		<item><term>Horizontal</term><description>Only the upper and lower borders of the cells in a data listing control are displayed.</description></item>
        '/		<item><term>Vertical</term><description>Only the left and right borders of the cells in the data list control are displayed.</description></item>
        '/		<item><term>Both</term><description>All borders of the cells in a data listing control are displayed.</description></item>
        '/ </list>
        '/ </remarks>

        <Category("Appearance"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Specifies the menu gridline setting.")> _
        Public Property GridLines() As GridLines
            Get
                Dim o As Object = ViewState("MenuGridLines")
                If o Is Nothing Then
                    Return GridLines.None
                Else
                    Return CType(o, GridLines)
                End If
            End Get
            Set(ByVal Value As GridLines)
                ViewState("MenuGridLines") = Value
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the number of half seconds to display the menu after the user's mouse
        '/ has left the menu, before hiding the menu.
        '/ </summary>
        '/ <value>The number of half-seconds to delay after the user's mouse has left the menu before hiding the menu.  Must have an integer value greater than
        '/ or equal to 0.  The default is 2, causing the submenus to delay for one second before vanishing.</value>
        '/ <remarks>It is recommended that this property have at least a value of 1.  Setting it to 0 will cause the
        '/ submenu to disappear <b>immediately</b> after the user's mouse cursor leaves the submenu area.  This can
        '/ result in a frustrating user experience.</remarks>

        <Category("Behavior"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Specifies the number of half seconds it takes an inactive menu to fade."), DefaultValue(2)> _
        Public Property MenuFadeDelay() As Integer
            Get
                Dim o As Object = ViewState("MenuFadeDelay")
                If o Is Nothing Then
                    Return 2
                Else
                    Return CInt(ViewState("MenuFadeDelay"))
                End If
            End Get
            Set(ByVal Value As Integer)
                If Value < 0 Then
                    Throw New ArgumentException("MenuFadeDelay must have a value greater than or equal to 0.")
                End If
                ViewState("MenuFadeDelay") = Value
            End Set
        End Property


        '/ <summary>
        '/ Specifies what SRC property should be loaded into an IFRAME.
        '/ </summary>
        '/ <remarks>When the user is using Internet Explorer to render a page with skmMenu on it, the skmMenu submenus
        '/ are rendered as IFRAMEs.  By default, the SRC is a blank string.  If you are using skmMenu on SSL, however, this
        '/ blank SCR will cause a dialog box to appear warning the user that both secure and non-secure items are
        '/ being displayed.  To circumvent this, you need to create a blank page, like blank.html.  Configure this
        '/ property so that the SCR of the IFRAME is then blank.html.  This is only needed when using skmMenu over
        '/ SSL.</remarks>

        <Category("Behavior"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies what SRC property should be loaded into an IFRAME."), DefaultValue("")> _
        Public Property IFrameSrc() As String
            Get
                Dim o As Object = ViewState("MenuIFrameSrc")
                If o Is Nothing Then
                    Return String.Empty
                Else
                    Return CStr(o)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("MenuIFrameSrc") = Value
            End Set
        End Property
#End Region

    End Class 'Menu
End Namespace