Imports System.Collections.ObjectModel
Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Common
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Enumerations

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

        <HttpGet>
        Public Function GetFunctionOrder(<FromUri()> ByVal functionSeqId As Integer) As List(Of UIFuncitonMenuOrder)
            Dim mRetVal As List(Of UIFuncitonMenuOrder) = New List(Of UIFuncitonMenuOrder)
            Dim mProfile As MFunctionProfile = FunctionUtility.GetProfile(functionSeqId)
            Dim mDataView As DataView = FunctionUtility.GetFunctionMenuOrder(mProfile).DefaultView
            For Each mRow As DataRowView In mDataView
                Dim mItem As UIFuncitonMenuOrder = New UIFuncitonMenuOrder()
                mItem.Function_Seq_Id = mRow("Function_Seq_Id").ToString()
                mItem.Name = mRow("Name").ToString()
                mItem.Action = mRow("Action").ToString()
                mRetVal.Add(mItem)
            Next
            Return mRetVal
        End Function

        <HttpPost>
        Public Function MoveMenu(<FromUri()> ByVal functionSeqId As Integer, <FromUri()> ByVal direction As String) As Boolean
            Dim mRetVal As Boolean = False
            Dim mProfile As MFunctionProfile = FunctionUtility.GetProfile(functionSeqId)
            Dim mAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
            Try
                If direction = "up" Then
                    FunctionUtility.Move(mProfile, DirectionType.Up, mAccountProfile.Id, DateTime.Now)
                Else
                    FunctionUtility.Move(mProfile, DirectionType.Down, mAccountProfile.Id, DateTime.Now)
                End If
                mRetVal = True
            Catch ex As Exception
                Dim mLogger As Logger = Logger.Instance()
                mLogger.Error(ex)
            End Try
            Return mRetVal
        End Function


    End Class

    Public Class FunctionInformation
        Public Property Action() As String
        Public Property Location() As String
        Public Property Description() As String
        Public Property LinkBehavior As Integer
    End Class

    Public Class UIFuncitonMenuOrder
        Public Function_Seq_Id As String
        Public Action As String
        Public Name As String
    End Class
End Namespace