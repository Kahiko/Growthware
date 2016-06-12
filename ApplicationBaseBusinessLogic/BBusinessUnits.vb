Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Factory
Imports ApplicationBase.Interfaces
Imports ApplicationBase.Model.BusinessUnits
Imports System.Runtime.InteropServices

Public Class BBusinessUnits
	'Private Shared iBaseDAL As IBusinessUnits = FBusinessUnits.Create(Configuration.ConfigurationManager.AppSettings("BaseDAL"))
    Private Shared iBaseDAL As IBusinessUnits = FactoryObject.Create(BaseSettings.applicationBaseDAL, "DBusinessUnits")

	Public Shared Function GetAllRolesForBusinessUnit(ByVal dstRoles As DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
		Return iBaseDAL.GetAllRolesForBusinessUnit(dstRoles, BUSINESS_UNIT_SEQ_ID)
	End Function

	Public Shared Function GetAllGroupsForBusinessUnit(ByVal dstGroups As DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
		Return iBaseDAL.GetAllGroupsForBusinessUnit(dstGroups, BUSINESS_UNIT_SEQ_ID)
	End Function

	Public Shared Function GetAllBusinessUnits(ByRef dsStates As DataSet) As DataSet
		Return iBaseDAL.GetAllBusinessUnits(dsStates)
	End Function

	Public Shared Function UpdateBusinessUnitsProfileInfo(ByVal businessUnitProfileInfo As MBusinessUnitProfileInfo, Optional ByVal Account_seq_Id As Integer = 1) As Boolean
		Return iBaseDAL.UpdateBusinessUnitProfileInfo(businessUnitProfileInfo, Account_seq_Id)
	End Function

	Public Shared Function AddBusinessUnitsProfileInfo(ByRef businessUnitProfileInfo As MBusinessUnitProfileInfo) As Boolean
		Return iBaseDAL.AddBusinessUnitProfileInfo(businessUnitProfileInfo)
	End Function

	Public Shared Function GetAllBusinessUnits() As MBusinessUnitProfileInfoCollection
		Return iBaseDAL.GetAllBusinessUnits
	End Function

	Public Shared Sub GetValidBusinessUnits(ByRef theirDataSet As DataSet, ByVal Account_Seq_Id As Integer, ByVal isSysAdmin As Integer)
		iBaseDAL.GetValidBusinessUnits(theirDataSet, Account_Seq_Id, isSysAdmin)
	End Sub
End Class