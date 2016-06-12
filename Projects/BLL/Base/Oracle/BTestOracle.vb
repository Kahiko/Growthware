Imports DALFactory.Base.Application
Imports DALModel
Imports DALInterface.Base.Interfaces

Namespace Base.Oracle
    Public Class BTestOracle
		'Private Shared iDal As ITestOracle = FTestOracle.Create
		Private Shared iBaseDAL As ITestOracle = AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DTestOracle")

        Public Shared Function testOracle(ByRef dstAddresses As DataSet) As DataSet
			Return iBaseDAL.GetAddresses(dstAddresses)
        End Function
    End Class
End Namespace