Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Model.WorkFlows
Imports System.Data

Partial Class AddEditWorkFlow
	Inherits ClientChoices.ClientChoicesUserControl

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
			Dim myWorkFlowProfileInfoCollection As New MWorkFlowProfileInfoCollection
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
        BaseHelperOld.SetDropSelection(dropAction, Profile.Action)
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
				Dim myWorkFlowProfileInfoCollection As New MWorkFlowProfileInfoCollection
				WorkFlowUtility.GetWorkFlowCollection(myWorkProfileInfo.WorkFlowName, myWorkFlowProfileInfoCollection)
			End If
		Else
			PopulateFromPage(myWorkProfileInfo)
			success = BWorkFlows.AddProfile(myWorkProfileInfo)
			If success Then
				WorkFlowUtility.RemoveCachedWorkFlowProfile(myWorkProfileInfo.WorkFlowName)
				WorkFlowUtility.RemoveWorkFlowDataView(myWorkProfileInfo.WorkFlowName)
				Dim myWorkFlowProfileInfoCollection As New MWorkFlowProfileInfoCollection
				WorkFlowUtility.GetWorkFlowCollection(myWorkProfileInfo.WorkFlowName, myWorkFlowProfileInfoCollection)
				myWorkProfileInfo = myWorkFlowProfileInfoCollection.GetWorkFlowByOrder(myWorkProfileInfo.Order)
				Dim myUrl As String = "selectworkflow"
				NavControler.NavTo(myUrl)
			End If
		End If
	End Sub
End Class
