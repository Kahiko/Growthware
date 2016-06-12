<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.GroupsGeneral" Codebehind="GroupsGeneral.ascx.vb" %>
<asp:textbox id="txtGroup_seq_id" Runat="server" Visible="False"></asp:textbox>
<table cellPadding="2" width="100%">
	<tr>
		<td align="center" colSpan="2"><B>General&nbsp; </B>
		</td>
	</tr>
	<tr>
		<td align="center" colSpan="2"><asp:label id="lblError" Runat="server" ForeColor="Red"></asp:label></td>
	</tr>
	<tr>
		<td><span class="Form_LabelText">Group Name: </span>
		</td>
		<td><asp:literal id="litGroup" runat="server" visible="False"></asp:literal><asp:textbox id="txtGrpName" Runat="server" MaxLength="50"></asp:textbox><asp:literal id="litGroupWarning" Runat="server" visible="False"></asp:literal><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txtGrpName" ErrorMessage="(required)"
				Display="Dynamic" CssClass="Form_Message"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td><span class="Form_LabelText">Group Description: </span>
		</td>
		<td><asp:textbox id="txtGrpDescription" Runat="Server" MaxLength="50" CssClass="Form_Field"></asp:textbox></td>
	</tr>
	<tr>
		<td align="center" colSpan="2"><asp:button id="btnSave" Visible="False" runat="server" Text="Save"></asp:button></td>
	</tr>
</table>
