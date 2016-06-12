Imports DALModel.Base.Accounts.Security
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices
Imports BLL.Base.ClientChoices

Public Class ChooseBusinessUnit
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents clientMSG As System.Web.UI.WebControls.Literal

    Protected WithEvents btnGo As System.Web.UI.WebControls.Button
    Protected WithEvents dropBusinessUnits As System.Web.UI.WebControls.DropDownList

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private NeedRedirect As Boolean = False
    Private RedirectURL As String = String.Empty

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim myDataSet As New DataSet
        Dim myAccountUtility As New AccountUtility(HttpContext.Current)
        Dim myAccountProfileInfo As New MAccountProfileInfo
		myAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name)
		Dim myAccountSecurityInfo As MAccountSecurityInfo = myAccountUtility.GetAccountSecurityInfo(myAccountSecurityInfo)
        Dim isAdmin As Boolean = False
        If myAccountSecurityInfo.IsSystemAdministrator OrElse HttpContext.Current.User.IsInRole("Super User") Then
            isAdmin = True
        End If
        BusinessUnitUtility.GetValidBusinessUnits(myDataSet, myAccountProfileInfo.ACCOUNT_SEQ_ID, isAdmin)
        dropBusinessUnits.DataSource = myDataSet.Tables(0).DefaultView
        dropBusinessUnits.DataValueField = "BUSINESS_UNIT_SEQ_ID"
        dropBusinessUnits.DataTextField = "Name"
        dropBusinessUnits.DataBind()
        BaseHelper.SetDropSelection(dropBusinessUnits, ClientChoicesState(MClientChoices.BusinessUnitID))
        If Not IsPostBack Then
            If Not context.Session("clientMSG") Is Nothing Then
                clientMSG.Text = context.Session("clientMSG")
                context.Session.Remove("clientMSG")
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If NeedRedirect Then NavControler.NavTo(RedirectURL)
    End Sub ' Page_PreRender

    Private Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        ' save the client state to the database must be done before getting the roles
        ClientChoicesState(MClientChoices.BusinessUnitID) = dropBusinessUnits.SelectedValue
        ClientChoicesState(MClientChoices.BusinessUnitName) = dropBusinessUnits.SelectedItem.Text
        ' update all of your in memeory information
        If HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			myAccountUtility.RemoveAccountInMemoryInformation()
        End If
        context.Session("clientMSG") = "The " & BaseHelper.BusinessUnitTranslation & " has been changed to " & dropBusinessUnits.SelectedItem.Text
        RedirectURL = Request.QueryString("Action")
        NeedRedirect = True
        ' refresh the view
        If Not Request.QueryString("ReturnURL") Is Nothing Then NavControler.NavTo(Request.QueryString("ReturnURL"))
    End Sub
End Class