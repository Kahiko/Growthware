Imports System.Data.SqlClient
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.SQLServer.V2008
    Public Class DGroups
        Inherits DDBInteraction
        Implements IDGroups

        ''' <summary>
        ''' Deletes the group.
        ''' </summary>
        Public Sub DeleteGroup() Implements IDGroups.DeleteGroup
            Dim mStoredProcedure As String = "ZGWSecurity.Delete_Group"
            Dim mParameters() As SqlParameter =
            {
              New SqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntityID),
              New SqlParameter("@P_Group_SeqID", Profile.Id)
            }
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Public Function GetGroupRoles() As DataTable Implements IDGroups.GetGroupRoles
            If GroupRolesProfile.GROUP_SEQ_ID = -1 Then
                Throw New ApplicationException("The GroupRoles Profile must be set.")
            End If
            Dim mStoreProcedure As String = "ZGWSecurity.Get_Group_Roles"
            Dim mParameters() As SqlParameter =
              {
             New SqlParameter("@P_Security_Entity_SeqID", GroupRolesProfile.SE_SEQ_ID),
             New SqlParameter("@P_Group_SeqID", GroupRolesProfile.GROUP_SEQ_ID)
              }
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetGroupsBySecurityEntity() As DataTable Implements IDGroups.GetGroupsBySecurityEntity
            Dim mStoredProcedure As String = "ZGWSecurity.Get_Group"
            Dim mParameters() As SqlParameter =
            {
               New SqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntityID),
               New SqlParameter("@P_Group_SeqID", -1)
            }
            Return MyBase.GetDataTable(mStoredProcedure, mParameters)
        End Function

        Public Function GetProfileData() As System.Data.DataRow Implements IDGroups.GetProfileData
            Dim mStoredProcedure As String = "ZGWSecurity.Get_Group"
            Dim myParameters() As SqlParameter =
            {
              New SqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntityID),
              New SqlParameter("@P_Group_SeqID", Profile.Id)
            }
            Return MyBase.GetDataRow(mStoredProcedure, myParameters)
        End Function

        Public Property GroupRolesProfile As MGroupRoles Implements IDGroups.GroupRolesProfile

        Public Property Profile As MGroupProfile Implements IDGroups.Profile

        Public Sub Save() Implements IDGroups.Save
            Dim mParameters() As SqlParameter = GetInsertUpdateParameters()
            Dim mStoredProcedure As String = "ZGWSecurity.Set_Group"
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Public Sub UpdateGroupRoles() Implements IDGroups.UpdateGroupRoles
            If GroupRolesProfile.GROUP_SEQ_ID = -1 Then
                Throw New ApplicationException("The GroupRoles Profile must be set.")
            End If
            Dim mStoredProcedure As String = "ZGWSecurity.Set_Group_Roles"
            Dim mParameters() As SqlParameter =
            {
             New SqlParameter("@P_Group_SeqID", GroupRolesProfile.GROUP_SEQ_ID),
             New SqlParameter("@P_Security_Entity_SeqID", GroupRolesProfile.SE_SEQ_ID),
             New SqlParameter("@P_Roles", GroupRolesProfile.Roles),
             New SqlParameter("@P_Added_Updated_By", GroupRolesProfile.ADD_UP_BY)
            }
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Private Function GetInsertUpdateParameters() As SqlParameter()
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_Group_SeqID", Profile.Id),
              New SqlParameter("@P_Name", Profile.Name),
              New SqlParameter("@P_Description", Profile.Description),
              New SqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntityId),
              New SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(Profile)),
              GetSqlParameter("@P_PRIMARY_KEY", Profile.Id, ParameterDirection.Output)
             }
            Return mParameters
        End Function

        Public Function Search(ByRef searchCriteria As MSearchCriteria) As DataTable Implements IDGroups.Search
            Dim mStoredProcedure As String = "ZGWSystem.Get_Paginated_Data"
            Dim mRetVal As DataTable = Nothing
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_Columns", searchCriteria.Columns),
              New SqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              New SqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              New SqlParameter("@P_PageSize", searchCriteria.PageSize),
              New SqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              New SqlParameter("@P_TableOrView", "ZGWSecurity.vwSearchGroups"),
              New SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
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

