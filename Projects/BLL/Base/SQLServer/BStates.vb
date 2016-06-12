Imports DALFactory.Base.Application
Imports DALInterface.Base.Interfaces
Imports DALModel.Base.States
Imports System.Runtime.InteropServices

Namespace Base.SQLServer
    Public Class BStates

		'Private Shared iBaseDAL As IStates = FStates.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"))
		Private Shared iBaseDAL As IStates = AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DStates")

        Public Shared Function GetAllStates(ByRef dsStates As DataSet) As DataSet
			Return iBaseDAL.GetAllStates(dsStates)
        End Function

        Public Shared Function GetStateArray(ByVal ACCOUNT_SEQ_ID As Integer, ByVal isAdmin As Boolean) As String
            If Not isAdmin Then
				Return iBaseDAL.GetStateArray(ACCOUNT_SEQ_ID)
            Else
				Return iBaseDAL.GetAdminStateArray(ACCOUNT_SEQ_ID)
            End If
        End Function

        Public Shared Function UpdateStateProfileInfo(ByVal stateProfileInfo As MStateProfileInfo) As Boolean
			Return iBaseDAL.UpdateStateProfileInfo(stateProfileInfo)
        End Function

        Public Shared Function GetAllStates() As MStateProfileInfoCollection
			Return iBaseDAL.GetAllStates
        End Function
    End Class
End Namespace