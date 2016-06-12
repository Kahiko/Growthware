<%@ Control Language="vb" AutoEventWireup="false" Codebehind="SelectAccounts.ascx.vb" Inherits="BaseApplication.SelectAccounts" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<table class="pageDescription" width="100%">
	<tr>
		<td noWrap colSpan="3"><CustomWebControls:ALPHAPICKER id="AlphaPicker" Runat="Server"></CustomWebControls:ALPHAPICKER></td>
	</tr>
	<tr>
		<td>Records Per Page:
			<asp:textbox id="txtRecordsPerPage" Runat="server" AutoPostBack="True">10</asp:textbox></td>
		<td>Sort By:
			<asp:dropdownlist id="dropSortBy" Runat="Server" AutoPostBack="True">
				<asp:ListItem Text="Account" Value="0" />
				<asp:ListItem Text="Full Name" Value="1" />
			</asp:dropdownlist></td>
		<td>Order By:
			<asp:dropdownlist id="dropOrderBy" Runat="Server" AutoPostBack="True">
				<asp:ListItem Text="Ascending" value="0" />
				<asp:ListItem Text="Descending" value="1" />
			</asp:dropdownlist>
		</td>
	</tr>
	<tr>
		<td colspan="3">
			<table class="pageDescription" border="0" cellpadding="0" cellspacing="3">
				<tr>
					<td>
						Search: <asp:TextBox ID="txtSearch" Runat="server"></asp:TextBox>
					</td>
					<td>
						<asp:Button ID="btnSearch" Text=" Search " Runat="server"></asp:Button>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="center">
			<asp:datagrid 
				id="dgResults" 
				runat="server" 
				AllowPaging="True" 
				AlternatingItemStyle-CssClass="alternatingItemStyle" 
				ItemStyle-CssClass="itemStyle" 
				HeaderStyle-CssClass="headerItemStyle" 
				AutoGenerateColumns="False" 
				BorderStyle="None" 
				GridLines="None"
				CellPadding="5">
					<AlternatingItemStyle CssClass="alternatingItemStyle"></AlternatingItemStyle>
					<ItemStyle CssClass="itemStyle"></ItemStyle>
					<HeaderStyle CssClass="headerItemStyle"></HeaderStyle>
					<Columns>
						<asp:TemplateColumn>
							<HeaderTemplate>
								&nbsp; Edit &nbsp;
							</HeaderTemplate>
							<ItemTemplate>
								<table border="0" cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td align="center">
											<asp:HyperLink ImageUrl="AdmedEdit.gif" Text="Edit Account" NavigateUrl="SetByCodeBehind" Runat="server" ID="hyperEdit" />
										</td>
									</tr>
								</table>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn DataField="Account" HeaderText="Account" />
						<asp:BoundColumn DataField="FullName" HeaderText="Full Name" />
						<asp:TemplateColumn HeaderText="E-mail">
							<ItemTemplate>
								<table border="0" cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td align="center">
											<asp:Literal ID="EMail" Runat="server" />
										</td>
									</tr>
								</table>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
					<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
		</td>
	</tr>
</table>
