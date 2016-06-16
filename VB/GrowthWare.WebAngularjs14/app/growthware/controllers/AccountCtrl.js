(function () {
    'use strict';

    var accountCtrl = function (acctSvc, $scope, $route) {
        // init
        var thisCtrl = this;
        thisCtrl.navigationObjects = new Array();

        thisCtrl.getRoutes = function () {
            return $route.routes;
        };

        return thisCtrl;
    };

    accountCtrl.$inject = ['AccountService', 'SearchService', '$scope'];

    angular.module('growthwareApp').controller('AccountController', accountCtrl);

})();
