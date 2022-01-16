(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'CopyFunctionSecurityController';
    var mDependencyInjection = ['SecurityEntityService', 'MessageService', '$scope'];

    var mRetCtrl = function (securityEntitySvc, msgSvc, $scope) {
        var thisCtrlr = this;
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
        m_ViewModel.securityEntities = [];
        m_ViewModel.selectedSource = "0";
        m_ViewModel.selectedTarget = "0";

        msgSvc.brodcastClientMsg('Need to re-create storedproceedure then finished');

        function initCtrl() {
            securityEntitySvc.getValidSecurityEntities().then(function (response) {
                // work with the response
                // System is special b/c it is the root for the inheritence
                // so we do not want it to appear in the drop downs
                const index = response.findIndex(prop => prop.SE_SEQ_ID === '1')
                response.splice(index, 1)
                m_ViewModel.securityEntities = response;
                console.log(m_ViewModel.securityEntities);
                // Objects to be used by HTML
                $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
            });
        }

        // Functions that are avalible to the HTML
        $scope.go = function () {
            // does nothing
            var mMsg = '';
            var selectedSource = m_ViewModel.selectedSource * 1;
            var selectedTarget = m_ViewModel.selectedTarget * 1;
            if (selectedSource === 0 || selectedTarget === 0) {
                mMsg = 'Selection can not be "Please choose one"';
            };

            if (selectedSource === selectedTarget) {
                if (mMsg.length > 0) {
                    mMsg += ' and Source and Target can not be the same'
                } else {
                    mMsg = 'Source and Target can not be the same'
                }
            };
            if (mMsg.length > 0) {
                msgSvc.brodcastClientMsg(mMsg);
                return;
            }
            msgSvc.brodcastClientMsg('Function security has been copied.');
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();