<%@ Register TagPrefix="uc1" TagName="LeftHandModulesLoader" Src="../Default/UserControls/LeftHandModulesLoader.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="BaseApplication._Default" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="HierarchalMenu" Src="../Default/UserControls/HierarchalMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RightHandModulesLoader" Src="../Default/UserControls/RightHandModulesLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LineMenu" Src="../Default/UserControls/LineMenu.ascx" %>

<HTML>
  <HEAD>
		<title runat="server" id=PageTitle></title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name=vs_defaultClientScript content="JavaScript">
		<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<script src="Scripts/JS/Common/common.js" type=text/javascript></script>
		<script src="Scripts/JS/Common/nifty.js" type=text/javascript></script>
		<LINK href="UI/Default/Styles/main.css" type=text/css rel=stylesheet >
		<LINK href="UI/CommMed/Styles/HierarchalMenu.css" type=text/css rel=stylesheet >
		<LINK href="UI/CommMed/Styles/styles.css" type=text/css rel=stylesheet >
		<LINK href="UI/CommMed/Styles/udm-style.css" type=text/css rel=stylesheet >
		<LINK href="UI/Default/Styles/niftyCorners.css" type=text/css rel=stylesheet >
		<LINK media=Print href="UI/Arc/Styles/niftyPrint.css" type=text/css >
		<style id=CustomStyles runat="server"></style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table border="0" cellpadding="0" cellspacing="0" width="98%">
				<tr>
					<td>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td background="UI/CommMed/Images/header_bgd.gif" WIDTH="184px" height="96">
									<img src="UI/CommMed/Images/logo_CWM_06.gif">
								</td>
								<td background="UI/CommMed/Images/header_bgd.gif" height="96">
									&nbsp;
								</td>
								<td valign="bottom"><img src="UI/CommMed/Images/mainnav_tab.gif"></td>
								<td width="100%">
									<table border="0" cellpadding="0" cellspacing="0" width="100%" height="96">
										<tr>
											<td>&nbsp;</td>
										</tr>
										<tr>
											<td valign="top" align="right" background="UI/CommMed/Images/mainnav_bgd.gif" height="32">
												<uc1:LineMenu id="LineMenu1" runat="server"></uc1:LineMenu>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td width="1" valign="top" style="BACKGROUND: url(UI/CommMed/Images/Nav2_bgd.jpg) no-repeat" height="400">
									<uc1:LeftHandModulesLoader id="LeftHandModulesLoader1" runat="server"></uc1:LeftHandModulesLoader>
									<br>
									<uc1:HierarchalMenu id="HierarchalMenu" MenuType="Vertical" ClickToOpen="False" runat="server"></uc1:HierarchalMenu>
								</td>
								<td align="left" valign="top">
									<uc1:RightHandModulesLoader id="RightHandModulesLoader" runat="server"></uc1:RightHandModulesLoader>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="center" id="footer">
									<P>
										This is an <A title="Click here to Go to the UMass Medical School Official Designation" href="http://www.umassmed.edu/about/official.cfm">official</A> Page/Publication of the University of Massachusetts Worcester Campus<BR><B>Commonwealth Medicine</B>
										<IMG height=10 alt="" src="Images/blackbullet.gif" width=9 border=0> 100 Century Drive Worcester, MA 01606 <BR>
										Questions or Comments? Email: <A href="mailto:CommMedWebInfo@umassmed.edu">CommMedWebInfo@umassmed.edu</A>&nbsp;&nbsp; Phone: 508-856-6222 
										<SPAN id=linkInternet>
											<BR><BR>
											<A title="Go to the UMass Medical School web site" href="http://www.umassmed.edu/">University of Massachusetts Medical School</A> 
										</SPAN>
									</P>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>