Imports ApplicationBase.Common.Globals
Imports ApplicationBase.ClientChoices
Imports System.Text

Public Class TestPage
    Inherits ClientChoices.ClientChoicesPage

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents btnThrowError As System.Web.UI.WebControls.Button
	Protected WithEvents btnTestMail As System.Web.UI.WebControls.Button
	Protected WithEvents FirstName As System.Web.UI.WebControls.TextBox
	Protected WithEvents btnLogMSG As System.Web.UI.WebControls.Button
	Protected WithEvents btnGUID As System.Web.UI.WebControls.Button
	Protected WithEvents litOutput As System.Web.UI.WebControls.Literal

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Shadows Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

    Protected CustomStyles As System.Web.UI.HtmlControls.HtmlGenericControl
	Protected MainStyle As System.Web.UI.HtmlControls.HtmlGenericControl
	Protected NiftyCornersStyle As System.Web.UI.HtmlControls.HtmlGenericControl
	Protected NiftyPrintStyle As System.Web.UI.HtmlControls.HtmlGenericControl

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MainStyle.Attributes.Item("href") = BaseSettings.rootSite & "Styles/main.css"
        NiftyCornersStyle.Attributes.Item("href") = BaseSettings.rootSite & "Styles/niftyCorners.css"
        NiftyPrintStyle.Attributes.Item("href") = BaseSettings.rootSite & "Styles/niftyPrint.css"

        Dim strBuilder As New StringBuilder
        strBuilder.Append("        <script language=""JavaScript"" type=""text/javascript"">" & vbCrLf)
        strBuilder.Append("           var imagePath='" & BaseSettings.ImagePath & "';" & vbCrLf)
		strBuilder.Append("           var HeadColor='" & ClientChoicesState("HEAD_COLOR") & "';" & vbCrLf)
		strBuilder.Append("           var SubheadColor='" & ClientChoicesState("SUB_HEAD_COLOR") & "';" & vbCrLf)
        strBuilder.Append("           var calendarPage='" & BaseSettings.rootSite & "Pages/System/DatePicker.aspx" & "';" & vbCrLf)
        strBuilder.Append("        </script>" & vbCrLf)
        'Page.RegisterClientScriptBlock("CustomScripts", strBuilder.ToString)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "CustomScripts", strBuilder.ToString)
    End Sub 'Page_Load

    Private Sub btnThrowError_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnThrowError.Click
        Dim ex As New Exception("just testing errors")
        ex.Source = "TestPage"
        Throw New ApplicationException("just testing error", ex)
    End Sub 'btnThrowError_Click

    Private Sub btnTestMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestMail.Click
        Dim myRecipient As String = "michael.regan@umassmed.edu"
        Dim myFromAccount As String = myRecipient
        Dim mySubject As String = "Complete ADHD form"
        Dim myBody As String = mySubject & vbCrLf & "First Name = " & FirstName.Text
        Try
            NotifyUtility.SendMail(mySubject, myBody, myRecipient)
        Catch ex As Exception
            ' show an error message on the page
        End Try
    End Sub 'btnTestMail_Click

    Private Sub btnLogMSG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogMSG.Click
		Dim myUrl As String = "testpage&myYa=yaya&mYDa=dada"
		NavControler.NavTo(myUrl)
    End Sub

End Class