Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.Cache
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Accounts

Public Class CopyModuleSecurity
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents litClientMSG As System.Web.UI.WebControls.Literal
	Protected WithEvents SourceBU As System.Web.UI.WebControls.DropDownList
	Protected WithEvents TargetBU As System.Web.UI.WebControls.DropDownList
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

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		If Not Page.IsPostBack Then
			bindData()
		End If
	End Sub

	Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
		If SourceBU.SelectedValue = TargetBU.SelectedValue Then
			litClientMSG.Visible = True
			litClientMSG.Text = "Source and Target can not be the same"
		Else
			litClientMSG.Visible = True
			Dim myAccountProfile As New MAccountProfileInfo
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			myAccountProfile = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name)
			BAppModules.CopyModuleSecurity(myAccountProfile.ACCOUNT_SEQ_ID, SourceBU.SelectedValue, TargetBU.SelectedValue)
			litClientMSG.Text = "Module Security has been copied"
			AppModulesUtility.ReBuildModuleCollection()
			BusinessUnitUtility.RemoveRoleCache(TargetBU.SelectedValue)
		End If
	End Sub

	Private Sub bindData()
		Dim dvBusinessUnits As New DataView
		dvBusinessUnits = BusinessUnitUtility.GetBusinessUnitsDataView
        dvBusinessUnits.RowFilter = "BUSINESS_UNIT_SEQ_ID <> '" & BaseSettings.DefaultBusinessUnitID & "'"
		dvBusinessUnits.Sort = "Name ASC"
		SourceBU.DataSource = dvBusinessUnits
		SourceBU.DataTextField = "Name"
		SourceBU.DataValueField = "BUSINESS_UNIT_SEQ_ID"
		SourceBU.DataBind()

		TargetBU.DataSource = dvBusinessUnits
		TargetBU.DataTextField = "Name"
		TargetBU.DataValueField = "BUSINESS_UNIT_SEQ_ID"
		TargetBU.DataBind()

	End Sub
End Class