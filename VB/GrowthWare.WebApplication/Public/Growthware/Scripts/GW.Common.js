/// <reference path="GW.Model.js" />

// Array Remove - By John Resig (MIT Licensed)
Array.remove = function (array, from, to) {
	var rest = array.slice((to || from) + 1 || array.length);
	array.length = from < 0 ? array.length + from : from;
	return array.push.apply(array, rest);
};

if (!"console" in window || typeof console == "undefined") {
	var methods = ["log", "debug", "info", "warn", "error", "assert", "dir", "dirxml", "group", "groupEnd", "time", "timeEnd", "count", "trace", "profile", "profileEnd"];
	var emptyFn = function () { };
	window.console = {};
	for (var i = 0; i < methods.length; ++i) {
		window.console[methods[i]] = emptyFn;
	}
}

// Namespaces
if (typeof GW == "undefined" || !GW) {
	window.GW = {
		name: 'Growthware Core Web',
		version: '1.0.0.0'
	};
};

if (typeof GW.Common == "undefined" || !GW.Common) {
	GW.Common = {

		requestObj: function () {
			var mRetVal = {
				url: '',
				webRequestObj: []
			};
			return mRetVal;
		},

		name: 'Growthware Core Web Common javascript',

		version: '1.0.0.0',

		isDebug: true,

		debug: function (msg) {
			if (this.isDebug && console.debug) {
				console.debug(msg);
			}
		},

		getParameterByName: function (name) {
			name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
			var regexS = "[\\?&]" + name + "=([^&#]*)";
			var regex = new RegExp(regexS);
			var results = regex.exec(window.location.search);
			if (results == null)
				return "";
			else
				return decodeURIComponent(results[1].replace(/\+/g, " "));
		},

		getBaseURL: function () {
			var mCurrentLocation = window.location;
			var mPort = mCurrentLocation.port;
			var mCurrentPath = window.location.pathname
			if (mPort == "80" || mPort.length == 0) {
				mPort = "";
			} else {
				mPort = ":" + mPort;
			}
			var mURL = mCurrentLocation.protocol + "//" + mCurrentLocation.hostname + mPort;
			var n = mCurrentPath.lastIndexOf("/");
			if (n != 0) {
				mURL = mURL + '/' + mCurrentPath;
			}
			return mURL;
		},

		arrayContains: function (obj, objArray) {
			for (var i = 0; i < objArray.length; i++) {
				if (objArray[i] === obj) {
					return true;
				}
			}
			return false;
		},

		showHelpMSG: function (html, title) {
			var dialogMessageTemplate = html;
			var dialogId = '#helpPopup';
			var options = GW.Model.DefaultDialogOptions();
			options.title = title;
			options.width = 300;
			options.resizable = true;
			$helpDialog = this.JQueryHelper.getDialog(options, dialogId, dialogMessageTemplate);
			$helpDialog.dialog("open");
		},

		sleep: function (milliSeconds) {
			var startTime = new Date().getTime(); // get the current time
			while (new Date().getTime() < startTime + milliSeconds); // hog cpu
		},

		getQueryParams: function (documentLocationSearch) {
			documentLocationSearch = documentLocationSearch.split("+").join(" ");
			var params = {},
				tokens, re = /[?&]?([^=]+)=([^&]*)/g;

			while (tokens = re.exec(documentLocationSearch)) {
				params[decodeURIComponent(tokens[1])] = decodeURIComponent(tokens[2]);
			}

			return params;
		},

		getCookieVal: function (offset) {
			var endstr = document.cookie.indexOf(";", offset);
			if (endstr == -1)
				endstr = document.cookie.length;
			return unescape(document.cookie.substring(offset, endstr));
		},

		getCookie: function (name) {
			var arg = name + "=";
			var alen = arg.length;
			var clen = document.cookie.length;
			var i = 0;
			while (i < clen) {
				var j = i + alen;
				if (document.cookie.substring(i, j) == arg)
					return getCookieVal(j);
				i = document.cookie.indexOf(" ", i) + 1;
				if (i == 0) break;
			}
			return null;
		},

		setCookie: function (name, value) {
			var argv = setCookie.arguments;
			var argc = setCookie.arguments.length;
			var expires = (argc > 2) ? argv[2] : null;
			var path = (argc > 3) ? argv[3] : null;
			var domain = (argc > 4) ? argv[4] : null;
			var secure = (argc > 4) ? argv[5] : false;
			document.cookie = name + "=" + escape(value) + ((expires == null) ? "" : ("; expires=" + expires.toGMTString())) + ((path == null) ? "" : ("; path=" + path)) + ((domain == null) ? "" : ("; domain=" + domain));
		},

		deleteCookie: function (name) {
			exp = new Date();
			exp.setTime(exp.getTime() - 1);
			var cval = getCookie("name");
			document.cookie = name + "=" + cval + "; expires=" + exp.toGMTString();
			return;
		},

		highlightAll: function (theField) {
			var copytoclip = 1;
			var tempval = $('#' + theField);
			tempval.focus()
			tempval.select()
			if (document.all && copytoclip == 1) {
				therange = tempval.createTextRange()
				therange.execCommand("Copy")
				window.status = "Contents highlighted and copied to clipboard!"
				setTimeout("window.status=''", 1800)
			}
		},

		checkSpecialKeys: function (event) {
			if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 35 && event.keyCode != 36 && event.keyCode != 37 && event.keyCode != 38 && event.keyCode != 39 && event.keyCode != 40)
				return false;
			else
				return true;
		},


	    /********** Natural Sorting *****************/
	    /*
        * Natural Sort algorithm for Javascript - Version 0.6 - Released under MIT license
        * Author: Jim Palmer (based on chunking idea from Dave Koelle)
        * Contributors: Mike Grier (mgrier.com), Clint Priest, Kyle Adams, guillermo
        */
		naturalSort: function (a, b) {
		    var re = /(^-?[0-9]+(\.?[0-9]*)[df]?e?[0-9]?$|^0x[0-9a-f]+$|[0-9]+)/gi,
                sre = /(^[ ]*|[ ]*$)/g,
                dre = /(^([\w ]+,?[\w ]+)?[\w ]+,?[\w ]+\d+:\d+(:\d+)?[\w ]?|^\d{1,4}[\/\-]\d{1,4}[\/\-]\d{1,4}|^\w+, \w+ \d+, \d{4})/,
                hre = /^0x[0-9a-f]+$/i,
                ore = /^0/,
            // convert all to strings and trim()
                x = a.toString().replace(sre, '') || '',
                y = b.toString().replace(sre, '') || '',
            // chunk/tokenize
                xN = x.replace(re, '\0$1\0').replace(/\0$/, '').replace(/^\0/, '').split('\0'),
                yN = y.replace(re, '\0$1\0').replace(/\0$/, '').replace(/^\0/, '').split('\0'),
            // numeric, hex or date detection
                xD = parseInt(x.match(hre)) || (xN.length != 1 && x.match(dre) && Date.parse(x)),
                yD = parseInt(y.match(hre)) || xD && y.match(dre) && Date.parse(y) || null;
		    // first try and sort Hex codes or Dates
		    if (yD)
		        if (xD < yD) return -1;
		        else if (xD > yD) return 1;
		    // natural sorting through split numeric strings and default strings
		    for (var cLoc = 0, numS = Math.max(xN.length, yN.length); cLoc < numS; cLoc++) {
		        // find floats not starting with '0', string or 0 if not defined (Clint Priest)
		        oFxNcL = !(xN[cLoc] || '').match(ore) && parseFloat(xN[cLoc]) || xN[cLoc] || 0;
		        oFyNcL = !(yN[cLoc] || '').match(ore) && parseFloat(yN[cLoc]) || yN[cLoc] || 0;
		        // handle numeric vs string comparison - number < string - (Kyle Adams)
		        if (isNaN(oFxNcL) !== isNaN(oFyNcL)) return (isNaN(oFxNcL)) ? 1 : -1;
		            // rely on string comparison if different types - i.e. '02' < 2 != '02' < '2'
		        else if (typeof oFxNcL !== typeof oFyNcL) {
		            oFxNcL += '';
		            oFyNcL += '';
		        }
		        if (oFxNcL < oFyNcL) return -1;
		        if (oFxNcL > oFyNcL) return 1;
		    }
		    return 0;
		},
	    /********** End Natural Sorting *****************/

		JQueryHelper: {
			// Make a JQuery.ajax call to a web method defined in options.url
			//
			// Example:
			// var options = {async: false, url: 'someurl', data: {mydata: 'string'} };
			// var request = webMethod(options, onSuccessCallBack, onErrorCallBack);
			//
			callWeb: function (customOptions, onSuccess, onError) {
				var defaultOptions = GW.Model.DefaultWebMethodOptions();
				var options = $.extend({}, defaultOptions, customOptions);
				if ($.type(options.data) != 'string') {
					options.data = JSON.stringify(options.data);
				}
				if (options.url.indexOf("?Action=") == -1 && options.url.indexOf("&Action=") == -1) {
					if (options.url.indexOf("?") == -1) {
						options.url += "?Action=" + GW.Navigation.currentAction;
					} else {
						options.url += "&Action=" + GW.Navigation.currentAction;
					}
				}
				GW.Common.debug(options.url);
				var $webRequest = new $.ajax({
					url: options.url,
					type: options.type,
					async: options.async,
					cache: options.cache,
					timeout: options.timeout,
					data: options.data,
					dataType: options.dataType,
					contentType: options.contentType,
					success: function (xhr) {
						GW.Common.JQueryHelper.removeWebRequest(options.url);
						if (typeof (onSuccess) == 'function') {
							onSuccess(xhr);
						} else {
							GW.Common.debug('No onSuccess function provided');
						}
					},
					error: function (xhr, status, error) {
						GW.Common.JQueryHelper.removeWebRequest(options.url);
						if (typeof (onError) == 'function') {
							onError(xhr, status, error);
						} else {
							var msgText = 'Error GW.Common.JQueryHelper.callWeb::\n responseText:' + xhr.responseText + '\n';
							msgText = msgText + 'status: ' + status + '\n';
							msgText = msgText + 'error: ' + error;
							throw (msgText);
						}
					}
				});
				if (this.abortableRequests.length == 0) {
					if (options.abortable) {
						GW.Common.debug('no abortable web request adding new one');
						var mRequestObject = GW.Common.requestObj();
						mRequestObject.url = options.url;
						mRequestObject.webRequestObj = $webRequest;
						this.abortableRequests.push(mRequestObject);
					}
				} else {
					if (options.abortable) {
						GW.Common.debug('Adding to existing web requests');
						var mTempRequests = new Array();
						for (var i = this.abortableRequests.length - 1; i > -1; i--) {
							var mRequestObject = this.abortableRequests[i];
							mTempRequests.push(mRequestObject);
						}
						var mRequestObject = GW.Common.requestObj();
						mRequestObject.url = options.url;
						mRequestObject.webRequestObj = $webRequest;
						mTempRequests.push(mRequestObject);
						this.abortableRequests = mTempRequests;
					}
				}
				return $webRequest
			},

			abortableRequests: new Array(),

			abort: function () {
				for (var i = this.abortableRequests.length - 1; i > -1; i--) {
					this.abortableRequests[i].webRequestObj.abort();
					Array.remove(this.abortableRequests, i);
					GW.Common.debug('Aborted and Removed webRequestObj[' + i + '] from the array');
				}
			},

			removeWebRequest: function (url) {
				for (var i = this.abortableRequests.length - 1; i > -1; i--) {
					var mRequestObject = this.abortableRequests[i];
					if (this.abortableRequests[i].url == url) {
						GW.Common.debug('Removing web request from array');
						Array.remove(this.abortableRequests, i);
					}
				}

			},

			customConfirm: function (dialogId, height, width, okFunc, cancelFunc, dialogTitle, dialogMessageTemplate, okCallBackData) {
				if (dialogId.substring(0, 1) == "#") dialogId = dialogId.substring(1, dialogId.length - 1);
				var $popupBodyDialogWindow = {};
				if ($(dialogId).length > 0) {
					$popupBodyDialogWindow = $(dialogId);
				} else {
					$popupBodyDialogWindow = $('<div id="' + dialogId + '"></div>');
					$popupBodyDialogWindow.appendTo('body');
				}
				dialogId = '#' + dialogId;
				if ($(dialogId).length > 0) {
					var $dialogElement = $(dialogId);
					$dialogElement.html('');
					$dialogElement.dialog({
						draggable: false,
						modal: true,
						autoOpen: false,
						resizable: false,
						autoOpen: false,
						height: height,
						width: width,
						title: dialogTitle || 'Confirm',
						buttons: {
							OK: function () {
								if (typeof (okFunc) == 'function') {
									if (okCallBackData != null) {
										okFunc(okCallBackData);
									} else {
										okFunc();
									}
								}
								$(this).dialog("close");
							},
							Cancel: function () {
								if (typeof (cancelFunc) == 'function') {
									setTimeout(cancelFunc, 50);
								}
								$(this).dialog("close");
							}
						},
						close: function (event, ui) {
						    $(this).dialog('destroy').remove();
						}
					});

					// Set the HTML
					$dialogElement.html(dialogMessageTemplate);
					// open the dialog box.
					$dialogElement.dialog("open");
				} else {
					alert('The dialogId "' + dialogId + '" does not exist!');
				}
			},

			openDialog: function (dialogOptions, dialogId, dialogMessageTemplate) {
				if (dialogId.substring(0, 1) != "#") dialogId = "#" + dialogId;
				var defaultOptions = GW.Model.DefaultDialogOptions();
				var options = $.extend({}, defaultOptions, dialogOptions);
				$(dialogId).dialog({
					autoOpen: options.autoOpen,
					buttons: options.buttons,
					closeOnEscape: options.closeOnEscape,
					closeText: options.closeText,
					dialogClass: options.dialogClass,
					disabled: options.disabled,
					draggable: options.draggable,
					height: options.height,
					hide: options.hide,
					modal: options.modal,
					position: options.position,
					resizable: options.resizable,
					show: options.show,
					stack: options.stack,
					title: options.title,
					width: options.width,
					zindex: options.zindex
				});

				// Set the HTML
				$(dialogId).html(dialogMessageTemplate);
				// open the dialog box if needed
				if (!options.autoOpen) {
					$(dialogId).dialog("open");
				}
			},

			getDialog: function (dialogOptions, dialogId, dialogMessageTemplate) {
			    if (dialogId.substring(0, 1) != "#") dialogId = "#" + dialogId;
			    if (typeof jQuery.ui != 'undefined') {
			        var defaultOptions = GW.Model.DefaultDialogOptions();
			        var options = $.extend({}, defaultOptions, dialogOptions);
			        if ($(dialogId).length > 0) {
			            var $dialogId = $(dialogId);
			            $dialogId.html('');
			            $dialogId.dialog({
			                autoOpen: false,
			                buttons: options.buttons,
			                closeOnEscape: options.closeOnEscape,
			                closeText: options.closeText,
			                dialogClass: options.dialogClass,
			                disabled: options.disabled,
			                draggable: options.draggable,
			                height: options.height,
			                hide: options.hide,
			                cache: options.cache,
			                modal: options.modal,
			                position: options.position,
			                resizable: options.resizable,
			                show: options.show,
			                stack: options.stack,
			                title: options.title,
			                width: options.width,
			                zindex: options.zindex
			            });
			            // Set the HTML
			            $dialogId.html(dialogMessageTemplate);
			            return $(dialogId);
			        } else {
			            alert('The dialogId "' + dialogId + '" does not exist!');
			        }
			    } else {
			        var options = $.extend({}, defaultOptions, dialogOptions);
			        var dialog = new BootstrapDialog({
			            title: options.title,
			            message: dialogMessageTemplate
			        });
			        dialog.open();
			    }
			},

			/*
			Adds the contents of a web page to the containerId.html by using jQuery's load method.
			1.) Get the container as a jQuery object.
			2.) Set the container's html to ''.
			3.) Destroys the containers dialog to clean up the dom as much as possible.
			4.) Sets the container's dialog options using autoOpen as true and the options associated
			with the dialogOptions object.

			var dialogId = '#popupAddEditAccount';
			var options = GW.Model.DefaultDialogOptions();
			options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Accounts/AddEditAccount.aspx?AccountSeqID=" + mAccountSeqID;
			options.title = 'Edit Account';
			options.height = 600;
			options.width = 1100;
			options.resizable = true;
			options.buttons = {
			'Save': function () { saveAddEditAccount($(this)); },
			'Cancel': function () { $(this).dialog("close"); }
			};

			@param dialogOptions from GW.Model.DefaultDialogOptions
			@param containerId general the id of a div
		  
			@type jQuery
		 
			@name OpenDialogWithWebContent
			@cat GW.Common.JQueryHelper
			@author Michael Regan
			*/
			openDialogWithWebContent: function (dialogOptions, dialogId) {
			    var defaultOptions = GW.Model.DefaultDialogOptions();
			    var options = $.extend({}, defaultOptions, dialogOptions);
			    if (typeof jQuery.ui != 'undefined') {
				    if (dialogId.substring(0, 1) != "#") dialogId = "#" + dialogId;
				    var newElementID = dialogId.substring(1, dialogId.length) + 'DialogWindow';
				    var $popupBodyDialogWindow = {};
				    if ($('#' + newElementID).length > 0) {
				        $popupBodyDialogWindow = $('#' + newElementID);
				    } else {
				        $popupBodyDialogWindow = $('<div id="' + newElementID + '"></div>');
				        $popupBodyDialogWindow.appendTo('body');
				    }
				    $popupBodyDialogWindow.load(options.url, function (response, status, xhr) {
				        if (status != "error") {
				            $(this).dialog({
				                autoOpen: true,
				                buttons: options.buttons,
				                closeOnEscape: options.closeOnEscape,
				                closeText: options.closeText,
				                dialogClass: options.dialogClass,
				                disabled: options.disabled,
				                draggable: options.draggable,
				                height: options.height,
				                hide: options.hide,
				                cache: options.cache,
				                modal: options.modal,
				                position: options.position,
				                resizable: options.resizable,
				                show: options.show,
				                stack: options.stack,
				                title: options.title,
				                width: options.width,
				                zindex: options.zindex,
				                close: function (event, ui) {
				                    $(this).dialog('destroy').remove();
				                }
				            });
				        } else {
				            var msg = "Sorry but there was an error: ";
				            $container.html(msg + xhr.status + " " + xhr.statusText);
				        }
				    });
			    } else {
			        //BootstrapDialog.show({
			        //    title: options.title,
			        //    message: $('<div></div>').load(options.url)
			        //});


			        if (!$('#myModal').length) {
			            $('body').append(GW.Model.BoostrapModal);
			        }
			        var $mModal = $('#myModal');
			        $mModal.on("show.bs.modal", function () {
			            var height = $(window).height() - 200;
			            $(this).find(".modal-body").css("max-height", height);
			        });

			        $mModal.modal({
			            backdrop: false,
			            show: false,
			            keyboard: true
			        });
			        $mModal.on('show', function () {
			            $('.modal-body', this).css({ width: options.width, height: options.height, 'max-height': '100%', 'max-width': '100%' });
			        });
			        var mModalTitle = $('#myModalTitle');
                    var saveBtnFunction
			        mModalTitle.html(options.title);
			        $.each(options.buttons, function (key, value) {
			            if (key == 'Save') {
			                saveBtnFunction = value;
			            }
			        });
			        //$('#MainContentDiv').load(options.url);
			        $('.modal-body').load(options.url);
			        //$mModal.find('.callback-btn').off('click.callback').on('click.callback', function () {saveBtnFunction;$mModal.modal('hide');}).end();
			        $('#mModalBtnSave').click(function () {
			            eval("var fn = " + saveBtnFunction);
			            fn();
			            $('#myModal').modal('hide');
			        });
				    $mModal.modal('show');

				}
			}

		} // End GW.Common.JQueryHelper namespace
	};
};

if (typeof GW.Common.Validation == "undefined" || !GW.Common.Validation) {
	GW.Common.Validation = {
		name: 'Growthware Core Web Common Validation javascripts',

		version: '1.0.0.0',

		isDebug: true,

		textboxMultilineMaxNumber: function (txt, maxLen, msg, event) {
//			if (msg.length == 'undefined' || !msg.length) {
//				event = msg;
//			}
			try {
				if (!GW.Common.checkSpecialKeys(event)) {
					if (txt.value.length > (maxLen - 1)) {
						try {
							if (msg !== undefined) {
								alert(msg);
							} else {
								alert('Text is too nong');
							}
						} catch (err) {
							// do nothing
						}
						return false;
					}
				}
			} catch (e) {
				// do nothing
			}
		}
	};
};
