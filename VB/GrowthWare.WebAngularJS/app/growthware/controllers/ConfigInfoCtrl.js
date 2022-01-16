(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'ConfigInfoController';
    var mDependencyInjection = ['ConfigurationService', '$scope'];
    var mRetCtrl = function (configurationSvc, $scope) {
        // init
        var thisCtrlr = this;
        var m_ViewModel = {};
        thisCtrlr.preferences = [];

        thisCtrlr.initCtrl = function () {
            configurationSvc.getConfigInfo()
                .then(function (data) {
                    m_ViewModel = data;
                    thisCtrlr.vm = m_ViewModel;
                }, function (result) {
                    console.log("Failed to get getPreferences, result is " + result);
                });
        };

        thisCtrlr.initCtrl();

        return thisCtrlr;
    };

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();
