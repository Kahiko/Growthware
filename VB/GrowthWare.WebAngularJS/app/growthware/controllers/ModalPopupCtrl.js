(function () {
    'use strict';

    var mRetCtrl = function ($scope, popupData) {
        // File scope variables
        var thisCtrlr = this;
        var m_ViewModel = {};  // this will be used by all methods

        function initCtrl() {
            console.log($scope.modalInstance);
        }

        $scope.ok = function () {
            $scope.modalInstance.close('ModalPopupController close');
        };

        $scope.cancel = function () {
            $scope.modalInstance.dismiss('ModalPopupController cancel');
        };

        // Objects to be used by HTML
        $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = ['$scope', 'popupData'];

    angular.module('growthwareApp').controller('ModalPopupController', mRetCtrl);

})();