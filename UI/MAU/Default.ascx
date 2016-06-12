<%@ Control Language="vb" AutoEventWireup="false" Inherits="BaseApplication._Default" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<%@ Register TagPrefix="uc1" TagName="ClientLogonInformation" Src="../Default/UserControls/ClientLogonInformation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HierarchalMenu" Src="../Default/UserControls/HierarchalMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ApplicationInformation" Src="../Default/UserControls/ApplicationInformation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LineMenu" Src="../Default/UserControls/LineMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LeftHandModulesLoader" Src="../Default/UserControls/LeftHandModulesLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RightHandModulesLoader" Src="../Default/UserControls/RightHandModulesLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CopyRight" Src="../Default/UserControls/CopyRight.ascx" %>
<HTML>
  <HEAD>
		<title runat="server" id=PageTitle></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name=GENERATOR>
		<meta content="Visual Basic .NET 7.1" name=CODE_LANGUAGE>
		<meta content=JavaScript name=vs_defaultClientScript>
		<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema>
		<script src="Scripts/JS/Common/common.js" type=text/javascript></script>
		<script src="Scripts/JS/Common/nifty.js" type=text/javascript></script>
		<script src="Scripts/JS/MAUI/common.js" type=text/javascript></script>
		<script src="Scripts/JS/MAUI/caseSummaryData.js" type=text/javascript></script>
		<LINK href="UI/Default/Styles/Main.css" type=text/css rel=stylesheet >
		<LINK href="UI/Default/Styles/niftyCorners.css" type=text/css rel=stylesheet >
		<LINK href="UI/MAU/Styles/HierarchalMenu.css" type=text/css rel=stylesheet >
		<LINK href="UI/MAU/Styles/screens.css" type="text/css" rel="stylesheet" >
		<LINK media=Print href="UI/Default/Styles/niftyPrint.css" type=text/css >
		<style id="CustomStyles" runat="server">
	</style>
</HEAD>
	<body>
		<form id=Default runat="server">
			<table width="100%" border="0" cellpadding="0" cellspacing="0">
				<tr bgcolor="#ffcc33">
					<td width="130" height="102" valign="top">
						<img WIDTH="130" HEIGHT="102" Alt="Umass Logo" src="UI/MAU/Images/white_sm2.gif">
					</td>
					<td valign="top" colspan="2">
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td>
									<table border="0" width="100%" cellpadding="0" cellspacing="0">
										<tr>
											<td height="77" background="UI/MAU/Images/topcross.gif" align="center">
												<uc1:ApplicationInformation id="ApplicationInformation" runat="server"></uc1:ApplicationInformation>
											</td>
											<td height="77" background="UI/MAU/Images/topcross.gif"><img height="73" Alt="Umass Logo" src="UI/MAU/Images/applogo.gif"></td>							</tr>
										<tr>
											<td width="100%" colspan="2">
												<uc1:HierarchalMenu MenuType="Horizontal" ClickToOpen="false" id="HierarchalMenu" runat="server"></uc1:HierarchalMenu>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="2" height="20px">
						<uc1:ClientLogonInformation id="ClientLogonInformation" runat="server"></uc1:ClientLogonInformation>
					</td>
					<td align="right" width="100%" colspan="2">
						<uc1:LineMenu id="LineMenu" runat="server"></uc1:LineMenu>
					</td>
				</tr>
				<tr>
					<td valign="top" background="UI/MAU/Images/bottonside.gif">
						<table border="0" cellpadding="0" cellspacing="0" width="130px">
							<tr height="88">
								<td background="UI/MAU/Images/uppermidleft.gif">
								</td>
							</tr>
						</table>
						<table border="0" cellpadding="0" cellspacing="0" width="130px">
							<tr>
								<td>
									<uc1:LeftHandModulesLoader id="LeftHandModulesLoader" runat="server"></uc1:LeftHandModulesLoader>
								</td>
							</tr>
						</table>
					</td>
					<td valign="top" colspan="2">
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td rowspan="2">
									<table border="0" cellpadding="0" cellspacing="0" width="100%">
										<tr>
											<td valign="top" height="37" background="UI/MAU/Images/middlecross.gif">
												&nbsp;
											</td>
										</tr>
										<tr>
											<td>
												<table bgcolor="lightgrey" border="1" cellpadding="0" cellspacing="10" width="95%">
													<tr>
														<td>
															<fieldset class="inset">
																<uc1:RightHandModulesLoader id="RightHandModulesLoader" runat="server"></uc1:RightHandModulesLoader>
															</fieldset>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td valign="top" background="UI/MAU/Images/bottonside.gif">
						&nbsp;
					</td>
					<td colspan="2">
						<uc1:CopyRight id="Copyright1" runat="server"></uc1:CopyRight>
					</td>
				</tr>
				<tr height="250px">
					<td valign="top" background="UI/MAU/Images/bottonside.gif">
						&nbsp;
					</td>
					<td colspan="2">
						&nbsp;
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
