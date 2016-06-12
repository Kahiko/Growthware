<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AccountGroups.ascx.vb" Inherits="BaseApplication.AccountGroups" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<table cellpadding=0 cellspacing=0 border=0 width="100%">
	<caption><b>Groups</b></caption>
	<tr>
		<td align=center>
			<CustomWebControls:ListPicker id="ctlGroups" AllItemsText="All Groups" SelectedItemsText="Selected Groups" Runat="Server" />
		</td>
	</tr>
</table>