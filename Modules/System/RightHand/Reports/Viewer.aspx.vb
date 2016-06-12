Imports CrystalDecisions.Enterprise
Imports CrystalDecisions.ReportAppServer.ClientDoc
Imports CrystalDecisions.Enterprise.Viewing
Imports CrystalDecisions.ReportAppServer.Controllers
Imports BLL.Base.ClientChoices
Imports Common.Cache

Public Class Viewer
	Inherits ClientChoices.ClientChoicesPage

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Shadows Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Protected WithEvents CrystalReportViewer1 As CrystalDecisions.Web.CrystalReportViewer

	Private ceSession As EnterpriseSession
	Private ceInfoStore As InfoStore
	Private ceEnterpriseService As EnterpriseService

	'for page server
	Private myEnterpriseService As EnterpriseService
	Private serviceAsObject As Object
	Private myPSReportFactory As PSReportFactory
	Private myReportSource As ReportSource
	Private myISCRReportSource As ISCRReportSource

	'for RAS server
	Private rptClientDoc As ReportClientDocument
	Private rptAppFactory As ReportAppFactory

	'holder for report ID
	Private iRptID As Integer

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Try
			If Not IsPostBack Then
				'grab the Enterprise session
				If TypeOf Cache.Item(HttpContext.Current.User.Identity.Name & "ceSession") Is Object Then

					ceSession = Cache.Item(HttpContext.Current.User.Identity.Name & "ceSession")
					'Create the infostore object
					ceEnterpriseService = ceSession.GetService("", "InfoStore")
					ceInfoStore = New InfoStore(ceEnterpriseService)

					'make sure we have the report ID
					If TypeOf Request.Item("id") Is Object Then
						iRptID = Request.Item("id")

						If Request.Item("server").ToString = "page" Then

							'view the report using page server
							myEnterpriseService = ceSession.GetService("PSReportFactory")
							serviceAsObject = myEnterpriseService.Interface
							myPSReportFactory = CType(serviceAsObject, PSReportFactory)
							myReportSource = myPSReportFactory.OpenReportSource(iRptID)
							myISCRReportSource = myReportSource

							'place the report source in session for use with postbacks
							CacheControler.AddToCacheDependency(HttpContext.Current.User.Identity.Name & "ceSession", myISCRReportSource)
							'Session.Add("ReportSource", myISCRReportSource)

							CrystalReportViewer1.Visible = True
							CrystalReportViewer1.EnterpriseLogon = ceSession
							CrystalReportViewer1.ReportSource = myISCRReportSource

						Else
							'grab the report app factory from the session
							rptAppFactory = ceInfoStore.EnterpriseSession.Interface.Service("", "RASReportFactory")

							'use RAS to open the report
							rptClientDoc = rptAppFactory.OpenDocument(iRptID, CdReportClientDocumentOpenOptionsEnum.cdReportClientDocumentOpenAsReadOnly)

							'place the report source in session for use with postbacks
							CacheControler.AddToCacheDependency(HttpContext.Current.User.Identity.Name & "ceSession", myISCRReportSource)
							'Session.Add("ReportSource", rptClientDoc)

							'view the report
							CrystalReportViewer1.Visible = True
							CrystalReportViewer1.ReportSource = rptClientDoc
						End If
					Else
						'no report ID
						Response.Write("You have not selected a report to view.<br>")
						Response.Write("Please click <a href='Index.aspx'>here</a> to return to the selection page.<br>")
					End If

				Else
					'no Enterprise session available
					Response.Write("No Valid Enterprise Session Found!<br>")
					Response.Write("Please click <a href='Index.aspx'>here</a> to return to the logon page.<br>")
				End If
			Else
				'grab the report source out of session and pass to the viewer
				CrystalReportViewer1.ReportSource = Session.Item("ReportSource")
				CrystalReportViewer1.Visible = True
			End If

		Catch err As Exception
			Response.Write("There was an error: <br>")
			Response.Write(err.Message.ToString + "<br>")
			Response.Write("Please click <a href='Index.aspx'>here</a> to return to the logon page.<br>")
		End Try
	End Sub
End Class