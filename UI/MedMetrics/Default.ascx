<%@ Control Language="vb" AutoEventWireup="false" Inherits="BaseApplication._Default" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LineMenu" Src="../Default/UserControls/LineMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HierarchalMenu" Src="../Default/UserControls/HierarchalMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RightHandModulesLoader" Src="../Default/UserControls/RightHandModulesLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LeftHandModulesLoader" Src="../Default/UserControls/LeftHandModulesLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ClientLogonInformation" Src="../Default/UserControls/ClientLogonInformation.ascx" %>
<HEAD>
	<title runat="server" id=PageTitle></title>
</HEAD>
<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
<meta content="JavaScript" name="vs_defaultClientScript">
<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
<script src="Scripts/JS/Common/common.js" type="text/javascript"></script>
<script src="Scripts/JS/Common/nifty.js" type="text/javascript"></script>
<style id="CustomStyles" runat="server">
</style>
<LINK href="UI/Default/Styles/Main.css" type="text/css" rel="stylesheet">
<link href='<%=ResolveUrl( "Styles/styles.css")%>' type="text/css" rel="stylesheet" >
<LINK href='<%=ResolveUrl( "Styles/HierarchalMenu.css")%>' type=text/css rel=stylesheet >
<LINK href="UI/Default/Styles/niftyCorners.css" type="text/css" rel="stylesheet">
<LINK media="Print" href="UI/Default/Styles/niftyPrint.css" type="text/css">
<form id="Default" runat="server">
	<table border="0" cellpadding="0" cellspacing="0" width="750">
		<tr>
			<td class="topNav">
				<asp:Image ID="MedMetricsBanner" ImageUrl="Images/banner.jpg" Width="750" BorderWidth="0" AlternateText="MedMetricShp Banner" Runat="server"></asp:Image>
			</td>
		</tr>
		<tr>
			<td height="25" align="right">
				<table border="0" cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td align="left">
							<uc1:ClientLogonInformation id="ClientLogonInformation" runat="server"></uc1:ClientLogonInformation>
						</td>
						<td align="right">
							<uc1:LineMenu id="LineMenu" runat="server"></uc1:LineMenu>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<table border="0" cellpadding="3" cellspacing="0" width="100%">
					<tr>
						<td vAlign="top" width="150">
							<TABLE class="sidenav1" height="500" cellSpacing="0" cellPadding="5" width="150" border="0">
								<tr>
									<td class="sideNav1" vAlign="top">
										<uc1:LeftHandModulesLoader id="LeftHandModulesLoader" runat="server"></uc1:LeftHandModulesLoader>
										<uc1:HierarchalMenu MenuType="Vertical" ClickToOpen="False" id="HierarchalMenu" runat="server"></uc1:HierarchalMenu>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
									</td>
								</tr>
								<tr>
									<TD class="sideNav2" vAlign="top">
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
										<BR>
									</TD>
								</tr>
							</TABLE>
						</td>
						<TD vAlign="top" width="600">
							<uc1:RightHandModulesLoader id="RightHandModulesLoader" runat="server"></uc1:RightHandModulesLoader>
						</TD>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<TABLE cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
					<TBODY>
						<TR>
							<TD class="footertext" colSpan="3">
								<HR color="#24238c" SIZE="3">
							</TD>
						</TR>
						<TR>
							<TD class="footertext" align="center" width="19%">
								<A onmouseover="window.status='Click here to go to the UMass Medical School web site';return true;"
									title="Click here to go to the UMass Medical School web site" onmouseout="window.status='';return true;"
									href="http://www.umassmed.edu/"><IMG title="Click here to go to the UMass Medical School web site" height="75" alt="UMMS Graphic"
										src="Images//umms.gif" width="50" border="0"> </A>
							</TD>
							<TD class="footertext" align="center" width="81%">
								©2005 MedMetrics Health Partners,Inc. All rights reserved.
								<BR>
								<A class="footertext" onmouseover="window.status='Click here to go to the UMass Medical School web site';return true;"
									title="Click here to go to the UMass Medical School web site" onmouseout="window.status='';return true;"
									href="http://www.umassmed.edu/">UMass Medical School </A>is pleased to host 
								this website
								<BR>
								For more information on MedMetricShp, please contact: 1-800-918-7551
							</TD>
						</TR>
					</TBODY>
				</TABLE>
			</td>
		</tr>
	</table>
</form>
