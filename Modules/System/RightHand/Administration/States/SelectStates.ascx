<%@ Control Language="vb" AutoEventWireup="false" Codebehind="SelectStates.ascx.vb" Inherits="BaseApplication.SelectStates" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<table class=pageDescription width="100%">
	<tr>
		<td noWrap colSpan=3>
			<CustomWebControls:ALPHAPICKER id=AlphaPicker Runat="Server"></CustomWebControls:ALPHAPICKER>
		</td>
	</tr>
	<tr>
		<td>
			Records Per Page: <asp:textbox id=txtRecordsPerPage Runat="server" AutoPostBack="True">10</asp:textbox>
		</td>
		<td>
			Sort By: 
			<asp:dropdownlist id=dropSortBy Runat="Server" AutoPostBack="True">
				<asp:ListItem Text="State" Value="0" />
				<asp:ListItem Text="Description" Value="1" />
			</asp:dropdownlist>
		</td>
		<td>
			Order By: 
			<asp:dropdownlist id=dropOrderBy Runat="Server" AutoPostBack="True">
				<asp:ListItem Text="Ascending" value="0" />
				<asp:ListItem Text="Descending" value="1" />
			</asp:dropdownlist>
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
				<HeaderStyle CssClass="headerItemStyle">
				</HeaderStyle>
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
					<asp:BoundColumn DataField="State" HeaderText="State" />
					<asp:BoundColumn DataField="LongName" HeaderText="Description" />
				</Columns>
				<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
		</td>
	</tr>
</table>
