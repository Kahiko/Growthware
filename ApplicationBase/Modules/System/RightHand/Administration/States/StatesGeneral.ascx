<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.StatesGeneral" Codebehind="StatesGeneral.ascx.vb" %>
<table cellpadding="2" width="100%">
	<tr>
		<td colspan="2" class="Form_SubTitle" align="center">
			<B>General&nbsp; </B>
		</td>
	</tr>
	<TR>
		<td>
			<table>
				<tr>
					<TD>State</TD>
					<TD>
						<asp:Literal id="litState" runat="server"></asp:Literal>
					</TD>
				</tr>
				<tr>
					<td>
						<span class="Form_LabelText">Description: </span>
					</td>
					<td>
						<asp:TextBox id="txtDescription" MaxLength="50" CssClass="Form_Field" Runat="Server" />
						<asp:RequiredFieldValidator ControlToValidate="txtDescription" Display="Dynamic" Text="(required)" ErrorMessage="Required" CssClass="Form_Message" Runat="Server" ID="Requiredfieldvalidator1">(required)</asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td>
						Status:
					</td>
					<td>
						<asp:DropDownList ID="STATUS_SEQ_ID" Runat="server">
							<asp:ListItem Value="0">Active</asp:ListItem>
							<asp:ListItem Value="3">Inactive</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
			</table>
		</td>
	</TR>
</table>
