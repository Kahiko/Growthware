Imports System.Configuration
Imports System.Data.SqlClient
Imports ApplicationBase.Common
Imports ApplicationBaseDALSQLServer.SharedSQLServer
Imports ApplicationBase.Common.Security
Imports ApplicationBase.Interfaces
Imports ApplicationBase.Model.BusinessUnits

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
    End Function 'CalculateDirectories

    '*********************************************************************
    '
    ' ConnectionString Method
    ' Gets the connection string from the web.config file.
    '
    '*********************************************************************
    Private Function ConnectionString() As String
        If _ConnectionString = String.Empty Then
            ' try to decrypt the connection string
            _ConnectionString = ConnectionHelper.GetConnectionString("ApplicationBaseDALSQLServer")
        End If
        Return _ConnectionString
    End Function

    Private Function PopulateProfileFromSqlDataReader(ByVal dr As SqlDataReader) As MBusinessUnitProfileInfo
        Dim profile As New MBusinessUnitProfileInfo
        profile.BUSINESS_UNIT_SEQ_ID = CStr(dr("BUSINESS_UNIT_SEQ_ID"))
        profile.Name = CStr(dr("Name"))
        profile.Description = CStr(dr("Description"))
        profile.Skin = CStr(dr("Skin"))
        profile.Parent_Business_Unit_Seq_ID = CStr(dr("Parent_Business_Unit_Seq_ID"))
        profile.STATUS_SEQ_ID = CInt(dr("STATUS_SEQ_ID"))
        profile.ConnctionString = CStr(dr("CONNECTION_STRING"))
        Return profile
    End Function 'PopulateProfileFromSqlDataReader
#End Region

	'Public Function GetAdminBusinessUnitArray(ByVal ACCOUNT_SEQ_ID As Integer) As String Implements IBusinessUnits.GetAdminBusinessUnitArray

	'End Function

    Public Function GetAllBusinessUnits() As MBusinessUnitProfileInfoCollection Implements IBusinessUnits.GetAllBusinessUnits
        Dim myDataSet As New DataSet
        Dim myBusinessUnitProfileInfoCollection As MBusinessUnitProfileInfoCollection
        Try
            myDataSet = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZBP_GET_ALL_BU_ALL_INFO")
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
            dstRoles = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZBP_GET_ALL_RLS_FOR_BU", New SqlParameter() { _
             New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
            })
        Catch ex As Exception
            Throw ex
        End Try
        Return dstRoles
    End Function

	'Public Function GetBusinessUnitArray(ByVal ACCOUNT_SEQ_ID As Integer) As String Implements IBusinessUnits.GetBusinessUnitArray

	'End Function

    '*********************************************************************
    '
    ' GetConnectionString Method
    ' Gets the connection string from the database for a given Business Unit.
    '
    '*********************************************************************
    Public Function GetConnectionString(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String Implements IBusinessUnits.GetConnectionString
        Dim retString As String = String.Empty
        retString = SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "ZBP_get_Connection_String", New SqlParameter() {New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID)})
        Try
            Dim myCryptoUtil As New CryptoUtil
            retString = myCryptoUtil.Decrypt(retString)
        Catch ex As Exception
            ' do nothing the most likely cause for failure
            ' is the value is clear text
        End Try
        Return retString
    End Function

    Public Function UpdateBusinessUnitProfileInfo(ByRef businessUnitProfileInfo As MBusinessUnitProfileInfo, Optional ByVal Account_Seq_id As Integer = 1) As Boolean Implements IBusinessUnits.UpdateBusinessUnitProfileInfo
        Dim retVal As Boolean = False
        Try
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZBP_UPDATE_BU_PROFILE", _
            New SqlParameter() { _
            New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", businessUnitProfileInfo.BUSINESS_UNIT_SEQ_ID), _
            New SqlParameter("@P_DESCRIPTION", businessUnitProfileInfo.Description), _
            New SqlParameter("@P_NAME", businessUnitProfileInfo.Name), _
            New SqlParameter("@P_PARENT_BUSINESS_UNIT_SEQ_ID", businessUnitProfileInfo.Parent_Business_Unit_Seq_ID), _
            New SqlParameter("@P_SKIN", businessUnitProfileInfo.Skin), _
            New SqlParameter("@P_STATUS_SEQ_ID", businessUnitProfileInfo.STATUS_SEQ_ID), _
            New SqlParameter("@P_CONNECTION_STRING", businessUnitProfileInfo.ConnctionString), _
            New SqlParameter("@P_DAL", businessUnitProfileInfo.DAL), _
            New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
            })
            retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function

    Public Function AddBusinessUnitProfileInfo(ByRef businessUnitProfileInfo As MBusinessUnitProfileInfo, Optional ByVal Account_Seq_id As Integer = 1) As Boolean Implements IBusinessUnits.AddBusinessUnitProfileInfo
        Dim retVal As Boolean = False
        Try
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZBP_add_Business_Unit_Profile", _
            New SqlParameter() { _
            New SqlParameter("@P_Name", businessUnitProfileInfo.Name), _
            New SqlParameter("@P_Description", businessUnitProfileInfo.Description), _
            New SqlParameter("@P_Skin", businessUnitProfileInfo.Skin), _
            New SqlParameter("@P_Parent_Business_Unit_Seq_ID", businessUnitProfileInfo.Parent_Business_Unit_Seq_ID), _
            New SqlParameter("@P_STATUS_SEQ_ID", businessUnitProfileInfo.STATUS_SEQ_ID), _
            New SqlParameter("@P_CONNECTION_STRING", businessUnitProfileInfo.ConnctionString), _
            New SqlParameter("@P_DAL", businessUnitProfileInfo.DAL), _
            New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
            })
            retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function

    Public Sub GetValidBusinessUnits(ByRef yourDataSet As DataSet, ByVal Account_Seq_Id As Integer, ByVal isSysAdmin As Integer) Implements IBusinessUnits.GetValidBusinessUnits
        Try
            yourDataSet = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZBP_GET_VAID_BU_FOR_ACCOUNT", New SqlParameter() { _
             New SqlParameter("@P_ACCOUNT_SEQ_ID", Account_Seq_Id), _
             New SqlParameter("@P_isSysAdmin", isSysAdmin) _
            })
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

	Public Function GetAllGroupsForBusinessUnit(ByVal dstGroups As System.Data.DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As System.Data.DataSet Implements IBusinessUnits.GetAllGroupsForBusinessUnit
		Try
			dstGroups = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZBP_GET_ALL_GRPS_FOR_BU", New SqlParameter() { _
			 New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
			})
		Catch ex As Exception
			Throw ex
		End Try
		Return dstGroups
	End Function
End Class