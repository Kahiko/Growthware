<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<%@ Register TagPrefix="uc1" TagName="ModulesGeneral" Src="ModulesGeneral.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GroupsControl" Src="../Groups/GroupsControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RolesControl" Src="../Roles/RolesControl.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AddEditModules.ascx.vb" Inherits="BaseApplication.AddEditModules" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script>
	function doWindowOnLoad(){
		RoundedTop("ul#tabStrip li","white",SubheadColor);
	}
</script>
<table cellSpacing=0 cellPadding=3 border=0>
	<tr>
		<td>
			<CustomWebControls:NAVTRAIL id=NavTrail1 runat="server">
				<NavTrailTab Text="Return to Select Modules" Action="SelectModules" />
			</CustomWebControls:NAVTRAIL>
		</td>
	</tr>
</table>
<BR>
<table cellSpacing=0 cellPadding=0 width="100%" border=0>
	<tr>
		<td align="center">
			Module:&nbsp;<b><asp:Literal ID="litModuleName" Runat="server"></asp:Literal></b>
		</td>
	</tr>
	<tr align=center>
		<td>
			<CustomWebControls:TABSTRIP id=Tabstrip1 runat="server">
				<tab Text="General" PanelID="RightHandModulesLoader__ctl0_pnlGeneral" />
				<tab Text="Roles" id="RolesTab" Roles="SysAdmin;Administrators;Super User" PanelID="RightHandModulesLoader__ctl0_pnlRoles" />
				<tab Text="Groups" id="GroupsTab" Roles="SysAdmin;Administrators;Super User" PanelID="RightHandModulesLoader__ctl0_pnlGroups" />
			</CustomWebControls:TABSTRIP>
		</td>
	</tr>
	<tr>
		<td id=bottomTabStrip runat="server">&nbsp;</td>
	</tr>
	<tr>
		<td>
			<asp:panel class=tabPanel id=pnlGeneral Runat="Server"><uc1:ModulesGeneral id="ModulesGeneral" runat="server"></uc1:ModulesGeneral>
			</asp:panel>
			<asp:panel class=tabPanel id=pnlRoles Runat="Server"><uc1:RolesControl id="RolesControl" runat="server"></uc1:RolesControl>
			</asp:panel>
			<asp:panel class=tabPanel id="pnlGroups" Runat="Server"><uc1:GroupsControl id="GroupsControl" runat="server"></uc1:GroupsControl>
			</asp:panel>
		</td>
	</tr>
	<tr>
		<td align=center>
			<asp:button id=btnSave runat="server" Text="Save"></asp:button>
		</td>
	</tr>
</table>
<table cellSpacing=0 cellPadding=3 border=0>
	<tr>
		<td>
			<CustomWebControls:NAVTRAIL id=Navtrail2 runat="server">
				<NavTrailTab Text="Return to Select Modules" Action="SelectModules" />
			</CustomWebControls:NAVTRAIL>
		</td>
	</tr>
</table>
