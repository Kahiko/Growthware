<%@ Control AutoEventWireup="false" Inherits="ApplicationBase._Default" Language="vb" %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Register TagPrefix="uc1" TagName="ApplicationInformation" Src="UserControls/ApplicationInformation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ClientLogonInformation" Src="../Default/UserControls/ClientLogonInformation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HierarchalMenu" Src="../Default/UserControls/HierarchalMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LineMenu" Src="UserControls/LineMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LeftHandModulesLoader" Src="../Default/UserControls/LeftHandModulesLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RightHandModulesLoader" Src="../Default/UserControls/RightHandModulesLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CopyRight" Src="../Default/UserControls/CopyRight.ascx" %>

<HTML>
  <HEAD id="head" runat="server">
		<title runat="server" id=PageTitle></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name=GENERATOR>
		<meta content="Visual Basic .NET 7.1" name=CODE_LANGUAGE>
		<meta content=JavaScript name=vs_defaultClientScript>
		<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema>
		<script src="Scripts/JS/Common/common.js" type=text/javascript></script>
		<script src="Scripts/JS/Common/nifty.js" type=text/javascript></script>
		<style id="CustomStyles" runat="server">
	</style>
		<link href='<%=ResolveUrl( "Styles/style01.css")%>' type="text/css" rel="stylesheet" >
		<LINK href="UI/Default/Styles/Main.css" type=text/css rel=stylesheet >
		<LINK href="UI/CHCF/Styles/HierarchalMenu.css" type=text/css rel=stylesheet >
		<LINK href="UI/Default/Styles/niftyCorners.css" type=text/css rel=stylesheet >
		<LINK media=Print href="UI/Default/Styles/niftyPrint.css" type=text/css >
  </HEAD>
	<body>
		<form id=Default runat="server">
			<table summary="This table contains the SBC header" border="0" cellpadding="0" cellspacing="0" width="100%">
				<tbody>
					<tr>
						<td bgcolor="#ffffff" height="5"></td>
					</tr>
					<tr>
						<td bgcolor="#ffffff" height="45" valign="bottom" width="160"><img src='<%=ResolveUrl( "Images/CHCF-logotop.gif" )%>' alt="CHCF logo" height="41" width="160"></td>
						<td nowrap width="65%">
							<span CLASS="H1">
								<uc1:ApplicationInformation id="ApplicationInformation" runat="server"></uc1:ApplicationInformation>
							</span>
							<br>
							<span class="h3">A MISSION-DRIVEN APPROACH TO COST-EFFECTIVE HEALTH CARE</span></td>
						<td width="18">&nbsp;</td>
						<td align="right" valign="top" width="102">
							<uc1:LineMenu id="LineMenu" runat="server"></uc1:LineMenu>
						</td>
						<td align="right" valign="top" width="10">&nbsp;</td>
					</tr>
					<tr>
						<td background='<%=ResolveUrl( "Images/banner-bg.gif" )%>' height="29" valign="top"><img src='<%=ResolveUrl( "Images/CHCF-logobot.gif" )%>' alt="CHCF logo" height="29" width="160"></td>
						<td background='<%=ResolveUrl( "Images/banner-bg.gif" )%>' nowrap >&nbsp;</td>
						<td width="18"><img src='<%=ResolveUrl( "Images/banner_rt-curve.gif" )%>' height="29" width="18"></td>
						<td class="nav" background='<%=ResolveUrl( "Images/banner_rt-end.gif" )%>' nowrap valign="bottom" width="100%"><uc1:ClientLogonInformation id="ClientLogonInformation" runat="server"></uc1:ClientLogonInformation></td>
					</tr>
				</tbody>
			</table>
			<table summary="This table is the main container for the current screen" border="0" cellpadding="0" cellspacing="0" width="100%">
				<tbody>
					<tr> 
<!---- Left links include -->
						<td valign="top" class="smallcontent" width="160" nowrap>
							<table border="0" width="100%" cellpadding="0" cellspacing="0">
								<tr>
									<td height="20" width="39">&nbsp;</td>
								</tr>

								<tr>
									<td>
										<uc1:LeftHandModulesLoader id="LeftHandModulesLoader" runat="server"></uc1:LeftHandModulesLoader>
									</td>
								</tr>
								<tr>
									<td>
										<uc1:HierarchalMenu MenuType="Vertical" ClickToOpen="False" id="HierarchalMenu" runat="server"></uc1:HierarchalMenu>
									</td>
								</tr>
								<tr>
									<td>
										<uc1:CopyRight id="CopyRight" runat="server"></uc1:CopyRight>
									</td>
								</tr>
							</table>
						</td>
<!---- End Left links include -->
						<td valign="top" id="applicationzone" width="94%"> 
							<table border="0" cellpadding="0" cellspacing="5" width="100%">
								<tbody>
									<tr>
										<td height="20" width="39">&nbsp;</td>
									</tr>
									<tr>
										<td class="generalcontent">
											<uc1:RightHandModulesLoader id="RightHandModulesLoader" runat="server"></uc1:RightHandModulesLoader>
										</td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
				</tbody>
			</table>
		</form>
	</body>
</HTML>