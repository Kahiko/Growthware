<%@ Register TagPrefix="uc1" TagName="AccountsGeneral" Src="AccountsGeneral.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="EditProfile.ascx.vb" Inherits="BaseApplication.EditProfile" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="AccountRoles" Src="AccountRoles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AccountGroups" Src="AccountGroups.ascx" %>
<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<script>
	function doWindowOnLoad(){
		RoundedTop("ul#tabStrip li","white",SubheadColor);
	}
</script>
<BR>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td align="center">
			Account:&nbsp;<b><asp:Literal ID="litAccountName" Runat="server"></asp:Literal></b>
		</td>
	</tr>
	<tr align="center">
		<td>
			<CustomWebControls:TABSTRIP id="Tabstrip1" runat="server">
				<tab Text="General" PanelID="RightHandModulesLoader__ctl0_pnlGeneral" />
				<tab Text="Roles" id="RolesTab" Roles="SysAdmin;Administrators;Super User" PanelID="RightHandModulesLoader__ctl0_pnlRoles" />
				<tab Text="Groups" id="GroupsTab" Roles="SysAdmin;Administrators;Super User" PanelID="RightHandModulesLoader__ctl0_pnlGroups" />
			</CustomWebControls:TABSTRIP>
		</td>
	</tr>
	<tr>
		<td id="bottomTabStrip" runat="server">&nbsp;</td>
	</tr>
	<tr>
		<td>
		</td>
	</tr>
	<tr>
		<td>
			<asp:panel class="tabPanel" id="pnlGeneral" Runat="Server"><uc1:AccountsGeneral id="AccountsGeneral" runat="server"></uc1:AccountsGeneral>
			</asp:panel>
			<asp:panel class="tabPanel" id="pnlRoles" Runat="Server"><uc1:AccountRoles id="AccountRoles" runat="server"></uc1:AccountRoles>
			</asp:panel>
			<asp:panel class="tabPanel" id="pnlGroups" Runat="Server"><uc1:AccountGroups id="AccountGroups" runat="server"></uc1:AccountGroups>
			</asp:panel>
		</td>
	</tr>
	<tr>
		<td align="center"><asp:button id="btnSave" runat="server" Text="Save"></asp:button></td>
	</tr>
</table>
