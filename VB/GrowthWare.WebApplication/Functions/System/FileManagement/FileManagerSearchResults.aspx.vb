Imports GrowthWare.WebSupport.Base
Imports GrowthWare.Framework.Model.Profiles
Imports System.Data
Imports GrowthWare.WebSupport.Utilities
Imports System.Drawing
Imports GrowthWare.WebSupport
Imports GrowthWare.Framework.Common

Public Class FileManagerSearchResults
    Inherits ClientChoicesPage

    Private m_ShowDeleteLink As Boolean = False

    Private m_DirectoryProfile As MDirectoryProfile = Nothing

    Private m_CurrentDirectory As String = "/"

    Public Property ShowDeleteLink() As Boolean
        Get
            Return m_ShowDeleteLink
        End Get
        Set(value As Boolean)
            m_ShowDeleteLink = value
        End Set
    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim mAction = GWWebHelper.GetQueryValue(Request, "action")
        If (Not String.IsNullOrEmpty(mAction)) Then
            Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(mAction)
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(mFunctionProfile, AccountUtility.CurrentProfile())
            If Not mSecurityInfo.MayDelete Then
                searchResults.Columns.RemoveAt(1)
            End If
        End If
        searchResults.Columns.RemoveAt(0)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        noResults.Visible = False
        searchResults.HeaderStyle.ForeColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeaderForeColor))
        searchResults.HeaderStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
        searchResults.AlternatingRowStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.AlternatingRowBackColor))
        searchResults.RowStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.RowBackColor))
        If Not String.IsNullOrEmpty(GWWebHelper.GetQueryValue(Request, "Columns")) Then
            Dim mSearchCriteria As New MSearchCriteria()
            mSearchCriteria.Columns = GWWebHelper.GetQueryValue(Request, "Columns")
            mSearchCriteria.OrderByColumn = Server.UrlDecode(GWWebHelper.GetQueryValue(Request, "OrderByColumn"))
            mSearchCriteria.OrderByDirection = GWWebHelper.GetQueryValue(Request, "OrderByDirection")
            Dim mTryParse As Integer = 0
            If Integer.TryParse(GWWebHelper.GetQueryValue(Request, "PageSize"), mTryParse) Then
                mSearchCriteria.PageSize = Integer.Parse(GWWebHelper.GetQueryValue(Request, "PageSize"))
            Else
                mSearchCriteria.PageSize = 10
            End If
            If Integer.TryParse(GWWebHelper.GetQueryValue(Request, "SelectedPage"), mTryParse) Then
                mSearchCriteria.SelectedPage = Integer.Parse(GWWebHelper.GetQueryValue(Request, "SelectedPage"))
            Else
                mSearchCriteria.SelectedPage = 1
            End If
            mSearchCriteria.WhereClause = Server.UrlDecode(GWWebHelper.GetQueryValue(Request, "WhereClause"))
            mSearchCriteria.WhereClause = mSearchCriteria.WhereClause.Replace("""", String.Empty)

            Dim mDesiredPath As String = Server.UrlDecode(GWWebHelper.GetQueryValue(Request, "desiredPath"))
            If Not mDesiredPath.StartsWith("/") Then mDesiredPath = "/" + mDesiredPath
            If Not mDesiredPath.Length = 0 Then
                m_CurrentDirectory = mDesiredPath
            End If
            bindData(mSearchCriteria)
        End If

    End Sub

    Private Sub bindData(ByVal searchCriteria As MSearchCriteria)
        Dim mServer As HttpServerUtility = Server
        Dim mAction As String = GWWebHelper.GetQueryValue(HttpContext.Current.Request, "Action")
        Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(mAction)
        m_DirectoryProfile = DirectoryUtility.GetProfile(mFunctionProfile.Id)
        Dim mDirectoryPath As String = m_DirectoryProfile.Directory + m_CurrentDirectory
        Dim mDataTable As DataTable = FileUtility.GetDirectoryTableData(mDirectoryPath, m_DirectoryProfile, False)
        Dim mSorter As SortTable = New SortTable()
        Dim mColName As String = searchCriteria.OrderByColumn
        mSorter.Sort(mDataTable, mColName, searchCriteria.OrderByDirection)

        Dim mView As DataView = mDataTable.DefaultView
        mView.Sort = "type desc"
        mDataTable = DataHelper.GetTable(mView)
        Dim mSort As String = "type desc, " + searchCriteria.OrderByColumn + " " + searchCriteria.OrderByDirection
        mDataTable = DataHelper.GetPageOfData(mDataTable, mSort, searchCriteria.WhereClause, searchCriteria)


        If Not mDataTable Is Nothing And mDataTable.Rows.Count > 0 Then
            Dim mDataView As DataView = mDataTable.DefaultView()
            recordsReturned.Value = mDataTable.Rows(0)(DataHelper.TotalRowColumnName).ToString()
            searchResults.DataSource = mDataTable
            searchResults.DataBind()
        Else
            noResults.Visible = True
        End If
    End Sub

    Private Sub searchResults_DataBound(sender As Object, e As GridViewRowEventArgs) Handles searchResults.RowDataBound
        Dim rowType As DataControlRowType = e.Row.RowType
        If rowType = DataControlRowType.DataRow Then
            Dim mDeleteData As String = "{"
            mDeleteData += String.Format(
                        """FileName"" : ""{0}"",""FileType"" : ""{1}"",""CurrentDirectory"" : ""{2}"",""FunctionSeqId"" : ""{3}""",
                        Server.UrlEncode(DataBinder.Eval(e.Row.DataItem, "Name").ToString()),
                        DataBinder.Eval(e.Row.DataItem, "Type").ToString(),
                        Server.UrlEncode(m_CurrentDirectory),
                        m_DirectoryProfile.FunctionSeqId.ToString()).ToString()
            mDeleteData += "}"

            Dim chkDelete As HtmlInputCheckBox = CType(e.Row.FindControl("DeleteCheckBox"), HtmlInputCheckBox)
            If Not chkDelete Is Nothing Then
                ' Add confirmation to delete button
                chkDelete.Attributes.Add("Data", mDeleteData)
            End If
            Dim imgType As System.Web.UI.WebControls.Image = CType(e.Row.FindControl("imgType"), System.Web.UI.WebControls.Image)
            Dim type As String = DataBinder.Eval(e.Row.DataItem, "type", "")
            If Not imgType Is Nothing Then
                Dim mFileName As String = DataBinder.Eval(e.Row.DataItem, "Name").ToString()
                Select Case type.ToLower()
                    Case "folder"
                        imgType.ImageUrl = "Public/Images/GrowthWare/Folder.gif"
                        Dim changeDirectoryLink As HtmlAnchor = CType(e.Row.FindControl("lnkName"), HtmlAnchor)
                        If Not changeDirectoryLink Is Nothing Then
                            Dim mCurrentDirectory As String = m_CurrentDirectory
                            Dim mPath As String = mCurrentDirectory + "/" + mFileName
                            changeDirectoryLink.InnerText = mFileName + "\"
                            changeDirectoryLink.HRef = String.Format("javascript:GW.FileManager.changeDirectory('{0}','{1}')", mPath, m_DirectoryProfile.FunctionSeqId.ToString())
                        End If
                    Case "file"
                        imgType.ImageUrl = "Public/Images/GrowthWare/File.gif"
                        Dim downloadLink As HtmlAnchor = CType(e.Row.FindControl("lnkName"), HtmlAnchor)
                        If Not downloadLink Is Nothing Then
                            Dim mCurrentDirectory As String = m_CurrentDirectory
                            downloadLink.InnerText = mFileName
                            downloadLink.HRef = String.Format("javascript:GW.FileManager.downLoad('{0}','{1}','{2}')", mCurrentDirectory, mFileName, m_DirectoryProfile.FunctionSeqId.ToString())
                        End If
                    Case Else
                        imgType.Visible = False
                End Select
            End If

            ' add the hover behavior
            If e.Row.RowState = DataControlRowState.Normal Then
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='Beige'")
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='" & ClientChoicesState(MClientChoices.RowBackColor) & "'")
            Else ' the alternate row.
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='Beige'")
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='" & ClientChoicesState(MClientChoices.AlternatingRowBackColor) & "'")
            End If
        End If
    End Sub

End Class