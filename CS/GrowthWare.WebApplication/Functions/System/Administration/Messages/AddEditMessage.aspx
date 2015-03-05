<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEditMessage.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Messages.AddEditMessage" %>

<!DOCTYPE html>
<form id="frmAddEditMessage" runat="server">
    <div>
		<div class="pageDescription">
			In this page, you can add or edit messages, which can be used as error or general information text in response to an action by a user. For example, 'Invalid Password' or 'Please allow 24 hours for your submission to appear.'
		</div>
		<label id="ClientMSG" runat="server" style="font-weight: bold; color: red" />
		<input type="text" style="display: none" id="txtMessageSeqID" runat="server" />
		<table border="0" cellpadding="0" cellspacing="3" width="100%">
			<tr>
				<td>
					<table border="0" cellpadding="0" cellspacing="3">
						<tr>
							<td colspan="2">
								<span class="Form_LabelText">Name: </span>&nbsp;&nbsp;
								<asp:Label ID="lblName" runat="server" />
								<asp:TextBox ID="txtName" CssClass="rounded" Style="display: none;" runat="server" MaxLength="50" />
								<asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="failureNotification" runat="server" ErrorMessage="(required)" ControlToValidate="txtName"></asp:RequiredFieldValidator>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<table border="0" cellpadding="0" cellspacing="3">
						<tr>
							<td style="width: 330px">
								<span class="Form_LabelText">Description: </span>
							</td>
						</tr>
						<tr>
							<td style="width: 330px">
								<asp:TextBox ID="txtDescription" CssClass="rounded" runat="server" Height="49px" TextMode="MultiLine" Width="887px" MaxLength="128" onKeyPress="return GW.Common.Validation.textboxMultilineMaxNumber(this,128)" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<table border="0" cellpadding="0" cellspacing="3" width="98%">
						<tr>
							<td style="width:30px;">
								<span class="Form_LabelText">Title: </span>
							</td>
							<td>
								<span class="Form_LabelText">Avalible Tags: </span>
							</td>
						</tr>
						<tr>
							<td valign="top">
								<asp:TextBox ID="txtMessageTitle" CssClass="rounded" Columns="50" runat="Server" MaxLength="100" />
							</td>
							<td valign="top">
								<asp:TextBox ID="txtTags" CssClass="rounded" TextMode="MultiLine" Width="98%" Columns="15" Rows="5" runat="server" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<span class="Form_LabelText">Message Body:
						<asp:CheckBox ID="chkFormatAsHTML" Text="Format as HTML?" runat="server" />
					</span>
					<br />
					<asp:TextBox ID="txtMessageBody" CssClass="rounded" TextMode="MultiLine" Width="100%" Columns="50" Rows="16" runat="Server" />
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
        profile.Body = escape($("#<%=txtMessageBody.ClientID%>").val());
        profile.Description = $("#<%=txtDescription.ClientID %>").val();
        profile.FormatAsHtml = $("#<%=chkFormatAsHTML.ClientID %>").is(":checked");
        profile.Id = parseInt($("#<%=txtMessageSeqID.ClientID %>").val());
        profile.Name = $("#<%=txtName.ClientID %>").val();
        profile.Title = $("#<%=txtMessageTitle.ClientID %>").val();
        return profile;
	}

    function saveAddEditMessage($dialogWindow) {
        if (Page_ClientValidate()) {
            var profile = updateData();
            GW.Common.debug(profile);
            var options = GW.Model.DefaultWebMethodOptions();
            options.async = false;
            options.data = profile;
            options.contentType = 'application/json; charset=utf-8';
            options.dataType = 'json';
            options.url = GW.Common.getBaseURL() + "/gw/api/Messages/Save?Action=Search_Messages";
            GW.Common.JQueryHelper.callWeb(options, saveAddEditMessageSucess, saveAddEditMessageError);
            if (!($dialogWindow === undefined)) {
                $dialogWindow.dialog("destroy")
                $dialogWindow.remove();
            };
        }
    }

    function saveAddEditMessageSucess(xhr) {
        GW.Search.GetSearchResults();
    }


    function saveAddEditMessageError(xhr, status, error) {
        var mErrorException = JSON.parse(xhr.responseText);
        var mErrorMessage = 'Error getting content';
        mErrorMessage += '\nStatus: ' + status;
        mErrorMessage += '\nError: ' + error;
        mErrorMessage += '\nMessage: ' + mErrorException.ExceptionMessage;
        alert(mErrorMessage);
        GW.Search.GetSearchResults();
    }
</script>