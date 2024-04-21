﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchRoleResults.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Roles.SearchRoleResults" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title></title>
	<style type="text/css">
		noHeader
		{
			display:none;	
		}
	</style>
	<script type="text/javascript">
		GW.Search.RecordsReturned = $('#recordsReturned').val();
		$(document).ready(function () {
			GW.Search.setSortImage();
		});
	</script>
</head>
<body>
	<form id="form1" runat="server">
	<input id="recordsReturned" type="hidden" runat="server" />
	<div>
		<asp:GridView ID="searchResults" class="table table-striped table-bordered" Width="100%" OnRowDataBound="searchResults_RowDatabound" AutoGenerateColumns="False" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Both">
			<AlternatingRowStyle BackColor="#6699cc" />
			<Columns>
				<asp:TemplateField>
					<HeaderTemplate>
						<span>Edit</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="center">
									<img alt="Edit" id="btnDetails" src="Content/GrowthWare/Images/list-edit.png" style="height: 16px; width: 16px;" runat="server" />
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span>Members</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="center">
									<img alt="Edit" id="btnMembers" src="Content/GrowthWare/Images/list-edit.png" style="height: 16px; width: 16px;" runat="server" />
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span><a href="#" name="headerSort[Name]" onclick="javascript:GW.Search.toggleSort(this);">Name</a>
							<img alt="" name="headerSort[Name]" id="imgSort[Name]" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="center" style="white-space: nowrap;">
									<%# Eval("Name").ToString()%>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span><a href="#" name="headerSortDescription" onclick="javascript:GW.Search.toggleSort(this);">Description</a>
							<img alt="" name="headerSortDescription" id="imgSortDescription" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="center">
									<%# Eval("Description").ToString()%>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField HeaderText="System" DataField="Is_System" SortExpression="Is_System">
					<ItemStyle HorizontalAlign="Left"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField HeaderText="System Only" DataField="Is_System_Only" SortExpression="Is_System_Only">
					<ItemStyle HorizontalAlign="Left"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField HeaderText="Added By" DataField="Added_By" SortExpression="Added_By">
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField HeaderText="Added Date" DataField="Added_Date" SortExpression="Added_Date" DataFormatString="{0:d}">
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField HeaderText="Updated By" DataField="Updated_By" SortExpression="Updated_By">
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField HeaderText="Updated Date" DataField="Updated_Date" SortExpression="Updated_Date" DataFormatString="{0:d}">
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
			</Columns>
			<SelectedRowStyle ForeColor="#333333" Font-Bold="True" BackColor="#D1DDF1"></SelectedRowStyle>
			<EditRowStyle BackColor="#2461BF" />
			<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
			<HeaderStyle BackColor="#C7C7C7" />
			<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
			<RowStyle BackColor="#b6cbeb"></RowStyle>
			<SortedAscendingCellStyle BackColor="#F5F7FB" />
			<SortedAscendingHeaderStyle BackColor="#6D95E1" />
			<SortedDescendingCellStyle BackColor="#E9EBEF" />
			<SortedDescendingHeaderStyle BackColor="#4870BE" />
		</asp:GridView>
	</div>
	<div id="noResults" style="color: Red; border: thin #000000 solid; height: 100%;" runat="server">
		No results found
	</div>
	</form>
</body>
</html>
