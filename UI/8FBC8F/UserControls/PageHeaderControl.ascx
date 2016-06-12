<%@ Register TagPrefix="uc1" TagName="ClientLogonInformation" Src="ClientLogonInformation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HierarchalMenu" Src="HierarchalMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ApplicationInformation" Src="ApplicationInformation.ascx" %>
<%@ Control Language="vb" EnableViewState="false" AutoEventWireup="false" Codebehind="PageHeaderControl.ascx.vb" Inherits="BaseApplication.PageHeaderControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LineMenu" Src="LineMenu.ascx" %>
<div id="roundedCorner">
	<table border="0" cellSpacing="0" cellPadding="0" width="100%">
		<tr bgcolor='<%=ClientChoicesState("SUB_HEAD_COLOR")%>'>
			<td rowspan="3">
				<asp:Image ImageUrl="../images/umms.gif" ID="AppImage" Runat="server"></asp:Image>
			</td>
			<td width="100%" align="center">
				&nbsp;<uc1:ApplicationInformation id="ApplicationInformation" runat="server"></uc1:ApplicationInformation>
			</td>
		</tr>
		<tr bgcolor='<%=ClientChoicesState("HEAD_COLOR")%>'>
			<td width="100%" align="center">
				<table>
					<tr>
						<td width="100%" align="center">
							<uc1:ClientLogonInformation id="ClientLogonInformation" runat="server"></uc1:ClientLogonInformation>
						</td>
						<td width="1px">&nbsp;</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr bgcolor='<%=ClientChoicesState("SUB_HEAD_COLOR")%>'>
			<td id="tdClientSecurityInformation" align="right" runat="server">
				<uc1:LineMenu id="LineMenu1" runat="server"></uc1:LineMenu>
			</td>
		</tr>
		<tr bgcolor='<%=ClientChoicesState("HEAD_COLOR")%>'>
			<td colspan="2" align="center">
				<uc1:HierarchalMenu MenuType="Horizontal" ClickToOpen="false" id="HierarchalMenu" runat="server"></uc1:HierarchalMenu>&nbsp;
			</td>
		</tr>
	</table>
</div>
