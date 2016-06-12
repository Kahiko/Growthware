<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.RandomNumbers" Codebehind="RandomNumbers.ascx.vb" %>
<table border="0" cellpadding="0" cellspacing="3" width="90%">
	<tr>
		<td>
			Max:<br><asp:TextBox ID="txtMaxNumber" Runat="server">255</asp:TextBox>
		</td>
		<td>
			Min:<br><asp:TextBox ID="txtMinNumber" Runat="server">0</asp:TextBox>
		</td>
		<td nowrap>
			How Many Numbers:<br><asp:TextBox ID="txtAmountOfNumbers" Runat="server">8</asp:TextBox>
		</td>
	</tr>
	<tr>
		<td align="center" colspan="3">
			<asp:Literal ID="litResults" Runat="server"></asp:Literal>
		</td>
	</tr>
	<tr>
		<td align="center" colspan="3">
			<asp:Button ID="btnSubmit" Text=" Go " Runat="server"></asp:Button>
		</td>
	</tr>
</table>
