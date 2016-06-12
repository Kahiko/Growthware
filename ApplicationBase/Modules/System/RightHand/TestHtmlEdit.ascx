<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TestHtmlEdit.ascx.vb" Inherits="ApplicationBase.TestHtmlEdit" %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<asp:Button ID="btnSave" Text="Save" ToolTip="Save the Form" runat="server" />&nbsp;<br />
<CustomWebControls:NavTrail ID="NavTrail1" runat="server">
    <CustomWebControls:NavTrailTab Text="Logoff" ID="NavTrailTab1" Action="Logoff" runat="server" />
</CustomWebControls:NavTrail>
xx<br />
<CustomWebControls:HtmlTextBox ID="HtmlTextBox1" runat="server" Height="250px" HTMLDepth="full" Width="550px" />
