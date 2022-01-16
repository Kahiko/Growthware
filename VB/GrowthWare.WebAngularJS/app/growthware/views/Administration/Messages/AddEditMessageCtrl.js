(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'AddEditMessageController';
    var mDependencyInjection = ['AccountService', 'MessageService', '$scope', '$route', '$uibModalInstance', 'modalData'];
    var mRetCtrl = function (acctSvc, msgSvc, $scope, $route, $uibModalInstance, modalData) {
        // init
        var thisCrtl = this;
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        var m_ViewModel = {};
        m_ViewModel.profile = {};

        m_ViewModel.modalData = modalData.data;

        function initCtrl() {
            // Request #1 in the chain
            acctSvc.getSecurityInfo(m_Action)
                .then(function (securityInfo) {
                    // Response Handler #1
                    m_ViewModel.securityInfo = securityInfo;
                    var editId = m_ViewModel.modalData.editId;
                    console.log(editId);
                    return msgSvc.getProfileForEdit(editId);
                }).then(function (response) {
                    m_ViewModel.profile = response;
                })
                .catch(function (result) { /*** error ***/
                    m_ViewModel.clientMessage = 'Failed to load data';
                    console.log("Failed to load data for account, result is:");
                    console.log(result);
                });
            $scope.vm = m_ViewModel;
        };

        $scope.cancelEdit = function () {
            $uibModalInstance.close(m_ViewModel.modalData); // use for popup edit
        };

        $scope.save = function () {
            msgSvc.save(m_ViewModel.profile, m_Action).then(
                /*** success ***/
                function (result) {
                    if (result.toLowerCase() == "true") {
                        msgSvc.brodcastClientMsg('Message information has been saved');
                        $scope.cancelEdit();
                        $route.reload();
                    } else {
                        m_ViewModel.clientMessage = 'Message information was not saved!';
                    }
                },
                /*** error ***/
                function (result) {
                    msgSvc.brodcastClientMsg('Error: Message information was not saved!');
                    console.log(result);
                }
            );
            $scope.cancelEdit();
        };

        initCtrl();

        return thisCrtl;
    };

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();