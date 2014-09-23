Imports System.Collections.ObjectModel
Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Common
Imports GrowthWare.WebSupport.Utilities

Namespace Controllers
    Public Class FunctionsController
        Inherits ApiController

        <HttpGet>
        Public Function GetFunctionData() As Collection(Of FunctionInformation)
            Dim mFunctionInformation As FunctionInformation = Nothing
            Dim mRetVal As Collection(Of FunctionInformation) = New Collection(Of FunctionInformation)
            Dim mFunctions As Collection(Of MFunctionProfile) = FunctionUtility.Functions()
            Dim mAppName As String = ConfigSettings.AppName
            If mAppName.Length <> 0 Then
                mAppName = "/" + mAppName + "/"
            End If
            For Each mProfile In mFunctions
                If mProfile.FunctionTypeSeqId <> 2 Then
                    mFunctionInformation = New FunctionInformation()
                    mFunctionInformation.Action = mProfile.Action
                    mFunctionInformation.Location = mAppName + mProfile.Source
                    mFunctionInformation.Description = mProfile.Description
                    mFunctionInformation.LinkBehavior = mProfile.LinkBehavior
                    mRetVal.Add(mFunctionInformation)
                End If
            Next

            Return mRetVal
        End Function
    End Class

    Public Class FunctionInformation
        Public Property Action() As String
        Public Property Location() As String
        Public Property Description() As String
        Public Property LinkBehavior As Integer
    End Class
End Namespace