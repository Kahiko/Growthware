Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Enumerations
Imports System.Web
Imports System.Globalization

Namespace Utilities
    Public Module RoleUtility
        ''' <summary>
        ''' Securities the name of the entities roles cache.
        ''' </summary>
        ''' <param name="securityEntitySeqId">The security entity seq id.</param>
        ''' <returns>System.String.</returns>
        Public Function SecurityEntitiesRolesCacheName(securityEntitySeqId As Integer) As String
            Return "SecurityEntityRoles" + securityEntitySeqId.ToString(CultureInfo.InvariantCulture)
        End Function

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <param name="roleId">The role ID.</param>
        ''' <returns>MRoleProfile.</returns>
        Public Function GetProfile(roleId As Integer) As MRoleProfile
            Dim mBRoles As BRoles = New BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Dim mProfile As MRoleProfile = New MRoleProfile()
            mProfile.Id = roleId
            mProfile = mBRoles.GetProfile(mProfile)
            Return mProfile
        End Function

        ''' <summary>
        ''' Deletes the role.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub DeleteRole(profile As MRoleProfile)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be blank or a null reference (Nothing in Visual Basic)")
            Dim mSecurityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mBRoles As BRoles = New BRoles(mSecurityProfile, ConfigSettings.CentralManagement)
            profile.SecurityEntityId = mSecurityProfile.Id
            mBRoles.Delete(profile)
            RoleUtility.RemoveRoleCache(profile.SecurityEntityId)
            FunctionUtility.RemoveCachedFunctions()
        End Sub

        ''' <summary>
        ''' Gets the accounts in role.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns>ArrayList.</returns>
        Public Function GetAccountsInRole(profile As MRoleProfile) As ArrayList
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be blank or a null reference (Nothing in Visual Basic)")
            Dim colAccounts As ArrayList = New ArrayList()
            Dim mBRoles As BRoles = New BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Dim myDataTable As DataTable = mBRoles.GetAccountsInRole(profile)
            For Each accountsRow As DataRow In myDataTable.Rows
                colAccounts.Add(CStr(accountsRow("ACCT")))
            Next
            Return colAccounts
        End Function

        ''' <summary>
        ''' Gets the accounts not in role.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns>ArrayList.</returns>
        Public Function GetAccountsNotInRole(profile As MRoleProfile) As ArrayList
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be blank or a null reference (Nothing in Visual Basic)")
            Dim colAccounts As ArrayList = New ArrayList()
            Dim mBRoles As BRoles = New BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Dim myDataTable As DataTable = mBRoles.GetAccountsNotInRole(profile)
            For Each accountsRow As DataRow In myDataTable.Rows
                colAccounts.Add(CStr(accountsRow("ACCT")))
            Next
            Return colAccounts
        End Function

        ''' <summary>
        ''' Gets all roles by security entity.
        ''' </summary>
        ''' <param name="securityEntitySeqId">The security entity seq id.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetAllRolesBySecurityEntity(securityEntitySeqId As Integer) As DataTable
            Dim mySecurityEntityRoles As DataTable = CType(HttpContext.Current.Cache(RoleUtility.SecurityEntitiesRolesCacheName(securityEntitySeqId)), DataTable)
            If mySecurityEntityRoles Is Nothing Then
                Dim mBRoles As BRoles = New BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
                mySecurityEntityRoles = mBRoles.GetRolesBySecurityEntity(securityEntitySeqId)
                CacheController.AddToCacheDependency(RoleUtility.SecurityEntitiesRolesCacheName(securityEntitySeqId), mySecurityEntityRoles)
            End If
            Return mySecurityEntityRoles
        End Function

        ''' <summary>
        ''' Gets the roles array list by security entity.
        ''' </summary>
        ''' <param name="securityEntitySeqId">The security entity seq id.</param>
        ''' <returns>ArrayList.</returns>
        Public Function GetRolesArrayListBySecurityEntity(securityEntitySeqId As Integer) As ArrayList
            Dim colRoles As ArrayList = New ArrayList()
            Dim mySecurityEntityRoles As DataTable = RoleUtility.GetAllRolesBySecurityEntity(securityEntitySeqId)
            For Each roleRow As DataRow In mySecurityEntityRoles.Rows
                colRoles.Add(CStr(roleRow("NAME")))
            Next
            Return colRoles
        End Function

        ''' <summary>
        ''' Removes the role cache.
        ''' </summary>
        ''' <param name="securityEntitySeqId">The security entity seq id.</param>
        Public Sub RemoveRoleCache(securityEntitySeqId As Integer)
            CacheController.RemoveFromCache(RoleUtility.SecurityEntitiesRolesCacheName(securityEntitySeqId))
        End Sub

        ''' <summary>
        ''' Saves the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub Save(profile As MRoleProfile)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be blank or a null reference (Nothing in Visual Basic)")
            profile.SecurityEntityId = SecurityEntityUtility.CurrentProfile.Id
            Dim mBRoles As BRoles = New BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            mBRoles.Save(profile)
            RoleUtility.RemoveRoleCache(profile.SecurityEntityId)
            FunctionUtility.RemoveCachedFunctions()
        End Sub

        ''' <summary>
        ''' Searches the specified search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>DataTable.</returns>
        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be blank or a null reference (Nothing in Visual Basic)")
            Dim mBRoles As BRoles = New BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBRoles.Search(searchCriteria)
        End Function

        ''' <summary>
        ''' Updates all accounts for role.
        ''' </summary>
        ''' <param name="roleId">The role ID.</param>
        ''' <param name="securityEntitySeqId">The security entity seq id.</param>
        ''' <param name="accounts">The accounts.</param>
        ''' <param name="accountId">The account ID.</param>
        ''' <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        Public Function UpdateAllAccountsForRole(roleId As Integer, securityEntitySeqId As Integer, accounts As String(), accountId As Integer) As Boolean
            If accounts Is Nothing Then Throw New ArgumentNullException("accounts", "accounts cannot be blank or a null reference (Nothing in Visual Basic)")
            Dim mBRoles As BRoles = New BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBRoles.UpdateAllAccountsForRole(roleId, securityEntitySeqId, accounts, accountId)
        End Function

    End Module
End Namespace

