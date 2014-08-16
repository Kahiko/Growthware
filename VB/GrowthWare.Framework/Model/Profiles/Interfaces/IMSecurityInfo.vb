Namespace Model.Profiles.Interfaces
    <CLSCompliant(True)> _
    Public Interface IMSecurityInfo
        ReadOnly Property AddRoles() As String()
        ReadOnly Property DeleteRoles() As String()
        ReadOnly Property EditRoles() As String()
        ReadOnly Property ViewRoles() As String()
    End Interface
End Namespace
