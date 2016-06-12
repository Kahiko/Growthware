<%@ Control Language="vb" AutoEventWireup="false" Codebehind="EditStates.ascx.vb" Inherits="BaseApplication.EditStates" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="StatesGeneral" Src="StatesGeneral.ascx" %>
<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<script>
	function doWindowOnLoad(){
		RoundedTop("ul#tabStrip li","white",SubheadColor);
	}
</script>
<table cellpadding="3" cellspacing="0" border="0" class="body">
	<tr>
		<td>
			<a id="anchorReturnToSelectStatesTop" href="?Action=SelectStates">Return to Select
				State
			</a>
		</td>
	</tr>
</table>
<BR>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td align="center">
			State:&nbsp;<b><asp:Literal ID="litState" Runat="server"></asp:Literal></b>
		</td>
	</tr>
	<tr align="center">
		<td>
			<CustomWebControls:TABSTRIP id="Tabstrip1" runat="server">
				<tab Text="General" PanelID="RightHandModulesLoader__ctl0_pnlGeneral" />
			</CustomWebControls:TABSTRIP>
		</td>
	</tr>
	<tr>
		<td id="bottomTabStrip" runat="server">&nbsp;</td>
	</tr>
	<tr>
		<td>
			<asp:panel class="tabPanel" id="pnlGeneral" Runat="Server"><uc1:StatesGeneral id=StatesGeneral runat="server"></uc1:StatesGeneral>
			</asp:panel>
		</td>
	</tr>
	<tr>
		<td align="center"><asp:button id="btnSave" runat="server" Text="Save"></asp:button></td>
	</tr>
</table>
<table cellpadding="3" cellspacing="0" border="0" class="body">
	<tr>
		<td>
			<a id="anchorReturnToSelectStatesBottom" href="?Action=SelectStates">Return to
				Select State
			</a>
		</td>
	</tr>
</table>