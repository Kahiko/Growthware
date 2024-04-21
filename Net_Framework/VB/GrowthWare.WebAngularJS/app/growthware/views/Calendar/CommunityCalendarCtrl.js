(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'CommunityCalendarController';
    var mDependencyInjection = ['MessageService', '$scope'];

    var mRetCtrl = function (msgSvc, $scope) {
        var thisCtrlr = this;
        var viewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().

        msgSvc.brodcastClientMsg('the controller is not finished');

        function initCtrl() {
            //yourNamedSvc.methodWithPromise().then(function (response) {
            //    // work with the response
            //    m_ViewModel.clientMessage = response.Message;
            //    // make the next call in the chain
            //    return yourNamedSvc.secondMethodWithPromise();
            //}).then(function (response) {
            //    // work with the response
            //    m_ViewModel.variable = response;

            //    // Objects to be used by HTML
            //    thisCtrlr.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
            //});

            functionToBeUsedByController();
        }

        // Function called by other functions such as initCtrl
        function functionToBeUsedByController() {

        }

        // Functions that are avalible to the HTML
        $scope.ScopeFunction = function () {
            // does nothing
        }

        // Uncomment the next line if you have not set this when calling services in initCtrl
        $scope.vm = viewModel;

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();