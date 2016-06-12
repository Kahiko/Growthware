Imports BLL.Base.SQLServer
Imports BLL.Base.ClientChoices
Imports DALModel.Base.Accounts
Imports DALModel.Base.BusinessUnits
Imports DALModel.Base.Messages
Imports DALModel.Special.ClientChoices

Public Class AddEditMessages
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents dropMessageNames As System.Web.UI.WebControls.DropDownList
	Protected WithEvents lblMessageDescription As System.Web.UI.WebControls.Label
	Protected WithEvents txtMessageTitle As System.Web.UI.WebControls.TextBox
	Protected WithEvents txtMessageBody As System.Web.UI.WebControls.TextBox
	Protected WithEvents btnEdit As System.Web.UI.WebControls.Button
	Protected WithEvents ClientMSG As System.Web.UI.HtmlControls.HtmlGenericControl

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
		If Not Page.IsPostBack Then
			Dim myBusinessUnitProfile As New MBusinessUnitProfileInfo
			If ClientChoicesState(MClientChoices.BusinessUnitID) <> myBusinessUnitProfile.BUSINESS_UNIT_SEQ_ID Then
				BaseHelper.SelectBusinessUnit()
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