Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.IO
Imports System.IO.Path
Imports System.Globalization
Imports GrowthWare.Framework.Model.Profiles
Imports System.Security.Principal
Imports System.Web
Imports GrowthWare.Framework.Common

Namespace Utilities
    Module FileUtility
        Private s_Space As String = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"

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
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes")>
        Public Function GetDirectoryTableData(ByVal path As String, ByVal directoryProfile As MDirectoryProfile, ByVal filesOnly As Boolean) As DataTable
            If directoryProfile Is Nothing Then Throw New ArgumentNullException("directoryProfile", "directoryProfile can not be null.")
            Dim mRetTable As DataTable = Nothing
            Dim mRow As DataRow = Nothing
            Dim mStringBuilder As New StringBuilder(4096)
            Dim mDirectorySeparatorChar As Char = DirectorySeparatorChar
            Dim mImpersonatedUser As WindowsImpersonationContext = Nothing
            If Not filesOnly Then
                Try
                    mRetTable = New DataTable("MyTable")
                    mRow = mRetTable.NewRow()
                    Dim mDirs() As String
                    If directoryProfile.Impersonate Then
                        mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword)
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
                    mStringBuilder = New StringBuilder    ' Clear the string builder

                    mDirs = Directory.GetDirectories(path)
                    Dim mDir As String
                    For Each mDir In mDirs
                        Dim mDirName As String = System.IO.Path.GetFileName(mDir)
                        mStringBuilder = New StringBuilder       ' Clear the string builder
                        mRow = mRetTable.NewRow()        ' Create a new row
                        ' Populate the string for the new row
                        mStringBuilder.Append(mDirName)
                        ' Populate the cell in the row
                        mRow("Name") = mStringBuilder.ToString

                        mStringBuilder = New StringBuilder       ' Clear the string builder
                        mRow("ShortFileName") = mStringBuilder.ToString

                        mStringBuilder = New StringBuilder       ' Clear the string builder
                        mRow("Extension") = mStringBuilder.ToString

                        mStringBuilder = New StringBuilder       ' Clear the string builder
                        ' Populate the cell in the row
                        mRow("Delete") = mStringBuilder.ToString
                        mStringBuilder = New StringBuilder       ' Clear the string builder
                        mStringBuilder.Append("Folder")
                        ' Populate the cell in the row
                        mRow("Type") = mStringBuilder.ToString
                        mStringBuilder = New StringBuilder       ' Clear the string builder
                        mStringBuilder.Append("N/A")
                        ' Populate the cell in the row
                        mRow("Size") = mStringBuilder.ToString
                        mStringBuilder = New StringBuilder       ' Clear the string builder
                        mStringBuilder.Append(Directory.GetLastWriteTime(path & mDirectorySeparatorChar.ToString() & mDirName).ToString(CultureInfo.InvariantCulture))
                        ' Populate the cell in the row
                        mRow("Modified") = mStringBuilder.ToString
                        mStringBuilder = New StringBuilder
                        mStringBuilder.Append(mDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture) & mDirName & "\")
                        mRow("FullName") = mStringBuilder.ToString
                        mRetTable.Rows.Add(mRow)        ' Add the row to the table
                    Next
                Catch ex As IOException
                    If mRetTable IsNot Nothing Then mRetTable.Dispose()
                    Dim mLoger As Logger = Logger.Instance
                    mLoger.Error(ex)
                    Throw
                End Try

            End If
            Try    ' Add all of the directories to the table
                Dim mDirInfo As New DirectoryInfo(path)
                Dim mFiles() As FileInfo
                mFiles = mDirInfo.GetFiles()
                Dim mFileInfo As FileInfo
                If mRetTable Is Nothing Then
                    mRetTable = New DataTable("MyTable")
                    mRetTable.Locale = CultureInfo.InvariantCulture
                    mRetTable.Columns.Add("Name", System.Type.GetType("System.String"))
                    mRetTable.Columns.Add("ShortFileName", System.Type.GetType("System.String"))
                    mRetTable.Columns.Add("Extension", System.Type.GetType("System.String"))
                    mRetTable.Columns.Add("Delete", System.Type.GetType("System.String"))
                    mRetTable.Columns.Add("Type", System.Type.GetType("System.String"))
                    mRetTable.Columns.Add("Size", System.Type.GetType("System.String"))
                    mRetTable.Columns.Add("Modified", System.Type.GetType("System.String"))
                    mRetTable.Columns.Add("FullName", System.Type.GetType("System.String"))
                    mRetTable.Columns("FullName").ReadOnly = True
                End If
                For Each mFileInfo In mFiles
                    mRow = mRetTable.NewRow()
                    Dim mFilename As String = mFileInfo.Name
                    Dim mShortFileName As String = mFileInfo.Name
                    mShortFileName = mFilename.Remove(Len(mFilename) - Len(mFileInfo.Extension), Len(mFileInfo.Extension))
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
                    Dim fileExtension As String = mFileInfo.Extension
                    mStringBuilder.Append(fileExtension)
                    mRow("Extension") = mStringBuilder.ToString

                    mStringBuilder = New StringBuilder
                    mStringBuilder.Append(mFileInfo.Length.ToFileSize())

                    mRow("Size") = mStringBuilder.ToString
                    mStringBuilder = New StringBuilder
                    mStringBuilder.Append(File.GetLastWriteTime(path + mDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture) + mFileInfo.Name).ToString(CultureInfo.InvariantCulture))
                    mRow("Modified") = mStringBuilder.ToString
                    mStringBuilder = New StringBuilder
                    mStringBuilder.Append(mFileInfo.FullName)
                    mRow("FullName") = mStringBuilder.ToString()

                    mRetTable.Rows.Add(mRow)
                Next
            Catch ex As IOException
                If Not mRetTable Is Nothing Then mRetTable.Dispose()
                Dim mLoger As Logger = Logger.Instance
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
            Dim mSorter As SortTable = New SortTable()
            mSorter.Sort(mRetTable, "Name", "ASC")
            Return mRetTable
        End Function

        ''' <summary>
        ''' Up loads file from an HtmlInputFile to the directory specified in the MDirectoryProfile object.
        ''' </summary>
        ''' <param name="uploadFile">HtmlInputFile</param>
        ''' <param name="currentDirectory">string</param>
        ''' <param name="directoryProfile">MDirectoryProfile</param>
        ''' <returns>string</returns>
        Public Function DoUpload(ByVal uploadFile As HttpPostedFile, ByVal currentDirectory As String, ByVal directoryProfile As MDirectoryProfile) As String
            Return DoUpload(Nothing, uploadFile, currentDirectory, directoryProfile)
        End Function

        ''' <summary>
        ''' Does the upload.
        ''' </summary>
        ''' <param name="fileName">Name of the file.</param>
        ''' <param name="uploadFile">The upload file.</param>
        ''' <param name="currentDirectory">The current dir.</param>
        ''' <param name="directoryProfile">The directory profile.</param>
        ''' <returns>System.String.</returns>
        ''' <exception cref="System.ArgumentNullException">directoryProfile;Can not be null reference (Nothing in Visual Basic)</exception>
        Public Function DoUpload(fileName As String, uploadFile As HttpPostedFile, currentDirectory As String, directoryProfile As MDirectoryProfile) As String
            If directoryProfile Is Nothing Then
                Throw New ArgumentNullException("directoryProfile", "Can not be null reference (Nothing in Visual Basic)")
            End If
            Dim mRetVal As String = "Upload successfull"
            Dim mDirectorySeparatorChar As Char = System.IO.Path.DirectorySeparatorChar
            Dim mImpersonatedUser As WindowsImpersonationContext = Nothing
            If (uploadFile IsNot Nothing) Then
                Try
                    If directoryProfile.Impersonate Then
                        mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword)
                    End If
                    Dim mFilename As String = uploadFile.FileName
                    If fileName IsNot Nothing Then
                        mFilename = fileName
                    End If
                    System.IO.Path.GetFileName(uploadFile.FileName)
                    uploadFile.SaveAs(currentDirectory & mDirectorySeparatorChar.ToString() & mFilename)
                Catch ex As IOException
                    Dim mLog As Logger = Logger.Instance()
                    mLog.[Error](ex)
                    mRetVal = "Failed uploading file"
                Finally
                    If directoryProfile.Impersonate Then
                        ' Stop impersonating the user.
                        If (mImpersonatedUser IsNot Nothing) Then
                            mImpersonatedUser.Undo()
                        End If
                    End If
                End Try
            Else
                mRetVal = "fileToUpload can not be null or Nothing."
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
                Throw New ArgumentNullException("directoryProfile", "directoryProfile can not be null.")
            End If
            Dim mRetVal As String
            mRetVal = "Successfully created the new directory!"
            Dim mImpersonatedUser As WindowsImpersonationContext = Nothing
            Try
                If Not directoryProfile Is Nothing Then
                    If directoryProfile.Impersonate Then
                        mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword)
                    End If
                    Directory.CreateDirectory(currentDirectory & "\" & newDirectory)
                End If
            Catch ex As IOException
                Dim mLoger As Logger = Logger.Instance
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
                Throw New ArgumentNullException("directoryProfile", "directoryProfile can not be null.")
            End If
            Dim mRetVal As String
            Dim mImpersonatedUser As WindowsImpersonationContext = Nothing
            mRetVal = "Successfully deleted the directory(s)"
            Try
                If Not directoryProfile Is Nothing Then
                    If directoryProfile.Impersonate Then
                        mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword)
                    End If
                    Directory.Delete(currentDirectory)
                End If
            Catch ex As IOException
                Dim mLoger As Logger = Logger.Instance
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
                    mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword)
                End If
                File.Delete(fileName)
            Catch ex As IOException
                Dim mLog As Logger = Logger.Instance()
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
                    mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword)
                End If
                File.Move(sourceFileName, destinationfileName)
            Catch ex As IOException
                Dim mLog As Logger = Logger.Instance()
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
                    mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword)
                End If
                Directory.Move(sourceDirectoryName, destinationDirectoryName)
            Catch ex As IOException
                Dim mLog As Logger = Logger.Instance()
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
        ''' Gets the line count.
        ''' </summary>
        ''' <param name="theDirectory">The dir.</param>
        ''' <param name="level">An int representing the level.</param>
        ''' <param name="stringBuilder">The string builder.</param>
        ''' <param name="excludeList">The exclude list.</param>
        ''' <param name="directoryLineCount">The directory line count.</param>
        ''' <param name="totalLinesOfCode">The total lines of code.</param>
        ''' <param name="fileArray">The file array.</param>
        ''' <returns>System.String.</returns>
        Public Function GetLineCount(ByVal theDirectory As DirectoryInfo, ByVal level As Integer, ByRef stringBuilder As StringBuilder, ByVal excludeList As List(Of String), ByRef directoryLineCount As Integer, ByRef totalLinesOfCode As Integer, ByVal fileArray As String()) As String
            Dim subDirectories As DirectoryInfo() = Nothing
            Try
                subDirectories = theDirectory.GetDirectories()
                Dim x As Integer = 0
                Dim numDirectories As Integer = subDirectories.Length - 1
                If directoryLineCount > 0 Then
                    totalLinesOfCode += totalLinesOfCode
                    stringBuilder.AppendLine("<br>Lines of code for " + theDirectory.Name + " " + directoryLineCount)
                    stringBuilder.AppendLine("<br>Lines of so far " + totalLinesOfCode)
                    directoryLineCount = 0
                End If
                For x = 0 To numDirectories
                    If subDirectories(x).Name.Trim.ToUpper((CultureInfo.InvariantCulture)) <> "BIN" AndAlso subDirectories(x).Name.Trim.ToUpper(CultureInfo.InvariantCulture) <> "DEBUG" AndAlso subDirectories(x).Name.Trim.ToUpper(CultureInfo.InvariantCulture) <> "RELEASE" Then
                        CountDirectory(subDirectories(x), stringBuilder, excludeList, fileArray, directoryLineCount)
                        If (directoryLineCount > 0) Then
                            totalLinesOfCode += directoryLineCount
                            stringBuilder.AppendLine("<br>Lines of code for " + subDirectories(x).Name.ToString(CultureInfo.InvariantCulture) + " " + directoryLineCount.ToString(CultureInfo.InvariantCulture))
                            stringBuilder.AppendLine("<br>Lines of so far " + totalLinesOfCode.ToString(CultureInfo.InvariantCulture))
                            directoryLineCount = 0
                        End If
                    End If
                    GetLineCount(subDirectories(x), level + 1, stringBuilder, excludeList, directoryLineCount, totalLinesOfCode, fileArray)
                Next

            Catch
                stringBuilder.AppendLine("Directory not found")
            End Try
            Return stringBuilder.ToString()
        End Function

        ''' <summary>
        ''' Counts the directory.
        ''' </summary>
        ''' <param name="theDirectory">The directory.</param>
        ''' <param name="stringBuilder">The string builder.</param>
        ''' <param name="excludeList">The exclude list.</param>
        ''' <param name="fileArray">The file array.</param>
        ''' <param name="directoryLineCount">The directory line count.</param>
        Public Sub CountDirectory(ByVal theDirectory As DirectoryInfo, ByRef stringBuilder As StringBuilder, ByVal excludeList As List(Of String), ByVal fileArray As String(), ByRef directoryLineCount As Integer)
            Dim sFileType As [String]
            Dim writeDirectory As Boolean = True
            Dim FileLineCount As Integer = 0
            Dim mCountFile As Boolean = True
            For Each sFileType In fileArray
                ' this loops files
                Dim directoryFile As FileInfo
                For Each directoryFile In theDirectory.GetFiles(sFileType.Trim)
                    mCountFile = True
                    For Each item In excludeList
                        If directoryFile.Name.ToUpper(CultureInfo.InvariantCulture).IndexOf(item.Trim().ToUpper(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase) <> -1 Then
                            mCountFile = False
                            Exit For
                        End If
                    Next
                    If mCountFile Then
                        ' open files for streamreader
                        Dim mStreamReader As StreamReader = File.OpenText(directoryFile.FullName)
                        'loop until the end
                        While mStreamReader.Peek() > -1
                            Dim myString As String = mStreamReader.ReadLine
                            If (Not myString.Trim.StartsWith("'", StringComparison.OrdinalIgnoreCase) Or Not myString.Trim.StartsWith("//", StringComparison.OrdinalIgnoreCase)) And Not myString.Trim.Length = 0 Then
                                FileLineCount += 1
                            End If
                        End While
                        'close the streamreader
                        mStreamReader.Close()
                        If FileLineCount > 0 Then
                            If writeDirectory Then
                                stringBuilder.AppendLine("<br>" + theDirectory.FullName)
                                writeDirectory = False
                            End If
                            stringBuilder.AppendLine("<br>" + s_Space + directoryFile.Name.ToString(CultureInfo.InvariantCulture) + " " + FileLineCount.ToString(CultureInfo.InvariantCulture))
                        End If
                        If FileLineCount > 0 Then
                            directoryLineCount += FileLineCount
                        End If
                        FileLineCount = 0
                    End If
                Next directoryFile
            Next sFileType
        End Sub

        <Extension()>
        Public Function ToFileSize(ByVal source As Integer) As String
            Return ToFileSize(Convert.ToInt64(source))
        End Function

        <Extension()>
        Public Function ToFileSize(ByVal source As Long) As String
            Const byteConversion As Integer = 1024
            Dim bytes As Double = Convert.ToDouble(source)
            If bytes >= Math.Pow(byteConversion, 3) Then 'GB Range
                Return String.Concat(Math.Round(bytes / Math.Pow(byteConversion, 3), 2), " GB")
            ElseIf bytes >= Math.Pow(byteConversion, 2) Then 'MB Range
                Return String.Concat(Math.Round(bytes / Math.Pow(byteConversion, 2), 2), " MB")
            ElseIf bytes >= byteConversion Then 'KB Range
                Return String.Concat(Math.Round(bytes / byteConversion, 2), " KB")
            Else 'Bytes
                Return String.Concat(bytes, " Bytes")
            End If
        End Function
    End Module
End Namespace
