Namespace Model.Profiles
    <Serializable(), CLSCompliant(True)> _
    Public Class MRequestNewPassword
        Inherits MMessageProfile

        Public Property AccountName As String

        Public Property FullName As String

        Public Property Password As String

        Public Property Server As String
    End Class
End Namespace
