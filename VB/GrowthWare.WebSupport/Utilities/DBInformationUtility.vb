Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common

Namespace Utilities
    Public Class DBInformationUtility
        Public Shared Function DBInformation() As MDBInformation
            Dim mBLL As BDBInformation = New BDBInformation(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            Return mBLL.GetProfile()
        End Function

        Public Shared Function UpdateProfile(profile As MDBInformation) As Boolean
            Dim success As Boolean = False
            Dim mBLL As BDBInformation = New BDBInformation(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            success = mBLL.UpdateProfile(profile)
            Return success
        End Function
    End Class
End Namespace
