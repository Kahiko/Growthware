Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports System.Web
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.BusinessData.DataAccessLayer

Namespace Utilities
    Public Module FunctionUtility
        Private Const s_FunctionProfileInfoName As String = "FunctionProfileInfo"
        ''' <summary>
        ''' Retrieves all functions from the either the database or cache
        ''' </summary>
        ''' <returns>A Collection of MFunctinProfiles</returns>
        Public Function Functions() As Collection(Of MFunctionProfile)
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.GetCurrentProfile()
            Dim mBFunctions As BFunctions = New BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement)
            Dim mCacheName As String = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture) + "_Functions"
            Dim mFunctionCollection As Collection(Of MFunctionProfile) = Nothing
            mFunctionCollection = CType(HttpContext.Current.Cache(mCacheName), Collection(Of MFunctionProfile))
            If mFunctionCollection Is Nothing Then
                mFunctionCollection = mBFunctions.GetFunctions(mSecurityEntityProfile.Id)
                CacheController.AddToCacheDependency(mCacheName, mFunctionCollection)
            End If
            Return mFunctionCollection
        End Function

        ''' <summary>
        ''' Moves the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <param name="direction">The direction.</param>
        ''' <param name="updatedBy">The updated by.</param>
        ''' <param name="updatedDate">Up dated date.</param>
        Public Sub Move(ByVal profile As MFunctionProfile, ByVal direction As DirectionType, ByVal updatedBy As Integer, ByVal updatedDate As DateTime)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile can not be null!")
            profile.UpdatedBy = updatedBy
            profile.UpdatedDate = updatedDate
            Dim mBAppFunctions As BFunctions = New BFunctions(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement)
            mBAppFunctions.MoveMenuOrder(profile, direction)
            RemoveCachedFunctions()
        End Sub

        ''' <summary>
        ''' Get a single function given it's action.
        ''' </summary>
        ''' <param name="action">String</param>
        ''' <returns>MFunctionProfile</returns>
        ''' <remarks>Returns null object if not found</remarks>
        Public Function GetProfile(ByVal action As String) As MFunctionProfile
            Dim mRetVal As MFunctionProfile = Nothing
            If Not String.IsNullOrEmpty(action) Then
                Dim mResult = From mProfile In Functions() Where mProfile.Action.ToLower(CultureInfo.CurrentCulture) = action.ToLower(CultureInfo.CurrentCulture) Select mProfile
                mRetVal = New MFunctionProfile()
                Try
                    mRetVal = mResult.First
                Catch ex As NullReferenceException
                    mRetVal = Nothing
                End Try
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the current profile.
        ''' </summary>
        ''' <returns>MFunctionProfile.</returns>
        Public Function CurrentProfile() As MFunctionProfile
            Dim mRetVal As MFunctionProfile
            mRetVal = CType(HttpContext.Current.Items(s_FunctionProfileInfoName), MFunctionProfile)
            Return mRetVal
        End Function

        ''' <summary>
        ''' Get a single function given it's id.
        ''' </summary>
        ''' <param name="id">int or Integer</param>
        ''' <returns>MFunctionProfile</returns>
        ''' <remarks>Returns null object if not found</remarks>
        Public Function GetProfile(ByVal id As Integer) As MFunctionProfile
            Dim mResult = From mProfile In Functions() Where mProfile.Id = id Select mProfile
            Dim mRetVal As MFunctionProfile = New MFunctionProfile()
            Try
                mRetVal = mResult.First
            Catch ex As NullReferenceException
                mRetVal = Nothing
            End Try
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the function menu order.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetFunctionMenuOrder(ByVal profile As MFunctionProfile) As DataTable
            Dim mBFunctions As BFunctions = New BFunctions(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement)
            Return mBFunctions.GetMenuOrder(profile)
        End Function

        ''' <summary>
        ''' Removes the cached functions.
        ''' </summary>
        Public Sub RemoveCachedFunctions()
            Dim mCacheName As String = String.Empty
            Dim mSecurityProfiles As Collection(Of MSecurityEntityProfile) = SecurityEntityUtility.GetProfiles()
            For Each mProfile In mSecurityProfiles
                mCacheName = mProfile.Id.ToString(CultureInfo.InvariantCulture) + "_Functions"
                CacheController.RemoveFromCache(mCacheName)
            Next

        End Sub

        ''' <summary>
        ''' Returns a datatable of the search data
        ''' </summary>
        ''' <param name="searchCriteria">MSearchCriteria</param>
        ''' <returns>NULL/Nothing if no records are returned.</returns>
        ''' <remarks></remarks>
        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            Try
                Dim mBFunctions As BFunctions = New BFunctions(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement)
                Return mBFunctions.Search(searchCriteria)
            Catch ex As IndexOutOfRangeException
                'no data is not a problem
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Saves the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <param name="saveGroups">if set to <c>true</c> [save groups].</param>
        ''' <param name="saveRoles">if set to <c>true</c> [save roles].</param>
        Public Sub Save(ByVal profile As MFunctionProfile, ByVal saveGroups As Boolean, ByVal saveRoles As Boolean)
            Try
                Dim mBFunctions As BFunctions = New BFunctions(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement)
                mBFunctions.Save(profile, saveGroups, saveRoles)
            Catch ex As DataAccessLayerException
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
            Finally
                RemoveCachedFunctions()
            End Try
        End Sub

        ''' <summary>
        ''' Deletes the specified function seq id.
        ''' </summary>
        ''' <param name="functionSeqId">The function seq id.</param>
        Public Sub Delete(ByVal functionSeqId As Integer)
            Try
                Dim mBFunctions As BFunctions = New BFunctions(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement)
                mBFunctions.Delete(functionSeqId)
            Catch ex As DataAccessLayerException
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
            Finally
                RemoveCachedFunctions()
            End Try
        End Sub

        ''' <summary>
        ''' Sets the current profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub SetCurrentProfile(ByVal profile As MFunctionProfile)
            HttpContext.Current.Items(s_FunctionProfileInfoName) = profile
        End Sub
    End Module
End Namespace
