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
            var windowLocation = window.location.search;
            if (windowLocation.length == 0) windowLocation = document.location.hash;
            windowLocation = windowLocation.replace('#', '');
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(windowLocation);
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

        isNullOrEmpty: function (obj) {
            if (obj === undefined || obj == null) {
                return true;
            }
            if (typeof obj == 'string') {
                if (!obj || obj.trim() === "" || (obj.trim()).length === 0) {
                    return true;
                }
            }
            return false;
        },

        showHelpMSG: function (html, title) {
            var dialogMessageTemplate = html;
            var dialogId = '#helpPopup';
            var options = GW.Model.DefaultDialogOptions();
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
                    return this.getCookieVal(j);
                i = document.cookie.indexOf(" ", i) + 1;
                if (i == 0) break;
            }
            return null;
        },

        removeParameter: function (url, parameter){
          var urlparts= url.split('?');

            if (urlparts.length>=2)
            {
                var urlBase=urlparts.shift(); //get first part, and remove from array
                var queryString=urlparts.join("?"); //join it back up

                var prefix = encodeURIComponent(parameter)+'=';
                var pars = queryString.split(/[&;]/g);
                for (var i= pars.length; i-->0;)               //reverse iteration as may be destructive
                    if (pars[i].lastIndexOf(prefix, 0)!==-1)   //idiom for string.startsWith
                        pars.splice(i, 1);
                url = urlBase+'?'+pars.join('&');
            }
            return url;
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
            var cval = this.getCookie("name");
            document.cookie = name + "=" + cval + "; expires=" + exp.toGMTString();
            return;
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
        naturalSort: function (index) {
            index = index || 0;
            return function (a, b) {
                var re = /(^-?[0-9]+(\.?[0-9]*)[df]?e?[0-9]?$|^0x[0-9a-f]+$|[0-9]+)/gi,
                    sre = /(^[ ]*|[ ]*$)/g,
                    dre = /(^([\w ]+,?[\w ]+)?[\w ]+,?[\w ]+\d+:\d+(:\d+)?[\w ]?|^\d{1,4}[\/\-]\d{1,4}[\/\-]\d{1,4}|^\w+, \w+ \d+, \d{4})/,
                    hre = /^0x[0-9a-f]+$/i,
                    ore = /^0/,
                // convert all to strings and trim()
                    x = a[index].toString().replace(sre, '') || '',
                    y = b[index].toString().replace(sre, '') || '',
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
                for (var cLoc = 0, numS = Math.max(xN.length, yN.length) ; cLoc < numS; cLoc++) {
                    // find floats not starting with '0', string or 0 if not defined (Clint Priest)
                    var oFxNcL = !(xN[cLoc] || '').match(ore) && parseFloat(xN[cLoc]) || xN[cLoc] || 0;
                    var oFyNcL = !(yN[cLoc] || '').match(ore) && parseFloat(yN[cLoc]) || yN[cLoc] || 0;
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
            }
        },
        /********** End Natural Sorting *****************/
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
                                alert('Text is too long');
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
