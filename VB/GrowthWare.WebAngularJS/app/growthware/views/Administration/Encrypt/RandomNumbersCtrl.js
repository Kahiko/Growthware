(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'RandomNumbersController';
    var mDependencyInjection = ['ConfigurationService', 'MessageService', '$scope'];

    var mRetCtrl = function (configSvc, msgSvc, $scope) {
        var thisCtrlr = this;
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
        m_ViewModel.amountOfNumbers = 8;
        m_ViewModel.maxNumber = 255;
        m_ViewModel.minNumber = 0;
        m_ViewModel.results = '';

        function initCtrl() {
            // Objects to be used by HTML
            $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
        }

        // Functions that are avalible to the HTML
        $scope.doGetNumbers = function () {
            configSvc.getRandomNumbers(m_ViewModel.amountOfNumbers, m_ViewModel.maxNumber, m_ViewModel.minNumber).then(function (response) {
                m_ViewModel.results = response;
            }).catch(function (error) {
                console.log(error);
                msgSvc.brodcastClientMsg('Error generating the numbers');
            });
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();