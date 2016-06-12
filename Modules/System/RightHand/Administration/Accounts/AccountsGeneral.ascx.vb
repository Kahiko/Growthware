Imports BLL.Special
Imports BLL.Base.SQLServer
Imports Common.Security.BaseSecurity
Imports DALModel.Base.Accounts
Imports DALModel.Base.BusinessUnits
Imports DALModel.Base.Messages
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices
Imports BLL.Base.ClientChoices

Public Class AccountsGeneral
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtAccount_seq_id As System.Web.UI.WebControls.TextBox
    Protected WithEvents litAccount As System.Web.UI.WebControls.Literal
    Protected WithEvents txtAccount As System.Web.UI.WebControls.TextBox
    Protected WithEvents dropStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents litFailedAttempts As System.Web.UI.WebControls.Literal
    Protected WithEvents txtFailedAttempts As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator5 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator6 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtMiddleName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPreferedName As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents Regularexpressionvalidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents dropTimezone As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtLocation As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkEnableNotifications As System.Web.UI.WebControls.CheckBox
    Protected WithEvents btnSave As System.Web.UI.WebControls.Button
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
	Protected WithEvents litAccountWarning As System.Web.UI.WebControls.Literal
	Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Requiredfieldvalidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Requiredfieldvalidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RangeValidator1 As System.Web.UI.WebControls.RangeValidator

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private _UpdatedACCOUNT_SEQ_ID As Integer
    Private _UpdatedAccountName As String

    Public Property UpdatedACCOUNT_SEQ_ID() As Integer
        Get
            Return _UpdatedACCOUNT_SEQ_ID
        End Get
        Set(ByVal Value As Integer)
            _UpdatedACCOUNT_SEQ_ID = Value
        End Set
    End Property

    Public Property UpdateAccountName() As String
        Get
            Return _UpdatedAccountName
        End Get
        Set(ByVal Value As String)
            _UpdatedAccountName = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
			Dim accountProfileInfoToUpdate As MAccountProfileInfo
			GetProfile(accountProfileInfoToUpdate)
			PopulatePage(accountProfileInfoToUpdate)
			If accountProfileInfoToUpdate.ACCOUNT.Trim.ToLower = HttpContext.Current.User.Identity.Name.Trim.ToLower Then
				litAccountWarning.Text = "If you change this you will be logged off!"
				litAccountWarning.Visible = True
			End If
		End If
    End Sub

    Public Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
		Page.Validate()
		If Page.IsValid Then
			Dim accountProfileInfoToUpdate As MAccountProfileInfo
			Dim myaccountProfileInfo As MAccountProfileInfo
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			myaccountProfileInfo = myAccountUtility.GetAccountProfileInfo(context.User.Identity.Name)
			GetProfile(accountProfileInfoToUpdate)
			PopulateFromPage(accountProfileInfoToUpdate)
			accountProfileInfoToUpdate.UPDATED_BY = myaccountProfileInfo.ACCOUNT_SEQ_ID
			accountProfileInfoToUpdate.UPDATED_DATE = Now
			If Not Request.QueryString("Action").ToLower = "addaccount" Then
				BAccount.UpdateProfile(accountProfileInfoToUpdate)
            Else
                Dim msgInfo As MMessageInfo
                Dim myCryptoUtil As New CryptoUtil
                Dim randomPassword As String = BaseHelper.GetRandomPassword
                randomPassword = myCryptoUtil.EncryptTripleDES(randomPassword)
				accountProfileInfoToUpdate.CREATED_BY = myaccountProfileInfo.ACCOUNT_SEQ_ID
				accountProfileInfoToUpdate.DATE_CREATED = Now
				accountProfileInfoToUpdate.LAST_LOGIN = Now
				accountProfileInfoToUpdate.PWD = randomPassword
                BAccount.AddAccount(accountProfileInfoToUpdate, HttpContext.Current.User.Identity.Name)
				accountProfileInfoToUpdate = myAccountUtility.GetAccountProfileInfo(accountProfileInfoToUpdate.ACCOUNT, True)
				accountProfileInfoToUpdate.PWD = Server.UrlEncode(randomPassword)
				accountProfileInfoToUpdate.ACCOUNT = Server.UrlEncode(myCryptoUtil.EncryptTripleDES(accountProfileInfoToUpdate.ACCOUNT))
                msgInfo = BMessages.GetMessage("NewAccount")
				NotifyUtility.SendMail(msgInfo, accountProfileInfoToUpdate)
				accountProfileInfoToUpdate.ACCOUNT = myCryptoUtil.DecryptTripleDES(Server.UrlDecode(accountProfileInfoToUpdate.ACCOUNT))
            End If
            Dim myBusinessUnitProfileInfoCollection As MBusinessUnitProfileInfoCollection
            BusinessUnitUtility.GetBusinessProfileCollection(myBusinessUnitProfileInfoCollection)
			Dim myBusinessUnitID As String = String.Empty
			For Each myBusinessUnitID In myBusinessUnitProfileInfoCollection.Keys
				myAccountUtility.RemoveCachedAccounts(myBusinessUnitID)
			Next
			UpdatedACCOUNT_SEQ_ID = accountProfileInfoToUpdate.ACCOUNT_SEQ_ID
			UpdateAccountName = accountProfileInfoToUpdate.ACCOUNT
		End If
    End Sub ' btnSave_Click

	Private Function PopulatePage(ByVal profile As MAccountProfileInfo) As MAccountProfileInfo
        litAccount.Text = profile.ACCOUNT
        txtAccount.Text = profile.ACCOUNT
        dropStatus.SelectedIndex = profile.SYSTEM_STATUS_ID
        litFailedAttempts.Text = profile.FailedAttempts
        txtFailedAttempts.Text = profile.FailedAttempts
        txtFirstName.Text = profile.First_Name
        txtLastName.Text = profile.Last_Name
        txtMiddleName.Text = profile.Middle_Name
        txtPreferedName.Text = profile.Prefered_Name
        txtEmail.Text = profile.EMAIL
        BaseHelper.SetDropSelection(dropTimezone, profile.TIME_ZONE)
        txtLocation.Text = profile.Location
        chkEnableNotifications.Checked = profile.EnableNotifications
    End Function

    Private Sub PopulateFromPage(ByRef profile As MAccountProfileInfo)
        profile.ACCOUNT = txtAccount.Text
        profile.SYSTEM_STATUS_ID = dropStatus.SelectedValue
        profile.First_Name = txtFirstName.Text
        profile.Last_Name = txtLastName.Text
        profile.Middle_Name = txtMiddleName.Text
        profile.Prefered_Name = txtPreferedName.Text
		profile.EMAIL = txtEmail.Text
        profile.TIME_ZONE = dropTimezone.SelectedValue
        profile.Location = txtLocation.Text
        profile.EnableNotifications = chkEnableNotifications.Checked
    End Sub

    Private Sub GetProfile(ByRef profile As MAccountProfileInfo)
		If Request.QueryString("Action").ToLower = "editprofile" Then
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			If Not Request.QueryString("Account") Is Nothing Then
				profile = myAccountUtility.GetAccountProfileInfo(Request.QueryString("Account"), True)
			Else
				profile = myAccountUtility.GetAccountProfileInfo(context.User.Identity.Name)
			End If
		Else
			profile = New MAccountProfileInfo
		End If
    End Sub
End Class