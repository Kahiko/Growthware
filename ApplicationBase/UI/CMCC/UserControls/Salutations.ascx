<%@ Control AutoEventWireup="false" Codebehind="Salutations.ascx.vb" Inherits="ApplicationBase.Salutations" Language="vb" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td>
			<font style="FONT-SIZE: 14pt">
				<asp:Literal ID="litAppDisplayedName" Runat="server"></asp:Literal>
			</font>
		</td>
	</tr>
	<tr>
		<td>
			<font style="FONT-SIZE: 12pt">
				<asp:Literal ID="litSalutation" Runat="server"></asp:Literal>&nbsp;<asp:Literal ID="litSelectedLocation" Runat="server"></asp:Literal>
			</font>
		</td>
	</tr>
</table>
