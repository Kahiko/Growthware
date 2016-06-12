Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.Security
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.BusinessUnits
Imports ApplicationBase.Model.Messages
Imports ApplicationBase.Model.Special.ClientChoices

Partial Class AccountsGeneral
	Inherits ClientChoices.ClientChoicesUserControl

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
			Dim accountProfileInfoToUpdate As New MAccountProfileInfo
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
			Dim accountProfileInfoToUpdate As New MAccountProfileInfo
			Dim myaccountProfileInfo As New MAccountProfileInfo
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			myaccountProfileInfo = myAccountUtility.GetAccountProfileInfo(Context.User.Identity.Name)
			GetProfile(accountProfileInfoToUpdate)
			PopulateFromPage(accountProfileInfoToUpdate)
			accountProfileInfoToUpdate.UPDATED_BY = myaccountProfileInfo.ACCOUNT_SEQ_ID
			accountProfileInfoToUpdate.UPDATED_DATE = Now
			If Not Request.QueryString("Action").ToLower = "addaccount" Then
				BAccount.UpdateProfile(accountProfileInfoToUpdate)
			Else
				Dim msgInfo As MMessageInfo
				Dim myCryptoUtil As New CryptoUtil
                Dim randomPassword As String = BaseHelperOld.GetRandomPassword
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
			Dim myBusinessUnitProfileInfoCollection As New MBusinessUnitProfileInfoCollection
			BusinessUnitUtility.GetBusinessProfileCollection(myBusinessUnitProfileInfoCollection)
			Dim myBusinessUnitID As String = String.Empty
			For Each myBusinessUnitID In myBusinessUnitProfileInfoCollection.Keys
				myAccountUtility.RemoveCachedAccounts(myBusinessUnitID)
			Next
			UpdatedACCOUNT_SEQ_ID = accountProfileInfoToUpdate.ACCOUNT_SEQ_ID
			UpdateAccountName = accountProfileInfoToUpdate.ACCOUNT
		End If
	End Sub	' btnSave_Click

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
        BaseHelperOld.SetDropSelection(dropTimezone, profile.TIME_ZONE)
		txtLocation.Text = profile.Location
		chkEnableNotifications.Checked = profile.EnableNotifications
		Return profile
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
                profile = myAccountUtility.GetAccountProfileInfo(Context.User.Identity.Name)
            End If
        Else
            profile = New MAccountProfileInfo
        End If
    End Sub
End Class