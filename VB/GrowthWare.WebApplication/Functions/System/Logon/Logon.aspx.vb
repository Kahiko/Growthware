Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities
Imports System.Globalization

Public Class Logon
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mException As Exception = GWWebHelper.ExceptionError
        If Not mException Is Nothing Then
            clientMessage.Style.Add("display", "")
            clientMessage.InnerHtml = mException.Message.ToString()
            GWWebHelper.ExceptionError = Nothing
        Else
            If AccountUtility.CurrentProfile().Account.ToString().ToUpper(New CultureInfo("en-US", False)) <> "ANONYMOUS" Then
                clientMessage.Style.Add("display", "")
                clientMessage.InnerHtml = "You are currently logged on as " + AccountUtility.CurrentProfile().Account.ToString()
            End If
        End If
    End Sub

End Class