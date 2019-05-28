(function () {
    'use strict';


    var mRetCtrl = function ($scope, $uibModalInstance, modalData, $sanitize) {
        // File scope variables
        var m_ThisCtrlr = this;
        var m_ViewModel = modalData;

        function initCtrl() {
            m_ViewModel.content = $sanitize(m_ViewModel.content);
        };

        // Objects to be used by HTML
        $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once

        $scope.cancel = function () {
            $uibModalInstance.dismiss('dismissed by upperRightClose button');
        };

        initCtrl();

        return m_ThisCtrlr;
    }

    mRetCtrl.$inject = ['$scope', '$uibModalInstance', 'modalData', '$sanitize'];

    angular.module('growthwareApp').controller('ModalPopupController', mRetCtrl);

})();