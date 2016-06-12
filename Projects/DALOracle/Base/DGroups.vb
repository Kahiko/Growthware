Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports Common
Imports Common.Oracle
Imports DALModel.Base.Group
Imports DALInterface.Base.Interfaces
Imports System.Configuration

Public Class DGroups
	Implements IGroups

#Region " PRIVATE OBJECTS "
	Private _ConnectionString As String = String.Empty
#End Region

#Region " PRIVATE METHODS"
	Private Function ConnectionString() As String
		If _ConnectionString = String.Empty Then
			' try to decrypt the connection string
			_ConnectionString = ConnectionHelper.GetConnectionString("OracleDAL")
		End If
		Return _ConnectionString
	End Function

	Private Function PopulateGroupInfoFromOracleDataReader(ByVal dr As OracleDataReader) As MGroupInfo
		On Error Resume Next		  ' give what we can
		Dim groupInfo As New MGroupInfo
		With groupInfo
			.GroupId = Fix(dr("GROUP_SEQ_ID"))
			.GroupName = dr("GROUP_NAME")
			.GroupDescription = dr("DESCRIPTION")
		End With
		Return groupInfo
	End Function	'PopulateProfileFromOracleDataReader
#End Region

    Public Function AddGroup(ByRef GroupInfo As MGroupInfo, Optional ByVal Account_Seq_Id As Integer = 1) As Integer Implements IGroups.AddGroup
        Dim retVal As Integer
        Dim returnParam As New OracleParameter("@P_RETURN_VALUE", OracleDbType.Int32)
        returnParam.Direction = ParameterDirection.ReturnValue
        With GroupInfo
            OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_ADD_GROUP_INFO", New OracleParameter() { _
            New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", .BusinessUnitId), _
            New OracleParameter("@P_GROUP_NAME", .GroupName), _
            New OracleParameter("@P_GROUP_DESCRIPTION", .GroupDescription), _
New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id), _
returnParam})

            retVal = CInt(returnParam.Value)
        End With
        Return retVal
    End Function

    Public Function GetRolesByBusinessUnit(ByVal GroupId As Integer, ByVal BusinessUnitSeqId As Integer) As String() Implements IGroups.GetRolesByBusinessUnit
        Dim groupRoles As New ArrayList
        Dim reader As OracleDataReader = Nothing
        Try
            reader = OracleHelper.ExecuteReader( _
               ConnectionString, _
               CommandType.StoredProcedure, _
               "ZB_SECURITY_PKG.ZBP_GET_RLS_FOR_GROUP_BY_BU", _
               New OracleParameter() { _
             New OracleParameter("@P_GROUP_SEQ_ID", GroupId), _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BusinessUnitSeqId) _
               } _
               )
            While reader.Read()
                groupRoles.Add(reader("ROLE_NAME"))
            End While
        Catch ex As Exception
            Throw ex
        Finally
            If Not (reader Is Nothing) Then
                CType(reader, IDisposable).Dispose()
            End If
        End Try
        ' Return the String array of roles
        Return CType(groupRoles.ToArray(GetType(String)), String())

    End Function

    Public Function GetRolesForGroup(ByVal GroupId As Integer, Optional ByVal BusinessUnitSeqId As Integer = 1) As String() Implements IGroups.GetRolesForGroup
        Dim groupRoles As New ArrayList
        Dim reader As OracleDataReader = Nothing
        Try
            reader = OracleHelper.ExecuteReader( _
               ConnectionString, _
               CommandType.StoredProcedure, _
               "ZB_SECURITY_PKG.ZBP_GET_RLS_FOR_GROUP", _
               New OracleParameter() { _
             New OracleParameter("@P_GROUP_SEQ_ID", GroupId), _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BusinessUnitSeqId) _
               } _
               )
            While reader.Read()
                groupRoles.Add(reader("RLS"))
            End While
        Catch ex As Exception
            Throw ex
        Finally
            If Not (reader Is Nothing) Then
                CType(reader, IDisposable).Dispose()
            End If
        End Try
        ' Return the String array of roles
        Return CType(groupRoles.ToArray(GetType(String)), String())
    End Function

    Public Function SearchGroups(ByVal GroupName As String, ByVal BusinessUnitSeqId As Integer) As System.Data.DataSet Implements IGroups.SearchGroups
        Dim dsGroups As DataSet
        Try
            dsGroups = OracleHelper.ExecuteDataset( _
             ConnectionString, _
             CommandType.StoredProcedure, _
             "ZB_SECURITY_PKG.ZBP_GET_ALL_GRPS_BY_BU", New OracleParameter() _
             {New OracleParameter("@P_GROUP_NAME", GroupName), _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BusinessUnitSeqId)} _
             )
        Catch ex As Exception
            Throw ex
        End Try
        Return dsGroups
    End Function

    Public Function UpdateAGroup(ByVal GroupInfo As MGroupInfo, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IGroups.UpdateAGroup
        Dim retVal As Integer
        Dim returnParam As New OracleParameter("@P_RETURN_VALUE", OracleDbType.Int32)
        returnParam.Direction = ParameterDirection.ReturnValue
        With GroupInfo
            OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_UPDATE_GROUP_INFO", New OracleParameter() { _
            New OracleParameter("@P_GROUP_SEQ_ID", .GroupId), _
            New OracleParameter("@P_GROUP_NAME", .GroupName), _
            New OracleParameter("@P_GROUP_DESCRIPTION", .GroupDescription), _
New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id), _
returnParam})
        End With
        retVal = CInt(returnParam.Value)

        Return retVal
    End Function

    Public Sub UpdateRoles(ByVal GroupId As Integer, ByVal BusinessUnitSeqId As Integer, ByVal roles() As String, Optional ByVal Account_Seq_Id As Integer = 1) Implements IGroups.UpdateRoles
        Dim rolesCommaSeparated As String = String.Empty
        Dim role As String = String.Empty
        ' left extra code as an example of using transactional database interaction
        Try
            ' separate the roles with comma and send in one shot
            For Each role In roles
                rolesCommaSeparated += role.Trim & ","
            Next role
            If Not rolesCommaSeparated.Trim = String.Empty Then
                rolesCommaSeparated = Left(rolesCommaSeparated, Len(rolesCommaSeparated) - 1)
            End If
            OracleHelper.ExecuteNonQuery(ConnectionString, _
             CommandType.StoredProcedure, _
             "ZB_SECURITY_PKG.ZBP_UPDATE_GROUP_RLS", _
             New OracleParameter() { _
             New OracleParameter("@P_GROUP_SEQ_ID", GroupId), _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BusinessUnitSeqId), _
             New OracleParameter("@P_ROLES", rolesCommaSeparated), _
             New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
             })

        Catch ex As Exception
            Throw ex
        Finally   ' clean up

        End Try
    End Sub

    Public Function GetGroupInfo(ByVal GroupId As Integer) As MGroupInfo Implements IGroups.GetGroupInfo
        Dim profile As MGroupInfo
        Dim reader As OracleDataReader
        Try
            reader = OracleHelper.ExecuteReader( _
               ConnectionString, _
               CommandType.StoredProcedure, _
               "ZB_SECURITY_PKG.ZBP_GET_GROUP_INFO", _
               New OracleParameter() {New OracleParameter("@P_GROUP_ID", GroupId)} _
               )
            If reader.Read() Then
                profile = PopulateGroupInfoFromOracleDataReader(reader)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not (reader Is Nothing) Then
                CType(reader, IDisposable).Dispose()
            End If
        End Try
        Return profile
    End Function

    Public Sub DeleteGroup(ByVal GroupSeqId As String, ByVal BusinessUnitSeqId As Integer, Optional ByVal Account_Seq_Id As Integer = 1) Implements IGroups.DeleteGroup
        Try
            OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, _
                "ZBP_DEL_GROUP", New OracleParameter() { _
            New OracleParameter("@P_GROUP_SEQ_ID", GroupSeqId), _
            New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BusinessUnitSeqId), _
            New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
            })
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class