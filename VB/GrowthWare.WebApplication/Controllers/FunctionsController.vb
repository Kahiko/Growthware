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

        <HttpPost>
        Function SetSelectedSecurityEntity(ByVal selectedSecurityEntityId As Integer) As String
            Dim targetSEProfile As MSecurityEntityProfile = SecurityEntityUtility.GetProfile(selectedSecurityEntityId)
            Dim currentSEProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mClientChoicesState As MClientChoicesState = CType(HttpContext.Current.Cache(MClientChoices.SessionName), MClientChoicesState)
            Dim mMessageProfile As MMessageProfile = Nothing
            Try
                If Not ConfigSettings.CentralManagement Then
                    'SecurityEntityUtility.SetSessionSecurityEntity(targetSEProfile)
                    mClientChoicesState(MClientChoices.SecurityEntityId) = targetSEProfile.Id
                    mClientChoicesState(MClientChoices.SecurityEntityName) = targetSEProfile.Name
                Else
                    If currentSEProfile.ConnectionString = targetSEProfile.ConnectionString Then
                        mClientChoicesState(MClientChoices.SecurityEntityId) = targetSEProfile.Id
                        mClientChoicesState(MClientChoices.SecurityEntityName) = targetSEProfile.Name
                    Else
                        mClientChoicesState(MClientChoices.SecurityEntityId) = ConfigSettings.DefaultSecurityEntityId
                        mClientChoicesState(MClientChoices.SecurityEntityName) = "System"
                    End If
                End If
                ClientChoicesUtility.Save(mClientChoicesState)
                AccountUtility.RemoveInMemoryInformation(True)
                mMessageProfile = MessageUtility.GetProfile("ChangedSelectedSecurityEntity")
            Catch ex As Exception
                Dim myMessageProfile As New MMessageProfile
                Dim mLog As Logger = Logger.Instance()
                mMessageProfile = MessageUtility.GetProfile("NoDataFound")
                Dim myEx As New Exception("SelectSecurityEntity:: reported an error.", ex)
                mLog.Error(myEx)
            End Try
            ' update all of your in memory information
            Return mMessageProfile.Body
        End Function
    End Class

    Public Class FunctionInformation
        Public Property Action() As String
        Public Property Location() As String
        Public Property Description() As String
        Public Property LinkBehavior As Integer
    End Class
End Namespace