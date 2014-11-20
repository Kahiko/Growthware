Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.Interfaces
    Public Interface IDRoles
        Inherits IDDBInteraction

        Property SecurityEntitySeqId As Integer
        Property Profile As MRoleProfile

        ''' <summary>
        ''' Gets the accounts in role.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Function GetAccountsInRole() As DataTable

        ''' <summary>
        ''' Gets the accounts not in role.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Function GetAccountsNotInRole() As DataTable

        ''' <summary>
        ''' Updates all accounts for role.
        ''' </summary>
        ''' <param name="roleSeqID">The role seq ID.</param>
        ''' <param name="securityEntityID">The security entity ID.</param>
        ''' <param name="accounts">The accounts.</param>
        ''' <param name="accountSeqID">The account seq ID.</param>
        ''' <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        Function UpdateAllAccountsForRole(ByVal roleSeqId As Integer, ByVal securityEntityId As Integer, ByVal accounts() As String, ByVal accountSeqId As Integer) As Boolean

        ''' <summary>
        ''' Saves this instance.
        ''' </summary>
        Sub Save()

        ''' <summary>
        ''' Searches the specified search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>DataTable.</returns>
        Function Search(ByRef searchCriteria As MSearchCriteria) As DataTable

        ''' <summary>
        ''' Deletes the role.
        ''' </summary>
        Sub DeleteRole()

        ''' <summary>
        ''' Gets the profile data.
        ''' </summary>
        ''' <returns>DataRow.</returns>
        Function GetProfileData() As DataRow

        ''' <summary>
        ''' Gets the roles by security entity.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Function GetRolesBySecurityEntity() As DataTable
    End Interface
End Namespace
