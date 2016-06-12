Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base

Namespace DataAccessLayer
	Public Interface IMenus
		Inherits IDBInteraction

		Property ACCT() As String
		Property SE_SEQ_ID() As Integer
		Property NAV_TYPE() As Integer

		Function GetNavigationTypes() As DataTable
		Function GetMenus() As DataTable
	End Interface
End Namespace