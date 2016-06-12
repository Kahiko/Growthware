<%@ Page Language="vb" AutoEventWireup="false" Codebehind="TimeOut.aspx.vb" Inherits="BaseApplication.TimeOut" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>TimeOut</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name=vs_defaultClientScript content="JavaScript">
		<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<table border="1">
				<tr>
					<td align="center">
						The system has timed out.<br>
						Please use the return button to logon again.
					</td>
				</tr>
				<tr>
					<td align="center">
						<asp:Button ID="btnReturn" Runat="server" Text="Return"></asp:Button>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
