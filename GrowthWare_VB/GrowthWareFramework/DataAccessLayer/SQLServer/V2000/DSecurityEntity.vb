Imports System.Data.SqlClient
Imports GrowthWare.Framework.DataAccessLayer.SqlServer.Base
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.SqlServer.V2000
	''' <summary>
	''' Provides data access to SQL Server 2000
	''' </summary>
	Public Class DSecurityEntity
		Inherits DDBInteraction
		Implements IDSecurityEntity

		Function GetSecurityEntities() As DataTable Implements IDSecurityEntity.GetSecurityEntities
			Dim myParameters() As SqlParameter =
			{
			  New SqlParameter("@P_SE_SEQ_ID", -1),
			  MyBase.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.InputOutput)
			 }
			Return MyBase.GetDataTable("ZFP_GET_Security_Entity", myParameters)
		End Function

		Function GetSecurityEntities(ByVal account As String, ByVal securityEntityID As Integer, ByVal isSecurityEntityAdministrator As Boolean) As DataTable Implements IDSecurityEntity.GetSecurityEntities
			If String.IsNullOrEmpty(account) Then Throw New NotImplementedException("ACCT not given")
			If securityEntityID = -1 Then Throw New NotImplementedException("SE_SEQ_ID not given")

			Dim myStoreProcedure As String = "ZFP_GET_VALID_SES"
			Dim myParameters() As SqlParameter =
			{
			New SqlParameter("@P_ACCT", account),
			New SqlParameter("@P_IS_SE_ADMIN", isSecurityEntityAdministrator),
			New SqlParameter("@P_SE_SEQ_ID", securityEntityID),
			MyBase.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
			}
			Return MyBase.GetDataTable(myStoreProcedure, myParameters)
		End Function

		Function Save(ByRef profile As MSecurityEntityProfile) As Integer Implements IDSecurityEntity.Save
			Dim mParameters() As SqlParameter =
			 {
			   New SqlParameter("@P_SE_SEQ_ID", profile.Id),
			   New SqlParameter("@P_NAME", profile.Name),
			   New SqlParameter("@P_DESCRIPTION", profile.Description),
			   New SqlParameter("@P_URL", profile.Url),
			   New SqlParameter("@P_STATUS_SEQ_ID", profile.StatusSeqId),
			   New SqlParameter("@P_DAL", profile.DAL),
			   New SqlParameter("@P_DAL_NAME", profile.DAL),
			   New SqlParameter("@P_DAL_NAME_SPACE", profile.DALNamespace),
			   New SqlParameter("@P_DAL_STRING", profile.ConnectionString),
			   New SqlParameter("@P_SKIN", profile.Skin),
			   New SqlParameter("@P_STYLE", profile.Style),
			   New SqlParameter("@P_PARENT_SE_SEQ_ID", profile.ParentSeqId),
			   New SqlParameter("@P_ENCRYPTION_TYPE", profile.EncryptionType),
			   New SqlParameter("@P_ADDED_UPDATED_BY", profile.AddedBy),
			   New SqlParameter("@P_ADDED_UPDATED_DATE", Date.Now),
			   MyBase.GetSqlParameter("@P_PRIMARY_KEY", Nothing, ParameterDirection.Output),
			   MyBase.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
			 }
			MyBase.ExecuteNonQuery("ZFP_SET_SECURITY_ENTITIES", mParameters)
			profile.Id = Integer.Parse(MyBase.GetParameterValue("@P_PRIMARY_KEY", mParameters).ToString())
			Return profile.Id
		End Function
	End Class
End Namespace
