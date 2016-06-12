Public Interface IClientChoices
	Function CreateClientChoicesAccount(ByVal AccountName As String, Optional ByVal Account_Seq_id As Integer = 1) As Boolean
	Function GetClientChoicesData(ByVal AccountName As String) As DataSet
	Function Save(ByVal ClientChoices As System.Collections.Hashtable) As Boolean
End Interface