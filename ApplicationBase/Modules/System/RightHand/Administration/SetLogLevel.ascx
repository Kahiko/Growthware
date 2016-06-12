<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.SetLogLevel" Codebehind="SetLogLevel.ascx.vb" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr align="center">
		<td align="right">
			Level:
		</td>
		<td align="left">
			<asp:DropDownList ID="dropLogLevel" Runat="server">
				<asp:ListItem Value="0">Debug</asp:ListItem>
				<asp:ListItem Value="1">Information</asp:ListItem>
				<asp:ListItem Value="2">Warning</asp:ListItem>
				<asp:ListItem Value="3">Error</asp:ListItem>
				<asp:ListItem Value="4">Fatal</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="left">
			<asp:Button ID="btnSetLogLevel" Text="Set Level" Runat="server"></asp:Button>
		</td>
	</tr>
</table>