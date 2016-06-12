Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace CustomWebControls
    <DefaultProperty("Text"), ToolboxData("<{0}:ListPicker runat=server></{0}:ListPicker>")> _
    Public Class ListPicker
        Inherits WebControl
        Implements IPostBackDataHandler, INamingContainer

        <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> Property Text() As String
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

        Public Event ListChanged As EventHandler

        Private _isUplevel As Boolean
        Private _allItems As New ArrayList
        Private _selectedItems As New ArrayList
        Private _dataSource As IEnumerable = Nothing
        Private _dataField As String = String.Empty


        ' The following controls are used by downlevel browsers
        Private lstAllItems As ListBox
        Private lstSelectedItems As ListBox
        Private btnAddItem As Button
        Private btnRemoveItem As Button



        '*********************************************************************
        '
        ' Changed Property
        '
        ' True when new item picked. 
        '
        '*********************************************************************

        Public Property Changed() As Boolean
            Get
                If ViewState("Changed") Is Nothing Then
                    Return False
                Else
                    Return CBool(ViewState("Changed"))
                End If
            End Get
            Set(ByVal Value As Boolean)
                ViewState("Changed") = Value
            End Set
        End Property

        Public Property SelectedItemsText() As String
            Get
                If ViewState("SelectedItemsText") Is Nothing Then
                    Return "Selected Items"
                Else
                    Return CStr(ViewState("SelectedItemsText"))
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("SelectedItemsText") = Value
            End Set
        End Property

        Public Property AllItemsText() As String
            Get
                If ViewState("AllItemsText") Is Nothing Then
                    Return "All Items"
                Else
                    Return CStr(ViewState("AllItemsText"))
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("AllItemsText") = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' Size Property
        '
        ' Determines the number of items displayed by the picker.
        '
        '*********************************************************************

        Public Property Size() As Integer
            Get
                If ViewState("Size") Is Nothing Then
                    Return 5
                Else
                    Return Fix(ViewState("Size"))
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("Size") = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' SelectedState Property
        '
        ' Preserves state between postbacks so change notification can be raised
        ' only when state changes. 
        '
        '*********************************************************************

        Private Property SelectedState() As String
            Get
                If ViewState("selectedState") Is Nothing Then
                    Return String.Empty
                Else
                    Return CStr(ViewState("selectedState"))
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("selectedState") = Value
            End Set
        End Property


        '*********************************************************************
        '
        ' DataSource Property
        '
        ' The Data Source for the All Items listbox
        '
        '*********************************************************************

        Public Property DataSource() As IEnumerable
            Get
                Return _dataSource
            End Get
            Set(ByVal Value As IEnumerable)
                _dataSource = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' DataField Property
        '
        ' The Data Field for the All Items Data Source
        '
        '*********************************************************************

        Public Property DataField() As String
            Get
                Return _dataField
            End Get
            Set(ByVal Value As String)
                _dataField = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' SelectedHelperID Property
        '
        ' The ID of the hidden form field for the selected list box
        '
        '*********************************************************************

        Protected ReadOnly Property SelectedHelperID() As String
            Get
                Return ClientID + "_SelectedState"
            End Get
        End Property

        '*********************************************************************
        '
        ' AllHelperID Property
        '
        ' The ID of the hidden form field for the all list box
        '
        '*********************************************************************

        Protected ReadOnly Property AllHelperID() As String
            Get
                Return ClientID + "_AllState"
            End Get
        End Property

        '*********************************************************************
        '
        ' OnInit Method
        '
        ' Notifies framework that this control wants to be notified of data
        ' changes on postback.
        '
        '*********************************************************************
        Protected Overrides Sub OnInit(ByVal e As EventArgs)
            If Not (Page Is Nothing) Then
                Page.RegisterRequiresPostBack(Me)
            End If
        End Sub    'OnInit

        '*********************************************************************
        '
        ' LoadPostData Method
        '
        ' This method retrieves the posted data and update the control properties
        '
        '*********************************************************************
        Public Function LoadPostData(ByVal postDataKey As String, ByVal values As System.Collections.Specialized.NameValueCollection) As Boolean Implements IPostBackDataHandler.LoadPostData
            Dim _allState As String
            Dim _selectedState As String

            ' return if null
            If values(AllHelperID) Is Nothing Then
                Return False
            End If
            If _isUplevel Then
                _allState = values(AllHelperID).Trim()
                _selectedState = values(SelectedHelperID).Trim()


                If _allState = String.Empty Then
                    _allItems.Clear()
                Else
                    _allItems = New ArrayList(_allState.Split(","c))
                End If
                If _selectedState = String.Empty Then
                    _selectedItems.Clear()
                Else
                    _selectedItems = New ArrayList(_selectedState.Split(","c))
                End If

                ' No change, return false
                If SelectedState = _selectedState.Trim() Then
                    Return False
                End If
                ' Change, return true and update state
                Changed = True
                SelectedState = _selectedState
                Return True
            Else
                If Changed Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Function    'LoadPostData

        '*********************************************************************
        '
        ' RaisePostDataChangedEvent Method
        '
        ' This method raises the OnListChanged event if posted data changed
        '
        '*********************************************************************
        Public Sub RaisePostDataChangedEvent() Implements IPostBackDataHandler.RaisePostDataChangedEvent
            OnListChanged(EventArgs.Empty)
        End Sub    'RaisePostDataChangedEvent

        '*********************************************************************
        '
        ' OnListChanged Method
        '
        ' Method for handling list control post data changes
        '
        '*********************************************************************
        Protected Overridable Sub OnListChanged(ByVal e As EventArgs)
            RaiseEvent ListChanged(Me, e)
        End Sub    'OnListChanged


        '*********************************************************************
        '
        ' OnDataBinding Method
        '
        ' This method is called when the DataBind method is called
        '
        '*********************************************************************
        Protected Overrides Sub OnDataBinding(ByVal e As EventArgs)
            Dim objDataEnum As IEnumerator

            ' bind all items
            If Not (_dataSource Is Nothing) Then

                ' Populate all items
                objDataEnum = _dataSource.GetEnumerator()
                While objDataEnum.MoveNext()
                    If _dataField = String.Empty Then
                        _allItems.Add(CStr(objDataEnum.Current))
                    Else
                        _allItems.Add(CStr(DataBinder.Eval(objDataEnum.Current, _dataField)))
                    End If
                End While
            End If

            ' Remove selected items from all items
            Dim item As String
            For Each item In _selectedItems
                _allItems.Remove(item)
            Next item

            ' Bind to ListBox for downlevel browsers
            If Not _isUplevel Then
                EnsureChildControls()

                lstAllItems.DataSource = _allItems
                lstAllItems.DataBind()

                lstSelectedItems.DataSource = _selectedItems
                lstSelectedItems.DataBind()
            End If
        End Sub    'OnDataBinding

        '*********************************************************************
        '
        ' SelectedItems Property
        '
        ' Returns an array of selected items
        '
        '*********************************************************************

        Public Property SelectedItems() As String()
            Get
                If Not _isUplevel Then
                    EnsureChildControls()
                    _selectedItems.Clear()
                    Dim item As ListItem
                    For Each item In lstSelectedItems.Items
                        _selectedItems.Add(item.Text)
                    Next item
                End If
                Return CType(_selectedItems.ToArray(GetType(String)), String())
            End Get
            Set(ByVal Value As String())
                _selectedItems = New ArrayList(Value)
                SelectedState = String.Join(",", Value)
            End Set
        End Property

        '*********************************************************************
        '
        ' AllItems Property
        '
        ' Returns an array of all items.
        '
        '*********************************************************************

        Public ReadOnly Property AllItems() As String()
            Get
                Return CType(_allItems.ToArray(GetType(String)), String())
            End Get
        End Property

        '*********************************************************************
        '
        ' List Picker Constructor
        '
        ' Determine whether browser is uplevel.
        '
        '*********************************************************************
        Public Sub New()
            MyBase.New(HtmlTextWriterTag.Table)

            If Not (Context Is Nothing) Then
                If HttpContext.Current.Request.Browser.Browser = "IE" AndAlso HttpContext.Current.Request.Browser.MajorVersion >= 6 Then
                    _isUplevel = True
                Else
                    _isUplevel = False
                End If
            End If
        End Sub    'New

        '*********************************************************************
        '
        ' CreateChildControls Method
        '
        ' Create composite controls for downlevel browsers.
        '
        '*********************************************************************
        Protected Overrides Sub CreateChildControls()
            If Not _isUplevel Then

                lstAllItems = New ListBox
                Controls.Add(lstAllItems)

                lstSelectedItems = New ListBox
                Controls.Add(lstSelectedItems)

                btnAddItem = New Button
                btnAddItem.Text = "->"
                btnAddItem.CausesValidation = False
                AddHandler btnAddItem.Click, AddressOf btnAddClick
                Controls.Add(btnAddItem)

                btnRemoveItem = New Button
                btnRemoveItem.Text = "<-"
                btnRemoveItem.CausesValidation = False
                AddHandler btnRemoveItem.Click, AddressOf btnRemoveClick
                Controls.Add(btnRemoveItem)
            End If
        End Sub    'CreateChildControls

        '*********************************************************************
        '
        ' btnAddClick Method
        '
        ' Raised by clicking add button on downlevel browser.
        '
        '*********************************************************************
        Sub btnAddClick(ByVal s As [Object], ByVal e As EventArgs)
            If lstAllItems.SelectedIndex <> -1 Then
                ' Move the item
                lstSelectedItems.SelectedIndex = -1
                lstSelectedItems.Items.Add(lstAllItems.SelectedItem)
                lstAllItems.Items.RemoveAt(lstAllItems.SelectedIndex)

                ' update changed status items
                Changed = True
            End If
        End Sub    'btnAddClick

        '*********************************************************************
        '
        ' btnRemoveClick Method
        '
        ' Raised by clicking remove button on downlevel browser.
        '
        '*********************************************************************
        Sub btnRemoveClick(ByVal s As [Object], ByVal e As EventArgs)
            If lstSelectedItems.SelectedIndex <> -1 Then
                ' Move the item
                lstAllItems.SelectedIndex = -1
                lstAllItems.Items.Add(lstSelectedItems.SelectedItem)
                lstSelectedItems.Items.RemoveAt(lstSelectedItems.SelectedIndex)

                ' update changed status items
                Changed = True
            End If
        End Sub    'btnRemoveClick

        '*********************************************************************
        '
        ' OnPreRender Method
        '
        ' Add reference to client-side script.
        '
        '*********************************************************************
        Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
            If _isUplevel Then
                ' next two lines for conversion to framework 2.0
                'Page.ClientScript.RegisterHiddenField(SelectedHelperID, String.Join(",", SelectedItems))
                'Page.ClientScript.RegisterHiddenField(AllHelperID, String.Join(",", AllItems))
                Page.RegisterHiddenField(SelectedHelperID, String.Join(",", SelectedItems))
                Page.RegisterHiddenField(AllHelperID, String.Join(",", AllItems))
            End If
        End Sub    'OnPreRender
        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Displays either uplevel or downlevel content.
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            If _isUplevel Then
                RenderUplevelContent(writer)
            Else
                RenderDownlevelContent(writer)
            End If
        End Sub    'RenderContents

        '*********************************************************************
        '
        ' RenderUplevelContents Method
        '
        ' Displays uplevel content.
        '
        '*********************************************************************
        Sub RenderUplevelContent(ByVal writer As HtmlTextWriter)
            Dim item As String

            writer.RenderBeginTag(HtmlTextWriterTag.Tr)

            ' Add Labels
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)

            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.Write(AllItemsText)
            writer.RenderEndTag()
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.Write("&nbsp;")
            writer.RenderEndTag()
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.Write(SelectedItemsText)
            writer.RenderEndTag()

            writer.RenderEndTag()

            ' Add All listbox
            writer.RenderBeginTag(HtmlTextWriterTag.Td)

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_SrcList")
            writer.AddAttribute(HtmlTextWriterAttribute.Size, Size.ToString())
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "listPicker")
			writer.AddAttribute(HtmlTextWriterAttribute.Multiple, "true")
            writer.RenderBeginTag(HtmlTextWriterTag.Select)

            For Each item In _allItems
                writer.Write(String.Format("<option value=""{0}"">{0}</option>", item))
            Next item
            writer.RenderEndTag()

            writer.RenderEndTag()

            ' Add Button
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
			writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("switchList('{0}','{1}')", ClientID + "_SrcList", ClientID + "_DstList"))
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "listPickerArrow")
            writer.RenderBeginTag(HtmlTextWriterTag.Button)
            writer.Write("-&gt;")
            writer.RenderEndTag()

            ' Delete Button
            writer.RenderBeginTag(HtmlTextWriterTag.P)
			writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("switchList('{0}','{1}')", ClientID + "_DstList", ClientID + "_SrcList"))
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "listPickerArrow")
            writer.RenderBeginTag(HtmlTextWriterTag.Button)
            writer.Write("&lt;-")
            writer.RenderEndTag()
            writer.RenderEndTag()

            writer.RenderEndTag()


            ' Add selected listbox
            writer.RenderBeginTag(HtmlTextWriterTag.Td)

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_DstList")
            writer.AddAttribute(HtmlTextWriterAttribute.Size, Size.ToString())
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "listPicker")
			writer.AddAttribute(HtmlTextWriterAttribute.Multiple, "true")
			writer.RenderBeginTag(HtmlTextWriterTag.Select)
            For Each item In _selectedItems
                writer.Write(String.Format("<option value=""{0}"">{0}</option>", item))
            Next item
            writer.RenderEndTag()

            writer.RenderEndTag()

            writer.RenderEndTag()
        End Sub    'RenderUplevelContent

        '*********************************************************************
        '
        ' RenderDownlevelContents Method
        '
        ' Displays uplevel content.
        '
        '*********************************************************************
        Sub RenderDownlevelContent(ByVal writer As HtmlTextWriter)
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)


            ' Add Labels
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)

            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.Write(AllItemsText)
            writer.RenderEndTag()
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.Write("&nbsp;")
            writer.RenderEndTag()
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.Write(SelectedItemsText)
            writer.RenderEndTag()

            writer.RenderEndTag()


            ' Add All listbox
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            lstAllItems.RenderControl(writer)
            writer.RenderEndTag()

            ' Add Button
            writer.RenderBeginTag(HtmlTextWriterTag.Td)

            btnAddItem.RenderControl(writer)

            ' Delete Button
            writer.RenderBeginTag(HtmlTextWriterTag.P)
            btnRemoveItem.RenderControl(writer)
            writer.RenderEndTag()

            writer.RenderEndTag()


            ' Add selected listbox
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            lstSelectedItems.RenderControl(writer)
            writer.RenderEndTag()

            writer.RenderEndTag()
        End Sub    'RenderDownlevelContent 
    End Class 'ListPicker
    <ParseChildren(True)> _
    Public Class AlphaPicker

        Inherits WebControl
        Implements INamingContainer

        Private currentLetter As String = "A"


        ' *********************************************************************
        '  CreateChildControls
        '
        '/ <summary>
        '/ Renders the Alpha picker options
        '/ </summary>
        '/ 
        ' ********************************************************************/ 
        Protected Overrides Sub CreateChildControls()
            Dim label As Label

            ' Add the series of linkbuttons
            Dim chrStart As Char = "A"c
            Dim chrStop As Char = "Z"c

            ' Loop through all the characters
            Dim iLoop As Integer
            For iLoop = AscW(chrStart) To AscW(chrStop)
                Controls.Add(CreateLetteredLinkButton(ChrW(iLoop).ToString()))

                label = New Label
                label.CssClass = "Content_Small"
                label.Text = " | "
                Controls.Add(label)
            Next iLoop

            Controls.Add(CreateLetteredLinkButton("All"))
        End Sub    'CreateChildControls



        ' *********************************************************************
        '  CreateLetteredLinkButton
        '
        '/ <summary>
        '/ Creates link buttons
        '/ </summary>
        '/ 
        ' ********************************************************************/ 
        Private Function CreateLetteredLinkButton(ByVal buttonText As String) As LinkButton

            ' Add a new link button
            Dim btnTmp As New LinkButton
            btnTmp.Text = buttonText
            btnTmp.CssClass = "linkSmallBold"
            btnTmp.CommandArgument = buttonText
            AddHandler btnTmp.Click, AddressOf Letter_Clicked

            Return btnTmp
        End Function    'CreateLetteredLinkButton

        ' *********************************************************************
        '  Letter_Clicked
        '
        '/ <summary>
        '/ Event raised when a letter has been clicked by the end user
        '/ </summary>
        '/ 
        ' ********************************************************************/
        Public Event LetterChanged As System.EventHandler


        ' *********************************************************************
        '  Letter_Clicked
        '
        '/ <summary>
        '/ Event raised when a letter is clicked upon.
        '/ </summary>
        '/ 
        ' ********************************************************************/
        Private Sub Letter_Clicked(ByVal sender As Object, ByVal e As EventArgs)

            SelectedLetter = CType(sender, LinkButton).CommandArgument

            RaiseEvent LetterChanged(sender, e)
        End Sub    'Letter_Clicked

        ' *********************************************************************
        '  SelectedLetter
        '
        '/ <summary>
        '/ Property that returns the currently selected Letter
        '/ </summary>
        '/ 
        ' ********************************************************************/

        Public Property SelectedLetter() As String
            Get
                If ViewState("SelectedLetter") Is Nothing Then
                    Return currentLetter
                End If
                Return CStr(ViewState("SelectedLetter"))
            End Get
            Set(ByVal Value As String)
                ViewState("SelectedLetter") = Value
            End Set
        End Property
    End Class
End Namespace