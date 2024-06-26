﻿Imports GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports System.Data.SqlClient
Imports GrowthWare.Framework.Model.Profiles
Imports System.Globalization

Namespace DataAccessLayer.SQLServer.V2008
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

        Public Property AccountId As Integer Implements IDNameValuePair.AccountId

        Public Sub DeleteNameValuePairDetail(ByVal profile As MNameValuePairDetail) Implements IDNameValuePair.DeleteNameValuePairDetail
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mStoreProcedure As String = "ZGWSystem.Delete_Name_Value_Pair_Detail"
            Dim myParameters() As SqlParameter =
             {
              New SqlParameter("@P_NVP_Detail_SeqID", profile.Id),
              New SqlParameter("@P_NVP_SeqID", profile.NameValuePairSeqId)
             }
            MyBase.ExecuteNonQuery(mStoreProcedure, myParameters)
        End Sub

        Public Property DetailProfile As MNameValuePairDetail Implements IDNameValuePair.DetailProfile

        Public Function AllNameValuePairs() As DataTable Implements IDNameValuePair.AllNameValuePairs
            Return MyBase.GetDataTable("ZGWSystem.Get_Name_Value_Pair", GetSelectParameters)
        End Function

        Public Function GetAllNameValuePairDetail() As DataTable Implements IDNameValuePair.AllNameValuePairDetail
            Dim mStoreProcedure As String = "ZGWSystem.Get_Name_Value_Pair_Details"
            Dim mParameters() As SqlParameter = {New SqlParameter("@P_NVP_SeqID", -1)}
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetAllNameValuePairDetail(ByVal nameValuePairSeqId As Integer) As DataTable Implements IDNameValuePair.GetAllNameValuePairDetail
            Dim mStoreProcedure As String = "ZGWSystem.Get_Name_Value_Pair_Details"
            Dim mParameters() As SqlParameter = {New SqlParameter("@P_NVP_SeqID", nameValuePairSeqId)}
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetGroups(ByVal nameValuePairSeqId As Integer) As DataTable Implements IDNameValuePair.GetGroups
            Dim mParameters() As SqlParameter = {New SqlParameter("@P_NVP_SeqID", nameValuePairSeqId), New SqlParameter("@P_Security_Entity_SeqID", SecurityEntityId)}
            Dim mStoreProcedure As String = "ZGWSecurity.Get_Name_Value_Pair_Groups"
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetNameValuePair() As DataRow Implements IDNameValuePair.NameValuePair
            Dim storeProc As String = "ZGWSystem.Get_Name_Value_Pair"
            Dim mParameters() As SqlParameter = GetSelectParameters()
            Return MyBase.GetDataRow(storeProc, mParameters)
        End Function

        Public Function GetNameValuePairDetail() As DataRow Implements IDNameValuePair.NameValuePairDetail
            Dim mStoreProcedure As String = "ZGWSystem.Get_Name_Value_Pair_Detail"
            Dim mParameters() As SqlParameter =
             {
               New SqlParameter("@P_NVP_Detail_SeqID", DetailProfile.Id),
               New SqlParameter("@P_NVP_SeqID", DetailProfile.NameValuePairSeqId)
             }
            Return MyBase.GetDataRow(mStoreProcedure, mParameters)
        End Function

        Public Function GetNameValuePairDetails(ByVal nameValuePairSeqDetailId As Integer, ByVal nameValuePairSeqId As Integer) As DataRow Implements IDNameValuePair.NameValuePairDetails
            Dim mStoreProcedure As String = "ZGWSystem.Get_Name_Value_Pair_Details"
            Dim mParameters() As SqlParameter = {New SqlParameter("@P_NVP_SeqID", nameValuePairSeqId)}
            Return MyBase.GetDataRow(mStoreProcedure, mParameters)
        End Function

        Public Function GetRoles(ByVal nameValuePairSeqId As Integer) As DataTable Implements IDNameValuePair.GetRoles
            Dim mParameters() As SqlParameter = {New SqlParameter("@P_NVP_SeqID", nameValuePairSeqId), New SqlParameter("@P_Security_Entity_SeqID", SecurityEntityId)}
            Dim mStoreProcedure As String = "ZGWSecurity.Get_Name_Value_Pair_Roles"
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Property NameValuePairProfile As MNameValuePair Implements IDNameValuePair.NameValuePairProfile

        Public Function Save() As Integer Implements IDNameValuePair.Save
            Dim mStoreProcedure As String = "ZGWSystem.Set_Name_Value_Pair"
            Dim mParameters() As SqlParameter = GetInsertUpdateParameters()
            Dim mRetVal As Integer = -1
            MyBase.ExecuteNonQuery(mStoreProcedure, mParameters)
            mRetVal = Integer.Parse(GetParameterValue("@P_Primary_Key", mParameters), CultureInfo.InvariantCulture)
            Return mRetVal
        End Function

        Public Sub SaveNameValuePairDetail(ByVal profile As MNameValuePairDetail) Implements IDNameValuePair.SaveNameValuePairDetail
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mStoreProcedure As String = "ZGWSystem.Set_Name_Value_Pair_Detail"
            Dim mParameters() As SqlParameter =
             {
             New SqlParameter("@P_NVP_Detail_SeqID", profile.Id),
             New SqlParameter("@P_NVP_SeqID", profile.NameValuePairSeqId),
             New SqlParameter("@P_NVP_Detail_Name", profile.Value),
             New SqlParameter("@P_NVP_Detail_Value", profile.Text),
             New SqlParameter("@P_Status_SeqID", profile.Status),
             New SqlParameter("@P_Sort_Order", profile.SortOrder),
             New SqlParameter("@P_Added_Updated_BY", GetAddedUpdatedBy(profile)),
             GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output),
             GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output)
            }
            MyBase.ExecuteNonQuery(mStoreProcedure, mParameters)
            'Return Integer.Parse(MyBase.GetParameterValue("@P_PRIMARY_KEY", mParameters))
        End Sub

        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable Implements IDNameValuePair.Search
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As DataTable
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_Columns", searchCriteria.Columns),
              New SqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              New SqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              New SqlParameter("@P_PageSize", searchCriteria.PageSize),
              New SqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              New SqlParameter("@P_TableOrView", "ZGWSystem.vwSearchNVP"),
              New SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             }
            mRetVal = MyBase.GetDataTable("ZGWSystem.Get_Paginated_Data", mParameters)
            Return mRetVal
        End Function

        Public Property SecurityEntityId As Integer Implements IDNameValuePair.SecurityEntityId

        Public Sub UpdateGroups(ByVal nameValuePairSeqId As Integer, ByVal securityEntityId As Integer, ByVal commaSeparatedGroups As String, ByVal profile As MNameValuePair) Implements IDNameValuePair.UpdateGroups
            Dim myStoreProcedure As String = "ZGWSecurity.Set_Name_Value_Pair_Groups"
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_NVP_SeqID", nameValuePairSeqId),
              New SqlParameter("@P_Security_Entity_SeqID", securityEntityId),
              New SqlParameter("@P_Groups", commaSeparatedGroups),
              New SqlParameter("@P_Permissions_NVP_Detail_SeqID", m_PermissionSeqId),
              New SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(profile))
             }
            MyBase.ExecuteNonQuery(myStoreProcedure, mParameters)
        End Sub

        Public Sub UpdateRoles(ByVal nameValuePairSeqId As Integer, ByVal securityEntityId As Integer, ByVal commaSeparatedRoles As String, ByVal profile As MNameValuePair) Implements IDNameValuePair.UpdateRoles
            Dim myStoreProcedure As String = "ZGWSecurity.Set_Name_Value_Pair_Roles"
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_NVP_SeqID", nameValuePairSeqId),
              New SqlParameter("@P_Security_Entity_SeqID", securityEntityId),
              New SqlParameter("@P_Role", commaSeparatedRoles),
              New SqlParameter("@P_Permissions_NVP_Detail_SeqID", m_PermissionSeqId),
              New SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(profile))
             }
            MyBase.ExecuteNonQuery(myStoreProcedure, mParameters)
        End Sub

        Private Function GetInsertUpdateParameters() As SqlParameter()
            Dim mParameters() As SqlParameter =
            {
             New SqlParameter("@P_NVP_SeqID", NameValuePairProfile.Id),
             New SqlParameter("@P_Schema_Name", NameValuePairProfile.SchemaName),
             New SqlParameter("@P_Static_Name", NameValuePairProfile.StaticName),
             New SqlParameter("@P_Display", NameValuePairProfile.Display),
             New SqlParameter("@P_Description", NameValuePairProfile.Description),
             New SqlParameter("@P_Status_SeqID", NameValuePairProfile.Status),
             New SqlParameter("@P_Added_Updated_BY", GetAddedUpdatedBy(NameValuePairProfile)),
             GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output),
             GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output)
            }
            Return mParameters
        End Function

        Private Function GetSelectParameters() As SqlParameter()
            If NameValuePairProfile Is Nothing Then NameValuePairProfile = New MNameValuePair()
            Dim mParameters() As SqlParameter =
            {
             New SqlParameter("@P_NVP_SeqID", NameValuePairProfile.Id),
             New SqlParameter("@P_Account_SeqID", AccountId),
             New SqlParameter("@P_Security_Entity_SeqID", SecurityEntityId)
            }
            Return mParameters
        End Function
    End Class
End Namespace
