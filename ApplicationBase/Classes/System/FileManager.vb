Imports ApplicationBase.Common.Logging
Imports ApplicationBase.Model.Directories
Imports System.IO
Imports System.Text
Imports System.Security.Principal
Imports System.Data

#Region " Notes "
' The FileManager class gives access to a file structure both
' for the local as well as a file server elsewhere in the network.
' Pay close attention to windows impersonation.
#End Region
Public Class FileManager
	Public Shared Function GetParent(ByVal pPath As String) As String
		Dim retVal As String
		Dim dirInfo As New DirectoryInfo(pPath)
		retVal = dirInfo.Parent.FullName.ToString
		Return retVal
	End Function

    Public Shared Function GetDirectoryTableData(ByVal pPath As String, ByRef Server As Object, ByVal directoryInfo As MDirectoryProfileInformation, Optional ByVal filesOnly As Boolean = False) As DataTable
        Dim oTable As New DataTable("MyTable")
        Dim oRow As DataRow = oTable.NewRow()
        Dim sb As New StringBuilder(4096)
        Dim dirs() As String
        Dim iKiloBytes As Integer = 1024
        Dim iMegaBytes As Integer = iKiloBytes * 1024
        Dim iGigaBytes As Integer = iMegaBytes * 1024
        'Dim iTeraBytes As Int64 = iGigaBytes * 1024
        Dim directorySeparatorChar As Char = System.IO.Path.DirectorySeparatorChar
		Dim impersonatedUser As WindowsImpersonationContext = Nothing
        If directoryInfo.Impersonate Then
            impersonatedUser = ImpersonateNow(impersonatedUser, directoryInfo)
        End If

        ' Add the column header

        oTable.Columns.Add("Name", System.Type.GetType("System.String"))
        oTable.Columns.Add("ShortFileName", System.Type.GetType("System.String"))
        oTable.Columns.Add("Extension", System.Type.GetType("System.String"))
        oTable.Columns.Add("Delete", System.Type.GetType("System.String"))
        oTable.Columns.Add("Type", System.Type.GetType("System.String"))
        oTable.Columns.Add("Size", System.Type.GetType("System.String"))
        oTable.Columns.Add("Modified", System.Type.GetType("System.String"))
        oTable.Columns.Add("FullName", System.Type.GetType("System.String"))
        oTable.Columns("FullName").ReadOnly = True

        oRow("Name") = sb.ToString
        sb = New StringBuilder    ' Clear the string builder
        If Not filesOnly Then
            Try
                dirs = Directory.GetDirectories(pPath)
                Dim d As String
                For Each d In dirs
                    Dim dirName As String = System.IO.Path.GetFileName(d)
                    sb = New StringBuilder       ' Clear the string builder
                    oRow = oTable.NewRow()       ' Create a new row
                    ' Populate the string for the new row
                    sb.Append(dirName)
                    ' Populate the cell in the row
                    oRow("Name") = sb.ToString

                    sb = New StringBuilder       ' Clear the string builder
                    oRow("ShortFileName") = sb.ToString

                    sb = New StringBuilder       ' Clear the string builder
                    oRow("Extension") = sb.ToString

                    sb = New StringBuilder       ' Clear the string builder
                    ' Populate the cell in the row
                    oRow("Delete") = sb.ToString
                    sb = New StringBuilder       ' Clear the string builder
                    sb.Append("Folder")
                    ' Populate the cell in the row
                    oRow("Type") = sb.ToString
                    sb = New StringBuilder       ' Clear the string builder
                    sb.Append("N/A")
                    ' Populate the cell in the row
                    oRow("Size") = sb.ToString
                    sb = New StringBuilder       ' Clear the string builder
                    sb.Append(Directory.GetLastWriteTime(pPath & directorySeparatorChar.ToString() & dirName).ToString())
                    ' Populate the cell in the row
                    oRow("Modified") = sb.ToString
                    sb = New StringBuilder
                    sb.Append(directorySeparatorChar.ToString() & dirName & "\")
                    oRow("FullName") = sb.ToString
                    oTable.Rows.Add(oRow)       ' Add the row to the table
                Next
            Catch ex As Exception
                Throw ex
            End Try

        End If
        Try    ' Add all of the directories to the table
            Dim dirInfo As New DirectoryInfo(pPath)
            Dim files() As FileInfo
            files = dirInfo.GetFiles()
            Dim f As FileInfo
            For Each f In files
                oRow = oTable.NewRow()
                Dim filename As String = f.Name
                Dim shortFileName As String = f.Name
                shortFileName = filename.Remove(Len(filename) - Len(f.Extension), Len(f.Extension))
                Dim sFileSize As String
				sb = New StringBuilder
                sb.Append(filename)
                oRow("Name") = sb.ToString
                sb = New StringBuilder
                sb.Append("File")
                oRow("Type") = sb.ToString

                sb = New StringBuilder
                sb.Append(shortFileName)
                oRow("shortFileName") = sb.ToString

                sb = New StringBuilder
                Dim fileExtension As String = f.Extension
                sb.Append(fileExtension)
                oRow("Extension") = sb.ToString

                sb = New StringBuilder
                ' Show size in either KB or MB.  GB files will cause a problem
                If (f.Length / iKiloBytes) <= iKiloBytes Then
                    sFileSize = f.Length / iKiloBytes
                    sb.Append(FormatNumber(sFileSize, 1, , , TriState.True) & " KB")
                ElseIf (f.Length / iMegaBytes) <= iMegaBytes Then
                    sFileSize = f.Length / iMegaBytes
                    sb.Append(FormatNumber(sFileSize, 1, , , TriState.True) & " MB")
                Else
                    sFileSize = f.Length / iGigaBytes
                    sb.Append(FormatNumber(sFileSize, 1, , , TriState.True) & " GB")
                End If

                oRow("Size") = sb.ToString
                sb = New StringBuilder
                sb.Append(File.GetLastWriteTime(pPath & _
                  directorySeparatorChar.ToString() & f.Name).ToString())
                oRow("Modified") = sb.ToString
                sb = New StringBuilder
                sb.Append(f.FullName)
                oRow("FullName") = sb.ToString

                oTable.Rows.Add(oRow)
            Next
        Catch ex As Exception
            Throw ex
        Finally
            If directoryInfo.Impersonate Then
                ' Stop impersonating the user.
                If Not impersonatedUser Is Nothing Then
                    impersonatedUser.Undo()
                End If
            End If
        End Try
        ' Return the table object as the data source
        Return oTable
    End Function

    Public Shared Function DoUpload(ByRef FileToUpload As Object, ByVal currentDir As String, ByVal directoryInfo As MDirectoryProfileInformation) As String
        Dim sMessage As String = "Upload successfull"
        Dim directorySeparatorChar As Char = System.IO.Path.DirectorySeparatorChar
		Dim impersonatedUser As WindowsImpersonationContext = Nothing
        If Not (FileToUpload.PostedFile Is Nothing) Then
            Try
                If directoryInfo.Impersonate Then
                    impersonatedUser = ImpersonateNow(impersonatedUser, directoryInfo)
                End If

                Dim postedFile = FileToUpload.PostedFile
                Dim filename As String = System.IO.Path.GetFileName(postedFile.FileName)
                Dim contentType As String = postedFile.ContentType
                Dim contentLength As Integer = postedFile.ContentLength

                postedFile.SaveAs(currentDir & _
                  directorySeparatorChar.ToString() & filename)
            Catch ex As Exception
                sMessage = "Failed uploading file"
            Finally
                If directoryInfo.Impersonate Then
                    ' Stop impersonating the user.
                    If Not impersonatedUser Is Nothing Then
                        impersonatedUser.Undo()
                    End If
                End If
            End Try
        End If
        Return sMessage
    End Function

    Public Shared Function CreateDirectory(ByVal pCurrentDirectory As String, ByVal pNewDirectory As String, ByVal directoryInfo As MDirectoryProfileInformation) As String
        Dim retVal As String
        retVal = "Successfully created the new directory!"
		Dim impersonatedUser As WindowsImpersonationContext = Nothing
        Try
            If directoryInfo.Impersonate Then
                impersonatedUser = ImpersonateNow(impersonatedUser, directoryInfo)
            End If
            Directory.CreateDirectory(pCurrentDirectory & "\" & pNewDirectory)
        Catch ex As Exception
            retVal = ex.Message.ToString
        Finally
            If directoryInfo.Impersonate Then
                ' Stop impersonating the user.
                If Not impersonatedUser Is Nothing Then
                    impersonatedUser.Undo()
                End If
            End If
        End Try
        Return retVal
    End Function

    Public Shared Function DeleteDirectory(ByVal pCurrentDirectory As String, ByVal directoryInfo As MDirectoryProfileInformation) As String
        Dim retVal As String
		Dim impersonatedUser As WindowsImpersonationContext = Nothing
        retVal = "Successfully deleted the directory(s)"
        Try
            If directoryInfo.Impersonate Then
                impersonatedUser = ImpersonateNow(impersonatedUser, directoryInfo)
            End If
            Directory.Delete(pCurrentDirectory)
        Catch ex As Exception
            retVal = ex.Message.ToString
        Finally
            If directoryInfo.Impersonate Then
                ' Stop impersonating the user.
                If Not impersonatedUser Is Nothing Then
                    impersonatedUser.Undo()
                End If
            End If
        End Try
        Return retVal
    End Function

    Public Shared Function DeleteFile(ByVal pFile As String, ByVal directoryInfo As MDirectoryProfileInformation) As String
        Dim retVal As String
        retVal = "Successfully deleted the file(s)"
		Dim impersonatedUser As WindowsImpersonationContext = Nothing
        Try
            If directoryInfo.Impersonate Then
                impersonatedUser = ImpersonateNow(impersonatedUser, directoryInfo)
            End If
            File.Delete(pFile)
        Catch ex As Exception
            retVal = ex.Message.ToString
        Finally
            If directoryInfo.Impersonate Then
                ' Stop impersonating the user.
                If Not impersonatedUser Is Nothing Then
                    impersonatedUser.Undo()
                End If
            End If
        End Try
        Return retVal
    End Function

    Public Shared Function RenameFile(ByVal pSourceFileName As String, ByVal pDestfileName As String, ByVal directoryInfo As MDirectoryProfileInformation) As String
        Dim retVal As String
        retVal = "Successfully renamed the file!"
		Dim impersonatedUser As WindowsImpersonationContext = Nothing
        Try
            If directoryInfo.Impersonate Then
                impersonatedUser = ImpersonateNow(impersonatedUser, directoryInfo)
            End If
            File.Move(pSourceFileName, pDestfileName)
        Catch ex As Exception
            retVal = ex.Message.ToString
        Finally
            If directoryInfo.Impersonate Then
                ' Stop impersonating the user.
                If Not impersonatedUser Is Nothing Then
                    impersonatedUser.Undo()
                End If
            End If
        End Try
        Return retVal
    End Function

    Public Shared Function RenameDirectory(ByVal pSourceDirectoryName As String, ByVal pDestDirectoryName As String, ByVal directoryInfo As MDirectoryProfileInformation) As String
        Dim retVal As String
		Dim impersonatedUser As WindowsImpersonationContext = Nothing
        retVal = "Successfully renamed the directory!"
        Try
            If directoryInfo.Impersonate Then
                impersonatedUser = ImpersonateNow(impersonatedUser, directoryInfo)
            End If
            Directory.Move(pSourceDirectoryName, pDestDirectoryName)
        Catch ex As Exception
            retVal = ex.Message.ToString
        Finally
            If directoryInfo.Impersonate Then
                ' Stop impersonating the user.
                If Not impersonatedUser Is Nothing Then
                    impersonatedUser.Undo()
                End If
            End If
        End Try
        Return retVal
    End Function

    Public Shared Function ImpersonateNow(ByVal impersonatedUser As WindowsImpersonationContext, ByVal directoryInfo As MDirectoryProfileInformation) As WindowsImpersonationContext
        Dim tokenHandle As New IntPtr(0)
        Dim dupeTokenHandle As New IntPtr(0)
        Dim myImpersonate As New WebImpersonate

        'Dim windowsIdentity As WindowsIdentity = windowsIdentity.GetCurrent()
        Dim userName As String = String.Empty
        Dim domainName As String = String.Empty
        Dim posSlash As Integer = InStr(directoryInfo.Impersonate_Account, "\")
        If posSlash <> 0 Then
            userName = directoryInfo.Impersonate_Account
            domainName = directoryInfo.Impersonate_Account
            userName = userName.Remove(0, posSlash)
            domainName = domainName.Remove((posSlash - 1), (directoryInfo.Impersonate_Account.Length - posSlash) + 1)
        Else
            userName = directoryInfo.Impersonate_Account
        End If
        Dim returnValue As Boolean = WebImpersonate.LogonUser(userName, domainName, directoryInfo.Impersonate_PWD, myImpersonate.LOGON32_LOGON_INTERACTIVE, myImpersonate.LOGON32_PROVIDER_DEFAULT, tokenHandle)
        If Not returnValue Then
            Dim mike As String = WebImpersonate.GetErrorMessage(Err.LastDllError)
            Dim log As AppLogger = AppLogger.GetInstance
            log.Fatal("Could not impersonate user: " & userName & " for domain " & domainName)
            Throw New Exception("Could not read the directory")
        End If
        Dim retVal As Boolean = WebImpersonate.DuplicateToken(tokenHandle, myImpersonate.SecurityImpersonation, dupeTokenHandle)
        If Not retVal Then
            WebImpersonate.CloseHandle(tokenHandle)
            Throw New Exception("Exception thrown in trying to duplicate token.")
        End If
        ' The token that is passed to the following constructor must 
        ' be a primary token in order to use it for impersonation.
        Dim newId As New WindowsIdentity(dupeTokenHandle)
        impersonatedUser = newId.Impersonate()
        Return impersonatedUser
    End Function
End Class