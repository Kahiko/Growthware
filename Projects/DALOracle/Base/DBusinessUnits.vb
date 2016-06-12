Imports Common
Imports Common.Oracle
Imports DALInterface.Base.Interfaces
Imports DALModel.Base
Imports DALModel.Base.Modules
Imports DALModel.Base.BusinessUnits
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Configuration
Imports Common.Security.BaseSecurity

Public Class DBusinessUnits
	Implements IBusinessUnits

#Region " Private Objects "
	Private _ConnectionString As String = String.Empty
#End Region

#Region " Private Methods "
	'*********************************************************************
	'
	' CalculateDirectories Method
	'
	' Calculate directory information such as directy paths and 
	' inherited properties by iterating through each 
	' row in a DataSet containing directory rows.
	'
	'*********************************************************************
	Private Shared Function CalculateBusinessUnits(ByVal dstBusinessUnits As DataSet) As MBusinessUnitProfileInfoCollection
		Dim businessUnitInfoCollection As New MBusinessUnitProfileInfoCollection
		Dim dtblBusinessUnits As DataTable = dstBusinessUnits.Tables(0)

		' Calculated inherited properties for each Business unit
		Dim drowBusinessUnit As DataRow
		For Each drowBusinessUnit In dtblBusinessUnits.Rows
			If Not drowBusinessUnit("BUSINESS_UNIT_SEQ_ID") Is Nothing Then
				' Add the directoryInfo to the collection
				businessUnitInfoCollection.Add(Trim(CStr(drowBusinessUnit("BUSINESS_UNIT_SEQ_ID"))), New MBusinessUnitProfileInfo(drowBusinessUnit))
			End If
		Next drowBusinessUnit

		Return businessUnitInfoCollection
	End Function	'CalculateDirectories

	'*********************************************************************
	'
	' ConnectionString Method
	' Gets the connection string from the web.config file.
	'
	'*********************************************************************
	Private Function ConnectionString() As String
		If _ConnectionString = String.Empty Then
			' try to decrypt the connection string
			_ConnectionString = ConnectionHelper.GetConnectionString("OracleDAL")
		End If
		Return _ConnectionString
	End Function

	Private Function PopulateProfileFromOracleDataReader(ByVal dr As OracleDataReader) As MBusinessUnitProfileInfo
		Dim profile As New MBusinessUnitProfileInfo
		profile.BUSINESS_UNIT_SEQ_ID = CStr(dr("BUSINESS_UNIT_SEQ_ID"))
		profile.Name = CStr(dr("Name"))
		profile.Description = CStr(dr("Description"))
		profile.Skin = CStr(dr("Skin"))
		profile.Parent_Business_Unit_Seq_ID = CStr(dr("Parent_Business_Unit_Seq_ID"))
		profile.STATUS_SEQ_ID = CInt(dr("STATUS_SEQ_ID"))
		profile.ConnctionString = CStr(dr("CONNECTION_STRING"))
		Return profile
	End Function	'PopulateProfileFromOracleDataReader
#End Region

	Public Function GetAdminBusinessUnitArray(ByVal ACCOUNT_SEQ_ID As Integer) As String Implements IBusinessUnits.GetAdminBusinessUnitArray

	End Function

	Public Function GetAllBusinessUnits() As MBusinessUnitProfileInfoCollection Implements IBusinessUnits.GetAllBusinessUnits
		Dim myDataSet As New DataSet
		Dim myBusinessUnitProfileInfoCollection As MBusinessUnitProfileInfoCollection
		Try
            myDataSet = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_GET_ALL_BU_ALL_INFO")
			myBusinessUnitProfileInfoCollection = CalculateBusinessUnits(myDataSet)
		Catch ex As Exception
			Throw ex
		Finally
			If Not myDataSet Is Nothing Then
				myDataSet = Nothing
			End If
		End Try
		Return myBusinessUnitProfileInfoCollection
	End Function

	Public Function GetAllRolesForBusinessUnit(ByVal dstRoles As System.Data.DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As System.Data.DataSet Implements IBusinessUnits.GetAllRolesForBusinessUnit
		Try
            dstRoles = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_GET_ALL_RLS_FOR_BU", New OracleParameter() { _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
            })
		Catch ex As Exception
			Throw ex
		End Try
		Return dstRoles
	End Function

	Public Function GetBusinessUnitArray(ByVal ACCOUNT_SEQ_ID As Integer) As String Implements IBusinessUnits.GetBusinessUnitArray

	End Function

	'*********************************************************************
	'
	' GetConnectionString Method
	' Gets the connection string from the database for a given Business Unit.
	'
	'*********************************************************************
	Public Function GetConnectionString(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String Implements IBusinessUnits.GetConnectionString
		Dim retString As String = String.Empty
		retString = OracleHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_Connection_String", New OracleParameter() {New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID)})
		Try
			Dim myCryptoUtil As New CryptoUtil
			retString = myCryptoUtil.Decrypt(retString)
		Catch ex As Exception
			' do nothing the most likely cause for failure
			' is the value is clear text
		End Try
		Return retString
	End Function

    Public Function UpdateBusinessUnitProfileInfo(ByRef businessUnitProfileInfo As MBusinessUnitProfileInfo, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IBusinessUnits.UpdateBusinessUnitProfileInfo
        Dim retVal As Boolean = False
        Try
			OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_UPDATE_BU_PROFILE", _
			New OracleParameter() { _
			New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", businessUnitProfileInfo.BUSINESS_UNIT_SEQ_ID), _
			New OracleParameter("@P_NAME", businessUnitProfileInfo.Name), _
			New OracleParameter("@P_DESCRIPTION", businessUnitProfileInfo.Description), _
			New OracleParameter("@P_SKIN", businessUnitProfileInfo.Skin), _
			New OracleParameter("@P_PARENT_BUSINESS_UNIT_SEQ_ID", businessUnitProfileInfo.Parent_Business_Unit_Seq_ID), _
			New OracleParameter("@P_STATUS_SEQ_ID", businessUnitProfileInfo.STATUS_SEQ_ID), _
			New OracleParameter("@P_CONNECTION_STRING", businessUnitProfileInfo.ConnctionString), _
			New OracleParameter("@P_DAL", businessUnitProfileInfo.DAL), _
			New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
			})
            retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function

    Public Function AddBusinessUnitProfileInfo(ByRef businessUnitProfileInfo As MBusinessUnitProfileInfo, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IBusinessUnits.AddBusinessUnitProfileInfo
        Dim retVal As Boolean = False
        Try
			OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_ADD_BU_PROFILE", _
			New OracleParameter() { _
			New OracleParameter("@P_Name", businessUnitProfileInfo.Name), _
			New OracleParameter("@P_Description", businessUnitProfileInfo.Description), _
			New OracleParameter("@P_Skin", businessUnitProfileInfo.Skin), _
			New OracleParameter("@P_Parent_Business_Unit_Seq_ID", businessUnitProfileInfo.Parent_Business_Unit_Seq_ID), _
			New OracleParameter("@P_STATUS_SEQ_ID", businessUnitProfileInfo.STATUS_SEQ_ID), _
			New OracleParameter("@P_CONNECTION_STRING", businessUnitProfileInfo.ConnctionString), _
			New OracleParameter("@P_DAL", businessUnitProfileInfo.DAL), _
			New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
			})
            retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function

    Public Sub GetValidBusinessUnits(ByRef yourDataSet As DataSet, ByVal Account_Seq_Id As Integer, ByVal isSysAdmin As Integer) Implements IBusinessUnits.GetValidBusinessUnits
		Try
			Dim BoolSysAdmin As Integer = -1 * CInt(isSysAdmin)
			yourDataSet = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_GET_VAID_BU_FOR_ACCOUNT", New OracleParameter() { _
			 New OracleParameter("@P_ACCOUNT_SEQ_ID", Account_Seq_Id), _
			 New OracleParameter("@P_isSysAdmin", BoolSysAdmin) _
			})
		Catch ex As Exception
			Throw ex
		End Try
    End Sub

    Public Function GetAllGroupsForBusinessUnit(ByVal dstGroups As System.Data.DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As System.Data.DataSet Implements IBusinessUnits.GetAllGroupsForBusinessUnit
        Try
            dstGroups = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_GET_ALL_GRPS_FOR_BU", New OracleParameter() { _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
            })
        Catch ex As Exception
            Throw ex
        End Try
        Return dstGroups
    End Function
End Class