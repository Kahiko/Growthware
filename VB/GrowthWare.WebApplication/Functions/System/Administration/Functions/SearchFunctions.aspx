<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SearchFunctions.aspx.vb" Inherits="GrowthWare.WebApplication.SearchFunctions" %>
<%@ Register Src="~/UserControls/Search.ascx" TagName="Search" TagPrefix="ucSearch" %>

<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		GW.Search.init();
		GW.Search.SearchColumn = '[Name]';
		GW.Search.URL = GW.Common.getBaseURL() + "/Functions/System/Administration/Functions/SearchFunctionResults.aspx?";
		GW.Search.Criteria.SelectedPage = 1;
		GW.Search.Criteria.Columns = 'Function_SeqID, Name, Description, Action, Added_By, Added_Date, Updated_By, Updated_Date';
		GW.Search.Criteria.OrderByColumn = '[Name]';
		GW.Search.Criteria.OrderByDirection = 'ASC';
		GW.Search.Criteria.WhereClause = '"1 = 1"';
		GW.Search.GetSearchResults();
	});

	function addNew(e) {
		editFunction(-1);
		return true;
	}

	function editFunction(functionSeqID) {
		mFunctionSeqID = functionSeqID;
		var options = GW.Model.DefaultDialogOptions();
		options.title = 'Edit Function';
		options.height = 595;
		options.width = 1050;
		options.async = false;
		options.resizable = true;
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Functions/AddEditFunction.aspx?FunctionSeqID=" + mFunctionSeqID;
		options.buttons = {
			'Save': function () { saveAddEditFunciton($(this)); },
			'Cancel': function () { $(this).dialog("destroy"); $(this).remove(); }
		};
		var dialogId = 'addEditFunction';
		GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
	}

	function deleteFunction(functionSeqID, name) {
		var callBackData = { functionSeqId: functionSeqID };
		var dialogId = 'addEditPopupDiv';
		var dialogTitle = 'Are you Sure';
		var dialogMessageTemplate = 'You would like to delete function \n"' + name + '"';
		GW.Common.JQueryHelper.customConfirm(dialogId, 300, 300, okDeleteFunc, null, dialogTitle, dialogMessageTemplate, callBackData);
	}

	function okDeleteFunc(jsonObj) {
		var options = GW.Model.DefaultWebMethodOptions();
		options.async = true;
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Functions/AddEditFunction.aspx/DeleteFunction";
		options.data = JSON.stringify(jsonObj);
		options.contentType = 'application/json; charset=utf-8';
		options.dataType = 'json';
		GW.Common.JQueryHelper.callWeb(options);
		GW.Search.GetSearchResults();
	}
</script>
<ucSearch:Search ID="SearchControl" runat="server" />