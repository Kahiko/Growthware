Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Web.Http
Imports System.Web.Script.Serialization
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Hosting

Namespace Controllers
    Public Class FileManagerController
        Inherits ApiController

        Private m_Log As Logger = Logger.Instance()

        <HttpPost>
        Public Function CreateDirectory(<FromUri()> ByVal currentDirectoryString As String, <FromUri()> ByVal newDirectory As String, <FromUri()> ByVal currentAction As String) As IHttpActionResult
            Dim mRetVal As String = "Unable to create directory"

            If String.IsNullOrEmpty(currentDirectoryString) Or currentAction Is Nothing Or String.IsNullOrEmpty(newDirectory) Then
                mRetVal = "All parameters must be passed!"
                Dim ex As ArgumentException = New ArgumentException(mRetVal)
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(mRetVal)
                Throw ex
            End If
            Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(currentAction)
            Dim mFunctionSeqId = mFunctionProfile.FunctionTypeSeqId
            Dim mServer As HttpServerUtility = HttpContext.Current.Server
            Dim mDirectoryProfile As MDirectoryProfile = DirectoryUtility.GetProfile(mFunctionSeqId)
            Dim mCurrentDirectory As String = mDirectoryProfile.Directory
            If currentDirectoryString.Length > 0 Then
                mCurrentDirectory += "\" + currentDirectoryString
            End If
            mRetVal = FileUtility.CreateDirectory(mServer.UrlDecode(mCurrentDirectory), mServer.UrlDecode(newDirectory), mDirectoryProfile)
            Return Me.Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function DeleteFiles(<FromBody> ByVal filesToDelete As List(Of MUIFileInfo)) As IHttpActionResult
            If filesToDelete Is Nothing Or filesToDelete.Count = 0 Then Throw New ArgumentNullException("filesToDelete", "'filesToDelete' cannot be a null reference (Nothing in Visual Basic)!")
            Dim mServer As HttpServerUtility = HttpContext.Current.Server
            Dim mRetVal As String = "Done"
            Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(filesToDelete(0).Action)
            Dim mSecurityInfo As MSecurityInfo = Me.GetSecurityInfo(mFunctionProfile.Id)
            Dim mDirectoryProfile As MDirectoryProfile = DirectoryUtility.GetProfile(mFunctionProfile.Id)
            Dim mCurrentDirectory As String = mServer.UrlDecode(filesToDelete(0).CurrentDirectory)
            Dim mRootDirectory As String = mDirectoryProfile.Directory
            mCurrentDirectory = mCurrentDirectory.Replace("/", Path.DirectorySeparatorChar)
            mCurrentDirectory = mCurrentDirectory.Replace("\", Path.DirectorySeparatorChar)
            mRootDirectory = mRootDirectory.Replace("/", Path.DirectorySeparatorChar)
            mRootDirectory = mRootDirectory.Replace("\", Path.DirectorySeparatorChar)

            If mCurrentDirectory.Length > 1 And Not mCurrentDirectory.EndsWith(Path.DirectorySeparatorChar) Then
                mCurrentDirectory += Path.DirectorySeparatorChar
            End If
            If Not mRootDirectory.EndsWith(Path.DirectorySeparatorChar) Then
                mRootDirectory += Path.DirectorySeparatorChar
            End If
            If mCurrentDirectory.Length > 1 Then
                mRootDirectory += mCurrentDirectory
            End If
            If mSecurityInfo.MayDelete Then
                For Each item As MUIFileInfo In filesToDelete
                    Select Case item.FileType
                        Case "File"
                            Dim mFileName As String = mRootDirectory + mServer.UrlDecode(item.FileName)
                            mRetVal = FileUtility.DeleteFile(mFileName, mDirectoryProfile)
                            If mRetVal.IndexOf("Successfully") = -1 Then Exit For
                        Case "Folder"
                            Dim mDirectoryToDelete = mRootDirectory + mServer.UrlDecode(item.FileName)
                            Try
                                mRetVal = FileUtility.DeleteDirectory(mDirectoryToDelete, mDirectoryProfile)
                            Catch ex As Exception
                                mRetVal = ex.Message
                            End Try
                            If mRetVal.IndexOf("Successfully") = -1 Then Exit For
                        Case Else

                    End Select
                Next
                Return Me.Ok(mRetVal)
            Else
                m_Log.Error(String.Format("Account '{0}' does not have permission to delete file(s) in: '{1}'", AccountUtility.CurrentProfile().Account, mRootDirectory))
                Return Me.BadRequest("Account does not have permission.")
            End If
        End Function

        <HttpPost>
        Public Function DownloadFile(<FromBody> ByVal requestDirectory As MUIRequestDirectory, <FromUri> ByVal fileName As String) As HttpResponseMessage
            If requestDirectory Is Nothing Then Throw New ArgumentNullException("requestDirectory", "'requestDirectory' cannot be a null reference (Nothing in Visual Basic)!")
            Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(requestDirectory.Action)
            Dim mSecurityInfo As MSecurityInfo = Me.GetSecurityInfo(mFunctionProfile.Id)
            Dim mHttpResponseMessage As HttpResponseMessage
            If (mSecurityInfo.MayView) Then
                Dim mDirectoryProfile As MDirectoryProfile = DirectoryUtility.GetProfile(mFunctionProfile.Id)
                Dim mDirectoryPath As String = mDirectoryProfile.Directory + requestDirectory.CurrentDirectoryString
                mDirectoryPath = mDirectoryPath.Replace("/", Path.DirectorySeparatorChar)
                If Not mDirectoryPath.EndsWith(Path.DirectorySeparatorChar) Then
                    mDirectoryPath += Path.DirectorySeparatorChar
                End If
                Dim mFileNameWithPath As String = mDirectoryPath + fileName
                mHttpResponseMessage = New HttpResponseMessage(HttpStatusCode.OK)
                Dim mFileStream As New FileStream(mFileNameWithPath, FileMode.Open, FileAccess.Read)
                ' Set the Response Content.
                mHttpResponseMessage.Content = New StreamContent(mFileStream)
                ' Set the File Content Type.
                mHttpResponseMessage.Content.Headers.ContentType = New MediaTypeHeaderValue(MimeMapping.GetMimeMapping(mFileNameWithPath))
                ' Set the Content Disposition Header Value and FileName.
                mHttpResponseMessage.Content.Headers.ContentDisposition = New ContentDispositionHeaderValue("attachment")
                mHttpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName
            Else
                mHttpResponseMessage = New HttpResponseMessage(HttpStatusCode.Forbidden)
            End If
            Return mHttpResponseMessage
        End Function

        ' POST api/FileManager/GetDirectory?Action=xx - requestDirectory
        <HttpPost>
        Public Function GetDirectory(<FromBody> ByVal requestDirectory As MUIRequestDirectory) As Collection(Of MUIDirectoryData)
            If requestDirectory Is Nothing Then Throw New ArgumentNullException("requestDirectory", "'requestDirectory' cannot be a null reference (Nothing in Visual Basic)!")
            Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(requestDirectory.Action)
            Dim mSecurityInfo As MSecurityInfo = Me.GetSecurityInfo(mFunctionProfile.Id)
            Dim mRetVal As New Collection(Of MUIDirectoryData)
            If mSecurityInfo.MayView Then
                Dim mCriteria As MSearchCriteria = requestDirectory.SearchCriteria
                Dim mDirectoryProfile As MDirectoryProfile = DirectoryUtility.GetProfile(mFunctionProfile.Id)
                Dim mDirectoryPath As String = mDirectoryProfile.Directory + requestDirectory.CurrentDirectoryString
                Dim mDataTable As DataTable = FileUtility.GetDirectoryTableData(mDirectoryPath, mDirectoryProfile, False)
                Dim mSorter As New SortTable()
                Dim mColName As String = mCriteria.OrderByColumn
                mSorter.Sort(mDataTable, mColName, mCriteria.OrderByDirection)
                Dim mView As DataView = mDataTable.DefaultView
                mView.Sort = "type desc"
                mDataTable = DataHelper.GetTable(mView)
                Dim mSort As String = "type desc, " + mCriteria.OrderByColumn + " " + mCriteria.OrderByDirection
                mDataTable = DataHelper.GetPageOfData(mDataTable, mSort, mCriteria)
                If mDataTable IsNot Nothing And mDataTable.Rows.Count > 0 Then
                    Dim mDataView As DataView = mDataTable.DefaultView()
                    For Each mRow As DataRow In mDataView.ToTable.Rows
                        Dim mItem As New MUIDirectoryData
                        With mItem
                            .Delete = mSecurityInfo.MayDelete
                            .Modified = mRow("Modified").ToString()
                            .Name = mRow("Name").ToString()
                            .Size = mRow("Size").ToString()
                            .TotalRows = mRow(DataHelper.TotalRowColumnName).ToString()
                            .Type = mRow("Type").ToString()
                        End With
                        mRetVal.Add(mItem)
                    Next
                End If
            End If
            Return mRetVal
        End Function

        <HttpPost>
        Public Function GetDirectoryLinks(<FromBody> ByVal requestDirectoryLinks As MUIRequestDirectoryLinksInfo) As IHttpActionResult
            Dim mRetVal As String = String.Empty
            If Me.GetSecurityInfo(requestDirectoryLinks.FunctionSeqId).MayView Then
                mRetVal = FileUtility.GetDirectoryLinks(requestDirectoryLinks.CurrentDirectoryString, requestDirectoryLinks.FunctionSeqId)
            End If
            Return Me.Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function GetLineCount(<FromBody> ByVal countInfo As CountInfo) As String
            Dim mRetVal As String = String.Empty
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
            Try
                mRetVal = FileUtility.GetLineCount(currentDirectory, 0, mSB, mExcludeList, mDirectoryLineCount, mTotalLinesOfCode, mFileArray).ToString()
            Catch ex As Exception
                mRetVal = ex.Message
            End Try
            Return mRetVal
        End Function

        Private Function GetSecurityInfo(ByVal functionSeqId As Integer) As MSecurityInfo
            Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(functionSeqId)
            Dim mRetVal As MSecurityInfo = New MSecurityInfo(mFunctionProfile, AccountUtility.CurrentProfile())
            Return mRetVal
        End Function

        <HttpPost>
        Public Function GetTestNaturalSort(sortDirection As String) As MUITestNaturalSort
            Dim mRetVal As MUITestNaturalSort = New MUITestNaturalSort()
            Dim oTable As New DataTable("MyTable")
            oTable.Columns.Add("COL1", System.Type.GetType("System.String"))
            oTable.Columns.Add("COL2", System.Type.GetType("System.String"))
            Dim oRow As DataRow = oTable.NewRow()
            oRow = oTable.NewRow()
            oRow("COL1") = "Chapter(10)"
            oRow("COL2") = "Chapter(10)"
            oTable.Rows.Add(oRow)
            oRow = oTable.NewRow()
            oRow("COL1") = "Chapter 2 Ep 2-3"
            oRow("COL2") = "Chapter 2 Ep 2-3"
            oTable.Rows.Add(oRow)
            oRow = oTable.NewRow()
            oRow("COL1") = "Chapter 2 Ep 1-2"
            oRow("COL2") = "Chapter 2 Ep 1-2"
            oTable.Rows.Add(oRow)
            oRow = oTable.NewRow()
            oRow("COL1") = ""
            oRow("COL2") = ""
            oTable.Rows.Add(oRow)
            oRow = oTable.NewRow()
            oRow("COL1") = "Rocky(IV)"
            oRow("COL2") = "Rocky(IV)"
            oTable.Rows.Add(oRow)
            oRow = oTable.NewRow()
            oRow("COL1") = "Chapter(1)"
            oRow("COL2") = "Chapter(1)"
            oTable.Rows.Add(oRow)
            oRow = oTable.NewRow()
            oRow("COL1") = "Chapter(11)"
            oRow("COL2") = "Chapter(11)"
            oTable.Rows.Add(oRow)
            oRow = oTable.NewRow()
            oRow("COL1") = "Rocky(I)"
            oRow("COL2") = "Rocky(I)"
            oTable.Rows.Add(oRow)
            oRow = oTable.NewRow()
            oRow("COL1") = "Rocky(II)"
            oRow("COL2") = "Rocky(II)"
            oTable.Rows.Add(oRow)
            oRow = oTable.NewRow()
            oRow("COL1") = "Rocky(IX)"
            oRow("COL2") = "Rocky(IX)"
            oTable.Rows.Add(oRow)
            oRow = oTable.NewRow()
            oRow("COL1") = "Rocky(X)"
            oRow("COL2") = "Rocky(X)"
            oTable.Rows.Add(oRow)
            oRow = oTable.NewRow()
            oRow("COL1") = "Chapter(2)"
            oRow("COL2") = "Chapter(2)"
            oTable.Rows.Add(oRow)
            oRow = oTable.NewRow()
            oRow("COL1") = "Chapter 1 Ep 2-3"
            oRow("COL2") = "Chapter 1 Ep 2-3"
            oTable.Rows.Add(oRow)
            Dim myDV As DataView = oTable.DefaultView
            Dim mySorter As New Framework.Common.SortTable
            mySorter.Sort(oTable, "COL1", sortDirection)
            mRetVal.StartTime = mySorter.StartTime
            mRetVal.StopTime = mySorter.StopTime
            Dim ts As TimeSpan = mySorter.StopTime.Subtract(mySorter.StartTime)

            mRetVal.TotalMilliseconds = ts.TotalMilliseconds
            mRetVal.DataTable = oTable
            Dim mySort As String = "COL1 " + sortDirection
            myDV.Sort = mySort
            mRetVal.DataView = myDV
            oTable.Dispose()
            myDV.Dispose()
            Return mRetVal
        End Function

        <HttpPost>
        Public Function Upload() As IHttpActionResult
            Dim mRawRequestDirectory As String = HttpContext.Current.Request.Params("requestDirectory").ToString()
            If mRawRequestDirectory Is Nothing Then Throw New ArgumentNullException("requestDirectory", "'requestDirectory' cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRequestDirectory As MUIRequestDirectory = New JavaScriptSerializer().Deserialize(Of MUIRequestDirectory)(mRawRequestDirectory)
            Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(mRequestDirectory.Action)
            Dim mSecurityInfo As MSecurityInfo = GetSecurityInfo(mFunctionProfile.Id)
            Dim mDirectoryProfile As MDirectoryProfile = DirectoryUtility.GetProfile(mFunctionProfile.Id)
            Dim mDirectoryPath As String = mDirectoryProfile.Directory + mRequestDirectory.CurrentDirectoryString

            If mSecurityInfo.MayAdd Then
                Try
                    mDirectoryPath = mDirectoryPath.Replace("/", Path.DirectorySeparatorChar)

                    Dim iUploadedCnt As Integer = 0
                    Dim mFileCollection As HttpFileCollection = HttpContext.Current.Request.Files

                    For mFileCount As Integer = 0 To mFileCollection.Count - 1        ' CHECK THE FILE COUNT.
                        Dim mPostedFile As HttpPostedFile = mFileCollection(mFileCount)
                        If mPostedFile.ContentLength > 0 Then
                            If File.Exists(mDirectoryPath & Path.GetFileName(mPostedFile.FileName)) Then
                                File.Delete(mDirectoryPath & Path.GetFileName(mPostedFile.FileName))
                            End If
                            mPostedFile.SaveAs(mDirectoryPath & Path.GetFileName(mPostedFile.FileName))
                            iUploadedCnt += 1
                        End If
                    Next

                    If Val(iUploadedCnt) > 0 Then
                        Return Me.Ok(iUploadedCnt & " Files Uploaded Successfully")
                    Else
                        Return Me.BadRequest("Upload Failed")
                    End If
                Catch ex As Exception
                    m_Log.Fatal(ex)
                    Return Me.BadRequest("Upload Failed")
                End Try
            Else
                m_Log.Error(String.Format("Account '{0}' does not have permission to upload file to: {1}", AccountUtility.CurrentProfile().Account, mDirectoryPath))
                Return Me.BadRequest("Account does not have permission.")
            End If
        End Function
    End Class
End Namespace