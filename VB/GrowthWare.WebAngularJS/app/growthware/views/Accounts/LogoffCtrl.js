(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'LogoffController';
    var mDependencyInjection = ['AccountService', '$controller', '$scope', '$location'];

    var mRetCtrl = function (acctSvc, $controller, $scope, $location) {
        // init
        var thisCtrlr = this;

        thisCtrlr.logoff = function () {
            acctSvc.logoff(logoffCallBack);
        };

        function logoffCallBack(response) {
            $location.path('/Generic_Home');
        };

        thisCtrlr.logoff();

        return thisCtrlr;
    };

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);
})();
