<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.AddEditDropDown" Codebehind="AddEditDropDown.ascx.vb" %>
<%@ Register TagPrefix="uc1" TagName="RolesControl" Src="../Roles/RolesControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DropDownGeneral" Src="DropDownGeneral.ascx" %>
<script>
	function doWindowOnLoad(){
		RoundedTop("ul#tabStrip li","white",SubheadColor);
	}
</script>
<table cellSpacing=0 cellPadding=3 border=0 id=Table1>
	<tr>
		<td>
			<CustomWebControls:NAVTRAIL id=NavTrail1 runat="server">
				<NavTrailTab Text="Return to Select Drop Downs" Action="SelectDropDowns" />
			</CustomWebControls:NAVTRAIL>
		</td>
	</tr>
</table>
<BR>
<table cellSpacing=0 cellPadding=0 width="100%" border=0 id=Table2>
	<tr align=center>
		<td>
			<CustomWebControls:TABSTRIP id=Tabstrip1 runat="server">
				<CustomWebControls:tab Text="General" Roles="SysAdmin;Super User" PanelID="pnlGeneral" runat="server" />
				<CustomWebControls:tab Text="Security" PanelID="pnlSecurity" runat="server" />
			</CustomWebControls:TABSTRIP>
		</td>
	</tr>
	<tr>
		<td id=bottomTabStrip runat="server">&nbsp;</td>
	</tr>
	<tr>
		<td>
			<asp:Panel ID="pnlGeneral" runat="Server" class="tabPanel">
				<uc1:DropDownGeneral ID="DropDownGeneral" runat="server" />
			</asp:Panel>
			<asp:panel CssClass=tabPanel id=pnlSecurity Runat="Server">
				<uc1:RolesControl id="RolesControl" runat="server"></uc1:RolesControl>
			</asp:panel>
		</td>
	</tr>
	<tr>
		<td align=center>
			<asp:button id=btnSave runat="server" Text="Save"></asp:button>
		</td>
	</tr>
</table>
<table cellSpacing=0 cellPadding=3 border=0 id=Table3>
	<tr>
		<td>
			<CustomWebControls:NAVTRAIL id=Navtrail2 runat="server">
				<NavTrailTab Text="Return to Select Drop Downs" Action="SelectDropDowns" />
			</CustomWebControls:NAVTRAIL>
		</td>
	</tr>
</table>
