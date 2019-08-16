(function () {
    'use strict';
    var mDependencyInjection = ['YourNamedService', '$controller', '$scope'];
    var mRetCtrl = function (yourNamedSvc, $controller, $scope) {
        // Method scope variables
        var thisCtrlr = this;
        var m_ViewModel = {};  // this will be used by all methods for the view

        function initCtrl() {
            yourNamedSvc.methodWithPromise().then(function (response) {
                // work with the response
                m_ViewModel.clientMessage = response.Message;
                // make the next call in the chain
                return yourNamedSvc.secondMethodWithPromise();
            }).then(function (response) {
                // work with the response
                m_ViewModel.variable = response;
            })

            functionToBeUsedByController();
        }

        // File scope variables
        function functionToBeUsedByController() {

        }

        // Objects to be used by HTML
        $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once

        $scope.functionForView = function () {

        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module('growthwareApp').controller('YourNamedController', mRetCtrl);

})();