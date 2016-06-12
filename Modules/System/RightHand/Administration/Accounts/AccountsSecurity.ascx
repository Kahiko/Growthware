<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AccountsSecurity.ascx.vb" Inherits="BaseApplication.AccountsSecurity" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<table cellpadding=0 cellspacing=0 border=0 width="100%">
	<caption><b>Security</b></caption>
	<tr>
		<td align=center>
			<CustomWebControls:ListPicker id="ctlRoles" AllItemsText="All Roles" SelectedItemsText="Selected Roles" Runat="Server" />
		</td>
	</tr>
</table>
