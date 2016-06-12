<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AddEditWorkFlow.ascx.vb" Inherits="BaseApplication.AddEditWorkFlow" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellPadding="2" width="100%">
	<tr>
		<td class="ClientMsg" align="center" colSpan="2">&nbsp; <B>
				<asp:literal id="litClientMsg" Runat="server" Visible="False"></asp:literal></B></td>
	</tr>
	<TR>
		<td>
			<table>
				<tr>
					<td align="right">Seq. ID:
					</td>
					<td><asp:literal id="litWorkFlowSeqID" Runat="server" Visible="False"></asp:literal></td>
				</tr>
				<tr>
					<TD align="right">Work Flow Name:
					</TD>
					<TD><asp:textbox id="txtWorkFlowName" Runat="server" MaxLength="50"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator2" Runat="Server" CssClass="Form_Message" ErrorMessage="Required"
							Text="(required)" Display="Dynamic" ControlToValidate="txtWorkFlowName">(required)</asp:requiredfieldvalidator></TD>
				</tr>
				<tr>
					<td align="right"><span class="Form_LabelText">Order: </span>
					</td>
					<td><asp:textbox id="txtOrder_ID" Runat="Server" MaxLength="50" CssClass="Form_Field"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator1" Runat="Server" CssClass="Form_Message" ErrorMessage="Required"
							Text="(required)" Display="Dynamic" ControlToValidate="txtOrder_ID">(required)</asp:requiredfieldvalidator></td>
				</tr>
				<tr>
					<td align="right"><span class="Form_LabelText">Acton: </span>
					</td>
					<td><asp:dropdownlist id="dropAction" Runat="server"></asp:dropdownlist></td>
				</tr>
			</table>
		</td>
	</TR>
	<tr>
		<td align="center"><asp:button id="btnSave" Runat="server" Text="&nbsp;Save&nbsp;"></asp:button></td>
	</tr>
</table>
