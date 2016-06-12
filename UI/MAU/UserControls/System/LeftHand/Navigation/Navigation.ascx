<%@ Control Language="vb" AutoEventWireup="false" Inherits="BaseApplication.Navigation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table border="0" cellpadding="0" cellpadding="3" width="100%">
	<tr>
		<td>
			<asp:repeater id="NavigationMenu" Runat="Server">
				<ItemTemplate>
					<a style="COLOR: black" id="lnkMenu" href="" runat="server">HyperLink</a>
					<br>
				</ItemTemplate>
			</asp:repeater>
		</td>
	</tr>
</table>