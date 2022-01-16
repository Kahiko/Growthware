(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'AddEditRoleController';
    var mDependencyInjection = ['AccountService', 'RoleService', 'MessageService', '$uibModalInstance', '$scope', '$route', 'modalData'];

    var mRetCtrl = function (acctSvc, roleSvc, msgSvc, $uibModalInstance, $scope, $route, modalData) {
        var thisCtrlr = this;
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
        m_ViewModel.isSystemDisabled = false;
        m_ViewModel.modalData = modalData.data;
        m_ViewModel.profile = {};
        m_ViewModel.mayDelete = false;

        function initCtrl() {
            roleSvc.getRoleForEdit(m_ViewModel.modalData.editId).then(function (response) {
                // work with the response
                m_ViewModel.profile = response;
                m_ViewModel.mayDelete = m_ViewModel.profile
                if (m_ViewModel.profile.IsSystem || m_ViewModel.profile.IsSystemOnly) {
                    m_ViewModel.isSystemDisabled = true;
                }
                return acctSvc.getSecurityInfo(m_Action);
            }).then(function (response) {
                m_ViewModel.securityInfo = response;
                if (m_ViewModel.modalData.editId == -1 && m_ViewModel.securityInfo.MayDelete) {
                    // can't delete when adding
                    m_ViewModel.clientMessage = 'Delete has been disabed when adding a new role.';
                    m_ViewModel.securityInfo.MayDelete = false;
                };

                if (m_ViewModel.profile.IsSystem || m_ViewModel.profile.IsSystemOnly) {
                    m_ViewModel.securityInfo.MayDelete = false;
                };

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
            roleSvc.delete(m_ViewModel.modalData.editId, m_Action).then(
                /*** Success ***/
                function (result) {
                    if (result == true) {
                        acctSvc.clearCache();
                        msgSvc.brodcastClientMsg('The "' + m_ViewModel.profile.Name + '" role has been delete');
                        closeModal();
                        $route.reload();
                    } else {
                        m_ViewModel.clientMessage = 'Could not delete role: "' + m_ViewModel.profile.Name + '"!';
                        console.log(result);
                    }
                },
                /*** Failure ***/
                function (result) {
                    m_ViewModel.clientMessage = 'Could not delete role: "' + m_ViewModel.profile.Name + '"!';
                    console.log(result);
                }
            );
        };

        $scope.onSave = function () {
            roleSvc.save(m_ViewModel.profile, m_Action).then(
                /*** success ***/
                function (result) {
                    if (result == true) {
                        acctSvc.clearCache();
                        msgSvc.brodcastClientMsg('The "' + m_ViewModel.profile.Name + '" role has been saved');
                        closeModal();
                        $route.reload();
                    } else {
                        m_ViewModel.clientMessage = 'Could not save role: "' + m_ViewModel.profile.Name + '"!';
                    }
                },
                /*** error ***/
                function (result) {
                    m_ViewModel.clientMessage = 'Could not save role: "' + m_ViewModel.profile.Name + '"!';
                    console.log(result);
                }
            );
        }

        // Uncomment the next line if you have not set this when calling services in initCtrl
        // $scope.vm = viewModel;

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();