Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports System.Web

Namespace Utilities
    Public Module GroupUtility
        ''' <summary>
        ''' Searches the specified search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>DataTable.</returns>
        Public Function Search(ByRef searchCriteria As MSearchCriteria) As DataTable
            Dim mBGroups As BGroups = New BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBGroups.Search(searchCriteria)
        End Function

        ''' <summary>
        ''' Securities the name of the entities groups cache.
        ''' </summary>
        ''' <param name="securityEntityId">The security entity id.</param>
        ''' <returns>System.String.</returns>
        Public Function SecurityEntitiesGroupsCacheName(ByVal securityEntityId As Integer) As String
            Dim retVal As String = securityEntityId & "SecurityEntityGroups"
            Return retVal
        End Function

        ''' <summary>
        ''' Gets all groups by security entity.
        ''' </summary>
        ''' <param name="securityEntityId">The security entity id.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetAllGroupsBySecurityEntity(ByVal securityEntityId As Integer) As DataTable
            Dim mySecurityEntityGroups As DataTable

            ' attempt to retrieve the information from cache
            mySecurityEntityGroups = HttpContext.Current.Cache.Item(SecurityEntitiesGroupsCacheName(securityEntityId))
            ' if the information was not avalible in cache
            ' then retieve the information from the DB and put it into
            ' cache for subsequent use.
            If mySecurityEntityGroups Is Nothing Then
                Dim mBGroups As BGroups = New BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
                mySecurityEntityGroups = mBGroups.GetGroupsBySecurityEntity(securityEntityId)
                CacheController.AddToCacheDependency(SecurityEntitiesGroupsCacheName(securityEntityId), mySecurityEntityGroups)
            End If
            Return mySecurityEntityGroups
        End Function

        ''' <summary>
        ''' Gets the groups array list by security entity.
        ''' </summary>
        ''' <param name="securityEntityId">The security entity id.</param>
        ''' <returns>ArrayList.</returns>
        Public Function GetGroupsArrayListBySecurityEntity(ByVal securityEntityId As Integer) As ArrayList
            Dim mySecurityEntityGroups As DataTable
            Dim colGroups As New ArrayList
            Dim groupRow As DataRow
            mySecurityEntityGroups = GetAllGroupsBySecurityEntity(securityEntityId)
            For Each groupRow In mySecurityEntityGroups.Rows
                colGroups.Add(CStr(groupRow("NAME")))
            Next groupRow
            Return colGroups
        End Function

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <param name="groupId">The group ID.</param>
        ''' <returns>MGroupProfile.</returns>
        Public Function GetProfile(ByVal groupId As Integer) As MGroupProfile
            Dim mBGroups As BGroups = New BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBGroups.GetProfile(groupId)
        End Function

        ''' <summary>
        ''' Saves the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub Save(ByVal profile As MGroupProfile)
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mBGroups As BGroups = New BGroups(mSecurityEntityProfile, ConfigSettings.CentralManagement)
            profile.SecurityEntityId = mSecurityEntityProfile.Id
            mBGroups.Save(profile)
            CacheController.RemoveFromCache(SecurityEntitiesGroupsCacheName(profile.SecurityEntityId))
            CacheController.RemoveAllCache()
        End Sub

        ''' <summary>
        ''' Deletes the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub Delete(ByVal profile As MGroupProfile)
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mBGroups As BGroups = New BGroups(mSecurityEntityProfile, ConfigSettings.CentralManagement)
            profile.SecurityEntityId = mSecurityEntityProfile.Id
            mBGroups.DeleteGroup(profile)
            CacheController.RemoveFromCache(SecurityEntitiesGroupsCacheName(profile.SecurityEntityId))
            CacheController.RemoveAllCache()
        End Sub

        ''' <summary>
        ''' Updates the group roles.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub UpdateGroupRoles(ByVal profile As MGroupRoles)
            Dim mBGroups As BGroups = New BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            mBGroups.UpdateGroupRoles(profile)
            CacheController.RemoveAllCache()
        End Sub

    End Module
End Namespace

