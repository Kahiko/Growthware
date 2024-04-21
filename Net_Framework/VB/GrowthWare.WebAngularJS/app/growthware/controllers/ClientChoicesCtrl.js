(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'ClientChoicesController';
    var mDependencyInjection = ['AccountService', '$scope'];
    var mRetCtrl = function (acctSvc, $scope) {
        // init
        var thisCtrlr = this;
        thisCtrlr.preferences = [];

        $scope.$on('accountChanged', function (event) {
            GW.Common.debug('Loading Client Preferences: from event');
            thisCtrlr.getPreferences(true);
        });

        thisCtrlr.getPreferences = function (force) {
            acctSvc.getPreferences(force)
            .then(function (data) {
                thisCtrlr.preferences = data;   // set the controller property
                $scope.clientChoices = data;    // or set a property in $scope
                // html defined controller directive as 'data-ng-controller="AccountController as acctCtrl"'
                // difference being that the area can simply use scope like: 
                //      { { clientChoices.Environment } }
                // or from the controller like:
                //      {{ actCtrl.preferences.Environment }}
                // in our case it doesn't matter session and principal are used on the server side to non of this
                // defined needs to be passed to get data associated with the "current account".
                //GW.Common.debug(this.preferences);
            }, function (result) {
                console.log("Failed to get getPreferences, result is " + result);
            });
        };

        thisCtrlr.getPreferences();

        return thisCtrlr;
    };

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();
