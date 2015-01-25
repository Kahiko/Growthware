Imports GrowthWare.WebSupport.Utilities

Public Class Logoff
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AccountUtility.Logoff()
    End Sub

End Class