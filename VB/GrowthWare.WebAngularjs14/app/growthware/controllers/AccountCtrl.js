(function () {
    'use strict';

    var mRetCtrl = function (acctSvc, $scope, $route) {
        // init
        var thisCtrl = this;
        thisCtrl.navigationObjects = new Array();

        thisCtrl.getRoutes = function () {
            return $route.routes;
        };

        return thisCtrl;
    };

    mRetCtrl.$inject = ['AccountService', 'SearchService', '$scope'];

    angular.module('growthwareApp').controller('AccountController', mRetCtrl);

})();
