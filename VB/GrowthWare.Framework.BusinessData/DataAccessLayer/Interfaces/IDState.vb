Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.Interfaces
    Public Interface IDState
        Inherits IDDBInteraction
        ''' <summary>
        ''' Used by all methods and must be set to send parameters to the data store
        ''' </summary>
        Property Profile() As MStateProfile

        Property SecurityEntitySeqId() As Integer

        ''' <summary>
        ''' Deletes an account
        ''' </summary>
        Sub Delete()

        ''' <summary>
        ''' Retrieves Account information
        ''' </summary>
        ''' <returns>DataRow</returns>
        ReadOnly Property GetState() As DataRow

        ''' <summary>
        ''' Returns all accounts associated with a given SecurityEntitySeqID.
        ''' </summary>
        ''' <returns>DataTable</returns>
        ''' <remarks>Does not caculate security for accounts.</remarks>
        ReadOnly Property GetStates() As DataTable

        ''' <summary>
        ''' Inserts or updates account information
        ''' </summary>
        Sub Save()

        ''' <summary>
        ''' Searches the specified search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>DataTable.</returns>
        Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
    End Interface
End Namespace
