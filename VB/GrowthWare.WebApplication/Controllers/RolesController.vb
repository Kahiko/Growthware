Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities

Namespace Controllers
    Public Class RolesController
        Inherits ApiController

        <HttpPost>
        Public Function Save(ByVal profile As MRoleProfile) As IHttpActionResult
            If profile Is Nothing Then Throw New ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mLog As Logger = Logger.Instance()
            Dim mRetVal As String = "false"
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditRoles", True)), AccountUtility.CurrentProfile())
            If Not HttpContext.Current.Items("EditId") Is Nothing Then
                If profile.Id = -1 Then
                    If Not mSecurityInfo.MayAdd Then
                        Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to add")
                        mLog.Error(mError)
                        Return Me.InternalServerError(mError)
                    End If
                    profile.AddedBy = AccountUtility.CurrentProfile().Id
                    profile.AddedDate = DateAndTime.Now
                Else
                    If Not mSecurityInfo.MayEdit Then
                        Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to edit")
                        mLog.Error(mError)
                        Return Me.InternalServerError(mError)
                    End If
                    profile.UpdatedBy = AccountUtility.CurrentProfile().Id
                    profile.UpdatedDate = DateAndTime.Now
                End If
            End If
            If profile.IsSystem Then
                profile.IsSystem = True
            End If
            If profile.IsSystemOnly Then
                profile.IsSystemOnly = True
            End If
            RoleUtility.Save(profile)
            Return Ok(mRetVal)
        End Function
    End Class
End Namespace