/// <reference path="GW.Model.js" />
/// <reference path="GW.Common.js" />

// Namespaces
if (typeof GW.Navigation == "undefined" || !GW.Navigation) {
    GW.Navigation = {
        name: 'Growthware Core Web Navigation javascript',
        version: '1.0.0.0',

        currentAction: '',
        // Object to contain information related to
        // updating a "ContentAreaID" using ajax.
        // The action should be used withing the ajax call
        // to supply the data to rendor the given
        // section of the UI
        RefreshObject: function () {
            //properties
            this.ContentAreaID = '';
            //methods
            this.Refresh = function refresh() {
                alert('Please over write this method before registering the RefreshObject');
            }
            return true;
        },

        NavigationObject: function () {
            //properties
            this.Action = '';
            this.Location = '';
            this.Description = '';
            this.LinkBehavior = '';
            return true;
        },

        buildUL: function (parent, items) {
            $.each(items, function () {
                if (this.label) {
                    // create LI element and append it to the parent element.
                    var li = $("<li></li>");
                    // if there are sub items, call the buildUL function.
                    if (this.items && this.items.length > 0) {
                        var anchor = $("<a title='" + this.Description + "' href='#'><span>" + this.label + "</span></a>");
                        anchor.appendTo(li);
                        li.addClass("has-sub");
                        var ul = $("<ul></ul>");
                        ul.appendTo(li);
                        GW.Navigation.buildUL(ul, this.items);
                    } else {
                        var anchor = $("<a title='" + this.Description + "' href='?Action=" + this.Action + "'><span>" + this.label + "</span></a>");
                        anchor.appendTo(li);
                    }
                    li.appendTo(parent);
                }
            });
        },

        buildData: function (menuData) {
            var source = [];
            var items = [];
            // build hierarchical source.
            for (i = 0; i < menuData.length; i++) {
                var item = menuData[i];
                var id = item["MenuID"];
                var label = item["Title"];
                var description = item["Description"];
                var action = item["URL"].replace("?Action=", "");
                action = action.replace("&Action=", "");
                var parentid = item["ParentID"];

                if (items[parentid]) {
                    var item = { parentid: parentid, label: label, Description: description, Action: action, item: item };
                    if (!items[parentid].items) {
                        items[parentid].items = [];
                    }
                    items[parentid].items[items[parentid].items.length] = item;
                    items[id] = item;
                }
                else {
                    items[id] = { parentid: parentid, label: label, Description: description, Action: action, item: item };
                    source[id] = items[id];
                }
            }
            return source;
        },

        NavigationController: {
            // Methods
            RegisterRefreshObject: function (refreshObject) {
                $(document).bind('~refreshUI', function (e) {
                    refreshObject.Refresh();
                });
            },

            RegisterNavigationObject: function (navigationObject) {
                if (!GW.Common.arrayContains(navigationObject, this.NavigationObjects)) {
                    this.NavigationObjects.push(navigationObject);
                }
            },

            LoadPage: function (action, containerID) {
                GW.Navigation.currentAction = action;
                var mRetHTML = "";
                var mNavObject = new this.GetNavigationObject(action);
                if (mNavObject.Location.length > 0) {
                    var mLocation = mNavObject.Location;
                    if (mLocation.substr(0, 1) != "/") mLocation = "/" + mLocation;
                    var mURL = GW.Common.getBaseURL() + mLocation + "?Action=" + action;
                    var vars = [], hash;
                    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                    for (var i = 0; i < hashes.length; i++) {
                        hash = hashes[i].split('=');
                        vars[hash[0]] = hash[1];
                    }
                    for (var i in vars) {
                        if (i != "Action") {
                            mURL += "&" + i + "=" + vars[i];
                        }
                    }
                    var $contentContainer = $('#' + containerID);
                    var $pageMessage = $('#pageMessage');
                    switch (mNavObject.LinkBehavior) {
                        case 1: // internal
                            var options = GW.Model.DefaultWebMethodOptions();
                            options.url = mURL;
                            options.async = true;
                            options.abortable = true;
                            GW.Common.JQueryHelper.callWeb(options, successLoadPage, errorLoadPage);
                            break;
                        case 2: // Popup
                            var options = GW.Model.DefaultDialogOptions();
                            mLocation = mLocation.substring(1, mLocation.length);
                            options.type = 'get';
                            options.async = false;
                            options.resizable = true;
                            options.url = mLocation;
                            options.buttons = {
                                'Cancel': function () { $(this).dialog("close"); }
                            };
                            var dialogId = 'popupFromMenu';
                            GW.Common.JQueryHelper.openDialogWithWebContent(options, dialogId);
                            break;
                        case 3: // External
                            window.open(mNavObject.Location, mNavObject.Description);
                            break;
                        case 4: // NewPage
                            window.open(mURL, mNavObject.Description);
                            break;
                        default: // same as internal
                            var options = GW.Model.DefaultWebMethodOptions();
                            options.url = mURL;
                            options.async = true;
                            GW.Common.JQueryHelper.callWeb(options, successLoadPage, errorLoadPage);
                    }
                } else {
                    GW.Common.debug("Could not find Action:" + action);
                }

                function successLoadPage(msg) {
                    $contentContainer.html(msg.toString());
                    if (containerID == "MainContentDiv") {
                        $pageMessage.css({ display: 'none' });
                        $pageMessage.html(mNavObject.Description).fadeIn(1000);
                    }
                };

                function errorLoadPage(xhr, status, error) {
                    if (!m_Development) {
                        mRetHTML = 'Error getting content';
                        mRetHTML += '\nStatus: ' + status;
                        mRetHTML += '\nError: ' + error;
                        $contentContainer.css({ display: 'none' });
                        $contentContainer.html(mRetHTML.toString()).fadeIn(1000);
                    } else {
                        $pageMessage.css({ display: 'none' });
                        $pageMessage.html(mURL.toString()).fadeIn(1000)
                        if (containerID == "MainContentDiv") {
                            mRetHTML = 'Error getting content\n' + xhr.responseText;
                            mRetHTML += '\nStatus: ' + status;
                            mRetHTML += '\nError: ' + error;
                        } else {
                            mRetHTML = 'Error getting content';
                            mRetHTML += '\nStatus: ' + status;
                            mRetHTML += '\nError: ' + error;
                        }
                        $contentContainer.css({ display: 'none' });
                        $contentContainer.html(mRetHTML.toString()).fadeIn(1000);
                    }
                };
            },

            LoadFunctions: function (onSuccess) {
                this.NavigationObjects = new Array();
                GW.Navigation.createReloadUIEventHandler();
                GW.Common.debug('Creating ~refreshUI');
                var options = GW.Model.DefaultWebMethodOptions();
                options.url = GW.Common.getBaseURL() + "/gw/api/Functions/GetFunctionData";
                options.async = false;
                options.contentType = 'application/json; charset=utf-8';
                options.dataType = 'json';
                options.type = "GET";
                GW.Common.JQueryHelper.callWeb(options, loadFunctionsSuccess, loadFunctionsError);

                function loadFunctionsSuccess(xhr) {
                    var list = xhr;
                    for (i = 0; i < list.length; i++) {
                        var mNavigationObject = new GW.Navigation.NavigationObject();
                        mNavigationObject.Action = list[i].Action;
                        mNavigationObject.Location = list[i].Location;
                        mNavigationObject.Description = list[i].Description;
                        mNavigationObject.LinkBehavior = list[i].LinkBehavior;
                        GW.Navigation.NavigationController.RegisterNavigationObject(mNavigationObject);
                    }
                    if (typeof (onSuccess) == 'function') {
                        onSuccess();
                    }
                    jQuery.event.trigger('~refreshUI', '');
                }

                function loadFunctionsError(xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            },

            GetNavigationObject: function (action) {
                var mRetVal = new GW.Navigation.NavigationObject();
                for (var i = 0; i < GW.Navigation.NavigationController.NavigationObjects.length; i++) {
                    if (GW.Navigation.NavigationController.NavigationObjects[i].Action == action) {
                        mRetVal = GW.Navigation.NavigationController.NavigationObjects[i];
                        break;
                    }
                }
                return mRetVal;
            },

            Refresh: function () {
                GW.Common.debug('Triggering event ~refreshUI');
                jQuery.event.trigger('~refreshUI', '');
            },

            // Properties
            Count: function count() {
                return m_RefreshObjects.length;
            },

            NavigationObjects: new Array(),

            RefreshObjects: new Array()
        },

        createReloadUIEventHandler: function () {
            $(document).bind('~reLoadUI', function () {
                location.reload(true);
            });
        }
    }
};
var m_Development = true;