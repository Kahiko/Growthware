(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'GUIDHelperController';
    var mDependencyInjection = ['ConfigurationService', 'MessageService', '$scope', '$timeout'];

    var mRetCtrl = function (configSvc, msgSvc, $scope, $timeout) {
        var thisCtrlr = this;
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().

        function initCtrl() {
            // Objects to be used by HTML
            $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
        }

        // Functions that are avalible to the HTML
        $scope.doGetGUID = function () {
            configSvc.getGUID().then(function (response) {
                m_ViewModel.processedText = response;
            }).catch(function (error) {
                console.log(error);
            });
        }

        $scope.doHighlightAll = function (theField) {
            var mTempval = $('#' + theField);
            mTempval.focus()
            mTempval.select()
            try {
                if (document.all) {
                    therange = mTempval.createTextRange();
                    therange.execCommand("Copy");
                    msgSvc.brodcastClientMsg("Contents highlighted and copied to clipboard!");
                    $timeout(function () {
                        msgSvc.brodcastClientMsg('');
                    }, 3000);
                }
            } catch (e) {
                // do nothing
            }
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();