Imports GrowthWare.Framework.ModelObjects
Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base

Namespace DataAccessLayer
	Public Interface IStates
		Inherits IDBInteraction

		Property Profile() As MStateProfile

		Function GetStates() As DataTable
		Function UpdateState() As Boolean
	End Interface
End Namespace