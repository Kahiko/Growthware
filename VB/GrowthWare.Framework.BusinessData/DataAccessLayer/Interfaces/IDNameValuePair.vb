Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.Interfaces
    Public Interface IDNameValuePair
        Inherits IDDBInteraction

        ''' <summary>
        ''' Gets or sets the name value pair profile.
        ''' </summary>
        ''' <value>The name value pair profile.</value>
        Property NameValuePairProfile() As MNameValuePair

        ''' <summary>
        ''' Gets or sets the S e_ SE q_ ID.
        ''' </summary>
        ''' <value>The S e_ SE q_ ID.</value>
        Property SecurityEntityId() As Integer

        ''' <summary>
        ''' Gets or sets the account ID.
        ''' </summary>
        ''' <value>The account ID.</value>
        Property AccountId() As Integer

        ''' <summary>
        ''' Gets or sets the detail profile.
        ''' </summary>
        ''' <value>The detail profile.</value>
        Property DetailProfile() As MNameValuePairDetail

        ''' <summary>
        ''' Deletes the NVP detail.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Sub DeleteNameValuePairDetail(ByVal profile As MNameValuePairDetail)

        ''' <summary>
        ''' Gets the NVP detail.
        ''' </summary>
        ''' <returns>DataRow.</returns>
        Function GetNameValuePairDetail() As DataRow

        ''' <summary>
        ''' Gets the NVP details.
        ''' </summary>
        ''' <param name="nameValuePairSeqDetailId">The NVP seq det ID.</param>
        ''' <param name="nameValuePairSeqId">The NVP seq ID.</param>
        ''' <returns>DataRow.</returns>
        Function GetNameValuePairDetails(ByVal nameValuePairSeqDetailId As Integer, ByVal nameValuePairSeqId As Integer) As DataRow

        ''' <summary>
        ''' Gets all NVP detail.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Function GetAllNameValuePairDetail() As DataTable

        ''' <summary>
        ''' Gets all NVP detail.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The NVP seq ID.</param>
        ''' <returns>DataTable.</returns>
        Function GetAllNameValuePairDetail(ByVal nameValuePairSeqId As Integer) As DataTable

        ''' <summary>
        ''' Gets the groups.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        ''' <returns>DataTable.</returns>
        Function GetGroups(ByVal nameValuePairSeqId As Integer) As DataTable

        ''' <summary>
        ''' Gets the roles.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        ''' <returns>DataTable.</returns>
        Function GetRoles(ByVal nameValuePairSeqId As Integer) As DataTable

        ''' <summary>
        ''' Saves the NVP detail.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Sub SaveNameValuePairDetail(ByVal profile As MNameValuePairDetail)

        ''' <summary>
        ''' Searches the specified search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>DataTable.</returns>
        Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable

        ''' <summary>
        ''' Updates the groups.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">Integer</param>
        ''' <param name="securityEntityId">Integer</param>
        ''' <param name="commaSeparatedGroups">Comma separated string of group names</param>
        ''' <param name="profile">MNameValuePair</param>
        ''' <remarks></remarks>
        Sub UpdateGroups(ByVal nameValuePairSeqId As Integer, ByVal securityEntityId As Integer, ByVal commaSeparatedGroups As String, ByVal profile As MNameValuePair)

        ''' <summary>
        ''' Updates the roles.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">int or Integer</param>
        ''' <param name="securityEntityId">int or Integer</param>
        ''' <param name="commaSeparatedRoles">Comma separated string of role names</param>
        ''' <param name="profile">MNameValuePair</param>
        ''' <remarks></remarks>
        Sub UpdateRoles(ByVal nameValuePairSeqId As Integer, ByVal securityEntityId As Integer, ByVal commaSeparatedRoles As String, ByVal profile As MNameValuePair)

        ''' <summary>
        ''' Gets all NVP.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Function AllNameValuePairs() As DataTable

        ''' <summary>
        ''' Gets the NVP.
        ''' </summary>
        ''' <returns>DataRow.</returns>
        Function GetNameValuePair() As DataRow

        ''' <summary>
        ''' Saves this instance.
        ''' </summary>
        ''' <returns>System.Int32.</returns>
        Function Save() As Integer
    End Interface
End Namespace
