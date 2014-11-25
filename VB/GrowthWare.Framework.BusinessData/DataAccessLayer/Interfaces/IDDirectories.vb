Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.Interfaces
    Public Interface IDDirectories
        Inherits IDDBInteraction

        ''' <summary>
        ''' Gets the directories.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Function Directories() As DataTable

        ''' <summary>
        ''' Saves the instance of the Profile.
        ''' </summary>
        Sub Save(ByVal profile As MDirectoryProfile)

        ''' <summary>
        ''' Gets or sets the security entity seq ID.
        ''' </summary>
        ''' <value>The security entity seq ID.</value>
        Property SecurityEntitySeqId As Integer
    End Interface

End Namespace
