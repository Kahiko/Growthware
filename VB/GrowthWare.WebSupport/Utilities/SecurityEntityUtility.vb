Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Common
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports System.Web
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer

Namespace Utilities
    ''' <summary>
    ''' SecurityEntityUtility serves as the focal point for any web application needing to utiltize the GrowthWare framework.
    ''' Web needs such as caching are handeled here
    ''' </summary>
    Public Class SecurityEntityUtility
        Private Shared m_ProfileContextName As String = "ContextSecurityEntityProfile"
        Private Shared m_DefaultProfile As MSecurityEntityProfile = Nothing
        'Private Shared m_BSecurityEntity As BSecurityEntity = Nothing
        Private Shared m_CacheName As String = "SecurityEntityProfiles"

        ''' <summary>
        ''' Creates and returns MSecurityEntityProfile populated with information from the
        ''' configuration file.
        ''' </summary>
        ''' <returns>MSecurityEntityProfile</returns>
        Public Shared Function GetDefaultProfile() As MSecurityEntityProfile
            If m_DefaultProfile Is Nothing Then
                Dim mDefaultProfile As MSecurityEntityProfile = New MSecurityEntityProfile()
                mDefaultProfile.Id = ConfigSettings.DefaultSecurityEntityId
                mDefaultProfile.DataAccessLayer = ConfigSettings.DataAccessLayer
                mDefaultProfile.DataAccessLayerNamespace = ConfigSettings.DataAccessLayerNamespace(mDefaultProfile.DataAccessLayer)
                mDefaultProfile.DataAccessLayerAssemblyName = ConfigSettings.DataAccessLayerAssemblyName(mDefaultProfile.DataAccessLayer)
                mDefaultProfile.ConnectionString = ConfigSettings.ConnectionString(mDefaultProfile.DataAccessLayer)
                m_DefaultProfile = mDefaultProfile
            End If
            Return m_DefaultProfile
        End Function

        ''' <summary>
        ''' Returns the current MSecurityEntityProfile from context.  If one is not found in context then 
        ''' the default values from the config file will be returned.
        ''' </summary>
        ''' <returns>MSecurityEntityProfile</returns>
        Public Shared Function GetCurrentProfile() As MSecurityEntityProfile
            Dim mAccount As String = AccountUtility.GetHttpContextUserName
            Dim mClientChoicesState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(mAccount)
            Dim mCurrentSecurityEntityID As Integer = mClientChoicesState(MClientChoices.SecurityEntityId)
            Dim mProfiles As Collection(Of MSecurityEntityProfile)
            mProfiles = GetProfiles()

            Dim mResult = From mProfile In mProfiles Where mProfile.Id = mCurrentSecurityEntityID Select mProfile
            Dim mRetVal As MSecurityEntityProfile = mResult.First
            If mRetVal Is Nothing Then mRetVal = GetDefaultProfile()
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the profiles.
        ''' </summary>
        ''' <returns>Collection{MSecurityEntityProfile}.</returns>
        Public Shared Function GetProfiles() As Collection(Of MSecurityEntityProfile)
            Dim mSecurityEntityCollection As Collection(Of MSecurityEntityProfile) = Nothing
            mSecurityEntityCollection = CType(HttpContext.Current.Cache(m_CacheName), Collection(Of MSecurityEntityProfile))
            If mSecurityEntityCollection Is Nothing Then
                Dim mBSecurityEntity As BSecurityEntity = New BSecurityEntity(GetDefaultProfile(), ConfigSettings.CentralManagement)
                mSecurityEntityCollection = mBSecurityEntity.SecurityEntities()
                CacheController.AddToCacheDependency(m_CacheName, mSecurityEntityCollection)
            End If
            Return mSecurityEntityCollection
        End Function

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <param name="securityEntityID">The security entity ID.</param>
        ''' <returns>MAccountProfile.</returns>
        Public Shared Function GetProfile(ByVal securityEntityID As Integer) As MSecurityEntityProfile
            Dim mResult = From mProfile In GetProfiles() Where mProfile.Id = securityEntityID Select mProfile
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
        Public Shared Function GetProfile(ByVal name As String) As MSecurityEntityProfile
            Dim mResult = From mProfile In GetProfiles() Where mProfile.Name.ToLower(CultureInfo.CurrentCulture) = name.ToLower(CultureInfo.CurrentCulture) Select mProfile
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
        ''' <param name="Account">The account.</param>
        ''' <param name="SecurityEntityID">The security entity ID.</param>
        ''' <param name="IsSecurityEntityAdministrator">if set to <c>true</c> [is security entity administrator].</param>
        ''' <returns>DataView.</returns>
        Public Shared Function GetValidSecurityEntities(ByVal Account As String, ByVal SecurityEntityID As Integer, ByVal IsSecurityEntityAdministrator As Boolean) As DataView
            Dim mBSecurityEntity As BSecurityEntity = New BSecurityEntity(GetCurrentProfile(), ConfigSettings.CentralManagement)
            Return mBSecurityEntity.GetValidSecurityEntities(Account, SecurityEntityID, IsSecurityEntityAdministrator).DefaultView()
        End Function

        ''' <summary>
        ''' Saves the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Shared Sub Save(ByRef profile As MSecurityEntityProfile)
            Try
                Dim mBSecurityEntity As BSecurityEntity = New BSecurityEntity(GetCurrentProfile(), ConfigSettings.CentralManagement)
                mBSecurityEntity.Save(profile)
                CacheController.RemoveAllCache()
            Catch ex As Exception
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
        Public Shared Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            Try
                Dim mBSecurityEntity As BSecurityEntity = New BSecurityEntity(GetCurrentProfile(), ConfigSettings.CentralManagement)
                Return mBSecurityEntity.Search(searchCriteria)
            Catch ex As IndexOutOfRangeException
                'no data is not a problem
                Return Nothing
            End Try
        End Function

    End Class
End Namespace
