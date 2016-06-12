Imports BLL.Base.ClientChoices
Imports Common.CustomWebControls
Imports DALModel.Base
Imports DALModel.Base.States
Imports DALModel.Special.ClientChoices

Public Class EditStates
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Tabstrip1 As Common.CustomWebControls.TabStrip
    Protected WithEvents pnlGeneral As System.Web.UI.WebControls.Panel
	Protected WithEvents btnSave As System.Web.UI.WebControls.Button
	Protected WithEvents bottomTabStrip As System.Web.UI.HtmlControls.HtmlTableCell
	Protected WithEvents litState As System.Web.UI.WebControls.Literal
    Protected WithEvents AlphaPicker1 As Common.CustomWebControls.AlphaPicker

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Protected WithEvents statesGeneral As statesGeneral

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		If Not IsPostBack Then
			bottomTabStrip.BgColor = ClientChoicesState(MClientChoices.HeadColor)
		End If
        Dim statesProfileInfo As MStateProfileInfo
        Dim myStatesUtility As New StatesUtility
        statesProfileInfo = myStatesUtility.GetInfoByState(Request.QueryString("id"))
        litState.Text = statesProfileInfo.LongName.Trim
	End Sub

	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		statesGeneral.btnSave_Click(sender, e)
		StatesUtility.RemoveCachedStatesDV()
	End Sub
End Class
