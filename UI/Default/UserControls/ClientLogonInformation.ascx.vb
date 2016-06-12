Imports BLL.Base.ClientChoices
Imports DALModel.Special.ClientChoices

Public Class ClientLogonInformation
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
	Protected WithEvents Label1 As System.Web.UI.WebControls.Label
	Protected WithEvents lblAccount As System.Web.UI.WebControls.Label
	Protected WithEvents Label2 As System.Web.UI.WebControls.Label
	Protected WithEvents trClientSecurityInformation As System.Web.UI.HtmlControls.HtmlTableCell
	Protected WithEvents lblSelectedClientName As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblSelectedBusinessUnit As System.Web.UI.WebControls.Label
    Protected WithEvents lblSelectedBusinessUnitName As System.Web.UI.WebControls.Label

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
		If context.User.Identity.IsAuthenticated Then
            lblAccount.Text = ClientChoicesState(MClientChoices.AccountName)
            Label3.Text = Label3.Text.Replace("BusinessUnitTranslation", BaseHelper.BusinessUnitTranslation)
            lblSelectedBusinessUnitName.Text = ClientChoicesState(MClientChoices.BusinessUnitName)
        Else
            trClientSecurityInformation.Visible = False
        End If
	End Sub
End Class