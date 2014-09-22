Namespace Model.Profiles.Interfaces
    Public Interface IMessageProfile
        Inherits IMProfile

        Property Body() As String
        Property Title() As String
        Property FormatAsHTML() As Boolean
        Sub FormatBody()
        Function GetTags(ByVal seporator As String) As String
    End Interface
End Namespace
