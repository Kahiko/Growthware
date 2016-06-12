// used to keep an array of refresh objects
var m_RefreshObjects = new Array();

// Object to contain information related to
// updating a "ContentAreaID" using ajax.
// The action should be used withing the ajax call
// to supply the data to rendor the given
// section of the UI
function RefreshObject() {
    //properties
    this.ContentAreaID = '';
    //methods
    this.Refresh = function refresh() {
        alert('Please over write this method before registering the RefreshObject');
    }
    return true;
}

function NavigationControler() {
    // Methods
    this.RegisterRefreshObject = function (RefreshObject) {
        if (!contains(RefreshObject)) {
            m_RefreshObjects.push(RefreshObject);
        }

    };

    this.Load = function loader2(action, containerID) {
        loader3(action, confirm, null);
    }

    this.Load = function loader3(action, containerID, jsonData) {
        var mRetHTML = "";
        var l = arguments.length;
        var mURL = getFQDN(action);
        if (jsonData != undefined) {
            alert(jsonData);
        }

        $.ajax({
            type: "POST",
            url: mURL,
            dataType: "html",
            data: "{" + jsonData + "}",
            success: function (msg) {
                $('#' + containerID).empty();
                $('#' + containerID).html(msg.toString()).fadeIn(30);
            },
            error: function () {
                mRetHTML = 'Error getting content';
                $('#' + containerID).empty();
                $('#' + containerID).html(mRetHTML.toString()).fadeIn(30);
            }
        });
    }


    this.Refresh = function refresh() {
        for (var i = 0; i < m_RefreshObjects.length; i++) {
            m_RefreshObjects[i].Refresh();
        }
    }

    function contains(obj) {
        for (var i = 0; i < m_RefreshObjects.length; i++) {
            if (m_RefreshObjects[i] === obj) {
                return true;
            }
        }
        return false;
    }

    function getFQDN(action) {
        var mCurrentURL = window.location;
        var mPort = mCurrentURL.port;
        if (mPort == "80") {
            mPort = "";
        } else {
            mPort = ":" + mPort
        }
        return mCurrentURL.protocol + "//" + mCurrentURL.hostname + mPort + "/GetContent.aspx?Action=" + action;
    }
    // Properties
    this.Count = function count() {
        return m_RefreshObjects.length;
    }

    return true;
}