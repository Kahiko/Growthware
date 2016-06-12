<%@ Control AutoEventWireup="false" Inherits="ApplicationBase.Modules_System_LeftHand_Navigation_Navigation" Language="vb" Codebehind="~/Modules/System/LeftHand/Navigation/Navigation.ascx.vb" %>
<table width="100%">
	<tr>
		<td class="sideNav1">
			<asp:repeater id="NavigationMenu" Runat="Server">
				<ItemTemplate>
					<a id="lnkMenu" href="" runat="server">HyperLink</a>
					<br>
				</ItemTemplate>
			</asp:repeater>
		</td>
	</tr>
</table>