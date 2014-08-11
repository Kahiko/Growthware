Imports GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports System.Data.SqlClient
Imports GrowthWare.Framework.Model.Profiles
Imports System.Globalization

Namespace DataAccessLayer.SQLServer.V2008
    ''' <summary>
    ''' Provides data access to SQL Server 2008
    ''' </summary>
    Public Class DSecurityEntity
        Inherits DDBInteraction
        Implements IDSecurityEntity

        Function GetSecurityEntities() As DataTable Implements IDSecurityEntity.GetSecurityEntities
            Dim myParameters() As SqlParameter =
            {
              New SqlParameter("@P_Security_Entity_SeqID", -1)
             }
            Return MyBase.GetDataTable("ZGWSecurity.Get_Security_Entity", myParameters)
        End Function

        Function GetSecurityEntities(ByVal account As String, ByVal securityEntityId As Integer, ByVal isSecurityEntityAdministrator As Boolean) As DataTable Implements IDSecurityEntity.GetSecurityEntities
            If String.IsNullOrEmpty(account) Then Throw New ArgumentNullException("account", "account not given")
            If securityEntityId = -1 Then Throw New ArgumentNullException("securityEntityId", "securityEntityId not given")

            Dim myStoreProcedure As String = "ZGWSecurity.Get_Valid_Security_Entity"
            Dim myParameters() As SqlParameter =
            {
             New SqlParameter("@P_ACCT", account),
             New SqlParameter("@P_IS_SE_ADMIN", isSecurityEntityAdministrator),
             New SqlParameter("@P_SE_SEQ_ID", securityEntityId),
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
            If account = String.Empty Then Throw New ArgumentNullException("account", "account can not be blank")
            If securityEntityId = -1 Then Throw New ArgumentNullException("securityEntityId", "securityEntityId must be greater than -1")

            Dim mStoreProcedure As String = "ZGWSecurity.Get_Valid_Security_Entity"
            Dim mParameters() As SqlParameter =
            {
              New SqlParameter("@P_Account", account),
              New SqlParameter("@P_IS_SE_ADMIN", isSecurityEntityAdministrator),
              New SqlParameter("@P_Security_Entity_SeqID", securityEntityId)
            }
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Function Save(ByVal profile As MSecurityEntityProfile) As Integer Implements IDSecurityEntity.Save
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile can not be nothing")
            Dim mPrimaryKey As SqlParameter = GetSqlParameter("@P_PRIMARY_KEY", Nothing, ParameterDirection.Output)
            mPrimaryKey.Size = 10
            Dim mParameters() As SqlParameter =
             {
             New SqlParameter("@P_Security_Entity_SeqID", profile.Id),
             New SqlParameter("@P_NAME", profile.Name),
             New SqlParameter("@P_DESCRIPTION", profile.Description),
             New SqlParameter("@P_URL", profile.Url),
             New SqlParameter("@P_Status_SeqID", profile.StatusSeqId),
             New SqlParameter("@P_DAL", profile.DataAccessLayer),
             New SqlParameter("@P_DAL_Name", profile.DataAccessLayerAssemblyName),
             New SqlParameter("@P_DAL_NAME_SPACE", profile.DataAccessLayerNamespace),
             New SqlParameter("@P_DAL_STRING", profile.ConnectionString),
             New SqlParameter("@P_SKIN", profile.Skin),
             New SqlParameter("@P_STYLE", profile.Style),
             New SqlParameter("@P_ENCRYPTION_TYPE", profile.EncryptionType),
             New SqlParameter("@P_Parent_Security_Entity_SeqID", profile.ParentSeqId),
             New SqlParameter("@P_Added_Updated_By", profile.AddedBy),
             mPrimaryKey
             }
            MyBase.ExecuteNonQuery("ZGWSecurity.Set_Security_Entity", mParameters)
            profile.Id = Integer.Parse(MyBase.GetParameterValue("@P_PRIMARY_KEY", mParameters).ToString(), CultureInfo.InvariantCulture)
            Return profile.Id
        End Function

        Public Function Search(searchCriteria As Model.Profiles.MSearchCriteria) As System.Data.DataTable Implements Interfaces.IDSecurityEntity.Search
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria can not be nothing.")
            Dim mRetVal As DataTable
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_Columns", searchCriteria.Columns),
              New SqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              New SqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              New SqlParameter("@P_PageSize", searchCriteria.PageSize),
              New SqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              New SqlParameter("@P_TableOrView", "ZGWSecurity.Security_Entities"),
              New SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             }
            mRetVal = MyBase.GetDataTable("ZGWSystem.Get_Paginated_Data", mParameters)
            Return mRetVal
        End Function

    End Class
End Namespace

