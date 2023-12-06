Goal - A component with the following features:
    1.) Bread crumb directory indicator
        a.) Display the current selected directory and it's parrents
        b.) Clickable links to help navigate
    2.) Role based right click context menu for both directories and files
    3.) A treeview of the directory structure
        (x) a.) Left click select directory
        (x) b.) Right click context menu
              i.) Create directory (role) (updates directory/file views)
                1.) Automatically select the newly created directory
                2.) Update the breadcrumb indicator
             ii.) Delete directories (role) (updates directory/file views)
                1.) Warn if directory is not empty
                2.) Automatically select the "previous" directory if deleting a directory
            iii.) Rename directory (role)  (updates directory view)
             iv.) Properties
    4.) Selecteable view of the files (like list, detailed list, snake view, etc)
        a.) Right click context menu
              i.) Rename file (role) (updates file view)
             ii.) Delete file (role) (updates file view)
            iii.) Properties
    5.) Upload Files to a selected director
    6.) Refresh the directory and files view
        a.) Will require keeping track of the directory structure (possibly related to the breadcrumb)
Nice to have:
    Possibly work with SignalR to update when directories/files change