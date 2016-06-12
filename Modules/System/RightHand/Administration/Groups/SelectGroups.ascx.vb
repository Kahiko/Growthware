Imports BLL.Base.ClientChoices
Imports Common.CustomWebControls
Imports DALModel.Special.ClientChoices
Imports BLL.Base.SQLServer

Public Class SelectGroups
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents AlphaPicker As Common.CustomWebControls.AlphaPicker
    Protected WithEvents txtRecordsPerPage As System.Web.UI.WebControls.TextBox
    Protected WithEvents dropSortBy As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dropOrderBy As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dgResults As System.Web.UI.WebControls.DataGrid

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            txtRecordsPerPage.Text = ClientChoicesState(MClientChoices.RecordsPerPage)
            AlphaPicker.SelectedLetter = "All"
            dgResults.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
        End If
    End Sub 'Page_Load
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim recsPerPage As Integer
        Try
            recsPerPage = CInt(txtRecordsPerPage.Text)
            If recsPerPage <= 0 Then
                recsPerPage = CInt(ClientChoicesState(MClientChoices.RecordsPerPage))
                txtRecordsPerPage.Text = recsPerPage
            End If
        Catch ex As Exception
            recsPerPage = 10
            txtRecordsPerPage.Text = CInt(ClientChoicesState(MClientChoices.RecordsPerPage))
        Finally
            dgResults.PageSize = recsPerPage
        End Try
        bindData()
    End Sub 'Page_PreRender
    Private Sub bindData()
        Dim dvGoups As New DataView
        Dim sortBy As String = dropSortBy.SelectedItem.Value
        dvGoups = BGroups.SearchGroups("", ClientChoicesState(MClientChoices.BusinessUnitID)).Tables(0).DefaultView
        If sortBy = "0" Then   ' determin what column to filter and sort
            sortBy = "Name"
        Else
            sortBy = "Description"
        End If
        If AlphaPicker.SelectedLetter = "All" Then   ' filter out the sortBy column
            dvGoups.RowFilter = sortBy & " like '%'"
        Else
            dvGoups.RowFilter = sortBy & " like '" & AlphaPicker.SelectedLetter & "%'"
        End If
        ' sort asc or desc
        If dropOrderBy.SelectedItem.Value = "0" Then
            dvGoups.Sort = sortBy & " ASC"
        Else
            dvGoups.Sort = sortBy & " DESC"
        End If
        dgResults.DataSource = dvGoups
        dgResults.DataKeyField = "GROUP_SEQ_ID"
        dgResults.DataBind()
    End Sub 'bindData
    Private Sub dgResults_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgResults.ItemDataBound
        ' skip if no dataitem
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim HyperEdit As HyperLink = CType(e.Item.FindControl("hyperEdit"), HyperLink)
            HyperEdit.ImageUrl = BaseHelper.ImagePath & "AdminEdit.gif"
            HyperEdit.NavigateUrl = BaseHelper.FQDNBasePage & String.Format("?Action=editgroupinfo&groupId={0}", e.Item.DataItem("GROUP_SEQ_ID"))

            ' Add confirmation to delete button
            Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
            btnDelete.ImageUrl = BaseHelper.ImagePath & "delete.gif"
            btnDelete.Attributes.Add("onclick", String.Format("return confirm('Are you sure you want to delete the ""{0}"" Group?');", CStr(DataBinder.Eval(e.Item.DataItem, "Name"))))
        End If
    End Sub 'dgStates_ItemDataBound

    Private Sub dgResults_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgResults.PageIndexChanged
        dgResults.CurrentPageIndex = e.NewPageIndex
    End Sub 'dgStates_PageIndexChanged

    Sub OrderChanged(ByVal sender As [Object], ByVal e As EventArgs) Handles AlphaPicker.LetterChanged, dropOrderBy.SelectedIndexChanged, dropSortBy.SelectedIndexChanged
        dgResults.CurrentPageIndex = 0
    End Sub 'OrderChanged

    Public Sub txtRecordsPerPage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecordsPerPage.TextChanged
        dgResults.CurrentPageIndex = 0
    End Sub 'txtRecordsPerPage_TextChanged

    Private Sub dgResults_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgResults.DeleteCommand
        Dim GroupSeqId As String = CStr(dgResults.DataKeys(e.Item.ItemIndex))
        ' Delete group
        BGroups.DeleteGroup(GroupSeqId, ClientChoicesState(MClientChoices.BusinessUnitID))
        ' UnSelect row for editing
        dgResults.EditItemIndex = -1
        bindData()
    End Sub

    Private Sub dgResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgResults.SelectedIndexChanged

    End Sub
End Class
