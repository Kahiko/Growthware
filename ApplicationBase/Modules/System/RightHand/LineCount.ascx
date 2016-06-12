<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.LineCount" Codebehind="LineCount.ascx.vb" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="right" width="50%">
			<asp:TextBox ID="txtDirectoryName" Width="500" Runat="server"></asp:TextBox>
		</td>
		<td align="left">
			<asp:Button ID="btnSubmit" Text="Count" Runat="server"></asp:Button>
		</td>
		<td>
			<asp:Literal ID="litTotalLines" Runat="server"></asp:Literal>
		</td>
	</tr>
</table>
<asp:Literal ID="litLineCount" Runat="server"></asp:Literal>
