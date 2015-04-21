Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports System.Web
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports GrowthWare.Framework.BusinessData.DataAccessLayer

Namespace Utilities
    Public Class DirectoryUtility
        Private Shared s_DirectoryInfoCachedName As String = "DirectoryInfoCollection"

        ''' <summary>
        ''' DirectoryInfo Cached Collection Name
        ''' </summary>
        Public Shared ReadOnly DirectoryInfoCachedCollection As String = "DirectoryInfoCollection"

        ''' <summary>
        ''' Gets the directory collection.
        ''' </summary>
        ''' <returns>Collection{MDirectoryProfile}.</returns>
        Public Shared Function Directories() As Collection(Of MDirectoryProfile)
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mBDirectories As BDirectories = New BDirectories(mSecurityEntityProfile, ConfigSettings.CentralManagement)
            Dim mCacheName As String = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture) + "_" + s_DirectoryInfoCachedName
            Dim mRetVal As Collection(Of MDirectoryProfile) = Nothing
            mRetVal = CType(HttpContext.Current.Cache(mCacheName), Collection(Of MDirectoryProfile))
            If mRetVal Is Nothing Then
                mRetVal = mBDirectories.Directories()
                CacheController.AddToCacheDependency(mCacheName, mRetVal)
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <param name="id">The Function Profile id.</param>
        ''' <returns>MDirectoryProfile.</returns>
        Public Shared Function GetProfile(ByVal id As Integer) As MDirectoryProfile
            Dim mResult = From mProfile In Directories() Where mProfile.FunctionSeqId = id Select mProfile
            Dim mRetVal As MDirectoryProfile = New MDirectoryProfile()
            Try
                mRetVal = mResult.First
            Catch ex As NullReferenceException
                mRetVal = Nothing
            Catch ex As InvalidOperationException
                mRetVal = Nothing
            End Try
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <param name="name">The name.</param>
        ''' <returns>MDirectoryProfile.</returns>
        Public Shared Function GetProfile(ByVal name As String) As MDirectoryProfile
            Dim mResult = From mProfile In Directories() Where mProfile.Name.ToLower(CultureInfo.CurrentCulture) = name.ToLower(CultureInfo.CurrentCulture) Select mProfile
            Dim mRetVal As MDirectoryProfile = New MDirectoryProfile()
            Try
                mRetVal = mResult.First
            Catch ex As NullReferenceException
                mRetVal = Nothing
            Catch ex As InvalidOperationException
                mRetVal = Nothing
            End Try
            Return mRetVal
        End Function

        Public Shared Sub Save(ByVal profile As MDirectoryProfile)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            CacheController.RemoveFromCache(s_DirectoryInfoCachedName)
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mLog As Logger = Logger.Instance()
            Try
                profile.ImpersonatePassword = CryptoUtility.Decrypt(profile.ImpersonatePassword, mSecurityEntityProfile.EncryptionType)
            Catch ex As CryptoUtilityException
                profile.ImpersonatePassword = CryptoUtility.Encrypt(profile.ImpersonatePassword, mSecurityEntityProfile.EncryptionType)
            End Try
            Try
                profile.Directory = CryptoUtility.Decrypt(profile.Directory, mSecurityEntityProfile.EncryptionType)
            Catch ex As CryptoUtilityException
                profile.Directory = CryptoUtility.Encrypt(profile.Directory, mSecurityEntityProfile.EncryptionType)
            End Try
            Try
                profile.ImpersonateAccount = CryptoUtility.Decrypt(profile.ImpersonateAccount, mSecurityEntityProfile.EncryptionType)
            Catch ex As CryptoUtilityException
                profile.ImpersonateAccount = CryptoUtility.Encrypt(profile.ImpersonateAccount, mSecurityEntityProfile.EncryptionType)
            End Try
            Dim mBLL As BDirectories = New BDirectories(mSecurityEntityProfile, ConfigSettings.CentralManagement)
            Try
                mBLL.Save(profile)
            Catch ex As DataAccessLayerException
                mLog.Error(ex)
                Throw New WebSupportException("Could not save the directory information!")
            End Try
            Dim mCacheName As String = mSecurityEntityProfile.Id.ToString(CultureInfo.CurrentCulture) + "_" + s_DirectoryInfoCachedName
            CacheController.RemoveFromCache(mCacheName)
        End Sub
    End Class
End Namespace