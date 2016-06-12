Imports System.Web
Imports System.Web.Services
Imports GrowthWare.WebSupport
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports System.IO
Imports GrowthWare.Framework.Common

Public Class UploadHandler
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim mResponse As String = "Success"
        Try
            Dim mAction As String = GWWebHelper.GetQueryValue(context.Request, "Action")
            Dim mCurrentDirectory As String = GWWebHelper.GetQueryValue(context.Request, "CurrentDirectory")
            Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(mAction)
            Dim mDirectoryInfo As MDirectoryProfile = DirectoryUtility.GetProfile(mFunctionProfile.Id)
            Dim mDirectory As String = mDirectoryInfo.Directory
            If Not String.IsNullOrEmpty(mCurrentDirectory) Then
                If Not mDirectory.LastIndexOf(Path.DirectorySeparatorChar) = mDirectory.Length Then
                    mDirectory += Path.DirectorySeparatorChar + mCurrentDirectory
                End If
            End If
            Dim mUploadDirectory As String = String.Empty
            Try
                mUploadDirectory = Path.DirectorySeparatorChar + context.Request.Files.AllKeys(0).ToString().Substring(0, context.Request.Files.AllKeys(0).LastIndexOf("."))

            Catch
                mUploadDirectory = context.Request("fileName").ToString()
                mUploadDirectory = mUploadDirectory.Substring(0, mUploadDirectory.LastIndexOf("."))
                mUploadDirectory = Path.DirectorySeparatorChar + mUploadDirectory
            End Try
            If Not Directory.Exists(mUploadDirectory) Then
                FileUtility.CreateDirectory(mDirectory, mUploadDirectory, mDirectoryInfo)
            End If

            mUploadDirectory = mDirectory + mUploadDirectory
            If context.Request("completed") = Nothing Then
                If Not mDirectoryInfo Is Nothing Then
                    FileUtility.DoUpload(context.Request.Files.AllKeys(0), context.Request.Files(0), mUploadDirectory, mDirectoryInfo)
                    If Not context.Request("single") Is Nothing Then
                        Dim mFileName As String = context.Request.Files(0).FileName()
                        Dim mNewpath As String = Path.Combine(mUploadDirectory, mFileName)
                        FileUtility.RenameFile(mNewpath, mDirectory + mFileName, mDirectoryInfo)
                        FileUtility.DeleteDirectory(mUploadDirectory, mDirectoryInfo)
                    End If
                End If
            Else
                If context.Request("completed").ToString().ToLowerInvariant() = "true" Then
                    Dim mFileName As String = context.Request("fileName").ToString()
                    Dim mPath As String = mUploadDirectory
                    Dim mNewpath As String = Path.Combine(mPath, mFileName)
                    Dim mDirectorySeparatorChar As Char = Path.DirectorySeparatorChar
                    If Not mDirectoryInfo Is Nothing Then
                        Dim mFileTable As DataTable = FileUtility.GetDirectoryTableData(mUploadDirectory, mDirectoryInfo, True)
                        Dim mSorter As SortTable = New SortTable()
                        Dim mColName As String = "Name"
                        mSorter.Sort(mFileTable, mColName, "ASC")
                        Dim mFiles As DataView = mFileTable.DefaultView
                        mFiles.RowFilter = "Name like '" + mFileName.Substring(0, mFileName.Length - 4) + "%'"
                        For Each rowView As DataRowView In mFiles
                            Dim row As DataRow = rowView.Row
                            Dim mPartialFileName As String = mUploadDirectory + mDirectorySeparatorChar.ToString() + row("Name").ToString()
                            If mPartialFileName.EndsWith("_UploadNumber_1") Then
                                If File.Exists(mNewpath) Then
                                    File.Delete(mNewpath)
                                End If
                            End If
                            If Not mPartialFileName = mNewpath Then
                                mergeFiles(mNewpath, mPartialFileName)
                            End If
                        Next
                        FileUtility.RenameFile(mNewpath, mDirectory + mFileName, mDirectoryInfo)
                        FileUtility.DeleteDirectory(mUploadDirectory, mDirectoryInfo)
                    End If
                End If
            End If
        Catch ex As Exception
            Dim mLog As Logger = Logger.Instance
            mLog.Error(ex)
            mResponse = "Error"
        End Try
        context.Response.ContentType = "text/plain"
        context.Response.Write(mResponse)

    End Sub

    Private Shared Sub mergeFiles(file1 As String, file2 As String)
        Dim fs1 As FileStream = Nothing
        Dim fs2 As FileStream = Nothing
        Try
            fs1 = File.Open(file1, FileMode.Append)
            fs2 = File.Open(file2, FileMode.Open)
            Dim fs2Content As Byte() = New Byte(fs2.Length - 1) {}
            fs2.Read(fs2Content, 0, CInt(fs2.Length))
            fs1.Write(fs2Content, 0, CInt(fs2.Length))
        Catch ex As Exception
            Dim mLog As Logger = Logger.Instance
            mLog.Error(ex)
        Finally
            fs1.Close()
            fs2.Close()
            File.Delete(file2)
        End Try
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class