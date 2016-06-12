Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer
	''' <summary>
	''' IDAccount defines the contract for any
	''' class implementing the interface.
	''' </summary>
	Public Interface IDAccount
		Inherits IDDBInteraction

		''' <summary>
		''' Used by all methds and must be set to send parameters to the datastore
		''' </summary>
		Property Profile() As MAccountProfile

		''' <summary>
		''' Used by all methds and must be set to send parameters to the datastore
		''' </summary>
		Property SecurityEntitySeqID() As Integer

		''' <summary>
		''' Deletes an account
		''' </summary>
		Sub Delete()

		''' <summary>
		''' Retrieves Account information
		''' </summary>
		''' <returns>DataRow</returns>
		ReadOnly Property GetAccount() As DataRow

		''' <summary>
		''' Returns all accounts associated with a given SecurityEntitySeqID.
		''' </summary>
		''' <returns>DataTable</returns>
		''' <remarks>Does not caculate security for accounts.</remarks>
		ReadOnly Property GetAccounts() As DataTable

		''' <summary>
		''' Returns all roles associated with a given SecurityEntitySeqID.
		''' </summary>
		''' <returns>DataTable</returns>
		Function GetGroups() As DataTable

		''' <summary>
		''' Retrieves menu data for a given account
		''' </summary>
		''' <param name="account">String</param>
		''' <param name="menuType">MenuType</param>
		''' <returns>DataTable</returns>
		''' <remarks></remarks>
		Function GetMenu(ByVal account As String, ByVal menuType As MenuType) As DataTable

		''' <summary>
		''' Returns all groups associated with a given SecurityEntitySeqID.
		''' </summary>
		''' <returns>DataTable</returns>
		Function GetRoles() As DataTable

		''' <summary>
		''' Returns all roles either direct association or by association via
		''' groups.
		''' </summary>
		''' <returns>DataTable</returns>
		Function GetSecurity() As DataTable

		''' <summary>
		''' Inserts or updates account information
		''' </summary>
		''' <returns>int</returns>
		Function Save() As Integer

		''' <summary>
		''' Save groups by passing a string or comma seporated groups to the database.
		''' </summary>
		Sub SaveGroups()

		''' <summary>
		''' Save roles by passing a string or comma seporated roles to the database.
		''' </summary>
		Sub SaveRoles()

	End Interface
End Namespace