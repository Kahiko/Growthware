<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Logon.aspx.vb" Inherits="GrowthWare.WebApplication.Logon" %>

<!DOCTYPE html>

	<asp:Button ID="btnLogon" Text="    Logon    " runat="server" style="z-index: 100; left: 8px; position: absolute; top: 152px" TabIndex="4" />
	<asp:CheckBox cssclass="Form_LabelText" ID="chkRememberMe" Text="Remember Me" runat="server" style="z-index: 101; left: 8px; position: absolute; top: 120px" TabIndex="3" />
	<asp:Button ID="btnRequestPasswordReset" Visible="False" Text="Reset Password" CausesValidation="false" Runat="server" style="z-index: 102; left: 8px; position: absolute; top: 184px" Width="120px" TabIndex="5" />
	<asp:textbox id="txtAccount" style="size: 14px; z-index: 103; left: 8px; position: absolute; top: 32px;" Runat="server" Width="120px" TabIndex="1" />
	<asp:TextBox ID="txtPassword" TextMode="password" style="size: 16px; z-index: 104; left: 8px; position: absolute; top: 88px;" runat="server" Width="120px" TabIndex="2" />
	<asp:RequiredFieldValidator id="RequiredFieldValidatorPassword" runat="server" ErrorMessage="(required)" ControlToValidate="txtPassword" style="z-index: 105; left: 72px; position: absolute; top: 64px"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator id="RequiredFieldValidatorAccount" runat="server" ErrorMessage="(required)" ControlToValidate="txtAccount" style="z-index: 106; left: 64px; position: absolute; top: 8px"></asp:RequiredFieldValidator>
	<asp:Label ID="lblAccount" CssClass="Form_LabelText" runat="server" style="z-index: 107; left: 8px; position: absolute; top: 8px" Text="Account:"></asp:Label>
	<asp:Label ID="lblPassword" CssClass="Form_LabelText" runat="server" Style="z-index: 108; left: 8px; position: absolute; top: 64px" Text="Password:" Width="64px"></asp:Label>
	<asp:Label ID="lblError" CssClass="Form_Message" runat="server" Visible="false" style="z-index: 110; left: 8px; position: absolute; top: 216px"></asp:Label>
