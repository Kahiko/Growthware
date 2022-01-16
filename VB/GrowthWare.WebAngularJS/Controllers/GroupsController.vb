Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities

Namespace Controllers
    Public Class GroupsController
        Inherits ApiController

        ' GET api/Groups/Delete?groupId=
        <HttpPost>
        Public Function Delete(<FromUri> groupSeqId As Integer) As IHttpActionResult
            Dim mLog As Logger = Logger.Instance()
            Dim mRetVal As String = "false"
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditGroups", True)), AccountUtility.CurrentProfile())
            If Not mSecurityInfo.MayDelete Then
                Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to delete")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            Else
                If Not HttpContext.Current.Session("EditId") = Nothing Then
                    Dim mEditId As Integer = Integer.Parse(HttpContext.Current.Session("EditId").ToString())
                    If mEditId = groupSeqId Then
                        Dim mProfile As MGroupProfile = GroupUtility.GetProfile(groupSeqId)
                        GroupUtility.Delete(mProfile)
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

        <HttpPost>
        Public Function GetSearchResults(<FromBody> searchCriteria As MSearchCriteria) As IHttpActionResult
            Dim mDataTable As DataTable = Nothing
            Try
                mDataTable = GroupUtility.Search(searchCriteria)
            Catch ex As Exception
                ' Only going to log message
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
            End Try
            Return Ok(mDataTable)
        End Function

        ' GET api/Groups/GetGroupForEdit?groupId=
        <HttpGet>
        Public Function GetGroupForEdit(<FromUri> ByVal groupId As Integer) As MUIGroupProfile
            Dim mRetVal As New MUIGroupProfile()
            Dim mProfile As MGroupProfile = GroupUtility.GetProfile(groupId)
            Dim mGroupRoles As New MGroupRoles
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            With mGroupRoles
                .GroupSeqId = groupId
                .SecurityEntityId = mSecurityEntityProfile.Id
            End With
            If (groupId <> -1) Then
                mRetVal.RolesInGroup = GroupUtility.GetSelectedRoles(mGroupRoles)
            End If
            With mRetVal
                .Description = mProfile.Description
                .Id = mProfile.Id
                .Name = mProfile.Name
                .RolesNotInGroup = RoleUtility.GetRolesArrayListBySecurityEntity(mSecurityEntityProfile.Id).ToArray(Type.GetType("System.String"))
            End With
            HttpContext.Current.Session.Add("EditId", mRetVal.Id)
            Return mRetVal
        End Function

        ' GET api/Groups/GetGroupForEdit?groupId=
        <HttpGet>
        Public Function GetGroup(groupId) As IHttpActionResult
            Dim mRetVal As MGroupProfile = GroupUtility.GetProfile(groupId)
            Return Ok(mRetVal)
        End Function

        <HttpGet>
        Public Function GetGroups() As IHttpActionResult
            Dim mRetVal As ArrayList
            mRetVal = GroupUtility.GetGroupsArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id)
            Return Ok(mRetVal)
        End Function

        ' GET api/Groups/Save?Action= - profile
        <HttpPost>
        Public Function Save(ByVal profile As MUIGroupProfile) As IHttpActionResult
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mLog As Logger = Logger.Instance()
            Dim mProfileToSave As MGroupProfile = populateProfile(profile)
            Dim mSecurityInfo As New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditGroups", True)), AccountUtility.CurrentProfile())
            Dim mGroupSeqId As Integer
            If HttpContext.Current.Session("EditId") IsNot Nothing Then
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
                        Dim mError As New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to edit")
                        mLog.Error(mError)
                        Return Me.InternalServerError(mError)
                    End If
                    mProfileToSave = populateProfile(profile)
                    mProfileToSave.UpdatedBy = AccountUtility.CurrentProfile().Id
                    mProfileToSave.UpdatedDate = DateAndTime.Now
                End If
            End If

            mGroupSeqId = GroupUtility.Save(mProfileToSave)
            If (profile.Id = -1) Then
                HttpContext.Current.Session("EditId") = mGroupSeqId
            End If
            Dim mGroupAccounts As New MUIAccounts
            mGroupAccounts.SeqId = mGroupSeqId
            mGroupAccounts.Accounts = profile.RolesInGroup
            SaveMembers(mGroupAccounts)
            Return Ok(True)
        End Function

        <HttpPost>
        Public Function SaveMembers(ByVal groupAccounts As MUIAccounts) As IHttpActionResult
            If groupAccounts Is Nothing Then Throw New ArgumentNullException("groupAccounts", "groupAccounts cannot be a null reference (Nothing in Visual Basic)!")
            Dim mLog As Logger = Logger.Instance()
            Dim mRetVal As String = "false"
            Dim mManageGroupsProfile As MFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditGroups", True))
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(mManageGroupsProfile, AccountUtility.CurrentProfile())
            If Not mSecurityInfo.MayEdit Then
                Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to add")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            End If
            If HttpContext.Current.Session("EditId") Is Nothing Or HttpContext.Current.Session("EditId").ToString().ToLowerInvariant() <> groupAccounts.SeqId.ToString().ToLowerInvariant() Then
                Dim mError As Exception = New Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            End If
            Dim accountProfile As MAccountProfile = AccountUtility.CurrentProfile()
            Dim MClientChoicesState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(accountProfile.Account)
            Dim mProfile As New MGroupRoles
            mProfile.SecurityEntityId = SecurityEntityUtility.CurrentProfile.Id
            mProfile.GroupSeqId = groupAccounts.SeqId
            mProfile.Roles = String.Join(",", groupAccounts.Accounts)
            mProfile.AddedUpdatedBy = accountProfile.Id
            GroupUtility.UpdateGroupRoles(mProfile)
            Return Ok(mRetVal)
        End Function

        Private Function populateProfile(ByVal profile As MUIGroupProfile) As MGroupProfile
            Dim mRetVal As New MGroupProfile With {
                .Name = profile.Name,
                .Description = profile.Description,
                .Id = profile.Id,
                .SecurityEntityId = SecurityEntityUtility.CurrentProfile().Id
            }
            Return mRetVal
        End Function
    End Class
End Namespace