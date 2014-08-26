Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base

Namespace DataAccessLayer.Interfaces
    Public Interface IDFunction
        Inherits IDDBInteraction

        ''' <summary>
        ''' Used by all methds and must be set to send parameters to the datastore
        ''' </summary>
        Property Profile() As MFunctionProfile

        ''' <summary>
        ''' Used by all methds and must be set to send parameters to the datastore
        ''' </summary>
        Property SecurityEntitySeqID() As Integer

        ''' <summary>
        ''' Deletes an account
        ''' </summary>
        Sub Delete(ByVal functionSeqId As Integer)

        ''' <summary>
        ''' Retrieves Function information
        ''' </summary>
        ''' <returns>DataRow</returns>
        ReadOnly Property GetFunction() As DataRow

        ''' <summary>
        ''' Returns all functions associated with a given SecurityEntitySeqID.
        ''' </summary>
        ''' <returns>DataTable</returns>
        ''' <remarks>Does not caculate security for accounts.</remarks>
        ReadOnly Property GetFunctions() As DataSet

        Function GetFunctionTypes() As DataTable

        ''' <summary>
        ''' Gets the menu order.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns>DataTable.</returns>
        Function GetMenuOrder(ByRef profile As MFunctionProfile) As DataTable

        ''' <summary>
        ''' Saves this instance.
        ''' </summary>
        ''' <returns>System.Int32.</returns>
        Function Save() As Integer

        ''' <summary>
        ''' Save groups by passing a string or comma seporated groups to the database.
        ''' </summary>
        Sub SaveGroups(ByVal permission As PermissionType)

        ''' <summary>
        ''' Save roles by passing a string or comma seporated roles to the database.
        ''' </summary>
        Sub SaveRoles(ByVal permission As PermissionType)

        ''' <summary>
        ''' Returns a data table based on the search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">MSearchCriteria</param>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
    End Interface
End Namespace
