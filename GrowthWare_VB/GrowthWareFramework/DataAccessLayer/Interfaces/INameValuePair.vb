Imports GrowthWare.Framework.ModelObjects
Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base

Namespace DataAccessLayer
	Public Interface INameValuePair
		Inherits IDBInteraction

		Property NameValuePairProfile() As MNameValuePair
		Property SE_SEQ_ID() As Integer
		Property AccountID() As Integer
		Property DetailProfile() As MNameValuePairDetail

		Function DeleteNVPDetail(ByRef Profile As MNameValuePairDetail) As Boolean
		Function GetNVPDetail() As DataRow
		Function GetNVPDetails(ByRef NVPSeqDetID As Integer, ByRef NVPSeqID As Integer) As DataRow
		Function GetAllNVPDetail() As DataTable
		Function GetAllNVPDetail(ByRef NVPSeqID As Integer) As DataTable
		Function GetGroups(ByRef NameValuePairSeqID As Integer) As DataTable
		Function GetRoles(ByRef NameValuePairSeqID As Integer) As DataTable
		Function SaveNVPDetail(ByRef Profile As MNameValuePairDetail) As Integer

		Sub UpdateGroups(ByRef NVP_ID As Integer, ByRef SecurityEntityID As Integer, ByRef CommaSeperatedGroups As String, ByRef AddUpd_By As Integer)
		Sub UpdateRoles(ByRef NVP_ID As Integer, ByRef SecurityEntityID As Integer, ByRef CommaSeperatedRoles As String, ByRef AddUpd_By As Integer)

		Function GetAllNVP() As DataTable
		Function GetNVP() As DataRow
		Function Save() As Boolean
	End Interface
End Namespace