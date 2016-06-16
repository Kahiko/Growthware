(function () {
    'use strict';

    angular.module('growthwareApp').controller('LogoffController', ['AccountService', '$controller', '$scope', '$location', function (acctSvc, $controller, $scope, $location) {
        // init
        var thisCtrlr = this;

        thisCtrlr.logoff = function () {
            acctSvc.logoff(logoffCallBack);
        };

        function logoffCallBack(response) {
            $location.path('/Generic_Home');
            //location.reload();
        };

        thisCtrlr.logoff();

        return thisCtrlr;
    }]);
})();
