Imports System.Web.Services
Imports GrowthWare.WebSupport

Public Class GUIDHelper
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <WebMethod(CacheDuration:=0, EnableSession:=False)>
    Public Shared Function GetGUID() As String
        Dim mRetVal As String = String.Empty
        mRetVal = GWWebHelper.GetNewGuid()
        Return mRetVal
    End Function
End Class