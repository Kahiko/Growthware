<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SearchSecurityEntities.aspx.vb" Inherits="GrowthWare.WebApplication.SearchSecurityEntities" %>
<%@ Register Src="~/UserControls/Search.ascx" TagName="Search" TagPrefix="ucSearch" %>
<script type="text/javascript" language="javascript">
	$(document).ready(function () {
	    GW.Search.init();
		GW.Search.SearchColumn = '[Name]';
		GW.Search.URL = GW.Common.getBaseURL() + "/Functions/System/Administration/SecurityEntities/SearchSecurityEntitiesResults.aspx?Action=SearchSecurityEntities";
		GW.Search.Criteria.SelectedPage = 1;
		GW.Search.Criteria.Columns = 'Security_Entity_SeqID, Name, Description, Skin';
		GW.Search.Criteria.OrderByColumn = '[Name]';
		GW.Search.Criteria.OrderByDirection = 'ASC';
		GW.Search.Criteria.WhereClause = '"1 = 1"';
		GW.Search.GetSearchResults();
	});

	function addNew(e) {
		edit(-1, true);
		return true;
	}

	function edit(securityEntitySeqId, mayEdit) {
	    if (typeof mayEdit == undefined) mayEdit = false;
		mSecurityEntitySeqId = securityEntitySeqId;
		var options = GW.Model.DefaultDialogOptions();
		options.title = 'Edit Security Entity';
		options.height = 650;
		options.width = 800;
		options.async = false;
		options.resizable = true;
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/SecurityEntities/AddEditSecurityEntities.aspx?SecurityEntitySeqId=" + securityEntitySeqId;
		if (mayEdit) {
		    options.buttons = {
		        'Save': function () { saveAddEdit($(this)); },
		        'Cancel': function () { $(this).dialog("destroy"); $(this).remove(); }
		    };

		} else {
		    options.buttons = {
		        'Cancel': function () { $(this).dialog("destroy"); $(this).remove(); }
		    };
		}
		var dialogId = 'popupAddEditSecurityEntity';
		GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
	}
</script>
<ucSearch:Search ID="SearchControl" runat="server" />
