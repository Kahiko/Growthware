<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchNVPChildren.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.NVP.SearchNVPChildren" %>
<%@ Register Src="~/UserControls/Search.ascx" TagName="Search" TagPrefix="ucSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<form id="frmSearchNVPChildren" runat="server">
    <div>
        <input id="NVP_SEQ_ID" type="text" style="display: none;" runat="server" />
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
		GW.Search.URL = GW.Common.getBaseURL() + "/Functions/System/Administration/NVP/SearchNVPResultsChildren.aspx?Action=Search_Name_Value_Pairs" + "&NVP_SEQ_ID=" + $("#<%=NVP_SEQ_ID.ClientID %>").val() + "&";
		GW.Search.Criteria.SelectedPage = 1;
		GW.Search.Criteria.Columns = 'NVP_SEQ_DET_ID, NVP_SEQ_ID, Table_Name, NVP_DET_VALUE, NVP_DET_TEXT, STATUS_SEQ_ID, Added_By, Added_Date, Updated_By, Updated_Date';
		GW.Search.Criteria.OrderByColumn = 'NVP_Detail_Name';
		GW.Search.Criteria.OrderByDirection = 'ASC';
		GW.Search.Criteria.WhereClause = "NVP_SEQ_ID = " + $("#<%=NVP_SEQ_ID.ClientID %>").val();
		GW.Search.GetSearchResults();
	});

	function addNew() {
		edit(-1, -1);
		return true;
	}

	function edit(seqID, detailSeqID) {
		seqID = $("#NVP_SEQ_ID").val();
		//alert('seqID: ' + seqID + ' detailSeqID: ' + detailSeqID)
		var options = GW.Model.DefaultDialogOptions();
		options.title = 'Add or Edit Name Value Pair Detail';
		options.height = 300;
		options.width = 350;
		options.async = false;
		options.resizable = true;
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/NVP/AddEditNVPDetails.aspx?NVP_SEQ_ID=" + seqID + "&NVP_Detail_SeqID=" + detailSeqID;
		options.buttons = {
			'Save': function () { saveAddEditNVPDetails($(this)); },
			'Cancel': function () { $(this).dialog("destroy"); $(this).remove(); }
		};
		var dialogId = 'popupAddEditNVP';
		GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
	}

	function getAddEditError() {
		alert('sorry');
	}

	function deleteNVPDetail(seqID, name) {
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