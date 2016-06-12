Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports ApplicationBase.Common
Imports ApplicationBase.Model.Directories
Imports ApplicationBase.Interfaces
Imports ApplicationBaseDALOracle.SharedOracle

Public Class DDirectoryInfo
	Implements IDirectoryInfo

	Private _ConnectionString As String = String.Empty

    Public Function addUpdateDirectoryInfo(ByVal directoryInfo As MDirectoryProfileInformation, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IDirectoryInfo.addUpdateDirectoryInfo
		Dim retVal As Boolean = True
		Dim isImpersonate As Integer = 0
		If directoryInfo.Impersonate Then
			isImpersonate = 1
		End If
		Try
			OracleHelper.ExecuteNonQuery(ConnectionString, _
			CommandType.StoredProcedure, _
			"ZB_SECURITY_PKG.ZBP_ADD_UPDATE_DIRECTORY_INFO", _
			 New OracleParameter() { _
			 New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", directoryInfo.BUSINESS_UNIT_SEQ_ID), _
			 New OracleParameter("@P_Directory", directoryInfo.Directory), _
			 New OracleParameter("@P_Impersonate", isImpersonate), _
			 New OracleParameter("@P_Impersonate_Account", directoryInfo.Impersonate_Account), _
			 New OracleParameter("@P_Impersonate_PWD", directoryInfo.Impersonate_PWD), _
			 New OracleParameter("@P_ACCOUNT_SEQ_ID", Account_Seq_Id) _
			 })
		Catch ex As Exception
			Throw ex
		End Try
		Return retVal
    End Function

    Public Function GetDirectoryCollectionFromDB() As MDirectoryProfileInfoCollection Implements IDirectoryInfo.GetDirectoryCollectionFromDB
        Dim dstDirectories As New DataSet
        Dim reader As OracleDataReader = Nothing
        Try
            reader = OracleHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, _
               "ZB_SECURITY_PKG.ZBP_get_All_DirectoryInfo")
            OracleHelperExtension.Fill(reader, dstDirectories, "Directories", 0, 0)
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
            _ConnectionString = ConnectionHelper.GetConnectionString("ApplicationBaseDALOracle")
        End If
        Return _ConnectionString
    End Function
#End Region
End Class