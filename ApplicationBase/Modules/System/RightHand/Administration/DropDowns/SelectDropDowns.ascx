<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.SelectDropDowns" Codebehind="SelectDropDowns.ascx.vb" %>
<table class="pageDescription" width="100%" border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td noWrap colSpan="3">
			<CustomWebControls:ALPHAPICKER id="AlphaPicker" Runat="Server"></CustomWebControls:ALPHAPICKER>
		</td>
	</tr>
	<tr>
		<td>Records Per Page:
			<asp:textbox id="txtRecordsPerPage" Runat="server" AutoPostBack="True">10</asp:textbox></td>
		<td><!--Sort By: -->
			<asp:dropdownlist Visible="False" id="dropSortBy" Runat="Server" AutoPostBack="True">
				<asp:ListItem Text="DESCRIPTION" Value="0" />
			</asp:dropdownlist></td>
		<td>Order By:
			<asp:dropdownlist id="dropOrderBy" Runat="Server" AutoPostBack="True">
				<asp:ListItem Text="Ascending" value="0" />
				<asp:ListItem Text="Descending" value="1" />
			</asp:dropdownlist></td>
	</tr>
	<tr>
		<td colspan="3">
			<table class="pageDescription" border="0" cellpadding="0" cellspacing="3">
				<tr>
					<td>
						Drop Down Name: <asp:TextBox ID="txtSearch" Runat="server"></asp:TextBox>
					</td>
					<td>
						<asp:Button ID="btnSearch" Text=" Search " Runat="server"></asp:Button>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<hr />
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="center">
			<span class="Form_Message">
				<asp:Literal ID="litNoData" Runat="server" Visible="False" Text="No Data Found"></asp:Literal>
			</span>
			<asp:datagrid id="dgResults" runat="server" CellPadding="5" GridLines="None" BorderStyle="None"
				AutoGenerateColumns="False" HeaderStyle-CssClass="headerItemStyle" ItemStyle-CssClass="itemStyle"
				AlternatingItemStyle-CssClass="alternatingItemStyle" AllowPaging="True">
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
										<asp:HyperLink ImageUrl="AdmedEdit.gif" Text="Edit Drop Box" NavigateUrl="SetByCodeBehind" Runat="server" ID="hyperEdit" />
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False">
						<HeaderTemplate>
							&nbsp; Delete &nbsp;
						</HeaderTemplate>
						<ItemTemplate>
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td align="center">
										<asp:ImageButton id="btnDelete" CommandName="Delete" ImageUrl="~/UI/Default/Images/Delete.gif" AlternateText="Delete" Runat="Server" />
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Description" HeaderText="Description" />
				</Columns>
				<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
		</td>
	</tr>
</table>
