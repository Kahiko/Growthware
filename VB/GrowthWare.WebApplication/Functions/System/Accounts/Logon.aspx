<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Logon.aspx.vb" Inherits="GrowthWare.WebApplication.Logon" %>
<%@ Register src="~/UserControls/OpenAuthProviders.ascx" tagname="OpenAuthProviders" tagprefix="uc" %>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        GW.Navigation.currentAction = 'Logon';
        if (typeof jQuery.ui != 'undefined') {
            $("#btnLogon").button();
            $("#btnRequestChange").button();
        }
        if ($('input:text')) {
            $('#Account').focus();
            $('input').bind("keydown", function (e) {
                if (e.which == 13) {   //Enter key
                    e.preventDefault(); //to skip default behaviour of enter key
                    var nextIndex = $('input:text').index(this) + 1;
                    if (typeof this.id != undefined) {
                        var $element = $('#' + this.id);
                        if (this.id === "Password" && $element.val().length > 0 && $('#Account').val().length > 0) {
                            document.getElementById('btnLogon').click();
                        }
                    }
                    if (nextIndex == $('input:text').length) nextIndex = 0;
                    if ($('input:text')[nextIndex]) {
                        $('input:text')[nextIndex].focus();
                    }
                }
            });
        }

        $("#chkShowPassword").bind("click", function () {
            var txtPassword = $("#Password");
            if ($(this).is(":checked")) {
                txtPassword.after('<input onchange = "PasswordChanged(this);" id = "txt_' + txtPassword.attr("id") + '" type = "text" value = "' + txtPassword.val() + '" />');
                txtPassword.hide();
            } else {
                txtPassword.val(txtPassword.next().val());
                txtPassword.next().remove();
                txtPassword.show();
            }
        });

        GW.Common.debug('finished with document.ready');
    });

    function logon() {
        var $mClientMessage = $("<%=clientMessage.ClientID %>");
        var $mIncorrectLogon = $("<%=incorrectLogon.ClientID %>");
        var $mBtnRequestChange = $('#btnRequestChange');
        var $mLogonPage = $('#LogonPage');
        //$mIncorrectLogon.css({ display: 'none' });
        //$mClientMessage.css({ display: 'none' });
        //$mBtnRequestChange.css({ display: 'none' });

        $mIncorrectLogon.css('visibility', 'hidden');
        $mClientMessage.css('visibility', 'hidden');
        $mBtnRequestChange.css('visibility', 'hidden');

        var mRetHTML = "";
        var mLogonInfo = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
        $('#LogonData').children('input').each(function () {
            if (this.type == 'text' || this.type == 'password') {
                mLogonInfo[this.id] = this.value;
            }
        });

        try {
            GW.Common.debug(mLogonInfo);
            var options = GW.Model.DefaultWebMethodOptions();
            options.url = GW.Common.getBaseURL() + "/gw/api/Accounts/Logon?Action=Logon";
            options.data = mLogonInfo;
            options.contentType = 'application/json; charset=utf-8';
            options.dataType = 'json';
            GW.Common.JQueryHelper.callWeb(options, logonSuccess, logonError);
        } catch (e) {
            mRetHTML = 'Error attempting to call logon\n' + e.Message;
            //$mClientMessage.css({ display: 'none' });
            $mClientMessage.css('visibility', 'hidden');
            $mClientMessage.html(mRetHTML.toString()).fadeIn(3000);
        }
        return true;
    }

    function requestChange() {
        var options = GW.Model.DefaultWebMethodOptions();
        options.url = GW.Common.getBaseURL() + "/Functions/System/Logon/Logon.aspx/RequestChange";
        options.data = JSON.stringify({ account: $('#Account').val() });
        options.contentType = 'application/json; charset=utf-8';
        options.dataType = 'json';
        GW.Common.JQueryHelper.callWeb(options, requestChangeSuccess, requestChangeError);
    }

    function logonSuccess(xhr) {
        GW.Common.debug(xhr);
        var $mClientMessage = $("#<%=clientMessage.ClientID %>");
	    var $mIncorrectLogon = $("#<%=incorrectLogon.ClientID %>");
	    var $mBtnRequestChange = $('#btnRequestChange');
	    var $mLogonPage = $('#LogonPage');
	    if (xhr.toString() == "true") {
	        window.location.hash = "?Action=Favorite";
	        //jQuery.event.trigger('~reLoadUI');
	        location.reload();
	    } else {
	        if (xhr.toString() == "Request") {
	            $mBtnRequestChange.css({ display: 'inline' });
	            $mBtnRequestChange.css('visibility', 'visible');
	        } else {
	            mRetHTML = xhr;
	            $mClientMessage.html(mRetHTML.toString()).fadeIn(3000);
	            $mIncorrectLogon.fadeIn(3000);
	        }
	    }
	}

	function logonError(xhr, status, error) {
	    var $mClientMessage = $("<%=clientMessage.ClientID %>");
	    var mRetHTML = 'Error logging on\n' + xhr.responseText;
	    //$mClientMessage.css({ display: 'none' });
	    $mClientMessage.css('visibility', 'hidden');
	    $mClientMessage.html(mRetHTML.toString()).fadeIn(3000);
	}

	function requestChangeSuccess(xhr) {
	    GW.Common.debug(xhr);
	    var $mClientMessage = $("<%=clientMessage.ClientID %>");
	    var $mIncorrectLogon = $("<%=incorrectLogon.ClientID %>");
	    var $mBtnRequestChange = $('#btnRequestChange');
	    var $mLogonPage = $('#LogonPage');
	    if (xhr.toString() == "true") {
	        jQuery.event.trigger('~reLoadUI');
	    } else {
	        if (xhr.toString() == "Request") {
	            //$mBtnRequestChange.css({ display: 'inline' });
	            $mBtnRequestChange.css('visibility', 'visible');
	        } else {
	            mRetHTML = xhr.d;
	            $mClientMessage.html(mRetHTML.toString()).fadeIn(3000);
	            $mClientMessage.css('visibility', 'visible');
	            //$mIncorrectLogon.fadeIn(3000);
	        }
	    }
	}

	function requestChangeError(xhr, status, error) {
	    mRetHTML = 'Error logging on\n' + xhr.responseText;
	    $mClientMessage.css({ display: 'none' });
	    $mClientMessage.html(mRetHTML.toString()).fadeIn(3000);
	}


	function PasswordChanged(txt) {
	    $(txt).prev().val($(txt).val());
	}
</script>
<form id="frmLogon" runat="server">
    <div id="LogonPage">
	    <div id="LogonData">
		    <div style="text-align: right; width: 80px; float: left;">Account:&nbsp;&nbsp;</div><input type="text" class="Form_Field rounded" id="Account" />
		    <br /><br />
		    <div style="text-align: right; width: 80px; float: left;">Password:&nbsp;&nbsp;</div><input type="password" class="Form_Field rounded" id="Password" />
            <label for="chkShowPassword">
                    <input type="checkbox" id="chkShowPassword" />
                    Show password
            </label>
	    </div>
	    <br />
	    <div style="text-align: right; width: 80px; float: left;">&nbsp;</div><input type="button" class="btn btn-primary" id="btnLogon" onclick="javascript: logon();" value="Logon" />&nbsp;<input type="button" id="btnRequestChange" style="display: none" onclick="    javascript: requestChange();" value="Change Password" />
        <br />
	    <uc:OpenAuthProviders runat="server" ID="OpenAuthLogin" />

    </div>
    <div style="height: 26px;">
	    <div id="clientMessage" class="Form_Message" style="display: none" runat="server"></div>
	    <div id="incorrectLogon" style="display: none;" runat="server"></div>
    </div>
</form>