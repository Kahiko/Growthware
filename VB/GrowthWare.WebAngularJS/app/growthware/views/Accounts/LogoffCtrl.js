(function () {
    'use strict';

    var mRetCtrl = function (acctSvc, $controller, $scope, $location) {
        // init
        var thisCtrlr = this;

        thisCtrlr.logoff = function () {
            acctSvc.logoff(logoffCallBack);
        };

        function logoffCallBack(response) {
            $location.path('/Generic_Home');
            GW.Navigation.NavigationController.Reload();
        };

        thisCtrlr.logoff();

        return thisCtrlr;
    };

    mRetCtrl.$inject = ['AccountService', '$controller', '$scope', '$location'];

    angular.module('growthwareApp').controller('LogoffController', mRetCtrl);
})();
