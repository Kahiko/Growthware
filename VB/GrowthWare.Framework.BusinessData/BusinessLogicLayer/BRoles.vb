Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports GrowthWare.Framework.Common

Namespace BusinessLogicLayer
    Public Class BRoles
        Inherits BaseBusinessLogic

        Private m_BRoles As IDRoles
        Private Sub New()
        End Sub

        Public Sub New(securityEntityProfile As MSecurityEntityProfile, centralManagement As Boolean)
            If securityEntityProfile Is Nothing Then
                Throw New ArgumentNullException("securityEntityProfile", "The securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!!")
            End If
            If centralManagement Then
                If m_BRoles Is Nothing Then
                    m_BRoles = CType(ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DRoles"), IDRoles)
                End If
            Else
                m_BRoles = CType(ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DRoles"), IDRoles)
            End If
            m_BRoles.ConnectionString = securityEntityProfile.ConnectionString
            m_BRoles.SecurityEntitySeqID = securityEntityProfile.Id
        End Sub

        ''' <summary>
        ''' Saves the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub Save(profile As MRoleProfile)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!")
            m_BRoles.Profile = profile
            If DatabaseIsOnline() Then m_BRoles.Save()
        End Sub

        ''' <summary>
        ''' Searches the specified search critera.
        ''' </summary>
        ''' <param name="searchCriteria">The search critera.</param>
        ''' <returns>System.Data.DataTable.</returns>
        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!!")
            Dim mRetVal As DataTable = Nothing
            If DatabaseIsOnline() Then mRetVal = m_BRoles.Search(searchCriteria)
            Return mRetVal
        End Function

        ''' <summary>
        ''' Deletes the role.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub Delete(profile As MRoleProfile)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!")
            m_BRoles.Profile = profile
            If DatabaseIsOnline() Then m_BRoles.DeleteRole()
        End Sub

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub GetProfile(ByVal profile As MRoleProfile)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!")
            m_BRoles.Profile = profile
            If DatabaseIsOnline() Then profile = New MRoleProfile(m_BRoles.ProfileData())
        End Sub

        ''' <summary>
        ''' Gets the roles by security entity.
        ''' </summary>
        ''' <param name="securityEntityId">The security entity ID.</param>
        ''' <returns>System.Data.DataTable.</returns>
        Public Function GetRolesBySecurityEntity(securityEntityId As Integer) As DataTable
            Dim mRetVal As DataTable = Nothing
            m_BRoles.SecurityEntitySeqId = securityEntityId
            If DatabaseIsOnline() Then mRetVal = m_BRoles.RolesBySecurityEntity()
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the accounts in role.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns>System.Data.DataTable.</returns>
        Public Function GetAccountsInRole(profile As MRoleProfile) As DataTable
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!")
            Dim mRetVal As DataTable = Nothing
            m_BRoles.Profile = profile
            If DatabaseIsOnline() Then mRetVal = m_BRoles.AccountsInRole()
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the accounts not in role.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns>System.Data.DataTable.</returns>
        Public Function GetAccountsNotInRole(profile As MRoleProfile) As DataTable
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!")
            Dim mRetVal As DataTable = Nothing
            m_BRoles.Profile = profile
            If DatabaseIsOnline() Then mRetVal = m_BRoles.AccountsNotInRole()
            Return mRetVal
        End Function

        ''' <summary>
        ''' Updates all accounts for role.
        ''' </summary>
        ''' <param name="roleSeqId">The role seq ID.</param>
        ''' <param name="securityEntityId">The security entity ID.</param>
        ''' <param name="accounts">The accounts.</param>
        ''' <param name="accountSeqId">The account seq ID.</param>
        ''' <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        Public Function UpdateAllAccountsForRole(roleSeqId As Integer, securityEntityId As Integer, accounts As String(), accountSeqId As Integer) As Boolean
            If accounts Is Nothing Then Throw New ArgumentNullException("accounts", "accounts cannot be a null reference (Nothing in Visual Basic)!!")
            Dim mRetVal As Boolean = False
            If DatabaseIsOnline() Then mRetVal = m_BRoles.UpdateAllAccountsForRole(roleSeqId, securityEntityId, accounts, accountSeqId)
            Return mRetVal
        End Function

    End Class
End Namespace
