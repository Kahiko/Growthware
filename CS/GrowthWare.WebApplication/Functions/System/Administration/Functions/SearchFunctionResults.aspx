<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchFunctionResults.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Functions.SearchFunctionResults" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
		<asp:GridView ID="searchResults" class="table table-striped table-bordered" Width="100%" OnRowDatabound="searchResults_RowDatabound" AutoGenerateColumns="False" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Both">
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
						<span style="white-space: nowrap;"><a href="#" name="headerSortName" onclick="javascript:GW.Search.toggleSort(this);">Name</a>
							<img alt="" name="headerSortName" id="imgSortName" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
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
						<span style="white-space: nowrap;">
							<a href="#" name="headerSortDescription" onclick="javascript:GW.Search.toggleSort(this);">Description</a>
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
				<asp:TemplateField>
					<HeaderTemplate>
						<span style="white-space: nowrap;"><a href="#" name="headerSortAction" onclick="javascript:GW.Search.toggleSort(this);">Action</a>
							<img alt="" name="headerSortAction" id="imgSortAction" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="center">
									<%# Eval("Action").ToString()%>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span style="white-space: nowrap;"><a href="#" name="headerSortAdded_By" onclick="javascript:GW.Search.toggleSort(this);">Added By</a>
							<img alt="" name="headerSortAdded_By" id="imgSortAdded_By" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="right">
									<%# Eval("Added_By").ToString()%>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span style="white-space: nowrap;"><a href="#" name="headerSortAdded_Date" onclick="javascript:GW.Search.toggleSort(this);">Added Date</a>
							<img alt="" name="headerSortAdded_Date" id="imgSortAdded_Date" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="right" style="white-space: nowrap;">
									<%# Eval("Added_Date").ToString()%>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span style="white-space: nowrap;"><a href="#" name="headerSortUpdated_By" onclick="javascript:GW.Search.toggleSort(this);" style="white-space: nowrap;">Updated By</a>
							<img alt="" name="headerSortUpdated_By" id="imgSortUpdated_By" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="right" style="white-space: nowrap;">
									<%# Eval("Updated_By").ToString()%>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span style="white-space: nowrap;"><a href="#" name="headerSortUpdated_Date" onclick="javascript:GW.Search.toggleSort(this);" style="white-space: nowrap;">Updated Date</a>
							<img alt="" name="headerSortUpdated_Date" id="imgSortUpdated_Date" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="right" style="white-space: nowrap;">
									<%# Eval("Updated_Date").ToString()%>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
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