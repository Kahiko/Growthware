<%@ Page Language="vb" AutoEventWireup="false" Codebehind="TestPage.aspx.vb" Inherits="BaseApplication.TestPage" %>
<%@ Register TagPrefix="uc1" TagName="PageHeaderControl" Src="~/UI/Default/UserControls/PageHeaderControl.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
    <title>TestPage</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
	<link id="mainStyle" href="Main.css" type="text/css" rel="stylesheet" runat="server"/>
	<link id="NiftyCornersStyle" href="niftyCorners.css" type="text/css" rel="stylesheet" runat="server"/>
	<link id="NiftyPrintStyle" href="niftyPrint.css" type="text/css" rel="stylesheet" media="print" runat="server"/>
	<script type="text/javascript" src="../../Scripts/JS/Common/common.js"></script>
	<script type="text/javascript" src="../../Scripts/JS/Common/nifty.js"></script>
	<style id="CustomStyles" runat="server">
	</style>
</HEAD>
  <body MS_POSITIONING="GridLayout">

    <form id="Form1" method="post" runat="server">
    <table width="100%">
		<tr>
			<td colspan=2>
			    <uc1:PageHeaderControl id="PageHeaderControl" runat="server"></uc1:PageHeaderControl>
			</td>
		</tr>
		<tr>
			<td width="100%" align=left valign=top>
				<table>
					<tr>
						<td align="right">First Name:</td>
						<td align="left">
							<asp:TextBox ID="FirstName" Runat="server"></asp:TextBox>
						</td>
					</tr>
				</table>
				<asp:Literal id="litOutput" runat="server"></asp:Literal>
  			</td>
		</tr>
		<tr>
			<td align=center>
				<asp:Button id="btnThrowError" runat="server" Text="Throw Error"></asp:Button>
				<asp:Button id="btnTestMail" runat="server" Text="TestMail"></asp:Button>
				<asp:Button id="btnLogMSG" runat="server" Text="Log Message"></asp:Button>
			</td>
		</tr>
    </table>

    </form>

  </body>
</HTML>
