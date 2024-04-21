Imports GrowthWare.WebSupport.Base
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Enumerations
Imports System.Web.Services

Public Class SetLogLevel
    Inherits BaseWebpage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mLog As Logger = Logger.Instance()
        dropLogLevel.SelectedIndex = mLog.CurrentLogLevel
    End Sub

    <WebMethod(CacheDuration:=0, EnableSession:=False)>
    Public Shared Sub InvokeSetLogLevel(ByVal logLevel As Integer)
        Dim mLog As Logger = Logger.Instance()
        Select Case logLevel
            Case 0
                mLog.SetThreshold(LogPriority.Debug)
            Case 1
                mLog.SetThreshold(LogPriority.Info)
            Case 2
                mLog.SetThreshold(LogPriority.Warn)
            Case 3
                mLog.SetThreshold(LogPriority.Error)
            Case 4
                mLog.SetThreshold(LogPriority.Fatal)
            Case Else
                mLog.SetThreshold(LogPriority.Error)
        End Select
    End Sub
End Class