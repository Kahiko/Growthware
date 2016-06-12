Imports System.IO
Imports System.IO.Path
Imports System.Security.Principal
Imports System.Text
Imports System.Web
Imports System.Web.UI.HtmlControls
Imports GrowthWare.Framework.Model.Profiles

Namespace Utilities
	''' <summary>
	''' The FileUtility is a utility class used to help with file and directory management.
	''' </summary>
	Public Module FileUtility

		''' <summary>
		''' Retruns the parent full name.
		''' </summary>
		''' <param name="path">string</param>
		''' <returns>string</returns>
		Public Function GetParent(ByVal path As String) As String
			Dim mRetVal As String
			Dim mDirInfo As New DirectoryInfo(path)
			mRetVal = mDirInfo.Parent.FullName.ToString
			Return mRetVal
		End Function

		''' <summary>
		''' Returns a table of files and directories.
		''' </summary>
		''' <param name="path">string</param>
		''' <param name="directoryProfile">MDirectoryProfile</param>
		''' <returns>DataTable</returns>
		Public Function GetDirectoryTableData(ByVal path As String, ByVal directoryProfile As MDirectoryProfile) As DataTable
			Return GetDirectoryTableData(path, directoryProfile, False)
		End Function

		''' <summary>
		''' Returns a table of files and/or directories.
		''' </summary>
		''' <param name="path">string</param>
		''' <param name="directoryProfile">MDirectoryProfile</param>
		''' <param name="filesOnly">bool</param>
		''' <returns>DataTable</returns>
		Public Function GetDirectoryTableData(ByVal path As String, ByVal directoryProfile As MDirectoryProfile, ByVal filesOnly As Boolean) As DataTable
			If directoryProfile Is Nothing Then
				Throw New ArgumentException("directoryProfile", "Can not be null.")
			End If
			Dim mRetTable As DataTable = Nothing
			Dim mRow As DataRow = Nothing
			Dim mKiloBytes As Integer = 1024
			Dim mMegaBytes As Integer = mKiloBytes * 1024
			Dim mGigaBytes As Integer = mMegaBytes * 1024
			Dim mStringBuilder As New StringBuilder(4096)
			Dim mDirectorySeparatorChar As Char = DirectorySeparatorChar
			Dim mImpersonatedUser As WindowsImpersonationContext = Nothing
			If Not filesOnly Then
				Try
					mRetTable = New DataTable("MyTable")
					mRow = mRetTable.NewRow()
					Dim mDirs() As String
					If directoryProfile.Impersonate Then
						mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD)
					End If

					' Add the column header

					mRetTable.Columns.Add("Name", System.Type.GetType("System.String"))
					mRetTable.Columns.Add("ShortFileName", System.Type.GetType("System.String"))
					mRetTable.Columns.Add("Extension", System.Type.GetType("System.String"))
					mRetTable.Columns.Add("Delete", System.Type.GetType("System.String"))
					mRetTable.Columns.Add("Type", System.Type.GetType("System.String"))
					mRetTable.Columns.Add("Size", System.Type.GetType("System.String"))
					mRetTable.Columns.Add("Modified", System.Type.GetType("System.String"))
					mRetTable.Columns.Add("FullName", System.Type.GetType("System.String"))
					mRetTable.Columns("FullName").ReadOnly = True

					mRow("Name") = mStringBuilder.ToString
					mStringBuilder = New StringBuilder	  ' Clear the string builder

					mDirs = Directory.GetDirectories(path)
					Dim mDir As String
					For Each mDir In mDirs
						Dim mDirName As String = System.IO.Path.GetFileName(mDir)
						mStringBuilder = New StringBuilder		 ' Clear the string builder
						mRow = mRetTable.NewRow()		 ' Create a new row
						' Populate the string for the new row
						mStringBuilder.Append(mDirName)
						' Populate the cell in the row
						mRow("Name") = mStringBuilder.ToString

						mStringBuilder = New StringBuilder		 ' Clear the string builder
						mRow("ShortFileName") = mStringBuilder.ToString

						mStringBuilder = New StringBuilder		 ' Clear the string builder
						mRow("Extension") = mStringBuilder.ToString

						mStringBuilder = New StringBuilder		 ' Clear the string builder
						' Populate the cell in the row
						mRow("Delete") = mStringBuilder.ToString
						mStringBuilder = New StringBuilder		 ' Clear the string builder
						mStringBuilder.Append("Folder")
						' Populate the cell in the row
						mRow("Type") = mStringBuilder.ToString
						mStringBuilder = New StringBuilder		 ' Clear the string builder
						mStringBuilder.Append("N/A")
						' Populate the cell in the row
						mRow("Size") = mStringBuilder.ToString
						mStringBuilder = New StringBuilder		 ' Clear the string builder
						mStringBuilder.Append(Directory.GetLastWriteTime(path & mDirectorySeparatorChar.ToString() & mDirName).ToString())
						' Populate the cell in the row
						mRow("Modified") = mStringBuilder.ToString
						mStringBuilder = New StringBuilder
						mStringBuilder.Append(mDirectorySeparatorChar.ToString() & mDirName & "\")
						mRow("FullName") = mStringBuilder.ToString
						mRetTable.Rows.Add(mRow)		' Add the row to the table
					Next
				Catch ex As IOException
					If Not mRetTable Is Nothing Then mRetTable.Dispose()
					Dim mLoger As LogUtility = LogUtility.GetInstance
					mLoger.Error(ex)
					Throw
				End Try

			End If
			Try	   ' Add all of the directories to the table
				Dim mDirInfo As New DirectoryInfo(path)
				Dim mFiles() As FileInfo
				mFiles = mDirInfo.GetFiles()
				Dim mFile As FileInfo
				For Each mFile In mFiles
					mRow = mRetTable.NewRow()
					Dim mFilename As String = mFile.Name
					Dim mShortFileName As String = mFile.Name
					mShortFileName = mFilename.Remove(Len(mFilename) - Len(mFile.Extension), Len(mFile.Extension))
					Dim sFileSize As String
					mStringBuilder = New StringBuilder
					mStringBuilder.Append(mFilename)
					mRow("Name") = mStringBuilder.ToString
					mStringBuilder = New StringBuilder
					mStringBuilder.Append("File")
					mRow("Type") = mStringBuilder.ToString

					mStringBuilder = New StringBuilder
					mStringBuilder.Append(mShortFileName)
					mRow("shortFileName") = mStringBuilder.ToString

					mStringBuilder = New StringBuilder
					Dim fileExtension As String = mFile.Extension
					mStringBuilder.Append(fileExtension)
					mRow("Extension") = mStringBuilder.ToString

					mStringBuilder = New StringBuilder
					' Show size in either KB or MB.  GB files will cause a problem
					If (mFile.Length / mKiloBytes) <= mKiloBytes Then
						sFileSize = mFile.Length / mKiloBytes
						mStringBuilder.Append(FormatNumber(sFileSize, 1, , , TriState.True) & " KB")
					ElseIf (mFile.Length / mMegaBytes) <= mMegaBytes Then
						sFileSize = mFile.Length / mMegaBytes
						mStringBuilder.Append(FormatNumber(sFileSize, 1, , , TriState.True) & " MB")
					Else
						sFileSize = mFile.Length / mGigaBytes
						mStringBuilder.Append(FormatNumber(sFileSize, 1, , , TriState.True) & " GB")
					End If

					mRow("Size") = mStringBuilder.ToString
					mStringBuilder = New StringBuilder
					mStringBuilder.Append(File.GetLastWriteTime(path & _
					  mDirectorySeparatorChar.ToString() & mFile.Name).ToString())
					mRow("Modified") = mStringBuilder.ToString
					mStringBuilder = New StringBuilder
					mStringBuilder.Append(mFile.FullName)
					mRow("FullName") = mStringBuilder.ToString

					mRetTable.Rows.Add(mRow)
				Next
			Catch ex As IOException
				If Not mRetTable Is Nothing Then mRetTable.Dispose()
				Dim mLoger As LogUtility = LogUtility.GetInstance
				mLoger.Error(ex)
				Throw
			Finally
				If directoryProfile.Impersonate Then
					' Stop impersonating the user.
					If Not mImpersonatedUser Is Nothing Then
						mImpersonatedUser.Undo()
					End If
				End If
			End Try
			' Return the table object as the data source
			Return mRetTable
		End Function

		''' <summary>
		''' Up loads file from an HtmlInputFile to the directory specified in the MDirectoryProfile object.
		''' </summary>
		''' <param name="uploadFile">HtmlInputFile</param>
		''' <param name="currentDir">string</param>
		''' <param name="directoryProfile">MDirectoryProfile</param>
		''' <returns>string</returns>
		Public Function DoUpload(ByVal uploadFile As HtmlInputFile, ByVal currentDir As String, ByVal directoryProfile As MDirectoryProfile) As String
			If directoryProfile Is Nothing Then
				Throw New ArgumentException("directoryProfile", "Can not be null.")
			End If

			Dim mRetVal As String = "Upload successfull"
			Dim mDirectorySeparatorChar As Char = System.IO.Path.DirectorySeparatorChar
			Dim mImpersonatedUser As WindowsImpersonationContext = Nothing
			If directoryProfile Is Nothing Then
				Throw New ArgumentNullException("directoryProfile", "directoryProfile Can not be null!")
			End If
			If uploadFile Is Nothing Then
				Throw New ArgumentNullException("uploadFile", "uploadFile Can not be null!")
			End If

			If Not (uploadFile.PostedFile Is Nothing) Then
				Try
					If directoryProfile.Impersonate Then
						mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD)
					End If
					Dim mPostedFile As HttpPostedFile = uploadFile.PostedFile
					Dim mFilename As String = System.IO.Path.GetFileName(mPostedFile.FileName)
					'Dim mContentType As String = mPostedFile.ContentType
					'Dim mContentLength As Integer = mPostedFile.ContentLength

					mPostedFile.SaveAs(currentDir & mDirectorySeparatorChar.ToString() & mFilename)
				Catch ex As IOException
					Dim mLogger As LogUtility = LogUtility.GetInstance
					mLogger.Error(ex)
					mRetVal = "Failed uploading file"
				Finally
					If directoryProfile.Impersonate Then ' Stop impersonating the user.
						If Not mImpersonatedUser Is Nothing Then
							mImpersonatedUser.Undo()
						End If
					End If
				End Try
			End If
			Return mRetVal
		End Function

		''' <summary>
		''' Creates the directory specified in the MDirectoryProfile given the currentDirecty.
		''' </summary>
		''' <param name="currentDirectory">string</param>
		''' <param name="newDirectory">string</param>
		''' <param name="directoryProfile">MDirectoryProfile</param>
		''' <returns>string</returns>
		Public Function CreateDirectory(ByVal currentDirectory As String, ByVal newDirectory As String, ByVal directoryProfile As MDirectoryProfile) As String
			If directoryProfile Is Nothing Then
				Throw New ArgumentException("directoryProfile", "Can not be null.")
			End If
			Dim mRetVal As String
			mRetVal = "Successfully created the new directory!"
			Dim mImpersonatedUser As WindowsImpersonationContext = Nothing
			Try
				If Not directoryProfile Is Nothing Then
					If directoryProfile.Impersonate Then
						mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD)
					End If
					Directory.CreateDirectory(currentDirectory & "\" & newDirectory)
				End If
			Catch ex As IOException
				Dim mLoger As LogUtility = LogUtility.GetInstance
				mLoger.Error(ex)
				Throw
			Finally
				If Not directoryProfile Is Nothing Then
					If directoryProfile.Impersonate Then
						' Stop impersonating the user.
						If Not mImpersonatedUser Is Nothing Then
							mImpersonatedUser.Undo()
						End If
					End If
				End If
			End Try
			Return mRetVal
		End Function

		''' <summary>
		''' Deletes a directory specified in the MDirectoryProfile in the current directory.
		''' </summary>
		''' <param name="currentDirectory">string</param>
		''' <param name="directoryProfile">MDirectoryProfile</param>
		''' <returns></returns>
		Public Function DeleteDirectory(ByVal currentDirectory As String, ByVal directoryProfile As MDirectoryProfile) As String
			If directoryProfile Is Nothing Then
				Throw New ArgumentException("directoryProfile", "Can not be null.")
			End If
			Dim mRetVal As String
			Dim mImpersonatedUser As WindowsImpersonationContext = Nothing
			mRetVal = "Successfully deleted the directory(s)"
			Try
				If Not directoryProfile Is Nothing Then
					If directoryProfile.Impersonate Then
						mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD)
					End If
					Directory.Delete(currentDirectory)
				End If
			Catch ex As IOException
				Dim mLoger As LogUtility = LogUtility.GetInstance
				mLoger.Error(ex)
				Throw
			Finally
				If Not directoryProfile Is Nothing Then
					If directoryProfile.Impersonate Then
						' Stop impersonating the user.
						If Not mImpersonatedUser Is Nothing Then
							mImpersonatedUser.Undo()
						End If
					End If
				End If
			End Try
			Return mRetVal
		End Function

		''' <summary>
		''' Deletes a file in the directory specified in the MDirectoryProfile object.
		''' </summary>
		''' <param name="fileName"></param>
		''' <param name="directoryProfile"></param>
		''' <returns>string</returns>
		Public Function DeleteFile(ByVal fileName As String, ByVal directoryProfile As MDirectoryProfile) As String
			If directoryProfile Is Nothing Then
				Throw New ArgumentNullException("directoryProfile", "Can not be null.")
			End If
			Dim mRetVal As String
			mRetVal = "Successfully deleted the file(s)"
			Dim mImpersonatedUser As WindowsImpersonationContext = Nothing
			Try
				If directoryProfile.Impersonate Then
					mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD)
				End If
				File.Delete(fileName)
			Catch ex As IOException
				Dim mLog As LogUtility = LogUtility.GetInstance()
				mLog.Error(ex)
				mRetVal = ex.Message.ToString
			Finally
				If directoryProfile.Impersonate Then
					' Stop impersonating the user.
					If Not mImpersonatedUser Is Nothing Then
						mImpersonatedUser.Undo()
					End If
				End If
			End Try
			Return mRetVal
		End Function

		''' <summary>
		''' Renames a file from the "source" to the "destination"
		''' </summary>
		''' <param name="sourceFileName">string</param>
		''' <param name="destinationfileName">string</param>
		''' <param name="directoryProfile">MDirectoryProfile</param>
		''' <returns>string</returns>
		''' <remarks>The MDirectoryProfile object is used for impersonation if necessary.</remarks>
		Public Function RenameFile(ByVal sourceFileName As String, ByVal destinationfileName As String, ByVal directoryProfile As MDirectoryProfile) As String
			If directoryProfile Is Nothing Then
				Throw New ArgumentNullException("directoryProfile", "Can not be null.")
			End If
			Dim mRetVal As String
			mRetVal = "Successfully renamed the file!"
			Dim mImpersonatedUser As WindowsImpersonationContext = Nothing
			Try
				If directoryProfile.Impersonate Then
					mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD)
				End If
				File.Move(sourceFileName, destinationfileName)
			Catch ex As IOException
				Dim mLog As LogUtility = LogUtility.GetInstance()
				mLog.Error(ex)
				mRetVal = ex.Message.ToString
			Finally
				If directoryProfile.Impersonate Then
					' Stop impersonating the user.
					If Not mImpersonatedUser Is Nothing Then
						mImpersonatedUser.Undo()
					End If
				End If
			End Try
			Return mRetVal
		End Function

		''' <summary>
		''' Renames a directory from the "source" to the "destination"
		''' </summary>
		''' <param name="sourceDirectoryName">string</param>
		''' <param name="destinationDirectoryName">string</param>
		''' <param name="directoryProfile">MDirectoryProfile</param>
		''' <returns>string</returns>
		''' <remarks>The MDirectoryProfile object is used for impersonation if necessary.</remarks>
		Public Function RenameDirectory(ByVal sourceDirectoryName As String, ByVal destinationDirectoryName As String, ByVal directoryProfile As MDirectoryProfile) As String
			If directoryProfile Is Nothing Then
				Throw New ArgumentNullException("directoryProfile", "Can not be null.")
			End If
			Dim mRetVal As String
			Dim mImpersonatedUser As WindowsImpersonationContext = Nothing
			mRetVal = "Successfully renamed the directory!"
			Try
				If directoryProfile.Impersonate Then
					mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD)
				End If
				Directory.Move(sourceDirectoryName, destinationDirectoryName)
			Catch ex As IOException
				Dim mLog As LogUtility = LogUtility.GetInstance()
				mLog.Error(ex)
				mRetVal = ex.Message.ToString
			Finally
				If directoryProfile.Impersonate Then
					' Stop impersonating the user.
					If Not mImpersonatedUser Is Nothing Then
						mImpersonatedUser.Undo()
					End If
				End If
			End Try
			Return mRetVal
		End Function

	End Module
End Namespace