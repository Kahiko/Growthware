<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.SelectModules" Codebehind="SelectModules.ascx.vb" %>
<script type="text/javascript">
function doWindowOnLoad(){
	oButtonArray[oButtonArray.length] = document.getElementById('ctl00_RightHandModulesLoader_ctl00_btnSearch'); // Add button to the disable array
	enableButtions();
	setFocus('ctl00_RightHandModulesLoader__ctl00_txtSearch');
	var txtSearch = document.getElementById('ctl00_RightHandModulesLoader_ctl00_txtSearch');
	txtSearch.select();
	deleteCookie('selectedTab');
}
</script>
<table class="pageDescription" width="100%" border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td align="left">
			<table class="pageDescription" border="0" cellpadding="0" cellspacing="3">
	            <tr>
		            <td colspan="3">
			            <CustomWebControls:ALPHAPICKER id="AlphaPicker" runat="Server"></CustomWebControls:ALPHAPICKER>
		            </td>
	            </tr>
			    <tr>
			        <td>
			            Search Text:
			        </td>
			        <td>
			            Search In Column:
			        </td>
			        <td>
			            &nbsp;
			        </td>
			    </tr>
			    <tr>
			        <td>
			            <asp:TextBox ID="txtSearch" Runat="server"></asp:TextBox>
			        </td>
			        <td>
			            <asp:dropdownlist Visible="True" id="dropSearchBy" Runat="Server" AutoPostBack="True">
				            <asp:ListItem Text="Name" Value="Name" />
				            <asp:ListItem Text="Description" Value="Description" />
				            <asp:ListItem Text="Action" Value="Action" />
			            </asp:dropdownlist>
			        </td>
			        <td>
			            <asp:Button ID="btnSearch" Text=" Search " Runat="server"></asp:Button>
			        </td>
			    </tr>
                <tr>
                    <td colspan="4">
                        Records Per Page:&nbsp;<asp:textbox id="txtRecordsPerPage" Runat="server" AutoPostBack="True">10</asp:textbox>
                    </td>
                </tr>
			</table>
		</td>
	</tr>
</table>
<hr />
<table border="0" cellpadding="0" cellspacing="3" width="100%">
	<tr>
		<td align="left">
			<span class="Form_Message">
				<asp:Literal ID="litNoData" Runat="server" Visible="False" Text="No Data Found"></asp:Literal>
			</span>
			<asp:datagrid id="dgResults" runat="server" CssClass="searchResults" CellPadding="4" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ForeColor="#333333" GridLines="Both">
				<Columns>
					<asp:TemplateColumn>
						<HeaderTemplate>
						    <table border="0" cellpadding="0" cellspacing="0" width="30%">
						        <tr>
						            <td>
						                Details
						            </td>
						        </tr>
						    </table>
						</HeaderTemplate>
						<ItemTemplate>
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td align="center">
										<asp:HyperLink ImageUrl="~/Images/AdminEdit.gif" Text="Details" NavigateUrl="" Runat="server" ID="hyperEdit" />
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<HeaderTemplate>
						    <table border="0" cellpadding="0" cellspacing="0" width="30%">
						        <tr>
						            <td>
						                Delete
						            </td>
						        </tr>
						    </table>
						</HeaderTemplate>
						<ItemTemplate>
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td align="center">
										<asp:ImageButton id="btnDelete" CommandName="Delete" ImageUrl="~/Images/Delete.gif" AlternateText="Delete" Runat="Server" />
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn SortExpression="Name" ItemStyle-Wrap="false" DataField="Name" HeaderText="Module Name" />
					<asp:BoundColumn SortExpression="Description" ItemStyle-Wrap="false" DataField="Description" HeaderText="Module Description" />
					<asp:BoundColumn SortExpression="Action" ItemStyle-Wrap="false" DataField="Action" HeaderText="Action" />
				</Columns>
                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <EditItemStyle BackColor="#2461BF" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Left" Mode="NumericPages" Position="TopAndBottom" />
                <AlternatingItemStyle BackColor="White" />
                <ItemStyle BackColor="#EFF3FB" Wrap="false" HorizontalAlign="left" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
			</asp:datagrid>
		</td>
	</tr>
</table>