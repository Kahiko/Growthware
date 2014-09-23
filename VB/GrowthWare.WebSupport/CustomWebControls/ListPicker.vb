Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports GrowthWare.WebSupport.CustomWebControls.Designers
Imports System.Globalization

Namespace CustomWebControls
    <DefaultProperty("Text"), Designer(GetType(CustomDesigner)), ToolboxData("<{0}:ListPicker runat=server></{0}:ListPicker>")> _
    Public Class ListPicker
        Inherits WebControl
        Implements IPostBackDataHandler, INamingContainer

        <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> _
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

        Public Event ListChanged As EventHandler

        Private m_AllItems As New ArrayList
        Private m_SelectedItems As New ArrayList
        Private m_DataSource As IEnumerable = Nothing
        Private m_DataField As String = String.Empty
        Private m_SelectButtonText As String = " -> "
        Private m_SelectAllButtonText As String = " ->> "
        Private m_DeselectButtonText As String = " <- "
        Private m_DeselectAllButtonText As String = " <<- "
        Private m_ButtonWidth As String = "50px"

        ' The following controls are used by downlevel browsers
        Private lstAllItems As ListBox
        Private lstSelectedItems As ListBox
        Private btnAddItem As Button
        Private btnAddAllItem As Button
        Private btnRemoveItem As Button
        Private btnRemoveAllItem As Button



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
                ViewState("AllItemsText") = Value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Used to determine if the destination will be sorted
        ''' when a selection is made.
        ''' </summary>
        Public Property SortOnChange() As Boolean
            Get
                If ViewState("SortOnChange") Is Nothing Then
                    Return True
                Else
                    Return CStr(ViewState("SortOnChange"))
                End If
            End Get
            Set(ByVal Value As Boolean)
                ViewState("SortOnChange") = Value
            End Set
        End Property

        ''' <summary>
        ''' Used to determine if the sort up and down
        ''' buttons will appear for the destination
        ''' </summary>
        Public Property DestinationSortable() As Boolean
            Get
                If ViewState("DestinationSortable") Is Nothing Then
                    Return False
                Else
                    Return CStr(ViewState("DestinationSortable"))
                End If
            End Get
            Set(ByVal Value As Boolean)
                ViewState("DestinationSortable") = Value
            End Set
        End Property

        Property SelectButtonText() As String
            Get
                Return m_SelectButtonText
            End Get

            Set(ByVal Value As String)
                m_SelectButtonText = Value.Trim
            End Set
        End Property

        Property SelectAllButtonText() As String
            Get
                Return m_SelectAllButtonText
            End Get

            Set(ByVal Value As String)
                m_SelectAllButtonText = Value.Trim
            End Set
        End Property

        Property DeselectButtonText() As String
            Get
                Return m_DeselectButtonText
            End Get

            Set(ByVal Value As String)
                m_DeselectButtonText = Value
            End Set
        End Property

        Property DeselectAllButtonText() As String
            Get
                Return m_DeselectAllButtonText
            End Get

            Set(ByVal Value As String)
                m_DeselectAllButtonText = Value
            End Set
        End Property

        Property ButtonWidth() As String
            Get
                Return m_ButtonWidth
            End Get
            Set(ByVal value As String)
                If value.Trim.EndsWith("px") Then
                    m_ButtonWidth = value.Trim
                Else
                    m_ButtonWidth = value.Trim & "px"
                End If
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
                    Return 200
                Else
                    Return Fix(ViewState("Size"))
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("Size") = Value
            End Set
        End Property

        Public Property Rows() As Integer
            Get
                If ViewState("Rows") Is Nothing Then
                    Return 6
                Else
                    Return Fix(ViewState("Rows"))
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("Rows") = Value
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

        Public Property SelectedState() As String
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
                Return m_DataSource
            End Get
            Set(ByVal Value As IEnumerable)
                m_DataSource = Value
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
                Return m_DataField
            End Get
            Set(ByVal Value As String)
                m_DataField = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' SelectedHelperId Property
        '
        ' The ID of the hidden form field for the selected list box
        '
        '*********************************************************************

        Protected ReadOnly Property SelectedHelperId() As String
            Get
                Return ClientId + "_SelectedState"
            End Get
        End Property

        '*********************************************************************
        '
        ' AllHelperId Property
        '
        ' The ID of the hidden form field for the all list box
        '
        '*********************************************************************

        Protected ReadOnly Property AllHelperId() As String
            Get
                Return ClientId + "_AllState"
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
                MyBase.OnInit(e)
                Page.RegisterRequiresPostBack(Me)
                Dim scriptUrl As String = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "GrowthWare.CustomWebControls.ListPicker.js")
                Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "GrowthWareListPicker", scriptUrl)
            End If
        End Sub       'OnInit

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
            If values(AllHelperId) Is Nothing Then
                Return False
            End If
            _allState = values(AllHelperId).Trim()
            _selectedState = values(SelectedHelperId).Trim()
            If _allState = String.Empty Then
                m_AllItems.Clear()
            Else
                m_AllItems = New ArrayList(_allState.Split(","c))
            End If
            If _selectedState = String.Empty Then
                m_SelectedItems.Clear()
            Else
                m_SelectedItems = New ArrayList(_selectedState.Split(","c))
            End If
            ' No change, return false
            If SelectedState = _selectedState.Trim() Then
                Return False
            End If
            ' Change, return true and update state
            Changed = True
            SelectedState = _selectedState
            Return True
        End Function          'LoadPostData

        '*********************************************************************
        '
        ' RaisePostDataChangedEvent Method
        '
        ' This method raises the OnListChanged event if posted data changed
        '
        '*********************************************************************
        Public Sub RaisePostDataChangedEvent() Implements IPostBackDataHandler.RaisePostDataChangedEvent
            OnListChanged(EventArgs.Empty)
        End Sub       'RaisePostDataChangedEvent

        '*********************************************************************
        '
        ' OnListChanged Method
        '
        ' Method for handling list control post data changes
        '
        '*********************************************************************
        Protected Overridable Sub OnListChanged(ByVal e As EventArgs)
            RaiseEvent ListChanged(Me, e)
        End Sub       'OnListChanged


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
            If Not (m_DataSource Is Nothing) Then

                ' Populate all items
                objDataEnum = m_DataSource.GetEnumerator()
                While objDataEnum.MoveNext()
                    If m_DataField = String.Empty Then
                        m_AllItems.Add(CStr(objDataEnum.Current.ToString()))
                    Else
                        m_AllItems.Add(CStr(DataBinder.Eval(objDataEnum.Current, m_DataField)))
                    End If
                End While
            End If

            ' Remove selected items from all items
            Dim item As String
            For Each item In m_SelectedItems
                m_AllItems.Remove(item)
            Next item
        End Sub       'OnDataBinding

        '*********************************************************************
        '
        ' SelectedItems Property
        '
        ' Returns an array of selected items
        '
        '*********************************************************************

        Public Property SelectedItems() As String()
            Get
                Return CType(m_SelectedItems.ToArray(GetType(String)), String())
            End Get
            Set(ByVal Value As String())
                m_SelectedItems = New ArrayList(Value)
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
                Return CType(m_AllItems.ToArray(GetType(String)), String())
            End Get
        End Property

        '*********************************************************************
        '
        ' List Picker Constructor
        '
        '*********************************************************************
        Public Sub New()
            MyBase.New(HtmlTextWriterTag.Table)
        End Sub       'New

        '*********************************************************************
        '
        ' BtnAddClick Method
        '
        ' Raised by clicking add button on downlevel browser.
        '
        '*********************************************************************
        Sub BtnAddClick(ByVal s As [Object], ByVal e As EventArgs)
            If lstAllItems.SelectedIndex <> -1 Then
                ' Move the item
                Dim x As Integer = 0
                For x = lstAllItems.Items.Count - 1 To 0 Step -1
                    If lstAllItems.Items(x).Selected Then
                        lstSelectedItems.Items.Add(lstAllItems.Items(x))
                        lstAllItems.Items.Remove(lstAllItems.Items(x))
                    End If
                Next
                lstSelectedItems.SelectedIndex = -1

                ' update changed status items
                Changed = True
            End If
        End Sub       'btnAddClick

        '*********************************************************************
        '
        ' BtnAddAllClick Method
        '
        ' Raised by clicking add button on downlevel browser.
        '
        '*********************************************************************
        Sub BtnAddAllClick(ByVal s As [Object], ByVal e As EventArgs)
            ' Move the item
            Dim x As Integer = 0
            For x = lstAllItems.Items.Count - 1 To 0 Step -1
                lstSelectedItems.Items.Add(lstAllItems.Items(x))
                lstAllItems.Items.Remove(lstAllItems.Items(x))
            Next
            lstSelectedItems.SelectedIndex = -1

            ' update changed status items
            Changed = True
        End Sub       'btnAddAllClick


        '*********************************************************************
        '
        ' BtnRemoveClick Method
        '
        ' Raised by clicking remove button on downlevel browser.
        '
        '*********************************************************************
        Sub BtnRemoveClick(ByVal s As [Object], ByVal e As EventArgs)
            If lstSelectedItems.SelectedIndex <> -1 Then
                Dim x As Integer = 0
                For x = lstSelectedItems.Items.Count - 1 To 0 Step -1
                    If lstSelectedItems.Items(x).Selected Then
                        lstAllItems.Items.Add(lstSelectedItems.Items(x))
                        lstSelectedItems.Items.Remove(lstSelectedItems.Items(x))
                    End If
                Next

                ' update changed status items
                Changed = True
            End If
        End Sub       'btnRemoveClick

        '*********************************************************************
        '
        ' BtnRemoveAllClick Method
        '
        ' Raised by clicking remove button on downlevel browser.
        '
        '*********************************************************************
        Sub BtnRemoveAllClick(ByVal s As [Object], ByVal e As EventArgs)
            Dim x As Integer = 0
            For x = lstSelectedItems.Items.Count - 1 To 0 Step -1
                lstAllItems.Items.Add(lstSelectedItems.Items(x))
                lstSelectedItems.Items.Remove(lstSelectedItems.Items(x))
            Next

            ' update changed status items
            Changed = True
        End Sub       'btnRemoveAllClick


        '*********************************************************************
        '
        ' OnPreRender Method
        '
        ' Add reference to client-side script.
        '
        '*********************************************************************
        Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
            ' next two lines for conversion to framework 2.0
            Page.ClientScript.RegisterHiddenField(SelectedHelperId, String.Join(",", SelectedItems))
            Page.ClientScript.RegisterHiddenField(AllHelperId, String.Join(",", AllItems))
        End Sub       'OnPreRender
        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Displays downlevel content.
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            Dim item As String
            writer.RenderBeginTag(HtmlTextWriterTag.Tr) ' start the row
            ' Add Labels
            writer.RenderBeginTag(HtmlTextWriterTag.Td) ' Start first cell
            writer.Write(AllItemsText)
            writer.RenderEndTag() ' End fist cell

            writer.RenderBeginTag(HtmlTextWriterTag.Td) ' Start second cell
            writer.Write("&nbsp;")
            writer.RenderEndTag() ' End second cell

            writer.RenderBeginTag(HtmlTextWriterTag.Td) ' Start third cell
            writer.Write(SelectedItemsText)
            writer.RenderEndTag() ' End the td

            If DestinationSortable And Not SortOnChange Then ' add a forth cell
                writer.RenderBeginTag(HtmlTextWriterTag.Td)
                writer.RenderEndTag()
            End If

            writer.RenderEndTag() ' End the tr

            ' start the next row
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)

            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top") ' set the alignment to top
            writer.RenderBeginTag(HtmlTextWriterTag.Td) ' begin the first cell

            Dim mySelect As New HtmlControls.HtmlSelect ' All list box
            mySelect.Multiple = True
            mySelect.Attributes.Add("Style", "width: " & Size & "px")
            mySelect.Size = Rows.ToString
            mySelect.ID = ClientId + "_SrcList"
            For Each item In m_AllItems
                Dim myItem As New WebControls.ListItem(item.ToString, item.ToString)
                myItem.Attributes.Add("title", item.ToString())
                mySelect.Items.Add(myItem)
            Next item
            mySelect.RenderControl(writer)
            writer.RenderEndTag() ' end the first cell


            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top") ' begin the second cell
            writer.RenderBeginTag(HtmlTextWriterTag.Td) ' begin the second cell
            ' Add Button
            Dim myButton As New HtmlControls.HtmlInputButton
            myButton.Value = m_SelectButtonText
            myButton.Attributes.Add("onclick", String.Format(CultureInfo.InvariantCulture, "switchList(this.form.{0}_SrcList, this.form.{0}_DstList,'{1}')", ClientId, SortOnChange))
            myButton.Attributes.Add("class", "listPickerArrow")
            myButton.Attributes.Add("style", "width: " & m_ButtonWidth)
            myButton.RenderControl(writer)
            writer.WriteBreak()

            myButton = New HtmlControls.HtmlInputButton
            myButton.Value = m_SelectAllButtonText
            myButton.Attributes.Add("onclick", String.Format(CultureInfo.InvariantCulture, "switchAll(this.form.{0}_SrcList, this.form.{0}_DstList,'{1}')", ClientId, SortOnChange))
            myButton.Attributes.Add("class", "listPickerArrow")
            myButton.Attributes.Add("style", "width: " & m_ButtonWidth)
            myButton.RenderControl(writer)
            writer.WriteBreak()

            myButton = New HtmlControls.HtmlInputButton
            myButton.Value = m_DeselectButtonText
            myButton.Attributes.Add("onclick", String.Format(CultureInfo.InvariantCulture, "switchList(this.form.{0}_DstList, this.form.{0}_SrcList,'true')", ClientId))
            myButton.Attributes.Add("class", "listPickerArrow")
            myButton.Attributes.Add("style", "width: " & m_ButtonWidth)
            myButton.RenderControl(writer)
            writer.WriteBreak()

            myButton = New HtmlControls.HtmlInputButton
            myButton.Value = m_DeselectAllButtonText
            myButton.Attributes.Add("onclick", String.Format(CultureInfo.InvariantCulture, "switchAll(this.form.{0}_DstList, this.form.{0}_SrcList,'true')", ClientId))
            myButton.Attributes.Add("class", "listPickerArrow")
            myButton.Attributes.Add("style", "width: " & ButtonWidth)
            myButton.RenderControl(writer)

            writer.RenderEndTag() ' end the second cell

            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
            writer.RenderBeginTag(HtmlTextWriterTag.Td) 'begin third cell
            mySelect = New HtmlControls.HtmlSelect
            mySelect.Multiple = True
            mySelect.Attributes.Add("Style", "width: " & Size & "px")
            mySelect.Size = Rows.ToString
            mySelect.ID = ClientId + "_DstList"
            For Each item In m_SelectedItems
                Dim myItem As New WebControls.ListItem(item.ToString, item.ToString)
                mySelect.Items.Add(myItem)
                'writer.Write(String.Format(CultureInfo.InvariantCulture, "<option value=""{0}"">{0}</option>", item))
            Next item
            mySelect.RenderControl(writer)

            writer.RenderEndTag() ' end third cell

            If DestinationSortable And Not SortOnChange Then ' add a forth cell
                writer.RenderBeginTag(HtmlTextWriterTag.Td)
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
                writer.RenderBeginTag(HtmlTextWriterTag.Td)
                myButton = New HtmlControls.HtmlInputButton
                myButton.Value = "▲"
                myButton.Attributes.Add("onclick", String.Format(CultureInfo.InvariantCulture, "moveUp(this.form.{0}_DstList)", ClientId))
                myButton.Attributes.Add("class", "listPickerArrow")
                myButton.Attributes.Add("style", "width: " + m_ButtonWidth)
                myButton.RenderControl(writer)
                writer.WriteBreak()

                myButton = New HtmlControls.HtmlInputButton
                myButton.Value = "▼"
                myButton.Attributes.Add("onclick", String.Format(CultureInfo.InvariantCulture, "moveDown(this.form.{0}_DstList)", ClientId))
                myButton.Attributes.Add("class", "listPickerArrow")
                myButton.Attributes.Add("style", "width: " + m_ButtonWidth)
                myButton.RenderControl(writer)
                writer.WriteBreak()
                writer.RenderEndTag()



                writer.RenderEndTag()
            End If

            writer.RenderEndTag() ' end the tr
        End Sub
    End Class
End Namespace
