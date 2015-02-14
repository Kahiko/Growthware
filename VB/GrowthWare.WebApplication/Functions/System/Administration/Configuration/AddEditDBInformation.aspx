<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddEditDBInformation.aspx.vb" Inherits="GrowthWare.WebApplication.AddEditDBInformation" %>

<!DOCTYPE html>
<form id="frmAddEditDBInformation" runat="server">
	<div>
		<asp:Table ID="tblInformation" runat="server">
			<asp:TableRow>
				<asp:TableCell HorizontalAlign="right">
					Version:&nbsp;
				</asp:TableCell>
				<asp:TableCell HorizontalAlign="left">
					<asp:Label ID="lblVersion" runat="server"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell HorizontalAlign="right">
					Enable Inheritance:&nbsp;
				</asp:TableCell>
				<asp:TableCell HorizontalAlign="left">
					<asp:DropDownList ID="dropEnableInheritance" Width="120px" CssClass="rounded" runat="server">
						<asp:ListItem Value="1" Text="True"></asp:ListItem>
						<asp:ListItem Value="0" Text="False"></asp:ListItem>
					</asp:DropDownList>
					<br />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell ColumnSpan="2" HorizontalAlign="Right">
					<input type="button" value="Save" id="btnSave" runat="server" />
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>    
	</div>
</form>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        var btnSave = $("#btnSave");
        if (typeof jQuery.ui != 'undefined') {
            btnSave.button();
        }
		btnSave.click(function () {
			saveProfile();
		});

	});

	function saveProfile() {
		var data = '{ "enableInheritance" : "' + $("#<%=dropEnableInheritance.ClientID %> option:selected").val() + '" }'
		var options = GW.Model.DefaultWebMethodOptions();
		options.async = false;
		options.data = data;
		options.contentType = 'application/json; charset=utf-8';
		options.dataType = 'json';
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Configuration/AddEditDBInformation.aspx/InvokeSave"
		GW.Common.JQueryHelper.callWeb(options, saveSuccess, saveError);
	}

	function saveSuccess(xhr) {
		alert('Infomration has been saved');

	}

	function saveError(xhr, status, error) {
		alert('status: ' + status + ' error: ' + error);
	}
</script>
