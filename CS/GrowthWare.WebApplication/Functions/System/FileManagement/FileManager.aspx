<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileManager.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.FileManagement.FileManager" %>
<%@ Register Src="~/UserControls/FileManagerControl.ascx" TagName="FileManagerUC" TagPrefix="ucFileManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<form id="frmFileManager" runat="server">
	<div>
		<ucFileManager:FileManagerUC ID="FileManagerUC" runat="server" />
	</div>
</form>
	<script src="Scripts/GW.FileManager.js" type="text/javascript"></script>
	<script language="javascript" type="text/javascript">
		$(document).ready(function () {
			var uploadHandler = GW.Common.getBaseURL();
			uploadHandler = uploadHandler + '/Functions/System/FileManagement/UploadHandler.ashx?Action=' + GW.Navigation.currentAction;
			try {
				GW.Upload.BtyesPerChunk = 4194304
				GW.Upload.BtyesPerChunk = 2097152;
				GW.Upload.BtyesPerChunk = 1048576
				GW.Upload.BtyesPerChunk = 3145728;
				//GW.Upload.BtyesPerChunk = 30720;
				GW.Upload.uploadHandler = uploadHandler;
				GW.Upload.init();
			} catch (e) {
				alert("Error in FileManager.aspx document.ready::\n" + e.Message);
			}

			GW.Search.init();
			GW.Search.SearchColumn = 'Name';
			GW.Search.URL = GW.Common.getBaseURL() + "/Functions/System/FileManagement/FileManagerSearchResults.aspx?Action=" + GW.Navigation.currentAction + '&';
			GW.Search.Criteria.SelectedPage = 1;
			GW.Search.Criteria.Columns = 'None, Comes, From, Directory';
			GW.Search.Criteria.OrderByColumn = 'Name';
			GW.Search.Criteria.OrderByDirection = 'ASC';
			GW.Search.Criteria.WhereClause = '"1 = 1"';
			//GW.Search.$CurrentPage = $('#FileManagerUC_SearchControl_currentPage');
			GW.Search.GetSearchResults();

			GW.FileManager.directorySelector = $('#FileManagerUC_directorySelector');

			$(document).bind('~refreshDirectory', function (e) {
				GW.Search.GetSearchResults();
			});

			$(document).bind('~refreshDirectoryLinks', function (e) {
				var options = GW.Model.DefaultWebMethodOptions();
				options.timeout = 600000;
				var theData = {
					'currentDirectoryString': GW.FileManager.currentDirectory,
					'functionSeqId': GW.FileManager.currentFunctionSeqID
				}
				GW.Common.debug(theData);
				// currently in a user control and can't be there must move it to a page
				options.url = GW.Common.getBaseURL() + "/Functions/System/FileManagement/FileManager.aspx/GetDirectoryLinks";
				options.data = theData;
				options.contentType = 'application/json; charset=utf-8';
				options.dataType = 'json';
				GW.Common.JQueryHelper.callWeb(options, getDirectoryLinksSuccess, getDirectoryLinksError);
			});

			function getDirectoryLinksSuccess(xhr) {
				//alert(xhr.d);
				GW.FileManager.directorySelector.html(unescape(xhr.d));
			}

			function getDirectoryLinksError(xhr, status, error) {
				mRetHTML = 'Error deleting files\n' + xhr.responseText;
				alert(mRetHTML);
			}
		});	
	</script>