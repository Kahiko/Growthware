(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'SetLogLevelController';
    var mDependencyInjection = ['ConfigurationService', 'MessageService', '$route'];

    var mRetCtrl = function (configSvc, msgSvc, $route) {
        var thisCtrlr = this;
        // file level objects {} is declarative shorthand for new Object().
        var m_Action = {};
        var m_Route = {};
        // set intial values
        m_Route = $route.current.$$route.originalPath;
        m_Action = m_Route.substr(1, m_Route.length - 1);

        function initCtrl() {
            configSvc.getLogLevel().then(function (response) {
                // work with the response
                thisCtrlr.selectedLogLevel = JSON.stringify(response);
                // make the next call in the chain
            });
        }

        thisCtrlr.doSetLogLevel = function () {
            var selectedLogLevel = thisCtrlr.selectedLogLevel * 1;
            configSvc.setLogLevel(selectedLogLevel).then(function (response) {
                msgSvc.brodcastClientMsg(response);
            }).catch(function () {
                msgSvc.brodcastClientMsg('An error occured attempting to set the log level!');
            });
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();