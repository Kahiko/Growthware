<%@ Control Language="vb" EnableViewState="False" AutoEventWireup="false" Codebehind="ClientLogonInformation.ascx.vb" Inherits="BaseApplication.ClientLogonInformation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE border="0" cellpadding="3" cellspacing="0">
	<TR>
		<TD id="trClientSecurityInformation" width="100%" nowrap runat="server">
			&nbsp;<asp:Label id="Label1" Runat="server">Logged on as: </asp:Label>
			&nbsp;<asp:Label id="lblAccount" Font-Names="Arial" Font-Bold="True" runat="server">lblAccount</asp:Label>
			&nbsp;<asp:Label id="Label3" runat="server">Selected BusinessUnitTranslation: </asp:Label>
			&nbsp;<asp:Label id="lblSelectedBusinessUnitName" Font-Bold="True" runat="server">lblSelectedBusinessUnit</asp:Label>
		</TD>
	</TR>
</TABLE>
