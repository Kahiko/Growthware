<%@ Control Language="vb" EnableViewState="False" AutoEventWireup="false" Inherits="ApplicationBase.ClientLogonInformation" Codebehind="ClientLogonInformation.ascx.vb" %>
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
