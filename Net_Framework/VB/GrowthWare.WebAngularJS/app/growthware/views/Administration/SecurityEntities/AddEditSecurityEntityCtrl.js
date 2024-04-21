(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'AddEditSecurityEntityController';
    var mDependencyInjection = ['SecurityEntityService', 'AccountService', 'ConfigurationService', 'MessageService', '$uibModalInstance', '$scope', '$route', 'modalData'];

    var mRetCtrl = function (securityEntitySvc, acctSvc, configSvc, msgSvc, $uibModalInstance, $scope, $route, modalData) {
        var thisCtrlr = this;
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
        m_ViewModel.avalibleStatuses = [
            { text: 'Active', value: 1 },
            { text: 'Inactive', value: 2 }
        ];
        m_ViewModel.avalibleEncryptTypes = [
            { text: 'Triple DES', value: 1 },
            { text: 'DES', value: 2 },
            { text: 'None', value: 3 }
        ];
        m_ViewModel.avalibleParents = [];
        m_ViewModel.avalibleSkins = [];
        m_ViewModel.avalibleStyles = [];
        m_ViewModel.configInfo = {};

        m_ViewModel.clientMessage = 'Add has not been tested';
        m_ViewModel.configInfo = {};
        m_ViewModel.dalTypes = [
            { text: 'SQL Server', value: 'SQLServer' },
            { text: 'MySql', value: 'MySql' },
            { text: 'Oracle', value: 'Oracle' }
        ];
        m_ViewModel.securityInfo = {};
        m_ViewModel.selectedDAL = {};
        m_ViewModel.selectedEncryptType = {};
        m_ViewModel.selectedParent = {};
        m_ViewModel.selectedSkin = {};
        m_ViewModel.selectedStatus = {};
        m_ViewModel.selectedStyle = {};
        m_ViewModel.modalData = modalData.data;
        m_ViewModel.profile = {};

        function initCtrl() {
            // first call in the chain
            acctSvc.getSecurityInfo(m_Action).then(function (securityInfo) {
                // work with the response
                m_ViewModel.securityInfo = securityInfo;
                // call the next in chain
                return securityEntitySvc.getAvalibleParrents(m_ViewModel.modalData.editId, m_Action);
            })
            .then(function (response) {
                // work with the response
                m_ViewModel.avalibleParents = response;
                // call the next in chain
                return securityEntitySvc.getAvalibleStyles(m_Action);
            }).then(function (response) {
                m_ViewModel.avalibleStyles = response;
                // call the next in chain
                return securityEntitySvc.getAvalibleSkins(m_Action);
            }).then(function (response) {
                m_ViewModel.avalibleSkins = response;
                // call the next in the chain
                return securityEntitySvc.GetSecurityEntityForEdit(m_ViewModel.modalData.editId, m_Action);
            }).then(function (response) {
                // work with the response
                m_ViewModel.profile = response;
                m_ViewModel.selectedDAL = m_ViewModel.profile.DAL;
                m_ViewModel.selectedParent = JSON.stringify(m_ViewModel.profile.ParentSeqId);
                m_ViewModel.selectedSkin = m_ViewModel.profile.Skin;
                m_ViewModel.selectedStatus = JSON.stringify(m_ViewModel.profile.StatusSeqId);
                m_ViewModel.selectedStyle = m_ViewModel.profile.Style;
                m_ViewModel.selectedEncryptType = JSON.stringify(m_ViewModel.profile.EncryptionType);
                // console.log(JSON.stringify(m_ViewModel.profile));
                return configSvc.getConfigInfo();
            }).then(function (response) {
                m_ViewModel.configInfo = response;
                // Objects to be used by HTML
                $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
            }).catch(function (result) { /*** error ***/
                m_ViewModel.clientMessage = 'Failed to load data';
                console.log("Failed to load data for account, result is:");
                console.log(result);
                $scope.vm = m_ViewModel;
            });
        }

        function populateProfile() {
            m_ViewModel.profile.EncryptionType = m_ViewModel.selectedEncryptType * 1;
            m_ViewModel.profile.DAL = m_ViewModel.selectedDAL
            m_ViewModel.profile.ParentSeqId = m_ViewModel.selectedParent * 1;
            m_ViewModel.profile.Skin = m_ViewModel.selectedSkin;
            m_ViewModel.profile.StatusSeqId = m_ViewModel.selectedStatus * 1
            m_ViewModel.profile.Style = m_ViewModel.selectedStyle;
        };

        // Functions that are avalible to the HTML
        $scope.cancelEdit = function () {
            $uibModalInstance.close(m_ViewModel.modalData); // use for popup edit
        };

        $scope.doSave = function () {
            populateProfile();
            securityEntitySvc.save(m_ViewModel.profile, m_Action).then(function (response) {
                if (response.toLowerCase() == "true") {
                    msgSvc.brodcastClientMsg('Successfully saved');
                    $scope.cancelEdit();
                    $route.reload();
                } else {
                    m_ViewModel.clientMessage = 'Information was not saved!';
                }
            }).catch(function (response) {
                m_ViewModel.clientMessage = 'Information was not saved!';
                console.log(response);
            });
        }

        // Uncomment the next line if you have not set this when calling services in initCtrl
        // $scope.vm = viewModel;

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();