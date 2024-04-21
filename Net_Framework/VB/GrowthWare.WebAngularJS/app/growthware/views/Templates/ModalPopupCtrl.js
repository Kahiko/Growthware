(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'ModalPopupController';
    var mDependencyInjection = ['$uibModalInstance', 'modalData', '$sanitize'];
    var mRetCtrl = function ($uibModalInstance, modalData, $sanitize) {
        // File scope variables
        var thisCtlr = this;
        var m_ViewModel = modalData;

        function initCtrl() {
            thisCtlr.content = $sanitize(thisCtlr.content);
        };

        thisCtlr.ok = function () {
            $uibModalInstance.close(m_ViewModel);
        };

        thisCtlr.cancel = function () {
            $uibModalInstance.dismiss('dismissed by upperRightClose button');
        };

        initCtrl();

        return thisCtlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();