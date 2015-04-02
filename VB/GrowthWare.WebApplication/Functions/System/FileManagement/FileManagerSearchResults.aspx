<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FileManagerSearchResults.aspx.vb" Inherits="GrowthWare.WebApplication.FileManagerSearchResults" %>

<!DOCTYPE html>

<form id="frmFileManagerSearchResults" runat="server">
	<input id="recordsReturned" type="hidden" runat="server" />
	<div>
		<font style="color: red">
			<asp:Literal ID="litErrorMSG" runat="server" EnableViewState="true" Visible="False"></asp:Literal>
		</font>
		<asp:GridView ID="searchResults" Width="100%" AutoGenerateColumns="False" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Both">
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
						<span>Delete</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="center">
									<input type="checkbox" id="DeleteCheckBox" runat="server" name="DeleteCheckBox" />
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<a href="#" name="headerSortName" onclick="javascript:GW.Search.toggleSort(this);">Name</a>
						<img alt="" name="headerSortName" id="imgSortName" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="3px" cellspacing="0" width="100%">
							<tr>
								<td style="width: 16px;">
									<asp:Image ID="imgType" runat="server" BorderWidth="0" BorderStyle="None" />
								</td>
								<td align="left">
									<a id="lnkName" runat="server"></a>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField HeaderText="Type" DataField="Type" SortExpression="Type"></asp:BoundField>
				<asp:BoundField HeaderText="Size" DataField="Size" SortExpression="Size"></asp:BoundField>
				<asp:TemplateField>
					<HeaderTemplate>
						<span>
							<a href="#" name="headerSortModified" onclick="javascript:GW.Search.toggleSort(this);">Modified</a>
							<img alt="" name="headerSortModified" id="imgSortModified" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
						</span>
					</HeaderTemplate>
					<ItemTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="center">
									<%# Eval("Modified").ToString()%>
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