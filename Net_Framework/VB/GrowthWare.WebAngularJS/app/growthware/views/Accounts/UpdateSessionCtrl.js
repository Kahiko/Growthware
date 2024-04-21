(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'UpdateSessionController';
    var mDependencyInjection = ['AccountService', 'MessageService', '$scope', '$rootScope', '$location'];

    var mRetCtrl = function (acctSvc, msgSvc, $scope, $rootScope, $location) {
        var thisCtrlr = this;
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
        m_ViewModel.clientMessage = '';

        function initCtrl() {
            acctSvc.updateAccountSession(false).then(function (response) {
                console.log(response);
                var mMsg = 'Your security and session information has been updated.';
                // work with the response
                if (response == true) {
                    $rootScope.$broadcast('accountChanged', []);
                    m_ViewModel.clientMessage = mMsg;
                    msgSvc.brodcastClientMsg(mMsg);
                    $location.path('/Home');
                } else {
                    m_ViewModel.clientMessage = 'Error updating your security and session information.';
                }

                // Objects to be used by HTML
                $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
            });
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();