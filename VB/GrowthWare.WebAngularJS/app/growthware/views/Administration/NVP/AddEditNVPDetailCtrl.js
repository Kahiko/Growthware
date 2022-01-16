(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'AddEditNVPDetailController';
    var mDependencyInjection = ['MessageService', '$uibModalInstance', '$scope', '$route', 'modalData'];

    var mRetCtrl = function (msgSvc, $uibModalInstance, $scope, $route, modalData) {
        var thisCtrlr = this;
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().

        m_ViewModel.clientMessage = 'the controller is not finished';
        m_ViewModel.modalData = modalData.data;

        function initCtrl() {
            // Objects to be used by HTML
            $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
        }

        // Functions that are avalible to the HTML
        $scope.cancelEdit = function () {
            $uibModalInstance.close(m_ViewModel.modalData); // use for popup edit
        };

        $scope.save = function () {
            $route.reload();
            $scope.cancelEdit();
        }

        // Uncomment the next line if you have not set this when calling services in initCtrl
        // $scope.vm = viewModel;

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();