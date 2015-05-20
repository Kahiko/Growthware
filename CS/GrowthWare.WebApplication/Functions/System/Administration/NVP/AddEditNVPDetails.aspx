<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEditNVPDetails.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.NVP.AddEditNVPDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<form id="frmAddEditNVPDetails" runat="server">
	<input type="hidden" id="hdnNVP_SEQ_ID" runat="server" />
	<input type="hidden" id="hdnNVP_SEQ_DET_ID" runat="server" />
	<div>
		<table>
			<tr>
				<td align="right" style="white-space: nowrap;">
					* Value :
				</td>
				<td>
					<input type="text" id="txtValue" class="rounded" runat="server" />
				</td>
				<td>
					<asp:RequiredFieldValidator ID="reqTxtValue" CssClass="failureNotification" ControlToValidate="txtValue" ErrorMessage="Required" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" style="white-space: nowrap;">
					* Text :
				</td>
				<td>
					<input type="text" id="txtText" class="rounded" runat="server" />
				</td>
				<td>
					<asp:RequiredFieldValidator ID="reqTxtText" CssClass="failureNotification" ControlToValidate="txtText" ErrorMessage="Required" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" style="white-space: nowrap;">
					Sort Order :
				</td>
				<td colspan="2">
					<input type="text" id="txtSortOrder" value="0" class="rounded" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right" style="white-space: nowrap;">
					Status :
				</td>
				<td colspan="s">
					<asp:DropDownList runat="server" CssClass="rounded" ID="dropStatusID">
						<asp:ListItem Value="1">Active</asp:ListItem>
						<asp:ListItem Value="2">Inactive</asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
		</table>
	</div>
</form>
<script language="javascript" type="text/javascript">

	$(document).ready(function () {
			
	});

	function saveAddEditNVPDetails($dialogWindow) {
		if (Page_ClientValidate()) {
			//alert('saving NVP Detail');
			var theData = updateData();
			GW.Common.debug(theData);
			var options = GW.Model.DefaultWebMethodOptions();
			options.async = false;
			options.data = theData;
			options.contentType = 'application/json; charset=utf-8';
			options.dataType = 'json';
			options.url = GW.Common.getBaseURL() + "/gw/api/NameValuePair/SaveNameValuePairDetail"
			GW.Common.JQueryHelper.callWeb(options, saveAddEditNVPDetailsSucess, saveAddEditNVPDetailsError);
			if (!($dialogWindow === undefined)) {
				$dialogWindow.dialog("destroy")
				$dialogWindow.remove();
			}
		}
	}

	function updateData() {
		var profile = {};
		profile.Value = $("#<%=txtValue.ClientID %>").val();
		profile.Text = $("#<%=txtText.ClientID %>").val();
		profile.NVP_Seq_ID = parseInt($("#<%=hdnNVP_SEQ_ID.ClientID %>").val());
		profile.NVP_SEQ_DET_ID = parseInt($("#<%=hdnNVP_SEQ_DET_ID.ClientID %>").val());
		profile.SortOrder = parseInt($("#<%=txtSortOrder.ClientID %>").val());
		profile.Status = parseInt($("#<%=dropStatusID.ClientID %> option:selected").val());
		return profile;
	}

	function saveAddEditNVPDetailsSucess(xhr) {
		GW.Search.GetSearchResults();
	}

	function saveAddEditNVPDetailsError(xhr, status, error) {

	}
</script>