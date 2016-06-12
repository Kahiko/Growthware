Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.Interfaces
    ''' <summary>
    ''' Interface IDMessages
    ''' </summary>
    Public Interface IDMessages
        Inherits IDDBInteraction

        ''' <summary>
        ''' Gets or sets the profile.
        ''' </summary>
        ''' <value>The profile.</value>
        Property Profile() As MMessageProfile

        ''' <summary>
        ''' Gets or sets the security entity seq ID.
        ''' </summary>
        ''' <value>The security entity seq ID.</value>
        Property SecurityEntitySeqId As Integer

        ''' <summary>
        ''' Gets the messages.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Function Messages() As DataTable

        ''' <summary>
        ''' Gets the messages.
        ''' </summary>
        ''' <returns>DataTable.</returns>

        Function GetMessage(ByVal messageSeqId As Integer) As DataRow

        ''' <summary>
        ''' Saves this instance.
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
