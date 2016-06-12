<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AccountRoles.ascx.vb" Inherits="BaseApplication.AccountRoles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellpadding=0 cellspacing=0 border=0 width="100%">
	<caption><b>Roles</b></caption>
	<tr>
		<td align=center>
			<CustomWebControls:ListPicker id="ctlRoles" AllItemsText="All Roles" SelectedItemsText="Selected Roles" Runat="Server" />
		</td>
	</tr>
</table>
