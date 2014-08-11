Imports GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports System.Data.SqlClient
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Model.Enumerations
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
    Public Class DAccounts
        Inherits DDBInteraction
        Implements IDAccount

#Region "Private Fields"
        Private m_Profile As MAccountProfile = Nothing
        Private m_SecurityEntitySeqID As Integer = -2
#End Region

#Region "Public Properties"
        Protected Property SecurityEntitySeqId() As Integer Implements IDAccount.SecurityEntitySeqId
            Get
                Return m_SecurityEntitySeqID
            End Get
            Set(ByVal value As Integer)
                m_SecurityEntitySeqID = value
            End Set
        End Property

        Protected Property Profile() As MAccountProfile Implements IDAccount.Profile
            Get
                Return m_Profile
            End Get
            Set(ByVal value As MAccountProfile)
                m_Profile = value
            End Set
        End Property

        ''' <summary>
        ''' Retruns a DataRow of account the account details
        ''' </summary>
        ''' <returns>DataRow</returns>
        ''' <remarks>Usefull for populating MAccountProfile</remarks>
        Protected ReadOnly Property GetAccount() As DataRow Implements IDAccount.GetAccount
            Get
                checkValid()
                Dim mStoredProcedure As String = "ZGWSecurity.Get_Account"
                Dim mParameters As SqlParameter() =
                {
                 New SqlParameter("@P_Is_System_Admin", m_Profile.IsSystemAdmin),
                 New SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqID),
                 New SqlParameter("@P_Account", m_Profile.Account)
                }
                Return MyBase.GetDataRow(mStoredProcedure, mParameters)
            End Get
        End Property

        Protected ReadOnly Property GetAccounts() As DataTable Implements IDAccount.GetAccounts
            Get
                checkValid()
                Dim mStoredProcedure As String = "ZGWSecurity.Get_Account"
                Dim mParameters As SqlParameter() =
                {
                 New SqlParameter("@P_Is_System_Admin", m_Profile.IsSystemAdmin),
                 New SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqID),
                 New SqlParameter("@P_Account", "")
                }
                Return MyBase.GetDataTable(mStoredProcedure, mParameters)
            End Get
        End Property

#End Region

#Region "Public Methods"
        Protected Sub Delete() Implements IDAccount.Delete
            Dim mStoredProcedure As String = "ZGWSecurity.Delete_Account"
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_Account_SeqID", m_Profile.Id)
             }
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Protected Function GetGroups() As DataTable Implements IDAccount.Groups
            checkValid()
            Dim mStoredProcedure As String = "ZGWSecurity.Get_Account_Groups"
            Dim mParameters() As SqlParameter =
            { _
              New SqlParameter("@P_Account", m_Profile.Account),
              New SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqID)
            }
            Return MyBase.GetDataTable(mStoredProcedure, mParameters)
        End Function

        Protected Function GetMenu(ByVal account As String, menuType As MenuType) As DataTable Implements IDAccount.GetMenu
            Dim mStoredProcedure As String = "ZGWSecurity.Get_Menu_Data"
            Dim mParameters As SqlParameter() =
            {
             New SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqID),
             New SqlParameter("@P_Navigation_Types_NVP_Detail_SeqID", menuType),
             New SqlParameter("@P_Account", account)
            }
            Return MyBase.GetDataTable(mStoredProcedure, mParameters)
        End Function

        Protected Function GetSecurity() As DataTable Implements IDAccount.Security
            checkValid()
            Dim mStoredProcedure As String = "ZGWSecurity.Get_Account_Security"
            Dim mParameters() As SqlParameter =
            { _
              New SqlParameter("@P_Account", m_Profile.Account),
              New SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqID)
            }
            Return MyBase.GetDataTable(mStoredProcedure, mParameters)
        End Function

        Protected Function Save() As Integer Implements IDAccount.Save
            checkValid()
            Dim mRetInt As Integer
            Dim mStoredProcedure As String = "ZGWSecurity.Set_Account"
            Dim mParameters As SqlParameter() =
            {
             New SqlParameter("@P_Account_SeqID", m_Profile.Id),
             New SqlParameter("@P_Status_SeqID", m_Profile.Status),
             New SqlParameter("@P_Account", m_Profile.Account),
             New SqlParameter("@P_First_Name", m_Profile.FirstName),
             New SqlParameter("@P_Last_Name", m_Profile.LastName),
             New SqlParameter("@P_Middle_Name", m_Profile.MiddleName),
             New SqlParameter("@P_Preferred_Name", m_Profile.PreferredName),
             New SqlParameter("@P_Email", m_Profile.Email),
             New SqlParameter("@P_Password", m_Profile.Password),
             New SqlParameter("@P_Password_Last_Set", m_Profile.PasswordLastSet),
             New SqlParameter("@P_Failed_Attempts", m_Profile.FailedAttempts),
             New SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile)),
             New SqlParameter("@P_Last_Login", m_Profile.LastLogOn),
             New SqlParameter("@P_Time_Zone", m_Profile.TimeZone),
             New SqlParameter("@P_Location", m_Profile.Location),
             New SqlParameter("@P_Enable_Notifications", m_Profile.EnableNotifications),
             New SqlParameter("@P_Is_System_Admin", m_Profile.IsSystemAdmin)
            }
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
            mRetInt = Integer.Parse(GetParameterValue("@P_Account_SeqID", mParameters), CultureInfo.InvariantCulture)
            Return mRetInt
        End Function

        Protected Sub SaveGroups() Implements IDAccount.SaveGroups
            Dim mStoredProcedure As String = "ZGWSecurity.Set_Account_Groups"
            Dim mParameters As SqlParameter() = {
              New SqlParameter("@P_Account", m_Profile.Account),
              New SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqID),
              New SqlParameter("@P_Groups", m_Profile.GetCommaSeparatedAssignedGroups()),
              New SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile))
             }
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Protected Sub SaveRoles() Implements IDAccount.SaveRoles
            Dim mStoredProcedure As String = "ZGWSecurity.Set_Account_Roles"
            Dim mParameters As SqlParameter() = {
              New SqlParameter("@P_Account", m_Profile.Account),
              New SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqID),
              New SqlParameter("@P_Roles", m_Profile.GetCommaSeparatedAssignedRoles()),
              New SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile))
             }
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Protected Function GetRoles() As DataTable Implements IDAccount.Roles
            checkValid()
            Dim mStoredProcedure As String = "ZGWSecurity.Get_Account_Roles"
            Dim mParameters() As SqlParameter =
            { _
              New SqlParameter("@P_Account", m_Profile.Account),
              New SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqID)
            }
            Return MyBase.GetDataTable(mStoredProcedure, mParameters)
        End Function

        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable Implements IDAccount.Search
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria can not be Nothing")
            Dim mRetVal As DataTable
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_Columns", searchCriteria.Columns),
              New SqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              New SqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              New SqlParameter("@P_PageSize", searchCriteria.PageSize),
              New SqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              New SqlParameter("@P_TableOrView", "ZGWSecurity.Accounts"),
              New SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             }
            mRetVal = MyBase.GetDataTable("ZGWSystem.Get_Paginated_Data", mParameters)
            Return mRetVal
        End Function
#End Region

#Region "Private Methods"
        Private Sub checkValid()
            MyBase.IsValid()
            If m_Profile Is Nothing Then
                Throw New DataAccessLayerException("Profile property must be set before calling methods from this class")
            End If
            If m_SecurityEntitySeqID = -2 Then
                Throw New DataAccessLayerException("SE_SEQ_ID property must be set before calling methods from this class")
            End If
        End Sub
#End Region
    End Class
End Namespace
