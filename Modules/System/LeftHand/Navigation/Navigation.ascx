<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Navigation.ascx.vb" Inherits="BaseApplication.Navigation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table width="100%">
	<tr>
		<td nowrap>
			<asp:repeater id="NavigationMenu" Runat="Server">
				<ItemTemplate>
					<img id="NavIcon" src="~/Images/NavIcon.gif" alt="" runat="server">
					<a class="VerticalMenu" id="lnkMenu" href="" runat="server">HyperLink</a>
					<br>
				</ItemTemplate>
			</asp:repeater>
		</td>
	</tr>
</table>