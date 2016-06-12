Imports System
Imports System.Collections
Imports System.IO
Imports BLL.Base.ClientChoices

Public Class LineCount
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents txtDirectoryName As System.Web.UI.WebControls.TextBox
	Protected WithEvents btnSubmit As System.Web.UI.WebControls.Button

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Protected FileNames As New ArrayList(200)
	Private Shared myFileArray() As [String] = {"*.vb", "*.aspx", "*.ascx", "*.xml", "*.asax", "*.config", "*.js"}
	Private Shared TotalLinesOfCode As Integer = 0
	Private Shared mySpace As String = String.Empty
    Protected WithEvents litLineCount As System.Web.UI.WebControls.Literal
	Protected WithEvents litTotalLines As System.Web.UI.WebControls.Literal

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Not IsPostBack Then
			txtDirectoryName.Text = Server.MapPath("~\")
		End If
	End Sub

	Public Sub GetLineCount(ByVal theDir As DirectoryInfo, ByVal iLevel As Integer)
		Dim subDirectories() As DirectoryInfo
		subDirectories = theDir.GetDirectories
		Dim x As Integer = 0
		Dim DirectoryLineCount As Integer = 0
		Dim FileLineCount As Integer = 0
		Dim numDirectories As Integer = subDirectories.Length - 1
		For x = 0 To numDirectories
			If subDirectories(x).Name.IndexOf("_") = 0 Then
				Exit Sub
			End If
			' this loops file types
			Dim sFileType As [String]
			Dim writeDirectory As Boolean = True
			For Each sFileType In myFileArray
				' this loops files
				Dim directoryFile As FileInfo
				For Each directoryFile In subDirectories(x).GetFiles(sFileType)
					If directoryFile.Name.ToLower = "assemblyinfo.vb" Then Exit For
					' open files for streamreader
					Dim sr As StreamReader = File.OpenText(directoryFile.FullName)
					'loop until the end
					While Not (sr.ReadLine() Is Nothing)
						FileLineCount += 1
					End While
					'close the streamreader
					sr.Close()
					If FileLineCount > 0 Then
						If writeDirectory Then
							litLineCount.Text &= "<br>" & subDirectories(x).FullName
							writeDirectory = False
						End If
						litLineCount.Text &= "<br>" & mySpace & directoryFile.Name & " " & FileLineCount
					End If
					If FileLineCount > 0 Then
						DirectoryLineCount += FileLineCount
					End If
					FileLineCount = 0
				Next directoryFile
			Next sFileType
			If DirectoryLineCount > 0 Then
				TotalLinesOfCode += DirectoryLineCount
				litLineCount.Text &= "<br>Lines of code for " & subDirectories(x).Name & " " & DirectoryLineCount
				litLineCount.Text &= "<br>Lines of so far " & TotalLinesOfCode
			End If
			GetLineCount(subDirectories(x), iLevel + 1)
		Next
	End Sub

	Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
		If Not txtDirectoryName.Text.Trim.Length = 0 Then
			Dim log As AppLogger = AppLogger.GetInstance
			log.Debug("Started Line Count")
			Dim currentDirectory As New DirectoryInfo(txtDirectoryName.Text)
			TotalLinesOfCode = 0
			mySpace = ""
			Dim x As Integer
			For x = 1 To 10
				mySpace &= "&nbsp;"
			Next
			GetLineCount(currentDirectory, 0)
			litLineCount.Text &= "<br>Total lines of code for the project = " & TotalLinesOfCode
			litTotalLines.Text = "<br>Total lines of code: " & TotalLinesOfCode
			log.Debug("Ended Line Count")
		End If
	End Sub
End Class