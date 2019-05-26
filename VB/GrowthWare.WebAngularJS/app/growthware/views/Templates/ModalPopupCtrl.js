(function () {
    'use strict';

    var mRetCtrl = function ($scope, $uibModalInstance, popupData) {
        // File scope variables
        var mThisCtrlr = this;
        var m_ViewModel = popupData;

        function initCtrl() {
            //m_ViewModel.content = $sce.trustAsHtml(m_ViewModel.content);

            var html = "<p> Hello world! </p>";
            //m_ViewModel.content = $sce.trustAsHtml(html);
        };

        // Objects to be used by HTML
        $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once

        $scope.cancel = function () {
            $uibModalInstance.dismiss('dismissed by upperRightClose button');
        };

        initCtrl();

        return mThisCtrlr;
    }

    mRetCtrl.$inject = ['$scope', '$uibModalInstance', 'popupData'];

    angular.module('growthwareApp').controller('ModalPopupController', mRetCtrl);

})();