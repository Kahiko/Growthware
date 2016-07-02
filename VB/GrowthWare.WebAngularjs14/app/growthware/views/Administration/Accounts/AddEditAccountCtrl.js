(function () {
    'use strict';

    var addEditAccountCtrl = function (acctSvc, searchSvc, $scope, $route, $location) {
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
                console.log(searchSvc.editId);
                $scope.validStatus = m_validStatus;
                acctSvc.getAccount(searchSvc.editId, m_Action).then(
                    /*** success ***/
                    function (profile) {
                        viewModel.profile = profile;
                        console.log(m_validStatus[profile.Status]);
                        viewModel.selectedStatus = m_validStatus[profile.Status];
                    },
                    /*** error ***/
                    function (result) {
                        console.log("Failed to getAccount, result is " + result);
                    }
                );
            }
            $scope.vm = viewModel;
        };

        $scope.cancelEdit = function () {
            var lastSearchRoute = searchSvc.lastSearchRoute || "";
            if (lastSearchRoute.length > 0) {
                $location.path(lastSearchRoute);
            } else {
                $location.path('/Home');
            }
        };

        initCtrl();

        return thisCrtl;
    };

    addEditAccountCtrl.$inject = ['AccountService', 'SearchService', '$scope', '$route', '$location'];

    angular.module('growthwareApp').controller('AddEditAccountController', addEditAccountCtrl);

})();