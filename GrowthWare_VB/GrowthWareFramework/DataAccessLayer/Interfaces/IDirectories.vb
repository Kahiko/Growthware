Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.ModelObjects

Namespace DataAccessLayer
	Public Interface IDirectories
		Inherits IDBInteraction

		Property Profile() As MDirectoryProfile

		Function GetDirectories() As DataTable
		Function SaveDirectory() As Boolean
		Function AddDirectory() As Integer
	End Interface
End Namespace