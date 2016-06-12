Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.ModelObjects
Imports GrowthWare.Framework.Enumerations

Namespace DataAccessLayer
	Public Interface IFunctions
		Inherits IDBInteraction

		Property Profile() As MFunctionProfile
		Property SE_SEQ_ID() As Integer
		Property Roles() As String
		Property Groups() As String
		Property Permission() As PermissionType
		Property Direction() As DirectionType

		Sub UpdateMenuOrder()

		Function GetMenuOrder(ByRef Profile As MFunctionProfile) As DataTable
		Function GetSecurity(ByVal SecurityEntityID As Integer) As DataTable
		Function GetFunctionTypes() As DataTable
		Function GetGroups(ByVal SecurityEntityID As Integer, ByVal FUNCTION_SEQ_ID As Integer, ByVal Permission As PermissionType) As DataTable
		Function GetRoles(ByVal SecurityEntityID As Integer, ByVal FUNCTION_SEQ_ID As Integer, ByVal Permission As PermissionType) As DataTable
		Function UpdateGroups() As Boolean
		Function UpdateRoles() As Boolean

		Function Add() As Integer
		Function Delete() As Boolean
		Function GetAllEnabledFunctions() As DataTable
		Function UpdateFunction() As Boolean
	End Interface
End Namespace