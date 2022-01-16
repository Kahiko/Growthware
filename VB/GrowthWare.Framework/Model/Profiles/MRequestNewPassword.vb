Namespace Model.Profiles
    ''' <summary>
    ''' Model object representing the Request New Password Profile
    ''' </summary>
    <Serializable(), CLSCompliant(True)> _
    Public Class MRequestNewPassword
        Inherits MMessageProfile

        Public Sub New()

        End Sub

        Public Property AccountName As String

        Public Property FullName As String

        Public Property Password As String

        Public Property Server As String

        Public Sub New(ByVal profile As MMessageProfile)
            MyBase.New(profile)
        End Sub
    End Class
End Namespace
