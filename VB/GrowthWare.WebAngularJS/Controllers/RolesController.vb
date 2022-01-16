Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities

Namespace Controllers
    Public Class RolesController
        Inherits ApiController

        ' POST api/Roles/Delete?roleSeqId=
        <HttpPost>
        Public Function Delete(<FromUri> roleSeqId As Integer) As IHttpActionResult
            Dim mLog As Logger = Logger.Instance()
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditRoles", True)), AccountUtility.CurrentProfile())
            If Not mSecurityInfo.MayDelete Then
                Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to delete")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            Else
                If Not HttpContext.Current.Session("EditId") = Nothing Then
                    Dim mEditId As Integer = Integer.Parse(HttpContext.Current.Session("EditId").ToString())
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
            Return Ok(true)
        End Function

        ' GET api/Roles/GetRoleForEdit?roleSeqId=
        <HttpGet>
        Public Function GetRoleForEdit(<FromUri> ByVal roleSeqId As Integer) As IHttpActionResult
            Dim mRetVal As MUIRoleProfile = GetRoleProfile(roleSeqId)
            HttpContext.Current.Session.Add("EditId", mRetVal.Id)
            Return Ok(mRetVal)
        End Function

        Private Function GetRoleProfile(ByVal roleSeqId As Integer) As MUIRoleProfile
            Dim mProfile As MRoleProfile = New MRoleProfile()
            Dim mRetVal As MUIRoleProfile = New MUIRoleProfile()
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            mProfile.SecurityEntityId = mSecurityEntityProfile.Id
            If Not String.IsNullOrEmpty(roleSeqId) Then
                Dim mRoleID As Integer = Integer.Parse(roleSeqId)
                If Not mRoleID = -1 Then
                    mProfile = RoleUtility.GetProfile(mRoleID)
                    With mRetVal
                        .Description = mProfile.Description
                        .Id = mProfile.Id
                        .IsSystem = mProfile.IsSystem
                        .IsSystemOnly = mProfile.IsSystemOnly
                        .Name = mProfile.Name
                        .SecurityEntityId = mSecurityEntityProfile.Id
                    End With
                    mRetVal.AccountsInRole = RoleUtility.GetAccountsInRole(mProfile).ToArray(Type.GetType("System.String"))
                    mRetVal.AccountsNotInRole = RoleUtility.GetAccountsNotInRole(mProfile).ToArray(Type.GetType("System.String"))
                Else
                    mRetVal.AccountsNotInRole = RoleUtility.GetAllAccountsBySecurityEntity().ToArray(Type.GetType("System.String"))
                End If
            End If
            Return mRetVal
        End Function

        ' GET api/Roles/GetRole?roleSeqId=
        <HttpGet>
        Public Function GetRole(<FromUri> ByVal roleSeqId As Integer) As IHttpActionResult
            Return Ok(GetRoleProfile(roleSeqId))
        End Function

        ' POST api/Roles/GetRoles?Action=
        <HttpGet>
        Public Function GetRoles() As IHttpActionResult
            Dim mRetVal As ArrayList = Nothing
            mRetVal = RoleUtility.GetRolesArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id)
            Return Ok(mRetVal)
        End Function

        ' POST api/Roles/GetSearchResults?Action= - searchCriteria
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

        ' POST api/Roles/Save - profile
        <HttpPost>
        Public Function Save(ByVal profile As MUIRoleProfile) As IHttpActionResult
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mLog As Logger = Logger.Instance()
            Dim mProfileToSave As MRoleProfile = populateProfile(profile)
            Dim mSecurityInfo As New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditRoles", True)), AccountUtility.CurrentProfile())
            Dim mGropuSeqId As Integer
            If HttpContext.Current.Session("EditId") IsNot Nothing Then
                If profile.Id = -1 Then
                    If Not mSecurityInfo.MayAdd Then
                        Dim mError As New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to add")
                        mLog.Error(mError)
                        Return Me.InternalServerError(mError)
                    End If
                    mProfileToSave.AddedBy = AccountUtility.CurrentProfile().Id
                    mProfileToSave.AddedDate = DateAndTime.Now
                Else
                    If Not mSecurityInfo.MayEdit Then
                        Dim mError As New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to edit")
                        mLog.Error(mError)
                        Return Me.InternalServerError(mError)
                    End If
                    If profile.IsSystem Then
                        mProfileToSave.IsSystem = True
                    End If
                    If profile.IsSystemOnly Then
                        mProfileToSave.IsSystemOnly = True
                    End If
                    mProfileToSave.UpdatedBy = AccountUtility.CurrentProfile().Id
                    mProfileToSave.UpdatedDate = DateAndTime.Now
                End If
            End If
            mGropuSeqId = RoleUtility.Save(mProfileToSave)
            If (profile.AccountsInRole IsNot Nothing) Then
                Dim mRoleAccounts = New MUIAccounts()
                mRoleAccounts.SeqId = mGropuSeqId
                mRoleAccounts.Accounts = profile.AccountsInRole
                SaveMembers(mRoleAccounts)
            End If
            Return Ok(True)
        End Function

        ' POST api/Roles/SaveMembers - roleAccounts
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
            If HttpContext.Current.Session("EditId") Is Nothing Then
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
            With mRetVal
                .Name = profile.Name
                .Description = profile.Description
                .Id = profile.Id
                .SecurityEntityId = SecurityEntityUtility.CurrentProfile().Id
            End With
            Return mRetVal
        End Function

    End Class
End Namespace