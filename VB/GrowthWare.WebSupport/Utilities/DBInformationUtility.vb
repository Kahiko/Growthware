Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common

Namespace Utilities
    ''' <summary>
    ''' DBInformationUtility serves as the focal point for any web application needing to utilize the GrowthWare framework.
    ''' Web needs such as caching are handled here.
    ''' </summary>
    Public Class DBInformationUtility
        ''' <summary>
        ''' New instance of the class
        ''' </summary>
        ''' <returns>MDBInformation</returns>
        Public Shared Function DBInformation() As MDBInformation
            Dim mBLL As BDBInformation = New BDBInformation(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            Return mBLL.GetProfile
        End Function
        ''' <summary>
        ''' Updated the information in the data store
        ''' </summary>
        ''' <param name="profile">MDBInformation</param>
        ''' <returns>bool or exception</returns>
        Public Shared Function UpdateProfile(profile As MDBInformation) As Boolean
            Dim success As Boolean = False
            Dim mBLL As BDBInformation = New BDBInformation(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            success = mBLL.UpdateProfile(profile)
            Return success
        End Function
    End Class
End Namespace
