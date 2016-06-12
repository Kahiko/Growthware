<%@ Control AutoEventWireup="false" Inherits="ApplicationBase.LeftHandModuleHeader" Language="VB" Codebehind="LeftHandModuleHeader.ascx.vb" %>
<table width="205" cellpadding="0px" cellspacing="0px">
		<tr id="trLeftHandModuleHeader" runat="server">
			<td style="BORDER-RIGHT:black 1px solid; BORDER-TOP:black 1px solid; BORDER-LEFT:black 1px solid; BORDER-BOTTOM:black 1px solid">
				<table width="100%" border="1" cellpadding="0px" cellspacing="2px" bgcolor="<%= ClientChoicesState("HEAD_COLOR")%>">
					<tr>
						<td width="100%" nowrap><font face='Arial' color='white'><b><%=Title%></b></font></td>
						<td id="tdEdit" runat=server>
							<a href="" id="anchorEditPage" style="border:0px"  runat="server">
								<img src="<%= ConfigurationSettings.AppSettings("ImagePath") %>edit.gif" border="0">
							</a>
						</td>
						<td id="tdClose" runat=server>
							<a href="" id="anchorClose" style="border:0px" runat=server>
								<img src="<%= ConfigurationSettings.AppSettings("ImagePath") %>x.gif" border="0px">
							</a>
						</td>
					</tr>
				</table>
			
			</td>
		</tr>
		<tr>
			<td bgcolor="<%= ClientChoicesState("LEFT_COLOR")%>" colspan="3" style="BORDER-RIGHT:black 1px solid; BORDER-TOP:black 1px solid; BORDER-LEFT:black 1px solid; BORDER-BOTTOM:black 1px solid">
