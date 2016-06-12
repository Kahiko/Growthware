<%@ Control AutoEventWireup="false" Inherits="ApplicationBase._Default" Language="vb" %>
<%@ Register TagPrefix="uc1" TagName="ClientLogonInformation" Src="../Default/UserControls/ClientLogonInformation.ascx" %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Register TagPrefix="uc1" TagName="HierarchalMenu" Src="../Default/UserControls/HierarchalMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ApplicationInformation" Src="../Default/UserControls/ApplicationInformation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LineMenu" Src="../Default/UserControls/LineMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LeftHandModulesLoader" Src="../Default/UserControls/LeftHandModulesLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RightHandModulesLoader" Src="../Default/UserControls/RightHandModulesLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CopyRight" Src="../Default/UserControls/CopyRight.ascx" %>
<HTML>
  <head id="head" runat="server">
		<title runat="server" id=PageTitle></title>
		<script src="Scripts/JS/Common/common.js" type=text/javascript></script>
		<script src="Scripts/JS/Common/nifty.js" type=text/javascript></script>
		<link href="UI/Default/Styles/Main.css" type=text/css rel=stylesheet />
		<link href="UI/Professional/Styles/HierarchalMenu.css" type=text/css rel=stylesheet />
		<link href="UI/Professional/Styles/CustomControls.css" type=text/css rel=stylesheet />
		<link href="UI/Default/Styles/niftyCorners.css" type=text/css rel=stylesheet />
		<link media=Print href="UI/Default/Styles/niftyPrint.css" type=text/css />
		<link href='<%=ResolveUrl( "Styles/Layout.css")%>' type="text/css" rel="stylesheet" />
		<style id=CustomStyles runat="server"></style>
	</head>
	<body>
		<form id=Default runat="server">
			<table width="100%" border="0" cellspacing="0" cellpadding="0" id=Table1>
				<tr>
					<td align="center" valign="top">
						<table width="100%" border="0" cellspacing="0" cellpadding="0" id=Table2>
							<tr>
								<td width="20%" align="left" valign="middle" bgcolor="#cbcdd1">
<!--Logo Start-->
									<table width="100%" border="0" cellspacing="0" cellpadding="0" height="70" id=Table3>
										<tr>
											<td align="center" valign="middle"> 
												<asp:Image ImageUrl="~/Images/umms.gif" ID="AppImage" Runat="server"></asp:Image>
											</td>
										</tr>
									</table>
								</td>
								<td align="left" valign="top" width="103" bgcolor="#cbcdd1">
									<img src='<%=ResolveUrl( "Images/top_blue_curve.gif" )%>' width="103" height="84" border="0" alt=""><br>
								</td>
								<td bgcolor="#cbcdd1" style="BACKGROUND-REPEAT: repeat-x" background='<%=ResolveUrl( "Images/top_blue_strip.gif" )%>'  align="left" valign="top" height="84" width="100%"> 
<!--Banner Start-->
									<div align="center">
										<table width="100%" border="0" cellspacing="0" cellpadding="4" id=Table4>
											<tr>
												<td align="center" valign="middle" class="Banner">
													<uc1:ApplicationInformation id="ApplicationInformation" runat="server"></uc1:ApplicationInformation>
													<uc1:ClientLogonInformation id="ClientLogonInformation" runat="server"></uc1:ClientLogonInformation>
													<uc1:LineMenu id="LineMenu" runat="server"></uc1:LineMenu>
												</td>
											</tr>
										</table>
									</div>
								</td>
							</tr>
							<tr>
								<td colspan="3" bgcolor="#cbcdd1" background='<%=ResolveUrl( "Images/top_black_stripes.gif" )%>' align="left" valign="top" height="5" width="100%"></td>
							</tr>
							<tr>
								<td colspan="3" bgcolor="#cbcdd1" background='<%=ResolveUrl( "Images/blue_topic_background.gif" )%>' valign="middle" align="center" height="26" width="100%">
									<table border="0" cellspacing="0" cellpadding="0" width="100%" id=Table5>
										<tr>
<!--Middle blank row-->
											<td align="center" valign="middle" width="100%"></td>
											<!--
											<td align="right" valign="middle" width="180" class="Search" nowrap>
												Search: <asp:TextBox id="txtSearch" height="15" font-size="7" width="110" Runat="Server"/><asp:LinkButton id="lnkSearch" Text=" Go" Runat="Server"/>
											</td>
											-->
										</tr>
									</table>
								</td> 
							</tr>
							<tr>
								<td colspan="3" bgcolor="#cbcdd1" background='<%=ResolveUrl( "Images/top_black_stripe.gif" )%>' align="left" valign="middle" height="2" width="100%">
								</td>
							</tr>
						</table>
						<table border="0" cellspacing="0" cellpadding="0" id=Table6>
							<tr>
								<td bgcolor="#d5d5d5" align="left" valign="top">
									<table width="165" border="0" cellspacing="0" cellpadding="0" id=Table7>
										<tr>
											<td align="left" valign="top">
												<img src='<%=ResolveUrl( "Images/blue_section_top.gif" )%>' width="100%" height="14" border="0" alt=""><br>
											</td>
										</tr>
										<tr>
											<td bgcolor="#90a1b9" align="left" valign="top">
<!--Navigation and LeftHand modules-->
												<table cellspacing="0" cellpadding="0" border="0" id=Table8>
													<tr>
														<td>
															<uc1:HierarchalMenu MenuType="Vertical" ClickToOpen="False" id="HierarchalMenu" runat="server"></uc1:HierarchalMenu><br>
															<hr>
															<uc1:LeftHandModulesLoader id="LeftHandModulesLoader" runat="server"></uc1:LeftHandModulesLoader>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td align="left" valign="top">
												<img src='<%=ResolveUrl( "Images/blue_section_divider.gif" )%>' height="1" border="0" alt=""><br>
											</td>
										</tr>
										<tr>
											<td align="left" valign="top">
												<img src='<%=ResolveUrl( "Images/blue_section_bottom.gif" )%>' width="100%" height="11" border="0" alt=""><br>
											</td>
										</tr>
										<tr>
											<td background='<%=ResolveUrl( "Images/blue_background_right.gif" )%>'>
												<br>
<!--Static Navigation Start-->
												<div align="center">
													<table width="140" border="0" cellspacing="0" cellpadding="0" class="StaticNav" id=Table9>
														<tr> 
															<td align="left" valign="top">
																&nbsp;
															</td>
														</tr>
<!--Copyright notice -->
														<tr>
															<td height="50" valign="bottom">
																<uc1:CopyRight id="CopyRight" runat="server"></uc1:CopyRight>
															</td>
														</tr>
													</table>
												</div>
												<p>
												<br></p>
											</td>
										</tr>
									</table>
								</td>
								<td align="left" valign="top" background='<%=ResolveUrl( "Images/black_line_left.gif" )%>' width="40"></td>
<!--Content or RightHandModules-->
								<td width="100%" align="left" valign="top" class="body">
									<table width="100%" border="0" cellpadding="3" cellspacing="3" id=Table10>
										<tr>
											<td width="100%" align="left" valign="top" class="body">
												<uc1:RightHandModulesLoader id="RightHandModulesLoader" runat="server"></uc1:RightHandModulesLoader>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td height="1" bgcolor="#000000"></td>
				</tr>
			</table>
<!-- Static Navigation -->
			<table width="100%" id=Table11>
				<tr>
					<td width="100%" align="center">
						<uc1:LineMenu id="LineMenu1" runat="server"></uc1:LineMenu>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
