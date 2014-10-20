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
        ''' <param name="searchCritera">The search critera.</param>
        ''' <returns>System.Data.DataTable.</returns>
        Public Function Search(ByRef searchCritera As MSearchCriteria) As System.Data.DataTable
            Return m_BRoles.Search(searchCritera)
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
        Public Sub GetProfile(ByRef profile As MRoleProfile)
            m_BRoles.Profile = profile
            profile = New MRoleProfile(m_BRoles.GetProfileData())
        End Sub

        ''' <summary>
        ''' Gets the roles by security entity.
        ''' </summary>
        ''' <param name="SecurityEntityID">The security entity ID.</param>
        ''' <returns>System.Data.DataTable.</returns>
        Public Function GetRolesBySecurityEntity(SecurityEntityID As Integer) As System.Data.DataTable
            m_BRoles.SecurityEntitySeqID = SecurityEntityID
            Return m_BRoles.GetRolesBySecurityEntity()
        End Function

        ''' <summary>
        ''' Gets the accounts in role.
        ''' </summary>
        ''' <param name="Profile">The profile.</param>
        ''' <returns>System.Data.DataTable.</returns>
        Public Function GetAccountsInRole(Profile As MRoleProfile) As System.Data.DataTable
            m_BRoles.Profile = Profile
            Return m_BRoles.GetAccountsInRole()
        End Function

        ''' <summary>
        ''' Gets the accounts not in role.
        ''' </summary>
        ''' <param name="Profile">The profile.</param>
        ''' <returns>System.Data.DataTable.</returns>
        Public Function GetAccountsNotInRole(Profile As MRoleProfile) As System.Data.DataTable
            m_BRoles.Profile = Profile
            Return m_BRoles.GetAccountsNotInRole()
        End Function

        ''' <summary>
        ''' Updates all accounts for role.
        ''' </summary>
        ''' <param name="RoleSeqID">The role seq ID.</param>
        ''' <param name="SecurityEntityID">The security entity ID.</param>
        ''' <param name="Accounts">The accounts.</param>
        ''' <param name="AccountSeqID">The account seq ID.</param>
        ''' <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        Public Function UpdateAllAccountsForRole(RoleSeqID As Integer, SecurityEntityID As Integer, Accounts As String(), AccountSeqID As Integer) As Boolean
            Return m_BRoles.UpdateAllAccountsForRole(RoleSeqID, SecurityEntityID, Accounts, AccountSeqID)
        End Function

    End Class
End Namespace
