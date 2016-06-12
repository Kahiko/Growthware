Imports System.Drawing
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Common.CustomWebControls.Menu

Namespace CustomWebControls.Menu
    '/ <summary>
    '/ A MenuItem represents a single item in a menu.</summary>
    '/ <remarks>A MenuItem is a single "item" in a menu.  Typically a MenuItem will have some <see cref="Text"/>
    '/ associated with it, and often a <see cref="Url"/> or <see cref="CommandName"/>.  MenuItems can also optionally
    '/ have a set of <see cref="SubItems"/>, which represents a nested submenu.</remarks>
    <ToolboxItem(False), Serializable()> _
       Public Class MenuItem
        Inherits WebControl
        Implements IStateManager

#Region "Private Member Variables"
        ' private member variables
        Private _subItems As New MenuItemCollection
        Private _roles As New RoleCollection
#End Region

#Region "Contructors"

        '/ <summary>
        '/ Creates a new MenuItem instance.
        '/ </summary>
        Public Sub New()    ' empty, default constructor
        End Sub    'New
        '/ <summary>
        '/ Creates a new MenuItem instance.
        '/ </summary>
        Public Sub New(ByVal itemText As String)
            MyClass.New(itemText, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
        End Sub    'New
        '/ <summary>
        '/ Creates a new MenuItem instance.
        '/ </summary>
        Public Sub New(ByVal itemText As String, ByVal itemUrl As String)
            MyClass.New(itemText, itemUrl, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
        End Sub    'New
        '/ <summary>
        '/ Creates a new MenuItem instance.
        '/ </summary>
        Public Sub New(ByVal itemText As String, ByVal itemUrl As String, ByVal itemToolTip As String)
            MyClass.New(itemText, itemUrl, itemToolTip, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
        End Sub    'New
        '/ <summary>
        '/ Creates a new MenuItem instance.
        '/ </summary>
        Public Sub New(ByVal itemText As String, ByVal itemUrl As String, ByVal itemToolTip As String, ByVal itemCssClass As String)
            MyClass.New(itemText, itemUrl, itemToolTip, itemCssClass, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
        End Sub    'New
        '/ <summary>
        '/ Creates a new MenuItem instance.
        '/ </summary>
        Public Sub New(ByVal itemText As String, ByVal itemUrl As String, ByVal itemToolTip As String, ByVal itemCssClass As String, ByVal itemMouseOverCssClass As String)
            MyClass.New(itemText, itemUrl, itemToolTip, itemCssClass, itemMouseOverCssClass, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
        End Sub    'New
        '/ <summary>
        '/ Creates a new MenuItem instance.
        '/ </summary>
        Public Sub New(ByVal itemText As String, ByVal itemUrl As String, ByVal itemToolTip As String, ByVal itemCssClass As String, ByVal itemMouseOverCssClass As String, ByVal itemMouseDownCssClass As String, ByVal itemMouseUpCssClass As String)
            MyClass.New(itemText, itemUrl, itemToolTip, itemCssClass, itemMouseOverCssClass, itemMouseUpCssClass, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
        End Sub    'New
        '/ <summary>
        '/ Creates a new MenuItem instance.
        '/ </summary>
        Public Sub New(ByVal itemText As String, ByVal itemUrl As String, ByVal itemToolTip As String, ByVal itemCssClass As String, ByVal itemMouseOverCssClass As String, ByVal itemMouseDownCssClass As String, ByVal itemMouseUpCssClass As String, ByVal itemImage As String, ByVal itemMouseOverImage As String)
            MyClass.New(itemText, itemUrl, itemToolTip, itemCssClass, itemMouseOverCssClass, itemMouseUpCssClass, itemMouseOverImage, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
        End Sub    'New
        '/ <summary>
        '/ Creates a new MenuItem instance.
        '/ </summary>
        Public Sub New(ByVal itemText As String, ByVal itemUrl As String, ByVal itemToolTip As String, ByVal itemCssClass As String, ByVal itemMouseOverCssClass As String, ByVal itemMouseDownCssClass As String, ByVal itemMouseUpCssClass As String, ByVal itemImage As String, ByVal itemMouseOverImage As String, ByVal itemMouseDownImage As String, ByVal itemMouseUpImage As String, ByVal itemName As String)
            If itemText = String.Empty OrElse itemText Is Nothing AndAlso (itemImage = String.Empty OrElse itemImage Is Nothing) Then
                Throw New ArgumentException("Either itemText or itemImage must be specified.")
            End If
            [Text] = itemText
            Url = itemUrl
            ToolTip = itemToolTip
            CssClass = itemCssClass
            MouseOverCssClass = itemMouseOverCssClass
            MouseDownCssClass = itemMouseDownCssClass
            MouseUpCssClass = itemMouseUpCssClass
            Image = itemImage
            MouseOverImage = itemMouseOverImage
            MouseDownImage = itemMouseDownImage
            MouseUpImage = itemMouseUpImage
            Name = itemName
            MenuType = MenuItemType.MenuItem
        End Sub    'New
#End Region

#Region "IStateManager Implementation"

        '/ <summary>
        '/ This method saves the state for a particular menuitem.</summary>
        '/ <remarks>This method returns a Triplet, where the first item is the result from the MenuItem's ViewState's
        '/ SaveViewState() method call.  The second is the ViewState saved from the
        '/ subItems MenuItemCollection instance.  The third item is the ViewState saved from the
        '/ RolesCollection.
        '/ </remarks>
        '/ <returns>A Triplet containing the ViewState and state of the subitems.</returns>
        Shadows Function SaveViewState() As Object Implements IStateManager.SaveViewState
            Dim baseState As Object = CType(Me.ViewState, IStateManager).SaveViewState()
            Dim subItemsState As Object = CType(Me._subItems, IStateManager).SaveViewState()
            Dim rolesState As Object = CType(Me._roles, IStateManager).SaveViewState()

            If baseState Is Nothing AndAlso subItemsState Is Nothing AndAlso rolesState Is Nothing Then
                Return Nothing
            Else
                Return New Triplet(baseState, subItemsState, rolesState)
            End If
        End Function    'IStateManager.SaveViewState

        '/ <summary>
        '/ Loads the state upon postback back into the MenuItem.
        '/ </summary>
        '/ <param name="savedState">The state preserved from the SaveViewState() method in the
        '/ previous page invocation.</param>
        Shadows Sub LoadViewState(ByVal savedState As Object) Implements IStateManager.LoadViewState
            If Not (savedState Is Nothing) Then
                Dim t As Triplet = CType(savedState, Triplet)
                If Not (t.First Is Nothing) Then
                    CType(Me.ViewState, IStateManager).LoadViewState(t.First)
                End If
                If Not (t.Second Is Nothing) Then
                    CType(Me._subItems, IStateManager).LoadViewState(t.Second)
                End If
                If Not (t.Third Is Nothing) Then
                    CType(Me._roles, IStateManager).LoadViewState(t.Third)
                End If
            End If
        End Sub    'IStateManager.LoadViewState

        '/ <summary>
        '/ Starts tracking view state for the ViewState and subItems properties.
        '/ </summary>
        Shadows Sub TrackViewState() Implements IStateManager.TrackViewState
            MyBase.TrackViewState()

            If Not (_subItems Is Nothing) Then
                CType(_subItems, IStateManager).TrackViewState()
            End If
            If Not (_roles Is Nothing) Then
                CType(_roles, IStateManager).TrackViewState()
            End If
        End Sub    'IStateManager.TrackViewState
#End Region

#Region "MenuItem Properties"
#Region "Web Control Properties"
        '/ <summary>
        '/ Gets or sets a value indicating whether the Web server control is enabled.
        '/ </summary>
        '/ <value><b>true</b> if the control is enabled, <b>false</b> otherwise; the default is <b>true</b>.</value>

        Public Shadows Property Enabled() As Boolean
            Get
                Dim o As Object = ViewState("MenuItemEnabled")

                If Not (o Is Nothing) Then
                    Return CBool(o)
                Else
                    Return True
                End If
            End Get
            Set(ByVal Value As Boolean)
                ViewState("MenuItemEnabled") = Value
                ViewState.SetItemDirty("MenuItemEnabled", True)
            End Set
        End Property

        '/ <summary>
        '/ Gets or sets the background color of the Web server control.
        '/ </summary>
        '/ <remarks>Use the BackColor property to specify the background color of the Web server control.</remarks>

        Public Shadows Property BackColor() As System.Drawing.Color
            Get
                Dim o As Object = ViewState("MenuItemBackColor")

                If Not (o Is Nothing) Then
                    Return CType(o, Color)
                Else
                    Return Color.Empty
                End If
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ViewState("MenuItemBackColor") = Value
                ViewState.SetItemDirty("MenuItemBackColor", True)
            End Set
        End Property

        '/ <summary>
        '/ Gets or sets the border color of the Web control.
        '/ </summary>
        '/ <value>A System.Drawing.Color that represents the border color of the control. The default is Color.Empty, which indicates that this property is not set.</value>
        '/ <remarks>Use the BorderColor property to specify the border color of the Web Server control.</remarks>

        Public Shadows Property BorderColor() As System.Drawing.Color
            Get
                Dim o As Object = ViewState("MenuItemBorderColor")

                If Not (o Is Nothing) Then
                    Return CType(o, Color)
                Else
                    Return Color.Empty
                End If
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ViewState("MenuItemBorderColor") = Value
                ViewState.SetItemDirty("MenuItemBorderColor", True)
            End Set
        End Property

        '/ <summary>
        '/ Gets or sets the border style of the Web server control.
        '/ </summary>
        '/ <value>One of the BorderStyle enumeration values. The default is <b>NotSet</b>.</value>
        '/ <remarks>Use the BorderStyle property to specify the border style for the Web server control. This property is set using one of the BorderStyle enumeration values.</remarks>

        Public Shadows Property BorderStyle() As BorderStyle
            Get
                Dim o As Object = ViewState("MenuItemBorderStyle")

                If Not (o Is Nothing) Then
                    Return CType(o, BorderStyle)
                Else
                    Return BorderStyle.NotSet
                End If
            End Get
            Set(ByVal Value As BorderStyle)
                ViewState("MenuItemBorderStyle") = Value
                ViewState.SetItemDirty("MenuItemBorderStyle", True)
            End Set
        End Property

        '/ <summary>
        '/ Gets or sets the border width of the Web server control.
        '/ </summary>
        '/ <value>A Unit that represents the border width of a Web server control. The default value is Unit.Empty, which indicates that this property is not set.</value>
        '/ <remarks>Use the BorderWidth property to specify a border width for a control.</remarks>

        Public Shadows Property BorderWidth() As Unit
            Get
                Dim o As Object = ViewState("MenuItemBorderWidth")

                If Not (o Is Nothing) Then
                    Return CType(o, Unit)
                Else
                    Return Unit.Empty
                End If
            End Get
            Set(ByVal Value As Unit)
                ViewState("MenuItemBorderWidth") = Value
                ViewState.SetItemDirty("MenuItemBorderWidth", True)
            End Set
        End Property

        '/ <summary>
        '/ Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
        '/ </summary>

        Public Shadows Property CssClass() As String
            Get
                Dim o As Object = ViewState("MenuItemCssClass")

                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("MenuItemCssClass") = Value
                ViewState.SetItemDirty("MenuItemCssClass", True)
            End Set
        End Property

        '/ <summary>
        '/ Gets the font properties associated with the Web server control.
        '/ </summary>
        '/ <value>A FontInfo that represents the font properties of the Web server control.</value>

        Public Shadows Property Font() As FontInfo
            Get
                Dim o As Object = ViewState("MenuItemFont")

                If Not (o Is Nothing) Then
                    Return CType(o, FontInfo)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal Value As FontInfo)
                ViewState("MenuItemFont") = Value
                ViewState.SetItemDirty("MenuItemFont", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the foreground color (typically the color of the text) of the Web server control.
        '/ </summary>

        Public Shadows Property ForeColor() As Color
            Get
                Dim o As Object = ViewState("MenuItemForeColor")

                If Not (o Is Nothing) Then
                    Return CType(o, Color)
                Else
                    Return Color.Empty
                End If
            End Get
            Set(ByVal Value As Color)
                ViewState("MenuItemForeColor") = Value
                ViewState.SetItemDirty("MenuItemForeColor", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the height of the Web server control.
        '/ </summary>

        Public Shadows Property Height() As Unit
            Get
                Dim o As Object = ViewState("MenuItemHeight")

                If Not (o Is Nothing) Then
                    Return CType(o, Unit)
                Else
                    Return Unit.Empty
                End If
            End Get
            Set(ByVal Value As Unit)
                ViewState("MenuItemHeight") = Value
                ViewState.SetItemDirty("MenuItemHeight", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the text displayed when the mouse pointer hovers over the Web server control.
        '/ </summary>

        Public Shadows Property ToolTip() As String
            Get
                Dim o As Object = ViewState("MenuItemToolTip")

                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("MenuItemToolTip") = Value
                ViewState.SetItemDirty("MenuItemToolTip", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the width of the Web server control.
        '/ </summary>

        Public Shadows Property Width() As Unit
            Get
                Dim o As Object = ViewState("MenuItemWidth")

                If Not (o Is Nothing) Then
                    Return CType(o, Unit)
                Else
                    Return Unit.Empty
                End If
            End Get
            Set(ByVal Value As Unit)
                ViewState("MenuItemWidth") = Value
                ViewState.SetItemDirty("MenuItemWidth", True)
            End Set
        End Property
#End Region

        '/ <summary>
        '/ Specifies if the MenuItem is tracking ViewState.  Required, since MenuItem
        '/ implements the IStateManager interface.
        '/ </summary>

        Shadows ReadOnly Property IsTrackingViewState() As Boolean Implements IStateManager.IsTrackingViewState
            Get
                Return MyBase.IsTrackingViewState
            End Get
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's image.
        '/ </summary>
        '/ <remarks>If both of the MenuItem's <see cref="Text"/> and <see cref="Image"/> properties are specified, 
        '/ <see cref="Text"/> will be used.</remarks>

        <Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("The MenuItem's image.")> _
        Public Overridable Property Image() As String
            Get
                Dim o As Object = ViewState("ItemImage")

                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemImage") = Value
                ViewState.SetItemDirty("ItemImage", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the alternate text for the MenuItem's image.
        '/ </summary>
        '/ <remarks>This value, if specified, is rendered as the <b>alt</b> attribute in the generated
        '/ <b>&lt;img&gt;</b> element.</remarks>

        <Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("The alternate text for the MenuItem's image.  Output as the Alt tag.")> _
        Public Overridable Property ImageAltText() As String
            Get
                Dim o As Object = ViewState("ItemImageAltText")

                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemImageAltText") = Value
                ViewState.SetItemDirty("ItemImageAltText", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets an image to be shown to the right of the menuitem's text or main image.</summary>
        '/ <remarks>You can only have either a <see cref="LeftImage"/> or a <see cref="RightImage"/>.  If both 
        '/ are specified, only the <see cref="LeftImage"/> will be used.
        '/ </remarks>

        <Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("An image to show on the right of the MenuItem's text or main image.")> _
        Public Overridable Property RightImage() As String
            Get
                Dim o As Object = ViewState("ItemRightImage")

                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemRightImage") = Value
                ViewState.SetItemDirty("ItemRightImage", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the width of the space to show an image in on the right of the MenuItem's <see cref="Text"/> or 
        '/ main image.
        '/ </summary>
        '/ <value>The default value is <b>Unit.Empty</b>.</value>

        <Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("The width of the space to show an image in on the right of the MenuItem's text or main image.")> _
        Public Overridable Property RightImageLeftPadding() As Unit
            Get
                Dim o As Object = ViewState("ItemRightImageLeftPadding")

                If Not (o Is Nothing) Then
                    Return CType(o, Unit)
                Else
                    Return Unit.Empty
                End If
            End Get
            Set(ByVal Value As Unit)
                ViewState("ItemRightImageLeftPadding") = Value
                ViewState.SetItemDirty("ItemRightImageLeftPadding", True)
            End Set
        End Property


        '/ <summary>
        '/ Specifies alignment for an image to be shown to the right of the MenuItem's <see cref="Text"/> or main image.
        '/ </summary>
        '/ <value>Set to one of the values of the <b>System.Web.UI.WebControls.ImageAlign</b> enumeration.
        '/ The default is <b>ImageAlign.NotSet</b></value>

        <Category("Behavior"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies alignment for an image to be shown to the right of the MenuItem's text or main image.")> _
        Public Overridable Property RightImageAlign() As System.Web.UI.WebControls.ImageAlign
            Get
                Dim o As Object = ViewState("ItemRightImageAlign")
                If o Is Nothing Then
                    Return System.Web.UI.WebControls.ImageAlign.NotSet
                Else
                    Return CType(o, System.Web.UI.WebControls.ImageAlign)
                End If
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.ImageAlign)
                ViewState("ItemRightImageAlign") = Value
                ViewState.SetItemDirty("ItemRightImageAlign", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets an image to be shown to the left of the MenuItem's <see cref="Text"/> or main image.		
        '/ </summary>
        '/ <remarks>You can only have either a <see cref="LeftImage"/> or a <see cref="RightImage"/>.  If both are 
        '/ specified, only the <see cref="LeftImage"/> will be used.
        '/ </remarks>

        <Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("An image to show on the left of the MenuItem's text or main image.")> _
        Public Overridable Property LeftImage() As String
            Get
                Dim o As Object = ViewState("ItemLeftImage")

                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemLeftImage") = Value
                ViewState.SetItemDirty("ItemLeftImage", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the width of the space to show an image in on the left of the MenuItem's <see cref="Text"/> or main image.
        '/ </summary>
        '/ <value>The default value is <b>Unit.Empty</b>.</value>

        <Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("The width of the space to show an image in on the left of the MenuItem's text or main image.")> _
        Public Overridable Property LeftImageRightPadding() As Unit
            Get
                Dim o As Object = ViewState("ItemLeftImageRightPadding")

                If Not (o Is Nothing) Then
                    Return CType(o, Unit)
                Else
                    Return Unit.Empty
                End If
            End Get
            Set(ByVal Value As Unit)
                ViewState("ItemLeftImageRightPadding") = Value
                ViewState.SetItemDirty("ItemLeftImageRightPadding", True)
            End Set
        End Property


        '/ <summary>
        '/ Specifies alignment for an image to be shown to the left of the MenuItem's <see cref="Text"/> or main image.
        '/ </summary>
        '/ <value>Set to one of the values of the <b>System.Web.UI.WebControls.ImageAlign</b> enumeration.
        '/ The default is <b>ImageAlign.NotSet</b></value>

        <Category("Behavior"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies alignment for an image to be shown to the left of the MenuItem's text or main image.")> _
        Public Overridable Property LeftImageAlign() As System.Web.UI.WebControls.ImageAlign
            Get
                Dim o As Object = ViewState("ItemLeftImageAlign")
                If o Is Nothing Then
                    Return System.Web.UI.WebControls.ImageAlign.NotSet
                Else
                    Return CType(o, System.Web.UI.WebControls.ImageAlign)
                End If
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.ImageAlign)
                ViewState("ItemLeftImageAlign") = Value
                ViewState.SetItemDirty("ItemLeftImageAlign", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's mouseover image.
        '/ </summary>
        '/ <remarks>The client-side <b>mouseover</b> event fires when the user's mouse moves over the MenuItem.</remarks>

        <Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("The image displayed when the mouse is moved over the MenuItem.")> _
        Public Overridable Property MouseOverImage() As String
            Get
                Dim o As Object = ViewState("ItemMouseOverImage")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemMouseOverImage") = Value
                ViewState.SetItemDirty("ItemMouseOverImage", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's mouseup image.
        '/ </summary>

        <Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("The image displayed on mouse up over the MenuItem.")> _
        Public Overridable Property MouseUpImage() As String
            Get
                Dim o As Object = ViewState("ItemMouseUpImage")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemMouseUpImage") = Value
                ViewState.SetItemDirty("ItemMouseUpImage", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's mousedown image.
        '/ </summary>
        '/ <remarks>The client-side <b>mousedown</b> event fires when the mouse is over the MenuItem
        '/ and the uses clicks the mouse button.</remarks>

        <Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("The image displayed on mousedown over the menuitem.")> _
        Public Overridable Property MouseDownImage() As String
            Get
                Dim o As Object = ViewState("ItemMouseDownImage")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemMouseDownImage") = Value
                ViewState.SetItemDirty("ItemMouseDownImage", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's name.
        '/ </summary>
        '/ <value>A string value denoting the MenuItem's <b>Name</b>.  The default is an empty string.</value>
        '/ <remarks>Use the <b>Name</b> property to give a unique, identifying name to a MenuItem instance.
        '/ The <see cref="MenuItemCollection"/> class, which contains a collection of MenuItem instances, can
        '/ be searched for a MenuItem with a specified <b>Name</b>.</remarks>

        <Description("The menuitem's name."), NotifyParentProperty(True), DefaultValue("")> _
        Public Overridable Property Name() As String
            Get
                Dim o As Object = ViewState("ItemName")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemName") = Value
                ViewState.SetItemDirty("ItemName", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's text content.
        '/ </summary>
        '/ <remarks>For total customization of the appearance of the MenuItem, the <b>Text</b> property can have 
        '/ HTML content.</remarks>
        '/ <example>
        '/ The following example illustrates using HTML content in the <b>Text</b> property:<p />[C#]
        '/ <code>
        '/ MenuItem mi = new MenuItem();
        '/ mi.Text = "&lt;b&gt;This will be bold!&lt;/b&gt;";
        '/ </code>
        '/ </example>

        <Description("The MenuItem's text."), NotifyParentProperty(True), DefaultValue("")> _
        Public Overridable Property [Text]() As String
            Get
                Dim o As Object = ViewState("ItemText")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemText") = Value
                ViewState.SetItemDirty("ItemText", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's Url.
        '/ </summary>
        '/ <remarks>If a MenuItem has a <b>Url</b> value, the MenuItem is "clickable."  That is, the end user
        '/ will be able to click the MenuItem and be whisked to the specified URL.<p />The <b>Url</b> value can use
        '/ the ~ notation.  For this to work, though, the Menu class's <b>DefaultResolveUrl</b> property must be
        '/ set to True.</remarks>
        '/ <value>Specifies the URL the user will be whisked to when the MenuItem is clicked.  The default value
        '/ is <b>String.Empty</b>.  The <b>Url</b> property is optional.</value>

        <Description("The optional URL for the MenuItem."), NotifyParentProperty(True), DefaultValue("")> _
        Public Overridable Property Url() As String
            Get
                Dim o As Object = ViewState("ItemURL")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemURL") = Value
                ViewState.SetItemDirty("ItemURL", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's Javascript command.
        '/ </summary>

        <Description("The optional javascript command for the menuitem."), NotifyParentProperty(True), DefaultValue("")> _
        Public Overridable Property JavascriptCommand() As String
            Get
                Dim o As Object = ViewState("ItemJavascriptCommand")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemJavascriptCommand") = Value
                ViewState.SetItemDirty("ItemJavascriptCommand", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's target used when the <see cref="Url"/> is navigated to.
        '/ </summary>
        '/ <remarks>The <see cref="Url"/> property must be set to a non-empty string for <b>Target</b> to have any affect.
        '/ <p />To have a MenuItem opened in a new window, set <b>Target</b> to _blank</remarks>

        <Description("The optional target for the MenuItem URL."), NotifyParentProperty(True), DefaultValue("")> _
        Public Overridable Property Target() As String
            Get
                Dim o As Object = ViewState("ItemTarget")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemTarget") = Value
                ViewState.SetItemDirty("ItemTarget", True)
            End Set
        End Property


        '/ <summary>
        '/ Retrieves the MenuItem's set of SubItems.
        '/ </summary>
        '/ <remarks>The <b>SubItems</b> collection is useful when programmatically creating or modifying
        '/ a menu's content.</remarks>

        <Category("Behavior"), Description("The collection of submenu items."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), PersistenceMode(PersistenceMode.InnerProperty)> _
        Public Overridable ReadOnly Property SubItems() As MenuItemCollection
            Get
                If MyBase.IsTrackingViewState Then
                    CType(_subItems, IStateManager).TrackViewState()
                End If
                Return Me._subItems
            End Get
        End Property


        '/ <summary>
        '/ Specifies the MenuItem's roles.
        '/ </summary>

        <Category("Behavior"), Description("Indicates the menu item's roles.")> _
        Public Overridable ReadOnly Property Roles() As RoleCollection
            Get
                If MyBase.IsTrackingViewState Then
                    CType(_roles, IStateManager).TrackViewState()
                End If
                Return Me._roles
            End Get
        End Property



        '/ <summary>
        '/ Gets or sets the MenuItem's ID.  It is not recommended that this be set directly.
        '/ </summary>
        '/ <remarks>The <b>ID</b> for each MenuItem is programmatically set in the <see cref="Menu"/> class's
        '/ <see cref="Menu.BuildMenuItem"/> method.</remarks>

        <Browsable(False)> _
        Public Overridable Property MenuID() As String
            Get
                Dim o As Object = ViewState("ItemID")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemID") = Value
                ViewState.SetItemDirty("ItemID", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's type.  It is not recommended that this be set directly.
        '/ </summary>
        '/ <value>The MenuItem type can be one of the available <see cref="MenuItemType"/> enumeration values.
        '/ The default is <b>MenuItemType.MenuItem</b>.</value>

        <Description("The type of menuitem."), NotifyParentProperty(True), DefaultValue("")> _
        Public Overridable Property MenuType() As MenuItemType
            Get
                Dim o As Object = ViewState("ItemType")
                If Not (o Is Nothing) Then
                    Return CType(o, MenuItemType)
                Else
                    Return MenuItemType.MenuItem
                End If
            End Get
            Set(ByVal Value As MenuItemType)
                ViewState("ItemType") = Value
                ViewState.SetItemDirty("ItemType", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or Sets the CommandName property.
        '/ </summary>
        '/ <remarks>If a MenuItem's <b>CommandName</b> property is set, and the <see cref="Url"/> property is
        '/ <i>not</i> set, then the MenuItem, when clicked, will cause the Web Form to postback, and a
        '/ MenuItemClicked event will be raised.</remarks>

        <Category("Behavior"), Description("The optional command name for the menuitem."), NotifyParentProperty(True), DefaultValue("")> _
        Public Overridable Property CommandName() As String
            Get
                Dim o As Object = ViewState("ItemCommandName")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemCommandName") = Value
                ViewState.SetItemDirty("ItemCommandName", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's mouseover CSS class.
        '/ </summary>

        <Category("Appearance"), Description("The menuitem's mouse over stylesheet class."), NotifyParentProperty(True), DefaultValue("")> _
        Public Overridable Property MouseOverCssClass() As String
            Get
                Dim o As Object = ViewState("ItemMouseOverCssClass")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemMouseOverCssClass") = Value
                ViewState.SetItemDirty("ItemMouseOverCssClass", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's mouseup CSS class.
        '/ </summary>

        <Category("Appearance"), Description("The MenuItem's mouse up stylesheet class."), NotifyParentProperty(True), DefaultValue("")> _
        Public Overridable Property MouseUpCssClass() As String
            Get
                Dim o As Object = ViewState("ItemMouseUpCssClass")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemMouseUpCssClass") = Value
                ViewState.SetItemDirty("ItemMouseUpCssClass", True)
            End Set
        End Property


        '/ <summary>
        '/ Gets or sets the MenuItem's mousedown CSS class.
        '/ </summary>

        <Category("Appearance"), Description("The MenuItem's mouse down stylesheet class."), NotifyParentProperty(True), DefaultValue("")> _
        Public Overridable Property MouseDownCssClass() As String
            Get
                Dim o As Object = ViewState("ItemMouseDownCssClass")
                If Not (o Is Nothing) Then
                    Return CStr(o)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemMouseDownCssClass") = Value
                ViewState.SetItemDirty("ItemMouseDownCssClass", True)
            End Set
        End Property


        '/ <summary>
        '/ Specifies whether URL should be resolved before being output.  </summary>
        '/ <remarks>If either <see cref="Menu.DefaultResolveURL"/> in the <see cref="Menu"/> class is true or this value is true, 
        '/ then the URL for the menuitem will be resolved.
        '/ </remarks>

        <Category("Behavior"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies whether URL should be resolved before being output.  If either DefaultResolveURL in the MenuClass is true or this value is true, then the URL for the menuitem will be resolved.")> _
         Public Overridable Shadows Property ResolveURL() As Boolean
            Get
                Dim o As Object = ViewState("ItemResolveURL")
                If o Is Nothing Then
                    Return False
                Else
                    Return CBool(o)
                End If
            End Get
            Set(ByVal Value As Boolean)
                ViewState("ItemResolveURL") = Value
                ViewState.SetItemDirty("ItemResolveURL", True)
            End Set
        End Property


        '/ <summary>
        '/ Specifies horizontal alignment for the MenuItem.
        '/ </summary>
        '/ <value>One of the horizontal alignment options from the <b>System.Web.UI.WebControls.HorizontalAlign</b> enumeration.
        '/ The default is <b>HorizontalAlign.NotSet</b>.</value>
        '/ <remarks>The horizontal alignment indicates how the text and/or images of the MenuItem are aligned.</remarks>

        <Category("Behavior"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies horizontal alignment for the MenuItem.")> _
        Public Overridable Property HorizontalAlign() As System.Web.UI.WebControls.HorizontalAlign
            Get
                Dim o As Object = ViewState("ItemHorizontalAlign")
                If o Is Nothing Then
                    Return System.Web.UI.WebControls.HorizontalAlign.NotSet
                Else
                    Return CType(o, System.Web.UI.WebControls.HorizontalAlign)
                End If
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.HorizontalAlign)
                ViewState("ItemHorizontalAlign") = Value
                ViewState.SetItemDirty("ItemHorizontalAlign", True)
            End Set
        End Property


        '/ <summary>
        '/ Specifies vertical alignment for the MenuItem.
        '/ </summary>
        '/ <value>One of the vertical alignment options from the <b>System.Web.UI.WebControls.VerticalAlign</b> enumeration.
        '/ The default is <b>VerticalAlign.NotSet</b>.</value>

        <Category("Behavior"), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(True), Description("Specifies vertical alignment for the MenuItem.")> _
        Public Overridable Property VerticalAlign() As System.Web.UI.WebControls.VerticalAlign
            Get
                Dim o As Object = ViewState("ItemVerticalAlign")
                If o Is Nothing Then
                    Return System.Web.UI.WebControls.VerticalAlign.NotSet
                Else
                    Return CType(o, System.Web.UI.WebControls.VerticalAlign)
                End If
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.VerticalAlign)
                ViewState("ItemVerticalAlign") = Value
                ViewState.SetItemDirty("ItemVerticalAlign", True)
            End Set
        End Property
#End Region
    End Class 'MenuItem
End Namespace