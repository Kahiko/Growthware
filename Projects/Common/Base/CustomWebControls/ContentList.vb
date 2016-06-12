Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Collections
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Diagnostics
Imports DALModel.Base.Accounts.Security
Imports DALModel.Base.Modules

Namespace CustomWebControls
    '*********************************************************************
    '
    ' ContentList Class
    '
    ' The ContentList control is used to display all the content items
    ' in the applicatin. The ContentList control has support
    ' for paging using the database custom paging mechanism.
    '
    '*********************************************************************
    <Designer(GetType(CustomDesigner)), DefaultProperty("Text"), ToolboxData("<{0}:ContentList runat=server></{0}:ContentList>")> _
    Public Class ContentList
        Inherits WebControl
        Implements INamingContainer, IPostBackEventHandler

        Private _itemTemplate As ITemplate = Nothing
        Private _separatorTemplate As ITemplate = Nothing
        Private _noContentTemplate As ITemplate = Nothing
        Private _headerTemplate As ITemplate = Nothing
        Private _footerTemplate As ITemplate = Nothing
        Private _alternatingItemTemplate As ITemplate = Nothing
        Private _colContentItems As New ArrayList
        Private _moduleProfileInfo As MModuleProfileInfo
        Private _accountSecurityInfo As MAccountSecurityInfo
        Private _totalPages As Integer
        Private _dataSource As ArrayList = Nothing
        Private _repeatLayout As RepeatLayout = RepeatLayout.Flow
        Private _repeatColumns As Integer = 1
        Private _pagerText As String = String.Empty

        Private Shared EventItemDataBound As New Object



        '*********************************************************************
        '
        ' AddParsedSubObject Method
        '
        ' Ignore everything except for content items between the opening
        ' and closing ContentList tags.
        '
        '*********************************************************************
        Protected Overrides Sub AddParsedSubObject(ByVal obj As [Object])

            If TypeOf obj Is ContentItem Then
                Controls.Add(CType(obj, ContentItem))
            End If
        End Sub 'AddParsedSubObject


        Public Event SelectedIndexChanged As EventHandler
        Public Event ItemCommand As EventHandler



        '*********************************************************************
        '
        ' OnSelectedIndexChanged Method
        '
        ' This method executes when a user clicks a page number. You can
        ' override to do something custom.
        '
        '*********************************************************************
        Protected Overridable Sub OnSelectedIndexChanged(ByVal e As EventArgs)
            RaiseEvent SelectedIndexChanged(Me, e)
        End Sub 'OnSelectedIndexChanged



        '*********************************************************************
        '
        ' RaisePostBackEvent Method
        '
        ' This method is called after the user clicks a page number
        '
        '*********************************************************************
        Public Sub RaisePostBackEvent(ByVal eventArgument As String) Implements IPostBackEventHandler.RaisePostBackEvent
            If eventArgument = "next" Then
                CurrentPage += 1
                OnSelectedIndexChanged(EventArgs.Empty)
                Return
            End If

            If eventArgument = "prev" Then
                CurrentPage -= 1
                OnSelectedIndexChanged(EventArgs.Empty)
                Return
            End If

            CurrentPage = Int32.Parse(eventArgument)
            OnSelectedIndexChanged(EventArgs.Empty)
        End Sub 'RaisePostBackEvent




        '*********************************************************************
        '
        ' ItemDataBound Event
        '
        ' We need this event to perform custom actions during databinding
        '
        '*********************************************************************
        Public Event ItemDataBound As ContentListItemEventHandler




        '*********************************************************************
        '
        ' OnItemDataBound Method
        '
        ' We handle the event raised by this method in the Comments control
        '
        '*********************************************************************
        Protected Overridable Sub OnItemDataBound(ByVal e As ContentListItemEventArgs)
            RaiseEvent ItemDataBound(Me, e)
        End Sub 'OnItemDataBound



        '*********************************************************************
        '
        ' OnItemCommand Method
        '
        ' This method is invoked when a child control raises an event.
        ' For example, the ItemEditContent control fires this method
        '
        '*********************************************************************
        Protected Overridable Sub OnItemCommand(ByVal s As [Object], ByVal e As EventArgs)
            RaiseEvent ItemCommand(s, e)
        End Sub 'OnItemCommand


        '*********************************************************************
        '
        ' OnBubbleEvent Method
        '
        ' This method is executed when a child control bubbles an event
        '
        '*********************************************************************
        Protected Overrides Function OnBubbleEvent(ByVal s As Object, ByVal e As EventArgs) As Boolean
            OnItemCommand(s, e)
            Return False
        End Function 'OnBubbleEvent


        '*********************************************************************
        '
        ' DataSource Property
        '
        ' You can only use ArrayLists as a Data Source
        '
        '*********************************************************************

        Public Property DataSource() As ArrayList
            Get
                Return _dataSource
            End Get
            Set(ByVal Value As ArrayList)
                _dataSource = Value
            End Set
        End Property


        '*********************************************************************
        '
        ' RepeatLayout Property
        '
        ' Like the DataList control, the ContentList supports both flow
        ' and table layouts.
        '
        '*********************************************************************

        Public Property RepeatLayout() As RepeatLayout
            Get
                Return _repeatLayout
            End Get
            Set(ByVal Value As RepeatLayout)
                _repeatLayout = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' RepeatColumns Property
        '
        ' When table layout, determines the number of columns to display.
        ' This property is used in the PhotoGallery to display photo thumbnails.
        '
        '*********************************************************************

        Public Property RepeatColumns() As Integer
            Get
                Return _repeatColumns
            End Get
            Set(ByVal Value As Integer)
                _repeatColumns = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' CurrentPage Property
        '
        ' The current page selected in the pager
        '
        '*********************************************************************

        Public Property CurrentPage() As Integer
            Get
                If ViewState("CurrentPage") Is Nothing Then
                    Return 1
                Else
                    Return Fix(ViewState("CurrentPage"))
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("CurrentPage") = Value
            End Set
        End Property

        '*********************************************************************
        '
        ' TotalRecords Property
        '
        ' We need the total records to display the proper number of pages
        ' in the pager
        '
        '*********************************************************************

        Public Property TotalRecords() As Integer
            Get
                If ViewState("TotalRecords") Is Nothing Then
                    Return 0
                End If
                Return Fix(ViewState("TotalRecords"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("TotalRecords") = Value
            End Set
        End Property



        '*********************************************************************
        '
        ' NumItems Property
        '
        ' This is the number of content items that we displayed in the
        ' page. This property is used internally to track the number of
        ' items to display in postback scenarios.
        '
        '*********************************************************************       

        Private Property NumItems() As Integer
            Get
                If ViewState("NumItems") Is Nothing Then
                    Return 0
                Else
                    Return Fix(ViewState("NumItems"))
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("NumItems") = Value
            End Set
        End Property



        '*********************************************************************
        '
        ' PagerText Property
        '
        ' The text used to display Page blah of blah
        ' A typical value is "Page {0} of {1}"
        '
        '*********************************************************************

        Public Property PagerText() As String
            Get
                Return _pagerText
            End Get
            Set(ByVal Value As String)
                _pagerText = Value
            End Set
        End Property


        '*********************************************************************
        '
        ' TagKey Property
        '
        ' If Flow Layout then use Span, otherwise use Table.
        '
        '*********************************************************************

        Protected Overrides ReadOnly Property TagKey() As HtmlTextWriterTag
            Get
                If _repeatLayout = RepeatLayout.Flow Then
                    Return HtmlTextWriterTag.Span
                Else
                    Return HtmlTextWriterTag.Table
                End If
            End Get
        End Property



        '*********************************************************************
        '
        ' ContentList Constructor
        '
        ' Grab the SectionInfo and UserInfo objects.
        '
        '*********************************************************************
        Public Sub New()
            If Not (Context Is Nothing) Then
                _moduleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)
                _accountSecurityInfo = CType(Context.Items("AccountSecurityInfo"), MAccountSecurityInfo)

            Else
                _moduleProfileInfo = New MModuleProfileInfo
            End If
        End Sub 'New



        '*********************************************************************
        '
        ' DataBind Method
        '
        ' When databind is called, create the control hiearchy from the
        ' database.
        '
        '*********************************************************************
        Public Overrides Sub DataBind()
            CreateControlHierarchy(True)

            ' Prevent CreateChildControls from executing            
            ChildControlsCreated = True
        End Sub 'DataBind



        '*********************************************************************
        '
        ' CreateChildControls Method
        '
        ' When databind is not called, create the control hiearchy from
        ' view state.
        '
        '*********************************************************************
        Protected Overrides Sub CreateChildControls()
            CreateControlHierarchy(False)
        End Sub 'CreateChildControls




        '*********************************************************************
        '
        ' CreateControlHiearchy Method
        '
        ' This is where all the work is done. Add all the content items
        ' into the templates to display the contents of the control.
        '
        '*********************************************************************
        Sub CreateControlHierarchy(ByVal useDataSource As Boolean)
            Dim itemCount As Integer
            Dim item As [Object] = Nothing
            Dim ctlItem As ContentItem

            ' don't do anything if no ItemTemplate
            If _itemTemplate Is Nothing Then
                Return
            End If
            ' Initialize content items collection
            _colContentItems = New ArrayList

            ' if using data source, then populate arraylist
            If useDataSource Then
                If _dataSource Is Nothing Then
                    Return
                End If
                ' Clear any state
                Controls.Clear()
                ClearChildViewState()
                ' retrieve content count
                itemCount = _dataSource.Count
            Else
                itemCount = NumItems
            End If

            ' if no content, then show nocontent template
            If itemCount = 0 Then
                If Not (_noContentTemplate Is Nothing) Then
                    ctlItem = New ContentItem(0, Nothing, ListItemType.Header)
                    NoContentTemplate.InstantiateIn(ctlItem)
                    Controls.Add(ctlItem)
                    _colContentItems.Add(ctlItem)
                End If
            Else
                ' Create header template
                If Not (_headerTemplate Is Nothing) Then
                    ctlItem = New ContentItem(0, Nothing, ListItemType.Header)
                    HeaderTemplate.InstantiateIn(ctlItem)
                    Controls.Add(ctlItem)
                    _colContentItems.Add(ctlItem)
                End If

                ' loop through content, creating templates
                Dim itemIndex As Integer
                For itemIndex = 0 To itemCount - 1

                    ' if dataSource, then grab item
                    If useDataSource Then
                        item = _dataSource(itemIndex)
                    End If

                    ' Add the SeparatorTemplate
                    If itemIndex > 0 AndAlso Not (_separatorTemplate Is Nothing) Then
                        ctlItem = New ContentItem(itemIndex, Nothing, ListItemType.Separator)
                        SeparatorTemplate.InstantiateIn(ctlItem)
                        Controls.Add(ctlItem)
                        _colContentItems.Add(ctlItem)
                    End If


                    If itemIndex Mod 2 <> 0 AndAlso Not (_alternatingItemTemplate Is Nothing) Then

                        ' Add the AlternatingItemTemplate
                        ctlItem = New ContentItem(itemIndex, item, ListItemType.AlternatingItem)
                        AlternatingItemTemplate.InstantiateIn(ctlItem)
                        Controls.Add(ctlItem)
                        _colContentItems.Add(ctlItem)
                    Else
                        ' Add the ItemTemplate
                        ctlItem = New ContentItem(itemIndex, item, ListItemType.Item)
                        ItemTemplate.InstantiateIn(ctlItem)
                        Controls.Add(ctlItem)
                        _colContentItems.Add(ctlItem)
                    End If

                    ' Perform databinding
                    If useDataSource Then
                        Dim e As New ContentListItemEventArgs(ctlItem)
                        ctlItem.DataBind()
                        OnItemDataBound(e)
                    End If
                Next itemIndex

                ' Create footer template
                If Not (_footerTemplate Is Nothing) Then
                    ctlItem = New ContentItem(0, Nothing, ListItemType.Footer)
                    FooterTemplate.InstantiateIn(ctlItem)
                    Controls.Add(ctlItem)
                    _colContentItems.Add(ctlItem)
                End If
            End If

            ' store the number of items created in viewstate for postback scenarios
            NumItems = itemCount
        End Sub 'CreateControlHierarchy





        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Render the containing table and pager.
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)

            Dim repeatCounter As Integer = 0

            ' open main table row
            If _repeatLayout = RepeatLayout.Table Then
                writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            End If

            ' display all the content items
            Dim item As ContentItem
            For Each item In _colContentItems

                repeatCounter += 1

                ' open table cell (and possibly row)
                If _repeatLayout = RepeatLayout.Table Then
                    If repeatCounter > _repeatColumns Then
                        writer.RenderEndTag() ' for last open row
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr) ' for new open row
                        repeatCounter = 0
                    End If
                    writer.RenderBeginTag(HtmlTextWriterTag.Td)
                End If



                item.RenderControl(writer)


                ' close table cell
                If _repeatLayout = RepeatLayout.Table Then
                    writer.RenderEndTag()
                End If
            Next item


            ' close main table row
            If _repeatLayout = RepeatLayout.Table Then
                writer.RenderEndTag()
            End If
            ' add the pager ui
            RenderPager(writer)
        End Sub 'RenderContents





        '*********************************************************************
        '
        ' RenderPager Method
        '
        ' Render the page numbers for the pager.
        '
        '*********************************************************************
        Protected Sub RenderPager(ByVal writer As HtmlTextWriter)
            ' Calculate total Pages
            _totalPages = TotalRecords \ _moduleProfileInfo.RecordsPerPage


            ' Now do a mod for any remainder
            If (TotalRecords Mod _moduleProfileInfo.RecordsPerPage) > 0 Then
                _totalPages += 1
            End If
            ' don't render anything if only 1 page
            If _totalPages < 2 Then
                Return
            End If

            ' Open main pager table
            If _repeatLayout = RepeatLayout.Flow Then
                writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
                writer.RenderBeginTag(HtmlTextWriterTag.Table)
            End If

            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom")
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, _repeatColumns.ToString())

            writer.RenderBeginTag(HtmlTextWriterTag.Td)

            writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
            writer.RenderBeginTag(HtmlTextWriterTag.Table)
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.RenderBeginTag(HtmlTextWriterTag.Td)

            ' Open page numbers table
            writer.RenderBeginTag(HtmlTextWriterTag.Table)
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)

            ' show previous link
            If CurrentPage > 1 Then
                writer.RenderBeginTag(HtmlTextWriterTag.Td)

                writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + Page.GetPostBackEventReference(Me, "prev"))
                writer.RenderBeginTag(HtmlTextWriterTag.A)
                writer.Write("&lt;&lt;")
                writer.RenderEndTag()

                writer.RenderEndTag()
            End If


            ' if less than 11 pages, render all page numbers
            If _totalPages < 11 Then
                RenderAllPages(writer)
                ' otherwise, do complicated stuff
            Else
                RenderPageRange(writer)
            End If
            ' show next link
            If CurrentPage < _totalPages Then
                writer.RenderBeginTag(HtmlTextWriterTag.Td)

                writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + Page.GetPostBackEventReference(Me, "next"))
                writer.RenderBeginTag(HtmlTextWriterTag.A)
                writer.Write("&gt;&gt;")
                writer.RenderEndTag()

                writer.RenderEndTag()
            End If


            ' close page numbers table       
            writer.RenderEndTag()
            writer.RenderEndTag()
            writer.RenderEndTag()


            ' if pagerText exists, show it
            If PagerText <> String.Empty Then
                writer.AddAttribute(HtmlTextWriterAttribute.Align, "right")
                writer.RenderBeginTag(HtmlTextWriterTag.Td)

                writer.Write(PagerText, CurrentPage, _totalPages)

                writer.RenderEndTag()
            End If

            ' close table       
            writer.RenderEndTag()
            writer.RenderEndTag()

            writer.RenderEndTag()
            writer.RenderEndTag()

            If _repeatLayout = RepeatLayout.Flow Then
                writer.RenderEndTag()
            End If
        End Sub 'RenderPager





        '*********************************************************************
        '
        ' RenderAllPages Method
        '
        ' This is the easy case, we just loop through and display
        ' all the pages.
        '
        '*********************************************************************
        Private Sub RenderAllPages(ByVal writer As HtmlTextWriter)
            ' display page numbers
            Dim pageNumber As Integer
            For pageNumber = 1 To _totalPages
                AddPageNumber(writer, pageNumber)
            Next pageNumber
        End Sub 'RenderAllPages




        '*********************************************************************
        '
        ' RenderPageRange Method
        '
        ' This is the hard case. Too many pages to display them all so
        ' instead we display ellipsis.
        '
        '*********************************************************************
        Private Sub RenderPageRange(ByVal writer As HtmlTextWriter)
            Dim i As Integer
            Dim lowerbound As Integer = CurrentPage - 1
            Dim upperbound As Integer = CurrentPage + 1

            ' range check
            If lowerbound < 1 Then
                lowerbound = 1
            End If
            If upperbound > _totalPages Then
                upperbound = _totalPages
            End If
            ' Show Lower Range
            ' if the currentpage is closer to the beginning than to the end, 
            ' then show all pages to the current page, otherwise, just
            ' display first 3 pages
            If CurrentPage < _totalPages - CurrentPage Then
                For i = 1 To lowerbound - 1
                    AddPageNumber(writer, i)
                Next i
            Else
                For i = 1 To 3
                    If (i < lowerbound) Then
                        AddPageNumber(writer, i)
                    End If
                Next i ' Display ellipsis    
                If lowerbound > 4 Then
                    writer.RenderBeginTag(HtmlTextWriterTag.Td)
                    writer.Write("...")
                    writer.RenderEndTag()
                End If
            End If

            ' show mid range
            For i = lowerbound To upperbound
                AddPageNumber(writer, i)
            Next i

            ' Show Upper Range
            ' if the currentpage is closer to the end than to the beginning, 
            ' then show all pages from the end to the current page, otherwise, just
            ' display first 3 pages
            If upperbound < _totalPages Then
                If CurrentPage > _totalPages - CurrentPage Then
                    For i = upperbound + 1 To _totalPages
                        AddPageNumber(writer, i)
                    Next i
                Else
                    ' Display ellipsis    
                    If upperbound < _totalPages - 2 Then
                        writer.RenderBeginTag(HtmlTextWriterTag.Td)
                        writer.Write("...")
                        writer.RenderEndTag()
                    End If

                    For i = _totalPages - 1 To _totalPages
                        AddPageNumber(writer, i)
                    Next i
                End If
            End If
        End Sub 'RenderPageRange





        '*********************************************************************
        '
        ' AddPageNumber Method
        '
        ' Display an individual page number.
        '
        '*********************************************************************
        Sub AddPageNumber(ByVal writer As HtmlTextWriter, ByVal pageNumber As Integer)
            writer.RenderBeginTag(HtmlTextWriterTag.Td)

            If pageNumber = CurrentPage Then
                writer.Write("[")
            End If
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + Page.GetPostBackEventReference(Me, pageNumber.ToString()))
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID)
            writer.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID + "_" + pageNumber.ToString())

            writer.RenderBeginTag(HtmlTextWriterTag.A)
            writer.Write(pageNumber)
            writer.RenderEndTag()
            If pageNumber = CurrentPage Then
                writer.Write("]")
            End If
            writer.RenderEndTag()
        End Sub 'AddPageNumber



        '*********************************************************************
        '
        ' ItemTemplate Property
        '
        ' Represents the ItemTemplate.
        '
        '*********************************************************************

        Public Property ItemTemplate() As ITemplate
            Get
                Return _itemTemplate
            End Get
            Set(ByVal Value As ITemplate)
                _itemTemplate = Value
            End Set
        End Property



        '*********************************************************************
        '
        ' SeparatorTemplate Property
        '
        ' Represents the SeparatorTemplate.
        '
        '*********************************************************************

        Public Property SeparatorTemplate() As ITemplate
            Get
                Return _separatorTemplate
            End Get
            Set(ByVal Value As ITemplate)
                _separatorTemplate = Value
            End Set
        End Property


        '*********************************************************************
        '
        ' NoContentTemplate Property
        '
        ' Represents the NoContentTemplate.
        ' Whatever you add to the NoContentTemplate is displayed when
        ' there are no records.
        '
        '*********************************************************************

        Public Property NoContentTemplate() As ITemplate
            Get
                Return _noContentTemplate
            End Get
            Set(ByVal Value As ITemplate)
                _noContentTemplate = Value
            End Set
        End Property


        '*********************************************************************
        '
        ' HeaderTemplate Property
        '
        ' Represents the HeaderTemplate.
        '
        '*********************************************************************

        Public Property HeaderTemplate() As ITemplate
            Get
                Return _headerTemplate
            End Get
            Set(ByVal Value As ITemplate)
                _headerTemplate = Value
            End Set
        End Property


        '*********************************************************************
        '
        ' FooterTemplate Property
        '
        ' Represents the FooterTemplate.
        '
        '*********************************************************************

        Public Property FooterTemplate() As ITemplate
            Get
                Return _footerTemplate
            End Get
            Set(ByVal Value As ITemplate)
                _footerTemplate = Value
            End Set
        End Property



        '*********************************************************************
        '
        ' AlternatingItemTemplate Property
        '
        ' Represents the AlternatingItemTemplate.
        '
        '*********************************************************************

        Public Property AlternatingItemTemplate() As ITemplate
            Get
                Return _alternatingItemTemplate
            End Get
            Set(ByVal Value As ITemplate)
                _alternatingItemTemplate = Value
            End Set
        End Property
    End Class 'ContentList




    '*********************************************************************
    '
    ' ContentListItemEventArgs Class
    '
    ' This class is used when an ItemDataBound event is raised to pass
    ' information about the item that raised the event.
    '
    '*********************************************************************

    Public NotInheritable Class ContentListItemEventArgs
        Inherits EventArgs

        Private _item As ContentItem


        Public Sub New(ByVal item As ContentItem)
            _item = item
        End Sub 'New


        Public ReadOnly Property Item() As ContentItem
            Get
                Return _item
            End Get
        End Property
    End Class 'ContentListItemEventArgs



    Public Delegate Sub ContentListItemEventHandler(ByVal sender As Object, ByVal e As ContentListItemEventArgs)




    '*********************************************************************
    '
    ' ContentItem Class
    '
    ' The container control we use with the templates.
    '
    '*********************************************************************

    Public Class ContentItem
        Inherits Control
        Implements INamingContainer 'ToDo: Add Implements Clauses for implementation methods of these interface(s)
        Private _dataItem As [Object]
        Private _itemIndex As Integer
        Private _itemType As ListItemType


        Public Sub New()
        End Sub 'New


        Public Sub New(ByVal itemIndex As Integer, ByVal dataItem As [Object], ByVal itemType As ListItemType)
            _dataItem = dataItem
            _itemIndex = itemIndex
            _itemType = itemType
        End Sub 'New



        Public ReadOnly Property ItemIndex() As [Object]
            Get
                Return _itemIndex
            End Get
        End Property

        Public ReadOnly Property DataItem() As [Object]
            Get
                Return _dataItem
            End Get
        End Property

        Public ReadOnly Property ItemType() As ListItemType
            Get
                Return _itemType
            End Get
        End Property
    End Class 'ContentItem
End Namespace