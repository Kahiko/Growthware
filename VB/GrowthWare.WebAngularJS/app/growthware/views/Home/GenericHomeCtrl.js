(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'GenericHomeController';
    var mDependencyInjection = ['ConfigurationService'];
    var mRetCtrl = function (configurationSvc) {
        // Method scope variables
        var thisCtrlr = this;
        // File scope variables

        initCtrl = function () {
            configurationSvc.getConfigInfo().then(function (response) {
                // work with the response
                thisCtrlr.configInfo = response;
            });
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();