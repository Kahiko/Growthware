(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'AddDirectoryController';
    var mDependencyInjection = ['FileService', 'MessageService', '$scope', '$route', '$uibModalInstance', 'modalData'];

    var mRetCtrl = function (fileSvc, msgSvc, $scope, $route, $uibModalInstance, modalData) {
        var thisCtrlr = this;
        // file level objects {} is declarative shorthand for new Object().
        var m_Action = {}
        var m_Route = {};
        var m_ViewModel = {}; // Initialize the object, before adding data to it.
        // set intial values
        m_Route = $route.current.$$route.originalPath;
        m_Action = m_Route.substr(1, m_Route.length - 1);
        m_ViewModel.modalData = modalData.data;
        m_ViewModel.newDirectory = '';
        console.log(m_ViewModel.modalData);

        m_ViewModel.clientMessage = 'The controller is not finished';

        function initCtrl() {
            // Objects to be used by HTML
            $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
        }

        // Function called by other functions such as initCtrl
        function closeModal() {
            $uibModalInstance.close(m_ViewModel.modalData); // use for popup edit
        }

        // Functions that are avalible to the HTML
        $scope.cancelEdit = function () {
            closeModal();
        }

        $scope.doSave = function () {
            //createDirectory = function (currentDirectory, newDirectory, currentAction) {
            fileSvc.createDirectory(m_ViewModel.modalData.currentDirectory, m_ViewModel.newDirectory , m_Action).then(function (response) {
                var msg = "Folder '" + m_ViewModel.newDirectory + "'";
                // msgSvc.brodcastClientMsg(msg);
                msgSvc.brodcastClientMsg(response);
                $route.reload();
                closeModal();
            }).catch(function (response) {
                m_ViewModel.clientMessage = "Could not create directory: '" + m_ViewModel.newDirectory + "'";
            });
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();