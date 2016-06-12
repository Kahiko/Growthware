Imports System.Configuration
Imports Common.Security.BaseSecurity

Public Class ConnectionHelper
    '*********************************************************************
    '
    ' ConnectionString Method
    '
    ' Get the connection string from the web.config file.
    '
    '*********************************************************************
    Public Shared Function GetConnectionString(ByVal ConnectionKey As String) As String
        Dim ConnectionString As String = String.Empty
        Dim myEnvironment As String = ConfigurationSettings.AppSettings("Environment")
        Dim myCryptoUtil As New CryptoUtil
        Try
            ConnectionString = myCryptoUtil.DecryptTripleDES(ConfigurationSettings.AppSettings(myEnvironment & ConnectionKey))
        Catch ex As Exception
            ConnectionString = ConfigurationSettings.AppSettings(myEnvironment & ConnectionKey)
        End Try
        Return ConnectionString
    End Function
End Class