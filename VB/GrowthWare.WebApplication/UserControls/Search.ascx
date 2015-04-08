<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Search.ascx.vb" Inherits="GrowthWare.WebApplication.Search" %>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        if (typeof jQuery.ui != 'undefined') {
            $("#<%= addNew.ClientID %>").button();
            $("#<%= btnRefesh.ClientID %>").button();
            $("#<%= cmdSelect.ClientID %>").button();
        }
        GW.Search.Criteria.PageSize = document.getElementById("<%= txtRecordsPerPage.ClientID %>").value
        $("input").bind("keydown", function (event) {
            // track enter key
            var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
            if (keycode == 13) { // keycode for enter key
                if (this.id == "<%= txtRecordsPerPage.ClientID %>") {
                    mRecordsPerPage = document.getElementById("<%= txtRecordsPerPage.ClientID %>").value;
                    if (isNaN(mRecordsPerPage)) {
                        alert('Records per page must be a number.');
                        event.preventDefault();
                        return false;
                    } else {
                        GW.Search.Criteria.PageSize = mRecordsPerPage;
                        GW.Search.Criteria.SelectedPage = 1;
                        GW.Search.GetSearchResults();
                    }
                }
                if (this.id == 'txtSearch') {
                    GW.Search.Criteria.SelectedPage = 1;
                    GW.Search.Criteria.WhereClause = '"' + GW.Search.SearchColumn + ' like \'%' + document.getElementById('txtSearch').value + '%\'"';
                    GW.Search.Criteria.WhereClause = escape(GW.Search.Criteria.WhereClause);
                    GW.Search.GetSearchResults();
                }
                return false;
            } else {
                return true;
            }
        }); // end of function
    });

    function selectAll() {
        var btn = document.getElementById('<%=cmdSelect.ClientID %>');
        var checked = false;
        if (btn.value == "Select All") {
            btn.value = "De-Select All";
            checked = true;
        } else {
            btn.value = "Select All";
            checked = false;
        }
        try {
            var checkBoxes = document.getElementsByTagName('input');
            for (var i = 0; i < checkBoxes.length; i++) {
                var checkBox = checkBoxes[i];
                if (checkBox.type == 'checkbox') {
                    var id = checkBox.id
                    if (id.indexOf('DeleteCheckBox') > 0) {
                        checkBox.checked = checked;
                    }
                }
            }
        } catch (e) {
            //alert(e.message);
        }
    }

    function btnDelete_Click() {
        var inputBoxes = document.getElementsByTagName('input');
        var checkBoxes = new Array;
        var answer = false;
        for (var i = 0; i < inputBoxes.length; i++) {
            var checkBox = inputBoxes[i];
            if (checkBox.type == 'checkbox') {
                var id = checkBox.id
                if (id.indexOf('DeleteCheckBox') > 0 && checkBox.checked == true) {
                    checkBoxes.push(checkBox);
                }
            }
        }
        if (checkBoxes.length > 0) {
            answer = window.confirm("Are you sure want to delete this?");
        }

        if (answer) {
            var filesToDelete = new Array;
            for (var i = 0; i < checkBoxes.length; i++) {
                var checkBox = checkBoxes[i];
                if (checkBox.type == 'checkbox') {
                    var id = checkBox.id
                    var $checkBox = $('#' + id);
                    var theData = $.parseJSON($checkBox.attr('data'));
                    filesToDelete.push(theData);
                }
            }
            //GW.Common.debug(filesToDelete);
            try {
                var options = GW.Model.DefaultWebMethodOptions();
                GW.Common.debug(filesToDelete);
                options.url = GW.Common.getBaseURL() + "/gw/api/FileManager/DeleteFiles";
                options.data = filesToDelete;
                options.contentType = 'application/json; charset=utf-8';
                options.dataType = 'json';
                GW.Common.JQueryHelper.callWeb(options, deletFilesSuccess, deleteFilesError);
            } catch (e) {
                mRetHTML = 'Error attempting to call DeleteFiles\n' + e.message;
                alert(mRetHTML);
                //				$mClientMessage.css({ display: 'none' });
                //				$mClientMessage.html(mRetHTML.toString()).fadeIn(3000);
            }
            return true;
        }
        else {
            //alert('doing nothing');
            return false;
        }
    }

    function deletFilesSuccess(xhr) {
        GW.Common.debug(xhr);
        if (xhr.indexOf("Successfully") == -1) {
            alert(xhr);
        }
        var btn = document.getElementById('<%=cmdSelect.ClientID %>');
        btn.value = "Select All";
        jQuery.event.trigger('~refreshDirectory', '');
    }

    function deleteFilesError(xhr, status, error) {
        mRetHTML = 'Error deleting files\n' + xhr.responseText;
        alert(mRetHTML);
    }
</script>
<div class="searchResultsContainer Container">
	<div class="searchResultsHeader">
        <span style="display: inline-block; width: 33%; text-align: left;">
            Show <input id="txtRecordsPerPage" class="rounded10" style="width: 34px;" onblur="javascript:GW.Search.onRecordsChanged(this);" runat="server" value="10" />&nbsp;records per page&nbsp;&nbsp;
        </span>
        <span style="display: inline-block; width: 33%;">
            <input id="cmdSelect" class="btn btn-primary" type="button" value="Select All" onclick="selectAll();" name="cmdSelect" runat="server" />&nbsp;
            <img alt="Delete file(s)" id="imgDeleteAll" onclick="javascript:btnDelete_Click();" src="Public/GrowthWare/Images/delete_red.png" title="Delete selection" style="height: 16px; width: 16px;" runat="server" />
        </span>
        <span style="display: inline-block; width: 33%; text-align: right;">
            <a id="btnRefesh" class="btn btn-primary" runat="server" onclick="javascript:GW.Search.GetSearchResults();">Refresh </a>
            <a id="addNew" class="btn btn-primary" runat="server" onclick="javascript:addNew();">Add New</a>
            <input id="txtSearch" style="width: 164px;" placeholder="Search in selected column." class="rounded10" />
        </span>
	</div>
	<div id="searchResults" class="searchResultsMainbody">
	</div>
	<div class="searchResultsFooter">
		<div style="float: left">
			Page <span id="currentPage" runat="server">1</span> of <span id="totalPages">1</span> <span id="totalRecords">(0 total items)</span>
		</div>
		<div style="float: right;">
			<table>
				<tr>
					<td>
						<img alt="Go to first page" onclick="javascript:GW.Search.FirstPage();" src="Public/GrowthWare/Images/Paging/first_blue.png" height="16px" width="16px" />
					</td>
					<td style="vertical-align: middle;">
						<img alt="Previous page" onclick="javascript:GW.Search.PreviousPage();" src="Public/GrowthWare/Images/Paging/previous_blue.png" height="16px" width="16px" />
					</td>
					<td>
						Page&nbsp;<select id="ddSelectPage" onclick="javascript:GW.Search.onDropSelectPageChanged();"></select>
					</td>
					<td>
						<img alt="Next page" onclick="javascript:GW.Search.NextPage();" src="Public/GrowthWare/Images/Paging/next_blue.png" height="16px" width="16px" />
					</td>
					<td>
						<img alt="Go to last page" onclick="javascript:GW.Search.LastPage();" src="Public/GrowthWare/Images/Paging/last_blue.png" height="16px" width="16px" />
					</td>
					<td style="width: 16px;">
						&nbsp;
					</td>
				</tr>
			</table>
		</div>
	</div>
</div>
<div id="searchPopupDiv" style="display: none;">
</div>