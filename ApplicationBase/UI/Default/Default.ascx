<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Default.ascx.vb" Inherits="ApplicationBase._Default" %>
<%@ Register Src="UserControls/LeftHand/LeftHandModuleHeader.ascx" TagName="LeftHandModuleHeader" TagPrefix="uc1" %>
<%@ Register Src="UserControls/LeftHand/LeftHandModuleHeaderFooter.ascx" TagName="LeftHandModuleHeaderFooter" TagPrefix="uc1" %>
<%@ Register Src="UserControls/RightHand/RightHandModuleHeader.ascx" TagName="RightHandModuleHeader" TagPrefix="uc1" %>
<%@ Register Src="UserControls/RightHand/RightHandModuleHeaderFooter.ascx" TagName="RightHandModuleHeaderFooter" TagPrefix="uc1" %>
<%@ Register Src="UserControls/PageHeaderControl.ascx" TagName="PageHeaderControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/LeftHandModulesLoader.ascx" TagName="LeftHandModulesLoader" TagPrefix="uc1" %>
<%@ Register Src="UserControls/RightHandModulesLoader.ascx" TagName="RightHandModulesLoader" TagPrefix="uc1" %>
<%@ Register Src="UserControls/CopyRight.ascx" TagName="CopyRight" TagPrefix="uc1" %>
<html>
<head id="head" runat="server">
	<title id="PageTitle" runat="server"></title>
	<meta content="Microsoft Visual Studio .NET 8.0" name="GENERATOR"/>
	<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
	<meta content="JavaScript" name="vs_defaultClientScript"/>
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>

	<script src='<%=ResolveUrl("~/Scripts/JS/Common/common.js")%>' type="text/javascript"></script>

	<script src='<%=ResolveUrl("~/Scripts/JS/Common/nifty.js")%>' type="text/javascript"></script>

	<link href="~/UI/Default/Styles/Main.css" rel="stylesheet" type="text/css"/>
	<link href="~/UI/Default/Styles/HierarchalMenu.css" rel="stylesheet" type="text/css"/>
	<link href="~/UI/Default/Styles/niftyCorners.css" rel="stylesheet" type="text/css"/>
	<link href="~/UI/Default/Styles/niftyPrint.css" media="Print" type="text/css"/>
	<style id="CustomStyles" runat="server" type="text/css"></style>
</head>
<body>
	<form id="Default" runat="server">
		<table border="0" cellpadding="2" width="100%">
			<tr>
				<td>
					<asp:PlaceHolder ID="plcPageHeaderControl" runat="server">
						<uc1:PageHeaderControl ID="PageHeaderControl1" runat="server" />
					</asp:PlaceHolder>
				</td>
			</tr>
			<tr>
				<td>
					<table border="0" width="100%">
						<tr>
							<td valign="top">
								<uc1:LeftHandModuleHeader ID="LeftHandModuleHeader" runat="server" canClose="false" canEdit="false" Title="Set by code" />
								<uc1:LeftHandModulesLoader ID="LeftHandModulesLoader" runat="server" />
								<uc1:LeftHandModuleHeaderFooter ID="LeftHandModuleHeaderFooter" runat="server" />
							</td>
							<td align="center" valign="top" style="width: 100%">
								<uc1:RightHandModuleHeader ID="RightHandModuleHeader" runat="server" canClose="false" canEdit="false" Title="Welcome" />
								<uc1:RightHandModulesLoader ID="RightHandModulesLoader" runat="server" />
								<uc1:RightHandModuleHeaderFooter ID="RightHandModuleHeaderFooter" runat="server" />
							</td>
						</tr>
					</table>
					<uc1:CopyRight ID="CopyRight" runat="server" />
				</td>
			</tr>
		</table>
	</form>
</body>
</html>