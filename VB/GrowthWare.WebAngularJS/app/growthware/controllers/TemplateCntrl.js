(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'YourNamedController';
    var mDependencyInjection = ['yourNamedSvc', 'MessageService', '$scope', '$route', '$uibModalInstance', 'modalData'];

    var mRetCtrl = function (yourNamedSvc, msgSvc, $scope, $route, $uibModalInstance, modalData) {
        var thisCtrlr = this;
        // file level objects {} is declarative shorthand for new Object().
        var m_Action = {}l
        var m_Route = {};
        var m_ViewModel = {}; // Initialize the object, before adding data to it.
        // set intial values
        m_Route = $route.current.$$route.originalPath;
        m_Action = m_Route.substr(1, m_Route.length - 1);
        m_ViewModel.modalData = modalData.data;

        msgSvc.brodcastClientMsg('the controller is not finished');

        function initCtrl() {
            yourNamedSvc.methodWithPromise().then(function (response) {
                // work with the response
                msgSvc.brodcastClientMsg(response.Message);
                // make the next call in the chain
                return yourNamedSvc.secondMethodWithPromise();
            }).then(function (response) {
                // work with the response
                m_ViewModel.variable = response;

                // Objects to be used by HTML
                $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
            });
        }

        // Function called by other functions such as initCtrl
        function closeModal() {
            $uibModalInstance.close(m_ViewModel.modalData); // use for popup edit
        }

        // Functions that are avalible to the HTML
        $scope.doCancel = function () {
            closeModal()
        }

        $scope.doDelete = function () {
            closeModal()
        }

        $scope.doSave = function () {
            closeModal()
        }

        // Uncomment the next line if you have not set this when calling services in initCtrl
        // $scope.vm = viewModel;

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();