Imports GrowthWare.Framework.BusinessData.DataAccessLayer.MySql.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports MySql.Data.MySqlClient
Imports GrowthWare.Framework.Model.Profiles
Imports System.Globalization

Namespace DataAccessLayer.MySql.V5621
    ''' <summary>
    ''' Provides data access to SQL Server 2008
    ''' </summary>
    Public Class DSecurityEntity
        Inherits DDBInteraction
        Implements IDSecurityEntity

        Function GetSecurityEntities() As DataTable Implements IDSecurityEntity.GetSecurityEntities
            Dim myParameters() As MySqlParameter =
            {
              New MySqlParameter("@P_Security_Entity_SeqID", -1)
             }
            Return MyBase.GetDataTable("ZGWSecurity.Get_Security_Entity", myParameters)
        End Function

        Function GetSecurityEntities(ByVal account As String, ByVal securityEntityId As Integer, ByVal isSecurityEntityAdministrator As Boolean) As DataTable Implements IDSecurityEntity.GetSecurityEntities
            If String.IsNullOrEmpty(account) Then Throw New ArgumentNullException("account", "account cannot be a null reference (Nothing in Visual Basic)!")
            If securityEntityId = -1 Then Throw New ArgumentNullException("securityEntityId", "securityEntityId cannot be a null reference (Nothing in Visual Basic)!")

            Dim myStoreProcedure As String = "ZGWSecurity.Get_Valid_Security_Entity"
            Dim myParameters() As MySqlParameter =
            {
             New MySqlParameter("@P_ACCT", account),
             New MySqlParameter("@P_IS_SE_ADMIN", isSecurityEntityAdministrator),
             New MySqlParameter("@P_SE_SEQ_ID", securityEntityId),
             GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
            }
            Return MyBase.GetDataTable(myStoreProcedure, myParameters)
        End Function

        ''' <summary>
        ''' Gets the valid security entities.
        ''' </summary>
        ''' <param name="account">The account.</param>
        ''' <param name="SecurityEntityID">The security entity ID.</param>
        ''' <param name="isSecurityEntityAdministrator">if set to <c>true</c> [is security entity administrator].</param>
        ''' <returns>DataTable.</returns>
        Function GetValidSecurityEntities(ByVal account As String, ByVal securityEntityId As Integer, ByVal isSecurityEntityAdministrator As Boolean) As DataTable Implements IDSecurityEntity.GetValidSecurityEntities
            If account = String.Empty Then Throw New ArgumentNullException("account", "account cannot be a null reference (Nothing in Visual Basic) or blank.")
            If securityEntityId = -1 Then Throw New ArgumentNullException("securityEntityId", "securityEntityId must be greater than -1")

            Dim mStoreProcedure As String = "ZGWSecurity.Get_Valid_Security_Entity"
            Dim mParameters() As MySqlParameter =
            {
              New MySqlParameter("@P_Account", account),
              New MySqlParameter("@P_IS_SE_ADMIN", isSecurityEntityAdministrator),
              New MySqlParameter("@P_Security_Entity_SeqID", securityEntityId)
            }
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Function Save(ByVal profile As MSecurityEntityProfile) As Integer Implements IDSecurityEntity.Save
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mPrimaryKey As MySqlParameter = GetSqlParameter("@P_PRIMARY_KEY", Nothing, ParameterDirection.Output)
            mPrimaryKey.Size = 10
            Dim mParameters() As MySqlParameter =
             {
             New MySqlParameter("@P_Security_Entity_SeqID", profile.Id),
             New MySqlParameter("@P_NAME", profile.Name),
             New MySqlParameter("@P_DESCRIPTION", profile.Description),
             New MySqlParameter("@P_URL", profile.Url),
             New MySqlParameter("@P_Status_SeqID", profile.StatusSeqId),
             New MySqlParameter("@P_DAL", profile.DataAccessLayer),
             New MySqlParameter("@P_DAL_Name", profile.DataAccessLayerAssemblyName),
             New MySqlParameter("@P_DAL_NAME_SPACE", profile.DataAccessLayerNamespace),
             New MySqlParameter("@P_DAL_STRING", profile.ConnectionString),
             New MySqlParameter("@P_SKIN", profile.Skin),
             New MySqlParameter("@P_STYLE", profile.Style),
             New MySqlParameter("@P_ENCRYPTION_TYPE", profile.EncryptionType),
             New MySqlParameter("@P_Parent_Security_Entity_SeqID", profile.ParentSeqId),
             New MySqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(profile)),
             mPrimaryKey
             }
            MyBase.ExecuteNonQuery("ZGWSecurity.Set_Security_Entity", mParameters)
            profile.Id = Integer.Parse(GetParameterValue("@P_PRIMARY_KEY", mParameters).ToString(), CultureInfo.InvariantCulture)
            Return profile.Id
        End Function

        Public Function Search(searchCriteria As Model.Profiles.MSearchCriteria) As System.Data.DataTable Implements Interfaces.IDSecurityEntity.Search
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!.")
            Dim mRetVal As DataTable
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_Columns", searchCriteria.Columns),
              New MySqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              New MySqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              New MySqlParameter("@P_PageSize", searchCriteria.PageSize),
              New MySqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              New MySqlParameter("@P_TableOrView", "ZGWSecurity.Security_Entities"),
              New MySqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             }
            mRetVal = MyBase.GetDataTable("ZGWSystem.Get_Paginated_Data", mParameters)
            Return mRetVal
        End Function

    End Class
End Namespace

