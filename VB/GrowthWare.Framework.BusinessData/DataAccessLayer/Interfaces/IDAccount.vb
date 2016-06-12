Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.Interfaces
    Public Interface IDAccount
        Inherits IDDBInteraction
        ''' <summary>
        ''' Used by all methods and must be set to send parameters to the data store
        ''' </summary>
        Property Profile() As MAccountProfile

        ''' <summary>
        ''' Used by all methods and must be set to send parameters to the data store
        ''' </summary>
        Property SecurityEntitySeqId() As Integer

        ''' <summary>
        ''' Deletes an account
        ''' </summary>
        Sub Delete()

        ''' <summary>
        ''' Retrieves Account information
        ''' </summary>
        ''' <returns>DataRow</returns>
        ReadOnly Property GetAccount() As DataRow

        ''' <summary>
        ''' Returns all accounts associated with a given SecurityEntitySeqID.
        ''' </summary>
        ''' <returns>DataTable</returns>
        ''' <remarks>Does not caculate security for accounts.</remarks>
        ReadOnly Property GetAccounts() As DataTable

        ''' <summary>
        ''' Returns all roles associated with a given SecurityEntitySeqID.
        ''' </summary>
        ''' <returns>DataTable</returns>
        Function Groups() As DataTable

        ''' <summary>
        ''' Retrieves menu data for a given account and MenuType
        ''' </summary>
        ''' <param name="account">String</param>
        ''' <param name="menuType">MenuType</param>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Function GetMenu(ByVal account As String, ByVal menuType As MenuType) As DataTable

        ''' <summary>
        ''' Returns all groups associated with a given SecurityEntitySeqID.
        ''' </summary>
        ''' <returns>DataTable</returns>
        Function Roles() As DataTable

        ''' <summary>
        ''' Returns all roles either direct association or by association via
        ''' groups.
        ''' </summary>
        ''' <returns>DataTable</returns>
        Function Security() As DataTable

        ''' <summary>
        ''' Inserts or updates account information
        ''' </summary>
        ''' <returns>int</returns>
        Function Save() As Integer

        ''' <summary>
        ''' Save groups by passing a string or comma seporated groups to the database.
        ''' </summary>
        Sub SaveGroups()

        ''' <summary>
        ''' Save roles by passing a string or comma seporated roles to the database.
        ''' </summary>
        Sub SaveRoles()

        ''' <summary>
        ''' Searches the specified search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>DataTable.</returns>
        Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
    End Interface
End Namespace
