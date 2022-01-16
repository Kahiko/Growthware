(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'AccountController';
    var mDependencyInjection = ['AccountService', 'SearchService', '$scope'];

    var mRetCtrl = function (acctSvc, $scope, $route) {
        // init
        var thisCtrl = this;
        thisCtrl.navigationObjects = new Array();

        thisCtrl.getRoutes = function () {
            return $route.routes;
        };

        return thisCtrl;
    };

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();
