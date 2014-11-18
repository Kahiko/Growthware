Imports GrowthWare.Framework.BusinessData.DataAccessLayer.MySql.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports MySql.Data.MySqlClient
Imports GrowthWare.Framework.Model.Profiles
Imports System.Globalization

Namespace DataAccessLayer.MySql.V5_6_21
    ''' <summary>
    ''' DAccounts provides all database interaction to SQL Server 2008
    ''' </summary>
    ''' <remarks>
    '''		The Profile and SecurityEntitySeqID properties must be set
    '''		before using any methods.
    '''		Properties where chosen instead of parameters because all
    '''		methods will need one or both to perform their work.
    '''	</remarks>
    Public Class DNameValuePair
        Inherits DDBInteraction
        Implements IDNameValuePair

        Private m_PermissionSeqId As Integer = 1

        Public Property AccountID As Integer Implements IDNameValuePair.AccountId

        Public Sub DeleteNVPDetail(ByVal Profile As MNameValuePairDetail) Implements IDNameValuePair.DeleteNameValuePairDetail
            Dim mStoreProcedure As String = "ZGWSystem.Del_Name_Value_Pair_Detail"
            Dim myParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_NVP_Detail_SeqID", Profile.Id),
              New MySqlParameter("@P_NVP_SeqID", Profile.NameValuePairSeqId)
             }
            MyBase.ExecuteNonQuery(mStoreProcedure, myParameters)
        End Sub

        Public Property DetailProfile As MNameValuePairDetail Implements IDNameValuePair.DetailProfile

        Public Function AllNameValuePairs() As DataTable Implements IDNameValuePair.AllNameValuePairs
            Return MyBase.GetDataTable("ZGWSystem.Get_Name_Value_Pair", GetSelectParameters)
        End Function

        Public Function GetAllNVPDetail() As DataTable Implements IDNameValuePair.GetAllNameValuePairDetail
            Dim mStoreProcedure As String = "ZGWSystem.Get_Name_Value_Pair_Details"
            Dim mParameters() As MySqlParameter = {New MySqlParameter("@P_NVP_SeqID", -1)}
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetAllNVPDetail(ByVal NVPSeqID As Integer) As DataTable Implements IDNameValuePair.GetAllNameValuePairDetail
            Dim mStoreProcedure As String = "ZGWSystem.Get_Name_Value_Pair_Details"
            Dim mParameters() As MySqlParameter = {New MySqlParameter("@P_NVP_SeqID", -1)}
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetGroups(ByVal nameValuePairSeqID As Integer) As DataTable Implements IDNameValuePair.GetGroups
            Dim mParameters() As MySqlParameter = {New MySqlParameter("@P_NVP_SeqID", nameValuePairSeqID), New MySqlParameter("@P_Security_Entity_SeqID", SE_SEQ_ID)}
            Dim mStoreProcedure As String = "ZGWSecurity.Get_Name_Value_Pair_Groups"
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetNVP() As DataRow Implements IDNameValuePair.GetNameValuePair
            Dim storeProc As String = "ZGWSystem.Get_Name_Value_Pair"
            Dim mParameters() As MySqlParameter = GetSelectParameters()
            Return MyBase.GetDataRow(storeProc, mParameters)
        End Function

        Public Function GetNVPDetail() As DataRow Implements IDNameValuePair.GetNameValuePairDetail
            Dim mStoreProcedure As String = "ZGWSystem.Get_Name_Value_Pair_Detail"
            Dim mParameters() As MySqlParameter =
             {
               New MySqlParameter("@P_NVP_Detail_SeqID", DetailProfile.Id),
               New MySqlParameter("@P_NVP_SeqID", DetailProfile.NameValuePairSeqId)
             }
            Return MyBase.GetDataRow(mStoreProcedure, mParameters)
        End Function

        Public Function GetNVPDetails(ByVal nameValuePairSeqDetID As Integer, ByVal nameValuePairSeqId As Integer) As DataRow Implements IDNameValuePair.GetNameValuePairDetails
            Dim mStoreProcedure As String = "ZGWSystem.Get_Name_Value_Pair_Details"
            Dim mParameters() As MySqlParameter = {New MySqlParameter("@P_NVP_SeqID", nameValuePairSeqId)}
            Return MyBase.GetDataRow(mStoreProcedure, mParameters)
        End Function

        Public Function GetRoles(ByVal nameValuePairSeqID As Integer) As DataTable Implements IDNameValuePair.GetRoles
            Dim mParameters() As MySqlParameter = {New MySqlParameter("@P_NVP_SeqID", nameValuePairSeqID), New MySqlParameter("@P_Security_Entity_SeqID", SE_SEQ_ID)}
            Dim mStoreProcedure As String = "ZGWSecurity.Get_Name_Value_Pair_Roles"
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Property NameValuePairProfile As MNameValuePair Implements IDNameValuePair.NameValuePairProfile

        Public Function Save() As Integer Implements IDNameValuePair.Save
            Dim mStoreProcedure As String = "ZGWSystem.Set_Name_Value_Pair"
            Dim mParameters() As MySqlParameter = GetInsertUpdateParameters()
            Dim mRetVal As Integer = -1
            MyBase.ExecuteNonQuery(mStoreProcedure, mParameters)
            mRetVal = Integer.Parse(GetParameterValue("@P_Primary_Key", mParameters), CultureInfo.InvariantCulture)
            Return mRetVal
        End Function

        Public Sub SaveNVPDetail(ByVal profile As MNameValuePairDetail) Implements IDNameValuePair.SaveNameValuePairDetail
            Dim mStoreProcedure As String = "ZGWSystem.Set_Name_Value_Pair_Detail"
            Dim mParameters() As MySqlParameter =
             {
             New MySqlParameter("@P_NVP_Detail_SeqID", profile.Id),
             New MySqlParameter("@P_NVP_SeqID", profile.NameValuePairSeqId),
             New MySqlParameter("@P_NVP_Detail_Name", profile.Value),
             New MySqlParameter("@P_NVP_Detail_Value", profile.Text),
             New MySqlParameter("@P_Status_SeqID", profile.Status),
             New MySqlParameter("@P_Sort_Order", profile.SortOrder),
             New MySqlParameter("@P_Added_Updated_BY", GetAddedUpdatedBy(profile)),
             GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output),
             GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output)
            }
            MyBase.ExecuteNonQuery(mStoreProcedure, mParameters)
            'Return Integer.Parse(MyBase.GetParameterValue("@P_PRIMARY_KEY", mParameters))
        End Sub

        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable Implements IDNameValuePair.Search
            Dim mRetVal As DataTable
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_Columns", searchCriteria.Columns),
              New MySqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              New MySqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              New MySqlParameter("@P_PageSize", searchCriteria.PageSize),
              New MySqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              New MySqlParameter("@P_TableOrView", "ZGWSystem.vwSearchNVP"),
              New MySqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             }
            mRetVal = MyBase.GetDataTable("ZGWSystem.Get_Paginated_Data", mParameters)
            Return mRetVal
        End Function

        Public Property SE_SEQ_ID As Integer Implements IDNameValuePair.SecurityEntityId

        Public Sub UpdateGroups(ByVal NVP_ID As Integer, ByVal securityEntityID As Integer, ByVal commaSeperatedGroups As String, ByVal profile As MNameValuePair) Implements IDNameValuePair.UpdateGroups
            Dim myStoreProcedure As String = "ZGWSecurity.Set_Name_Value_Pair_Groups"
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_NVP_SeqID", NVP_ID),
              New MySqlParameter("@P_Security_Entity_SeqID", securityEntityID),
              New MySqlParameter("@P_Groups", commaSeperatedGroups),
              New MySqlParameter("@P_Permissions_NVP_Detail_SeqID", m_PermissionSeqId),
              New MySqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(profile))
             }
            MyBase.ExecuteNonQuery(myStoreProcedure, mParameters)
        End Sub

        Public Sub UpdateRoles(ByVal NVP_ID As Integer, ByVal securityEntityID As Integer, ByVal commaSeperatedRoles As String, ByVal profile As MNameValuePair) Implements IDNameValuePair.UpdateRoles
            Dim myStoreProcedure As String = "ZGWSecurity.Set_Name_Value_Pair_Roles"
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_NVP_SeqID", NVP_ID),
              New MySqlParameter("@P_Security_Entity_SeqID", securityEntityID),
              New MySqlParameter("@P_Role", commaSeperatedRoles),
              New MySqlParameter("@P_Permissions_NVP_Detail_SeqID", m_PermissionSeqId),
              New MySqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(profile))
             }
            MyBase.ExecuteNonQuery(myStoreProcedure, mParameters)
        End Sub

        Private Function GetInsertUpdateParameters() As MySqlParameter()
            Dim mParameters() As MySqlParameter =
            {
             New MySqlParameter("@P_NVP_SeqID", NameValuePairProfile.Id),
             New MySqlParameter("@P_Schema_Name", NameValuePairProfile.SchemaName),
             New MySqlParameter("@P_Static_Name", NameValuePairProfile.StaticName),
             New MySqlParameter("@P_Display", NameValuePairProfile.Display),
             New MySqlParameter("@P_Description", NameValuePairProfile.Description),
             New MySqlParameter("@P_Status_SeqID", NameValuePairProfile.Status),
             New MySqlParameter("@P_Added_Updated_BY", NameValuePairProfile.AddedBy),
             GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output),
             GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output)
            }
            Return mParameters
        End Function

        Private Function GetSelectParameters() As MySqlParameter()
            If NameValuePairProfile Is Nothing Then NameValuePairProfile = New MNameValuePair()
            Dim mParameters() As MySqlParameter =
            {
             New MySqlParameter("@P_NVP_SeqID", NameValuePairProfile.Id),
             New MySqlParameter("@P_Account_SeqID", AccountID),
             New MySqlParameter("@P_Security_Entity_SeqID", SE_SEQ_ID)
            }
            Return mParameters
        End Function
    End Class
End Namespace
