Imports GrowthWare.Framework.Model.Profiles
Imports System.Web
Imports System.Collections.ObjectModel
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common

Namespace Utilities
    Public Class AccountUtility
        Private Shared m_CachedAnonymousAccount As String = "AnonymousProfile"
        Private Shared m_AnonymousAccount As String = "Anonymous"

#Region "Constructors"
        ''' <summary>
        ''' Constructor sets up the private fields
        ''' </summary>
        Private Sub New()
            ' do nothing do not want anyone to create an instance of the class
        End Sub
#End Region

        ''' <summary>
        ''' Retruns a collection of MAccountProfiles given an MAccountProfile and the current SecurityEntitySeqID
        ''' </summary>
        ''' <param name="profile">MAccountProfile</param>
        ''' <remarks>If the Profiles.IsSysAdmin is true then all accounts will be returned</remarks>
        Public Shared Function GetAccounts(ByVal profile As MAccountProfile) As Collection(Of MAccountProfile)
            ' was thinking of adding cache here but
            ' when you think of it it's not needed
            ' account information needs to come from
            ' the db to help ensure passwords are correct and what not.
            ' besides which a list of accounts is only necessary
            ' when editing an account and it that case
            ' what accounts that are returned are dependend on the requesting account.IsSysAdmin bit.
            Dim mBAccount As BAccounts = New BAccounts(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement)
            Return mBAccount.GetAccounts(profile)
        End Function

        ''' <summary>
        ''' Retrieves the current profile.
        ''' </summary>
        ''' <returns>MAccountProfile</returns>
        ''' <remarks>If context does not contain a referance to an account anonymous will be returned</remarks>
        Public Shared Function GetCurrentProfile() As MAccountProfile
            Dim mLog As Logger = Logger.Instance()
            mLog.Debug("AccountUtility::GetCurrentProfile() Started")
            Dim mRetProfile As MAccountProfile = Nothing
            Dim mAccountName As String = HttpContext.Current.User.Identity.Name
            If mAccountName = String.Empty Then mAccountName = m_AnonymousAccount
            If mAccountName.Trim() = m_AnonymousAccount Then
                If Not HttpContext.Current.Cache Is Nothing Then
                    mRetProfile = CType(HttpContext.Current.Cache(m_CachedAnonymousAccount), MAccountProfile)
                    If mRetProfile Is Nothing Then
                        mRetProfile = GetProfile(mAccountName)
                        CacheController.AddToCacheDependency(m_CachedAnonymousAccount, mRetProfile)
                    End If
                Else
                    mLog.Debug("AccountUtility::GetCurrentProfile() No cache avalible")
                End If
            End If
            If mRetProfile Is Nothing Then
                mRetProfile = CType(HttpContext.Current.Cache(mAccountName + "_Session"), MAccountProfile)
                If mRetProfile Is Nothing Then
                    mRetProfile = GetProfile(mAccountName)
                    If Not mRetProfile Is Nothing Then
                        HttpContext.Current.Cache.Item(mAccountName + "_Session") = mRetProfile
                    Else
                        mLog.Debug("AccountUtility::GetCurrentProfile() No cache avalible")
                    End If
                End If
            End If
            mLog.Debug("AccountUtility::GetCurrentProfile() Done")
            Return mRetProfile
        End Function

        ''' <summary>
        ''' Retrieves an account profile given the account
        ''' </summary>
        ''' <param name="account">String</param>
        ''' <returns>a populated MAccountProfile</returns>
        Public Shared Function GetProfile(ByVal account As String) As MAccountProfile
            Dim mRetVal As MAccountProfile = Nothing
            Try
                Dim mBAccount As BAccounts = New BAccounts(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement)
                mRetVal = mBAccount.GetProfile(account)
            Catch ex As IndexOutOfRangeException
                Dim mMSG As String = "Count not find account: " + account + " in the database"
                Dim mEx As New ApplicationException(mMSG, ex)
                Dim mLog As Logger = Logger.Instance
                mLog.Error(mEx)
            End Try
            Return mRetVal
        End Function

        ''' <summary>
        ''' Retrieves an account profile given the account
        ''' </summary>
        ''' <param name="accountSeqID">Integer</param>
        ''' <returns>a populated MAccountProfile</returns>
        Public Shared Function GetProfile(ByVal accountSeqID As Integer) As MAccountProfile
            Dim mResult = From mProfile In GetAccounts(GetCurrentProfile()) Where mProfile.Id = accountSeqID Select mProfile
            Dim mRetVal As MAccountProfile = New MAccountProfile()
            Try
                mRetVal = mResult.First
            Catch ex As NullReferenceException
                mRetVal = Nothing
            End Try
            If Not mRetVal Is Nothing Then
                Dim mBAccount As BAccounts = New BAccounts(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement)
                mRetVal = mBAccount.GetProfile(mRetVal.Account)
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the name of the HTTP context user.
        ''' </summary>
        ''' <returns>System.String.</returns>
        Public Shared Function GetHttpContextUserName() As String
            Dim mRetVal As String = "Anonymous"
            If Not HttpContext.Current Is Nothing AndAlso Not HttpContext.Current.User Is Nothing AndAlso Not HttpContext.Current.User.Identity Is Nothing Then
                If HttpContext.Current.User.Identity.Name.Length > 0 Then
                    If Not ConfigSettings.StripDomainFromHttpContextUserName Then
                        mRetVal = HttpContext.Current.User.Identity.Name
                    Else
                        Dim mPos As Integer = HttpContext.Current.User.Identity.Name.IndexOf("\")
                        mRetVal = HttpContext.Current.User.Identity.Name.Substring(mPos, HttpContext.Current.User.Identity.Name.Length - mPos)
                    End If
                End If
            End If
            Return mRetVal
        End Function
    End Class
End Namespace
