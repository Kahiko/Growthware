Imports DALFactory.Base.Application
Imports DALInterface.Base.Interfaces
Imports DALModel.Base.Messages
Imports System.Runtime.InteropServices

Namespace Base.SQLServer
    Public Class BMessages
		'Private Shared iBaseDAL As IMessages = FMessages.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"))
		Private Shared iBaseDAL As IMessages = AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DMessages")

        Public Shared Function GetMessage(ByVal messageName As String, Optional ByVal BUSINESS_UNIT_SEQ_ID As Integer = 1) As MMessageInfo
			Return iBaseDAL.GetMessage(messageName, BUSINESS_UNIT_SEQ_ID)
        End Function

        Public Shared Sub GetMessageNames(ByRef yourDataSet As DataSet, ByVal State As String)
			iBaseDAL.GetMessageNames(yourDataSet, State)
        End Sub

        Public Shared Function UpdateMessage(ByVal yourMessageInfo As MMessageInfo, Optional ByVal Account_seq_Id As Integer = 1) As Boolean
			Return iBaseDAL.UpdateMessage(yourMessageInfo, Account_seq_Id)
        End Function
    End Class
End Namespace