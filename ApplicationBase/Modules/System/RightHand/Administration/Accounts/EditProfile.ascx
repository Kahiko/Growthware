<%@ Register TagPrefix="uc1" TagName="AccountsGeneral" Src="AccountsGeneral.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.EditProfile" Codebehind="EditProfile.ascx.vb" %>
<%@ Register TagPrefix="uc1" TagName="AccountRoles" Src="AccountRoles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AccountGroups" Src="AccountGroups.ascx" %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td align="center">
			Account:&nbsp;<b><asp:Literal ID="litAccountName" Runat="server"></asp:Literal></b>
		</td>
	</tr>
	<tr align="center">
		<td>
			<CustomWebControls:TABSTRIP id="Tabstrip1" runat="server">
				<CustomWebControls:Tab ID="Tab1" Text="General" PanelID="pnlGeneral" runat="server"/>
				<CustomWebControls:Tab ID="RolesTab" Text="Roles" PanelID="pnlRoles" Roles="SysAdmin;Administrators;Super User" runat="server"/>
				<CustomWebControls:Tab ID="GroupsTab" Text="Groups" PanelID="pnlGroups" Roles="SysAdmin;Administrators;Super User" runat="server"/>
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
			<asp:Panel ID="pnlGeneral" runat="Server" CssClass="tabPanel">
				<uc1:AccountsGeneral ID="AccountsGeneral" runat="server" />
			</asp:Panel>
			<asp:Panel ID="pnlRoles" runat="Server" CssClass="tabPanel">
				<uc1:AccountRoles ID="AccountRoles" runat="server" />
			</asp:Panel>
			<asp:Panel ID="pnlGroups" runat="Server" CssClass="tabPanel">
				<uc1:AccountGroups ID="AccountGroups" runat="server" />
			</asp:Panel>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:button id="btnSave" runat="server" Text="Save"></asp:button>
		</td>
	</tr>
</table>
