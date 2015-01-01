Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Common
Imports System.Web
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer

Namespace Utilities
    Public Module MessageUtility
        Private s_MessagesUnitCachedDVName As String = "dvMessages"
        Private s_MessagesUnitCachedCollectionName As String = "MessagesCollection"

        ''' <summary>
        ''' Messages the name of the unit cached collection.
        ''' </summary>
        ''' <param name="securityEntityId">The security entity ID.</param>
        ''' <returns>System.String.</returns>
        Public Function MessagesUnitCachedCollectionName(ByVal securityEntityId As Integer) As String
            Return securityEntityId.ToString(CultureInfo.InvariantCulture) + s_MessagesUnitCachedCollectionName + "_Messages"
        End Function

        ''' <summary>
        ''' Messages the name of the unit cached DV.
        ''' </summary>
        ''' <param name="securityEntityId">The security entity ID.</param>
        ''' <returns>System.String.</returns>
        Public Function MessagesUnitCachedDVName(ByVal securityEntityId As Integer) As String
            Return securityEntityId.ToString(CultureInfo.InvariantCulture) + s_MessagesUnitCachedDVName + "_Messages"
        End Function

        ''' <summary>
        ''' Gets the messages.
        ''' </summary>
        ''' <returns>Collection{MMessageProfile}.</returns>
        Public Function Messages() As Collection(Of MMessageProfile)
            Dim mMessageCollection As Collection(Of MMessageProfile) = Nothing
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile
            Dim mBMessages As BMessages = New BMessages(mSecurityEntityProfile, ConfigSettings.CentralManagement)
            Dim mCacheName As String = MessagesUnitCachedCollectionName(mSecurityEntityProfile.Id)
            mMessageCollection = CType(HttpContext.Current.Cache(mCacheName), Collection(Of MMessageProfile))
            If mMessageCollection Is Nothing Then
                mMessageCollection = mBMessages.GetMessages(mSecurityEntityProfile.Id)
                CacheController.AddToCacheDependency(mCacheName, mMessageCollection)
            End If
            Return mMessageCollection
        End Function

        ''' <summary>
        ''' Get a single function given it's action.
        ''' </summary>
        ''' <param name="name">String</param>
        ''' <returns>MMessageProfile</returns>
        ''' <remarks>Returns null object if not found</remarks>
        Public Function GetProfile(ByVal name As String) As MMessageProfile
            Dim mResult = From mProfile In Messages() Where mProfile.Name.ToLower(CultureInfo.CurrentCulture) = name.ToLower(CultureInfo.CurrentCulture) Select mProfile
            Dim mRetVal As MMessageProfile = New MMessageProfile()
            Try
                mRetVal = mResult.First
            Catch ex As IndexOutOfRangeException
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
                mRetVal = Nothing
            End Try
            Return mRetVal
        End Function

        ''' <summary>
        ''' Get a single function given it's id.
        ''' </summary>
        ''' <param name="id">int or Integer</param>
        ''' <returns>MMessageProfile</returns>
        ''' <remarks>Returns null object if not found</remarks>
        Public Function GetProfile(ByVal id As Integer) As MMessageProfile
            Dim mResult = From mProfile In Messages() Where mProfile.Id = id Select mProfile
            Dim mRetVal As MMessageProfile = New MMessageProfile()
            Try
                mRetVal = mResult.First
            Catch ex As InvalidOperationException
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
                mRetVal = Nothing
            End Try
            Return mRetVal
        End Function

        ''' <summary>
        ''' Removes the cached messages DV.
        ''' </summary>
        Private Sub RemoveCachedMessagesDV()
            Dim mySecurityEntity As Integer = ClientChoicesUtility.SelectedSecurityEntity()
            CacheController.RemoveFromCache(MessagesUnitCachedDVName(mySecurityEntity))
        End Sub

        ''' <summary>
        ''' Removes the cached messages collection.
        ''' </summary>
        Private Sub RemoveCachedMessagesCollection()
            Dim mySecurityEntity As Integer = ClientChoicesUtility.SelectedSecurityEntity
            CacheController.RemoveFromCache(MessagesUnitCachedCollectionName(mySecurityEntity))
            RemoveCachedMessagesDV()
        End Sub

        ''' <summary>
        ''' Saves the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub Save(ByVal profile As MMessageProfile)
            Dim mBMessages As BMessages = New BMessages(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            mBMessages.Save(profile)
            RemoveCachedMessagesCollection()
        End Sub

        ''' <summary>
        ''' Returns a DataTable of the search data
        ''' </summary>
        ''' <param name="searchCriteria">MSearchCriteria</param>
        ''' <returns>NULL/Nothing if no records are returned.</returns>
        ''' <remarks></remarks>
        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            Try
                Dim mBMessages As BMessages = New BMessages(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
                Return mBMessages.Search(searchCriteria)
            Catch ex As IndexOutOfRangeException
                'no data is not a problem
                Dim mLog As Logger = Logger.Instance()
                mLog.Debug(ex)
                Return Nothing
            End Try
        End Function
    End Module
End Namespace
