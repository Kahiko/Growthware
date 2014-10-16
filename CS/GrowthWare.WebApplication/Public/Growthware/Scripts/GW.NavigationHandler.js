$(document).ready(function () {
    $('#HorizontalHierarchicalDiv').delegate("a", "click", function (e) {
        e.stopImmediatePropagation();
        e.preventDefault();
        var mHREF = $(this).attr("href");
        window.location.hash = "?Action=" + mHREF;
        return false;
    });

    $('#HorizontalDiv').delegate("a", "click", function (e) {
        e.stopImmediatePropagation();
        e.preventDefault();
        var mHREF = $(this).attr("href");
        window.location.hash = "?Action=" + mHREF;
        return false;
    });

    $('#VMenuDiv').delegate("a", "click", function (e) {
        e.stopImmediatePropagation();
        e.preventDefault();
        var mHREF = $(this).attr("href");
        window.location.hash = "?Action=" + mHREF;
        return false;
    });

    $('#VHMenuDiv').delegate("a", "click", function (e) {
        e.stopImmediatePropagation();
        e.preventDefault();
        var mHREF = $(this).attr("href");
        window.location.hash = "?Action=" + mHREF;
        return false;
    });

    $('#VMenu').delegate("a", "click", function (e) {
        e.stopImmediatePropagation();
        e.preventDefault();
        var mHREF = $(this).attr("href");
        window.location.hash = "?Action=" + mHREF;
        return false;
    });

    $(window).bind('hashchange', function () {
        var newHash = window.location.hash.substring(1);
        newHash = newHash.replace("?Action=", "");
        if (newHash != "#" && newHash.length > 0) {
            var mNavigationObject = GW.Navigation.NavigationController.GetNavigationObject(newHash);
            document.title = mNavigationObject.Description;
            GW.Navigation.NavigationController.LoadPage(newHash, 'MainContentDiv', this);
        }
    });

    $('window').trigger('hashchange');

});