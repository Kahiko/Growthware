<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.SecurityByRole" Codebehind="SecurityByRole.ascx.vb" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="center">
			<CR:CrystalReportViewer id="CrystalReportViewer1" runat="server" AutoDataBind="true"></CR:CrystalReportViewer>
		</td>
	</tr>
</table>
