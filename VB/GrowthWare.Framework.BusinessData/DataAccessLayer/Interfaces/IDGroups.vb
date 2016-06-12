Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.Interfaces
    Public Interface IDGroups
        Inherits IDDBInteraction

        ''' <summary>
        ''' Searches the specified search critera.
        ''' </summary>
        ''' <param name="searchCriteria">The search critera.</param>
        ''' <returns>DataTable.</returns>
        Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable

        ''' <summary>
        ''' Gets or sets the security entity seq ID.
        ''' </summary>
        ''' <value>The security entity seq ID.</value>
        Property SecurityEntitySeqId As Integer

        ''' <summary>
        ''' Deletes the group.
        ''' </summary>
        Sub DeleteGroup()

        ''' <summary>
        ''' Gets the group roles.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Function GroupRoles() As DataTable

        ''' <summary>
        ''' Gets the groups by security entity.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Function GroupsBySecurityEntity() As DataTable

        ''' <summary>
        ''' Gets or sets the group roles profile.
        ''' </summary>
        ''' <value>The group roles profile.</value>
        Property GroupRolesProfile() As MGroupRoles

        ''' <summary>
        ''' Gets the profile data.
        ''' </summary>
        ''' <returns>DataRow.</returns>
        Function ProfileData() As DataRow

        ''' <summary>
        ''' Gets or sets the profile.
        ''' </summary>
        ''' <value>The profile.</value>
        Property Profile() As MGroupProfile

        ''' <summary>
        ''' Saves this instance.
        ''' </summary>
        Sub Save()

        ''' <summary>
        ''' Updates the group roles.
        ''' </summary>
        Sub UpdateGroupRoles()
    End Interface
End Namespace

