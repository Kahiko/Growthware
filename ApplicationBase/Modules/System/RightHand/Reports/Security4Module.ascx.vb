Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.CustomWebControls
imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine

Partial Class Security4Module
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim myDS As DataSet = BSecurityReports.Security4Module(ClientChoicesState(MClientChoices.BusinessUnitID), BaseSettings.Environment)
        Dim myReport As New ReportDocument
        myReport.Load(Server.MapPath("Modules\System\RightHand\Reports\Security4ModuleDSRPT.rpt"))

        BaseHelperOld.BindReport(CrystalReportViewer1, myReport, myDS)
	End Sub
End Class