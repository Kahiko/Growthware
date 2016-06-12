Imports DALModel.Special.ClientChoices
Imports DALModel.Special.Accounts

Public Class Salutations
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents litSalutation As System.Web.UI.WebControls.Literal
	Protected WithEvents litSelectedLocation As System.Web.UI.WebControls.Literal
	Protected WithEvents litAppDisplayedName As System.Web.UI.WebControls.Literal

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If HttpContext.Current.User.Identity.IsAuthenticated Then
			If Not IsPostBack Then
				litAppDisplayedName.Text = BaseHelper.AppDisplayedName
				Dim myAccountProfile As MAccountProfileInfo
				Dim myAccountUtility As New AccountUtility(HttpContext.Current)
				myAccountProfile = myAccountUtility.GetAccountProfileInfo(context.Current.User.Identity.Name)
				Dim hour As Integer = CInt(Format(Date.Now, "HH"))
				Dim RelTime As String = String.Empty
				Select Case hour
					Case 1 To 12
						RelTime = "Morning"
					Case 13 To 18
						RelTime = "Afternoon"
					Case Else
						RelTime = "Evening"
				End Select
				litSalutation.Text = "Good " & RelTime & ", " & myAccountProfile.First_Name & " Today's date is " & Format(Date.Now, "MM/dd/yyyy")
				litSelectedLocation.Text = "Selected " & BaseHelper.BusinessUnitTranslation & " is " & ClientChoicesState(MClientChoices.BusinessUnitName)
			End If
		End If
	End Sub

End Class
