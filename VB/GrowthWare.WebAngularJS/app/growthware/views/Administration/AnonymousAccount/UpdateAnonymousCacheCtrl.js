(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'UpdateAnonymousCacheController';
    var mDependencyInjection = ['AccountService', 'MessageService', '$scope'];

    var mRetCtrl = function (acctSvc, msgSvc, $scope) {
        var thisCtrlr = this;
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().

        function initCtrl() {
            acctSvc.updateUpdateAnonymousCache().then(function () {
                // Objects to be used by HTML
                $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
            }).catch(function (error) {
                msgSvc.brodcastClientMsg('Error updating the Anonymous cache');
            });
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();