Imports GrowthWare.Framework.Common
Imports System.Web.Services
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Profiles

Public Class EncryptDecrypt
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <WebMethod(CacheDuration:=0, EnableSession:=False)>
    Public Shared Function Encrypt(ByVal textValue) As String
        Dim mRetVal As String = "Not authroized"
        Dim mSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile("Encryption_Helper"), AccountUtility.CurrentProfile())
        If mSecurityInfo.MayView Then
            mRetVal = CryptoUtility.Encrypt(textValue.Trim, SecurityEntityUtility.CurrentProfile().EncryptionType)
        End If
        Return mRetVal
    End Function

    <WebMethod(CacheDuration:=0, EnableSession:=False)>
    Public Shared Function Decrypt(ByVal textValue) As String
        Dim mRetVal As String = "Not authroized"
        Dim mSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile("EncryptionHelper"), AccountUtility.CurrentProfile())
        If mSecurityInfo.MayView Then
            mRetVal = CryptoUtility.Decrypt(textValue.Trim, SecurityEntityUtility.CurrentProfile().EncryptionType)
        End If
        Return mRetVal
    End Function
End Class