Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Factory
Imports ApplicationBase.Interfaces

Public Class BClientChoices
	'Private Shared iBaseDAL As IClientChoices = FClientChoices.Create(Configuration.ConfigurationManager.AppSettings("BaseDAL"))
    Private Shared iBaseDAL As IClientChoices = FactoryObject.Create(BaseSettings.applicationBaseDAL, "DClientChoices")

	Public Shared Function GetPersonalizationData(ByVal dsResult As DataSet, ByVal AccountName As String) As DataSet
		Return iBaseDAL.GetClientChoicesData(AccountName)
	End Function

	Public Shared Function Save(ByVal ClientChoices As System.Collections.Hashtable, Optional ByVal Account_seq_Id As Integer = 1) As Boolean
		Return iBaseDAL.Save(ClientChoices)
	End Function
End Class