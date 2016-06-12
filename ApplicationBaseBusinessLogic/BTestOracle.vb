Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Factory
Imports ApplicationBase
Imports ApplicationBase.Interfaces

Public Class BTestOracle
	'Private Shared iDal As ITestOracle = FTestOracle.Create
    Private Shared iBaseDAL As ITestOracle = FactoryObject.Create(BaseSettings.applicationBaseDAL, "DTestOracle")

	Public Shared Function testOracle(ByRef dstAddresses As DataSet) As DataSet
		Return iBaseDAL.GetAddresses(dstAddresses)
	End Function
End Class