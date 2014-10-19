Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports GrowthWare.WebSupport.CustomWebControls.Designers
Imports System.Globalization
Imports System.Collections.Specialized

Namespace CustomWebControls
    <DefaultProperty("Text"), Designer(GetType(CustomDesigner)), ToolboxData("<{0}:ListPicker runat=server></{0}:ListPicker>")> _
    Public Class ListPicker
        Inherits WebControl
        Implements IPostBackDataHandler, INamingContainer

        ''' <summary>
        ''' Gets or sets the text.
        ''' </summary>
        ''' <value>The text.</value>
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

        ''' <summary>
        ''' Gets or sets a value indicating whether this <see cref="ListPicker" /> is changed.
        ''' </summary>
        ''' <value><c>true</c> if changed; otherwise, <c>false</c>.</value>
        ''' <returns>Boolean</returns>
        ''' <remarks>True when new item picked. </remarks>
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

        ''' <summary>
        ''' Gets or sets the selected items text.
        ''' </summary>
        ''' <value>The selected items text.</value>
        Public Property SelectedItemsText() As String
            Get
                If ViewState("SelectedItemsText") Is Nothing Then
                    Return "Selected Items"
                Else
                    Return CStr(ViewState("SelectedItemsText"))
                End If
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then ViewState("SelectedItemsText") = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets all items text.
        ''' </summary>
        ''' <value>All items text.</value>
        Public Property AllItemsText() As String
            Get
                If ViewState("AllItemsText") Is Nothing Then
                    Return "All Items"
                Else
                    Return CStr(ViewState("AllItemsText"))
                End If
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then ViewState("AllItemsText") = Value.Trim
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

        ''' <summary>
        ''' Gets or sets the select button text.
        ''' </summary>
        ''' <value>The select button text.</value>
        Property SelectButtonText() As String
            Get
                Return m_SelectButtonText
            End Get

            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then m_SelectButtonText = Value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the select all button text.
        ''' </summary>
        ''' <value>The select all button text.</value>
        Property SelectAllButtonText() As String
            Get
                Return m_SelectAllButtonText
            End Get

            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then m_SelectAllButtonText = Value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the de select button text.
        ''' </summary>
        ''' <value>The de select button text.</value>
        Property DeselectButtonText() As String
            Get
                Return m_DeselectButtonText
            End Get

            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then m_DeselectButtonText = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the de select all button text.
        ''' </summary>
        ''' <value>The de select all button text.</value>
        Property DeselectAllButtonText() As String
            Get
                Return m_DeselectAllButtonText
            End Get

            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then m_DeselectAllButtonText = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the width of the button.
        ''' </summary>
        ''' <value>The width of the button.</value>
        Property ButtonWidth() As String
            Get
                Return m_ButtonWidth
            End Get
            Set(ByVal value As String)
                If Not String.IsNullOrEmpty(value) Then
                    If value.Trim.EndsWith("px", StringComparison.OrdinalIgnoreCase) Then
                        m_ButtonWidth = value.Trim
                    Else
                        m_ButtonWidth = value.Trim & "px"
                    End If
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the size.
        ''' </summary>
        ''' <value>The size.</value>
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

        ''' <summary>
        ''' Gets or sets the rows.
        ''' </summary>
        ''' <value>The rows.</value>
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

        ''' <summary>
        ''' Gets or sets the state of the selected.
        ''' </summary>
        ''' <value>The state of the selected.</value>
        Public Property SelectedState() As String
            Get
                If ViewState("selectedState") Is Nothing Then
                    Return String.Empty
                Else
                    Return CStr(ViewState("selectedState"))
                End If
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then ViewState("selectedState") = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the data source.
        ''' </summary>
        ''' <value>The data source.</value>
        Public Property DataSource() As IEnumerable
            Get
                Return m_DataSource
            End Get
            Set(ByVal Value As IEnumerable)
                m_DataSource = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the data field.
        ''' </summary>
        ''' <value>The data field.</value>
        Public Property DataField() As String
            Get
                Return m_DataField
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then m_DataField = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets the selected helper ID.
        ''' </summary>
        ''' <value>The selected helper ID.</value>
        Protected ReadOnly Property SelectedHelperId() As String
            Get
                Return ClientID + "_SelectedState"
            End Get
        End Property

        ''' <summary>
        ''' Gets all helper ID.
        ''' </summary>
        ''' <value>All helper ID.</value>
        Protected ReadOnly Property AllHelperId() As String
            Get
                Return ClientID + "_AllState"
            End Get
        End Property

        ''' <summary>
        ''' Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        ''' </summary>
        ''' <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        Protected Overrides Sub OnInit(ByVal e As EventArgs)
            If Not (Page Is Nothing) Then
                MyBase.OnInit(e)
                Page.RegisterRequiresPostBack(Me)
                Dim scriptUrl As String = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "GrowthWare.WebSupport.ListPicker.js")
                Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "GrowthWareListPicker", scriptUrl)
            End If
        End Sub       'OnInit


        ''' <summary>
        ''' Loads the post data.
        ''' </summary>
        ''' <param name="postDataKey">The post data key.</param>
        ''' <param name="postCollection">The values.</param>
        ''' <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        Public Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As NameValueCollection) As Boolean Implements IPostBackDataHandler.LoadPostData
            If postCollection Is Nothing Then Throw New ArgumentNullException("postCollection", "postCollection cannot be a null reference (Nothing in Visual Basic)!")
            Dim _allState As String
            Dim _selectedState As String

            ' return if null
            If postCollection(AllHelperId) Is Nothing Then
                Return False
            End If
            _allState = postCollection(AllHelperId).Trim()
            _selectedState = postCollection(SelectedHelperId).Trim()
            If String.IsNullOrEmpty(_allState) Then
                m_AllItems.Clear()
            Else
                m_AllItems = New ArrayList(_allState.Split(","c))
            End If
            If String.IsNullOrEmpty(_selectedState) Then
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

        ''' <summary>
        ''' When implemented by a class, signals the server control to notify the ASP.NET application that the state of the control has changed.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RaisePostDataChangedEvent() Implements IPostBackDataHandler.RaisePostDataChangedEvent
            OnListChanged(EventArgs.Empty)
        End Sub

        ''' <summary>
        ''' Raises the <see cref="E:ListChanged" /> event.
        ''' </summary>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        Protected Overridable Sub OnListChanged(ByVal e As EventArgs)
            RaiseEvent ListChanged(Me, e)
        End Sub       'OnListChanged

        ''' <summary>
        ''' Raises the <see cref="E:System.Web.UI.Control.DataBinding" /> event.
        ''' </summary>
        ''' <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
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
        End Sub

        ''' <summary>
        ''' Gets or sets the selected items.
        ''' </summary>
        ''' <value>The selected items.</value>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")>
        Public Property SelectedItems() As String()
            Get
                Return CType(m_SelectedItems.ToArray(GetType(String)), String())
            End Get
            Set(ByVal Value As String())
                m_SelectedItems = New ArrayList(Value)
                SelectedState = String.Join(",", Value)
            End Set
        End Property

        ''' <summary>
        ''' Gets all items.
        ''' </summary>
        ''' <value>All items.</value>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")>
        Public ReadOnly Property AllItems() As String()
            Get
                Return CType(m_AllItems.ToArray(GetType(String)), String())
            End Get
        End Property

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ListPicker" /> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(HtmlTextWriterTag.Table)
        End Sub

        ''' <summary>
        ''' Add button click event.
        ''' </summary>
        ''' <param name="addButton">The addButton.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        Sub AddButtonClick(ByVal addButton As [Object], ByVal e As EventArgs)
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
        End Sub

        ''' <summary>
        ''' Add all button click event
        ''' </summary>
        ''' <param name="addAllButton">The addAllButton.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        Sub AddAllButtonClick(ByVal addAllButton As [Object], ByVal e As EventArgs)
            ' Move the item
            Dim x As Integer = 0
            For x = lstAllItems.Items.Count - 1 To 0 Step -1
                lstSelectedItems.Items.Add(lstAllItems.Items(x))
                lstAllItems.Items.Remove(lstAllItems.Items(x))
            Next
            lstSelectedItems.SelectedIndex = -1

            ' update changed status items
            Changed = True
        End Sub


        ''' <summary>
        ''' Remove button cClick event.
        ''' </summary>
        ''' <param name="removeButton">The removeButton.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        Sub RemoveButtonClick(ByVal removeButton As [Object], ByVal e As EventArgs)
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
        End Sub

        ''' <summary>
        ''' Remove all button click event.
        ''' </summary>
        ''' <param name="removeAllButton">The removeAllButton.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        Sub RemoveAllButtonClick(ByVal removeAllButton As [Object], ByVal e As EventArgs)
            Dim x As Integer = 0
            For x = lstSelectedItems.Items.Count - 1 To 0 Step -1
                lstAllItems.Items.Add(lstSelectedItems.Items(x))
                lstSelectedItems.Items.Remove(lstSelectedItems.Items(x))
            Next

            ' update changed status items
            Changed = True
        End Sub


        ''' <summary>
        ''' Raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        ''' </summary>
        ''' <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
            ' next two lines for conversion to framework 2.0
            Page.ClientScript.RegisterHiddenField(SelectedHelperId, String.Join(",", SelectedItems))
            Page.ClientScript.RegisterHiddenField(AllHelperId, String.Join(",", AllItems))
        End Sub

        ''' <summary>
        ''' Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        ''' </summary>
        ''' <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the output stream to render HTML content on the client.</param>
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            If writer Is Nothing Then Throw New ArgumentNullException("writer", "writer cannot be a null reference (Nothing in Visual Basic)!")
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

            Using mySelect As New HtmlControls.HtmlSelect ' All list box
                mySelect.Multiple = True
                mySelect.Attributes.Add("Style", "width: " & Size & "px")
                mySelect.Size = Rows.ToString(CultureInfo.InvariantCulture)
                mySelect.ID = ClientID + "_SrcList"
                For Each item In m_AllItems
                    Dim myItem As New WebControls.ListItem(item.ToString(CultureInfo.InvariantCulture), item.ToString(CultureInfo.InvariantCulture))
                    myItem.Attributes.Add("title", item.ToString(CultureInfo.InvariantCulture))
                    mySelect.Items.Add(myItem)
                Next item
                mySelect.RenderControl(writer)
            End Using
            writer.RenderEndTag() ' end the first cell


            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top") ' begin the second cell
            writer.RenderBeginTag(HtmlTextWriterTag.Td) ' begin the second cell
            ' Add Button
            Using myButton As New HtmlControls.HtmlInputButton
                myButton.Value = m_SelectButtonText
                myButton.Attributes.Add("onclick", String.Format(CultureInfo.InvariantCulture, "GW.ListPicker.switchList(this.form.{0}_SrcList, this.form.{0}_DstList,'{1}')", ClientID, SortOnChange))
                myButton.Attributes.Add("class", "listPickerArrow")
                myButton.Attributes.Add("style", "width: " & m_ButtonWidth)
                myButton.RenderControl(writer)
            End Using
            writer.WriteBreak()

            Using myButton As New HtmlControls.HtmlInputButton
                myButton.Value = m_SelectAllButtonText
                myButton.Attributes.Add("onclick", String.Format(CultureInfo.InvariantCulture, "GW.ListPicker.switchAll(this.form.{0}_SrcList, this.form.{0}_DstList,'{1}')", ClientID, SortOnChange))
                myButton.Attributes.Add("class", "listPickerArrow")
                myButton.Attributes.Add("style", "width: " & m_ButtonWidth)
                myButton.RenderControl(writer)
            End Using
            writer.WriteBreak()

            Using myButton As New HtmlControls.HtmlInputButton
                myButton.Value = m_DeselectButtonText
                myButton.Attributes.Add("onclick", String.Format(CultureInfo.InvariantCulture, "GW.ListPicker.switchList(this.form.{0}_DstList, this.form.{0}_SrcList,'true')", ClientID))
                myButton.Attributes.Add("class", "listPickerArrow")
                myButton.Attributes.Add("style", "width: " & m_ButtonWidth)
                myButton.RenderControl(writer)
            End Using
            writer.WriteBreak()

            Using myButton As New HtmlControls.HtmlInputButton
                myButton.Value = m_DeselectAllButtonText
                myButton.Attributes.Add("onclick", String.Format(CultureInfo.InvariantCulture, "GW.ListPicker.switchAll(this.form.{0}_DstList, this.form.{0}_SrcList,'true')", ClientID))
                myButton.Attributes.Add("class", "listPickerArrow")
                myButton.Attributes.Add("style", "width: " & ButtonWidth)
                myButton.RenderControl(writer)
            End Using

            writer.RenderEndTag() ' end the second cell

            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
            writer.RenderBeginTag(HtmlTextWriterTag.Td) 'begin third cell
            Using mySelect As New HtmlControls.HtmlSelect
                mySelect.Multiple = True
                mySelect.Attributes.Add("Style", "width: " & Size & "px")
                mySelect.Size = Rows.ToString(CultureInfo.InvariantCulture)
                mySelect.ID = ClientID + "_DstList"
                For Each item In m_SelectedItems
                    Dim myItem As New WebControls.ListItem(item.ToString(CultureInfo.InvariantCulture), item.ToString(CultureInfo.InvariantCulture))
                    mySelect.Items.Add(myItem)
                    'writer.Write(String.Format(CultureInfo.InvariantCulture, "<option value=""{0}"">{0}</option>", item))
                Next item
                mySelect.RenderControl(writer)
            End Using

            writer.RenderEndTag() ' end third cell

            If DestinationSortable And Not SortOnChange Then ' add a forth cell
                writer.RenderBeginTag(HtmlTextWriterTag.Td)
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
                writer.RenderBeginTag(HtmlTextWriterTag.Td)
                Using myButton As New HtmlControls.HtmlInputButton
                    myButton.Value = "▲"
                    myButton.Attributes.Add("onclick", String.Format(CultureInfo.InvariantCulture, "GW.ListPicker.moveUp(this.form.{0}_DstList)", ClientID))
                    myButton.Attributes.Add("class", "listPickerArrow")
                    myButton.Attributes.Add("style", "width: " + m_ButtonWidth)
                    myButton.RenderControl(writer)
                End Using
                writer.WriteBreak()

                Using myButton As New HtmlControls.HtmlInputButton
                    myButton.Value = "▼"
                    myButton.Attributes.Add("onclick", String.Format(CultureInfo.InvariantCulture, "GW.ListPicker.moveDown(this.form.{0}_DstList)", ClientID))
                    myButton.Attributes.Add("class", "listPickerArrow")
                    myButton.Attributes.Add("style", "width: " + m_ButtonWidth)
                    myButton.RenderControl(writer)
                End Using
                writer.WriteBreak()
                writer.RenderEndTag()
                writer.RenderEndTag()
            End If

            writer.RenderEndTag() ' end the tr
        End Sub
    End Class
End Namespace
