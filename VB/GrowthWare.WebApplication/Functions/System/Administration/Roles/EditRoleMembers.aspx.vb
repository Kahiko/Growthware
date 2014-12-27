Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.BasePages
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Profiles

Public Class EditRoleMembers
    Inherits ClientChoicesPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myProfile As MRoleProfile = New MRoleProfile()
        Dim accountSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.CurrentProfile(), AccountUtility.CurrentProfile())
        'if not accountSecurityInfo.MayEdit then btnSave.Visible = false
        'txtEditID.Text = HttpContext.Current.Session(AppConstants.ROLE_DATA_KEY_FIELD).ToString()
        txtEditID.Text = Request.QueryString(GWWebHelper.RoleDataKeyField).ToString()
        'HttpContext.Current.Session.Remove(AppConstants.ROLE_DATA_KEY_FIELD)
        myProfile.Id = Integer.Parse(txtEditID.Text)
        myProfile = RoleUtility.GetProfile(myProfile.Id)
        litRole.Text = myProfile.Name
        myProfile.SecurityEntityId = Integer.Parse(ClientChoicesState(MClientChoices.SecurityEntityId).ToString())
        ctlMembers.DataSource = RoleUtility.GetAccountsNotInRole(myProfile).ToArray(Type.GetType("System.String"))
        ctlMembers.SelectedItems = RoleUtility.GetAccountsInRole(myProfile).ToArray(Type.GetType("System.String"))
        ctlMembers.DataBind()
    End Sub

End Class