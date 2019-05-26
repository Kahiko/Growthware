(function () {
    'use strict';


    var mRetCtrl = function ($scope, $uibModalInstance, popupData, $sanitize) {
        // File scope variables
        var mThisCtrlr = this;
        var m_ViewModel = popupData;

        function initCtrl() {
            m_ViewModel.content = $sanitize(m_ViewModel.content);
        };

        // Objects to be used by HTML
        $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once

        $scope.cancel = function () {
            $uibModalInstance.dismiss('dismissed by upperRightClose button');
        };

        initCtrl();

        return mThisCtrlr;
    }

    mRetCtrl.$inject = ['$scope', '$uibModalInstance', 'popupData', '$sanitize'];

    angular.module('growthwareApp').controller('ModalPopupController', mRetCtrl);

})();