<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Logon.aspx.vb" Inherits="GrowthWare.WebApplication.Logon" %>
<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		GW.Navigation.currentAction = 'Logon';
		$("#btnLogon").button();
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
		GW.Common.debug('finished with document.ready');
	});

	function logon() {
		var $mClientMessage = $("<%=clientMessage.ClientID %>");
		var $mIncorrectLogon = $("<%=incorrectLogon.ClientID %>");
		var $mBtnRequestChange = $('#btnRequestChange');
		var $mLogonPage = $('#LogonPage');
		$mIncorrectLogon.css({ display: 'none' });
		$mClientMessage.css({ display: 'none' });
		$mBtnRequestChange.css({ display: 'none' });
		var mRetHTML = "";
		var LogonInfo = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
		$('#LogonData').children('input').each(function () {
			if (this.type == 'text' || this.type == 'password') {
				LogonInfo[this.id] = this.value;
			}
		});

		try {
			GW.Common.debug(LogonInfo);
			var options = GW.Model.DefaultWebMethodOptions();
			options.url = GW.Common.getBaseURL() + "/api/Accounts/Logon?Action=sdf";
			options.data = JSON.stringify(LogonInfo);
			options.contentType = 'application/json; charset=utf-8';
			options.dataType = 'json';
			GW.Common.JQueryHelper.callWeb(options, logonSuccess, logonError);
		} catch (e) {
			mRetHTML = 'Error attempting to call logon\n' + e.Message;
			$mClientMessage.css({ display: 'none' });
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
		GW.Common.debug(xhr.d);
		var $mClientMessage = $("#<%=clientMessage.ClientID %>");
		var $mIncorrectLogon = $("#<%=incorrectLogon.ClientID %>");
		var $mBtnRequestChange = $('#btnRequestChange');
		var $mLogonPage = $('#LogonPage');
		if (xhr.toString() == "true") {
		    //jQuery.event.trigger('~reLoadUI');
		    GW.Navigation.NavigationController.Refresh();
		} else {
			if (xhr.toString() == "Request") {
				$mBtnRequestChange.css({ display: 'inline' });
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
		$mClientMessage.css({ display: 'none' });
		$mClientMessage.html(mRetHTML.toString()).fadeIn(3000);
	}

	function requestChangeSuccess(xhr) {
		GW.Common.debug(xhr.d);
		var $mClientMessage = $("<%=clientMessage.ClientID %>");
		var $mIncorrectLogon = $("<%=incorrectLogon.ClientID %>");
		var $mBtnRequestChange = $('#btnRequestChange');
		var $mLogonPage = $('#LogonPage');
		if (xhr.d.toString() == "true") {
			jQuery.event.trigger('~reLoadUI');
		} else {
			if (xhr.d.toString() == "Request") {
				$mBtnRequestChange.css({ display: 'inline' });
			} else {
				mRetHTML = xhr.d;
				$mClientMessage.html(mRetHTML.toString()).fadeIn(3000);
				//$mIncorrectLogon.fadeIn(3000);
			}
		}
	}

	function requestChangeError(xhr, status, error) {
		alert('hi from logonError');
		mRetHTML = 'Error logging on\n' + xhr.responseText;
		$mClientMessage.css({ display: 'none' });
		$mClientMessage.html(mRetHTML.toString()).fadeIn(3000);
	}


</script>
<div id="LogonPage">
	<div id="LogonData">
		<div style="text-align: right; width: 80px; float: left;">Account:&nbsp;&nbsp;</div><input type="text" class="rounded" id="Account" />
		<br /><br />
		<div style="text-align: right; width: 80px; float: left;">Password:&nbsp;&nbsp;</div><input type="password" class="rounded" id="Password" />
	</div>
	<br />
	<div style="text-align: right; width: 80px; float: left;">&nbsp;</div><input type="button" id="btnLogon" onclick="javascript:logon();" value="Logon" />&nbsp;<input type="button" id="btnRequestChange" style="display: none" onclick="javascript:requestChange();" value="Change Password" />
</div>
<div style="height: 26px;">
	<div id="clientMessage" style="display: none" runat="server"></div>
	<div id="incorrectLogon" style="display: none;" runat="server"></div>
</div>
