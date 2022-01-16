(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'AddEditDBInformationController';
    var mDependencyInjection = ['ConfigurationService', 'MessageService', '$scope'];

    var mRetCtrl = function (configSvc, msgSvc, $scope) {
        var thisCtrlr = this;
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
        m_ViewModel.profile = {};
        m_ViewModel.selectedEnableInheritance = '';

        function initCtrl() {
            configSvc.getDBInformation().then(function (response) {
                // work with the response
                m_ViewModel.profile = response;
                m_ViewModel.selectedEnableInheritance = JSON.stringify(m_ViewModel.profile.enableInheritance);
                console.log(m_ViewModel.selectedEnableInheritance);
                // Objects to be used by HTML
                $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
            });
        }

        // Functions called by other functions such as initCtrl
        function populateFromPage() {
            m_ViewModel.profile.enableInheritance = m_ViewModel.selectedEnableInheritance * 1;
        }

        // Functions that are avalible to the HTML
        $scope.doSave = function () {
            populateFromPage();
            configSvc.saveDBInformation(m_ViewModel.profile.enableInheritance).then(function (response) {
                msgSvc.brodcastClientMsg('Database information has been saved.');
            }).catch(function (error) {
                msgSvc.brodcastClientMsg('Error no information was saved.');
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