<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddEditGroup.aspx.vb" Inherits="GrowthWare.WebApplication.AddEditGroup" %>

<!DOCTYPE html>

<form id="frmAddEditGroup" runat="server">
	<div>
		<table class="searchResults">
			<tr>
				<td align="center" colspan="2">
					<input id="txtGroupSeqId" style="display: none;" runat="server" />
				</td>
			</tr>
			<tr>
				<td>
					Group Name:
				</td>
				<td>
					<asp:TextBox ID="txtGroup" runat="Server" TextMode="MultiLine" CssClass="rounded" MaxLength="128" onKeyPress="return GW.Common.Validation.textboxMultilineMaxNumber(this,128)" Width="190"></asp:TextBox>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" ControlToValidate="txtGroup"></asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td>
					Group Description:
				</td>
				<td>
					<asp:TextBox ID="txtDescription" runat="Server" TextMode="MultiLine" Height="80px" CssClass="rounded" MaxLength="512" onKeyPress="return GW.Common.Validation.textboxMultilineMaxNumber(this,512)" Width="190" Columns="40"></asp:TextBox>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Text="*" ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
				</td>
			</tr>
		</table>    
	</div>
</form>
	<script type="text/javascript" language="javascript">
		$(document).ready(function () {
		});

		function updateData() {
			var profile = {};
			profile.Id = parseInt($("#<%=txtGroupSeqId.ClientID %>").val());
			profile.Name = $("#<%=txtGroup.ClientID %>").val();
			profile.Description = $("#<%=txtDescription.ClientID %>").val();
			//var theData = { profile: profile };
		    //return theData;
		    return profile;
		}

		function saveAddEdit($dialogWindow) {
			var theData = updateData();
			// profile is defined in AddEditAccounts.aspx
			GW.Common.debug(theData);
			var options = GW.Model.DefaultWebMethodOptions();
			options.async = false;
			options.data = theData;
			options.contentType = 'application/json; charset=utf-8';
			options.dataType = 'json';
			//options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Groups/AddEditGroup.aspx/InvokeSave"
			options.url = GW.Common.getBaseURL() + "/gw/api/Groups/Save";
			GW.Common.JQueryHelper.callWeb(options);
			if (!($dialogWindow === undefined)) {
				$dialogWindow.dialog("destroy")
				$dialogWindow.remove();
			};
			GW.Search.GetSearchResults();
		}
	</script>