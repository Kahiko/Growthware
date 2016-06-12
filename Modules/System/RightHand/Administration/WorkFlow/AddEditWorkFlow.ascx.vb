Imports BLL.Base.SQLServer
Imports BLL.Base.ClientChoices
Imports DALModel.Base.WorkFlows

Public Class AddEditWorkFlow
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
	Protected WithEvents litClientMsg As System.Web.UI.WebControls.Literal
	Protected WithEvents txtWorkFlowName As System.Web.UI.WebControls.TextBox
	Protected WithEvents Requiredfieldvalidator2 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents txtOrder_ID As System.Web.UI.WebControls.TextBox
	Protected WithEvents Requiredfieldvalidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents btnSave As System.Web.UI.WebControls.Button
	Protected WithEvents litWorkFlowSeqID As System.Web.UI.WebControls.Literal
    Protected WithEvents dropAction As System.Web.UI.WebControls.DropDownList

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
	End Sub

	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim myWorkProfileInfo As MWorkFlowProfileInfo
        If Not IsPostBack Then
            Dim myDataView As DataView = AppModulesUtility.GetModulesDataView()
            dropAction.DataSource = myDataView
            dropAction.DataTextField = "Action"
            dropAction.DataValueField = "MODULE_SEQ_ID"
            dropAction.DataBind()
        End If
        If Request.QueryString("Action").Trim.ToLower.LastIndexOf("add") = -1 Then
            Dim WorkFlowName As String = Request.QueryString("WorkFlowName")
            Dim WorkFlowID As String = Request.QueryString("ID")
            Dim myWorkFlowProfileInfoCollection As MWorkFlowProfileInfoCollection
            WorkFlowUtility.GetWorkFlowCollection(WorkFlowName, myWorkFlowProfileInfoCollection)
            myWorkProfileInfo = myWorkFlowProfileInfoCollection.GetWorkFlowByID(WorkFlowID)
            litWorkFlowSeqID.Visible = True
        Else
            myWorkProfileInfo = New MWorkFlowProfileInfo
            litWorkFlowSeqID.Visible = False
        End If
        PopulatePage(myWorkProfileInfo)
	End Sub

	Private Sub PopulatePage(ByRef Profile As MWorkFlowProfileInfo)
		litWorkFlowSeqID.Text = Profile.WORK_FLOW_SEQ_ID
		txtWorkFlowName.Text = Profile.WorkFlowName
        txtOrder_ID.Text = Profile.Order
        BaseHelper.SetDropSelection(dropAction, Profile.Action)
    End Sub

    Private Sub PopulateFromPage(ByRef Profile As MWorkFlowProfileInfo)
        Profile.WORK_FLOW_SEQ_ID = litWorkFlowSeqID.Text
        Profile.WorkFlowName = txtWorkFlowName.Text
        Profile.Order = txtOrder_ID.Text
        Profile.Action = dropAction.SelectedValue
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim myWorkProfileInfo As New MWorkFlowProfileInfo
        Dim success As Boolean = False
        If Request.QueryString("Action").Trim.ToLower.LastIndexOf("add") = -1 Then
            PopulateFromPage(myWorkProfileInfo)
            success = BWorkFlows.UpdateProfile(myWorkProfileInfo)
            If success Then
                WorkFlowUtility.RemoveCachedWorkFlowProfile(myWorkProfileInfo.WorkFlowName)
                WorkFlowUtility.RemoveWorkFlowDataView(myWorkProfileInfo.WorkFlowName)
                Dim myWorkFlowProfileInfoCollection As MWorkFlowProfileInfoCollection
                WorkFlowUtility.GetWorkFlowCollection(myWorkProfileInfo.WorkFlowName, myWorkFlowProfileInfoCollection)
            End If
        Else
            PopulateFromPage(myWorkProfileInfo)
            success = BWorkFlows.AddProfile(myWorkProfileInfo)
            If success Then
                WorkFlowUtility.RemoveCachedWorkFlowProfile(myWorkProfileInfo.WorkFlowName)
                WorkFlowUtility.RemoveWorkFlowDataView(myWorkProfileInfo.WorkFlowName)
                Dim myWorkFlowProfileInfoCollection As MWorkFlowProfileInfoCollection
                WorkFlowUtility.GetWorkFlowCollection(myWorkProfileInfo.WorkFlowName, myWorkFlowProfileInfoCollection)
                myWorkProfileInfo = myWorkFlowProfileInfoCollection.GetWorkFlowByOrder(myWorkProfileInfo.Order)
                Dim myUrl As String = "selectworkflow"
                NavControler.NavTo(myUrl)
            End If
        End If
    End Sub
End Class
