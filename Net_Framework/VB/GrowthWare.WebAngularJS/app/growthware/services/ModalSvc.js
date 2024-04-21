(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mServiceName = 'ModalService';
    var mDependencyInjection = ['$uibModal']
    function mRetSvc($uibModal) {
        var thisSvc = this;
        var m_Url = GW.Common.getBaseURL() + "/app/growthware/views/Templates/ModalPopup.html";
        var m_ModalInstance = {};
        var m_ModalOptions = {
            animation: true,
            btns: { showOk: false, showCancel: false },
            content: "content",
            controller: "ModalPopupController",
            data: {},
            size: 'sm',
            title: "title",
            url: m_Url
        };

        Object.defineProperty(this, "options", {
            get: function () {
                return JSON.parse(JSON.stringify(m_ModalOptions));
            },
            //set: function (value) {
            //    m_ModalOptions = value;
            //}
        });

        function initSvc() {
            // nothing for now ;)
        };

        thisSvc.showModal = function (modalOptions) {
            if (!modalOptions) {
                console.log('undefined argument passed "modalOptions" this is not the modal you are searching for!');
                return;
            };
            //Create method object to work with since we're in a singleton service
            var mModalOptions = $.extend({}, m_ModalOptions, modalOptions);

            m_ModalInstance = $uibModal.open({
                    animation: mModalOptions.animation,
                    templateUrl: mModalOptions.url,
                    controller: mModalOptions.controller,
                    resolve: {
                        modalData: function () { return mModalOptions; }
                    },
                    size: mModalOptions.size
                }
            );

            return m_ModalInstance.result;
        };

        initSvc();

        return thisSvc;
    }

    mRetSvc.$inject = mDependencyInjection;

    angular.module(mApplication).factory(mServiceName, mRetSvc);
})();