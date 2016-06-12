Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.ModelObjects

Namespace DataAccessLayer
	Public Interface IRoles
		Inherits IDBInteraction

		Property SE_SEQ_ID() As Integer
		Property Profile() As MRoleProfile
		Function GetAccountsInRole() As DataTable
		Function GetAccountsNotInRole() As DataTable
		Function UpdateAllAccountsForRole(ByVal RoleSeqID As Integer, ByVal SecurityEntityID As Integer, ByVal Accounts As String(), ByVal AccountSeqID As Integer) As Boolean

		Function AddRole() As Boolean
		Function DeleteRole() As Boolean
		Function GetProfileData() As DataRow
		Function GetRolesByBU() As DataTable
		Function UpdateRole() As Boolean
	End Interface
End Namespace