<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AddEditRoles.ascx.vb" Inherits="BaseApplication.AddEditRoles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table class="body" cellSpacing="0" cellPadding="3" border="0" width="100%">
	<tr>
		<td align="center">
			<asp:datagrid id="dgResults" Runat="Server" GridLines="Horizontal" CellPadding="5" BackColor="White"
				BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="false">
				<HeaderStyle Font-Bold="True" BackColor="#DCDDDE"></HeaderStyle>
				<AlternatingItemStyle BackColor="#eeeeee"></AlternatingItemStyle>
				<columns>
					<asp:BoundColumn DataField="ROLE_NAME" HeaderText="Role" ReadOnly="True" />
					<asp:BoundColumn DataField="DESCRIPTION" HeaderText="Description" />
					<asp:BoundColumn DataField="Is_System" ReadOnly="True" HeaderText="System" />
					<asp:EditCommandColumn ButtonType="LinkButton" HeaderText="Edit" EditText="Edit" UpdateText="Update" CancelText="Cancel" />
					<asp:TemplateColumn>
						<HeaderTemplate>
							&nbsp; Delete &nbsp;
						</HeaderTemplate>
						<ItemTemplate>
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td align="center">
										<asp:ImageButton id="btnDelete" CommandName="Delete" ImageUrl="~/UI/Default/Images/Delete.gif" AlternateText="Delete"
											Runat="Server" />
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<HeaderTemplate>
							&nbsp; Members &nbsp;
						</HeaderTemplate>
						<ItemTemplate>
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td align="center">
										<asp:HyperLink ImageUrl="AdmedEdit.gif" Text="Edit Module" NavigateUrl="SetByCodeBehind" Runat="server"
											ID="hyperMembers" />
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:TemplateColumn>
				</columns>
			</asp:datagrid>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:panel id="pnlAddRole" Runat="Server">
				<TABLE class="Form">
					<TR>
						<TD align="center" colSpan="2">Add Role</TD>
					<TR>
						<TD>Role Name:</TD>
						<TD>
							<asp:TextBox id="txtNewRole" Runat="Server" MaxLength="25" Width="190"></asp:TextBox>
							<asp:RequiredFieldValidator id="RequiredFieldValidator1" Runat="server" Text="*" ControlToValidate="txtNewRole"></asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD>Role Description:</TD>
						<TD>
							<asp:TextBox id="txtDescription" Runat="Server" MaxLength="25" Width="190" Columns="40"></asp:TextBox>
							<asp:RequiredFieldValidator id="RequiredFieldValidator2" Runat="server" Text="*" ControlToValidate="txtDescription"></asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD colSpan="2">
							<asp:Button id="btnAdd" Runat="Server" Text="Add Role"></asp:Button></TD>
					</TR>
				</TABLE>
			</asp:panel>
		</td>
	</tr>
</table>
