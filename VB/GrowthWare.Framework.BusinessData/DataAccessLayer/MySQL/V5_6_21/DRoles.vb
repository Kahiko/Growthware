Imports GrowthWare.Framework.BusinessData.DataAccessLayer.MySql.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports MySql.Data.MySqlClient
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.MySql.V5621
    Public Class DRoles
        Inherits DDBInteraction
        Implements IDRoles

        Public Sub DeleteRole() Implements IDRoles.DeleteRole
            Dim mStoredProcedure As String = "ZGWSecurity.Delete_Role"
            Dim mParameters() As MySqlParameter =
            {
             New MySqlParameter("@P_Name", Profile.Name),
             New MySqlParameter("@P_Security_Entity_SeqID", SecurityEntitySeqId)
            }
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Public Function GetAccountsInRole() As DataTable Implements IDRoles.AccountsInRole
            Dim mStoreProcedure As String = "ZGWSecurity.Get_Accounts_In_Role"
            Dim mParameters() As MySqlParameter =
            {
              New MySqlParameter("@P_Role_SeqID", Profile.Id),
              New MySqlParameter("@P_Security_Entity_SeqID", SecurityEntitySeqId)
            }
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetAccountsNotInRole() As DataTable Implements IDRoles.AccountsNotInRole
            Dim mStoreProcedure As String = "ZGWSecurity.Get_Accounts_Not_In_Role"
            Dim mParameters() As MySqlParameter =
            {
             New MySqlParameter("@P_Security_Entity_SeqID", SecurityEntitySeqId),
             New MySqlParameter("@P_Role_SeqID", Profile.Id)
            }
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetProfileData() As DataRow Implements IDRoles.ProfileData
            Dim mStoredProcedure As String = "ZGWSecurity.Get_Role"
            Dim mParameters() As MySqlParameter =
            {
              New MySqlParameter("@P_Role_SeqID", Profile.Id),
              New MySqlParameter("@P_Security_Entity_SeqID", -1)
            }
            Return MyBase.GetDataRow(mStoredProcedure, mParameters)
        End Function

        Public Function GetRolesBySecurityEntity() As DataTable Implements IDRoles.RolesBySecurityEntity
            Dim mStoredProcedure As String = "ZGWSecurity.Get_Role"
            Dim mParameters() As MySqlParameter =
            {
              New MySqlParameter("@P_Role_SeqID", -1),
              New MySqlParameter("@P_Security_Entity_SeqID", SecurityEntitySeqId)
            }
            Return MyBase.GetDataTable(mStoredProcedure, mParameters)
        End Function

        Public Property Profile As MRoleProfile Implements IDRoles.Profile

        Public Function Save() As Integer Implements IDRoles.Save
            Dim mRetVal As Integer
            Dim mStoredProcedure As String = "ZGWSecurity.Set_Role"
            Dim mParameters() As MySqlParameter = getInsertUpdateParameters()
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
            mRetVal = Integer.Parse(MyBase.GetParameterValue("@P_Primary_Key", mParameters))
            Return mRetVal
        End Function

        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable Implements IDRoles.Search
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!")
            Dim mStoredProcedure As String = "ZGWSystem.Get_Paginated_Data"
            Dim mRetVal As DataTable = Nothing
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_Columns", searchCriteria.Columns),
              New MySqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              New MySqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              New MySqlParameter("@P_PageSize", searchCriteria.PageSize),
              New MySqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              New MySqlParameter("@P_TableOrView", "ZGWSecurity.vwSearchRoles"),
              New MySqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             }
            mRetVal = MyBase.GetDataTable(mStoredProcedure, mParameters)
            Return mRetVal
        End Function

        Public Property SecurityEntitySeqId As Integer Implements IDRoles.SecurityEntitySeqId

        Public Function UpdateAllAccountsForRole(roleSeqId As Integer, securityEntityId As Integer, accounts() As String, accountSeqId As Integer) As Boolean Implements IDRoles.UpdateAllAccountsForRole
            If accounts Is Nothing Then Throw New ArgumentNullException("accounts", "accounts cannot be a null reference (Nothing in Visual Basic)!")
            Dim success As Boolean = False
            Dim db As MySqlConnection = Nothing
            Dim dbConn As MySqlCommand = Nothing
            Dim trans As MySqlTransaction = Nothing
            Dim account As String
            Try
                db = New MySqlConnection(ConnectionString)
                trans = db.BeginTransaction(IsolationLevel.Serializable)
                ' delete all the accounts for this role/SecurityEntity
                dbConn = New MySqlCommand("ZGWSecurity.Delete_Roles_Accounts", db)
                Dim myParameter As MySqlParameter = New MySqlParameter("@P_ROLE_SEQ_ID", roleSeqId)
                dbConn.Parameters.Add(myParameter)
                myParameter = New MySqlParameter("@P_Security_Entity_SeqID", securityEntityId)
                dbConn.Parameters.Add(myParameter)
                dbConn.ExecuteNonQuery()

                For Each account In accounts
                    dbConn.Parameters.Clear()
                    dbConn.CommandText = "ZGWSecurity.Set_Role_Accounts"
                    myParameter = New MySqlParameter("@P_Role_SeqID", roleSeqId)
                    dbConn.Parameters.Add(myParameter)
                    myParameter = New MySqlParameter("@P_Security_Entity_SeqID", securityEntityId)
                    dbConn.Parameters.Add(myParameter)
                    myParameter = New MySqlParameter("@P_Account", account)
                    dbConn.Parameters.Add(myParameter)
                    myParameter = New MySqlParameter("@P_Added_Updated_By", accountSeqId)
                    dbConn.Parameters.Add(myParameter)
                    dbConn.ExecuteNonQuery()
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
                If Not (db Is Nothing) Then
                    db.Close()
                End If
                If Not dbConn Is Nothing Then
                    dbConn.Dispose()
                    dbConn = Nothing
                End If
            End Try
            Return success

        End Function

        Private Function getInsertUpdateParameters() As MySqlParameter()
            Dim myParameters() As MySqlParameter =
             { _
             New MySqlParameter("@P_Role_SeqID", Profile.Id),
             New MySqlParameter("@P_Name", Profile.Name),
             New MySqlParameter("@P_Description", Profile.Description),
             New MySqlParameter("@P_Is_System", Profile.IsSystem),
             New MySqlParameter("@P_Is_System_Only", Profile.IsSystemOnly),
             New MySqlParameter("@P_Security_Entity_SeqID", SecurityEntitySeqId),
             New MySqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(Profile)),
             GetSqlParameter("@P_Primary_Key", Profile.Id, ParameterDirection.Output)
             }
            Return myParameters
        End Function

    End Class
End Namespace
