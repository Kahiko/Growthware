<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.AccountGroups" Codebehind="AccountGroups.ascx.vb" %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<table cellpadding=0 cellspacing=0 border=0 width="100%">
	<caption><b>Groups</b></caption>
	<tr>
		<td align=center>
			<CustomWebControls:ListPicker id="ctlGroups" AllItemsText="All Groups" SelectedItemsText="Selected Groups" Runat="Server" />
		</td>
	</tr>
</table>