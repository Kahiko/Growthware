/// <reference path="GW.Common.js" />

// Namespaces
if (typeof GW == "undefined" || !GW) {
	window.GW = {
		name: 'Growthware Core Web',
		version: '1.0.0.0'
	};
};

/// <reference path="GW.Search.js" />

if (typeof GW.Upload == "undefined" || !GW.Upload) {
	GW.Upload = {
		name: 'Growthware Core Web Upload javascript',

		version: '1.0.0.0',

		PROGRESS_INTERVAL: 500,

		PROGRESS_COLOR: '#000080',

		BtyesPerChunk: 2097152,

		_divFrame: {},
		_divUploadMessage: {},
		_divUploadProgress: {},

		_loopCounter: 1,
		_maxLoop: 10,
		_uploadProgressTimer: {},

		uploadHandler: '',

		uploadDirectory: '\\',

		form: function () {
		    var retVal = "<form id='uploadForm' method='post' enctype='multipart/form-data'>";
		    retVal = retVal + "	<table>";
		    retVal = retVal + "	<tr>";
		    retVal = retVal + "	<td>";
		    retVal = retVal + "	<div id='divUpload' style='padding-top: 4px'>";
		    retVal = retVal + "		<img alt='Upload File' onclick='javascript:alert(" + "\"" + "hi" + "\"" + ");' id='btnUpload' src='" + GW.Common.getBaseURL() + "/Public/GrowthWare/Images/Add.png' height='16px' width='16px' />";
		    retVal = retVal + "	</div>"
		    retVal = retVal + "	</td>";
		    retVal = retVal + "	<td>";
		    retVal = retVal + "		<input id='fileUploadControl' type='file' />";
		    retVal = retVal + "	</td>";
		    retVal = retVal + "	</tr>";
		    retVal = retVal + "	</table>";
		    retVal = retVal + "</form>";
		    return retVal;
			return retVal;
		},

		userFile: {},

		doUpload: function () {
			alert('hi');
		},

		iframe: function () {
			var mRetVal = '<iframe id="ifrUpload" scrolling="no" frameborder="0" hidefocus="true" style="text-align:center;vertical-align:middle;border-style:none;margin:0px;width:100%;height:55px"></iframe>'
			return mRetVal;
		},

		init: function () {
			try {
				GW.Upload._divUploadProgress = $('#divUploadProgress');
				GW.Upload._divUploadMessage = $('#divUploadMessage');
				GW.Upload._divFrame = $('#divFrame');

				GW.Upload.uploadComplete(' ', false);

				var form = GW.Upload.form();
				// add the iframe to the iframe div
				GW.Upload._divFrame.append(GW.Upload.iframe());

				var t = setInterval(function () {
					if (document.getElementById('ifrUpload') !== 'undefined') {
						var mUploadIframe = document.getElementById('ifrUpload');
						if (mUploadIframe != null) {
							// add the form to the iframe
							$('#ifrUpload').contents().find('body').append(form);
							if (mUploadIframe.contentWindow.document.getElementById('btnUpload') !== 'undefined' && mUploadIframe.contentWindow.document.getElementById('btnUpload') != null) {
								clearInterval(t);
								var btnUpload = mUploadIframe.contentWindow.document.getElementById('btnUpload');
								btnUpload.onclick = function (event) {
									GW.Upload._divUploadMessage.css("display", "none");
									GW.Upload.beginPhotoUploadProgress();
									var $selectedFile = $('#ifrUpload').contents().find('#fileUploadControl').get(0).files[0];
									var uri = GW.Upload.uploadHandler + "&CurrentDirectory=" + GW.Upload.uploadDirectory;
									if (!$selectedFile) {
										try {
											GW.Upload.uploadComplete("Please specify the file.", true)
										} catch (e) {
											alert(e);
										}
										return;
									}
									//https://github.com/weixiyen/jquery-filedrop/issues/23
									GW.Upload.upload($selectedFile, uri);
								}
							}
						}
					}
				}, 15);

			} catch (e) {
				// do nothing most likely cause is due to elements being hidden on the form for security
				// reasons.
			}
		},

		upload: function (file, uri) {
			//http://forums.asp.net/t/1742612.aspx/1
			if (!FormData) {
				GW.Upload.uploadComplete('Sorry, your browser doesn\'t support the File API', true);
				return true;
			}
			var BYTES_PER_CHUNK = GW.Upload.BtyesPerChunk;
			var SIZE = file.size;

			//upload content
			var start = 0;
			var end = BYTES_PER_CHUNK;
			var completed = 0;
			//var count = SIZE % BYTES_PER_CHUNK == 0 ? SIZE / BYTES_PER_CHUNK : Math.floor(SIZE / BYTES_PER_CHUNK) + 1;
			var count = SIZE % BYTES_PER_CHUNK == 0 ? SIZE / BYTES_PER_CHUNK : Math.floor(SIZE / BYTES_PER_CHUNK) + 1;
			var uploadNumber = 0;
			var multiUploadFileName = '';
			if (count > 1) {
				while (start < SIZE) {
					uploadNumber++
					//GW.Common.debug('Sending file ' + uploadNumber + ' of ' + count);
					multiUploadFileName = file.name + "_UploadNumber_" + uploadNumber;
					var slice = {};
					if (file.slice) {
						slice = file.slice;
					}
					else if (file.webkitSlice) {
						slice = file.webkitSlice;
					}
					else if (file.mozSlice) {
						slice = file.mozSlice;
					}
					else {
						GW.Upload.uploadComplete('Sorry, your browser doesn\'t support slice', true);
						return;
					}
					var chunk = slice.call(file, start, end);
					var xhr = new XMLHttpRequest();
					xhr.open("POST", uri, true);
					xhr.onload = function () {
						completed++;
						//GW.Common.debug('completed: ' + completed + ' count: ' + count);
						if (completed === count) {
							GW.Upload.multiUploadComplete(file.name, uri);
						}
					};
					var formData = new FormData();
					formData.append(multiUploadFileName, chunk);

					xhr.send(formData);

					start = end;
					end = start + BYTES_PER_CHUNK;
				}
			} else {
				var formData = new FormData();
				formData.append(file.name, file);
				formData.append('single', true);
				var xhr = new XMLHttpRequest();
				xhr.open("POST", uri, true);
				xhr.onreadystatechange = function () {
					if (xhr.readyState == 4 && xhr.status == 200) {
						// Handle response.
						GW.Upload.uploadComplete("Done", false);
						jQuery.event.trigger('~refreshDirectory', '');
					} else {
						if (xhr.readyState == 4 && xhr.status != 200) {
							alert(xhr.status); // handle response.
							GW.Upload.uploadComplete("Upload failed.", true);
						}
					}
				};
				xhr.send(formData);
			}
		},

		multiUploadComplete: function (fileName, uri) {
			var formData = new FormData();
			formData.append('fileName', fileName);
			formData.append('completed', true);

			var xhr2 = new XMLHttpRequest();
			xhr2.open("POST", uri, true); //combine the chunks together
			xhr2.onreadystatechange = function () {
				if (xhr2.readyState == 4 && xhr2.status == 200) {
					// Handle response.
					GW.Upload.uploadComplete("Upload complete.", false);
					GW.Search.GetSearchResults();
				} else {
					if (xhr2.readyState == 4 && xhr2.status != 200) {
						alert(xhr2.status); // handle response.
						GW.Upload.uploadComplete("Upload failed.", true);
						GW.Search.GetSearchResults();
					}
				}
			};
			xhr2.send(formData);
		},

		beginPhotoUploadProgress: function () {
			GW.Upload._divUploadMessage.css("display", "none");
			GW.Upload._divFrame.css("display", "none");
			GW.Upload._divUploadProgress.css("display", "inline-block");
			GW.Upload.clearUploadProgress();
			GW.Upload._uploadProgressTimer = setTimeout(GW.Upload.updateUploadProgress, GW.Upload.PROGRESS_INTERVAL);
		},

		clearUploadProgress: function () {
			for (var i = 1; i <= GW.Upload._maxLoop; i++) {
				document.getElementById('tdProgress' + i).style.backgroundColor = 'transparent';
			}

			document.getElementById('tdProgress1').style.backgroundColor = GW.Upload.PROGRESS_COLOR;
			GW.Upload._loopCounter = 1;
		},

		updateUploadProgress: function () {
			GW.Upload._loopCounter += 1;

			if (GW.Upload._loopCounter <= GW.Upload._maxLoop) {
				document.getElementById('tdProgress' + GW.Upload._loopCounter).style.backgroundColor = GW.Upload.PROGRESS_COLOR;
			}
			else {
				GW.Upload.clearUploadProgress();
			}

			if (GW.Upload._uploadProgressTimer) {
				clearTimeout(GW.Upload._uploadProgressTimer);
			}

			GW.Upload._uploadProgressTimer = setTimeout(GW.Upload.updateUploadProgress, GW.Upload.PROGRESS_INTERVAL);
		},

		uploadComplete: function (message, isError) {
			GW.Upload.clearUploadProgress();

			if (GW.Upload._uploadProgressTimer) {
				clearTimeout(GW.Upload._uploadProgressTimer);
			}

			GW.Upload._divUploadProgress.css("display", "none");
			GW.Upload._divUploadMessage.css("display", "none");
			GW.Upload._divFrame.css("display", "inline-block");

			if (message.length) {
				var color = (isError) ? '#ff0000' : '#008000';

				GW.Upload._divUploadMessage.html('<span style=\"color:' + color + '\;font-weight:bold">' + message + '</span>');
				GW.Upload._divUploadMessage.css("display", "inline-block");
			}
		}
	}
};