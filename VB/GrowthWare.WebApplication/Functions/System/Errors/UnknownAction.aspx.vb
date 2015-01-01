Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.BasePages

Public Class UnknownAction
    Inherits BaseWebpage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mEx As Exception = GWWebHelper.ExceptionError
        clientMsg.InnerHtml = mEx.Message.ToString()
    End Sub

End Class