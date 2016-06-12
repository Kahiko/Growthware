Imports DALModel.Base.Group
Imports BLL.Base.SQLServer
Imports DALModel.Special.ClientChoices
Imports DALModel.Base.Accounts.Security


Public Class AddEditGroups
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents litGroupName As System.Web.UI.WebControls.Literal
    Protected WithEvents Tabstrip1 As Common.CustomWebControls.TabStrip
    Protected WithEvents pnlGeneral As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlSecurity As System.Web.UI.WebControls.Panel
    Protected WithEvents btnSave As System.Web.UI.WebControls.Button
    Protected WithEvents bottomTabStrip As System.Web.UI.HtmlControls.HtmlTableCell

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Protected WithEvents GroupsGeneral As GroupsGeneral
    Protected WithEvents GroupsRoles As GroupRoles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim myAccountUtility As New AccountUtility(HttpContext.Current)
        Dim accountSecurityInfo As MAccountSecurityInfo = myAccountUtility.GetAccountSecurityInfo(accountSecurityInfo)
        Dim groupInfoTobesaved As New MGroupInfo
        If Not IsPostBack Then
            ' get the client security information
            If Not accountSecurityInfo.MayEdit And Not accountSecurityInfo.MayAdd Then btnSave.Visible = False
            bottomTabStrip.BgColor = ClientChoicesState(MClientChoices.HeadColor)
		End If
		If Request.QueryString("Action").Trim.ToLower.IndexOf("edit") > -1 Then
			If Not Request.QueryString("groupId") Is Nothing Then
				groupInfoTobesaved = BGroups.GetGroupInfo(Request.QueryString("groupId"))
			End If
		End If
		If groupInfoTobesaved.GroupName.Trim.Length > 0 Then
			litGroupName.Text = groupInfoTobesaved.GroupName
		Else
			litGroupName.Text = "New"
		End If

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Page.Validate()
        If Page.IsValid Then
            Dim mygroupInfo As MGroupInfo
            ' Update the general information
            GroupsGeneral.btnSave_Click(sender, e)
            Dim mySeq_id As Integer = GroupsGeneral.UpdatedGroupSeqId
            ' Update Roles
            If GroupsRoles.RolesChanged OrElse Request.QueryString("Action").ToLower = "addgroupinfo" Then
                Try
                    BGroups.UpdateRoles(mySeq_id, ClientChoicesState(MClientChoices.BusinessUnitID), GroupsRoles.Roles)
                Catch ex As Exception
                    Throw New ApplicationException("Error Writing to the DATABASE", ex)
                End Try
            End If
            NavControler.NavTo("editgroupinfo&groupid=" & GroupsGeneral.UpdatedGroupSeqId)
        End If
    End Sub

End Class
