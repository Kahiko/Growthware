Imports ApplicationBase.Model.WorkFlows

Public Interface IWorkFlows
	Function GetCollectionFromDB(ByVal WorkFlowName As String) As MWorkFlowProfileInfoCollection
	Sub GetWorkFlowsFromDB(ByRef YourDataSet As DataSet)
	Sub GetWorkFlowsFromDB(ByVal WORK_FLOW_NAME As String, ByRef YourDataSet As DataSet)
	Function AddProfile(ByVal profile As MWorkFlowProfileInfo, Optional ByVal Account_Seq_id As Integer = 1) As Boolean
	Function UpdateProfile(ByVal profile As MWorkFlowProfileInfo, Optional ByVal Account_seq_Id As Integer = 1) As Boolean
	Function DeleteProfile(ByVal Profile As MWorkFlowProfileInfo, Optional ByVal Account_seq_Id As Integer = 1) As Boolean
End Interface