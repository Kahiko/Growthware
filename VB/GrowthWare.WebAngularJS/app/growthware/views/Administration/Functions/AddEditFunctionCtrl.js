(function () {
    'use strict';

    var mRetCtrl = function (acctSvc, functionSvc, searchSvc, groupSvc, roleSvc, modalSvc, $route, $scope, $uibModalInstance, modalData) {
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
                .then(function (currentAccount) {
                    // Response Handler #3
                    m_ViewModel.currentAccountProfile = currentAccount;
                    var editId = searchSvc.editId;
                    // Request #4
                    return functionSvc.getFunction(editId, m_Action);
                })
                .then(function (functionProfile) {
                    // Response Handler #4
                    m_ViewModel.profile = functionProfile;
                    // setSelectedStatus(); this would set the value of the dropdown
                    // Request #5
                    return acctSvc.getSecurityInfo(m_Action);
                })
                .then(function (securityInfo) {
                    // Response Handler #5
                    m_ViewModel.securityInfo = securityInfo;
                    // Request #6
                    return acctSvc.getSecurityInfo('View_Function_Role_Tab');
                })
                .then(function (securityInfo) {
                    // Response Handler #6
                    m_ViewModel.securityInfoRoleTab = securityInfo;
                    // Request #7
                    return acctSvc.getSecurityInfo('View_Function_Group_Tab');
                })
                .then(function (securityInfo) {
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
            // console.log('calling dismiss');
            $uibModalInstance.dismiss('AddEditFunctionController cancel'); // use for popup edit
        };

        $scope.save = function () {
            $uibModalInstance.close($scope.selected.item); // return objects back to calling controller
        }

        $scope.showHelp = function (elementId, title) {
            var mModalOptions = modalSvc.options;
            mModalOptions.title = title;
            mModalOptions.content = document.getElementById(elementId).innerHTML;
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
            );;
        };

        // Objects to be used by HTML
        $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = ['AccountService', 'FunctionService', 'SearchService', 'GroupService', 'RoleService', 'ModalService', '$route', '$scope', '$uibModalInstance', 'modalData'];

    angular.module('growthwareApp').controller('AddEditFunctionController', mRetCtrl);

})();