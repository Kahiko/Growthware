<%@ Control AutoEventWireup="false" Codebehind="~/Modules/System/LeftHand/Navigation/Navigation.ascx.vb" Inherits="ApplicationBase.Modules_System_LeftHand_Navigation_Navigation" Language="vb" %>
<table width="100%">
	<tr>
		<td class="sideNav1" nowrap>
			<asp:repeater id="NavigationMenu" Runat="Server">
				<ItemTemplate>
					<a id="lnkMenu" href="" runat="server">HyperLink</a>
					<br>
				</ItemTemplate>
			</asp:repeater>
		</td>
	</tr>
</table>