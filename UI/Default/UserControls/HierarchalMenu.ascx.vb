Imports BLL.Base.SQLServer
Imports BLL.Base.ClientChoices
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices
Imports Common.CustomWebControls.Menu
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Xml

Public Class HierarchalMenu
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object
	Protected WithEvents Menu1 As Menu
	Protected WithEvents MenuDetail As System.Web.UI.HtmlControls.HtmlTableCell
	Public MenuType As String
	Public ClickToOpen As Boolean
	Public MenuStyle As String

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region "Notes"
	' There are two attributes for the HierarchalMenu
	' and they are:
	' MenuType="Vertical" ClickToOpen="False"
	' Valid MenuType's are horizontal or vertical and
	'	set the orientation of the menu
	' Valid ClickToOpen's are True or False
	'	Set this to true if you would like
	'	the customer to click on the menu item to open it.
	'		Please note if you plan on having multi-level
	'		items this is not a good options.  The system
	'		will take the menu click and try to do something
	'		with it and most likely cause an error.
	'		So when needing to have multi-level menu item's
	'		it is recommended to use false.
	'	Set this to false if you would like a "hover" over event
	'	to open the menu and display other memnu items.
#End Region

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If context.Session Is Nothing Then Exit Sub
		If context.User.Identity.IsAuthenticated Then
			Try
				Dim NumberOfColumns As Integer = 4
				' get information about the client
				Dim accountProfileInfo As MAccountProfileInfo
				Dim myAccountUtility As New AccountUtility(HttpContext.Current)
				accountProfileInfo = myAccountUtility.GetAccountProfileInfo(context.User.Identity.Name)
                Dim menuDataSet As DataSet = HttpContext.Current.Session("MenuDataSet")
                If menuDataSet Is Nothing Then
                    menuDataSet = BNavMenu.GetHierarchicalMenuData(menuDataSet, ClientChoicesState(MClientChoices.BusinessUnitID), accountProfileInfo.ACCOUNT_SEQ_ID)
                    'xdd = New XmlDataDocument(menuDataSet)
                    Dim rel As New DataRelation( _
                      "ParentChild", _
                      menuDataSet.Tables(0).Columns(0), _
                      menuDataSet.Tables(0).Columns(NumberOfColumns), _
                      False _
                     )
                    rel.Nested = True
                    menuDataSet.Relations.Add(rel)
                    HttpContext.Current.Session("MenuDataSet") = menuDataSet
                End If
                ' create an array of the menu itmems
                Dim myArray(NumberOfColumns, menuDataSet.Tables(0).Rows.Count - 1) As String
                ' populate the array with the dataset information
                Dim i As Integer
                For i = 0 To menuDataSet.Tables(0).Rows.Count - 1
                    myArray(0, i) = menuDataSet.Tables(0).Rows(i)(0)
                    myArray(1, i) = menuDataSet.Tables(0).Rows(i)(1)
                    myArray(2, i) = menuDataSet.Tables(0).Rows(i)(2)
                    myArray(3, i) = menuDataSet.Tables(0).Rows(i)(3)
                    myArray(4, i) = menuDataSet.Tables(0).Rows(i)(4)
                Next i
                Dim myMenu As New Menu       ' get a new menu
                If MenuType.ToLower = "horizontal" Then
                    myMenu.Layout = MenuLayout.Horizontal
                    myMenu.Style.Item("menustyle") = "border-bottom: Gray 1px solid; border-left: White 1px solid; border-top: White 1px solid; border-right: Gray 1px solid;background-image:url(" & BaseHelper.ImagePath & "SOLPART/MenuBar-Silver.gif); background-color: Silver"
                    myMenu.Style.Item("menuitem") = "color: Black; font-family: Arial; font-size: 10pt; font-weight: bold; font-style: italic; border-left: DarkGray 0px solid; border-bottom: Silver 1px solid; border-top: Silver 1px solid; border-right: Silver 0px solid;"
                    myMenu.Width = System.Web.UI.WebControls.Unit.Percentage(100)
                Else
                    myMenu.Layout = MenuLayout.Vertical
                End If
                'sessionMenu.Cursor = MouseCursor.Pointer
                myMenu.ClickToOpen = ClickToOpen
                myMenu = NavMenuUtility.BuildHierarchicalMenu(myArray, 0, 0, myMenu, , ClientChoicesState("HeadColor"))     ' populate the menu
                myMenu.EnableViewState = False
                HttpContext.Current.Session("MenuArray") = myArray
                'context.Session("HMenu") = sessionMenu					  ' put the menu into the session
                myMenu.ScriptPath = BaseHelper.ScriptPath & "JS/Common/skmMenu.js"
                myMenu.DefaultCssClass = "HierarchalMenuItem"
                myMenu.SubMenuCssClass = "HierarchalMenuSubMenu"
                myMenu.DefaultMouseDownCssClass = "HierarchalMenuMousedown"
                myMenu.DefaultMouseOverCssClass = "HierarchalMenuMouseover"
                myMenu.DefaultMouseUpCssClass = "HierarchalMenuMouseup"
                MenuDetail.Controls.Add(myMenu)
            Catch ex As Exception
				Throw ex
			End Try
		End If
	End Sub	'Page_Load
End Class