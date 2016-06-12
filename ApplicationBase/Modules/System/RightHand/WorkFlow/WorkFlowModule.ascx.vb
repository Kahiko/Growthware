Imports ApplicationBase.Common.Globals

Partial Class WorkFlowModule
    Inherits System.Web.UI.UserControl

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim WorkFlowName As String = Request.QueryString("WFN")
        If WorkFlowName Is Nothing OrElse WorkFlowName.Trim.Length = 0 Then
            Dim ex As New ApplicationException("Must set the WFN= parameter in the URL")
            BaseHelperOld.ExceptionError = ex
            NavControler.NavTo(ex)
        End If
        Dim myURL As String = BaseSettings.GetURL()
        NavControler.StartWorkFlow(WorkFlowName, myURL)
    End Sub

End Class
