<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.HierarchalMenu" Codebehind="HierarchalMenu.ascx.vb" %>
<table id="Table1" border="0" cellpadding="0" cellspacing="3" width="100%">
	<tr>
		<td id="MenuDetail" runat="server">
		    <asp:Menu ID="myMenu" runat="server">
                <StaticMenuItemStyle CssClass="HierarchalMenuItem" />
                <DynamicHoverStyle CssClass="toolbar" />
                <DynamicMenuStyle CssClass="toolbar" />
                <DynamicMenuItemStyle CssClass="HierarchalMenuItem" />
            </asp:Menu>
		</td>
	</tr>
</table>
