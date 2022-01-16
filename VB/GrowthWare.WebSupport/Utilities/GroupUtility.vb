Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports System.Web
Imports GrowthWare.Framework.BusinessData.DataAccessLayer

Namespace Utilities
    Public Class GroupUtility
        ''' <summary>
        ''' Searches the specified search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>DataTable.</returns>
        Public Shared Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria can not be null (Nothing in Visual Basic) or empty!")
            Dim mBGroups As BGroups = New BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBGroups.Search(searchCriteria)
        End Function

        ''' <summary>
        ''' Securities the name of the entities groups cache.
        ''' </summary>
        ''' <param name="securityEntityId">The security entity id.</param>
        ''' <returns>System.String.</returns>
        Public Shared Function SecurityEntitiesGroupsCacheName(ByVal securityEntityId As Integer) As String
            Dim retVal As String = securityEntityId & "SecurityEntityGroups"
            Return retVal
        End Function

        ''' <summary>
        ''' Gets all groups by security entity.
        ''' </summary>
        ''' <param name="securityEntityId">The security entity id.</param>
        ''' <returns>DataTable.</returns>
        Public Shared Function GetAllGroupsBySecurityEntity(ByVal securityEntityId As Integer) As DataTable
            Dim mySecurityEntityGroups As DataTable

            ' attempt to retrieve the information from cache
            mySecurityEntityGroups = HttpContext.Current.Cache.Item(SecurityEntitiesGroupsCacheName(securityEntityId))
            ' if the information was not available in cache
            ' then retrieve the information from the DB and put it into
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
        Public Shared Function GetGroupsArrayListBySecurityEntity(ByVal securityEntityId As Integer) As ArrayList
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
        Public Shared Function GetProfile(ByVal groupId As Integer) As MGroupProfile
            Dim mRetVal As New MGroupProfile()
            If (groupId <> -1) Then
                Dim mBGroups As BGroups = New BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
                mRetVal = mBGroups.GetProfile(groupId)
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the selected roles.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns>System.String[][].</returns>
        Public Shared Function GetSelectedRoles(ByVal profile As MGroupRoles) As String()
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!")
            Dim mBGroups As New BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBGroups.GetSelectedRoles(profile)
        End Function

        ''' <summary>
        ''' Saves the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Shared Function Save(ByVal profile As MGroupProfile) As Integer
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile can not be null (Nothing in Visual Basic) or empty!")
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mBGroups As BGroups = New BGroups(mSecurityEntityProfile, ConfigSettings.CentralManagement)
            Dim mGroupSeqId As Integer
            profile.SecurityEntityId = mSecurityEntityProfile.Id
            mGroupSeqId = mBGroups.Save(profile)
            CacheController.RemoveFromCache(SecurityEntitiesGroupsCacheName(profile.SecurityEntityId))
            CacheController.RemoveAllCache()
            Return mGroupSeqId
        End Function

        ''' <summary>
        ''' Deletes the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Shared Sub Delete(ByVal profile As MGroupProfile)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile can not be null (Nothing in Visual Basic) or empty!")
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
        Public Shared Sub UpdateGroupRoles(ByVal profile As MGroupRoles)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile can not be null (Nothing in Visual Basic) or empty!")
            Dim mBGroups As BGroups = New BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Try
                mBGroups.UpdateGroupRoles(profile)
            Catch ex As Exception
                Dim mLogger As Logger = Logger.Instance()
                mLogger.Error(ex)
                Throw New WebSupportException("Could not associate the roles to the group please see the logs for details.")
            End Try
            CacheController.RemoveAllCache()
        End Sub
    End Class
End Namespace

