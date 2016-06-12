Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base

Namespace DataAccessLayer.Interfaces
    Public Interface IDClientChoices
        Inherits IDDBInteraction

        ''' <summary>
        ''' Retrieves a row of data given the account
        ''' </summary>
        ''' <param name="account">String</param>
        ''' <returns>DataRow</returns>
        ''' <remarks></remarks>
        Function GetChoices(ByVal account As String) As DataRow

        ''' <summary>
        ''' Save the client choices
        ''' </summary>
        ''' <param name="clientChoicesStateHashTable">Hashtable</param>
        ''' <remarks></remarks>
        Sub Save(ByVal clientChoicesStateHashtable As Hashtable)
    End Interface
End Namespace

