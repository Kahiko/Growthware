Imports MySql.Data.MySqlClient
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.MySql.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.MySql.V5621
    Public Class DGroups
        Inherits DDBInteraction
        Implements IDGroups

        ''' <summary>
        ''' Deletes the group.
        ''' </summary>
        Public Sub DeleteGroup() Implements IDGroups.DeleteGroup
            Dim mStoredProcedure As String = "ZGWSecurity.Delete_Group"
            Dim mParameters() As MySqlParameter =
            {
              New MySqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntityId),
              New MySqlParameter("@P_Group_SeqID", Profile.Id)
            }
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Public Function GetGroupRoles() As DataTable Implements IDGroups.GetGroupRoles
            If GroupRolesProfile.GroupSeqId = -1 Then
                Throw New ArgumentException("The GroupRoles Profile must be set.")
            End If
            Dim mStoreProcedure As String = "ZGWSecurity.Get_Group_Roles"
            Dim mParameters() As MySqlParameter =
              {
             New MySqlParameter("@P_Security_Entity_SeqID", GroupRolesProfile.SecurityEntityId),
             New MySqlParameter("@P_Group_SeqID", GroupRolesProfile.GroupSeqId)
              }
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetGroupsBySecurityEntity() As DataTable Implements IDGroups.GetGroupsBySecurityEntity
            Dim mStoredProcedure As String = "ZGWSecurity.Get_Group"
            Dim mParameters() As MySqlParameter =
            {
               New MySqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntityId),
               New MySqlParameter("@P_Group_SeqID", -1)
            }
            Return MyBase.GetDataTable(mStoredProcedure, mParameters)
        End Function

        Public Function GetProfileData() As System.Data.DataRow Implements IDGroups.GetProfileData
            Dim mStoredProcedure As String = "ZGWSecurity.Get_Group"
            Dim myParameters() As MySqlParameter =
            {
              New MySqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntityId),
              New MySqlParameter("@P_Group_SeqID", Profile.Id)
            }
            Return MyBase.GetDataRow(mStoredProcedure, myParameters)
        End Function

        Public Property GroupRolesProfile As MGroupRoles Implements IDGroups.GroupRolesProfile

        Public Property Profile As MGroupProfile Implements IDGroups.Profile

        Public Sub Save() Implements IDGroups.Save
            Dim mParameters() As MySqlParameter = GetInsertUpdateParameters()
            Dim mStoredProcedure As String = "ZGWSecurity.Set_Group"
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Public Sub UpdateGroupRoles() Implements IDGroups.UpdateGroupRoles
            If GroupRolesProfile.GroupSeqId = -1 Then
                Throw New ArgumentException("The GroupRoles Profile must be set.")
            End If
            Dim mStoredProcedure As String = "ZGWSecurity.Set_Group_Roles"
            Dim mParameters() As MySqlParameter =
            {
             New MySqlParameter("@P_Group_SeqID", GroupRolesProfile.GroupSeqId),
             New MySqlParameter("@P_Security_Entity_SeqID", GroupRolesProfile.SecurityEntityId),
             New MySqlParameter("@P_Roles", GroupRolesProfile.Roles),
             New MySqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(GroupRolesProfile))
            }
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Private Function GetInsertUpdateParameters() As MySqlParameter()
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_Group_SeqID", Profile.Id),
              New MySqlParameter("@P_Name", Profile.Name),
              New MySqlParameter("@P_Description", Profile.Description),
              New MySqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntityId),
              New MySqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(Profile)),
              GetSqlParameter("@P_PRIMARY_KEY", Profile.Id, ParameterDirection.Output)
             }
            Return mParameters
        End Function

        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable Implements IDGroups.Search
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
              New MySqlParameter("@P_TableOrView", "ZGWSecurity.vwSearchGroups"),
              New MySqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             }
            mRetVal = MyBase.GetDataTable(mStoredProcedure, mParameters)
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets or sets the security entity seq ID.
        ''' </summary>
        ''' <value>The security entity seq ID.</value>
        Public Property SecurityEntitySeqId As Integer Implements IDGroups.SecurityEntitySeqId

    End Class
End Namespace

