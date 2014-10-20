Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Enumerations
Imports System.Web

Namespace Utilities
    Public Module RoleUtility
        ''' <summary>
        ''' Securities the name of the entities roles cache.
        ''' </summary>
        ''' <param name="securityEntitySeqId">The security entity seq id.</param>
        ''' <returns>System.String.</returns>
        Public Function SecurityEntitiesRolesCacheName(securityEntitySeqId As Integer) As String
            Return "SecurityEntityRoles" + securityEntitySeqId.ToString()
        End Function

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <param name="roleID">The role ID.</param>
        ''' <returns>MRoleProfile.</returns>
        Public Function GetProfile(roleID As Integer) As MRoleProfile
            Dim mBRoles As BRoles = New BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Dim mProfile As MRoleProfile = New MRoleProfile()
            mProfile.Id = roleID
            mBRoles.GetProfile(mProfile)
            Return mProfile
        End Function

        ''' <summary>
        ''' Deletes the role.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub DeleteRole(profile As MRoleProfile)
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
        Public Function Search(ByRef searchCriteria As MSearchCriteria) As DataTable
            Dim mBRoles As BRoles = New BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBRoles.Search(searchCriteria)
        End Function

        ''' <summary>
        ''' Updates all accounts for role.
        ''' </summary>
        ''' <param name="roleID">The role ID.</param>
        ''' <param name="securityEntitySeqId">The security entity seq id.</param>
        ''' <param name="accounts">The accounts.</param>
        ''' <param name="accountID">The account ID.</param>
        ''' <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        Public Function UpdateAllAccountsForRole(roleID As Integer, securityEntitySeqId As Integer, accounts As String(), accountID As Integer) As Boolean
            Dim mBRoles As BRoles = New BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBRoles.UpdateAllAccountsForRole(roleID, securityEntitySeqId, accounts, accountID)
        End Function

    End Module
End Namespace

