<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.SetForceHttps" Codebehind="SetForceHttps.ascx.vb" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr align="center">
		<td align="right">
			Level:
		</td>
		<td align="left">
			<asp:DropDownList ID="dropForceHTTPS" Runat="server">
				<asp:ListItem Value="0">False</asp:ListItem>
				<asp:ListItem Value="1">True</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="left">
			<asp:Button ID="btnGo" Text=" GO " Runat="server"></asp:Button>
		</td>
	</tr>
</table>
