Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer
	Public Interface IDClientChoices
		Inherits IDDBInteraction

		''' <summary>
		''' Retrieves a row of data given the account
		''' </summary>
		''' <param name="account">String</param>
		''' <returns>DataRow</returns>
		''' <remarks></remarks>
		Function GetChoices(ByRef account As String) As DataRow

		''' <summary>
		''' Save the client choices
		''' </summary>
		''' <param name="clientChoicesStateHashTable">Hashtable</param>
		''' <remarks></remarks>
		Sub Save(ByRef clientChoicesStateHashTable As Hashtable)
	End Interface
End Namespace
