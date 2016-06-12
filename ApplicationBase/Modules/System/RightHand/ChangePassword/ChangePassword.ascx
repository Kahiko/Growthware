<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.ChangePassword" Codebehind="ChangePassword.ascx.vb" %>
<P>
	<table width="100%" cellpadding="0" cellspacing="2" style="FONT: 8pt verdana, arial">
		<tr>
			<td colspan="2">
				&nbsp;<label id="ClientMSG" runat="server" style="FONT-WEIGHT: bold; COLOR: red"></label>
			</td>
		</tr>
		<tr id="trOldPassword" runat="server">
			<td align="right" valign="top">
				Old Password&nbsp;:
			</td>
			<td align="left">
				<input type="password" id="OldPassword" runat="server">
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Required Field" ControlToValidate="OldPassword"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				New Password&nbsp;:
			</td>
			<td align="left">
				<input type="password" id="NewPassword" runat="server" NAME="NewPassword">
				<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Required Field" ControlToValidate="NewPassword"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td align="right" valign="top">
				Confirm Password&nbsp;:
			</td>
			<td align="left">
				<input type="password" id="NewPassword2" runat="server" NAME="NewPassword2">
				<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="Required Field" ControlToValidate="NewPassword2"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td colspan="2" align="center">
				<asp:Button ID="btnChangePassword" Text="Change Password" Runat="server" EnableViewState="False"></asp:Button>
			</td>
		</tr>
		<tr>
			<td colspan="2" align="left">
				<b>Please Note:</b>
				<ul>
					<li>
					All fields are required
					<li>
					"Old password" can not match new password
					<li>
						"New Password" and "Confirm Password" must match
					</li>
				</ul>
			</td>
		</tr>
	</table>
</P>
