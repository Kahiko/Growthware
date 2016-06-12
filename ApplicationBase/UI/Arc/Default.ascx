<%@ Control AutoEventWireup="false" Inherits="ApplicationBase._Default" Language="vb" %>
<%@ Register TagPrefix="uc1" TagName="LineMenu" Src="../Default/UserControls/LineMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ClientLogonInformation" Src="../Default/UserControls/ClientLogonInformation.ascx" %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Register TagPrefix="uc1" TagName="HierarchalMenu" Src="../Default/UserControls/HierarchalMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ApplicationInformation" Src="../Default/UserControls/ApplicationInformation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RightHandModulesLoader" Src="../Default/UserControls/RightHandModulesLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LeftHandModulesLoader" Src="../Default/UserControls/LeftHandModulesLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CopyRight" Src="../Default/UserControls/CopyRight.ascx" %>
<HTML>
  <HEAD id="head" runat="server">
		<title runat="server" id=PageTitle></title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name=vs_defaultClientScript content="JavaScript">
		<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<script src="Scripts/JS/Common/common.js" type=text/javascript></script>
		<script src="Scripts/JS/Common/nifty.js" type=text/javascript></script>
		<LINK href="UI/Default/Styles/Main.css" type=text/css rel=stylesheet >
		<LINK href="UI/Arc/Styles/HierarchalMenu.css" type=text/css rel=stylesheet >
		<LINK href="UI/Arc/Styles/Default.css" type=text/css rel=stylesheet >
		<LINK href="UI/Arc/Styles/niftyCorners.css" type=text/css rel=stylesheet >
		<LINK media=Print href="UI/Arc/Styles/niftyPrint.css" type=text/css >
		<style id=CustomStyles runat="server"></style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr bgcolor="#dadfcb">
					<td align="center" width="1px">
						<asp:Image ImageUrl="../../Images/umms.gif" ID="AppImage" Runat="server"></asp:Image>
					</td>
					<td colspan="2" width="100%">
						<table width="100%">
							<tr>
								<td align="center">
									<uc1:ApplicationInformation id="ApplicationInformation" runat="server"></uc1:ApplicationInformation>
								</td>
							</tr>
							<tr>
								<td height="25" align="center">
									<uc1:ClientLogonInformation id="ClientLogonInformation" runat="server"></uc1:ClientLogonInformation>
								</td>
							</tr>
							<tr>
								<td height="25" align="right">
									<uc1:LineMenu id="LineMenu" runat="server"></uc1:LineMenu>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="3" style="BORDER-TOP:#000000 1px solid" bgcolor='#ffffff'>
						<img src='<%=ResolveUrl("Images/black_lines_top.gif")%>' width="192" height="17" alt="">
					</td>
				</tr>
				<tr valign="top">
					<td colspan="3" width="100%">
						<table border="0" cellpadding="0" cellspacing="0" width="100%"align="left" valign="top" rowspan="2">
							<tr>
								<td bgcolor="#cfd0c2"><img src='<%=ResolveUrl("Images/small_gray_line.gif")%>' width="50" height="16" alt=""></td>
								<td bgcolor="#cfd0c2"><img src='<%=ResolveUrl("Images/thick_gray_line.gif")%>' width="85" height="16" alt=""></td>
								<td valign="top" bgcolor='<%=ClientChoicesState("HeadColor")%>'><img src='<%=ResolveUrl("Images/top_curveT.gif")%>' width="55" height="16" alt=""></td>
								<td width="100%" bgcolor='<%=ClientChoicesState("HeadColor")%>'>&nbsp;</td>
							</tr>
							<tr height="600px">
								<td colspan="2" bgcolor="#cfd0c2" height="16" valign="top">
									<uc1:HierarchalMenu MenuType="Vertical" ClickToOpen="False" id="Hierarchalmenu1" runat="server"></uc1:HierarchalMenu>
									<uc1:LeftHandModulesLoader id="LeftHandModulesLoader" runat="server"></uc1:LeftHandModulesLoader>
									<uc1:CopyRight id="Copyright1" runat="server"></uc1:CopyRight>
								</td>
								<td background='<%=ResolveUrl("Images/menubg.gif")%>' valign="top">
									<img src='<%=ResolveUrl("Images/right_curve.gif")%>' width="55" height="129" alt="">
								</td>
								<td width="100%" valign="top">
									<table border="0" cellpadding="0" cellspacing="0" width="100%">
										<tr>
											<td>&nbsp;</td>
										</tr>
										<tr>
											<td>
												<uc1:RightHandModulesLoader id="RightHandModulesLoader" runat="server"></uc1:RightHandModulesLoader>
											</td>
										</tr>
										<tr>
											<td align="center">
												<uc1:LineMenu id="Linemenu1" runat="server"></uc1:LineMenu>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
