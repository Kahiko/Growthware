<%@ Control Language="vb" AutoEventWireup="false" Codebehind="LeftHandModuleHeader.ascx.vb" Inherits="BaseApplication.LeftHandModuleHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
	<table width="205" cellpadding="0px" cellspacing="0px">
		<tr id="trLeftHandModuleHeader" runat="server">
			<td style="border-color:black;border-style:solid;border-width:1;">
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
			<td bgcolor="<%= ClientChoicesState("LEFT_COLOR")%>" colspan="3" style="border-color:black;border-style:solid;border-width:1;border-top:0px">
