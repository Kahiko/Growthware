Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Common
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports System.Web
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.BusinessData.DataAccessLayer

Namespace Utilities
    ''' <summary>
    ''' SecurityEntityUtility serves as the focal point for any web application needing to utiltize the GrowthWare framework.
    ''' Web needs such as caching are handeled here
    ''' </summary>
    Public Module SecurityEntityUtility
        Private s_DefaultProfile As MSecurityEntityProfile = Nothing
        'Private Shared m_BSecurityEntity As BSecurityEntity = Nothing
        Private s_CacheName As String = "SecurityEntityProfiles"

        ''' <summary>
        ''' Creates and returns MSecurityEntityProfile populated with information from the
        ''' configuration file.
        ''' </summary>
        ''' <returns>MSecurityEntityProfile</returns>
        Public Function DefaultProfile() As MSecurityEntityProfile
            If s_DefaultProfile Is Nothing Then
                Dim mDefaultProfile As MSecurityEntityProfile = New MSecurityEntityProfile()
                mDefaultProfile.Id = Integer.Parse(ConfigSettings.DefaultSecurityEntityId.ToString(), CultureInfo.InvariantCulture)
                mDefaultProfile.DataAccessLayer = ConfigSettings.DataAccessLayer
                mDefaultProfile.DataAccessLayerNamespace = ConfigSettings.DataAccessLayerNamespace(mDefaultProfile.DataAccessLayer)
                mDefaultProfile.DataAccessLayerAssemblyName = ConfigSettings.DataAccessLayerAssemblyName(mDefaultProfile.DataAccessLayer)
                mDefaultProfile.ConnectionString = ConfigSettings.ConnectionString(mDefaultProfile.DataAccessLayer)
                s_DefaultProfile = mDefaultProfile
            End If
            Return s_DefaultProfile
        End Function

        ''' <summary>
        ''' Returns the current MSecurityEntityProfile from context.  If one is not found in context then 
        ''' the default values from the config file will be returned.
        ''' </summary>
        ''' <returns>MSecurityEntityProfile</returns>
        Public Function CurrentProfile() As MSecurityEntityProfile
            Dim mAccount As String = AccountUtility.HttpContextUserName
            Dim mClientChoicesState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(mAccount)
            Dim mRetVal As MSecurityEntityProfile = Nothing
            If mClientChoicesState IsNot Nothing Then
                Dim mCurrentSecurityEntityID As Integer = Integer.Parse(mClientChoicesState(MClientChoices.SecurityEntityId).ToString(), CultureInfo.InvariantCulture)
                Dim mProfiles As Collection(Of MSecurityEntityProfile)
                mProfiles = Profiles()

                Dim mResult = From mProfile In mProfiles Where mProfile.Id = mCurrentSecurityEntityID Select mProfile
                mRetVal = mResult.First
            End If
            If mRetVal Is Nothing Then mRetVal = DefaultProfile()
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the profiles.
        ''' </summary>
        ''' <returns>Collection{MSecurityEntityProfile}.</returns>
        Public Function Profiles() As Collection(Of MSecurityEntityProfile)
            Dim mSecurityEntityCollection As Collection(Of MSecurityEntityProfile) = Nothing
            mSecurityEntityCollection = CType(HttpContext.Current.Cache(s_CacheName), Collection(Of MSecurityEntityProfile))
            If mSecurityEntityCollection Is Nothing Then
                Dim mBSecurityEntity As BSecurityEntity = New BSecurityEntity(DefaultProfile(), ConfigSettings.CentralManagement)
                mSecurityEntityCollection = mBSecurityEntity.SecurityEntities()
                CacheController.AddToCacheDependency(s_CacheName, mSecurityEntityCollection)
            End If
            Return mSecurityEntityCollection
        End Function

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <param name="securityEntityId">The security entity ID.</param>
        ''' <returns>MAccountProfile.</returns>
        Public Function GetProfile(ByVal securityEntityId As Integer) As MSecurityEntityProfile
            Dim mResult = From mProfile In Profiles() Where mProfile.Id = securityEntityId Select mProfile
            Dim mRetVal As MSecurityEntityProfile = New MSecurityEntityProfile()
            Try
                mRetVal = mResult.First
            Catch ex As NullReferenceException
                mRetVal = Nothing
            End Try
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <param name="name">The name.</param>
        ''' <returns>MSecurityEntityProfile.</returns>
        Public Function GetProfile(ByVal name As String) As MSecurityEntityProfile
            Dim mResult = From mProfile In Profiles() Where mProfile.Name.ToLower(CultureInfo.CurrentCulture) = name.ToLower(CultureInfo.CurrentCulture) Select mProfile
            Dim mRetVal As MSecurityEntityProfile = New MSecurityEntityProfile()
            Try
                mRetVal = mResult.First
            Catch ex As NullReferenceException
                mRetVal = Nothing
            End Try
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the valid security entities.
        ''' </summary>
        ''' <param name="account">The account.</param>
        ''' <param name="securityEntityId">The security entity ID.</param>
        ''' <param name="IsSecurityEntityAdministrator">if set to <c>true</c> [is security entity administrator].</param>
        ''' <returns>DataView.</returns>
        Public Function GetValidSecurityEntities(ByVal account As String, ByVal securityEntityId As Integer, ByVal isSecurityEntityAdministrator As Boolean) As DataView
            Dim mBSecurityEntity As BSecurityEntity = New BSecurityEntity(CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBSecurityEntity.GetValidSecurityEntities(account, securityEntityId, isSecurityEntityAdministrator).DefaultView()
        End Function

        ''' <summary>
        ''' Saves the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub Save(ByVal profile As MSecurityEntityProfile)
            Try
                Dim mBSecurityEntity As BSecurityEntity = New BSecurityEntity(CurrentProfile(), ConfigSettings.CentralManagement)
                mBSecurityEntity.Save(profile)
                CacheController.RemoveAllCache()
            Catch ex As DataAccessLayerException
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Returns a datatable of the search data
        ''' </summary>
        ''' <param name="searchCriteria">MSearchCriteria</param>
        ''' <returns>NULL/Nothing if no records are returned.</returns>
        ''' <remarks></remarks>
        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            Try
                Dim mBSecurityEntity As BSecurityEntity = New BSecurityEntity(CurrentProfile(), ConfigSettings.CentralManagement)
                Return mBSecurityEntity.Search(searchCriteria)
            Catch ex As IndexOutOfRangeException
                'no data is not a problem
                Return Nothing
            End Try
        End Function

    End Module
End Namespace
