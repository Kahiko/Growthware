Imports BLL.Base.SQLServer
Imports BLL.Base.ClientChoices
Imports Common.CustomWebControls
Imports DALModel.Base.Accounts.Security
Imports DALModel.Base.Modules

Public Class ModulesGeneral
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents litModuleSeqId As System.Web.UI.WebControls.Literal
	Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
	Protected WithEvents Requiredfieldvalidator5 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents txtDescription As System.Web.UI.WebControls.TextBox
	Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents txtState As System.Web.UI.WebControls.TextBox
	Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents txtSource As System.Web.UI.WebControls.TextBox
	Protected WithEvents RequiredFieldValidator3 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents chkIsNav As System.Web.UI.WebControls.CheckBox
	Protected WithEvents dropNavType As System.Web.UI.WebControls.DropDownList
	Protected WithEvents txtAction As System.Web.UI.WebControls.TextBox
	Protected WithEvents btnSave As System.Web.UI.WebControls.Button
	Protected WithEvents Tabstrip1 As TabStrip
	Protected WithEvents chkIsSecure As System.Web.UI.WebControls.CheckBox
	Protected WithEvents chkIsSysAdmin As System.Web.UI.WebControls.CheckBox
	Protected WithEvents dropNavParent As System.Web.UI.WebControls.DropDownList
	Protected WithEvents chkEnableViewState As System.Web.UI.WebControls.CheckBox
	Protected WithEvents litSource As System.Web.UI.WebControls.Literal
	Protected WithEvents litAction As System.Web.UI.WebControls.Literal
	Protected WithEvents litSourceNote As System.Web.UI.WebControls.Literal
	Protected WithEvents litActionNote As System.Web.UI.WebControls.Literal

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Public moduleProfileInfo As MModuleProfileInfo
	Private _UpdatedMODULE_SEQ_ID As Integer

	Public Property UpdatedMODULE_SEQ_ID() As Integer
		Get
			Return _UpdatedMODULE_SEQ_ID
		End Get
		Set(ByVal Value As Integer)
			_UpdatedMODULE_SEQ_ID = Value
		End Set
	End Property

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim myAccountSecurityInfo As MAccountSecurityInfo = myAccountUtility.GetAccountSecurityInfo(myAccountSecurityInfo)
		Dim dsNavType As DataSet
		Dim dvModules As New DataView
		NavMenuUtility.GetNavType(dsNavType)
		dropNavType.DataSource = dsNavType
		dropNavType.DataTextField = "DESCRIPTION"
		dropNavType.DataValueField = "NAV_TYPE_SEQ_ID"
		dropNavType.DataBind()
		dvModules = AppModulesUtility.GetModulesDataView()
		Dim NewRow As DataRow = dvModules.Table.NewRow
		dvModules.RowFilter = Nothing
		NewRow("Name") = "As a Root Item"
		NewRow("MODULE_SEQ_ID") = 0
		dvModules.Table.Rows.Add(NewRow)
		dvModules.Sort = "Name asc"
		dropNavParent.DataSource = dvModules
		dropNavParent.DataTextField = "Name"
		dropNavParent.DataValueField = "MODULE_SEQ_ID"
		dropNavParent.DataBind()
		dvModules.Table.Rows.Remove(NewRow)
		If Not Request.QueryString("id") Is Nothing Then
            Dim myModuleProfileInfo As MModuleProfileInfo
            Dim myAppModulesUtility As New AppModulesUtility
            myModuleProfileInfo = myAppModulesUtility.GetModulesByID(CInt(Request.QueryString("id")))
			litModuleSeqId.Text = myModuleProfileInfo.MODULE_SEQ_ID
			txtName.Text = Trim(myModuleProfileInfo.Name)
			txtDescription.Text = Trim(myModuleProfileInfo.Description)
			txtAction.Text = myModuleProfileInfo.Action.Trim
			litAction.Text = myModuleProfileInfo.Action.Trim
			txtSource.Text = myModuleProfileInfo.Source.Trim
			litSource.Text = myModuleProfileInfo.Source.Trim
			chkEnableViewState.Checked = myModuleProfileInfo.EnableViewState
			chkIsNav.Checked = myModuleProfileInfo.IS_NAV
			Dim X As Integer
			For X = 0 To dropNavType.Items.Count - 1
				If dropNavType.Items(X).Value = myModuleProfileInfo.NAV_TYPE_SEQ_ID Then
					dropNavType.Items(X).Selected = True
					Exit For
				End If
			Next
			For X = 0 To dropNavParent.Items.Count - 1
				If dropNavParent.Items(X).Value = myModuleProfileInfo.ParentID Then
					dropNavParent.Items(X).Selected = True
					Exit For
				End If
			Next
		End If
		If Not myAccountSecurityInfo.IsSystemAdministrator Then
			txtSource.Visible = False
			litSourceNote.Visible = False
			txtAction.Visible = False
			litActionNote.Visible = False
			litAction.Visible = True
			litSource.Visible = True
			chkEnableViewState.Enabled = False
		Else
			txtSource.Visible = True
			litSourceNote.Visible = True
			txtAction.Visible = True
			litActionNote.Visible = True
			litAction.Visible = False
			litSource.Visible = False
		End If
	End Sub

	Public Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		Dim myModuleProfileInfo As New MModuleProfileInfo
		Dim myDBRetrun As Integer
		Dim X As Integer
		If Not Request.QueryString("id") Is Nothing Then
			myModuleProfileInfo.MODULE_SEQ_ID = CInt(litModuleSeqId.Text)
		End If
		myModuleProfileInfo.Name = txtName.Text.Trim
		myModuleProfileInfo.Description = txtDescription.Text.Trim
		myModuleProfileInfo.Source = txtSource.Text.Trim
		myModuleProfileInfo.EnableViewState = chkEnableViewState.Checked
		myModuleProfileInfo.IS_NAV = chkIsNav.Checked
		For X = 0 To dropNavType.Items.Count - 1
			If dropNavType.Items(X).Selected = True Then
				myModuleProfileInfo.NAV_TYPE_SEQ_ID = dropNavType.Items(X).Value
				Exit For
			End If
		Next
		For X = 0 To dropNavParent.Items.Count - 1
			If dropNavParent.Items(X).Selected = True Then
				myModuleProfileInfo.ParentID = dropNavParent.Items(X).Value
				Exit For
			End If
		Next
		myModuleProfileInfo.Action = txtAction.Text.Trim
		Try
			Dim myAppModulesUtility As New AppModulesUtility
			If Not Request.QueryString("id") Is Nothing Then
				myDBRetrun = BAppModules.UpdateProfile(myModuleProfileInfo)
				UpdatedMODULE_SEQ_ID = CInt(myModuleProfileInfo.MODULE_SEQ_ID)
			Else
				myDBRetrun = BAppModules.AddModule(myModuleProfileInfo)
				If myDBRetrun = 0 Then
					myAppModulesUtility.ReBuildModuleCollection()
					myModuleProfileInfo = myAppModulesUtility.GetModuleInfoByAction(myModuleProfileInfo.Action)
					UpdatedMODULE_SEQ_ID = CInt(myModuleProfileInfo.MODULE_SEQ_ID)
				End If
			End If
		Catch ex As Exception
			Throw ex
		End Try
	End Sub	' btnSave_Click
End Class