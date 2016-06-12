Imports BLL.Base.SQLServer
Imports Common.Cache
Imports DALModel.Base.Directories
Imports DALModel.Base.BusinessUnits

Public Class AddEditBusinessUnit
	Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents litBusinessUnitTranslation As System.Web.UI.WebControls.Literal
	Protected WithEvents litBusinessUnit As System.Web.UI.WebControls.Literal
	Protected WithEvents txtDescription As System.Web.UI.WebControls.TextBox
	Protected WithEvents Requiredfieldvalidator1 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents STATUS_SEQ_ID As System.Web.UI.WebControls.DropDownList
	Protected WithEvents txtConnectionString As System.Web.UI.WebControls.TextBox
	Protected WithEvents dropSkin As System.Web.UI.WebControls.DropDownList
	Protected WithEvents btnSave As System.Web.UI.WebControls.Button
	Protected WithEvents txtBusinessUnit As System.Web.UI.WebControls.TextBox
	Protected WithEvents dropParent As System.Web.UI.WebControls.DropDownList
    Protected WithEvents litClientMsg As System.Web.UI.WebControls.Literal
    Protected WithEvents Requiredfieldvalidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtDirectory As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkImpersonation As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtAccount As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPassword As System.Web.UI.WebControls.TextBox
	Protected WithEvents txtHidPwd As System.Web.UI.WebControls.TextBox

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

	End Sub

	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		Dim businessUnitProfileInfo As New MBusinessUnitProfileInfo
		Dim directoryProfileInfo As New MDirectoryProfileInformation
		Dim myClientMsg As String = String.Empty
		PopulateFromPage(businessUnitProfileInfo)
		PopulateFromPage(directoryProfileInfo)
		Dim success As Boolean = False
		If Request.QueryString("Action").ToLower.StartsWith("edit") Then
			Try
                businessUnitProfileInfo.BUSINESS_UNIT_SEQ_ID = CInt(Request.QueryString("id"))
                success = BusinessUnitUtility.UpdateBusinessUnitProfileInfo(businessUnitProfileInfo)
				If success Then
					If Not txtDirectory.Text.Trim.Length = 0 Then
						directoryProfileInfo.BUSINESS_UNIT_SEQ_ID = businessUnitProfileInfo.BUSINESS_UNIT_SEQ_ID
						success = BDirectoryInfo.addUpdateDirectoryInfo(directoryProfileInfo)
						Dim myCacheControler As CacheControler
						myCacheControler.RemoveFromCache(DirectoryInfoUtility.DirectoryInfoCachedCollection)
					End If
				End If
				myClientMsg = "The " & BaseHelper.BusinessUnitTranslation & " has been updated"
			Catch ex As Exception
				myClientMsg = "The " & BaseHelper.BusinessUnitTranslation & " has not been updated"
				Dim log As AppLogger = AppLogger.GetInstance
				log.Fatal(ex)
			End Try
		Else
			Try
				success = BusinessUnitUtility.AddBusinessUnitProfileInfo(businessUnitProfileInfo)
				Dim myBusinessUnitCollection As New MBusinessUnitProfileInfoCollection
				BusinessUnitUtility.GetBusinessProfileCollection(myBusinessUnitCollection)
				businessUnitProfileInfo = myBusinessUnitCollection.GetBusinessUnitByName(businessUnitProfileInfo.Name)
				NavControler.NavTo("Edit Business Units&id=" & businessUnitProfileInfo.BUSINESS_UNIT_SEQ_ID)
			Catch ex As Exception
				myClientMsg = ex.Message.ToString
			End Try
		End If
		litClientMsg.Text = myClientMsg
		litClientMsg.Visible = True
	End Sub

	Private Sub PopulateFromPage(ByRef businessUnitProfileInfo As MBusinessUnitProfileInfo)
		businessUnitProfileInfo.ConnctionString = txtConnectionString.Text
		businessUnitProfileInfo.Description = txtDescription.Text
		businessUnitProfileInfo.Name = txtBusinessUnit.Text
		businessUnitProfileInfo.Parent_Business_Unit_Seq_ID = dropParent.SelectedValue
		businessUnitProfileInfo.Skin = dropSkin.SelectedItem.Value
		businessUnitProfileInfo.STATUS_SEQ_ID = STATUS_SEQ_ID.SelectedValue
	End Sub
	Private Sub PopulateFromPage(ByRef directoryProfileInformation As MDirectoryProfileInformation)
		directoryProfileInformation.Directory = txtDirectory.Text
		directoryProfileInformation.Impersonate = chkImpersonation.Checked
		directoryProfileInformation.Impersonate_Account = txtAccount.Text
		Dim thePassword As String = String.Empty
		If txtPassword.Text.Trim.Length = 0 Then
			thePassword = txtHidPwd.Text.Trim
		Else
			thePassword = txtPassword.Text.Trim
		End If
		directoryProfileInformation.Impersonate_PWD = thePassword
	End Sub

	Private Sub PopulatePage(ByRef businessUnitProfileInfo As MBusinessUnitProfileInfo, ByRef directoryProfileInformation As MDirectoryProfileInformation)
		litBusinessUnit.Text = businessUnitProfileInfo.Name
		txtBusinessUnit.Text = businessUnitProfileInfo.Name
		txtDescription.Text = businessUnitProfileInfo.Description
		txtConnectionString.Text = businessUnitProfileInfo.ConnctionString
		litBusinessUnitTranslation.Text = BaseHelper.BusinessUnitTranslation
		Dim myFileManagement As New FileManager
		Dim myDirectoryInfo As New MDirectoryProfileInformation
		Dim dvSkin As New DataView
		dvSkin = myFileManagement.GetDirectoryTableData(BaseHelper.UIPath, Server, myDirectoryInfo, False).DefaultView
		dvSkin.RowFilter = "Type = 'folder'"
		dropSkin.DataSource = dvSkin
		dropSkin.DataTextField = "Name"
		dropSkin.DataValueField = "Name"
		dropSkin.DataBind()
		Dim myBusinessUnitCollection As New MBusinessUnitProfileInfoCollection
		Dim myBusinessUnitProfileInfo As New MBusinessUnitProfileInfo
		BusinessUnitUtility.GetBusinessProfileCollection(myBusinessUnitCollection)
		Dim BusinessUnitName As String = String.Empty
		For Each BusinessUnitName In myBusinessUnitCollection.Keys
			myBusinessUnitProfileInfo = myBusinessUnitCollection.GetBusinessUnitByID(BusinessUnitName)
			Dim lItem As New ListItem
			With lItem
				.Text = myBusinessUnitProfileInfo.Name
				.Value = myBusinessUnitProfileInfo.BUSINESS_UNIT_SEQ_ID
			End With
			dropParent.Items.Add(lItem)
		Next
		BaseHelper.SetDropSelection(dropParent, businessUnitProfileInfo.Parent_Business_Unit_Seq_ID)
		BaseHelper.SetDropSelection(dropSkin, businessUnitProfileInfo.Skin)
		BaseHelper.SetDropSelection(STATUS_SEQ_ID, businessUnitProfileInfo.STATUS_SEQ_ID)
		txtDirectory.Text = directoryProfileInformation.Directory
		chkImpersonation.Checked = directoryProfileInformation.Impersonate
		txtAccount.Text = directoryProfileInformation.Impersonate_Account
		txtPassword.Text = directoryProfileInformation.Impersonate_PWD
		txtHidPwd.Text = directoryProfileInformation.Impersonate_PWD
	End Sub

	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
		Dim businessUnitProfileInfo As New MBusinessUnitProfileInfo
		Dim directoryProfileInfo As New MDirectoryProfileInformation
		If Request.QueryString("Action").ToLower.StartsWith("edit") Then
			businessUnitProfileInfo = BusinessUnitUtility.GetProfileByID(Request.QueryString("id"))
			directoryProfileInfo = DirectoryInfoUtility.getDirectoryInfo(businessUnitProfileInfo.BUSINESS_UNIT_SEQ_ID)
			litBusinessUnit.Visible = True
		Else
			litBusinessUnit.Visible = False
			txtBusinessUnit.Visible = True
		End If
		PopulatePage(businessUnitProfileInfo, directoryProfileInfo)
	End Sub
End Class
