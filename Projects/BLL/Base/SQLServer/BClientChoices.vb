Imports DALFactory.Base.Application
Imports DALInterface.Base.Interfaces

Namespace Base.SQLServer
    Public Class BClientChoices
		'Private Shared iBaseDAL As IClientChoices = FClientChoices.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"))
		Private Shared iBaseDAL As IClientChoices = AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DClientChoices")

        Public Shared Function GetPersonalizationData(ByVal dsResult As DataSet, ByVal AccountName As String) As DataSet
			Return iBaseDAL.GetClientChoicesData(AccountName)
        End Function

        Public Shared Function Save(ByVal ClientChoices As System.Collections.Hashtable, Optional ByVal Account_seq_Id As Integer = 1) As Boolean
			Return iBaseDAL.Save(ClientChoices)
        End Function
    End Class
End Namespace