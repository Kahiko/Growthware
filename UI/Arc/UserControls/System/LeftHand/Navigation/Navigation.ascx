<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Navigation.ascx.vb" Inherits="BaseApplication.Navigation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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