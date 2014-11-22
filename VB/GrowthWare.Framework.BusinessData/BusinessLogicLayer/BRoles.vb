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
                Throw New ArgumentException("The securityEntityProfile and not be null!")
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
            m_BRoles.Profile = profile
            m_BRoles.Save()
        End Sub

        ''' <summary>
        ''' Searches the specified search critera.
        ''' </summary>
        ''' <param name="searchCriteria">The search critera.</param>
        ''' <returns>System.Data.DataTable.</returns>
        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            Return m_BRoles.Search(searchCriteria)
        End Function

        ''' <summary>
        ''' Deletes the role.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub Delete(profile As MRoleProfile)
            m_BRoles.Profile = profile
            m_BRoles.DeleteRole()
        End Sub

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub GetProfile(ByVal profile As MRoleProfile)
            m_BRoles.Profile = profile
            profile = New MRoleProfile(m_BRoles.GetProfileData())
        End Sub

        ''' <summary>
        ''' Gets the roles by security entity.
        ''' </summary>
        ''' <param name="securityEntityId">The security entity ID.</param>
        ''' <returns>System.Data.DataTable.</returns>
        Public Function GetRolesBySecurityEntity(securityEntityId As Integer) As DataTable
            m_BRoles.SecurityEntitySeqId = securityEntityId
            Return m_BRoles.GetRolesBySecurityEntity()
        End Function

        ''' <summary>
        ''' Gets the accounts in role.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns>System.Data.DataTable.</returns>
        Public Function GetAccountsInRole(profile As MRoleProfile) As DataTable
            m_BRoles.Profile = profile
            Return m_BRoles.GetAccountsInRole()
        End Function

        ''' <summary>
        ''' Gets the accounts not in role.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns>System.Data.DataTable.</returns>
        Public Function GetAccountsNotInRole(profile As MRoleProfile) As System.Data.DataTable
            m_BRoles.Profile = profile
            Return m_BRoles.GetAccountsNotInRole()
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
            Return m_BRoles.UpdateAllAccountsForRole(roleSeqId, securityEntityId, accounts, AccountSeqID)
        End Function

    End Class
End Namespace
