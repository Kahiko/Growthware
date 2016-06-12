<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.GroupRoles" Codebehind="GroupRoles.ascx.vb" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<caption>
		<b>Security</b></caption>
	<tr>
		<td align="center">
			<CustomWebControls:ListPicker id="ctlRoles" AllItemsText="All Roles" SelectedItemsText="Selected Roles" Runat="Server" />
		</td>
	</tr>
</table>
