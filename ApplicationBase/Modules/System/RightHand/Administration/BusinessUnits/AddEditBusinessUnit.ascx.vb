Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.Cache
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Common.Logging
Imports ApplicationBase.Common.Security
Imports ApplicationBase.Model.Directories
Imports ApplicationBase.Model.BusinessUnits
Imports System.Data

Partial Class AddEditBusinessUnit
	Inherits System.Web.UI.UserControl

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
						CacheControler.RemoveFromCache(DirectoryInfoUtility.DirectoryInfoCachedCollection)
					End If
				End If
                myClientMsg = "The " & BaseSettings.BusinessUnitTranslation & " has been updated"
			Catch ex As Exception
                myClientMsg = "The " & BaseSettings.BusinessUnitTranslation & " has not been updated"
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
		Dim myCryptoUtil As New CryptoUtil
		businessUnitProfileInfo.ConnctionString = myCryptoUtil.EncryptTripleDES(txtConnectionString.Text.Trim)
		businessUnitProfileInfo.DAL = txtDAL.Text.Trim
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
		Dim myCryptoUtil As New CryptoUtil
		Try
			txtConnectionString.Text = myCryptoUtil.DecryptTripleDES(businessUnitProfileInfo.ConnctionString)
		Catch ex As Exception
			txtConnectionString.Text = businessUnitProfileInfo.ConnctionString
		End Try
		txtDAL.Text = businessUnitProfileInfo.DAL
        litBusinessUnitTranslation.Text = BaseSettings.businessUnitTranslation
		Dim myDirectoryInfo As New MDirectoryProfileInformation
		Dim dvSkin As New DataView
        dvSkin = FileManager.GetDirectoryTableData(BaseSettings.uIPath, Server, myDirectoryInfo, False).DefaultView
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
        BaseHelperOld.SetDropSelection(dropParent, businessUnitProfileInfo.Parent_Business_Unit_Seq_ID)
        BaseHelperOld.SetDropSelection(dropSkin, businessUnitProfileInfo.Skin)
        BaseHelperOld.SetDropSelection(STATUS_SEQ_ID, businessUnitProfileInfo.STATUS_SEQ_ID)
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
