Imports Common
Imports Common.SQLServer
Imports Common.Security.BaseSecurity
Imports DALInterface.Base.Interfaces
Imports DALModel
Imports DALModel.Base.Directories
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO

Public Class DDirectoryInfo
    Implements IDirectoryInfo

    Private _ConnectionString As String = String.Empty

    Public Function addUpdateDirectoryInfo(ByVal directoryInfo As MDirectoryProfileInformation, Optional ByVal Account_Seq_id As Integer = 1) As Boolean Implements IDirectoryInfo.addUpdateDirectoryInfo
        Dim retVal As Boolean
        Try
            Dim returnParam As New SqlParameter("@P_RETURN_VALUE", SqlDbType.NVarChar)
            returnParam.Direction = ParameterDirection.ReturnValue
			SqlHelper.ExecuteNonQuery(ConnectionString, _
			CommandType.StoredProcedure, _
			"ZBP_ADD_UPDATE_DIRECTORY_INFO", _
			 New SqlParameter() { _
			 New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", directoryInfo.BUSINESS_UNIT_SEQ_ID), _
			 New SqlParameter("@P_Directory", directoryInfo.Directory), _
			 New SqlParameter("@P_Impersonate", directoryInfo.Impersonate), _
			 New SqlParameter("@P_Impersonate_Account", directoryInfo.Impersonate_Account), _
			 New SqlParameter("@P_Impersonate_PWD", directoryInfo.Impersonate_PWD), returnParam, _
			 New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
			 })
            retVal = Fix(returnParam.Value)
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function

    Public Function GetDirectoryCollectionFromDB() As MDirectoryProfileInfoCollection Implements IDirectoryInfo.GetDirectoryCollectionFromDB
        Dim dstDirectories As New DataSet
        Dim reader As SqlDataReader = Nothing
        Try
            reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, _
               "ZBP_get_All_DirectoryInfo")
            SqlHelperExtension.Fill(reader, dstDirectories, "Directories", 0, 0)
            reader.Close()
        Catch ex As Exception
            Throw ex
        Finally
            If Not (reader Is Nothing) Then
                CType(reader, IDisposable).Dispose()
            End If
        End Try
        Return CalculateDirectories(dstDirectories)
    End Function

#Region " Private "
    '*********************************************************************
    '
    ' CalculateDirectories Method
    ' Calculates directory information such as directy paths and 
    ' inherited properties by iterating through each 
    ' row in a DataSet containing directory rows.
    '
    '*********************************************************************
    Private Shared Function CalculateDirectories(ByVal dstDirectories As DataSet) As MDirectoryProfileInfoCollection
        Dim directoryInfoCollection As New MDirectoryProfileInfoCollection
        Dim dtblDirectories As DataTable = dstDirectories.Tables("Directories")

        ' Calculated inherited properties for each directory
        Dim drowDirectory As DataRow
        For Each drowDirectory In dtblDirectories.Rows
            If Not drowDirectory("BUSINESS_UNIT_SEQ_ID") Is Nothing Then
                ' Add the directoryInfo to the collection
                directoryInfoCollection.Add(Trim(CStr(drowDirectory("BUSINESS_UNIT_SEQ_ID"))), New MDirectoryProfileInformation(drowDirectory))
            End If
        Next drowDirectory

        Return directoryInfoCollection
    End Function 'CalculateDirectories

    '*********************************************************************
    '
    ' ConnectionString Method
    ' Get the connection string from the web.config file.
    '
    '*********************************************************************
    Private Function ConnectionString() As String
        If _ConnectionString = String.Empty Then
            ' try to decrypt the connection string
            _ConnectionString = ConnectionHelper.GetConnectionString("SQLServerAppDB")
        End If
        Return _ConnectionString
    End Function
#End Region
End Class