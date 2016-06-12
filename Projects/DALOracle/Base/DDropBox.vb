Imports System.Configuration
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports Common
Imports Common.Oracle
Imports DALInterface.Base.Interfaces

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
			_ConnectionString = ConnectionHelper.GetConnectionString("OracleDAL")
		End If
		Return _ConnectionString
	End Function
#End Region

	Public Sub GET_DROP_BOX_NAME(ByVal ACCOUNT_SEQ_ID As Integer, ByRef theirDataView As DataView) Implements IDropBox.GET_DROP_BOX_NAME
		Try
			Dim myDataSet As DataSet
			myDataSet = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_DROP_BOXES", New OracleParameter() { _
			 New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID) _
			})
			theirDataView = myDataSet.Tables(0).DefaultView
		Catch ex As Exception
			Throw ex
		End Try
	End Sub

	Public Sub GET_DROP_BOX_DETAIL(ByRef theirDataView As System.Data.DataView) Implements IDropBox.GET_DROP_BOX_DETAIL
		Try
			Dim myDataSet As DataSet
			myDataSet = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_DROP_BOX_DETAILS")
			theirDataView = myDataSet.Tables(0).DefaultView
		Catch ex As Exception
			Throw ex
		End Try
	End Sub

    
    Public Sub AddDropBoxRoles(ByVal DROP_BOX_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal moduleRoleType As DALModel.Base.MRoleType.value, ByVal roles() As String, Optional ByVal Account_Seq_Id As Integer = 1) Implements IDropBox.AddDropBoxRoles
        Dim role As String
        Dim dbConn As New OracleConnection(ConnectionString)
        dbConn.Open()
        Dim trans As OracleTransaction = dbConn.BeginTransaction
        Try
            ' delete the roles
			OracleHelper.ExecuteNonQuery(trans, _
			CommandType.StoredProcedure, _
			"ZB_SECURITY_PKG.ZBP_del_DroBox_Security_By_Role", _
			New OracleParameter() { _
			New OracleParameter("@P_DROP_BOX_SEQ_ID", DROP_BOX_SEQ_ID), _
			New OracleParameter("@P_PERMISSIONS_SEQ_ID", moduleRoleType), _
			   New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
			})
            ' Add the selected roles
            For Each role In roles
                OracleHelper.ExecuteNonQuery(trans, _
                CommandType.StoredProcedure, _
                "ZB_SECURITY_PKG.ZBP_add_DROP_BOX_Role", _
                New OracleParameter() { _
                New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
                New OracleParameter("@P_DROP_BOX_SEQ_ID", DROP_BOX_SEQ_ID), _
                New OracleParameter("@P_PERMISSIONS_SEQ_ID", moduleRoleType), _
                New OracleParameter("@P_role", role), _
                New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
                })
            Next role
            trans.Commit()
        Catch ex As Exception
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
        Dim returnParam As New OracleParameter("@P_RETURN_VALUE", OracleDbType.Int32)
        returnParam.Direction = ParameterDirection.ReturnValue
        OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_add_DROP_BOX", New OracleParameter() { _
        New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
        New OracleParameter("@P_DESCRIPTION", DESCRIPTION), _
        New OracleParameter("@P_ADDUPD_BY", Accnt_Seq_Id), _
        returnParam})
        retVal = CInt(returnParam.Value)
        Return retVal
    End Function

    Public Function UPDATE_DROP_BOX(ByVal DROP_BOX_SEQ_ID As Integer, ByVal DESCRIPTION As String, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IDropBox.UPDATE_DROP_BOX
        Dim retVal As Integer
        Dim returnParam As New OracleParameter("@P_RETURN_VALUE", OracleDbType.Int32)
        returnParam.Direction = ParameterDirection.ReturnValue
        OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_update_DROP_BOX", New OracleParameter() { _
        New OracleParameter("@P_DROP_BOX_SEQ_ID", DROP_BOX_SEQ_ID), _
        New OracleParameter("@P_DESCRIPTION", DESCRIPTION), _
        New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id), _
        returnParam})
        retVal = CInt(returnParam.Value)
        Return retVal
    End Function

	Public Function ADD_DROP_BOX_DETAIL(ByVal DROP_BOX_DET_code As Integer, ByVal DROP_BOX_DET_value As String, ByVal DROP_BOX_DET_status As Integer, ByVal DROP_BOX_ID As Integer, Optional ByVal ACCOUNT_SEQ_ID As Integer = 1) As Boolean Implements IDropBox.ADD_DROP_BOX_DETAIL
		Dim retVal As Boolean = False
		OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_Add_DROP_BOX_DETAILS", New OracleParameter() { _
		New OracleParameter("@P_Code", DROP_BOX_DET_code), _
		New OracleParameter("@P_value", DROP_BOX_DET_value), _
		New OracleParameter("@P_status", DROP_BOX_DET_status), _
		New OracleParameter("@P_DROP_BOX_SEQ_ID", DROP_BOX_ID), _
		New OracleParameter("@P_ADDUPD_BY", ACCOUNT_SEQ_ID)})
		Return retVal
	End Function

	Public Function UPDATE_DROP_BOX_DETAIL(ByVal DROP_BOX_DET_ID As Integer, ByVal DROP_BOX_DET_code As Integer, ByVal DROP_BOX_DET_value As String, ByVal DROP_BOX_DET_status As Integer, ByVal DROP_BOX_ID As Integer, Optional ByVal ACCOUNT_SEQ_ID As Integer = 1) As Boolean Implements IDropBox.UPDATE_DROP_BOX_DETAIL
		Dim retVal As Boolean = False
		OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_update_DROP_BOX_DETAILS", New OracleParameter() { _
		New OracleParameter("@P_ID", DROP_BOX_DET_ID), _
		New OracleParameter("@P_CODE", DROP_BOX_DET_code), _
		New OracleParameter("@P_VALUE", DROP_BOX_DET_value), _
		New OracleParameter("@P_STATUS", DROP_BOX_DET_status), _
		New OracleParameter("@P_DROP_BOX_SEQ_ID", DROP_BOX_ID), _
		New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
		New OracleParameter("@P_ADDUPD_BY", ACCOUNT_SEQ_ID)})
		Return retVal
	End Function
End Class
