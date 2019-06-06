/// <reference path="GW.Model.js" />
/// <reference path="GW.Common.js" />

// Name spaces
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

        createReloadUIEventHandler: function () {
            $(document).bind('~reLoadUI', function () {
                location.reload(true);
            });
        },

        NavigationController: {

            Reload: function () {
                GW.Common.debug('Triggering event ~reLoadUI');
                jQuery.event.trigger('~reLoadUI', '');
            }

        },

        NavigationObject: function () {
            //properties
            this.Action = '';
            this.Location = '';
            this.Description = '';
            this.Controller = '';
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
                        var anchor = $("<a title='" + this.Description + "' href='" +this.Action + "?Action=" +this.Action + "'><span>" +this.label + "</span></a>");
                        anchor.appendTo(li);
                    }
                    li.appendTo(parent);
                }
            });
        },

        buildData: function (menuData) {
            var source = [];
            var items = [];
            var sourceId = 0;
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
                    source[sourceId] = items[id];
                }
                sourceId = sourceId + 1;
            }
            //GW.Common.debug(source);
            return source;
        },
    }
};
var m_Development = true;