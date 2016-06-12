<%@ Register TagPrefix="uc1" TagName="GroupsGeneral" Src="GroupsGeneral.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GroupsRoles" Src="GroupRoles.ascx" %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.AddEditGroups" Codebehind="AddEditGroups.ascx.vb" %>
<table cellspacing="0" cellpadding="3" border="0">
	<tr>
		<td>
			<CustomWebControls:NAVTRAIL id="NavTrail1" runat="server">
				<CustomWebControls:NavTrailTab text="Return to Select Select Groups" action="selectgroupinfo" runat="server" />
			</CustomWebControls:NAVTRAIL>
		</td>
	</tr>
</table>
<br />
<table cellspacing="0" cellpadding="0" width="100%" border="0">
	<tr>
		<td align="center">
			Group:&nbsp;<b><asp:Literal ID="litGroupName" Runat="server"></asp:Literal></b>
		</td>
	</tr>
	<tr align="center">
		<td>
			<CustomWebControls:TABSTRIP id="Tabstrip1" runat="server">
				<CustomWebControls:tab Text="General" PanelID="pnlGeneral" runat="server" />
				<CustomWebControls:tab Text="Roles" Roles="SysAdmin;Administrators;Super User" PanelID="pnlSecurity" runat="server" />
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
			<asp:panel cssclass="tabPanel" id="pnlGeneral" Runat="Server">
				<uc1:GroupsGeneral id="myGroupsGeneral" runat="server"></uc1:GroupsGeneral>
			</asp:panel>
			<asp:panel cssclass="tabPanel" id="pnlSecurity" Runat="Server">
				<uc1:GroupsRoles id="myGroupsRoles" runat="server"></uc1:GroupsRoles>
			</asp:panel>
		</td>
	</tr>
	<tr>
		<td align="center"><asp:button id="btnSave" runat="server" Text="Save"></asp:button></td>
	</tr>
</table>
<br />
<table cellspacing="0" cellpadding="3" border="0">
	<tr>
		<td>
			<CustomWebControls:NAVTRAIL id="Navtrail2" runat="server">
				<CustomWebControls:NavTrailTab Text="Return to Select Select Groups" Action="selectgroupinfo" runat="server" />
			</CustomWebControls:NAVTRAIL>
		</td>
	</tr>
</table>