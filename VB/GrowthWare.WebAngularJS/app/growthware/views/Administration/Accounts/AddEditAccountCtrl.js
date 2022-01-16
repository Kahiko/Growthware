(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'AddEditAccountController';
    var mDependencyInjection = ['AccountService', 'GroupService', 'MessageService', 'RoleService', 'ModalService', '$scope', '$route', '$location', '$uibModalInstance', 'modalData'];
    var mRetCtrl = function (acctSvc, groupSvc, msgSvc, roleSvc, modalSvc, $scope, $route, $location, $uibModalInstance, modalData) {
        // init
        var thisCrtl = this;
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        var m_ViewModel = {};
        m_ViewModel.litAccountWarning = '';
        m_ViewModel.litFirstNameWarning = '';
        m_ViewModel.litLastNameWarning = '';
        m_ViewModel.litStatusWarning = '';
        m_ViewModel.litEMailWarning = '';
        var m_validStatus  = [
            { "id": 1, "Name": "Active" },
            { "id": 4, "Name": "Change Password" },
            { "id": 3, "Name": "Disabled" }
        ]

        m_ViewModel.modalData = modalData.data;

        function initCtrl() {
            m_ViewModel.clientMessage = '';
            if ((m_Action.toLowerCase() == 'editaccount' || m_Action.toLowerCase() == 'register')) {
                $location.path('/');
            } else {
                m_ViewModel.validStatus = m_validStatus;
                // Request #1 in the chain
                groupSvc.getGroups(m_Action)
                    .then(function (groupsResponse) {
                        // Response Handler #1
                        m_ViewModel.groups = groupsResponse;
                        // Request #2
                        return roleSvc.getRoles(m_Action);
                    })
                    .then(function (rolesResponse) {
                        // Response Handler #2
                        m_ViewModel.roles = rolesResponse;
                        // Request #3
                        return acctSvc.getCurrentAccount();
                    })
                    .then(function (profile) {
                        // Response Handler #3
                        m_ViewModel.currentProfile = profile;
                        // console.log(JSON.stringify(profile));
                        var editId = m_ViewModel.modalData.editId;
                        if (m_Action.toLowerCase() == 'register') editId = -1;
                        if (m_Action.toLowerCase() == 'editaccount') editId = -2;
                        // Request #4
                        return acctSvc.getAccount(editId, m_Action);
                    }).then(function (profile) {
                        // Response Handler #4
                        m_ViewModel.profile = profile;
                        console.log(m_ViewModel.profile);
                        setSelectedStatus();
                        // Request #5
                        return acctSvc.getSecurityInfo(m_Action);
                    }).then(function (securityInfo) {
                        // Response Handler #5
                        m_ViewModel.securityInfo = securityInfo;
                        // TODO: the following does not work as expected, if you view your
                        // account the delete button is hidden (and presumed disabled)
                        // if you then look at another account (not your own) the button
                        // is still missing????
                        //if (m_ViewModel.profile.Account == m_ViewModel.currentProfile.Account) {
                        //    m_ViewModel.securityInfo.MayDelete = false;
                        //}
                        // Request #6
                        return acctSvc.getSecurityInfo('View_Account_Role_Tab');
                    }).then(function (securityInfo) {
                        // Response Handler #6
                        m_ViewModel.securityInfoRoleTab = securityInfo;
                        // Request #7
                        return acctSvc.getSecurityInfo('View_Account_Group_Tab');
                    }).then(function (securityInfo) {
                        // Response Handler #7
                        m_ViewModel.securityInfoGroupTab = securityInfo;
                        if (m_ViewModel.modalData.editId == -1) {
                            m_ViewModel.selectedStatus = { "id": 4, "Name": "Change Password" };
                        };
                        $scope.vm = m_ViewModel;
                    })
                    .catch(function (result) { /*** error ***/
                        m_ViewModel.clientMessage = 'Failed to load data';
                        console.log("Failed to load data for account, result is:");
                        console.log(result);
                        $scope.vm = m_ViewModel;
                    });
            }
        };

        function setSelectedStatus() {
            for (var i = 0; i < m_validStatus.length; i++) {
                if (m_validStatus[i].id == m_ViewModel.profile.Status) {
                    m_ViewModel.selectedStatus = m_validStatus[i];
                    break;
                };
            };
        };

        $scope.cancelEdit = function () {
            if ((m_Action.toLowerCase() == 'addaccount' || m_Action.toLowerCase() == 'register')) {
                $route.reload();
            } else {
                $uibModalInstance.close(m_ViewModel.modalData); // use for popup edit
            };
        };

        $scope.doDelete = function () {
            var mModalOptions = modalSvc.options;
            var message = '';
            if (m_ViewModel.profile.Account != m_ViewModel.currentProfile.Account) {
                message = 'Are you sure you would like to delete account "' + m_ViewModel.profile.Account + '"?';
                mModalOptions.title = "Warring: Delete?";
                mModalOptions.content = message;
                mModalOptions.btns = { showOk: true, showCancel: true };
                modalSvc.showModal(mModalOptions).then(
                    /*** close ***/
                    function (result) {
                        acctSvc.delete(m_ViewModel.profile.Id).then(function (response) {
                            msgSvc.brodcastClientMsg('Account "' + m_ViewModel.profile.Account + '" has been deleted');
                            $scope.cancelEdit();
                            $route.reload();
                        }).catch(function (error) {
                            console.log(error);
                        });
                    },
                    /*** dismiss ***/
                    function (reason) {
                        message = 'Did not delete account "' + m_ViewModel.profile.Account + '"?';
                        GW.Common.debug(message);
                        
                    }
                );
            } else {
                message = 'Not allowed to delete the same account as the one your using.';
                mModalOptions.title = "Error: Unable to delete";
                mModalOptions.content = message;
                mModalOptions.btns = [];
                modalSvc.showModal(mModalOptions).then(
                    /*** close ***/
                    function (result) {
                        GW.Common.debug('close data: ' + result)
                    },
                    /*** dismiss ***/
                    function (reason) {
                        GW.Common.debug('Modal dismissed, reason : ', reason);
                    }
                );
            }
        };

        $scope.save = function () {
            var mIsVald = true;
            if (GW.Common.isNullOrEmpty(m_ViewModel.selectedStatus)) {
                mIsVald = false;
                m_ViewModel.litStatusWarning = '*';
            }
            if (GW.Common.isNullOrEmpty(m_ViewModel.profile.Account)) {
                m_ViewModel.litAccountWarning = '*';
                mIsVald = false;
            }
            if (GW.Common.isNullOrEmpty(m_ViewModel.profile.FirstName)) {
                m_ViewModel.litFirstNameWarning = '*';
                mIsVald = false;
            }
            if (GW.Common.isNullOrEmpty(m_ViewModel.profile.FirstName)) {
                m_ViewModel.litLastNameWarning = '*';
                mIsVald = false;
            }
            if (!mIsVald) {
                m_ViewModel.clientMessage = 'Please check required fields!';
                return;
            }
            m_ViewModel.profile.Status = m_ViewModel.selectedStatus.id;
            console.log(m_ViewModel.profile);
            acctSvc.save(m_ViewModel.profile, m_Action).then(
                /*** success ***/
                function (result) {
                    if (result.toLowerCase() == "true") {
                        msgSvc.brodcastClientMsg('Account information has been saved');
                        $scope.cancelEdit();
                        $route.reload();
                    } else {
                        m_ViewModel.clientMessage = 'Account information was not saved!';
                    }
                },
                /*** error ***/
                function (result) {
                    m_ViewModel.clientMessage = 'Account information was not saved!';
                    console.log(result);
                }
           );
        };

        initCtrl();

        return thisCrtl;
    };

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();