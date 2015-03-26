<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditGroupMembers.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Groups.EditGroupMembers" %>
<%@ Register Assembly="GrowthWare.WebSupport" Namespace="GrowthWare.WebSupport.CustomWebControls" TagPrefix="CustomWebControls" %>

<!DOCTYPE html>

<form id="frmEditGroupMembers" runat="server">
	<div>
		<asp:TextBox ID="txtEditID" Style="display: none;" runat="server"></asp:TextBox>
		<table border="0" cellpadding="0" cellspacing="0" width="1px">
			<tr>
				<td align="center">
					Group:&nbsp;<b><asp:Literal ID="litGroup" runat="server"></asp:Literal></b>
				</td>
			</tr>
			<tr>
				<td align="left">
					<CustomWebControls:ListPicker ID="ctlMembers" Size="200" Rows="6" runat="server" SelectedItemsText="Selected Roles" AllItemsText="All Roles"></CustomWebControls:ListPicker>
				</td>
			</tr>
		</table>
	</div>
</form>
<script type="text/javascript" language="javascript">
    function updateData() {
        var uiRoles = $.map($('#ctlMembers_DstList option'), function (e) { return $(e).val(); });
        var roles = {};
        roles.Roles = uiRoles;
        var groupSeqId = parseInt($("#<%=txtEditID.ClientID %>").val());
        var theData = { groupSeqId: groupSeqId, roles: roles };
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
        options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Groups/EditGroupMembers.aspx/InvokeSave"
        GW.Common.JQueryHelper.callWeb(options, saveMembersSucess, saveMembersError);
        if (!($dialogWindow === undefined)) {
            $dialogWindow.dialog("destroy")
            $dialogWindow.remove();
        }
    }

    function saveMembersSucess() {
        GW.Common.debug('done');
    }

    function saveMembersError() {
        GW.Common.debug('error');
    }
</script>