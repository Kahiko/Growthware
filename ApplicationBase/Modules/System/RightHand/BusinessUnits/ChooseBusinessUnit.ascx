<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.ChooseBusinessUnit" Codebehind="ChooseBusinessUnit.ascx.vb" %>
<font style="FONT-WEIGHT: bold; COLOR: red">
	<asp:Literal ID="clientMSG" Runat="server"/>
</font>
<table cellSpacing="2" cellPadding="2" width="100%" border="0">
	<tr align="center">
		<td align="center">
			<asp:DropDownList ID="dropBusinessUnits" Runat="server"></asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td align="center" colSpan="2">
			<asp:button id="btnGo" Text="Go" Runat="server"></asp:button>
		</td>
	</tr>
</table>
