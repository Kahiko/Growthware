(function () {
    'use strict';

    function mRetSvc($uibModal) {
        var thisSvc = this;
        var m_Url = GW.Common.getBaseURL() + "/app/growthware/views/Templates/ModalPopup.html";
        var m_ModalInstance = {};
        var m_ModalOptions = {
            animation: true,
            btns: [],
            content: "content",
            controller: "ModalPopupController",
            size: 'sm',
            title: "title",
            url: m_Url
        };
        var m_BtnOkay = {
            id: 'btnOK',
            label: 'OKay',
            cssClass: 'btn btn-primary',
            icon: 'glyphicon glyphicon-thumbs-up',
            onClick: function (returnData) {
                m_ModalInstance.close(returnData);
            }
        };

        Object.defineProperty(this, "options", {
            get: function () {
                return m_ModalOptions;
            },
            set: function (value) {
                m_ModalOptions = value;
            }
        });

        function initSvc() {
            m_ModalOptions.btns.push(m_BtnOkay);
        };

        thisSvc.showModal = function (modalOptions) {
            //Create method object to work with since we're in a singleton service
            var mModalOptions = $.extend({}, m_ModalOptions, modalOptions);

            m_ModalInstance = $uibModal.open({
                    animation: mModalOptions.animation,
                    templateUrl: mModalOptions.url,
                    controller: mModalOptions.controller,
                    resolve: {
                        popupData: function () { return mModalOptions; }
                    },
                    size: mModalOptions.size
                }
            );

            return m_ModalInstance.result;
        };

        initSvc();

        return thisSvc;
    }

    mRetSvc.$inject = ['$uibModal'];

    angular.module('growthwareApp').factory('ModalService', mRetSvc);
})();