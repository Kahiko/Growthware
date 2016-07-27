Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities

Namespace Controllers
    Public Class RolesController
        Inherits ApiController

        <HttpPost>
        Public Function Delete(<FromUri> roleSeqId As Integer) As IHttpActionResult
            Dim mLog As Logger = Logger.Instance()
            Dim mRetVal As String = "false"
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditRoles", True)), AccountUtility.CurrentProfile())
            If Not mSecurityInfo.MayDelete Then
                Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to delete")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            Else
                If Not HttpContext.Current.Items("EditId") = Nothing Then
                    Dim mEditId As Integer = Integer.Parse(HttpContext.Current.Items("EditId").ToString())
                    If mEditId = roleSeqId Then
                        Dim mProfile As MRoleProfile = RoleUtility.GetProfile(roleSeqId)
                        RoleUtility.DeleteRole(mProfile)
                    Else
                        Dim mError As Exception = New Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!")
                        mLog.Error(mError)
                        Return Me.InternalServerError(mError)
                    End If
                Else
                    Dim mError As Exception = New Exception("The identifier unknown and nothing has been saved!!!!")
                    mLog.Error(mError)
                    Return Me.InternalServerError(mError)
                End If
            End If
            Return Ok(mRetVal)
        End Function

        <HttpGet>
        Public Function GetRoles() As IHttpActionResult
            Dim mRetVal As ArrayList = Nothing
            mRetVal = RoleUtility.GetRolesArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id)
            Return Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function GetSearchResults(<FromBody> searchCriteria As MSearchCriteria) As IHttpActionResult
            Dim mDataTable As DataTable = Nothing
            Try
                mDataTable = RoleUtility.Search(searchCriteria)
            Catch ex As Exception
                ' Only going to log message
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
            End Try
            Return Ok(mDataTable)
        End Function

        <HttpPost>
        Public Function Save(ByVal profile As MUIRoleProfile) As IHttpActionResult
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mLog As Logger = Logger.Instance()
            Dim mRetVal As String = "false"
            Dim mProfileToSave As MRoleProfile = New MRoleProfile()
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditRoles", True)), AccountUtility.CurrentProfile())
            If Not HttpContext.Current.Items("EditId") Is Nothing Then
                If profile.Id = -1 Then
                    If Not mSecurityInfo.MayAdd Then
                        Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to add")
                        mLog.Error(mError)
                        Return Me.InternalServerError(mError)
                    End If
                    mProfileToSave.AddedBy = AccountUtility.CurrentProfile().Id
                    mProfileToSave.AddedDate = DateAndTime.Now
                Else
                    If Not mSecurityInfo.MayEdit Then
                        Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to edit")
                        mLog.Error(mError)
                        Return Me.InternalServerError(mError)
                    End If
                    mProfileToSave = RoleUtility.GetProfile(profile.Id)
                    If mProfileToSave.IsSystem Then
                        mProfileToSave.IsSystem = True
                    End If
                    If mProfileToSave.IsSystemOnly Then
                        mProfileToSave.IsSystemOnly = True
                    End If
                    mProfileToSave.UpdatedBy = AccountUtility.CurrentProfile().Id
                    mProfileToSave.UpdatedDate = DateAndTime.Now
                End If
            End If
            mProfileToSave = populateProfile(profile)
            RoleUtility.Save(mProfileToSave)
            Return Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function SaveMembers(ByVal roleAccounts As MUIAccounts) As IHttpActionResult
            If roleAccounts Is Nothing Then Throw New ArgumentNullException("roleAccounts", "roleAccounts cannot be a null reference (Nothing in Visual Basic)!")
            Dim mLog As Logger = Logger.Instance()
            Dim mRetVal As String = "false"
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditRoles", True)), AccountUtility.CurrentProfile())
            If Not mSecurityInfo.MayEdit Then
                Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to add")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            End If
            If HttpContext.Current.Items("EditId") Is Nothing Then
                Dim mError As Exception = New Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            End If
            Dim accountProfile As MAccountProfile = AccountUtility.CurrentProfile()
            Dim MClientChoicesState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(accountProfile.Account)
            Dim success As Boolean = RoleUtility.UpdateAllAccountsForRole(roleAccounts.SeqId, Integer.Parse(MClientChoicesState(MClientChoices.SecurityEntityId)), roleAccounts.Accounts, accountProfile.Id)
            Return Ok(mRetVal)
        End Function

        Private Function populateProfile(ByVal profile As MUIRoleProfile) As MRoleProfile
            Dim mRetVal As MRoleProfile = New MRoleProfile()
            mRetVal.Name = profile.Name
            mRetVal.Description = profile.Description
            mRetVal.Id = profile.Id
            mRetVal.IsSystem = profile.IsSystem
            mRetVal.IsSystemOnly = profile.IsSystemOnly
            mRetVal.SecurityEntityId = SecurityEntityUtility.CurrentProfile().Id
            Return mRetVal
        End Function

    End Class
End Namespace