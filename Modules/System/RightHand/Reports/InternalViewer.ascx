<%@ Control Language="vb" AutoEventWireup="false" Codebehind="InternalViewer.ascx.vb" Inherits="BaseApplication.InternalViewer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=11.0.3300.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<form>
	<CR:CrystalReportViewer id="CrystalReportViewer" runat="server" AutoDataBind="true" Width="350px" Height="50px" Visible="False"></CR:CrystalReportViewer>
</form>