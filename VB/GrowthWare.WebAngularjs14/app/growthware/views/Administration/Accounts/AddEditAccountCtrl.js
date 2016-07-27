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
            if (lastSearchRoute.length == 0) {
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
                        return acctSvc.getAccount(searchSvc.editId, m_Action);
                    })
                    .then(function (profile) {
                        // Response Handler #3
                        viewModel.profile = profile;
                        setSelectedStatus();
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
                $location.path('/Home');
            }
        };

        $scope.save = function () {
            console.log($scope.vm.profile);
        };

        initCtrl();

        return thisCrtl;
    };

    mRetCtrl.$inject = ['AccountService', 'SearchService', 'GroupService', 'RoleService', '$scope', '$route', '$location'];

    angular.module('growthwareApp').controller('AddEditAccountController', mRetCtrl);

})();