<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.AddEditBusinessUnit" Codebehind="AddEditBusinessUnit.ascx.vb" %>
<table cellpadding="2" width="100%">
	<tr>
		<td colspan="2" class="ClientMsg" align="center">
			&nbsp;
			<B><asp:Literal ID="litClientMsg" Visible="False" Runat="server"></asp:Literal></B>
		</td>
	</tr>
	<TR>
		<td>
			<table>
				<tr>
					<td align="right">
						<asp:Literal ID="litBusinessUnitTranslation" Runat="server"></asp:Literal>:&nbsp;
					</td>
					<td align="left">
						<asp:Literal id="litBusinessUnit" runat="server"></asp:Literal>
						<asp:TextBox ID="txtBusinessUnit" MaxLength="50" Visible="False" Runat="server"></asp:TextBox>
						<asp:RequiredFieldValidator ControlToValidate="txtBusinessUnit" Display="Dynamic" Text="(required)" ErrorMessage="Required" CssClass="Form_Message" Runat="Server" ID="Requiredfieldvalidator2">(required)</asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td align="right">
						<span class="Form_LabelText">Description: </span>
					</td>
					<td align="left">
						<asp:TextBox id="txtDescription" MaxLength="50" CssClass="Form_Field" Runat="Server" />
						<asp:RequiredFieldValidator ControlToValidate="txtDescription" Display="Dynamic" Text="(required)" ErrorMessage="Required" CssClass="Form_Message" Runat="Server" ID="Requiredfieldvalidator1">(required)</asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td align="right">
						<span class="Form_LabelText">Connection String: </span>
					</td>
					<td align="left">
						<asp:TextBox id="txtConnectionString" MaxLength="512" CssClass="Form_Field" Runat="Server" />
					</td>
				</tr>
				<tr>
					<td align="right">
						<span class="Form_LabelText">Data Access Layer Name: </span>
					</td>
					<td align="left">
						<asp:TextBox ID="txtDAL" runat="Server" CssClass="Form_Field" MaxLength="512">
						</asp:TextBox>
					</td>
				</tr>
				<tr>
					<td align="right">
						<span class="Form_LabelText">Parent: </span>
					</td>
					<td align="left">
						<asp:DropDownList ID="dropParent" Runat="server">
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right">
						<span class="Form_LabelText">Skin: </span>
					</td>
					<td align="left">
						<asp:DropDownList ID="dropSkin" Runat="server">
							
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right">
						<span class="Form_LabelText">Status: </span>
					</td>
					<td align="left">
						<asp:DropDownList ID="STATUS_SEQ_ID" Runat="server">
							<asp:ListItem Value="0">Active</asp:ListItem>
							<asp:ListItem Value="3">Inactive</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td align="right">
						<label>Directory:&nbsp;</label>
					</td>
					<td align="left">
						<asp:TextBox ID="txtDirectory" MaxLength="255" Runat="server"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td align="right">
						<label>Requries Impersonation:&nbsp;</label>
					</td>
					<td align="left">
						<asp:CheckBox ID="chkImpersonation" Runat="server"></asp:CheckBox>
					</td>
				</tr>
				<tr>
					<td align="right">
						<label>Account:&nbsp;</label>
					</td>
					<td align="left">
						<asp:TextBox ID="txtAccount" MaxLength="50" Runat="server"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td align="right">
						<label>Password:&nbsp;</label>
					</td>
					<td align="left">
						<asp:TextBox ID="txtPassword" MaxLength="50" TextMode="Password" Runat="server"></asp:TextBox>&nbsp;!Leave blank to keep the same password!
						<asp:TextBox ID="txtHidPwd" Visible="False" Runat="server"></asp:TextBox>
					</td>
				</tr>
			</table>
		</td>
	</TR>
	<tr>
		<td align="center">
			<asp:Button ID="btnSave" Text="&nbsp;Save&nbsp;" Runat="server"></asp:Button>
		</td>
	</tr>
</table>
