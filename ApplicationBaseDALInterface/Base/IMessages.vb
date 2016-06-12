Imports ApplicationBase.Model.Messages

Public Interface IMessages
	Function GetMessage(ByVal messageName As String, Optional ByVal BUSINESS_UNIT_SEQ_ID As Integer = 1) As MMessageInfo
	Sub GetMessageNames(ByRef yourDataSet As DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer)
	Function UpdateMessage(ByVal yourMessageInfo As MMessageInfo, Optional ByVal Account_Seq_id As Integer = 1) As Boolean
End Interface