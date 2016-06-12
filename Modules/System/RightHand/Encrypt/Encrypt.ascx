<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Encrypt.ascx.vb" Inherits="BaseApplication.Encrypt" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table>
	<tr>
		<td align="right">Text to encrypt: </td>
		<td align="left"><asp:TextBox ID="txtToEncrypt" Runat="server"></asp:TextBox></td>
	</tr>
	<tr>
		<td align="right">Results: </td>
		<td><asp:Literal ID="EncryptedResults" Runat="server"></asp:Literal></td>
	</tr>
	<tr>
		<td align="center"><asp:Button ID="cmdEncrypt" Text="Encrypt" Runat="server"></asp:Button></td>
		<td align="center"><asp:Button ID="cmdDecrypt" Text="Decrypt" Runat="server"></asp:Button></td>
	</tr>
</table>
