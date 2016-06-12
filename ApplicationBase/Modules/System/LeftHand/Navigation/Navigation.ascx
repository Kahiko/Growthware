<%@ Control Language="VB" AutoEventWireup="false" Inherits="ApplicationBase.Modules_System_LeftHand_Navigation_Navigation" Codebehind="Navigation.ascx.vb" %>
<table width="100%">
	<tr>
		<td nowrap="nowrap">
			<asp:Repeater ID="NavigationMenu" runat="Server">
				<ItemTemplate>
					<img id="NavIcon" runat="server" alt="" src="~/Images/NavIcon.gif">
					<a id="lnkMenu" runat="server" class="VerticalMenu" href="">HyperLink</a>
					<br>
				</ItemTemplate>
			</asp:Repeater>
		</td>
	</tr>
</table>