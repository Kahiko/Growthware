Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer
	Public Interface IDSecurityEntity
		Inherits IDDBInteraction

		''' <summary>
		''' Retrieves all Security Entities as a data table.
		''' </summary>
		''' <returns>DataTable</returns>
		''' <remarks></remarks>
		Function GetSecurityEntities() As DataTable

		''' <summary>
		''' Retrieves security entities for a given account.
		''' </summary>
		''' <param name="account">String</param>
		''' <param name="securityEntityID">int or Integer</param>
		''' <param name="isSecurityEntityAdministrator">Boolean or bool</param>
		''' <returns>Datatable</returns>
		''' <remarks></remarks>
		Function GetSecurityEntities(ByVal account As String, ByVal securityEntityID As Integer, ByVal isSecurityEntityAdministrator As Boolean) As DataTable

		''' <summary>
		''' Saves security entity information to the datastore.
		''' </summary>
		''' <param name="profile">MSecurityEntityProfile</param>
		''' <remarks></remarks>
		Function Save(ByRef profile As MSecurityEntityProfile) As Integer
	End Interface
End Namespace
