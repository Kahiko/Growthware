/// <reference path="GW.Common.js" />

if (typeof GW == "undefined" || !GW) {
	window.GW = { name: 'Growthware Core Web', version: '1.0.0.0' };
};
GW.FileManager = {

	currentDirectory: {},

	currentFunctionSeqID: {},

	directorySelector: {},

	downLoad: function (thePath, fileName, functionSeqID) {
		var mParameters = '&thePath=' + thePath + '&functionSeqID=' + functionSeqID + '&fileName=' + fileName + '&'
		var mURL = GW.Common.getBaseURL() + "/Functions/System/FileManagement/DownloadHandler.ashx?Action=" + GW.Navigation.currentAction + mParameters
		window.location = mURL;
	},

	changeDirectory: function (thePath, functionSeqID) {
		this.currentDirectory = escape(thePath);
		this.currentFunctionSeqID = functionSeqID;
		GW.Search.URL = GW.Common.getBaseURL() + "/Functions/System/FileManagement/FileManagerSearchResults.aspx?Action=" + GW.Navigation.currentAction + '&desiredPath=' + escape(thePath) + '&functionSeqID=' + functionSeqID + '&';
		GW.Common.debug(GW.Search.URL);
		GW.Search.Criteria.SelectedPage = 1;
		GW.Search.Criteria.Columns = 'None, Comes, From, Directory';
		GW.Search.Criteria.OrderByColumn = 'Name';
		GW.Search.Criteria.OrderByDirection = 'ASC';
		GW.Search.Criteria.WhereClause = '"1 = 1"';
		GW.Upload.uploadDirectory = thePath;
		this.trig('~refreshDirectoryLinks', '');
		this.trig('~refreshDirectory', '');
	},

	createDirectory: function () {
		GW.Common.debug('createDirectory start');
		var options = GW.Model.DefaultWebMethodOptions();
		var theData = {
		    'currentDirectoryString': escape(GW.FileManager.currentDirectory),
		    'functionSeqId': GW.FileManager.currentFunctionSeqID,
		    'newDirectory': escape($("#FileManagerUC_txtNewDirectory").val())
		}
		GW.Common.debug('theData: ' + JSON.stringify(theData));
		options.url = GW.Common.getBaseURL() + "/gw/api/FileManager/CreateDirectory";
		options.data = theData;
		options.contentType = 'application/json; charset=utf-8';
		options.dataType = 'json';
		GW.Common.JQueryHelper.callWeb(options, GW.FileManager.createDirectorySuccess, GW.FileManager.createDirectoryError);
		GW.Common.debug('createDirectory done');
	},

	createDirectorySuccess: function (xhr) {
		GW.Common.debug('created directory');
		GW.FileManager.trig('~refreshDirectory', '');
	},

	createDirectoryError: function (xhr, status, error) {
		GW.Common.debug('status: ' + status + ' error: ' + error);
	},

	trig: function (eventType, extraParameters) {
		jQuery.event.trigger(eventType, extraParameters);
	}
}


