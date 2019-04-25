(function () {
    'use strict';

    var mRetCtrl = function (acctSvc, searchSvc, groupSvc, roleSvc, $scope, $route, $location) {
        // init
        var thisCrtl = this;
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        var viewModel = {};
        var m_validStatus  = [
            { "id": 1, "Name": "Active" },
            { "id": 4, "Name": "Change Password" },
            { "id": 3, "Name": "Disabled" }
        ]

        function initCtrl() {
            var lastSearchRoute = searchSvc.lastSearchRoute || "";
            if (lastSearchRoute.length == 0 && !(m_Action.toLowerCase() == 'editaccount' || m_Action.toLowerCase() == 'register')) {
                $location.path('/');
            } else {
                viewModel.validStatus = m_validStatus;
                // Request #1 in the chain
                groupSvc.getGroups(m_Action)
                    .then(function (groupsResponse) {
                        // Response Handler #1
                        viewModel.groups = groupsResponse;
                        // Request #2
                        return roleSvc.getRoles(m_Action);
                    })
                    .then(function (rolesResponse) {
                        // Response Handler #2
                        viewModel.roles = rolesResponse;
                        // Request #3
                        return acctSvc.getCurrentAccount();
                    })
                    .then(function (profile) {
                        // Response Handler #3
                        viewModel.currentProfile = profile;
                        // Request #4
                        var editId = searchSvc.editId;
                        if (m_Action.toLowerCase() == 'register') editId = -1;
                        if (m_Action.toLowerCase() == 'editaccount') editId = -2;
                        return acctSvc.getAccount(editId, m_Action);
                    }).then(function (profile) {
                        // Response Handler #4
                        viewModel.profile = profile;
                        setSelectedStatus();
                        // Request #5
                        return acctSvc.getSecurityInfo(m_Action);
                    }).then(function (securityInfo) {
                        // Response Handler #5
                        viewModel.securityInfo = securityInfo;
                        // Request #6
                        return acctSvc.getSecurityInfo('View_Account_Role_Tab');
                    }).then(function (securityInfo) {
                        // Response Handler #6
                        viewModel.securityInfoRoleTab = securityInfo;
                        // Request #7
                        return acctSvc.getSecurityInfo('View_Account_Group_Tab');
                    }).then(function (securityInfo) {
                        // Response Handler #7
                        viewModel.securityInfoGroupTab = securityInfo;
                    })
                    .catch(function (result) { /*** error ***/
                        console.log("Failed to load data for account, result is:");
                        console.log(result);
                    });
            }
            $scope.vm = viewModel;
        };

        function setSelectedStatus() {
            for (var i = 0; i < m_validStatus.length; i++) {
                if (m_validStatus[i].id == viewModel.profile.Status) {
                    viewModel.selectedStatus = m_validStatus[i];
                    break;
                };
            };
        };

        $scope.cancelEdit = function () {
            var lastSearchRoute = searchSvc.lastSearchRoute || "";
            if (lastSearchRoute.length > 0) {
                $location.path(lastSearchRoute);
            } else {
                if (!(m_Action.toLowerCase() == 'addaccount' || m_Action.toLowerCase() == 'register')) {
                    $location.path('/Home');
                } else {
                    $location.path('/Generic_Home');
                };
            }
        };

        $scope.save = function () {
            acctSvc.save(viewModel.profile, m_Action).then(
                /*** success ***/
                function (result) {
                    if (result.toLowerCase == "false") {
                        alert('Account information was not saved!');
                    } else {
                        $scope.cancelEdit();
                    }
                },
                /*** error ***/
                function (result) {
                    console.log("Failed to getPreferences, result is:");
                    console.log(result);
                }
           );
        };

        initCtrl();

        return thisCrtl;
    };

    mRetCtrl.$inject = ['AccountService', 'SearchService', 'GroupService', 'RoleService', '$scope', '$route', '$location'];

    angular.module('growthwareApp').controller('AddEditAccountController', mRetCtrl);

})();