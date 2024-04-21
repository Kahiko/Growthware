Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.Interfaces
    ''' <summary>
    ''' Public interface for DDBInformation
    ''' </summary>
    Public Interface IDBInformation
        Inherits IDDBInteraction

        ''' <summary>
        ''' Gets or sets the profile.
        ''' </summary>
        ''' <value>The profile.</value>
        Property Profile As MDBInformation

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <returns>DataRow.</returns>
        ReadOnly Property GetProfileRow() As DataRow

        ''' <summary>
        ''' Updates the profile.
        ''' </summary>
        ''' <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        Function UpdateProfile() As Boolean
    End Interface

End Namespace