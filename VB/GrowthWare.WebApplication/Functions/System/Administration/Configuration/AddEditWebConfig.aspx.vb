Imports GrowthWare.WebSupport.Base
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common

Public Class AddEditWebConfig
    Inherits BaseWebpage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        populatePage()
    End Sub

    Private Sub populatePage()
        Dim mSecurityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
        Dim currentWebEnvironments As String() = ConfigSettings.Environments.Split(",")
        Dim environment As String = String.Empty
        dropEnvironments.Items.Clear()
        dropEnvironments.Items.Add(New ListItem("New", "New"))
        For Each environment In currentWebEnvironments
            Dim myListItem As New ListItem(environment, environment)
            dropEnvironments.Items.Add(myListItem)
        Next
        txtEnvironments.Text = ConfigSettings.Environments
    End Sub
End Class