Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.ModelObjects

Namespace DataAccessLayer
	Public Interface IClientChoices
		Inherits IDBInteraction

		Property Account() As String
		Property theChoices() As Hashtable

		Function GetChoices() As DataRow
		Function Update() As Boolean
	End Interface
End Namespace