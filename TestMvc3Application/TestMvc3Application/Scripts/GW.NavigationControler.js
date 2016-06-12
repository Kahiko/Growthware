// used to keep an array of refresh objects
var mRefreshObjects = new Array();

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
            mRefreshObjects.push(RefreshObject);
        }

    };

    this.Load = function load(action, containerID) {
        //alert('Not implemented!!');
        // make standard ajax call
        // to invoke controler for
        // the action given
        // make call to either get data or html
        var mRetHTML = "Welcome to ASP.NET! Loaded from javascript!!!";
        // if data only retruned then bind
        // the data to a template
        // put the results into the UI
        $('#main').html(mRetHTML.toString());
        //$('#' + containerID).html(mRetHTML.toString()).fadeIn("slow");
    }

    this.Refresh = function refresh() {
        for (var i = 0; i < mRefreshObjects.length; i++) {
            mRefreshObjects[i].Refresh();
        }
    }

    function contains(obj) {
        for (var i = 0; i < mRefreshObjects.length; i++) {
            if (mRefreshObjects[i] === obj) {
                return true;
            }
        }
        return false;
    }
    // Properties
    this.Count = function count() {
        return mRefreshObjects.length;
    }

    return true;
}