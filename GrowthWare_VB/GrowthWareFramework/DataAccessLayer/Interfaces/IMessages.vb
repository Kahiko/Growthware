Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.ModelObjects

Namespace DataAccessLayer
	Public Interface IMessages
		Inherits IDBInteraction

		Property Profile() As MMessageProfile

		Function GetAllMessages() As DataTable
		Function GetMessage() As DataRow
		Function UpdateMessage() As Boolean
		Function AddMessage() As Boolean
	End Interface
End Namespace