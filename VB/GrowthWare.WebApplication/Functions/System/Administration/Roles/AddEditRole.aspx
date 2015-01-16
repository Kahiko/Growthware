<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddEditRole.aspx.vb" Inherits="GrowthWare.WebApplication.AddEditRole" %>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
    });

    function updateData() {
        var profile = {};
        profile = {};
        profile.IsSystem = false;
        profile.IsSystemOnly = false;
        if ($("#<%=chkIsSystem.ClientID %>").is(':checked')) {
		    profile.IsSystem = true;
		}
		if ($("#<%=chkIsSystemOnly.ClientID %>").is(':checked')) {
	        profile.IsSystemOnly = true;
	    }
	    profile.Id = parseInt($("#<%=txtRoleSeqId.ClientID %>").val());
	    profile.Name = $("#<%=txtRole.ClientID %>").val();
	    profile.Description = $("#<%=txtDescription.ClientID %>").val();
	    var theData = { profile: profile };
	    return theData;
	}

    function saveAddEdit($dialogWindow) {
        if (Page_ClientValidate()) {
            var theData = updateData();
            GW.Common.debug(theData);
            var options = GW.Model.DefaultWebMethodOptions();
            options.async = false;
            options.data = theData;
            options.contentType = 'application/json; charset=utf-8';
            options.dataType = 'json';
            options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Roles/AddEditRole.aspx/InvokeSave"
            GW.Common.JQueryHelper.callWeb(options);
            if (!($dialogWindow === undefined)) {
                $dialogWindow.dialog("destroy")
                $dialogWindow.remove();
            }
            GW.Search.GetSearchResults();
        }
	}
</script>
<div id="helpPopup" style="display: none;">
</div>
<form id="frmAddEditRole" runat="server">
	<table cellpadding="0" width="100%">
		<tr>
			<td>
			</td>
			<td class="ClientMsg" align="center">
				<b>
					<asp:Literal ID="litClientMsg" Visible="False" runat="server"></asp:Literal>
				</b>
			</td>
		</tr>
		<tr>
			<td>
				<table class="searchResults">
					<tr>
						<td align="center" colspan="2">
							<input id="txtRoleSeqId" style="display: none;" runat="server" />
						</td>
					</tr>
					<tr>
						<td>
							Role Name:
						</td>
						<td>
							<asp:TextBox ID="txtRole" runat="Server" TextMode="MultiLine" CssClass="rounded" MaxLength="128" onKeyPress="return GW.Common.Validation.textboxMultilineMaxNumber(this,128,event)" Width="190"></asp:TextBox>
							<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" ControlToValidate="txtRole"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
						<td>
							Role Description:
						</td>
						<td>
							<asp:TextBox ID="txtDescription" runat="Server" TextMode="MultiLine" Height="80px" CssClass="rounded" MaxLength="512" onKeyPress="return GW.Common.Validation.textboxMultilineMaxNumber(this,512,event)" Width="190" Columns="40"></asp:TextBox>
							<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Text="*" ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
						<td>
							<asp:CheckBox ID="chkIsSystem" CssClass="rounded" Checked="true" runat="Server" Text="Is a system role" />
						</td>
						<td>
							<asp:CheckBox ID="chkIsSystemOnly" CssClass="rounded" Checked="true" runat="Server" Text="Is only a system role" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</form>