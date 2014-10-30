<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SearchAccounts.aspx.vb" Inherits="GrowthWare.WebApplication.SearchAccounts" %>

<%@ Register Src="~/UserControls/Search.ascx" TagName="Search" TagPrefix="ucSearch" %>
<script type="text/javascript" language="javascript">
	// used by SearchControl
	// search pages need to have the followint 2 javascript variables in the
	// "global" dom so that the SearchControl can
	// deal with any of the calls to get data

	$(document).ready(function () {
		GW.Search.init();
		GW.Search.SearchColumn = 'Account';
		GW.Search.URL = GW.Common.getBaseURL() + "/Functions/System/Administration/Accounts/SearchAccountResults.aspx?Action=SearchAccounts&";
		GW.Search.Criteria.SelectedPage = 1;
		GW.Search.Criteria.Columns = 'Account_SeqID, Account, First_Name, Last_Name, Email, Added_Date, Last_Login';
		GW.Search.Criteria.OrderByColumn = 'Account';
		GW.Search.Criteria.OrderByDirection = 'ASC';
		GW.Search.Criteria.WhereClause = '"1 = 1"';
		GW.Search.GetSearchResults();
	});

	function addNew() {
	    editAccount(-1);
	    return true;
	}

	function editAccount(accountSeqID, viewOnly) {
	    mAccountSeqID = accountSeqID;
	    var options = GW.Model.DefaultDialogOptions();
	    options.title = 'Edit Account';
	    options.height = 600;
	    options.width = 1000;
	    options.async = false;
	    options.resizable = true;
	    options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Accounts/AddEditAccount.aspx?AccountSeqID=" + mAccountSeqID;
	    GW.Common.debug(options.url);
	    if (!viewOnly) {
	        options.buttons = {
	            'Save': function () { saveAddEditAccount($(this)); },
	            'Cancel': function () { $(this).dialog("destroy"); $(this).remove(); }
	        };

	    } else {
	        options.buttons = {
	            'Cancel': function () { $(this).dialog("destroy"); $(this).remove(); }
	        };
	    }
	    var dialogId = 'popupAddEditAccount';
	    GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
	}

	function getAddEditError() {
	    alert('sorry');
	}

	function deleteAccount(accountSeqID, name) {
	    var callBackData = { accountSeqId: accountSeqID };
	    var dialogId = '#popupDeleteEditAccount';
	    var dialogTitle = 'Are you Sure';
	    var dialogMessageTemplate = 'You would like to delete Account \n"' + name + '"';
	    GW.Common.JQueryHelper.customConfirm(dialogId, 300, 300, okDeleteFunc, null, dialogTitle, dialogMessageTemplate, callBackData);
	}

	function okDeleteFunc(jsonObj) {
	    var options = GW.Model.DefaultWebMethodOptions();
	    options.async = true;
	    options.data = jsonObj;
	    options.contentType = 'application/json; charset=utf-8';
	    options.dataType = 'json';
	    options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Accounts/AddEditAccount.aspx/InvokeDelete"
	    GW.Common.JQueryHelper.callWeb(options);
	    GW.Search.GetSearchResults();
	}
</script>
<div id="popupDeleteEditAccount" />
<ucSearch:Search ID="SearchControl" runat="server" />
