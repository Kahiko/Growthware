<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchMessages.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Messages.SearchMessages" %>
<%@ Register src="~/UserControls/Search.ascx" tagname="Search" tagprefix="ucSearch" %>

<!DOCTYPE html>
<form id="frmSearchMessages" runat="server">
    <ucSearch:Search ID="SearchControl" runat="server" />
</form>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        GW.Search.init();
        GW.Search.SearchColumn = '[Name]';
        GW.Search.URL = GW.Common.getBaseURL() + "/Functions/System/Administration/Messages/SearchMessageResults.aspx?Action=Search_Messages&";
        GW.Search.Criteria.SelectedPage = 1;
        GW.Search.Criteria.Columns = 'Message_SeqID, Name, Title, Description, Added_By, Added_Date, Updated_by, Updated_Date';
        GW.Search.Criteria.OrderByColumn = '[Name]';
        GW.Search.Criteria.OrderByDirection = 'ASC';
        GW.Search.Criteria.WhereClause = '"1 = 1"';
        GW.Search.GetSearchResults();
    });

    function addNew(e) {
        edit(-1, true);
        return true;
    }

    function edit(messageSeqId, mayEdit) {
        if (typeof mayEdit == undefined) mayEdit = false;
        var options = GW.Model.DefaultDialogOptions();
        options.title = 'Add or Edit Messages';
        options.height = 700;
        options.width = 1000;
        options.async = false;
        options.resizable = true;
        options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Messages/AddEditMessage.aspx?messageSeqId=" + messageSeqId;

        var myButtons = {};
        if (mayEdit) {
            myButtons["Save"] = function () {
                saveAddEditMessage($(this));
            }
        }

        myButtons["Cancel"] = function () {
            $(this).dialog("close");
        }

        options.buttons = myButtons;


        var dialogId = 'addEditGroup';
        GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
    }
</script>