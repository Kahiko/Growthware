<%@ Register TagPrefix="uc1" TagName="GroupsGeneral" Src="GroupsGeneral.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GroupsRoles" Src="GroupRoles.ascx" %>
<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AddEditGroups.ascx.vb" Inherits="BaseApplication.AddEditGroups" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script>
	function doWindowOnLoad(){
		RoundedTop("ul#tabStrip li","white",SubheadColor);
	}
</script>
<table cellSpacing=0 cellPadding=3 border=0>
	<tr>
		<td>
			<CustomWebControls:NAVTRAIL id=NavTrail1 runat="server">
				<NavTrailTab Text="Return to Select Select Groups" Action="selectgroupinfo" />
			</CustomWebControls:NAVTRAIL>
		</td>
	</tr>
</table>
<BR>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td align="center">
			Group:&nbsp;<b><asp:Literal ID="litGroupName" Runat="server"></asp:Literal></b>
		</td>
	</tr>
	<tr align="center">
		<td>
			<CustomWebControls:TABSTRIP id="Tabstrip1" runat="server">
				<tab Text="General" PanelID="RightHandModulesLoader__ctl0_pnlGeneral" />
				<tab Text="Roles" Roles="SysAdmin;Administrators;Super User" PanelID="RightHandModulesLoader__ctl0_pnlSecurity" />
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
				<uc1:GroupsGeneral id="GroupsGeneral" runat="server"></uc1:GroupsGeneral>
			</asp:panel>
			<asp:panel cssclass="tabPanel" id="pnlSecurity" Runat="Server">
				<uc1:GroupsRoles id="GroupsRoles" runat="server"></uc1:GroupsRoles>
			</asp:panel>
		</td>
	</tr>
	<tr>
		<td align="center"><asp:button id="btnSave" runat="server" Text="Save"></asp:button></td>
	</tr>
</table>
<br>
<table cellSpacing=0 cellPadding=3 border=0>
	<tr>
		<td>
			<CustomWebControls:NAVTRAIL id="Navtrail2" runat="server">
				<NavTrailTab Text="Return to Select Select Groups" Action="selectgroupinfo" />
			</CustomWebControls:NAVTRAIL>
		</td>
	</tr>
</table>