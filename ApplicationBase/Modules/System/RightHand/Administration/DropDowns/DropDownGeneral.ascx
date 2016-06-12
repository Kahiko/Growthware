<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.DropDownGeneral" Codebehind="DropDownGeneral.ascx.vb" %>
<table cellPadding=2 width="100%">
	<tr>
		<td class=Form_SubTitle align=center colSpan=2><B>General&nbsp;</B>
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Name:</span>
		</td>
		<td>
			<asp:textbox id="txtName" MaxLength="255" Runat="Server" CssClass="Form_Field"/>
			<asp:requiredfieldvalidator id=Requiredfieldvalidator5 Runat="Server" CssClass="Form_Message" ErrorMessage="Required" Text="(required)" Display="Dynamic" ControlToValidate="txtName">(required)</asp:requiredfieldvalidator>
			&nbsp;Note: Name should be unique but does not have to.
		</td>
	</tr>
</table>
