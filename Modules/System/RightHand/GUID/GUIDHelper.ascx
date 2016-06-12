<%@ Control Language="vb" AutoEventWireup="false" Codebehind="GUIDHelper.ascx.vb" Inherits="BaseApplication.GUIDHelper" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="center">
			<asp:Literal id="litOutput" runat="server"></asp:Literal>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:Button id="btnGUID" runat="server" Text="GUID"></asp:Button>
		</td>
	</tr>
</table>