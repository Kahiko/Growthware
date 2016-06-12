Imports System.Configuration
Imports System.Data.SqlClient
Imports ApplicationBase.Common
Imports ApplicationBaseDALSQLServer.SharedSQLServer
Imports ApplicationBase.Common.Security
Imports ApplicationBase.Model.WorkFlows
Imports ApplicationBase.Interfaces

Public Class DWorkFlows
    Implements IWorkFlows

#Region " Private Objects "
    Private _ConnectionString As String = String.Empty
#End Region

#Region "Private Methods"
    '*********************************************************************
    ' ConnectionString Method
    ' Get the connection string from the web.config file.
    '*********************************************************************
    Private Function ConnectionString() As String
        If _ConnectionString = String.Empty Then
            ' try to decrypt the connection string
            _ConnectionString = ConnectionHelper.GetConnectionString("ApplicationBaseDALSQLServer")
        End If
        Return _ConnectionString
    End Function

    '*********************************************************************
    ' CalculateWorkFlows Method
    ' Calculate work flow information and 
    ' inherited properties by iterating through each 
    ' row in a DataSet containing work flow rows.
    '*********************************************************************
    Private Shared Function CalculateWorkFlows(ByVal dstWorkFlows As DataSet) As MWorkFlowProfileInfoCollection
        Dim workFlowInfoCollection As New MWorkFlowProfileInfoCollection
        Dim dtblWorkFlows As DataTable = dstWorkFlows.Tables(0)

        ' Calculated inherited properties for each work flow
        Dim drowWorkFlowProfile As DataRow
        For Each drowWorkFlowProfile In dtblWorkFlows.Rows
            If Not drowWorkFlowProfile(0) Is Nothing Then
                ' Add the workflowinfo to the collection
                workFlowInfoCollection.Add(Trim(CStr(drowWorkFlowProfile("ORDER_ID"))), New MWorkFlowProfileInfo(drowWorkFlowProfile))
            End If
        Next drowWorkFlowProfile

        Return workFlowInfoCollection
    End Function 'CalculateDirectories
#End Region

    Public Function AddProfile(ByVal profile As MWorkFlowProfileInfo, Optional ByVal Account_Seq_id As Integer = 1) As Boolean Implements IWorkFlows.AddProfile
        Dim retVal As Integer
        Try
            Dim returnParam As New SqlParameter("@P_RETURN_VALUE", SqlDbType.Int)
            returnParam.Direction = ParameterDirection.ReturnValue

            SqlHelper.ExecuteNonQuery( _
             ConnectionString, _
             CommandType.StoredProcedure, _
             "ZBP_add_Work_Flow_Profile", _
             New SqlParameter() { _
             New SqlParameter("@P_ORDER_ID", profile.Order), _
             New SqlParameter("@P_WORK_FLOW_NAME", profile.WorkFlowName), _
             New SqlParameter("@P_ACTION", profile.Action), returnParam, _
             New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
             })
            retVal = Fix(returnParam.Value)
        Catch ex As Exception
            Throw ex
            'Throw New ApplicationException(ResourceManager.GetString("RES_ExceptionCantCreateOrder"), e)
        End Try
        Return CBool(retVal)
    End Function

    Public Function DeleteProfile(ByVal Profile As MWorkFlowProfileInfo, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IWorkFlows.DeleteProfile
        Dim retVal As Integer
        Try
            Dim returnParam As New SqlParameter("@P_RETURN_VALUE", SqlDbType.Int)
            returnParam.Direction = ParameterDirection.ReturnValue
			SqlHelper.ExecuteNonQuery( _
			 ConnectionString, _
			 CommandType.StoredProcedure, _
			 "ZBP_DEL_WORK_FLOW_PROFILE", _
			 New SqlParameter() { _
			 New SqlParameter("@P_ORDER_ID", Profile.Order), _
			 New SqlParameter("@P_WORK_FLOW_NAME", Profile.WorkFlowName), _
			 New SqlParameter("@P_ADDUPD_BY", Account_Seq_Id), _
			 returnParam _
			 })
            retVal = Fix(returnParam.Value)
        Catch ex As Exception
            Throw ex
            'Throw New ApplicationException(ResourceManager.GetString("RES_ExceptionCantCreateOrder"), e)
        End Try
        Return CBool(retVal)
    End Function

    Public Function GetCollectionFromDB(ByVal WorkFlowName As String) As MWorkFlowProfileInfoCollection Implements IWorkFlows.GetCollectionFromDB
        Dim myDataSet As New DataSet
        Dim myWorkFlowProfileInfoCollection As MWorkFlowProfileInfoCollection
        Try
            myDataSet = SqlHelper.ExecuteDataset(ConnectionString, _
             CommandType.StoredProcedure, _
             "ZBP_get_Work_Flows", _
             New SqlParameter() { _
             New SqlParameter("@P_WORK_FLOW_NAME", WorkFlowName)})
            myDataSet.Tables(0).TableName = "dtblWorkFlows"
            myWorkFlowProfileInfoCollection = CalculateWorkFlows(myDataSet)
        Catch ex As Exception
            Throw ex
        Finally
            If Not myDataSet Is Nothing Then
                myDataSet = Nothing
            End If
        End Try
        Return myWorkFlowProfileInfoCollection
    End Function

    Public Function UpdateProfile(ByVal profile As MWorkFlowProfileInfo, Optional ByVal Account_seq_Id As Integer = 1) As Boolean Implements IWorkFlows.UpdateProfile
        Dim retVal As Integer
        Try
            Dim returnParam As New SqlParameter("@P_RETURN_VALUE", SqlDbType.Int)
            returnParam.Direction = ParameterDirection.ReturnValue

            SqlHelper.ExecuteNonQuery( _
             ConnectionString, _
             CommandType.StoredProcedure, _
             "ZBP_update_Work_Flow_Profile", _
             New SqlParameter() { _
             New SqlParameter("@P_WORK_FLOW_SEQ_ID", profile.WORK_FLOW_SEQ_ID), _
             New SqlParameter("@P_ORDER_ID", profile.Order), _
             New SqlParameter("@P_WORK_FLOW_NAME", profile.WorkFlowName), _
             New SqlParameter("@P_ACTION", profile.Action), _
             New SqlParameter("@P_ADDUPD_BY", Account_seq_Id), _
             returnParam _
             })
            retVal = Fix(returnParam.Value)
        Catch ex As Exception
            Throw ex
            'Throw New ApplicationException(ResourceManager.GetString("RES_ExceptionCantCreateOrder"), e)
        End Try
        Return CBool(retVal)
    End Function

    Public Sub GetWorkFlowsFromDB(ByVal WORK_FLOW_NAME As String, ByRef YourDataSet As DataSet) Implements IWorkFlows.GetWorkFlowsFromDB
        Dim reader As SqlDataReader = Nothing
        Try
            reader = SqlHelper.ExecuteReader(ConnectionString, _
               CommandType.StoredProcedure, "ZBP_get_Work_Flows", New SqlParameter() {New SqlParameter("@P_WORK_FLOW_NAME", WORK_FLOW_NAME)})
            If Not reader Is Nothing Then
                SqlHelperExtension.Fill(reader, YourDataSet, "dsResult", 0, 0)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not reader Is Nothing Then
                reader = Nothing
            End If
        End Try
    End Sub


    Public Sub GetWorkFlowsFromDB(ByRef YourDataSet As DataSet) Implements IWorkFlows.GetWorkFlowsFromDB
        Dim reader As SqlDataReader = Nothing
        Try
            reader = SqlHelper.ExecuteReader(ConnectionString, _
               CommandType.StoredProcedure, "ZBP_get_Work_Flows")
            If Not reader Is Nothing Then
                SqlHelperExtension.Fill(reader, YourDataSet, "dsResult", 0, 0)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not reader Is Nothing Then
                reader = Nothing
            End If
        End Try
    End Sub
End Class