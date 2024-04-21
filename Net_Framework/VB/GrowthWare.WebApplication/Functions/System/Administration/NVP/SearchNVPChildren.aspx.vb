Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Base

Public Class SearchNVPChildren
    Inherits BaseWebpage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mNVP_SEQ_ID = GWWebHelper.GetQueryValue(Request, "NVP_SEQ_ID")
        If Not String.IsNullOrEmpty(mNVP_SEQ_ID) Then
            NVP_SEQ_ID.Value = mNVP_SEQ_ID.ToString()
        End If
    End Sub

End Class