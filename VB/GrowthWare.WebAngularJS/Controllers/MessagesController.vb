Imports System.Web.Http
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles.Interfaces
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities

Namespace Controllers
    Public Class MessagesController
        Inherits ApiController

        <HttpPost>
        Public Function Delete(<FromUri()> ByVal accountSeqId As Integer) As IHttpActionResult
            If accountSeqId < 1 Then Throw New ArgumentNullException("accountSeqId", "accountSeqId must be a positive number!")
            Dim mRetVal As String = False
            Dim mLog As Logger = Logger.Instance()
            If Not HttpContext.Current.Session("EditId") Is Nothing Then
                Dim mEditId = Integer.Parse(HttpContext.Current.Session("EditId").ToString())
                If mEditId = accountSeqId Then
                    Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditMessages", True)), AccountUtility.CurrentProfile())
                    If Not mSecurityInfo Is Nothing Then
                        If mSecurityInfo.MayDelete Then
                            Try
                                AccountUtility.Delete(accountSeqId)
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
        Public Function GetProfileForEdit(<FromUri()> ByVal messageSeqID As Integer) As IHttpActionResult
            If messageSeqID < 1 Then Throw New ArgumentOutOfRangeException("messageSeqID", "messageSeqID cannot be less than or equal to zero!")
            HttpContext.Current.Session("EditId") = messageSeqID
            Return GetMessageProfile(messageSeqID)
        End Function

        <HttpGet> Public Function GetExceptionError() As IHttpActionResult
            Dim mRetVal As Exception = GWWebHelper.ExceptionError
            If Not mRetVal Is Nothing Then
                GWWebHelper.ExceptionError = Nothing
            End If
            Return Ok(mRetVal)
        End Function

        <HttpGet>
        Public Function GetMessageProfile(<FromUri()> ByVal messageSeqID As Integer) As IHttpActionResult
            If messageSeqID < 1 Then Throw New ArgumentOutOfRangeException("messageSeqID", "messageSeqID cannot be less than or equal to zero!")
            Dim mRetVal As MUIMessageProfile = New MUIMessageProfile()
            Dim mProfile As MMessageProfile = MessageUtility.GetProfile(messageSeqID)
            Dim mAvalibleTags As String
            Dim mAssembley As String = "GrowthWare.Framework"
            Dim mNameSpace As String = "GrowthWare.Framework.Model.Profiles"
            Dim mTagProfile As IMessageProfile = Nothing
            Try
                mTagProfile = ObjectFactory.Create(mAssembley, mNameSpace, "M" + mProfile.Name)
            Catch ex As Exception
                Dim mLog As Logger = Logger.Instance()
                mLog.Debug(ex)
            Finally
                If mTagProfile Is Nothing Then
                    mAvalibleTags = "No tags avalible for this message."
                Else
                    mAvalibleTags = mTagProfile.GetTags(System.Environment.NewLine)
                End If
            End Try

            With mRetVal
                .AvalibleTags = mAvalibleTags
                .Body = mProfile.Body
                .Description = mProfile.Description
                .FormatAsHtml = mProfile.FormatAsHtml
                .Id = mProfile.Id
                .Name = mProfile.Name
                .Title = mProfile.Title
            End With
            Return Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function GetSearchResults(<FromBody> searchCriteria As MSearchCriteria) As IHttpActionResult
            Dim mDataTable As DataTable = Nothing
            Try
                mDataTable = MessageUtility.Search(searchCriteria)
            Catch ex As Exception
                ' Only going to log message
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
            End Try
            Return Ok(mDataTable)
        End Function

        <HttpPost>
        Public Function Save(<FromBody> ByVal profile As MUIMessageProfile) As IHttpActionResult
            If profile Is Nothing Then Throw New ArgumentNullException("MUIMessageProfile", "MUIMessageProfile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As String = False
            Dim mCurrentAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
            Dim mLog As Logger = Logger.Instance()
            Dim mProfileToSave = populateProfile(profile, mCurrentAccountProfile)
            If Not HttpContext.Current.Session("EditId") Is Nothing Then
                Dim mEditId = Integer.Parse(HttpContext.Current.Session("EditId").ToString())
                If mEditId = profile.Id Then
                    Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditMessages", True)), AccountUtility.CurrentProfile())
                    If Not mSecurityInfo Is Nothing Then
                        If mEditId <> -1 Then
                            If mSecurityInfo.MayEdit Then
                                MessageUtility.Save(mProfileToSave)
                                mLog.Debug("Saved message " + profile.Name + " by " + mCurrentAccountProfile.Account)
                                mRetVal = True
                            Else
                                Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to edit")
                                mLog.Error(mError)
                                Return Me.InternalServerError(mError)
                            End If
                        Else
                            If mSecurityInfo.MayAdd Then
                                MessageUtility.Save(mProfileToSave)
                                mLog.Debug("Added message " + profile.Name + " by " + mCurrentAccountProfile.Account)
                                mRetVal = "true"
                            Else
                                Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to add")
                                mLog.Error(mError)
                                Return Me.InternalServerError(mError)
                            End If
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
            End If
            Return Me.Ok(mRetVal)
        End Function

        Private Function populateProfile(ByVal uiProfile As MUIMessageProfile, ByVal accountProfile As MAccountProfile) As MMessageProfile
            Dim mProfile As New MMessageProfile()
            If uiProfile.Id > 0 Then
                mProfile = MessageUtility.GetProfile(uiProfile.Id)
                mProfile.UpdatedBy = accountProfile.Id
                mProfile.UpdatedDate = DateTime.Now()
            Else
                mProfile.AddedBy = accountProfile.Id
                mProfile.AddedDate = DateTime.Now()
            End If
            mProfile.Body = HttpContext.Current.Server.UrlDecode(uiProfile.Body)
            mProfile.Description = uiProfile.Description
            mProfile.FormatAsHtml = uiProfile.FormatAsHtml
            mProfile.Id = uiProfile.Id
            mProfile.Name = uiProfile.Name
            mProfile.SecurityEntitySeqId = SecurityEntityUtility.CurrentProfile().Id
            mProfile.Title = uiProfile.Title
            Return mProfile
        End Function

    End Class
End Namespace
