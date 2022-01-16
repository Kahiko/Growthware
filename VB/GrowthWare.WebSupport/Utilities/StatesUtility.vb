
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common

Namespace Utilities
    Public Class StatesUtility

        Public Shared Function Save(ByVal profile As MStateProfile) As Boolean
            Dim mBStates As BStates = New BStates(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBStates.Save(profile)
        End Function

        ''' <summary>
        ''' Searches the specified search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>DataTable.</returns>
        Public Shared Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be blank or a null reference (Nothing in Visual Basic)")
            Dim mBStates As BStates = New BStates(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBStates.Search(searchCriteria)
        End Function

        Public Shared Function GetProfile(ByVal state As String) As MStateProfile
            If state Is Nothing Or String.IsNullOrEmpty(state) Then Throw New ArgumentNullException("state", "state cannot be blank or a null reference (Nothing in Visual Basic)")
            Dim mBStates As BStates = New BStates(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBStates.GetProfile(state)
        End Function
    End Class
End Namespace