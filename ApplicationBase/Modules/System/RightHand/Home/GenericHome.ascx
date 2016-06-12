<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.GenericHome" Codebehind="GenericHome.ascx.vb" %>
<%@ Register TagPrefix="uc1" TagName="RightHandModuleHeaderFooter" Src="~/UI/Default/UserControls/RightHand/RightHandModuleHeaderFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RightHandModuleHeader" Src="~/UI/Default/UserControls/RightHand/RightHandModuleHeader.ascx" %>
<%@OutputCache Duration="720" Shared="true" VaryByParam="none" %>
<table border="0" width="100%" cellpadding="0" cellspacing="0" style="FONT: 8pt verdana, arial">
	<tr>
		<td align="left" valign="top" style="PADDING-RIGHT:0px; PADDING-LEFT:0px; PADDING-BOTTOM:0px; PADDING-TOP:0px">
			<asp:Image ID="SideImage" ImageUrl='<%=ResolveUrl( "~/UI/Default/images/sidebar_blue.gif" )%>' Runat="server"></asp:Image>
		</td>
		<td align="left" valign="top" width="100%" style="PADDING-RIGHT:15px; PADDING-LEFT:15px; PADDING-BOTTOM:15px; PADDING-TOP:15px">
			<b>Welcome to <asp:Label id=lblAppName runat="server">lblAppName</asp:Label></b>
		</td>
	</tr>
</table>