Imports System.Collections.ObjectModel
Imports System.Net
Imports System.Web.Http
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Profiles
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json

Namespace Controllers
    Public Class FileManagerController
        Inherits ApiController

        <HttpPost>
        Public Function DeleteFiles(ByVal filesToDelete As List(Of UIFileInfo)) As IHttpActionResult
            Dim mServer As HttpServerUtility = HttpContext.Current.Server
            Dim mRetVal As String = "Done"
            Dim mDirectoryProfile As MDirectoryProfile = DirectoryUtility.GetProfile(filesToDelete(0).FunctionSeqId)
            For Each item As UIFileInfo In filesToDelete
                Dim mCurrentDirectory As String = mServer.UrlDecode(item.CurrentDirectory)
                If mCurrentDirectory.Length = 0 Then
                    mCurrentDirectory = mDirectoryProfile.Directory
                Else
                    mCurrentDirectory = mDirectoryProfile.Directory + mCurrentDirectory
                End If

                Select Case item.FileType
                    Case "File"
                        Dim mFileName As String = mCurrentDirectory + "\" + mServer.UrlDecode(item.FileName)
                        mRetVal = FileUtility.DeleteFile(mFileName, mDirectoryProfile)
                        If mRetVal.IndexOf("Successfully") = -1 Then Exit For
                    Case "Folder"
                        mCurrentDirectory = mCurrentDirectory + "/" + mServer.UrlDecode(item.FileName)
                        mRetVal = FileUtility.DeleteDirectory(mCurrentDirectory, mDirectoryProfile)
                        If mRetVal.IndexOf("Successfully") = -1 Then Exit For
                    Case Else

                End Select
            Next
            Return Me.Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function GetDirectoryLinks(ByVal requestDirectoryInfo As RequestDirectoryLinksInfo) As IHttpActionResult
            Dim mRetVal As String = String.Empty
            mRetVal = FileUtility.GetDirectoryLinks(requestDirectoryInfo.CurrentDirectoryString, requestDirectoryInfo.FunctionSeqId)
            Return Me.Ok(mRetVal)
        End Function
    End Class

    Public Class RequestDirectoryLinksInfo
        Public Property CurrentDirectoryString() As String
        Public Property FunctionSeqId() As Integer
    End Class


    Public Class UIFileInfo
        Public CurrentDirectory As String
        Public FileName As String
        Public FileType As String
        Public FunctionSeqId As Integer
    End Class
End Namespace