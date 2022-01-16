(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'AddEditFunctionController';
    var mDependencyInjection = ['AccountService', 'FunctionService', 'GroupService', 'RoleService', 'MessageService', 'ModalService', '$route', '$scope', '$uibModalInstance', 'modalData']

    var mRetCtrl = function (acctSvc, functionSvc, groupSvc, roleSvc, msgSvc, modalSvc, $route, $scope, $uibModalInstance, modalData) {
        // File scope variables
        var thisCtrlr = this;
        var m_ViewModel = {};  // this will be used by all methods
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        m_ViewModel.avalibleParents = [];
        m_ViewModel.clientMessage = 'Have not tested Add';
        m_ViewModel.functionTypes = [];
        m_ViewModel.ImpersonatePassword = '';
        m_ViewModel.linkBehaviors = [];
        m_ViewModel.menuOrder = [];
        m_ViewModel.modalData = modalData.data;
        m_ViewModel.navigationTypes = [];
        m_ViewModel.profile = {};
        m_ViewModel.selectedParent = {}
        m_ViewModel.selectedFunctionType = {};
        m_ViewModel.selectedLinkBehavior = {};
        m_ViewModel.selectedNavigationType = {};
        m_ViewModel.selectedDropFunctions = {};
        m_ViewModel.sortedMenuOrder = [];

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
                    return functionSvc.getFunctionTypes();
                }).then(function (functionTypes) {
                    // Response Handler #3
                    functionTypes.forEach(function (element) {
                        // We don't want Calendar or Security
                        if (element["FUNCTION_TYPE_SEQ_ID"] != 4 && element["FUNCTION_TYPE_SEQ_ID"] != 2) {
                            m_ViewModel.functionTypes.push(element);
                        }
                    });
                    // Request #4
                    return functionSvc.getNavigationTypes();
                })
                .then(function (navigationTypes) {
                    // Response Handler #4
                    m_ViewModel.navigationTypes = navigationTypes;
                    // Request #5
                    return functionSvc.getLinkBehaviors();
                })
                .then(function (linkBehaviors) {
                    // Response Handler #5
                    m_ViewModel.linkBehaviors = linkBehaviors;
                    // Request #6
                    return acctSvc.getCurrentAccount();
                })
                .then(function (currentAccount) {
                    // Response Handler #7
                    m_ViewModel.currentAccountProfile = currentAccount;
                    var editId = m_ViewModel.modalData.editId;
                    // Request #8
                    return functionSvc.getFunction(editId, m_Action);
                })
                .then(function (functionProfile) {
                    // Response Handler #8
                    m_ViewModel.profile = functionProfile;
                    console.log(m_ViewModel.profile);
                    // The select doesn't bind well with integer 
                    // so selectedFunctionType was added to the view model and used in the
                    // data-ng-model of the dropFunctionType
                    m_ViewModel.selectedDropFunctions = JSON.stringify(m_ViewModel.profile.Id);
                    m_ViewModel.selectedFunctionType = JSON.stringify(m_ViewModel.profile.FunctionTypeSeqID);
                    m_ViewModel.selectedLinkBehavior = JSON.stringify(m_ViewModel.profile.LinkBehavior);
                    m_ViewModel.selectedNavigationType = JSON.stringify(m_ViewModel.profile.NavigationTypeSeqId);
                    m_ViewModel.selectedParent = JSON.stringify(m_ViewModel.profile.ParentID);
                    // Request #9
                    return functionSvc.getFunctionMenuOrder(m_ViewModel.profile.Id);
                })
                .then(function (functionMenuOrder) {
                    // Response Handler #9
                    m_ViewModel.menuOrder = functionMenuOrder;
                    m_ViewModel.sortedMenuOrder = functionMenuOrder.sort(function (a, b) {
                        return a.Name > b.Name ? 1 : a.Name < b.Name ? -1 : 0
                    })
                    // Request #10
                    return functionSvc.getAvalibleParents(m_ViewModel.profile.Id);
                })
                .then(function (avalibleParents) {
                    // Response Handler #10
                    m_ViewModel.avalibleParents = []; // ensure there is a viable array
                    if (avalibleParents) {
                        avalibleParents.forEach(function (element) {
                            if (element.FunctionTypeSeqID == GW.Enum.FunctionType.Menu_Item) {
                                var item = { Id: element.Id, Name: element.Name };
                                m_ViewModel.avalibleParents.push(item);
                            }
                        });
                    }
                    // Request #11
                    return acctSvc.getSecurityInfo(m_Action);
                })
                .then(function (securityInfo) {
                    // Response Handler #11
                    m_ViewModel.securityInfo = securityInfo;
                    // Request #12
                    return acctSvc.getSecurityInfo('View_Function_Role_Tab');
                })
                .then(function (securityInfo) {
                    // Response Handler #12
                    m_ViewModel.securityInfoRoleTab = securityInfo;
                    // Request #13
                    return acctSvc.getSecurityInfo('View_Function_Group_Tab');
                })
                .then(function (securityInfo) {
                    // Response Handler #13
                    m_ViewModel.securityInfoGroupTab = securityInfo;
                    // Objects to be used by HTML
                    $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once
                })
                .catch(function (result) { /*** error ***/
                    m_ViewModel.clientMessage = 'Failed to load data';
                    console.log("Failed to load data for function, result is:");
                    console.log(result);
                });
        }
        /**
         * Updates m_ViewModel.profile with values stored on other objects.  Call this before sending data
         * to the API.  Mostly this pertains to drop down values.
         */
        function populateFromPage() {
            m_ViewModel.profile.FunctionTypeSeqID = m_ViewModel.selectedFunctionType * 1;
            m_ViewModel.profile.Id = m_ViewModel.selectedDropFunctions * 1;
            m_ViewModel.profile.LinkBehavior = m_ViewModel.selectedLinkBehavior * 1;
            m_ViewModel.profile.NavigationTypeSeqId = m_ViewModel.selectedNavigationType * 1;
            m_ViewModel.profile.ParentID = m_ViewModel.selectedParent * 1;
        }

        // functions used by HTML
        $scope.cancelEdit = function () {
            $uibModalInstance.dismiss('AddEditFunctionController cancel'); // use for popup edit
        };

        $scope.delete = function () {
            var mModalOptions = modalSvc.options;
            var message = 'Are you sure you would like to delete function "' + m_ViewModel.profile.Name + '"?';
            mModalOptions.title = "Warring: Delete?";
            mModalOptions.content = message;
            mModalOptions.btns = { showOk: true, showCancel: true };
            modalSvc.showModal(mModalOptions).then(
                /*** close ***/
                function (result) {
                    functionSvc.delete(m_ViewModel.profile.Id).then(function (response) {
                        msgSvc.brodcastClientMsg('Function "' + m_ViewModel.profile.Name + '" has been deleted');
                        $scope.cancelEdit();
                        $route.reload();
                    }).catch(function (error) {
                        console.log(error);
                    });
                },
                /*** dismiss ***/
                function (reason) {
                    message = 'Did not delete function "' + m_ViewModel.profile.Name + '"?';
                    GW.Common.debug(message);

                }
            );
        }

        $scope.move = function(direction) {
            functionSvc.move(m_ViewModel.profile.Id, direction, m_Action)
                .then(function() {
                    // Request #2
                    return functionSvc.getFunctionMenuOrder(m_ViewModel.profile.Id);
                })
                .then(function(functionMenuOrder) {
                    // Response Handler #2
                    m_ViewModel.menuOrder = functionMenuOrder;
                })
                .catch(function (result) { /*** error ***/
                    var mMsg = 'Failed to move the item';
                    m_ViewModel.clientMessage = mMsg;
                    console.log(mMsg + ", result is:");
                    console.log(result);
                });
        }

        $scope.save = function () {
            populateFromPage();
            // console.log(m_ViewModel.profile);
            functionSvc.save(m_ViewModel.profile, m_Action).then(
                /*** success ***/
                function (response) {
                    if (response.toLowerCase() == "true") {
                        msgSvc.brodcastClientMsg('Function information has been saved');
                        $scope.cancelEdit();
                        $route.reload();
                    } else {
                        m_ViewModel.clientMessage = 'Function information was not saved!';
                    }
                },
                /*** failure ***/
                function (respnose) {
                    m_ViewModel.clientMessage = 'Could not save "' + m_ViewModel.profile.Action + '"!';
                    console.log(respnose);
                }
            );
        }

        $scope.showHelp = function (elementId, title) {
            var mModalOptions = modalSvc.options;
            mModalOptions.title = title;
            mModalOptions.content = document.getElementById(elementId).innerHTML;

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

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();