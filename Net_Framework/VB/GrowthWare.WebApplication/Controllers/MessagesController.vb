Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common
Imports System.Globalization
Imports GrowthWare.Framework.Model.Enumerations

Namespace Controllers
    Public Class MessagesController
        Inherits ApiController

        <HttpPost>
        Public Function Delete(<FromUri()> ByVal accountSeqId As Integer) As IHttpActionResult
            If accountSeqId < 1 Then Throw New ArgumentNullException("accountSeqId", "accountSeqId must be a positive number!")
            Dim mRetVal As String = False
            Dim mLog As Logger = Logger.Instance()
            If Not HttpContext.Current.Items("EditId") Is Nothing Then
                Dim mEditId = Integer.Parse(HttpContext.Current.Items("EditId").ToString())
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

        <HttpPost>
        Public Function Save(ByVal profile As UIMessageProfile) As IHttpActionResult
            If profile Is Nothing Then Throw New ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As String = False
            Dim mCurrentAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
            Dim mLog As Logger = Logger.Instance()
            Dim mProfileToSave = populateProfile(profile, mCurrentAccountProfile)
            If Not HttpContext.Current.Items("EditId") Is Nothing Then
                Dim mEditId = Integer.Parse(HttpContext.Current.Items("EditId").ToString())
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

        Private Function populateProfile(ByVal uiProfile As UIMessageProfile, ByVal accountProfile As MAccountProfile) As MMessageProfile
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

    Public Class UIMessageProfile
        Public Body As String
        Public Description As String
        Public FormatAsHtml As Boolean
        Public Id As Integer
        Public Name As String
        Public Title As String
    End Class
End Namespace
