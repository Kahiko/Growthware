<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchNVPResultsChildren.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.NVP.SearchNVPResultsChildren" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<form id="frmSearchNVPResultsChildren" runat="server">
	<input id="recordsReturned" type="hidden" runat="server" />
	<div>
		<asp:GridView ID="searchResults" Width="100%" AutoGenerateColumns="False" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Both" OnRowDataBound="searchResults_RowDatabound">
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
									<img alt="Edit" id="btnDetails" src="Public/GrowthWare/Images/list-edit.png" style="height: 16px; width: 16px;" runat="server" />
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span><a href="#" name="headerSortNVP_DET_VALUE" onclick="javascript:GW.Search.toggleSort(this);">Name</a>
							<img alt="" name="headerSortNVP_DET_VALUE" id="imgSortNVP_DET_VALUE" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="center">
									<%# Eval("NVP_DET_VALUE").ToString()%>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span><a href="#" name="headerSortNVP_Detail_Name" onclick="javascript:GW.Search.toggleSort(this);">Value</a>
							<img alt="" name="headerSortNVP_DET_TEXT" id="imgSortNVP_DET_TEXT" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="center">
									<%# Eval("NVP_DET_TEXT").ToString()%>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span><a href="#" name="headerSortAdded_By" onclick="javascript:GW.Search.toggleSort(this);">Added By</a>
							<img alt="" name="headerSortAdded_By" id="imgSortAdded_By" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="Right">
									<%# Eval("Added_By").ToString()%>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span><a href="#" name="headerSortAdded_Date" onclick="javascript:GW.Search.toggleSort(this);">Added Date</a>
							<img alt="" name="headerSortAdded_Date" id="imgSortAdded_Date" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="Right">
									<%# Eval("Added_Date").ToString()%>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span><a href="#" name="headerSortUpdated_By" onclick="javascript:GW.Search.toggleSort(this);">Updated By</a>
							<img alt="" name="headerSortUpdated_By" id="imgSortUpdated_By" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="Right">
									<%# Eval("Updated_By").ToString()%>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span><a href="#" name="headerSortUpdated_Date" onclick="javascript:GW.Search.toggleSort(this);">Updated Date</a>
							<img alt="" name="headerSortUpdated_Date" id="imgSortUpdated_Date" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="Right">
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
<script type="text/javascript">
	GW.Search.RecordsReturned = $('#recordsReturned').val();
	$(document).ready(function () {
		GW.Search.setSortImage();
	});
</script>