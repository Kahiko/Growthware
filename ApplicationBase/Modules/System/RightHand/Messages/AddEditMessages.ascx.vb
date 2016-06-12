Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.BusinessUnits
Imports ApplicationBase.Model.Messages
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Data

Partial Class AddEditMessages
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Not Page.IsPostBack Then
			Dim myBusinessUnitProfile As New MBusinessUnitProfileInfo
			If ClientChoicesState(MClientChoices.BusinessUnitID) <> myBusinessUnitProfile.BUSINESS_UNIT_SEQ_ID Then
                BaseHelperOld.SelectBusinessUnit()
			End If
			Dim myDataSet As New DataSet
			BMessages.GetMessageNames(myDataSet, ClientChoicesState(MClientChoices.BusinessUnitID))
			dropMessageNames.DataSource = myDataSet.Tables(0)
			dropMessageNames.DataTextField = "Name"
			dropMessageNames.DataBind()
			BindMessage()
		End If
	End Sub	'Page_Load

	'*******************************************************
	'
	' Display message body when user selects a new
	' message to view.
	'
	'*******************************************************
	Sub MessageChanged(ByVal s As [Object], ByVal e As EventArgs) Handles dropMessageNames.SelectedIndexChanged
		BindMessage()
	End Sub	'MessageChanged

	'*******************************************************
	'
	' Display body of the current message in the TextBox.
	'
	'*******************************************************
	Sub BindMessage()
		Dim myMessageInfo As MMessageInfo
		myMessageInfo = BMessages.GetMessage(dropMessageNames.SelectedItem.Text)
		lblMessageDescription.Text = myMessageInfo.Description
		txtMessageTitle.Text = myMessageInfo.Title
		txtMessageBody.Text = myMessageInfo.Body
	End Sub	'BindMessage

	'*******************************************************
	'
	' Save changes to the message.
	'
	'*******************************************************
	Sub btnEdit_Click(ByVal s As [Object], ByVal e As EventArgs) Handles btnEdit.Click
		Dim success As Boolean = False
		Dim myMessageInfo As MMessageInfo
		myMessageInfo = BMessages.GetMessage(dropMessageNames.SelectedItem.Text)
		myMessageInfo.Body = txtMessageBody.Text
		success = BMessages.UpdateMessage(myMessageInfo)
		ClientMSG.Visible = True
		If success Then
			ClientMSG.InnerHtml = "Message has been updated"
		Else
			ClientMSG.InnerHtml = "Could not update the message"
		End If
	End Sub	'btnEdit_Click
End Class