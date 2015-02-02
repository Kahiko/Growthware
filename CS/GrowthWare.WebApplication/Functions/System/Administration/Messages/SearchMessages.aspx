<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchMessages.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Messages.SearchMessages" %>
<%@ Register src="~/UserControls/Search.ascx" tagname="Search" tagprefix="ucSearch" %>
<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		GW.Search.init();
		GW.Search.SearchColumn = '[Name]';
		GW.Search.URL = GW.Common.getBaseURL() + "/Functions/System/Administration/Messages/SearchMessageResults.aspx?Action=SearchMessages&";
		GW.Search.Criteria.SelectedPage = 1;
		GW.Search.Criteria.Columns = 'Message_SeqID, Name, Title, Description, Added_By, Added_Date, Updated_by, Updated_Date';
		GW.Search.Criteria.OrderByColumn = '[Name]';
		GW.Search.Criteria.OrderByDirection = 'ASC';
		GW.Search.Criteria.WhereClause = '"1 = 1"';
		GW.Search.GetSearchResults();
	});

	function addNew(e) {
		edit(-1);
		return true;
	}

	function edit(messageSeqId) {
		var options = GW.Model.DefaultDialogOptions();
		options.title = 'Add or Edit Messages';
		options.height = 700;
		options.width = 1000;
		options.async = false;
		options.resizable = true;
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Messages/AddEditMessage.aspx?messageSeqId=" + messageSeqId;
		options.buttons = {
			'Save': function () { saveAddEdit($(this)); },
			'Cancel': function () { $(this).dialog("destroy"); $(this).remove(); }
		};
		var dialogId = 'addEditGroup';
		GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
	}

	function deleteMessage(messageSeqId, name) {
		var callBackData = { groupSeqId: groupSeqId };
		var dialogId = 'addEditPopupDiv';
		var dialogTitle = 'Are you Sure';
		var dialogMessageTemplate = 'You would like to delete group \n"' + name + '"';
		GW.Common.JQueryHelper.customConfirm(dialogId, 300, 300, okDeleteFunc, null, dialogTitle, dialogMessageTemplate, callBackData);
	}

	function okDeleteFunc(jsonObj) {
		var options = GW.Model.DefaultWebMethodOptions();
		options.async = true;
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Messages/AddEditMessage.aspx/InvokeDelete";
		options.data = JSON.stringify(jsonObj);
		options.contentType = 'application/json; charset=utf-8';
		options.dataType = 'json';
		GW.Common.JQueryHelper.callWeb(options);
		GW.Search.GetSearchResults();
	}
</script>

<ucSearch:Search ID="SearchControl" runat="server" />