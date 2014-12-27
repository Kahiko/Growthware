<%@ Page Language="C#" AutoEventWireup="true" UnobtrusiveValidationMode="None" CodeBehind="EditRoleMembers.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Roles.EditRoleMembers" %>
<%@ Register Assembly="GrowthWare.WebSupport" Namespace="GrowthWare.WebSupport.CustomWebControls" TagPrefix="CustomWebControls" %>

<script type="text/javascript" language="javascript">
	function updateData() {
		var uiAccounts = $.map($('#ctlMembers_DstList option'), function (e) { return $(e).val(); });
		var accounts = {};
		accounts.Accounts = uiAccounts;
		var roleSeqId = parseInt($("#<%=txtEditID.ClientID %>").val());
		var theData = { roleSeqId: roleSeqId, accounts: accounts };
		return theData;
	}

	function saveMembers($dialogWindow) {
		var theData = updateData();
		GW.Common.debug(theData);
		var options = GW.Model.DefaultWebMethodOptions();
		options.async = false;
		options.data = theData;
		options.contentType = 'application/json; charset=utf-8';
		options.dataType = 'json';
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Roles/EditRoleMembers.aspx/InvokeSave"
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
<form id="form1" runat="server">
	<asp:TextBox ID="txtEditID" style="display: none;" runat="server"></asp:TextBox>
	<table border="0" cellpadding="0" cellspacing="0" width="1px">
		<tr>
			<td align="center">
				Role:&nbsp;<b><asp:Literal ID="litRole" runat="server"></asp:Literal></b>
			</td>
		</tr>
		<tr>
			<td align="left">
				<customwebcontrols:listpicker id="ctlMembers" size="200" rows="6" runat="server" selecteditemstext="Selected Members" allitemstext="All Members"></customwebcontrols:listpicker>
			</td>
		</tr>
	</table>
</form>
