<%@ Register TagPrefix="uc1" TagName="LeftHandModuleHeader" Src="UserControls/LeftHand/LeftHandModuleHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LeftHandModuleHeaderFooter" Src="UserControls/LeftHand/LeftHandModuleHeaderFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RightHandModuleHeader" Src="UserControls/RightHand/RightHandModuleHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RightHandModuleHeaderFooter" Src="UserControls/RightHand/RightHandModuleHeaderFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PageHeaderControl" Src="UserControls/PageHeaderControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LeftHandModulesLoader" Src="UserControls/LeftHandModulesLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RightHandModulesLoader" Src="UserControls/RightHandModulesLoader.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Default.ascx.vb" Inherits="BaseApplication._Default" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CopyRight" Src="UserControls/CopyRight.ascx" %>

<HTML>
  <HEAD>
		<title runat="server" id=PageTitle></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name=GENERATOR>
		<meta content="Visual Basic .NET 7.1" name=CODE_LANGUAGE>
		<meta content=JavaScript name=vs_defaultClientScript>
		<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema>
		<script src="Scripts/JS/Common/common.js" type=text/javascript></script>
		<script src="Scripts/JS/Common/nifty.js" type=text/javascript></script>
		<LINK href="UI/Default/Styles/Main.css" type=text/css rel=stylesheet >
		<LINK href="UI/Default/Styles/HierarchalMenu.css" type=text/css rel=stylesheet >
		<LINK href="UI/Default/Styles/niftyCorners.css" type=text/css rel=stylesheet >
		<LINK media=Print href="UI/Default/Styles/niftyPrint.css" type=text/css >
		<style id=CustomStyles runat="server">
	</style>
</HEAD>
	<body>
		<form id=Default runat="server">
			<table cellPadding=2 width="100%" border=0>
				<tr>
					<td>
						<asp:PlaceHolder ID="plcPageHeaderControl" Runat="server"></asp:PlaceHolder><uc1:PageHeaderControl id="PageHeaderControl1" runat="server"></uc1:PageHeaderControl>
					</td>
				</tr>
				<tr>
					<td>
						<table width="100%" border=0>
							<tr>
								<td vAlign=top>
									<uc1:LeftHandModuleHeader Title="Set by code" canEdit="false" canClose="false" id="LeftHandModuleHeader" runat="server"></uc1:LeftHandModuleHeader>
									<uc1:LeftHandModulesLoader id="LeftHandModulesLoader" runat="server"></uc1:LeftHandModulesLoader>
									<uc1:LeftHandModuleHeaderFooter id="LeftHandModuleHeaderFooter" runat="server"></uc1:LeftHandModuleHeaderFooter>
								</td>
								<td vAlign=top align=center width="100%">
									<uc1:RightHandModuleHeader id="RightHandModuleHeader" Title="Welcome" canEdit="false" canClose="false" runat="server"></uc1:RightHandModuleHeader>
									<uc1:RightHandModulesLoader id="RightHandModulesLoader" runat="server"></uc1:RightHandModulesLoader>
									<uc1:RightHandModuleHeaderFooter id="RightHandModuleHeaderFooter" runat="server"></uc1:RightHandModuleHeaderFooter>
								</td>
							</tr>
						</table>
						<uc1:CopyRight id="CopyRight" runat="server"></uc1:CopyRight>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
