<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SelectSecurityEntity.aspx.vb" Inherits="GrowthWare.WebApplication.SelectSecurityEntity" %>
	<script type="text/javascript" language="javascript">
	    var $mClientMessage = $('#clientMessage');

	    $(document).ready(function () {
	        if (typeof jQuery.ui != 'undefined') {
	            $('#btnGo').button();
	        }
	    });

	    function InvokeSelectSecurityEntity() {
	        $mClientMessage.css({ display: 'none' });
	        var mRetHTML = "";
	        var selectedSecurityEntity = $('#dropSecurityEntities').find(":selected").val();
	        //var theData = 'selectedSecurityEntity:' + selectedSecurityEntity;
	        var theData = '{ selectedSecurityEntityId: ' + selectedSecurityEntity + ' }';
	        try {
	            var options = GW.Model.DefaultWebMethodOptions();
	            options.url = GW.Common.getBaseURL() + "/gw/api/Accounts/SelectSecurityEntity/?Action=SelectASecurityEntity&selectedSecurityEntityId=" + selectedSecurityEntity;
	            options.data = theData;
	            //options.data = selectedSEId;
	            options.contentType = 'application/json; charset=utf-8';
	            options.dataType = 'json';
	            GW.Common.JQueryHelper.callWeb(options, selectSecurityEntitySuccess, selectSecurityEntityError);
	        } catch (e) {
	            mRetHTML = 'Error attempting to call SetSelectedSecurityEntity\n' + e.Message;
	            $mClientMessage.css({ display: 'none' });
	            $mClientMessage.html(mRetHTML.toString()).fadeIn(3000);
	        }
	        return true;
	    }

	    function selectSecurityEntitySuccess(xhr) {
	        jQuery.event.trigger('~reLoadUI');
	    }

	    function selectSecurityEntityError(xhr, status, error) {
	        mRetHTML = 'Error selecting\n' + unescape(xhr.responseText);
	        $mClientMessage.css({ display: 'none' });
	        $mClientMessage.html(mRetHTML.toString()).fadeIn(3000);
	    }

	</script>
<form id="SelectSecurityEntity" runat="server">
<div>
	<table cellspacing="5" cellpadding="2" width="100%" border="0">
		<tr align="left">
			<td align="left">
				<asp:DropDownList ID="dropSecurityEntities" CssClass="rounded" runat="server" />
			</td>
		</tr>
		<tr>
			<td align="left">
				<input type="button" id="btnGo" class="btn btn-primary" onclick="javascript:InvokeSelectSecurityEntity();" value="Go" runat="server" />
			</td>
		</tr>
	</table>    
</div>
<div style="height: 26px;">
	<div id="clientMessage" style="display: none" runat="server"></div>
	<div id="incorrectLogon" style="display: none;" runat="server"></div>
</div>
</form>
