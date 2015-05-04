<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SearchNVP.aspx.vb" Inherits="GrowthWare.WebApplication.SearchNVP" %>
<%@ Register Src="~/UserControls/Search.ascx" TagName="Search" TagPrefix="ucSearch" %>

<!DOCTYPE html>

<form id="frmSearchNVP" runat="server">
    <div>
        <ucSearch:Search ID="SearchControl" runat="server" />
    </div>
</form>
<script type="text/javascript" language="javascript">
	// used by SearchControl
	// search pages need to have the followint 2 javascript variables in the
	// "global" dom so that the SearchControl can
	// deal with any of the calls to get data

	$(document).ready(function () {
		GW.Search.init();
		GW.Search.SearchColumn = '[Account]';
		GW.Search.URL = GW.Common.getBaseURL() + "/Functions/System/Administration/NVP/SearchNVPResults.aspx?Action=Search_Name_Value_Pairs&";
		GW.Search.Criteria.SelectedPage = 1;
		GW.Search.Criteria.Columns = 'NVP_SeqID, Name, Description, Status, Added_By, Added_Date, Updated_By, Updated_Date';
		GW.Search.Criteria.OrderByColumn = 'Name';
		GW.Search.Criteria.OrderByDirection = 'ASC';
		GW.Search.Criteria.WhereClause = '"1 = 1"';
		GW.Search.GetSearchResults();
	});

	function addNew() {
		edit(-1);
		return true;
	}

	function edit(seqID) {
		var options = GW.Model.DefaultDialogOptions();
		options.title = 'Edit Name Value Pair';
		options.height = 450;
		options.width = 1000;
		options.async = false;
		options.resizable = true;
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/NVP/AddEditNVP.aspx?NVP_SEQ_ID=" + seqID;
		options.buttons = {
			'Save': function () { saveAddEditNVP($(this)); },
			'Cancel': function () { $(this).dialog("destroy"); $(this).remove(); }
		};
		var dialogId = 'popupAddEditNVP';
		GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
	}

	function getAddEditError() {
		alert('sorry');
	}

	function deleteNVP(seqID, name) {
		mSeqID = seqID;
		var dialogId = '#popupAddEditNVP';
		var dialogTitle = 'Are you Sure';
		var dialogMessageTemplate = 'You would like to delete Name Value Pair \n"' + name + '"';
		GW.Common.JQueryHelper.customConfirm(dialogId, 300, 300, okDelete, null, dialogTitle, dialogMessageTemplate);
	}

	function okDelete() {
		alert('deleting ' + mSeqID);
		getSearchResults();
	}

	function manageChildren(seqID) {
		var containerID = 'MainContentDiv';
		var options = GW.Model.DefaultDialogOptions();
		options.title = 'Manage NVP Children';
		options.async = false;
		options.resizable = true;
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/NVP/SearchNVPChildren.aspx?NVP_SEQ_ID=" + seqID;
		GW.Common.JQueryHelper.callWeb(options, successLoadPage, errorLoadPage);
	}

	function successLoadPage(msg) {
		var $pageMessage = $('#pageMessage');
		$('#MainContentDiv').html(msg.toString());
		$pageMessage.css({ display: 'none' });
		$pageMessage.html("Manage NVP Children").fadeIn(1000);
	}

	function errorLoadPage(xhr, status, error) {
		var $contentContainer = $('#MainContentDiv');
		var $pageMessage = $('#pageMessage');
		var mRetHTML = '';
		if (!m_Development) {
			mRetHTML = 'Error getting content';
			$contentContainer.css({ display: 'none' });
			$contentContainer.html('/Functions/System/Administration/NVP/SearchNVPChildren.aspx?').fadeIn(1000);
		} else {
			$pageMessage.css({ display: 'none' });
			$pageMessage.html('/Functions/System/Administration/NVP/SearchNVPChildren.aspx?').fadeIn(1000)
			mRetHTML = 'Error getting content\n' + xhr.responseText;
			$contentContainer.css({ display: 'none' });
			$contentContainer.html(mRetHTML).fadeIn(1000);
		}
	}
</script>