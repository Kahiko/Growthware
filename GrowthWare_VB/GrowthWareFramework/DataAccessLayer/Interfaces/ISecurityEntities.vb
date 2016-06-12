Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.ModelObjects

Namespace DataAccessLayer
	Public Interface ISecurityEntities
		Inherits IDBInteraction

		Property Profile() As MSecurityEntityProfile

		Function GetValidSecurityEntities(ByVal account As String, ByVal SecurityEntityID As Integer, ByVal isSecurityEntityAdministrator As Boolean) As DataTable
		Function GetSecurityEntity() As DataRow
		Function GetAllSecurityEntities() As DataTable
		Function SaveSecurityEntity() As Boolean
	End Interface
End Namespace