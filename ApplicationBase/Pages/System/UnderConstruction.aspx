<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UnderConstruction.aspx.vb" Inherits="ApplicationBase.UnderConstruction"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
    <title>Under Construction</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </HEAD>
  <body>

    <form id="Form1" method="post" runat="server">
		<table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
				<td align="center">
					<asp:Image ID="imgUnderConstruction" ImageUrl="../../Images/UnderConstruction.gif" Runat="server"></asp:Image>
				</td>
			</tr>
			<tr>
				<td>
					Currently our site is under construction.<br>
					Please check back <asp:Label ID="lblExpectedUpBy" Runat="server"></asp:Label> the construction should be done.<br>
				</td>
			</tr>
		</table>
    </form>

  </body>
</HTML>
