Imports GrowthWare.WebSupport.BasePages
Imports GrowthWare.WebSupport.Utilities
Imports System.Web.Services
Imports System.IO

Public Class LineCount
    Inherits BaseWebpage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mExcludeList As List(Of String) = New List(Of String)
        txtDirectoryName.Value = Server.MapPath("~\")
        Dim mSB As StringBuilder = New StringBuilder()
        Dim mFileArray() As String = txtFiles.Value.Split(",")
        Dim mExclusionArray() As String = txtExclusionPattern.Value.ToString().Split(",")
        Dim mDirectoryLineCount As Integer = 0
        Dim mTotalLinesOfCode As Integer = 0
        For Each item In mExclusionArray
            mExcludeList.Add(item.ToString().ToUpper())
        Next
        litLineCount.InnerHtml = ""
        litTotalLines.InnerHtml = ""
        Dim currentDirectory As New DirectoryInfo(txtDirectoryName.Value)
        litLineCount.InnerHtml = FileUtility.GetLineCount(currentDirectory, 0, mSB, mExcludeList, mDirectoryLineCount, mTotalLinesOfCode, mFileArray)
    End Sub

    <WebMethod(CacheDuration:=0, EnableSession:=False)>
    Public Shared Function GetLineCount(ByVal countInfo As CountInfo) As String
        Dim mExcludeList As List(Of String) = New List(Of String)
        Dim mSB As StringBuilder = New StringBuilder()
        Dim mFileArray() As String = countInfo.IncludeFiles.ToString().Split(",")
        Dim mExclusionArray() As String = countInfo.ExcludePattern.ToString().Split(",")
        Dim currentDirectory As New DirectoryInfo(countInfo.TheDirectory.ToString())
        Dim mDirectoryLineCount As Integer = 0
        Dim mTotalLinesOfCode As Integer = 0
        For Each item In mExclusionArray
            mExcludeList.Add(item.ToString().ToUpper())
        Next
        Return FileUtility.GetLineCount(currentDirectory, 0, mSB, mExcludeList, mDirectoryLineCount, mTotalLinesOfCode, mFileArray).ToString()
    End Function
End Class

Public Class CountInfo
    Public TheDirectory As String
    Public ExcludePattern As String
    Public IncludeFiles As String
End Class