<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChangePassword.aspx.vb" Inherits="GrowthWare.WebApplication.ChangePassword" %>
<!DOCTYPE html>

<div id="clientMessage" class="Form_Message" style="display: none" runat="server"></div>
<body>
    <form id="frmChangePassword" runat="server">
	    <div>
		    <table border="0" cellpadding="3" cellspacing="3" style="font: 8pt verdana, arial;">
			    <tr id="trOldPassword" runat="server">
				    <td align="right" valign="top">
					    <span class="Form_LabelText">Old Password&nbsp;:</span>
				    </td>
				    <td align="left">
					    <input type="password" class="rounded" id="OldPassword" runat="server" />
					    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="Form_Message" SetFocusOnError="true" ErrorMessage="(required)" ControlToValidate="OldPassword" runat="server" />
				    </td>
			    </tr>
			    <tr>
				    <td align="right" valign="top">
					    <span class="Form_LabelText">New Password&nbsp;:</span>
				    </td>
				    <td align="left">
					    <input type="password" class="rounded" id="NewPassword" runat="server" />
					    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="Form_Message" Display="dynamic" SetFocusOnError="true" ControlToValidate="NewPassword" ErrorMessage="(required)" runat="server" />
					    <asp:CompareValidator ID="CompareValidator1" CssClass="Form_Message" ControlToCompare="OldPassword" Display="dynamic" ControlToValidate="NewPassword" Operator="NotEqual" ErrorMessage="New password can not match the old password." runat="server" />
				    </td>
			    </tr>
			    <tr>
				    <td align="right" valign="top">
					    <span class="Form_LabelText">Confirm Password&nbsp;:</span>
				    </td>
				    <td align="left">
					    <input type="password" class="rounded" id="ConfirmPassword" runat="server" />
					    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="Form_Message" SetFocusOnError="true" ErrorMessage="(required)" ControlToValidate="ConfirmPassword" runat="server" />
					    <asp:CompareValidator ID="CompareValidator2" CssClass="Form_Message" ControlToCompare="NewPassword" Display="dynamic" ControlToValidate="ConfirmPassword" Operator="Equal" ErrorMessage="New password and confirm password must match." runat="server" />
				    </td>
			    </tr>
			    <tr>
				    <td colspan="2" align="left">
					    <input type="button" id="btnChangePassword" class="btn btn-primary" value="Change Password" onclick="javascript: changePassword();" />
				    </td>
			    </tr>
			    <tr id="trNormalChange" runat="server">
				    <td colspan="2" align="left">
					    <b>Please Note:</b>
					    <ul>
						    <li>All fields are required </li>
						    <li>"Old password" can not match new password </li>
						    <li>"New Password" and "Confirm Password" must match </li>
					    </ul>
				    </td>
			    </tr>
			    <tr id="trForceChange" runat="server">
				    <td colspan="2" align="left">
					    <b>Please Note:</b>
					    <ul>
						    <li>All fields are required </li>
						    <li>"New Password" and "Confirm Password" must match </li>
					    </ul>
				    </td>
			    </tr>
		    </table>
	    </div>
    </form>
</body>

<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        if (typeof jQuery.ui != 'undefined') {
            $("#btnChangePassword").button();
        }
	    if ($('input:password')) {
	        //$('#OldPassword').focus();
	        $('input:password').bind("keydown", function (e) {
	            if (e.which == 13) {   //Enter key
	                e.preventDefault(); //to skip default behaviour of enter key
	                var nextIndex = $('input:password').index(this) + 1;
	                if (typeof this.id != undefined) {
	                    var $element = $('#' + this.id);
	                    if (this.id === "<%=ConfirmPassword.ClientID %>" && $element.val().length > 0 && $('#<%=NewPassword.ClientID %>').val().length > 0 && $('#<%=ConfirmPassword.ClientID %>').val().length > 0) {
	                        changePassword();
	                    }
                    }
                    if (nextIndex == $('input:password').length) nextIndex = 0;
                    if ($('input:password')[nextIndex]) {
                        $('input:password')[nextIndex].focus();
                    }
                }
	        });
        }
	});
    function changePassword() {
        var $mClientMessage = $('#clientMessage');
        $mClientMessage.html('');
        $mClientMessage.css({ display: 'none' });
        var mChangePassword = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
        var mRetHTML = "";
        var mOldPassword = $('#<%=OldPassword.ClientID %>').val();
        if (typeof mOldPassword === 'undefined') {
            mChangePassword.OldPassword = 'forced change';
        } else
        {
            mChangePassword.OldPassword = mOldPassword;
        }
        mChangePassword.NewPassword = $('#<%=NewPassword.ClientID %>').val();
        GW.Common.debug(JSON.stringify(mChangePassword));
        try {
            var options = GW.Model.DefaultWebMethodOptions();
            options.url = GW.Common.getBaseURL() + "/gw/api/Accounts/ChangePassword?Action=ChangePassword";
            options.data = mChangePassword;
            options.contentType = 'application/json; charset=utf-8';
            options.dataType = 'json';
            GW.Common.JQueryHelper.callWeb(options, changePasswordSuccess, changePasswordError);
        } catch (e) {
            var mRetHTML = 'Error attempting to call logon\n'// + e.Message;
            $mClientMessage.css({ display: 'none' });
            $mClientMessage.html(mRetHTML.toString());
        }
        return true;
    }

    function changePasswordSuccess(xhr) {
        GW.Common.debug(xhr);
        var $mClientMessage = $('#clientMessage');
        var mRetHTML = xhr;
        $mClientMessage.html(mRetHTML.toString()).fadeIn(800);
    }

    function changePasswordError(xhr, status, error) {
        var $mClientMessage = $('#clientMessage');
        var mRetHTML = 'Error changePasswordError on\n' + error;
        $mClientMessage.css({ display: 'none' });
        $mClientMessage.html(mRetHTML.toString());
    }
</script>
