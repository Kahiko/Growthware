Imports System.Security.Principal
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Directories
Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Drawing
Imports System.Data

Partial Class FileManagerControl
	Inherits ClientChoices.ClientChoicesUserControl

	Private moduleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetCurrentModule
	Private myAccountUtility As New AccountUtility(HttpContext.Current)
	Private clientSecurityInfo As MAccountSecurityInfo
    Public btnDeleteImage As String = BaseSettings.ImagePath & "btnDelete.gif"
    Public btnNewDirectoryImage As String = BaseSettings.ImagePath & "btnNewFolder.gif"
    Public imgUpImage As String = BaseSettings.ImagePath & "FolderUp.gif"
    Public filesOnly As Boolean
    Private directoryInfo As MDirectoryProfileInformation = DirectoryInfoUtility.getDirectoryInfo(ClientChoicesState(MClientChoices.BusinessUnitID))


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        myAccountUtility.GetAccountSecurityInfo(clientSecurityInfo)
        If Len(literalPath.Text) <> 0 Then
            ViewState("curDir") = literalPath.Text
        End If
        lblClientMSG.Visible = False
        If Not Page.IsPostBack Then
            DGFileSystem.PageSize = ClientChoicesState(MClientChoices.RecordsPerPage)
            If ViewState("curDir") = "" Then
                ViewState("curDir") = directoryInfo.Directory
            End If
            firstRow.BgColor = ClientChoicesState(MClientChoices.HeadColor)
            DGFileSystem.HeaderStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.SubheadColor))
            DGFileSystem.HeaderStyle.ForeColor = Color.WhiteSmoke
            DGFileSystem.PagerStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.SubheadColor))
            btnGoUp.ImageUrl = imgUpImage
            btnDelete.ImageUrl = btnDeleteImage
            btnNewDirectory.ImageUrl = btnNewDirectoryImage
            btnDelete.Attributes.Add("OnClick", "return btnDelete_Click()")
            If Not clientSecurityInfo.MayAdd Then
                litUploadFile.Visible = False
                txtFileToUpload.Visible = False
                btnUpLoad.Visible = False
                CreateNewDirectory.Visible = False
                txtNewDirectory.Visible = False
                btnNewDirectory.Visible = False
            End If
            If Not clientSecurityInfo.MayDelete Then
                btnDelete.Visible = False
                cmdSelect.Visible = False
            End If
            If Not directoryInfo.Directory = "" Then
                BindData()
            Else
                btnNewDirectory.Visible = False
                litUploadFile.Visible = False
                txtFileToUpload.Visible = False
                btnUpLoad.Visible = False
                btnDelete.Visible = False
                txtNewDirectory.Visible = False
                CreateNewDirectory.Visible = False
                btnGoUp.Visible = False
                litErrorMSG.Visible = True
                litErrorMSG.Text = "The Directory for " & BaseSettings.businessUnitTranslation & " " & ClientChoicesState(MClientChoices.BusinessUnitName) & " has not been setup!"
            End If
        End If

    End Sub
    Private Sub BindData()
        Dim currentDir As String
        currentDir = ViewState("curDir")
        Dim myDirectoryTableData As New DataTable
        DGFileSystem.DataSource = FileManager.GetDirectoryTableData(currentDir, Server, directoryInfo, filesOnly)
        DGFileSystem.DataKeyField = "type"
        DGFileSystem.DataBind()
        If currentDir = directoryInfo.Directory Then
            btnGoUp.Visible = False
            currentDirectory.Text = "Root "
        Else
            btnGoUp.Visible = True
            currentDirectory.Text = currentDir.Replace(directoryInfo.Directory, "")
        End If
    End Sub
    Sub DGFileSystem_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGFileSystem.EditCommand
        Dim OldFileName = e.Item.Cells(2).Controls(5)
        ViewState("PrevFileFolderName") = OldFileName.text()
        DGFileSystem.EditItemIndex = CInt(e.Item.ItemIndex)
        BindData()
    End Sub

    Sub DGFileSystem_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGFileSystem.CancelCommand
        DGFileSystem.EditItemIndex = -1
        BindData()
    End Sub

    Private Sub DGFileSystem_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGFileSystem.ItemCommand
        If e.CommandName.ToLower = "itemclicked" Then
            DGFileSystem.EditItemIndex = -1
            Dim lnkName As LinkButton = e.Item.FindControl("lnkName")
            Dim type As String
            ' since this does not interact with the clsFileManager directly
            ' but does interact with the directory structure, it is necessary
            ' to perform any impersonation now if it is needed
            Dim impersonatedUser As WindowsImpersonationContext
            type = DGFileSystem.DataKeys(e.Item.ItemIndex)
            If type.ToLower = "folder" Then
                DGFileSystem.CurrentPageIndex = 0
                ViewState("curDir") = ViewState("curDir") & "\" & lnkName.Text.ToString
                BindData()
            Else
                If directoryInfo.Impersonate Then
                    impersonatedUser = FileManager.ImpersonateNow(impersonatedUser, directoryInfo)
                End If
                Try
                    Dim myHref As String
                    myHref = ViewState("curDir") & "\" & lnkName.Text.ToString()
                    Dim filename As String = myHref
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + lnkName.Text.Trim())
                    Response.WriteFile(filename)
                    Response.End()
                Catch ex As Exception
                    Throw ex
                Finally
                    ' Stop impersonating the user.
                    ' Under normal conditions this would be necessary
                    ' because the response.end is used this will
                    ' cause the file down load to do pretty much
                    ' nothing at all.  I've tested this and proved it's true.
                    ' by removing this code the file download happens
                    ' as it should.  I've tested clsFileManagement
                    ' GetDirectoryTableData method to ensure that
                    ' upon next use of code that indeed the
                    ' aspnet account is still being used before
                    ' any impersonation.
                    'If Not impersonatedUser Is Nothing Then
                    '	impersonatedUser.Undo()
                    'End If
                End Try
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDelete.Click
        Dim Item As DataGridItem
        Try
            For Each Item In DGFileSystem.Items
                Dim chkDeleteCheckBox As New System.Web.UI.HtmlControls.HtmlInputCheckBox
                chkDeleteCheckBox = Item.FindControl("DeleteCheckBox")
                If chkDeleteCheckBox.Checked Then
                    Dim type As String
                    Dim lnkName As LinkButton = Item.FindControl("lnkName")
                    type = DGFileSystem.DataKeys(Item.ItemIndex).ToString.ToLower
                    Select Case type
                        Case "folder"
                            Dim folderToDelete As String = ViewState("curDir").ToString & "\" & lnkName.Text
                            lblClientMSG.Text = FileManager.DeleteDirectory(folderToDelete, directoryInfo)
                        Case "file"
                            Dim fileToDelete As String
                            fileToDelete = ViewState("curDir").ToString & "\" & lnkName.Text.ToString()
                            lblClientMSG.Text = FileManager.DeleteFile(fileToDelete, directoryInfo)
                        Case Else
                            lblClientMSG.Text = "Unknown type can not delete!"
                    End Select
                End If
            Next
        Catch ex As Exception
            lblClientMSG.Text = ex.ToString
        End Try
        lblClientMSG.Visible = True
        BindData()
    End Sub

    Private Sub btnGoUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGoUp.Click
        ViewState("curDir") = FileManager.GetParent(ViewState("curDir"))
        DGFileSystem.CurrentPageIndex = 0
        BindData()
    End Sub

    Private Sub DGFileSystem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGFileSystem.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.EditItem Then
            Dim imgType As System.Web.UI.WebControls.Image = e.Item.FindControl("imgType")
            Dim type As String
            type = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "type", "")
            If Not imgType Is Nothing Then
                Select Case type.ToLower
                    Case "folder"
                        imgType.ImageUrl = BaseSettings.ImagePath & "Folder.gif"
                    Case "file"
                        imgType.ImageUrl = BaseSettings.ImagePath & "File.gif"
                    Case Else
                        imgType.Visible = False
                End Select
            End If
        End If
        'If Not clientSecurityInfo.MayDelete Then
        '	e.Item.Cells(0).Controls.Clear()
        '	e.Item.Cells(0).Visible = False
        'End If
        'If Not clientSecurityInfo.MayEdit Then
        '	e.Item.Cells(1).Controls.Clear()
        '	e.Item.Cells(1).Visible = False
        'End If
    End Sub

	Private Sub btnNewDirectory_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnNewDirectory.Click
		lblClientMSG.Visible = True
		lblClientMSG.Text = FileManager.CreateDirectory(ViewState("curDir"), txtNewDirectory.Text, directoryInfo)
		txtNewDirectory.Text = ""
		BindData()
	End Sub

	Sub btnUpLoad_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpLoad.ServerClick
		lblClientMSG.Visible = True
		lblClientMSG.Text = FileManager.DoUpload(txtFileToUpload, ViewState("curDir").ToString, directoryInfo)
		BindData()
	End Sub

	Private Sub DGFileSystem_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGFileSystem.PageIndexChanged
		DGFileSystem.CurrentPageIndex = e.NewPageIndex
		If IsPostBack Then
			BindData()
		End If
	End Sub

	Sub DGFileSystem_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGFileSystem.UpdateCommand
		Try
			Dim controlNewFileName = e.Item.Cells(2).Controls(1)
			Dim NewFileName As String = ViewState("curDir").ToString & "\" & controlNewFileName.text()
			Dim OldFileName As String = ViewState("curDir").ToString & "\" & ViewState("PrevFileFolderName").ToString
			Dim type As String
			type = DGFileSystem.DataKeys(e.Item.ItemIndex).ToString.ToLower
			Select Case type
				Case "folder"
					lblClientMSG.Text = FileManager.RenameDirectory(OldFileName, NewFileName, directoryInfo)
				Case "file"
					lblClientMSG.Text = FileManager.RenameFile(OldFileName.ToString, NewFileName.ToString, directoryInfo)
				Case Else
					lblClientMSG.Text = "Unknown type can not rename!"
			End Select
		Catch ex As Exception
			lblClientMSG.Text = ex.ToString
		End Try
		lblClientMSG.Visible = True
		DGFileSystem.EditItemIndex = -1
		BindData()
	End Sub

	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
		If Not clientSecurityInfo.MayDelete Then
			DGFileSystem.Columns(0).Visible = False
		End If
		If Not clientSecurityInfo.MayEdit Then
			DGFileSystem.Columns(1).Visible = False
		End If
	End Sub
End Class