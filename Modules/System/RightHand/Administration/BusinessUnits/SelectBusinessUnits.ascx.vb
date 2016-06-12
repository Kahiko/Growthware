Imports BLL.Base.ClientChoices
Imports Common.CustomWebControls
Imports DALModel.Special.ClientChoices

Public Class SelectBusinessUnits
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents AlphaPicker As AlphaPicker
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
	End Sub	'Page_Load
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
	End Sub	'Page_PreRender
	Private Sub bindData()
		Dim dvBusinessUnits As New DataView
		Dim sortBy As String = dropSortBy.SelectedItem.Value
		dvBusinessUnits = BusinessUnitUtility.GetBusinessUnitsDataView
		If sortBy = "0" Then		 ' determin what column to filter and sort
			sortBy = "Name"
		Else
			sortBy = "Description"
		End If
		If AlphaPicker.SelectedLetter = "All" Then		 ' filter out the sortBy column
			dvBusinessUnits.RowFilter = sortBy & " like '%'"
		Else
			dvBusinessUnits.RowFilter = sortBy & " like '" & AlphaPicker.SelectedLetter & "%'"
		End If
		' sort asc or desc
		If dropOrderBy.SelectedItem.Value = "0" Then
			dvBusinessUnits.Sort = sortBy & " ASC"
		Else
			dvBusinessUnits.Sort = sortBy & " DESC"
		End If
		dgResults.DataSource = dvBusinessUnits
		dgResults.DataKeyField = "BUSINESS_UNIT_SEQ_ID"
		dgResults.DataBind()
	End Sub	'bindData
	Private Sub dgResults_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgResults.ItemDataBound
		' skip if no dataitem
		If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
			Dim HyperEdit As HyperLink = CType(e.Item.FindControl("hyperEdit"), HyperLink)
			HyperEdit.ImageUrl = BaseHelper.ImagePath & "AdminEdit.gif"
			HyperEdit.NavigateUrl = BaseHelper.FQDNBasePage & String.Format("?Action=Edit Business Units&id={0}", e.Item.DataItem("BUSINESS_UNIT_SEQ_ID"))
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
End Class
