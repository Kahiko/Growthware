Imports System.Web.Services
Imports GrowthWare.WebSupport

Public Class RandomNumbers
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <WebMethod(CacheDuration:=0, EnableSession:=False)>
    Public Shared Function GetRandomNumbers(ByVal amountOfNumbers As Integer, ByVal maxNumber As Integer, ByVal minNumber As Integer) As String
        Dim mRetVal As String = String.Empty
        Dim X As Integer = 1
        For X = 1 To amountOfNumbers
            mRetVal += GWWebHelper.GetRandomNumber(maxNumber, minNumber) + ", "
        Next
        Return mRetVal
    End Function
End Class