Imports GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports System.Data.SqlClient
Imports GrowthWare.Framework.Model.Profiles
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Namespace DataAccessLayer.SQLServer.V2008
    Public Class DRoles
        Inherits DDBInteraction
        Implements IDRoles

        Public Sub DeleteRole() Implements IDRoles.DeleteRole
            Dim mStoredProcedure As String = "ZGWSecurity.Delete_Role"
            Dim mParameters() As SqlParameter =
            {
             New SqlParameter("@P_Name", Profile.Name),
             New SqlParameter("@P_Security_Entity_SeqID", SecurityEntitySeqId)
            }
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Public Function GetAccountsInRole() As DataTable Implements IDRoles.GetAccountsInRole
            Dim mStoreProcedure As String = "ZGWSecurity.Get_Accounts_In_Role"
            Dim mParameters() As SqlParameter =
            {
              New SqlParameter("@P_Role_SeqID", Profile.Id),
              New SqlParameter("@P_Security_Entity_SeqID", SecurityEntitySeqId)
            }
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetAccountsNotInRole() As DataTable Implements IDRoles.GetAccountsNotInRole
            Dim mStoreProcedure As String = "ZGWSecurity.Get_Accounts_Not_In_Role"
            Dim mParameters() As SqlParameter =
            {
             New SqlParameter("@P_Security_Entity_SeqID", SecurityEntitySeqId),
             New SqlParameter("@P_Role_SeqID", Profile.Id)
            }
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetProfileData() As DataRow Implements IDRoles.GetProfileData
            Dim mStoredProcedure As String = "ZGWSecurity.Get_Role"
            Dim mParameters() As SqlParameter =
            {
              New SqlParameter("@P_Role_SeqID", Profile.Id),
              New SqlParameter("@P_Security_Entity_SeqID", -1)
            }
            Return MyBase.GetDataRow(mStoredProcedure, mParameters)
        End Function

        Public Function GetRolesBySecurityEntity() As DataTable Implements IDRoles.GetRolesBySecurityEntity
            Dim mStoredProcedure As String = "ZGWSecurity.Get_Role"
            Dim mParameters() As SqlParameter =
            {
              New SqlParameter("@P_Role_SeqID", -1),
              New SqlParameter("@P_Security_Entity_SeqID", SecurityEntitySeqId)
            }
            Return MyBase.GetDataTable(mStoredProcedure, mParameters)
        End Function

        Public Property Profile As MRoleProfile Implements IDRoles.Profile

        Public Sub Save() Implements IDRoles.Save
            Dim mStoredProcedure As String = "ZGWSecurity.Set_Role"
            Dim mParameters() As SqlParameter = getInsertUpdateParameters()
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable Implements IDRoles.Search
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!")
            Dim mStoredProcedure As String = "ZGWSystem.Get_Paginated_Data"
            Dim mRetVal As DataTable = Nothing
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_Columns", searchCriteria.Columns),
              New SqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              New SqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              New SqlParameter("@P_PageSize", searchCriteria.PageSize),
              New SqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              New SqlParameter("@P_TableOrView", "ZGWSecurity.vwSearchRoles"),
              New SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             }
            mRetVal = MyBase.GetDataTable(mStoredProcedure, mParameters)
            Return mRetVal
        End Function

        Public Property SecurityEntitySeqId As Integer Implements IDRoles.SecurityEntitySeqId

        Public Function UpdateAllAccountsForRole(roleSeqId As Integer, securityEntityId As Integer, accounts() As String, accountSeqId As Integer) As Boolean Implements IDRoles.UpdateAllAccountsForRole
            If accounts Is Nothing Then Throw New ArgumentNullException("accounts", "accounts cannot be a null reference (Nothing in Visual Basic)!")
            Dim success As Boolean = False
            Dim dbConn As SqlConnection = Nothing
            Dim trans As SqlTransaction = Nothing
            Dim account As String
            Try
                dbConn = New SqlConnection(ConnectionString)
                dbConn.Open()
                trans = dbConn.BeginTransaction(IsolationLevel.Serializable)
                Dim db As SqlDatabase = New SqlDatabase(MyBase.ConnectionString)
                ' delete all the accounts for this role/SecurityEntity
                Dim dbCommand As System.Data.Common.DbCommand = db.GetStoredProcCommand("ZGWSecurity.Delete_Roles_Accounts")
                Dim myParameter As SqlParameter = New SqlParameter("@P_ROLE_SEQ_ID", roleSeqId)
                dbCommand.Parameters.Add(myParameter)
                myParameter = New SqlParameter("@P_Security_Entity_SeqID", securityEntityId)
                dbCommand.Parameters.Add(myParameter)
                db.ExecuteNonQuery(dbCommand, trans)

                For Each account In accounts
                    dbCommand.Parameters.Clear()
                    dbCommand = db.GetStoredProcCommand("ZGWSecurity.Set_Role_Accounts")
                    myParameter = New SqlParameter("@P_Role_SeqID", roleSeqId)
                    dbCommand.Parameters.Add(myParameter)
                    myParameter = New SqlParameter("@P_Security_Entity_SeqID", securityEntityId)
                    dbCommand.Parameters.Add(myParameter)
                    myParameter = New SqlParameter("@P_Account", account)
                    dbCommand.Parameters.Add(myParameter)
                    myParameter = New SqlParameter("@P_Added_Updated_By", accountSeqId)
                    dbCommand.Parameters.Add(myParameter)
                    db.ExecuteNonQuery(dbCommand, trans)
                Next
                success = True
                trans.Commit()
            Catch ex As Exception
                If Not trans Is Nothing Then trans.Rollback()
                Throw
            Finally
                If Not trans Is Nothing Then
                    trans.Dispose()
                    trans = Nothing
                End If
                If Not dbConn Is Nothing Then
                    dbConn.Dispose()
                    dbConn = Nothing
                End If
            End Try
            Return success

        End Function

        Private Function getInsertUpdateParameters() As SqlParameter()
            Dim myParameters() As SqlParameter =
             { _
             New SqlParameter("@P_Role_SeqID", Profile.Id),
             New SqlParameter("@P_Name", Profile.Name),
             New SqlParameter("@P_Description", Profile.Description),
             New SqlParameter("@P_Is_System", Profile.IsSystem),
             New SqlParameter("@P_Is_System_Only", Profile.IsSystemOnly),
             New SqlParameter("@P_Security_Entity_SeqID", SecurityEntitySeqId),
             New SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(Profile)),
             GetSqlParameter("@P_Primary_Key", Profile.Id, ParameterDirection.Output)
             }
            Return myParameters
        End Function

    End Class
End Namespace
