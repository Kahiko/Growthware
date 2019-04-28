(function () {
    'use strict';

    var mRetCtrl = function (yourNamedSvc, $controller, $scope) {
        // File scope variables
        var thisCtrlr = this;
        var viewModel = {};  // this will be used by all methods

        function initCtrl() {
            yourNamedSvc.methodWithPromise().then(function (response) {
                // work with the response
                viewModel.clientMessage = response.Message;
                // make the next call in the chain
                return yourNamedSvc.secondMethodWithPromise();
            }).then(function (response) {
                // work with the response
                viewModel.variable = response;


            })

            functionToBeUsedByController();
        }

        // File scope variables
        function functionToBeUsedByController() {

        }

        // Objects to be used by HTML
        $scope.vm = viewModel; // Place all of the data elements on to scope at once

        $scope.changePassword = function () {

        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = ['YourNamedService', '$controller', '$scope'];

    angular.module('growthwareApp').controller('YourNamedController', mRetCtrl);

})();