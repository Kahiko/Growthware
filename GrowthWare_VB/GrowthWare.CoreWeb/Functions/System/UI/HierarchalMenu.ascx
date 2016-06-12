<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="HierarchalMenu.ascx.vb" Inherits="GrowthWare.CoreWeb.HierarchalMenu" %>
<asp:Panel ID="pnlHierarchalMenu" runat="server">
    panel with Heirarchal menu data
	<asp:Menu ID="myMenu" runat="server">
		<DataBindings>
		  <asp:MenuItemBinding DataMember="MenuItem" NavigateUrlField="NavigateUrl" TextField="Text" ToolTipField="ToolTip"/>
		</DataBindings>
		<StaticMenuItemStyle CssClass="HierarchalMenuItem" />
		<DynamicHoverStyle CssClass="toolbar" />
		<DynamicMenuStyle CssClass="toolbar" />
		<DynamicMenuItemStyle CssClass="HierarchalMenuItem" />
	</asp:Menu>
</asp:Panel>
<asp:XmlDataSource ID="xmlDataSource" EnableCaching="false" TransformFile="Set By Code" XPath="MenuItems/MenuItem" runat="server"/>