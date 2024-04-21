<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditRoleMembers.aspx.vb" Inherits="GrowthWare.WebApplication.EditRoleMembers" %>
<%@ Register Assembly="GrowthWare.WebSupport" Namespace="GrowthWare.WebSupport.CustomWebControls" TagPrefix="CustomWebControls" %>

<script type="text/javascript" language="javascript">
	function updateData() {
	    var uiAccounts = $.map($('#ctlMembers_DstList option'), function (e) { return $(e).val(); });
	    var accounts = {};
	    accounts.Accounts = uiAccounts;
	    var roleSeqId = parseInt($("#<%=txtEditID.ClientID %>").val());
	    accounts.RoleSeqId = roleSeqId;
	    return accounts;
	}

	function saveMembers($dialogWindow) {
		var theData = updateData();
		GW.Common.debug(theData);
		var options = GW.Model.DefaultWebMethodOptions();
		options.async = false;
		options.data = theData;
		options.contentType = 'application/json; charset=utf-8';
		options.dataType = 'json';
		options.url = GW.Common.getBaseURL() + "/gw/api/Roles/SaveMembers";
		GW.Common.JQueryHelper.callWeb(options, saveMembersSucess, saveMembersError);
		if (!($dialogWindow === undefined)) {
			$dialogWindow.dialog("destroy")
			$dialogWindow.remove();
		}
	}

	function saveMembersSucess() {

	}

	function saveMembersError() {

	}
</script>
<form id="frmEditRoleMembers" runat="server">
	<asp:TextBox ID="txtEditID" Style="display: none;" runat="server"></asp:TextBox>
	<table border="0" cellpadding="0" cellspacing="0" width="1px">
		<tr>
			<td align="center">
				Role:&nbsp;<b><asp:Literal ID="litRole" runat="server"></asp:Literal></b>
			</td>
		</tr>
		<tr>
			<td align="left">
				<CustomWebControls:ListPicker ID="ctlMembers" Size="200" Rows="6" runat="server" SelectedItemsText="Selected Members" AllItemsText="All Members"></CustomWebControls:ListPicker>
			</td>
		</tr>
	</table>
</form>