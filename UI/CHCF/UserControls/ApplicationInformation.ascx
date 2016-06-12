<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ApplicationInformation.ascx.vb" Inherits="BaseApplication.ApplicationInformation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@OutputCache Duration="3660" VaryByParam="none" %>
<TABLE id="Table1" border="0" cellpadding="3" cellspacing="0">
	<TR>
		<TD noWrap>
			<asp:Label ID="Label1" Runat="server">Application: </asp:Label>
			&nbsp;<asp:Label id="lblAppName" Font-Bold="True" Runat="server">lblAppName</asp:Label>
			&nbsp;<asp:Label id="Label2" runat="server">Environment: </asp:Label>
			&nbsp;<asp:Label id="lblEnvironment" Font-Bold="True" runat="server">Label</asp:Label>
			&nbsp;<asp:Label id="Label3" runat="server">Version:</asp:Label>
			&nbsp;<asp:Label id="lblVersion" Font-Bold="True" runat="server">lblVersion</asp:Label>
		</TD>
	</TR>
</TABLE>