Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities

Public Class ChangePassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
        If mAccountProfile.Status = 4 Then
            Me.trForceChange.Visible = True
            Me.trNormalChange.Visible = False
            Me.trOldPassword.Visible = False
            Me.NewPassword.Focus()
        Else
            Me.trForceChange.Visible = False
            Me.trNormalChange.Visible = True
            Me.trOldPassword.Visible = True
            Me.OldPassword.Focus()
        End If
        Dim mException As Exception = GWWebHelper.ExceptionError
        If Not mException Is Nothing Then
            clientMessage.Style.Add("display", "")
            clientMessage.InnerHtml = mException.Message.ToString()
            GWWebHelper.ExceptionError = Nothing
        End If
    End Sub

End Class