Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports System.Collections.ObjectModel
Imports System.Web

Namespace Utilities
    Public Module FunctionTypeUtility
        Private m_LogUtility As Logger = Logger.Instance()

        Public FunctionTypeCachedCollectionName As String = "FunctionTypeCollection"

        Public FunctionTypeCachedDVFunctions As String = "dvTypeFunctions"

        Public Function GetFunctionTypes() As DataTable
            Dim mBFunctions As BFunctions = New BFunctions(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBFunctions.FunctionTypes()
        End Function

        Public Function GetFunctionTypeCollection() As Collection(Of MFunctionTypeProfile)
            Dim mRetVal As Collection(Of MFunctionTypeProfile) = CType(HttpContext.Current.Cache(FunctionTypeCachedCollectionName), Collection(Of MFunctionTypeProfile))
            If mRetVal Is Nothing Then
                mRetVal = New Collection(Of MFunctionTypeProfile)
                Dim mBFunctions As BFunctions = New BFunctions(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
                For Each mDataRow As DataRow In GetFunctionTypes().Rows
                    If Not mDataRow("Function_Type_Seq_ID") Is Nothing Then
                        Dim mProfile As New MFunctionTypeProfile(mDataRow)
                        mRetVal.Add(mProfile)
                    End If
                Next
                CacheController.AddToCacheDependency(FunctionTypeCachedCollectionName, mRetVal)
            End If
            Return mRetVal
        End Function
    End Module
End Namespace