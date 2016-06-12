<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AddEditDropDown.ascx.vb" Inherits="BaseApplication.AddEditDropDown" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
				<tab Text="Security" PanelID="RightHandModulesLoader__ctl0_pnlSecurity" />
				<tab Text="General" Roles="SysAdmin;Super User" PanelID="RightHandModulesLoader__ctl0_pnlGeneral" />
			</CustomWebControls:TABSTRIP>
		</td>
	</tr>
	<tr>
		<td id=bottomTabStrip runat="server">&nbsp;</td>
	</tr>
	<tr>
		<td>
			<asp:panel class=tabPanel id=pnlSecurity Runat="Server"><uc1:RolesControl id="RolesControl" runat="server"></uc1:RolesControl>
			</asp:panel>
			<asp:panel class=tabPanel id=pnlGeneral Runat="Server"><uc1:DropDownGeneral id="DropDownGeneral" runat="server"></uc1:DropDownGeneral>
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
