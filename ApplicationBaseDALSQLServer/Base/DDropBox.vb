Imports System.Configuration
Imports System.Data.SqlClient
Imports ApplicationBase.Common
Imports ApplicationBaseDALSQLServer.SharedSQLServer
Imports ApplicationBase.Common.Security
Imports ApplicationBase.Interfaces
Imports ApplicationBase.Model

Public Class DDropBox
	Implements IDropBox

#Region " Private Objects "
	Private _ConnectionString As String = String.Empty
#End Region

#Region " Private Methods"
	'*********************************************************************
	'
	' ConnectionString Method
	'
	' Get the connection string from the web.config file.
	'
	'*********************************************************************
	Private Function ConnectionString() As String
		If _ConnectionString = String.Empty Then
			' try to decrypt the connection string
            _ConnectionString = ConnectionHelper.GetConnectionString("ApplicationBaseDALSQLServer")
		End If
		Return _ConnectionString
	End Function
#End Region

	Public Sub GET_DROP_BOX_NAME(ByVal ACCOUNT_SEQ_ID As Integer, ByRef theirDataView As DataView) Implements IDropBox.GET_DROP_BOX_NAME
		Try
			Dim myDataSet As DataSet
            myDataSet = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZBP_get_DROP_BOXES", New SqlParameter() { _
             New SqlParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID) _
            })
			theirDataView = myDataSet.Tables(0).DefaultView
		Catch ex As Exception
			Throw ex
		End Try
	End Sub

	Public Sub GET_DROP_BOX_DETAIL(ByRef theirDataView As System.Data.DataView) Implements IDropBox.GET_DROP_BOX_DETAIL
		Try
			Dim myDataSet As DataSet
			myDataSet = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZBP_get_DROP_BOX_DETAILS")
			theirDataView = myDataSet.Tables(0).DefaultView
		Catch ex As Exception
			Throw ex
		End Try
	End Sub

    Public Function UPDATE_DROP_BOX_DETAIL(ByVal DROP_BOX_DET_ID As Integer, ByVal DROP_BOX_DET_code As Integer, ByVal DROP_BOX_DET_value As String, ByVal DROP_BOX_DET_status As Integer, ByVal DROP_BOX_ID As Integer, Optional ByVal ACCOUNT_SEQ_ID As Integer = 1) As Boolean Implements IDropBox.UPDATE_DROP_BOX_DETAIL
        Dim retVal As Boolean = False
        SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZBP_update_DROP_BOX_DETAILS", New SqlParameter() { _
        New SqlParameter("@P_Id", DROP_BOX_DET_ID), _
        New SqlParameter("@P_Code", DROP_BOX_DET_code), _
        New SqlParameter("@P_value", DROP_BOX_DET_value), _
        New SqlParameter("@P_status", DROP_BOX_DET_status), _
        New SqlParameter("@P_DROP_BOX_SEQ_ID", DROP_BOX_ID), _
        New SqlParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
        New SqlParameter("@P_ADDUPD_BY", ACCOUNT_SEQ_ID)})
        Return retVal
    End Function

    Public Function ADD_DROP_BOX_DETAIL(ByVal DROP_BOX_DET_code As Integer, ByVal DROP_BOX_DET_value As String, ByVal DROP_BOX_DET_status As Integer, ByVal DROP_BOX_SEQ_ID As Integer, Optional ByVal ACCOUNT_SEQ_ID As Integer = 1) As Boolean Implements IDropBox.ADD_DROP_BOX_DETAIL
        Dim retVal As Boolean = False
        SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZBP_Add_DROP_BOX_DETAILS", New SqlParameter() { _
        New SqlParameter("@P_CODE", DROP_BOX_DET_code), _
        New SqlParameter("@P_VALUE", DROP_BOX_DET_value), _
        New SqlParameter("@P_STATUS", DROP_BOX_DET_status), _
        New SqlParameter("@P_DROP_BOX_SEQ_ID", DROP_BOX_SEQ_ID), _
        New SqlParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
        New SqlParameter("@P_ADDUPD_BY", ACCOUNT_SEQ_ID) _
        })
        Return retVal
    End Function

	Public Sub AddDropBoxRoles(ByVal DROP_BOX_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal moduleRoleType As MRoleType.value, ByVal roles() As String, Optional ByVal Account_Seq_Id As Integer = 1) Implements IDropBox.AddDropBoxRoles
		Dim role As String
		Dim dbConn As New SqlConnection(ConnectionString)
		dbConn.Open()
		Dim trans As SqlTransaction = dbConn.BeginTransaction
		Try
			' delete the roles
			SqlHelper.ExecuteNonQuery(trans, _
			CommandType.StoredProcedure, _
			"ZBP_del_DroBox_Security_By_Role", _
			 New SqlParameter() { _
			 New SqlParameter("@P_DROP_BOX_SEQ_ID", DROP_BOX_SEQ_ID), _
			 New SqlParameter("@P_PERMISSIONS_SEQ_ID", moduleRoleType), _
			 New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
			 New SqlParameter("@P_ADDUPD_BY", Account_Seq_Id) _
			})
			' Add the selected roles
			For Each role In roles
				SqlHelper.ExecuteNonQuery(trans, _
				CommandType.StoredProcedure, _
				"ZBP_add_DROP_BOX_Role", _
				New SqlParameter() { _
				New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
				New SqlParameter("@P_DROP_BOX_SEQ_ID", DROP_BOX_SEQ_ID), _
				New SqlParameter("@P_PERMISSIONS_SEQ_ID", moduleRoleType), _
				New SqlParameter("@P_role", role), _
				New SqlParameter("@P_ADDUPD_BY", Account_Seq_Id) _
				})
			Next role
			trans.Commit()
		Catch ex As Exception
			If Not trans Is Nothing Then trans.Rollback()
			Throw ex
		Finally
			If Not dbConn Is Nothing Then
				dbConn.Close()
				dbConn.Dispose()
			End If
			If Not trans Is Nothing Then
				trans.Dispose()
			End If
		End Try
	End Sub

    Public Function ADD_DROP_BOX(ByVal ACCOUNT_SEQ_ID As Integer, ByVal DESCRIPTION As String, Optional ByVal Accnt_Seq_Id As Integer = 1) As Integer Implements IDropBox.ADD_DROP_BOX
        Dim retVal As Integer
        Dim returnParam As New SqlParameter("@P_RETURN_VALUE", SqlDbType.Int)
        returnParam.Direction = ParameterDirection.ReturnValue
        SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZBP_add_DROP_BOX", New SqlParameter() { _
        New SqlParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
        New SqlParameter("@P_DESCRIPTION", DESCRIPTION), _
        New SqlParameter("@P_ADDUPD_BY", Accnt_Seq_Id), _
        returnParam})
        retVal = CInt(returnParam.Value)
        Return retVal
    End Function

	Public Function UPDATE_DROP_BOX(ByVal DROP_BOX_SEQ_ID As Integer, ByVal DESCRIPTION As String, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IDropBox.UPDATE_DROP_BOX
		Dim retVal As Integer
		Dim returnParam As New SqlParameter("@P_RETURN_VALUE", SqlDbType.Int)
		returnParam.Direction = ParameterDirection.ReturnValue
		SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZBP_update_DROP_BOX", New SqlParameter() { _
		New SqlParameter("@P_DROP_BOX_SEQ_ID", DROP_BOX_SEQ_ID), _
		New SqlParameter("@P_DESCRIPTION", DESCRIPTION), _
		New SqlParameter("@P_ADDUPD_BY", Account_Seq_Id), _
		returnParam})
		retVal = CInt(returnParam.Value)
		Return retVal
	End Function
End Class
