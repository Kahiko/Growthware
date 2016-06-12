Namespace Model.Profiles.Interfaces
    Public Interface IMessageProfile
        Inherits IMProfile

        Property Body() As String
        Property Title() As String
        Property FormatAsHtml() As Boolean
        Sub FormatBody()
        Function GetTags(ByVal separator As String) As String
    End Interface
End Namespace
