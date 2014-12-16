Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports System.Collections.ObjectModel
Imports System.Web
Imports System.Globalization

Namespace Utilities
    ''' <summary>
    ''' FunctionTypeUtility serves as the focal point for any web application needing to utilize the GrowthWare framework.
    ''' Web needs such as caching are handled here.
    ''' </summary>
    Public Module FunctionTypeUtility

        Private s_FunctionTypeCachedCollectionName As String = "FunctionTypeCollection"

        'Private s_FunctionTypeCachedDVFunctions As String = "dvTypeFunctions"

        ''' <summary>
        ''' Gets the function types.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Public Function FunctionTypes() As DataTable
            Dim mBFunctions As BFunctions = New BFunctions(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBFunctions.FunctionTypes()
        End Function

        ''' <summary>
        ''' Gets the function type by ID.
        ''' </summary>
        ''' <param name="id">The ID.</param>
        ''' <returns>MFunctionTypeProfile.</returns>
        Public Function GetProfile(ByVal id As Integer) As MFunctionTypeProfile
            Dim mResult = From mProfile In FunctionTypeCollection() Where mProfile.Id = id Select mProfile
            Dim mRetVal As MFunctionTypeProfile = New MFunctionTypeProfile()
            Try
                mRetVal = mResult.First
            Catch ex As InvalidOperationException
                mRetVal = Nothing
            End Try
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the function type collection.
        ''' </summary>
        ''' <returns>Collection{MFunctionTypeProfile}.</returns>
        Public Function GetProfile(ByVal name As String) As MFunctionTypeProfile
            Dim mRetVal As MFunctionTypeProfile = Nothing
            If Not String.IsNullOrEmpty(Action) Then
                Dim mResult = From mProfile In FunctionTypeCollection() Where mProfile.Name.ToLower(CultureInfo.CurrentCulture) = Action.ToLower(CultureInfo.CurrentCulture) Select mProfile
                mRetVal = New MFunctionTypeProfile()
                Try
                    mRetVal = mResult.First
                Catch ex As InvalidOperationException
                    Dim mMSG As String = "Count not find name: " + name + " in the database"
                    Dim mLog As Logger = Logger.Instance
                    mLog.Error(mMSG)
                    mRetVal = Nothing
                End Try
            End If
            Return mRetVal
        End Function

        ' ''' <summary>
        ' ''' Removes the cached function types.
        ' ''' </summary>
        'Public Sub RemoveCachedFunctionTypes()
        '    CacheController.RemoveFromCache(s_FunctionTypeCachedCollectionName)
        '    CacheController.RemoveFromCache(s_FunctionTypeCachedDVFunctions)
        'End Sub

        ' ''' <summary>
        ' ''' Res the build function collection.
        ' ''' </summary>
        'Public Sub RebuildFunctionCollection()
        '    RemoveCachedFunctionTypes()
        '    Dim mFunctionProfileInfo As MFunctionTypeProfile = New MFunctionTypeProfile()
        '    mFunctionProfileInfo = GetProfile(1)
        'End Sub

        ''' <summary>
        ''' Gets the function type collection.
        ''' </summary>
        ''' <returns>Collection{MFunctionTypeProfile}.</returns>
        Public Function FunctionTypeCollection() As Collection(Of MFunctionTypeProfile)
            Dim mRetVal As Collection(Of MFunctionTypeProfile) = CType(HttpContext.Current.Cache(s_FunctionTypeCachedCollectionName), Collection(Of MFunctionTypeProfile))
            If mRetVal Is Nothing Then
                mRetVal = New Collection(Of MFunctionTypeProfile)
                For Each mDataRow As DataRow In FunctionTypes().Rows
                    If Not mDataRow("Function_Type_Seq_ID") Is Nothing Then
                        Dim mProfile As New MFunctionTypeProfile(mDataRow)
                        mRetVal.Add(mProfile)
                    End If
                Next
                CacheController.AddToCacheDependency(s_FunctionTypeCachedCollectionName, mRetVal)
            End If
            Return mRetVal
        End Function
    End Module
End Namespace