Imports System.Web
'Imports System.Xml
'Imports System.Xml.XmlDocument
Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.Cache
Imports ApplicationBase.Common.Globals
Imports System.Data

#Region " Notes "
' The NavMenuUtil aids in getting data for the Hierarchical menu
#End Region
Public Class NavMenuUtility
	'*********************************************************************
	' GetLinks function
	' Represents all left hand menu items from either
	' cache or db.
	' note that GetLinks can be forced to update the cache
	'*********************************************************************
	Public Shared Function GetLinks(ByVal force As Boolean, ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
		' Put the menu items into a session variable
		Dim MenuDataSet As DataSet = Nothing
		If HttpContext.Current.Session Is Nothing Then
			Return BNavMenu.GetLinks(MenuDataSet, ACCOUNT_SEQ_ID, BUSINESS_UNIT_SEQ_ID)
		End If
		MenuDataSet = CType(HttpContext.Current.Session("NavMenu"), DataSet)
		If MenuDataSet Is Nothing OrElse force Then
			MenuDataSet = BNavMenu.GetLinks(MenuDataSet, ACCOUNT_SEQ_ID, BUSINESS_UNIT_SEQ_ID)
			HttpContext.Current.Session("NavMenu") = MenuDataSet
		End If
		Return MenuDataSet
	End Function

	'*********************************************************************
	' GetLineMenuLinks function
	' Represents all line menu items from either
	' cache or db.
	' note that GetLinks can be forced to update the cache
	'*********************************************************************
	Public Shared Function GetLineMenuLinks(ByVal force As Boolean, ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
		' Put the menu items into a session variable
		Dim MenuDataSet As DataSet = Nothing
		If HttpContext.Current.Session Is Nothing Then
			Return BNavMenu.GetLineMenuLinks(MenuDataSet, ACCOUNT_SEQ_ID, BUSINESS_UNIT_SEQ_ID)
		End If
		MenuDataSet = CType(HttpContext.Current.Session("LineMenu"), DataSet)
		If MenuDataSet Is Nothing OrElse force Then
			MenuDataSet = BNavMenu.GetLineMenuLinks(MenuDataSet, ACCOUNT_SEQ_ID, BUSINESS_UNIT_SEQ_ID)
			HttpContext.Current.Session("LineMenu") = MenuDataSet
		End If
		Return MenuDataSet
	End Function


	'*********************************************************************
	'
	' removeLinks function removes menu links from session
	'
	'*********************************************************************
	Public Shared Sub removeLinks()
		' Put the menu items into a session variable
		HttpContext.Current.Session.Item("NavMenu") = Nothing
	End Sub

	Public Shared Function NumberOfMenuItems(ByVal force As Boolean, ByVal ACCOUNT_SEQ_ID As Integer, ByVal State As String) As Integer
		Dim retVal As Integer = 0
		Dim MenuDataSet As DataSet = Nothing
		If HttpContext.Current.Session Is Nothing OrElse force Then
			MenuDataSet = BNavMenu.GetLinks(MenuDataSet, ACCOUNT_SEQ_ID, State)
		Else
			MenuDataSet = CType(HttpContext.Current.Session("NavMenu"), DataSet)
		End If
		If MenuDataSet Is Nothing Then
			MenuDataSet = BNavMenu.GetLinks(MenuDataSet, ACCOUNT_SEQ_ID, State)
			HttpContext.Current.Session("NavMenu") = MenuDataSet
		End If
		retVal = MenuDataSet.Tables(0).Rows.Count
		Return retVal
	End Function

	Public Shared Sub GetNavType(ByRef dsNavType As DataSet)
		dsNavType = CType(HttpContext.Current.Cache.Item("dsNavType"), DataSet)
		If dsNavType Is Nothing Then
			dsNavType = BNavMenu.GetNavType(dsNavType)
			CacheControler.AddToCacheDependency("dsNavType", dsNavType)
		End If
	End Sub	 'GetNavType

	Public Shared Sub GetRootLinks(ByRef YourDataSet As DataSet, ByVal ACCOUNT_SEQ_ID As Integer, ByVal State As String)
		BNavMenu.GetRootLinks(YourDataSet, ACCOUNT_SEQ_ID, State)
	End Sub

	Public Shared Function BuildHierarchicalMenu(ByVal aMenus As Array, ByVal iCurID As Integer, ByVal iDepth As Integer, ByRef Menu As Menu, Optional ByVal myItem As MenuItem = Nothing, Optional ByVal BackColor As String = "#ffffff") As Menu
		Dim MENUID As Integer = 0
		Dim ParentIDPosition As Integer = 4
		Dim Name As Integer = 1
		Dim Link As Integer = 3
		Dim iNumRecords As Integer
		Dim iLoop As Integer
		Dim i As Integer
		iNumRecords = UBound(aMenus, 2)
		For iLoop = 0 To iNumRecords
			If CInt(aMenus(ParentIDPosition, iLoop)) = iCurID Then
				If CInt(aMenus(ParentIDPosition, iLoop)) = 0 Then				' Root Menu Item
					Dim myRootItem As MenuItem = New MenuItem(aMenus(Name, iLoop))
					myRootItem.Selectable = False
					Menu.Items.Add(myRootItem)
					BuildHierarchicalMenu(aMenus, aMenus(MENUID, iLoop), iDepth + 1, Menu, myRootItem, BackColor)
				Else				' sub menu item
					Dim subMenuItem As New MenuItem
					For i = 1 To iDepth
                        subMenuItem = New MenuItem(aMenus(Name, iLoop), "", "", BaseSettings.FQDNPage & "?Action=" & Trim(aMenus(Link, iLoop)))
						If myItem.Depth = 0 Then
							myItem.Selectable = False
						End If
						BuildHierarchicalMenu(aMenus, aMenus(MENUID, iLoop), iDepth + 1, Menu, subMenuItem, BackColor)
					Next
					myItem.ChildItems.Add(subMenuItem)
				End If
			End If
		Next
		Return Menu
	End Function
End Class