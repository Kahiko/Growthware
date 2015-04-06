<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchGroups.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Groups.SearchGroups" %>
<%@ Register Src="~/UserControls/Search.ascx" TagName="Search" TagPrefix="ucSearch" %>

<!DOCTYPE html>
<form id="frmSearchGroups" runat="server">
    <div>
		<div id="addEditPopupDiv">
		</div>
		<ucsearch:search id="SearchControl" runat="server" />    
    </div>
</form>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        GW.Search.init();
        GW.Search.SearchColumn = '[Name]';
        GW.Search.URL = GW.Common.getBaseURL() + "/Functions/System/Administration/Groups/SearchGroupResults.aspx?Action=Manage_Groups&";
        GW.Search.Criteria.SelectedPage = 1;
        GW.Search.Criteria.Columns = 'GROUP_SEQ_ID, Name, Description, Added_By, Added_Date, Updated_by, Updated_Date';
        GW.Search.Criteria.OrderByColumn = '[Name]';
        GW.Search.Criteria.OrderByDirection = 'ASC';
        GW.Search.Criteria.WhereClause = '"1 = 1"';
        GW.Search.GetSearchResults();
    });

    function addNew() {
        edit(-1, true, false);
        return true;
    }

    function edit(groupSeqId, mayEdit, mayDelete) {
        if (typeof mayEdit == undefined) mayEdit = false;
        if (typeof mayDelete == undefined) mayDelete = false;
        var options = GW.Model.DefaultDialogOptions();
        options.title = 'Edit Group';
        options.height = 300;
        options.width = 500;
        options.async = false;
        options.resizable = true;
        options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Groups/AddEditGroup.aspx?GroupSeqId=" + groupSeqId;
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
                options.url = GW.Common.getBaseURL() + "/gw/api/Groups/Delete?groupSeqID=" + groupSeqId;
                options.contentType = 'application/json; charset=utf-8';
                options.dataType = 'json';
                GW.Common.JQueryHelper.callWeb(options, okDeleteGroupSuccess);
                $(this).dialog("close");
            }
        };
        myButtons["Cancel"] = function () {
            $(this).dialog("close");
        }
        options.buttons = myButtons;
        var dialogId = 'addEditGroup';
        GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
    }

    function editMembers(groupSeqId) {
        var options = GW.Model.DefaultDialogOptions();
        options.title = 'Associate roles to the group';
        options.height = 300;
        options.width = 500;
        options.async = false;
        options.resizable = true;
        options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Groups/EditGroupMembers.aspx?Action=Edit_Group_Members&GroupSeqId=" + groupSeqId;
        options.buttons = {
            'Save': function () { saveMembers($(this)); },
            'Cancel': function () { $(this).dialog("destroy"); $(this).remove(); }
        };
        var dialogId = 'addEditGroup';
        GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
    }

    function okDeleteGroupSuccess(xhr, status, error) {
        GW.Search.GetSearchResults();
    }
</script>
