(function () {
    'use strict';

    var mRetCtrl = function (acctSvc, searchSvc, groupSvc, roleSvc, $scope, $route, $location, $uibModalInstance, modalData) {
        // init
        var thisCrtl = this;
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        var m_ViewModel = {};
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
                        var editId = m_ViewModel.modalData.editId;
                        if (m_Action.toLowerCase() == 'register') editId = -1;
                        if (m_Action.toLowerCase() == 'editaccount') editId = -2;
                        // Request #4
                        return acctSvc.getAccount(editId, m_Action);
                    }).then(function (profile) {
                        // Response Handler #4
                        m_ViewModel.profile = profile;
                        setSelectedStatus();
                        // Request #5
                        return acctSvc.getSecurityInfo(m_Action);
                    }).then(function (securityInfo) {
                        // Response Handler #5
                        m_ViewModel.securityInfo = securityInfo;
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
                    })
                    .catch(function (result) { /*** error ***/
                        m_ViewModel.clientMessage = 'Failed to load data';
                        console.log("Failed to load data for account, result is:");
                        console.log(result);
                    });
            }
            $scope.vm = m_ViewModel;
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

        $scope.save = function () {
            m_ViewModel.profile.Status = m_ViewModel.selectedStatus.id;
            acctSvc.save(m_ViewModel.profile, m_Action).then(
                /*** success ***/
                function (result) {
                    if (result.toLowerCase == "false") {
                        m_ViewModel.clientMessage = 'Account information was not saved!';
                    } else {
                        m_ViewModel.clientMessage = 'Account information has been saved';
                        $scope.cancelEdit();
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

    mRetCtrl.$inject = ['AccountService', 'SearchService', 'GroupService', 'RoleService', '$scope', '$route', '$location', '$uibModalInstance', 'modalData'];

    angular.module('growthwareApp').controller('AddEditAccountController', mRetCtrl);

})();