<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SearchRoles.aspx.vb" Inherits="GrowthWare.WebApplication.SearchRoles" %>

<%@ Register Src="~/UserControls/Search.ascx" TagName="Search" TagPrefix="ucSearch" %>
<script type="text/javascript" language="javascript">
	$(document).ready(function () {
	    GW.Search.init();
	    GW.Search.SearchColumn = '[Name]';
	    GW.Search.URL = GW.Common.getBaseURL() + "/Functions/System/Administration/Roles/SearchRoleResults.aspx?Action=Search_Roles&";
	    GW.Search.Criteria.SelectedPage = 1;
	    GW.Search.Criteria.Columns = 'ROLE_SEQ_ID, Name, Description, Is_System, Is_System_Only, Added_By, Added_Date, Updated_by, Updated_Date';
	    GW.Search.Criteria.OrderByColumn = '[Name]';
	    GW.Search.Criteria.OrderByDirection = 'ASC';
	    GW.Search.Criteria.WhereClause = '"1 = 1"';
	    GW.Search.GetSearchResults();
	});

	function addNew(e) {
	    edit(-1, true, false);
		return true;
	}

	function edit(roleSeqId, mayEdit, mayDelete) {
	    if (typeof mayEdit == undefined) mayEdit = false;
	    if (typeof mayDelete == undefined) mayDelete = false;
		var options = GW.Model.DefaultDialogOptions();
		options.title = 'Edit Role';
		if (roleSeqId == -1) options.title = 'Add Role';
		options.height = 350;
		options.width = 500;
		options.async = false;
		options.resizable = true;
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Roles/AddEditRole.aspx?RoleSeqID=" + roleSeqId;
		var myButtons = {};
		if (mayEdit) {
		    myButtons["Save"] = function () {
		        saveAddEdit($(this));
		    }
		}
		if (mayDelete) {
		    myButtons["Delete"] = function () {
		        var options = GW.Model.DefaultWebMethodOptions();
		        options.async = true;
		        options.url = GW.Common.getBaseURL() + "/gw/api/Roles/Delete?roleSeqId=" + roleSeqId;
		        options.contentType = 'application/json; charset=utf-8';
		        options.dataType = 'json';
		        GW.Common.JQueryHelper.callWeb(options);
		        $(this).dialog("close");
		        GW.Search.GetSearchResults();
		    }
		};
		myButtons["Cancel"] = function () {
		    $(this).dialog("close");
		}
		options.buttons = myButtons;
		var dialogId = 'addEditRole';
		GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
	}

	function editMembers(roleSeqId) {
		mFunctionSeqID = roleSeqId;
		var options = GW.Model.DefaultDialogOptions();
		options.title = 'Edit Role Members';
		options.height = 300;
		options.width = 500;
		options.async = false;
		options.resizable = true;
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Roles/EditRoleMembers.aspx?Action=Edit_Role_Members&ROLE_SEQ_ID=" + roleSeqId;
		options.buttons = {
			'Save': function () { saveMembers($(this)); },
			'Cancel': function () { $(this).dialog("destroy"); $(this).remove(); }
		};
		var dialogId = 'addEditRole';
		GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
	}

	function deleteRole(roleSeqId, name) {
		var callBackData = { roleSeqId: roleSeqId };
		var dialogId = 'addEditPopupDiv';
		var dialogTitle = 'Are you Sure';
		var dialogMessageTemplate = 'You would like to delete role \n"' + name + '"';
		GW.Common.JQueryHelper.customConfirm(dialogId, 300, 300, okDeleteFunc, null, dialogTitle, dialogMessageTemplate, callBackData);
	}

	function okDeleteFunc(jsonObj) {
		var options = GW.Model.DefaultWebMethodOptions();
		options.async = true;
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Roles/AddEditRole.aspx/InvokeDelete";
		options.data = JSON.stringify(jsonObj);
		options.contentType = 'application/json; charset=utf-8';
		options.dataType = 'json';
		GW.Common.JQueryHelper.callWeb(options);
		GW.Search.GetSearchResults();
	}
</script>
<ucSearch:Search ID="SearchControl" runat="server" />
