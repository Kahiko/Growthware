Imports System.Collections.ObjectModel
Imports System.Web.Http
Imports System.Linq
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities

Namespace Controllers
    Public Class FunctionsController
        Inherits ApiController

        <HttpPost>
        Public Function Delete(<FromUri()> ByVal functionSeqID As Integer) As IHttpActionResult
            If functionSeqID < 1 Then Throw New ArgumentNullException("functionSeqID", "functionSeqID must be a positive number!")
            Dim mRetVal As String = False
            Dim mLog As Logger = Logger.Instance()
            If Not HttpContext.Current.Session("EditId") Is Nothing Then
                Dim mEditId = Integer.Parse(HttpContext.Current.Session("EditId").ToString())
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

        Public Function GetAvalibleParents(<FromUri()> ByVal functionSeqID As Integer) As IHttpActionResult
            Dim mRetVal As Collection(Of MUIFunctionProfile) = New Collection(Of MUIFunctionProfile)
            Dim mUIFunctionProfile As MUIFunctionProfile = Nothing
            For Each mProfile As MFunctionProfile In FunctionUtility.Functions()
                If (mProfile.Id <> functionSeqID) Then
                    mUIFunctionProfile = New MUIFunctionProfile()
                    mUIFunctionProfile.Action = mProfile.Action
                    mUIFunctionProfile.DerivedAddRoles = mProfile.DerivedAddRoles
                    mUIFunctionProfile.DerivedDeleteRoles = mProfile.DerivedDeleteRoles
                    mUIFunctionProfile.DerivedEditRoles = mProfile.DerivedEditRoles
                    mUIFunctionProfile.DerivedViewRoles = mProfile.DerivedViewRoles
                    mUIFunctionProfile.Description = mProfile.Description
                    'mUIFunctionProfile.DirectoryData = ???
                    mUIFunctionProfile.EnableNotifications = mProfile.EnableNotifications
                    mUIFunctionProfile.EnableViewState = mProfile.EnableViewState
                    mUIFunctionProfile.FunctionTypeSeqID = mProfile.FunctionTypeSeqId
                    mUIFunctionProfile.Id = mProfile.Id
                    mUIFunctionProfile.IsNav = mProfile.IsNavigable
                    mUIFunctionProfile.LinkBehavior = mProfile.LinkBehavior
                    mUIFunctionProfile.MetaKeyWords = mProfile.MetaKeywords
                    mUIFunctionProfile.Name = mProfile.Name
                    mUIFunctionProfile.NavigationTypeSeqId = mProfile.NavigationTypeSeqId
                    mUIFunctionProfile.NoUI = mProfile.NoUI
                    mUIFunctionProfile.Notes = mProfile.Notes
                    mUIFunctionProfile.ParentID = mProfile.ParentId
                    mUIFunctionProfile.RedirectOnTimeout = mProfile.RedirectOnTimeout
                    'mUIFunctionProfile.RolesAndGroups = ???
                    mUIFunctionProfile.Source = mProfile.Source
                    mUIFunctionProfile.Controller = mProfile.Controller
                    mRetVal.Add(mUIFunctionProfile)
                End If
            Next
            Return Ok(mRetVal)
        End Function

        <HttpGet>
        Public Function GetFunctionForEdit(<FromUri()> ByVal functionSeqID As Integer) As IHttpActionResult
            HttpContext.Current.Session("EditId") = functionSeqID
            Return Ok(Me.GetFunction(functionSeqID))
        End Function

        Private Function GetFunction(ByVal functionSeqId As Integer) As MUIFunctionProfile
            Dim mProfile As MFunctionProfile = Nothing
            Dim mRetVal As MUIFunctionProfile = New MUIFunctionProfile()
            Dim mFunctionSeqID As Integer = Integer.Parse(functionSeqId)
            Dim mDirectoryProfile As MDirectoryProfile = DirectoryUtility.GetProfile(mFunctionSeqID)
            Dim mDirectoryData = New MUIDirectoryProfile()
            If mDirectoryProfile IsNot Nothing Then
                With mDirectoryData
                    .Directory = mDirectoryProfile.Directory
                    .Impersonate = mDirectoryProfile.Impersonate
                    .ImpersonateAccount = mDirectoryProfile.ImpersonateAccount
                    .ImpersonatePassword = mDirectoryProfile.ImpersonatePassword
                End With
            End If

            If Not functionSeqId = -1 Then
                mProfile = FunctionUtility.GetProfile(mFunctionSeqID)
                With mRetVal
                    .Action = mProfile.Action
                    .Controller = mProfile.Controller
                    .Description = mProfile.Description
                    .DirectoryData = mDirectoryData
                    .EnableNotifications = mProfile.EnableNotifications
                    .EnableViewState = mProfile.EnableViewState
                    .FunctionTypeSeqID = mProfile.FunctionTypeSeqId
                    .Id = mProfile.Id
                    .IsNav = mProfile.IsNavigable
                    .LinkBehavior = mProfile.LinkBehavior
                    .MetaKeyWords = mProfile.MetaKeywords
                    .Name = mProfile.Name
                    .NavigationTypeSeqId = mProfile.NavigationTypeSeqId
                    .Notes = mProfile.Notes
                    .NoUI = mProfile.NoUI
                    .ParentID = mProfile.ParentId
                    .RedirectOnTimeout = mProfile.RedirectOnTimeout
                    .DerivedAddRoles = mProfile.DerivedAddRoles
                    .DerivedDeleteRoles = mProfile.DerivedDeleteRoles
                    .DerivedEditRoles = mProfile.DerivedEditRoles
                    .DerivedViewRoles = mProfile.DerivedViewRoles
                End With

                Dim mUIFunctionRolesGroups As MUIFunctionRolesGroups = New MUIFunctionRolesGroups
                mRetVal.RolesAndGroups = mUIFunctionRolesGroups

                mRetVal.RolesAndGroups.AddRoles = mProfile.AssignedAddRoles.ToArray()
                mRetVal.RolesAndGroups.DeleteRoles = mProfile.AssignedDeleteRoles.ToArray()
                mRetVal.RolesAndGroups.EditRoles = mProfile.AssignedEditRoles.ToArray()
                mRetVal.RolesAndGroups.ViewRoles = mProfile.AssignedViewRoles.ToArray()

                mRetVal.RolesAndGroups.AddGroups = mProfile.AddGroups.ToArray()
                mRetVal.RolesAndGroups.DeleteGroups = mProfile.DeleteGroups.ToArray()
                mRetVal.RolesAndGroups.EditGroups = mProfile.EditGroups.ToArray()
                mRetVal.RolesAndGroups.ViewGroups = mProfile.ViewGroups.ToArray()

                mRetVal.Source = mProfile.Source

            End If
            HttpContext.Current.Session("EditId") = mRetVal.Id
            Return mRetVal
        End Function

        <HttpGet>
        Public Function GetFunctionData() As Collection(Of MUIFunctionInformation)
            Dim mFunctionInformation As MUIFunctionInformation = Nothing
            Dim mRetVal As Collection(Of MUIFunctionInformation) = New Collection(Of MUIFunctionInformation)
            Dim mFunctions As Collection(Of MFunctionProfile) = FunctionUtility.Functions()
            Dim mAppName As String = ConfigSettings.AppName
            If mAppName.Length <> 0 Then
                mAppName = "/" + mAppName + "/"
            End If
            For Each mProfile In mFunctions
                If mProfile.FunctionTypeSeqId <> 2 Then
                    mFunctionInformation = New MUIFunctionInformation()
                    mFunctionInformation.Action = mProfile.Action
                    mFunctionInformation.Location = mAppName + mProfile.Source + "?Action=" + mProfile.Action
                    mFunctionInformation.Description = mProfile.Description
                    mFunctionInformation.LinkBehavior = mProfile.LinkBehavior
                    mFunctionInformation.Controller = mProfile.Controller
                    mRetVal.Add(mFunctionInformation)
                End If
            Next

            Return mRetVal
        End Function

        <HttpGet>
        Public Function GetFunctionOrder(<FromUri()> ByVal functionSeqId As Integer) As List(Of MUIFunctionMenuOrder)
            Dim mRetVal As List(Of MUIFunctionMenuOrder) = New List(Of MUIFunctionMenuOrder)
            If functionSeqId > 0 Then
                Dim mProfile As MFunctionProfile = FunctionUtility.GetProfile(functionSeqId)
                Dim mDataView As DataView = FunctionUtility.GetFunctionMenuOrder(mProfile).DefaultView
                For Each mRow As DataRowView In mDataView
                    Dim mItem As MUIFunctionMenuOrder = New MUIFunctionMenuOrder()
                    mItem.Function_Seq_Id = mRow("Function_Seq_Id").ToString()
                    mItem.Name = mRow("Name").ToString()
                    mItem.Action = mRow("Action").ToString()
                    mRetVal.Add(mItem)
                Next
            End If
            Return mRetVal
        End Function

        <HttpGet>
        Public Function GetLinkBehaviors() As IHttpActionResult
            Dim mDataTable As DataTable = Nothing
            Dim mNavType As Integer = GWWebHelper.LinkBehaviorNameValuePairSequenceId
            NameValuePairUtility.GetNameValuePairDetails(mDataTable, mNavType)
            Return Ok(mDataTable)
        End Function

        <HttpGet>
        Public Function GetNavigationTypes() As IHttpActionResult
            Dim mDataTable As DataTable = Nothing
            Dim mNavType As Integer = GWWebHelper.LinkBehaviorNavigationTypesSequenceId
            NameValuePairUtility.GetNameValuePairDetails(mDataTable, mNavType)
            Return Ok(mDataTable)
        End Function

        <HttpPost>
        Public Function GetSearchResults(<FromBody> searchCriteria As MSearchCriteria) As IHttpActionResult
            Dim mDataTable As DataTable = Nothing
            Try
                'searchCriteria.WhereClause = HttpUtility.UrlDecode(searchCriteria.WhereClause)
                mDataTable = FunctionUtility.Search(searchCriteria)
            Catch ex As Exception
                ' Only going to log message
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
            End Try
            Return Ok(mDataTable)
        End Function

        <HttpGet>
        Public Function GetFuncitonTypes() As IHttpActionResult
            Dim mRetVal As DataTable = FunctionTypeUtility.FunctionTypes()
            Dim mySorter As New SortTable
            mySorter.Sort(mRetVal, "Name")
            Return Me.Ok(mRetVal)
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
        Public Function Save(ByVal uiProfile As MUIFunctionProfile) As IHttpActionResult
            If uiProfile Is Nothing Then Throw New ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!")
            If uiProfile.Name Is Nothing Then Throw New ArgumentNullException("uiProfile", "uiProfile.Name cannot be a null reference (Nothing in Visual Basic)!")
            If uiProfile.Action Is Nothing Then Throw New ArgumentNullException("uiProfile", "uiProfile.Action cannot be a null reference (Nothing in Visual Basic)!")
            Dim mLog As Logger = Logger.Instance()
            Dim mRetVal As String = "false"
            Dim mSecurityInfo As New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditFunction", True)), AccountUtility.CurrentProfile())

            If HttpContext.Current.Session("EditId") IsNot Nothing Then
                Dim mEditId = Integer.Parse(HttpContext.Current.Session("EditId").ToString())
                If mEditId = uiProfile.Id Then
                    Dim accountProfile As MAccountProfile = AccountUtility.CurrentProfile()
                    Dim profile As MFunctionProfile = New MFunctionProfile()
                    Dim directoryProfile As MDirectoryProfile = Nothing
                    If Not uiProfile.Id = -1 Then
                        If Not mSecurityInfo.MayEdit Then
                            Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to edit")
                            mLog.Error(mError)
                            Return Me.InternalServerError(mError)
                        End If
                        profile = FunctionUtility.GetProfile(uiProfile.Id)
                        profile.UpdatedBy = accountProfile.Id
                        profile.UpdatedDate = DateTime.Now
                    Else
                        If Not mSecurityInfo.MayAdd Then
                            Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to add")
                            mLog.Error(mError)
                            Return Me.InternalServerError(mError)
                        End If
                        profile.AddedBy = accountProfile.Id
                        profile.AddedDate = DateTime.Now
                    End If
                    Dim viewCommaRoles As String = ""
                    Dim addCommaRoles As String = ""
                    Dim editCommaRoles As String = ""
                    Dim deleteCommaRoles As String = ""

                    Dim viewCommaGroups As String = ""
                    Dim addCommaGroups As String = ""
                    Dim editCommaGroups As String = ""
                    Dim deleteCommaGroups As String = ""

                    If uiProfile.RolesAndGroups IsNot Nothing Then
                        If uiProfile.RolesAndGroups.ViewRoles IsNot Nothing Then viewCommaRoles = String.Join(",", uiProfile.RolesAndGroups.ViewRoles)
                        If uiProfile.RolesAndGroups.AddRoles IsNot Nothing Then addCommaRoles = String.Join(",", uiProfile.RolesAndGroups.AddRoles)
                        If uiProfile.RolesAndGroups.EditRoles IsNot Nothing Then editCommaRoles = String.Join(",", uiProfile.RolesAndGroups.EditRoles)
                        If uiProfile.RolesAndGroups.DeleteRoles IsNot Nothing Then deleteCommaRoles = String.Join(",", uiProfile.RolesAndGroups.DeleteRoles)

                        If uiProfile.RolesAndGroups.ViewGroups IsNot Nothing Then viewCommaGroups = String.Join(",", uiProfile.RolesAndGroups.ViewGroups)
                        If uiProfile.RolesAndGroups.AddGroups IsNot Nothing Then addCommaGroups = String.Join(",", uiProfile.RolesAndGroups.AddGroups)
                        If uiProfile.RolesAndGroups.EditGroups IsNot Nothing Then editCommaGroups = String.Join(",", uiProfile.RolesAndGroups.EditGroups)
                        If uiProfile.RolesAndGroups.DeleteGroups IsNot Nothing Then deleteCommaGroups = String.Join(",", uiProfile.RolesAndGroups.DeleteGroups)
                    End If

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
End Namespace