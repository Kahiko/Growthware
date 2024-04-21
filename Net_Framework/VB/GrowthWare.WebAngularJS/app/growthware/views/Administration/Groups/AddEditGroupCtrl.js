(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'AddEditGroupController';
    var mDependencyInjection = ['AccountService', 'GroupService', 'RoleService', 'MessageService', '$uibModalInstance', '$scope', '$route', 'modalData'];

    var mRetCtrl = function (acctSvc, groupSvc, roleSvc, msgSvc, $uibModalInstance, $scope, $route, modalData) {
        var thisCtrlr = this;
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().

        m_ViewModel.modalData = modalData.data;
        m_ViewModel.profile = {};

        function initCtrl() {
            // Request #1
            groupSvc.getGroupForEdit(m_Action, m_ViewModel.modalData.editId).then(function (response) {
                // Response Handler #1
                m_ViewModel.profile = response;
                console.log(m_ViewModel.profile);
                // Request #2
                return roleSvc.getRoles();
            }).then(function (roles) {
                // Response Handler #2
                m_ViewModel.AvalibleRoles = roles;
                // Request #3
                return acctSvc.getSecurityInfo(m_Action);
            }).then(function (securityInfo) {
                // Response Handler #3
                m_ViewModel.securityInfo = securityInfo;
                // Objects to be used by HTML
                $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
            });
        }

        function closeModal() {
            $uibModalInstance.close(m_ViewModel.modalData); // use for popup edit
        }

        // Functions that are avalible to the HTML
        $scope.cancelEdit = function () {
            closeModal();
        };

        $scope.onCancel = function () {
            closeModal();
        };

        $scope.onDelete = function () {
            groupSvc.delete(m_Action, m_ViewModel.modalData.editId).then(
                /*** Success */
                function (response) {
                    if (response == true) {
                        acctSvc.clearCache();
                        msgSvc.brodcastClientMsg('The "' + m_ViewModel.profile.Name + '" group as been deleted!');
                        closeModal();
                        $route.reload();
                    } else {
                        if (response.startsWith("The account")) {
                            m_ViewModel.clientMessage = response;
                        } else {
                            m_ViewModel.clientMessage = 'Could not delete group: "' + m_ViewModel.profile.Name + '"!';
                        }
                    }
                },
                /*** Failure */
                function (response) {
                    m_ViewModel.clientMessage = 'Could not delete group: "' + m_ViewModel.profile.Name + '"!';
                    console.log(response);
                });
            $route.reload();
            closeModal();
        }

        $scope.onSave = function () {
            groupSvc.save(m_Action, m_ViewModel.profile).then(
                /*** Success */
                function (response) {
                    if (response == true) {
                        acctSvc.clearCache();
                        msgSvc.brodcastClientMsg('The "' + m_ViewModel.profile.Name + '" group as been saved');
                        closeModal();
                        $route.reload();
                    } else {
                        m_ViewModel.clientMessage = 'Could not save group: "' + m_ViewModel.profile.Name + '"!';
                    }
                },
                /*** Failure */
                function (response) {
                    m_ViewModel.clientMessage = 'Could not save group: "' + m_ViewModel.profile.Name + '"!';
                    console.log(response);
                });
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();