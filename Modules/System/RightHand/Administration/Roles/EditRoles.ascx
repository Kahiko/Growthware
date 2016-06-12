<%@ Control Language="vb" AutoEventWireup="false" Codebehind="EditRoles.ascx.vb" Inherits="BaseApplication.EditRoles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<table cellSpacing=0 cellPadding=3 border=0>
	<tr>
		<td>
			<CustomWebControls:NAVTRAIL id=NavTrail1 runat="server">
				<NavTrailTab Text="Return to Add Edit Roles" Action="addeditroles" />
			</CustomWebControls:NAVTRAIL>
		</td>
	</tr>
</table>
<BR>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="center">
			Role:&nbsp;<b><asp:Literal ID="litRole" Runat="server"></asp:Literal></b>
		</td>
	</tr>
	<tr>
		<td align="center">
			<CustomWebControls:LISTPICKER id=ctlMembers runat="server" SelectedItemsText="Selected Members" AllItemsText="All Members"></CustomWebControls:LISTPICKER>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:Button ID="btnSave" Text="Save" Runat="server"/>
		</td>
	</tr>
</table>
<table cellSpacing=0 cellPadding=3 border=0>
	<tr>
		<td>
			<CustomWebControls:NAVTRAIL id="Navtrail2" runat="server">
				<NavTrailTab Text="Return to Add Edit Roles" Action="addeditroles" />
			</CustomWebControls:NAVTRAIL>
		</td>
	</tr>
</table>
