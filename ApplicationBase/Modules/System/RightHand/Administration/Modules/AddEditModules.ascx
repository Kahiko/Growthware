<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Register TagPrefix="uc1" TagName="ModulesGeneral" Src="ModulesGeneral.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GroupsControl" Src="../Groups/GroupsControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RolesControl" Src="../Roles/RolesControl.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.AddEditModules" Codebehind="AddEditModules.ascx.vb" %>
<table cellspacing="0" cellpadding="3" border="0">
	<tr>
		<td>
			<a id="anchorReturnToSelectStatesTop" href="?Action=SelectModules">Return to Select Modules</a>
		</td>
	</tr>
</table>
<BR>
<table cellspacing="0" cellpadding="0" width="100%" border="0">
	<tr>
		<td align="center">
			Module:&nbsp;<b><asp:Literal ID="litModuleName" Runat="server"></asp:Literal></b>
		</td>
	</tr>
	<tr align="center">
		<td>
			<CustomWebControls:TABSTRIP id="Tabstrip1" runat="server">
				<CustomWebControls:tab Text="General" PanelID="pnlGeneral" runat="server" />
				<CustomWebControls:tab Text="Roles" id="RolesTab" Roles="SysAdmin;Administrators;Super User" PanelID="pnlRoles" runat="server" />
				<CustomWebControls:tab Text="Groups" id="GroupsTab" Roles="SysAdmin;Administrators;Super User" PanelID="pnlGroups" runat="server" />
			</CustomWebControls:TABSTRIP>
		</td>
	</tr>
	<tr>
		<td id="bottomTabStrip" runat="server">&nbsp;</td>
	</tr>
	<tr>
		<td>
			<asp:panel class="tabPanel" id="pnlGeneral" Runat="Server"><uc1:ModulesGeneral id="ModulesGeneral" runat="server"></uc1:ModulesGeneral>
			</asp:panel>
			<asp:panel class="tabPanel" id="pnlRoles" Runat="Server"><uc1:RolesControl id="RolesControl" runat="server"></uc1:RolesControl>
			</asp:panel>
			<asp:panel class="tabPanel" id="pnlGroups" Runat="Server"><uc1:GroupsControl id="GroupsControl" runat="server"></uc1:GroupsControl>
			</asp:panel>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:button id="btnSave" runat="server" Text="Save"></asp:button>
		</td>
	</tr>
</table>
<table cellspacing="0" cellpadding="3" border="0">
	<tr>
		<td>
			<CustomWebControls:NAVTRAIL id="Navtrail2" runat="server">
				<CustomWebControls:NavTrailTab Text="Return to Select Modules" Action="SelectModules" runat="server" />
			</CustomWebControls:NAVTRAIL>
		</td>
	</tr>
</table>
