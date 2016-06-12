Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Common.CustomWebControls
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine

Partial Class SecurityByRole
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim myDS As DataSet = BSecurityReports.SecurityByRole(ClientChoicesState(MClientChoices.BusinessUnitID), BaseSettings.Environment)
        Dim myReport As New ReportDocument
        myReport.Load(Server.MapPath("Modules\System\RightHand\Reports\SecurityByBUByRoleByAccount.rpt"))
        BaseHelperOld.BindReport(CrystalReportViewer1, myReport, myDS)
	End Sub
End Class
