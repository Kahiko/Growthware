Imports System.Web
Imports System.ComponentModel
Imports System.Web.UI
Imports Common.CustomWebControls.Menu

'/ <summary>
   '/ MenuItemCollection represents a collection of <see cref="MenuItem"/> instances.
   '/ </summary>
   '/ <remarks>Each item in a menu is represented by an instance of the <see cref="MenuItem"/> class.
   '/ The MenuItem class has a <see cref="SubItems"/> property, which is of type MenuItemCollection.  This
   '/ MenuItemCollection, then, allows for each MenuItem to have a submenu of MenuItems.<p />This flexible
   '/ object model allows for an unlimited number of submenu depths.</remarks>
<Serializable()> _
   Public Class MenuItemCollection
    Implements IStateManager
#Region "Private Member Variables"
    ' private member variables
    Private menuItems As New ArrayList
    Private _isTrackingViewState As Boolean = False
#End Region

#Region "ICollection Implementation"

    '/ <summary>
    '/ Adds a MenuItem to the collection.  If the ViewState is being tracked, the
    '/ MenuItem's TrackViewState() method is called and the item is set to dirty, so
    '/ that we don't lose any settings made prior to the Add() call.
    '/ </summary>
    '/ <param name="item">The MenuItem to add to the collection</param>
    '/ <returns>The ordinal position of the added item.</returns>
    Public Overridable Function Add(ByVal item As MenuItem) As Integer
        Dim result As Integer = menuItems.Add(item)

        Return result
    End Function 'Add



    '/ <summary>
    '/ Adds a spacer to the collection.  
    '/ </summary>
    '/ <param name="itemHeight">The height of the spacer to add to the collection</param>
    '/ <returns>The ordinal position of the added item.</returns>
    Public Overridable Overloads Function AddSpacer(ByVal itemHeight As Integer) As Integer
        ' A spacer is really just a menuitem which has blank text and image properties.
        Dim result As Integer = menuItems.Add(New MenuItem(" "))

        Dim added As MenuItem = CType(menuItems(result), MenuItem)

        added.Height = UI.WebControls.Unit.Pixel(itemHeight)
        added.Text = ""
        added.MenuType = MenuItemType.MenuSeparator

        Return result
    End Function    'AddSpacer



    '/ <summary>
    '/ Adds a spacer to the collection.  
    '/ </summary>
    '/ <param name="itemHeight">The height of the spacer to add to the collection</param>
    '/ <param name="itemCssClass">The CssClass for the spacer to add to the collection</param>
    '/ <returns>The ordinal position of the added item.</returns>
    Public Overridable Overloads Function AddSpacer(ByVal itemHeight As Integer, ByVal itemCssClass As String) As Integer
        ' A spacer is really just a menuitem which has (usually) blank text and image properties.
        Dim result As Integer = menuItems.Add(New MenuItem(" "))

        Dim added As MenuItem = CType(menuItems(result), MenuItem)
        added.Height = UI.WebControls.Unit.Pixel(itemHeight)
        added.CssClass = itemCssClass
        added.Text = ""
        added.MenuType = MenuItemType.MenuSeparator

        Return result
    End Function    'AddSpacer



    '/ <summary>
    '/ Adds a spacer to the collection.
    '/ </summary>
    '/ <param name="itemHeight">The height of the spacer to add to the collection</param>
    '/ <param name="itemCssClass">The CssClass for the spacer to add to the collection</param>
    '/ /// <param name="itemText">The Text for the spacer to add to the collection</param>
    '/ <returns>The ordinal position of the added item.</returns>
    Public Overridable Overloads Function AddSpacer(ByVal itemHeight As Integer, ByVal itemCssClass As String, ByVal itemText As String) As Integer
        ' A spacer is really just a menuitem which has blank text and image properties.
        Dim result As Integer = menuItems.Add(New MenuItem(itemText))

        Dim added As MenuItem = CType(menuItems(result), MenuItem)
        added.Height = UI.WebControls.Unit.Pixel(itemHeight)
        added.CssClass = itemCssClass
        added.MenuType = MenuItemType.MenuSeparator

        Return result
    End Function    'AddSpacer



    '/ <summary>
    '/ Adds a header to the collection.  
    '/ </summary>
    '/ <param name="itemText">The text for the header to add to the collection</param>
    '/ <returns>The ordinal position of the added item.</returns>
    Public Overridable Function AddHeader(ByVal itemText As String) As Integer
        ' A header is really just a menuitem which has only a text property.
        Dim result As Integer = menuItems.Add(New MenuItem(itemText))

        Dim added As MenuItem = CType(menuItems(result), MenuItem)

        added.MenuType = MenuItemType.MenuHeader

        Return result
    End Function    'AddHeader



    '/ <summary>
    '/ Adds the MenuItems in a MenuItemCollection.
    '/ </summary>
    '/ <param name="items">The MenuItemCollection instance whose MenuItems to add.</param>
    Public Overridable Sub AddRange(ByVal items As MenuItemCollection)
        menuItems.AddRange(items)
    End Sub    'AddRange


    '/ <summary>
    '/ Clears out the entire MenuItemCollection.
    '/ </summary>
    Public Overridable Sub Clear()
        menuItems.Clear()
    End Sub    'Clear


    '/ <summary>
    '/ Determines if a particular MenuItem exists within the collection.
    '/ </summary>
    '/ <param name="item">The MenuItem instance to check for.</param>
    '/ <returns>A Boolean - true if the MenuItem is in the collection, false otherwise.</returns>
    Public Overridable Function Contains(ByVal item As MenuItem) As Boolean
        Return menuItems.Contains(item)
    End Function    'Contains


    '/ <summary>
    '/ Returns the ordinal index of a MenuItem, if it exists; if the item does not exist,
    '/ -1 is returned.
    '/ </summary>
    '/ <param name="item">The MenuItem to search for.</param>
    '/ <returns>The ordinal position of the item in the collection.</returns>
    Public Overridable Function IndexOf(ByVal item As MenuItem) As Integer
        Return menuItems.IndexOf(item)
    End Function    'IndexOf


    '/ <summary>
    '/ Inserts a MenuItem instance at a particular location in the collection.
    '/ </summary>
    '/ <param name="index">The ordinal location to insert the item.</param>
    '/ <param name="item">The MenuItem to insert.</param>
    Public Overridable Sub Insert(ByVal index As Integer, ByVal item As MenuItem)
        menuItems.Insert(index, item)
    End Sub    'Insert


    '/ <summary>
    '/ Removes a specified MenuItem from the collection.
    '/ </summary>
    '/ <param name="item">The MenuItem instance to remove.</param>
    Public Sub Remove(ByVal item As MenuItem)
        menuItems.Remove(item)
    End Sub    'Remove


    '/ <summary>
    '/ Removes a MenuItem from a particular ordinal position in the collection.
    '/ </summary>
    '/ <param name="index">The ordinal position of the MenuItem to remove.</param>
    Public Sub RemoveAt(ByVal index As Integer)
        menuItems.RemoveAt(index)
    End Sub    'RemoveAt


    '/ <summary>
    '/ Copies the contents of the MenuItem to an array.
    '/ </summary>
    '/ <param name="array"></param>
    '/ <param name="index"></param>
    Public Sub CopyTo(ByVal array As Array, ByVal index As Integer)
        menuItems.CopyTo(array, index)
    End Sub    'CopyTo


    '/ <summary>
    '/ Gets an Enumerator for enumerating through the collection.
    '/ </summary>
    '/ <returns></returns>
    Public Function GetEnumerator() As IEnumerator
        Return menuItems.GetEnumerator()
    End Function    'GetEnumerator
#End Region

#Region "IStateManager Interface"

    '/ <summary>
    '/ Indicates that changes to the view state should be tracked.  Calls TrackViewState()
    '/ for each MenuItem in the collection.
    '/ </summary>
    Sub TrackViewState() Implements IStateManager.TrackViewState
        Me._isTrackingViewState = True
        Dim item As MenuItem
        For Each item In Me.menuItems
            CType(item, IStateManager).TrackViewState()
        Next item
    End Sub    'IStateManager.TrackViewState

    '/ <summary>
    '/ Saves the view state in an object array.  Each item in the collection has its
    '/ SaveViewState() method called.  This array is then returned, representing the
    '/ state of the MenuItemCollection.
    '/ </summary>
    '/ <returns>An object array.</returns>
    Function SaveViewState() As Object Implements IStateManager.SaveViewState
        Dim isAllNulls As Boolean = True
        Dim state(Me.menuItems.Count) As Object
        Dim i As Integer
        For i = 0 To (Me.menuItems.Count) - 1
            ' Save each item's viewstate...
            state(i) = CType(Me.menuItems(i), IStateManager).SaveViewState()
            If Not (state(i) Is Nothing) Then
                isAllNulls = False
            End If
        Next i
        ' If all items returned null, simply return a null rather than the object array
        If isAllNulls Then
            Return Nothing
        Else
            Return state
        End If
    End Function    'IStateManager.SaveViewState

    '/ <summary>
    '/ Iterate through the object array passed in.  For each element in the object array
    '/ passed-in, a new MenuItem instance is created, added to the collection, and populated
    '/ by calling LoadViewState().
    '/ </summary>
    '/ <param name="savedState">The object array returned by the SaveViewState() method in
    '/ the previous page visit.</param>
    Sub LoadViewState(ByVal savedState As Object) Implements IStateManager.LoadViewState
        If Not (savedState Is Nothing) Then
            Dim state As Object() = CType(savedState, Object())

            ' Create an ArrayList of the precise size
            menuItems = New ArrayList(state.Length)

            Dim i As Integer
            For i = 0 To state.Length - 1
                Dim mi As New MenuItem      ' create MenuItem
                CType(mi, IStateManager).TrackViewState()       ' Indicate that it needs to track its view state
                ' Add the MenuItem to the collection
                menuItems.Add(mi)

                If Not (state(i) Is Nothing) Then
                    ' Load its state via LoadViewState()
                    CType(menuItems(i), IStateManager).LoadViewState(state(i))
                End If
            Next i
        End If
    End Sub    'IStateManager.LoadViewState
#End Region

#Region "MenuItemCollection Properties"
    '/ <summary>
    '/ Returns the number of elements in the MenuItemCollection.
    '/ </summary>
    '/ <value>The actual number of elements contained in the <see cref="MenuItemCollection"/>.</value>

    <Browsable(False)> _
    Public Overridable ReadOnly Property Count() As Integer
        Get
            Return menuItems.Count
        End Get
    End Property


    '/ <summary>
    '/ Gets a value indicating whether access to the <see cref="MenuItemCollection"/> is synchronized (thread-safe).
    '/ </summary>

    <Browsable(False)> _
    Public Overridable ReadOnly Property IsSynchronized() As Boolean
        Get
            Return menuItems.IsSynchronized
        End Get
    End Property


    '/ <summary>
    '/ Gets an object that can be used to synchrnoize access to the <see cref="MenuItemCollection"/>.
    '/ </summary>

    <Browsable(False)> _
    Public Overridable ReadOnly Property SyncRoot() As Object
        Get
            Return menuItems.SyncRoot
        End Get
    End Property


    '/ <summary>
    '/ Gets the <see cref="MenuItem"/> at a specified ordinal index.
    '/ </summary>
    '/ <remarks>Allows read-only access to the <see cref="MenuItemCollection"/>'s elements by index.
    '/ For example, myMenuCollection[4] would return the fifth <see cref="MenuItem"/> instance.</remarks>

    Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As MenuItem
        Get
            Return CType(menuItems(index), MenuItem)
        End Get
    End Property


    '/ <summary>
    '/ Gets the <see cref="MenuItem"/> with a specified <see cref="Name"/>.
    '/ </summary>
    '/ <remarks>The <see cref="MenuItem"/> class has a <see cref="Name"/> property that allows for items
    '/ to be indexed by name.<p />For example, myMenuCollection["Contact"] would return the 
    '/ <see cref="MenuItem"/> instance with the <see cref="Name"/> "Contact", or <b>null</b> if no such
    '/ MenuItem existed in the MenuItemCollection.</remarks>

    Default Public Overridable ReadOnly Property Item(ByVal name As String) As MenuItem
        Get
            Dim myItem As MenuItem
            For Each myItem In menuItems
                If myItem.Name = name Then
                    Return myItem
                End If
            Next myItem
            Return Nothing
        End Get
    End Property


    '/ <summary>
    '/ A required property since MenuItemCollection implements IStateManager.  This
    '/ property simply indicates if the MenuItemCollection is tracking its view state or not.
    '/ </summary>

    ReadOnly Property IsTrackingViewState() As Boolean Implements IStateManager.IsTrackingViewState
        Get
            Return Me._isTrackingViewState
        End Get
    End Property
#End Region
End Class 'MenuItemCollection