Imports System.Collections.ObjectModel
Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Common
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Enumerations

Namespace Controllers
    Public Class FunctionsController
        Inherits ApiController

        <HttpPost>
        Public Function Delete(<FromUri()> ByVal functionSeqID As Integer) As IHttpActionResult
            If functionSeqID < 1 Then Throw New ArgumentNullException("functionSeqID", "functionSeqID must be a positive number!")
            Dim mRetVal As String = False
            Dim mLog As Logger = Logger.Instance()
            If Not HttpContext.Current.Items("EditId") Is Nothing Then
                Dim mEditId = Integer.Parse(HttpContext.Current.Items("EditId").ToString())
                If mEditId = functionSeqID Then
                    Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditFunction", True)), AccountUtility.CurrentProfile())
                    If Not mSecurityInfo Is Nothing Then
                        If mSecurityInfo.MayDelete Then
                            Try
                                FunctionUtility.Delete(functionSeqID)
                                mRetVal = True
                            Catch ex As Exception
                                mLog.Error(ex)
                            End Try
                        Else
                            Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to delete")
                            mLog.Error(mError)
                            Return Me.InternalServerError(mError)
                        End If
                    Else
                        Dim mError As Exception = New Exception("Security Info is not in context nothing has been saved!!!!")
                        mLog.Error(mError)
                        Return Me.InternalServerError(mError)
                    End If
                Else
                    Dim mError As Exception = New Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!")
                    mLog.Error(mError)
                    Return Me.InternalServerError(mError)
                End If
            Else
                Dim mError As Exception = New Exception("Can not verify the identifier you are trying to work with!")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            End If
            Return Me.Ok(mRetVal)
        End Function

        <HttpGet>
        Public Function GetFunctionData() As Collection(Of FunctionInformation)
            Dim mFunctionInformation As FunctionInformation = Nothing
            Dim mRetVal As Collection(Of FunctionInformation) = New Collection(Of FunctionInformation)
            Dim mFunctions As Collection(Of MFunctionProfile) = FunctionUtility.Functions()
            Dim mAppName As String = ConfigSettings.AppName
            If mAppName.Length <> 0 Then
                mAppName = "/" + mAppName + "/"
            End If
            For Each mProfile In mFunctions
                If mProfile.FunctionTypeSeqId <> 2 Then
                    mFunctionInformation = New FunctionInformation()
                    mFunctionInformation.Action = mProfile.Action
                    mFunctionInformation.Location = mAppName + mProfile.Source
                    mFunctionInformation.Description = mProfile.Description
                    mFunctionInformation.LinkBehavior = mProfile.LinkBehavior
                    mRetVal.Add(mFunctionInformation)
                End If
            Next

            Return mRetVal
        End Function

        <HttpGet>
        Public Function GetFunctionOrder(<FromUri()> ByVal functionSeqId As Integer) As List(Of UIFunctionMenuOrder)
            Dim mRetVal As List(Of UIFunctionMenuOrder) = New List(Of UIFunctionMenuOrder)
            If functionSeqId > 0 Then
                Dim mProfile As MFunctionProfile = FunctionUtility.GetProfile(functionSeqId)
                Dim mDataView As DataView = FunctionUtility.GetFunctionMenuOrder(mProfile).DefaultView
                For Each mRow As DataRowView In mDataView
                    Dim mItem As UIFunctionMenuOrder = New UIFunctionMenuOrder()
                    mItem.Function_Seq_Id = mRow("Function_Seq_Id").ToString()
                    mItem.Name = mRow("Name").ToString()
                    mItem.Action = mRow("Action").ToString()
                    mRetVal.Add(mItem)
                Next
            End If
            Return mRetVal
        End Function

        <HttpPost>
        Public Function MoveMenu(<FromUri()> ByVal functionSeqId As Integer, <FromUri()> ByVal direction As String) As Boolean
            Dim mRetVal As Boolean = False
            Dim mProfile As MFunctionProfile = FunctionUtility.GetProfile(functionSeqId)
            Dim mAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
            Try
                If direction = "up" Then
                    FunctionUtility.Move(mProfile, DirectionType.Up, mAccountProfile.Id, DateTime.Now)
                Else
                    FunctionUtility.Move(mProfile, DirectionType.Down, mAccountProfile.Id, DateTime.Now)
                End If
                mRetVal = True
            Catch ex As Exception
                Dim mLogger As Logger = Logger.Instance()
                mLogger.Error(ex)
            End Try
            Return mRetVal
        End Function

        <HttpPost>
        Public Function Save(ByVal uiProfile As UIFunctionProfile) As IHttpActionResult
            If uiProfile Is Nothing Then Throw New ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mLog As Logger = Logger.Instance()
            Dim mRetVal As String = "false"
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditFunction", True)), AccountUtility.CurrentProfile())

            If Not HttpContext.Current.Items("EditId") Is Nothing Then
                Dim mEditId = Integer.Parse(HttpContext.Current.Items("EditId").ToString())
                If mEditId = uiProfile.Id Then
                    Dim accountProfile As MAccountProfile = AccountUtility.CurrentProfile()
                    Dim profile As MFunctionProfile = New MFunctionProfile()
                    Dim directoryProfile As MDirectoryProfile = Nothing
                    If Not uiProfile.Id = -1 Then
                        If Not mSecurityInfo.MayAdd Then
                            Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to add")
                            mLog.Error(mError)
                            Return Me.InternalServerError(mError)
                        End If
                        profile = FunctionUtility.GetProfile(uiProfile.Id)
                        profile.UpdatedBy = accountProfile.Id
                        profile.UpdatedDate = DateTime.Now
                    Else
                        If Not mSecurityInfo.MayEdit Then
                            Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to edit")
                            mLog.Error(mError)
                            Return Me.InternalServerError(mError)
                        End If
                        profile.AddedBy = accountProfile.Id
                        profile.AddedDate = DateTime.Now
                    End If
                    Dim viewCommaRoles As String = String.Join(",", uiProfile.RolesAndGroups.ViewRoles)
                    Dim addCommaRoles As String = String.Join(",", uiProfile.RolesAndGroups.AddRoles)
                    Dim editCommaRoles As String = String.Join(",", uiProfile.RolesAndGroups.EditRoles)
                    Dim deleteCommaRoles As String = String.Join(",", uiProfile.RolesAndGroups.DeleteRoles)

                    Dim viewCommaGroups As String = String.Join(",", uiProfile.RolesAndGroups.ViewGroups)
                    Dim addCommaGroups As String = String.Join(",", uiProfile.RolesAndGroups.AddGroups)
                    Dim editCommaGroups As String = String.Join(",", uiProfile.RolesAndGroups.EditGroups)
                    Dim deleteCommaGroups As String = String.Join(",", uiProfile.RolesAndGroups.DeleteGroups)

                    Dim saveGroups As Boolean = False
                    Dim saveRoles As Boolean = False

                    If Not profile.GetCommaSeparatedAssignedRoles(PermissionType.View) = viewCommaRoles Then
                        profile.SetAssignedRoles(viewCommaRoles, PermissionType.View)
                        saveRoles = True
                    End If

                    If Not profile.GetCommaSeparatedAssignedRoles(PermissionType.Add) = addCommaRoles Then
                        profile.SetAssignedRoles(addCommaRoles, PermissionType.Add)
                        saveRoles = True
                    End If

                    If Not profile.GetCommaSeparatedAssignedRoles(PermissionType.Edit) = editCommaRoles Then
                        profile.SetAssignedRoles(editCommaRoles, PermissionType.Edit)
                        saveRoles = True
                    End If

                    If Not profile.GetCommaSeparatedAssignedRoles(PermissionType.Delete) = deleteCommaRoles Then
                        profile.SetAssignedRoles(deleteCommaRoles, PermissionType.Delete)
                        saveRoles = True
                    End If

                    If Not profile.GetCommaSeparatedGroups(PermissionType.Add) = addCommaGroups Then
                        profile.SetGroups(addCommaGroups, PermissionType.Add)
                        saveGroups = True
                    End If

                    If Not profile.GetCommaSeparatedGroups(PermissionType.Delete) = deleteCommaGroups Then
                        profile.SetGroups(deleteCommaGroups, PermissionType.Delete)
                        saveGroups = True
                    End If

                    If Not profile.GetCommaSeparatedGroups(PermissionType.Edit) = editCommaGroups Then
                        profile.SetGroups(editCommaGroups, PermissionType.Edit)
                        saveGroups = True
                    End If

                    If Not profile.GetCommaSeparatedGroups(PermissionType.View) = viewCommaGroups Then
                        profile.SetGroups(viewCommaGroups, PermissionType.View)
                        saveGroups = True
                    End If

                    profile.Action = uiProfile.Action
                    profile.EnableNotifications = uiProfile.EnableNotifications
                    profile.EnableViewState = uiProfile.EnableViewState
                    profile.FunctionTypeSeqId = uiProfile.FunctionTypeSeqID
                    profile.Id = uiProfile.Id
                    profile.IsNavigable = uiProfile.IsNav
                    profile.LinkBehavior = uiProfile.LinkBehavior
                    profile.MetaKeywords = uiProfile.MetaKeyWords
                    profile.Name = uiProfile.Name
                    profile.NavigationTypeSeqId = uiProfile.NavigationTypeSeqId
                    profile.Notes = uiProfile.Notes
                    profile.NoUI = uiProfile.NoUI
                    profile.ParentId = uiProfile.ParentID
                    profile.Source = uiProfile.Source
                    profile.Controller = uiProfile.Controller
                    profile.Description = uiProfile.Description
                    profile.RedirectOnTimeout = uiProfile.RedirectOnTimeout
                    FunctionUtility.Save(profile, saveGroups, saveRoles)
                    profile = FunctionUtility.GetProfile(uiProfile.Action)
                    directoryProfile = DirectoryUtility.GetProfile(profile.Id)
                    If Not String.IsNullOrEmpty(uiProfile.DirectoryData.Directory) Then
                        If directoryProfile Is Nothing Then directoryProfile = New MDirectoryProfile()
                        directoryProfile.FunctionSeqId = profile.Id
                        directoryProfile.Directory = uiProfile.DirectoryData.Directory
                        directoryProfile.Impersonate = uiProfile.DirectoryData.Impersonate
                        directoryProfile.ImpersonateAccount = uiProfile.DirectoryData.ImpersonateAccount
                        directoryProfile.ImpersonatePassword = uiProfile.DirectoryData.ImpersonatePassword
                        directoryProfile.Name = uiProfile.DirectoryData.Directory
                        directoryProfile.UpdatedBy = accountProfile.Id
                        DirectoryUtility.Save(directoryProfile)
                    Else
                        If Not directoryProfile Is Nothing Then
                            directoryProfile.Directory = ""
                            directoryProfile.Name = ""
                            DirectoryUtility.Save(directoryProfile)
                        End If
                    End If
                    AccountUtility.RemoveInMemoryInformation(True)
                    mRetVal = "true"
                Else
                    Dim mError As Exception = New Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!")
                    mLog.Error(mError)
                    Return Me.InternalServerError(mError)
                End If
            Else
                Dim mError As Exception = New Exception("Identifier could not be determined, nothing has been saved!!!!")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            End If

            Return Ok(mRetVal)
        End Function
    End Class

    Public Class UIFunctionProfile
        Public Action As String
        Public Description As String
        Public DirectoryData As UIDirectoryProfile
        Public EnableNotifications As Boolean
        Public EnableViewState As Boolean
        Public FunctionTypeSeqID As Integer
        Public Id As Integer
        Public IsNav As Boolean
        Public LinkBehavior As Integer
        Public MetaKeyWords As String
        Public Name As String
        Public NavigationTypeSeqId As Integer
        Public NoUI As Boolean
        Public Notes As String
        Public ParentID As Integer
        Public RedirectOnTimeout As Boolean
        Public RolesAndGroups As UIFunctionRolesGroups
        Public Source As String
        Public Controller As String
    End Class

    Public Class UIDirectoryProfile
        Public Directory As String
        Public Impersonate As Boolean
        Public ImpersonateAccount As String
        Public ImpersonatePassword As String

    End Class

    Public Class UIFunctionRolesGroups
        Public ViewRoles() As String
        Public AddRoles() As String
        Public EditRoles() As String
        Public DeleteRoles() As String
        Public ViewGroups() As String
        Public AddGroups() As String
        Public EditGroups() As String
        Public DeleteGroups() As String
    End Class

    Public Class FunctionInformation
        Public Property Action() As String
        Public Property Location() As String
        Public Property Description() As String
        Public Property LinkBehavior As Integer
    End Class

    Public Class UIFunctionMenuOrder
        Public Function_Seq_Id As String
        Public Action As String
        Public Name As String
    End Class
End Namespace