Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities

Namespace Controllers
    Public Class GroupsController
        Inherits ApiController

        <HttpPost>
        Public Function Delete(<FromUri> groupSeqId As Integer) As IHttpActionResult
            Dim mLog As Logger = Logger.Instance()
            Dim mRetVal As String = "false"
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_Manage_Groups", True)), AccountUtility.CurrentProfile())
            If Not mSecurityInfo.MayDelete Then
                Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to delete")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            Else
                If Not HttpContext.Current.Items("EditId") = Nothing Then
                    Dim mEditId As Integer = Integer.Parse(HttpContext.Current.Items("EditId").ToString())
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
        Public Function Save(ByVal profile As MUIGroupProfile) As IHttpActionResult
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mLog As Logger = Logger.Instance()
            Dim mRetVal As String = "false"
            Dim mProfileToSave As MGroupProfile = New MGroupProfile()
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_Manage_Groups", True)), AccountUtility.CurrentProfile())
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
                    mProfileToSave = GroupUtility.GetProfile(profile.Id)
                    mProfileToSave.UpdatedBy = AccountUtility.CurrentProfile().Id
                    mProfileToSave.UpdatedDate = DateAndTime.Now
                End If
            End If
            mProfileToSave = populateProfile(profile)
            GroupUtility.Save(mProfileToSave)
            Return Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function SaveMembers(ByVal groupAccounts As UIAccounts) As IHttpActionResult
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
            If HttpContext.Current.Items("EditId") Is Nothing Or HttpContext.Current.Items("EditId").ToString().ToLowerInvariant() <> groupAccounts.SeqId.ToString().ToLowerInvariant() Then
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
            Dim mRetVal As MGroupProfile = New MGroupProfile()
            mRetVal.Name = profile.Name
            mRetVal.Description = profile.Description
            mRetVal.Id = profile.Id
            mRetVal.SecurityEntityId = SecurityEntityUtility.CurrentProfile().Id
            Return mRetVal
        End Function
    End Class

    Public Class MUIGroupProfile

#Region "Public Properties"
        Public Property Id As Integer = -1

        Public Property Name() As String

        ''' <summary>
        ''' Gets or sets the description.
        ''' </summary>
        ''' <value>The description.</value>
        Public Property Description() As String
#End Region
    End Class
End Namespace