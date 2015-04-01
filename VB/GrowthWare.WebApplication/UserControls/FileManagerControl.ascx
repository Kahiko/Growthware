<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="FileManagerControl.ascx.vb" Inherits="GrowthWare.WebApplication.FileManagerControl" %>
<%@ Register Src="Search.ascx" TagName="Search" TagPrefix="ucSearch" %>
<%@ Register Src="UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<script type="text/javascript">
	$(document).ready(function () {

	});
</script>
<font style="color: red">
	<asp:Literal ID="litErrorMSG" runat="server" EnableViewState="true" Visible="False"></asp:Literal>
</font>
<table id="Table1" cellspacing="0" cellpadding="0" width="90%" border="0">
	<tr style="font-size: 8pt; font-family: verdana; height: 20px;" align="left">
		<td align="left" valign="middle" style="white-space: nowrap;" colspan="2">
			Select Directory:&nbsp;<div id="directorySelector" style="position: relative; display: inline-block;" runat="server">
			</div>
			&nbsp;&nbsp; <b>
				<asp:Label ID="lblClientMSG" runat="server" Visible="False"></asp:Label>
			</b>
		</td>
	</tr>
	<tr id="firstRow" style="font-size: 8pt; font-family: verdana; height: 18px;" runat="server">
		<td valign="middle" style="white-space: nowrap;" align="left">
			<uc1:UploadControl ID="UploadControl" runat="server" />
		</td>
		<td id="tdNewDirectory" valign="middle" style="white-space: nowrap;" align="right" width="286px" runat="server">
			<asp:Literal ID="CreateNewDirectory" runat="server" Text="Create New Directory&nbsp;&nbsp;"></asp:Literal>
			<asp:TextBox ID="txtNewDirectory" CssClass="rounded" runat="server" EnableViewState="True"></asp:TextBox>
			<img alt="New Folder" onclick="javascript:GW.FileManager.createDirectory();" src="Public/GrowthWare/Images/new_folder.png" height="16px" width="16px" />
		</td>
	</tr>
	<tr style="font-size: 8pt; font-family: verdana">
		<td colspan="2">
			<ucSearch:Search ID="SearchControl" ShowDeleteAll="true" ShowSelect="true" runat="server" />
		</td>
	</tr>
</table>
<asp:Label ID="literalPath" runat="server" Visible="False"></asp:Label>