Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.Interfaces
    Public Interface IDSecurityEntity
        Inherits IDDBInteraction

        ''' <summary>
        ''' Retrieves all Security Entities as a data table.
        ''' </summary>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Function GetSecurityEntities() As DataTable

        ''' <summary>
        ''' Retrieves security entities for a given account.
        ''' </summary>
        ''' <param name="account">String</param>
        ''' <param name="securityEntityID">int or Integer</param>
        ''' <param name="isSecurityEntityAdministrator">Boolean or bool</param>
        ''' <returns>Datatable</returns>
        ''' <remarks></remarks>
        Function GetSecurityEntities(ByVal account As String, ByVal securityEntityId As Integer, ByVal isSecurityEntityAdministrator As Boolean) As DataTable

        ''' <summary>
        ''' Gets the valid security entities.
        ''' </summary>
        ''' <param name="account">The account.</param>
        ''' <param name="securityEntityId">The security entity ID.</param>
        ''' <param name="isSecurityEntityAdministrator">if set to <c>true</c> [is security entity administrator].</param>
        ''' <returns>DataTable.</returns>
        Function GetValidSecurityEntities(ByVal account As String, ByVal securityEntityId As Integer, ByVal isSecurityEntityAdministrator As Boolean) As DataTable

        ''' <summary>
        ''' Saves security entity information to the datastore.
        ''' </summary>
        ''' <param name="profile">MSecurityEntityProfile</param>
        ''' <remarks></remarks>
        Function Save(ByVal profile As MSecurityEntityProfile) As Integer

        ''' <summary>
        ''' Searches the specified search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>DataTable.</returns>
        Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
    End Interface
End Namespace

