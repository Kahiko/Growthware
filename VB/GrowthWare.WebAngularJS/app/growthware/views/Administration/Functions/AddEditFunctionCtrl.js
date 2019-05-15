(function () {
    'use strict';

    var mRetCtrl = function (acctSvc, functionSvc, searchSvc, groupSvc, roleSvc, $route, $controller, $scope) {
        // File scope variables
        var thisCtrlr = this;
        var m_ViewModel = {};  // this will be used by all methods
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);

        function initCtrl() {
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
                    m_ViewModel.currentAccountProfile = profile;
                    var editId = searchSvc.editId;
                    if (m_Action.toLowerCase() == 'register') editId = -1; // not needed but going to look into what is 
                    // Request #4
                    return functionSvc.getFunction(editId, m_Action);
                }).then(function (profile) {
                    // Response Handler #4
                    m_ViewModel.profile = profile;
                    console.debug(m_ViewModel.profile);
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
                    console.log("Failed to load data for function, result is:");
                    console.log(result);
                });


            functionToBeUsedByController();
        }

        // File scope variables
        function functionToBeUsedByController() {

        }

        // functions used by HTML
        $scope.changePassword = function () {

        }


        $scope.cancelEdit = function () {
            var lastSearchRoute = searchSvc.lastSearchRoute || "";
            if (lastSearchRoute.length > 0) {
                var objectToSend = {};
                $uibModalInstance.close(objectToSend); // use for popup edit
            } else {
                if (!(m_Action.toLowerCase() == 'addaccount' || m_Action.toLowerCase() == 'register')) {
                    $route.reload();
                } else {
                    $location.path('/Generic_Home');
                };
            }
        };

        // Objects to be used by HTML
        $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = ['AccountService', 'FunctionService', 'SearchService', 'GroupService', 'RoleService', '$route' , '$controller', '$scope'];

    angular.module('growthwareApp').controller('AddEditFunctionController', mRetCtrl);

})();