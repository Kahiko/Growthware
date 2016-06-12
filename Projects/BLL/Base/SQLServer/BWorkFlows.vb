Imports DALFactory.Base.Application
Imports DALInterface.Base.Interfaces
Imports DALModel.Base.WorkFlows

Namespace Base.SQLServer
    Public Class BWorkFlows
		'Private Shared iBaseDAL As IWorkFlows = FWorkFlows.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"))
		Private Shared iBaseDAL As IWorkFlows = AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DWorkFlows")

        Public Shared Function GetCollectionFromDB(ByVal WorkFlowName As String) As MWorkFlowProfileInfoCollection
			Return iBaseDAL.GetCollectionFromDB(WorkFlowName)
        End Function

        Public Shared Sub GetWorkFlowsFromDB(ByRef YourDataSet As DataSet)
			iBaseDAL.GetWorkFlowsFromDB(YourDataSet)
        End Sub

        Public Shared Sub GetWorkFlowsFromDB(ByVal WORK_FLOW_NAME As String, ByRef YourDataSet As DataSet)
			iBaseDAL.GetWorkFlowsFromDB(WORK_FLOW_NAME, YourDataSet)
        End Sub

        Public Shared Function AddProfile(ByVal Profile As MWorkFlowProfileInfo) As Boolean
			Return iBaseDAL.AddProfile(Profile)
        End Function

        Public Shared Function DeleteProfile(ByVal Profile As MWorkFlowProfileInfo) As Boolean
			Return iBaseDAL.DeleteProfile(Profile)
        End Function

        Public Shared Function UpdateProfile(ByVal Profile As MWorkFlowProfileInfo) As Boolean
			Return iBaseDAL.UpdateProfile(Profile)
        End Function
    End Class
End Namespace