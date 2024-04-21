Imports GrowthWare.WebSupport.Base
Imports GrowthWare.WebSupport.Utilities

Public Class UpdateSession
    Inherits BaseWebpage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AccountUtility.RemoveInMemoryInformation(True)
    End Sub

End Class