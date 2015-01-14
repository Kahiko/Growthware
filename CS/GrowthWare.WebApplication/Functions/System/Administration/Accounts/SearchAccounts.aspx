<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchAccounts.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Accounts.SearchAccounts" %>

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

    function editAccount(accountSeqID, mayEdit, mayDelete) {
        if (typeof mayEdit == undefined) mayEdit = false;
        if (typeof mayDelete == undefined) mayDelete = false;
        mAccountSeqID = accountSeqID;
        var options = GW.Model.DefaultDialogOptions();
        options.title = 'Edit Account';
        if (accountSeqID == -1) options.title = 'Add Account';
        options.height = 600;
        options.width = 1000;
        options.async = false;
        options.resizable = true;
        options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Accounts/AddEditAccount.aspx?AccountSeqID=" + mAccountSeqID;
        GW.Common.debug(options.url);
        var myButtons = {};
        if (mayDelete) {
            myButtons["Delete"] = function () {
                var options = GW.Model.DefaultWebMethodOptions();
                options.async = false;
                options.contentType = 'application/json; charset=utf-8';
                options.dataType = 'json';
                options.url = GW.Common.getBaseURL() + "/gw/api/Accounts/Delete?accountSeqId=" + mAccountSeqID;
                GW.Common.JQueryHelper.callWeb(options);
                $(this).dialog("close");
                GW.Search.GetSearchResults();
            }
        };

        if (mayEdit) {
            myButtons["Save"] = function () {
                saveAddEditAccount($(this));
            }
        }

        myButtons["Cancel"] = function () {
            $(this).dialog("close");
        }

        options.buttons = myButtons;
        var dialogId = 'popupAddEditAccount';
        GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
    }

    function getAddEditError() {
        alert('sorry');
    }
</script>
<ucSearch:Search ID="SearchControl" runat="server" />
