(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'AddEditWorkFlowController';
    var mDependencyInjection = ['FunctionService', 'MessageService', '$scope'];

    var mRetCtrl = function (functionSvc, msgSvc, $scope) {
        var thisCtrlr = this;
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().

        msgSvc.brodcastClientMsg('the controller is not finished');

        function initCtrl() {
            //yourNamedSvc.methodWithPromise().then(function (response) {
            //    // work with the response
            //    msgSvc.brodcastClientMsg(response.Message);
            //    // make the next call in the chain
            //    return yourNamedSvc.secondMethodWithPromise();
            //}).then(function (response) {
            //    // work with the response
            //    m_ViewModel.variable = response;

            //    // Objects to be used by HTML
            //    $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
            //});
            // Objects to be used by HTML
            $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
        }

        // Function called by other functions such as initCtrl
        function nonScopFunction() {
            // do nothing atm
        }

        // Functions that are avalible to the HTML
        $scope.doCancel = function () {
            nonScopFunction()
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();