Namespace CustomWebControls.Menu
    '/ <summary>
    '/ Specifies the type of <see cref="MenuItem"/>.
    '/ </summary> 
    '/ </remarks>

    Public Enum MenuItemType
        '/ <summary>
        '/ A standard menu item.
        '/ </summary>
        MenuItem

        '/ <summary>
        '/ A separator menu item.  Separators provide a level of separation between menu items.
        '/ </summary>
        MenuSeparator

        '/ <summary>
        '/ A menu item header.
        '/ </summary>
        MenuHeader
    End Enum 'MenuItemType 

    '/ <summary>
    '/ Specifies the visual orientation for the <see cref="Menu"/>'s top-level menu items.
    '/ </summary> 
    '/ <remarks>The main menu in Windows applications use the <b>Horizontal</b> approach: for example, the File menu is
    '/ laid next to the Edit menu which is laid next to the View menu, and so on.  Many Web pages, however, use
    '/ a <b>Vertical</b> layout, displaying the top-level menu items along the left hand side of the Web page.
    '/ </remarks>

    Public Enum MenuLayout
        '/ <summary>
        '/ Each top-level menu item is laid out end-to-end.
        '/ </summary>
        Horizontal

        '/ <summary>
        '/ Each top-level menu item is laid one above the other.
        '/ </summary>
        Vertical
    End Enum 'MenuLayout 

    '/ <summary>
    '/ The <b>MouseCursor</b> enumeration specifies the different cursors for the menu.
    '/ </summary>
    '/ <remarks>The <b>Pointer</b> option will only display a pointer cursor for clickable menu items.</remarks>

    Public Enum MouseCursor
        '/ <summary>
        '/ Default pointer or pointer controlled by CSS.
        '/ </summary>
        [Default]
        '/ <summary>
        '/ Pointer normally associated with links (usually a hand).
        '/ </summary>
        Pointer
    End Enum 'MouseCursor 

    '/ <summary>
    '/ The <b>MenuPositioning</b> enumeration specifies whether absolute or relative positioning will be used.
    '/ </summary> 

    Public Enum MenuPositioning
        '/ <summary>
        '/ Absolute positioning.
        '/ </summary>
        Absolute
        '/ <summary>
        '/ Relative positioning.
        '/ </summary>
        Relative
    End Enum 'MenuPositioning
End Namespace