Imports System.Configuration
Imports ApplicationBase.Common.Security
Imports ApplicationBase.Common.Globals

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
        Dim myEnvironment As String = BaseSettings.environment
        Dim myCryptoUtil As New CryptoUtil
        Try
            ConnectionString = myCryptoUtil.DecryptTripleDES(ConfigurationManager.AppSettings(myEnvironment & ConnectionKey))
        Catch ex As Exception
            ConnectionString = ConfigurationManager.AppSettings(myEnvironment & ConnectionKey)
        End Try
        Return ConnectionString
    End Function
End Class