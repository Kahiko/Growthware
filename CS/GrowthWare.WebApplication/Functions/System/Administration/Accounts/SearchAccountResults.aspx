<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchAccountResults.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Accounts.SearchAccountResults" %>

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
			<asp:GridView class="table table-striped table-bordered" ID="searchResults" Width="100%"
				AutoGenerateColumns="False" runat="server" CellPadding="4" ForeColor="#333333" 
				GridLines="Both"
                 OnRowDataBound="searchResults_RowDatabound">
				<AlternatingRowStyle BackColor="#6699cc" />
				<Columns>
					<asp:TemplateField>
						<HeaderTemplate>
							<span>Details</span>
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
										<img alt="Delete" id="btnDelete" src="Public/GrowthWare/Images/delete_red.png" style="height: 16px; width: 16px;" runat="server" />
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<HeaderTemplate>
							<span><a href="#" name="headerSortAccount" onclick="javascript:GW.Search.toggleSort(this);">Account</a>
								<img alt="" name="headerSortAccount" id="imgSortAccount" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
							</span>
						</HeaderTemplate>
						<ItemTemplate>
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td align="center">
										<%# Eval("Account").ToString()%>
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<HeaderTemplate>
							<span><a href="#" name="headerSortFIRST_NAME" onclick="javascript:GW.Search.toggleSort(this);">First Name</a>
								<img alt="" name="headerSortFIRST_NAME" id="imgSortFIRST_NAME" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
							</span>
						</HeaderTemplate>
						<ItemTemplate>
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td align="center">
										<%# Eval("FIRST_NAME").ToString()%>
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<HeaderTemplate>
							<span><a href="#" name="headerSortLAST_NAME" onclick="javascript:GW.Search.toggleSort(this);">Last Name</a>
								<img alt="" name="headerSortLAST_NAME" id="imgSortLAST_NAME" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
							</span>
						</HeaderTemplate>
						<ItemTemplate>
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td align="center">
										<%# Eval("LAST_NAME").ToString()%>
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<HeaderTemplate>
							<span><a href="#" name="headerSortEMail" onclick="javascript:GW.Search.toggleSort(this);">E-Mail</a>
								<img alt="" name="headerSortEMail" id="imgSortEMail" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
							</span>
						</HeaderTemplate>
						<ItemTemplate>
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td align="center">
										<%# Eval("EMail").ToString()%>
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
							<span><a href="#" name="headerSortLAST_LOGIN" onclick="javascript:GW.Search.toggleSort(this);">Last Logged On</a>
								<img alt="" name="headerSortLAST_LOGIN" id="imgSortLAST_LOGIN" onclick="javascript:GW.Search.toggleSort(this);" style="height: 16px; width: 16px;" />
							</span>
						</HeaderTemplate>
						<ItemTemplate>
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td align="Right">
										<%# Eval("LAST_LOGIN").ToString()%>
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