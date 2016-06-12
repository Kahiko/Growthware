Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Model.Accounts
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Xml
Imports ApplicationBase.Model.Special.ClientChoices

Partial Class HierarchalMenu
	Inherits ClientChoices.ClientChoicesUserControl

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

	Public MenuType As String = String.Empty
	Public ClickToOpen As Boolean = False
	Public MenuStyle As String = String.Empty

	Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Context.Session Is Nothing Then Exit Sub
		Try
			Dim NumberOfColumns As Integer = 4
			' get information about the client
			Dim accountProfileInfo As MAccountProfileInfo
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			accountProfileInfo = myAccountUtility.GetAccountProfileInfo(Context.User.Identity.Name)
			If accountProfileInfo Is Nothing Then accountProfileInfo = myAccountUtility.GetAccountProfileInfo("Anonymous")
			Dim menuDataSet As DataSet = HttpContext.Current.Session("MenuDataSet")
			If menuDataSet Is Nothing Then
				menuDataSet = BNavMenu.GetHierarchicalMenuData(menuDataSet, ClientChoicesState(MClientChoices.BusinessUnitID), accountProfileInfo.ACCOUNT_SEQ_ID)
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
            'Dim myMenu As New Menu			 ' get a new menu
            myMenu.Orientation = Orientation.Vertical
			If MenuType.ToLower = "horizontal" Then
                myMenu.Orientation = Orientation.Horizontal
                myMenu.Width = System.Web.UI.WebControls.Unit.Percentage(100)
            End If
            myMenu = NavMenuUtility.BuildHierarchicalMenu(myArray, 0, 0, myMenu, , ClientChoicesState("HeadColor"))           ' populate the menu
            myMenu.EnableViewState = True
			HttpContext.Current.Session("MenuArray") = myArray
            MenuDetail.Controls.Add(myMenu)
        Catch ex As Exception
            Throw ex
		End Try
	End Sub	'Page_Load
End Class