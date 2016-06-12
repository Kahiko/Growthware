<%@ Control Language="vb" AutoEventWireup="false" Codebehind="RightHandModuleHeader.ascx.vb" Inherits="BaseApplication.RightHandModuleHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table width="100%" cellpadding="0" cellspacing="0">
	<tr id="trRightHandModuleHeader" runat="server">
		<td style="BORDER-RIGHT:black 1px solid; BORDER-TOP:black 1px solid; BORDER-LEFT:black 1px solid; BORDER-BOTTOM:black 1px solid">
			<table width="100%" border="1" cellpadding="0" cellspacing="2" bgcolor='<%= ClientChoicesState("HEAD_COLOR")%>'>
				<tr>
					<td width="100%" align="center" nowrap><font face='Arial' color='white'><b><%=Title%></b></font></td>
					<td id="tdEdit" runat="server">
						<a href="" id="anchorEditPage" style="BORDER-RIGHT:0px; BORDER-TOP:0px; BORDER-LEFT:0px; BORDER-BOTTOM:0px" runat="server">
							<img id="imgEdit" src="edit.gif" border="0" runat="server">
						</a>
					</td>
					<td id="tdClose" runat="server">
						<a href="" id="anchorClose" style="BORDER-RIGHT:0px; BORDER-TOP:0px; BORDER-LEFT:0px; BORDER-BOTTOM:0px" runat="server">
							<img id="imgClose" src="x.gif" border="0" runat="server">
						</a>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td bgcolor='<%= ClientChoicesState("LEFT_COLOR")%>' colspan="3" style="BORDER-RIGHT:black 1px solid; BORDER-TOP:0px; BORDER-LEFT:black 1px solid; BORDER-BOTTOM:black 1px solid">