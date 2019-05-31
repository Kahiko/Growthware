(function () {
    'use strict';

    var app = angular.module('growthwareApp');
    angular.module('growthwareApp').directive('gwDerivedRoles', ['$http', 'ModalService', function ($http, modalSvc) {

        var link = function (scope, element, attrs) {
            scope.showHelpMSG = function () {
                var message = scope.picklistTableHelp || "";
                if (message.length > 0) {
                    var mModalOptions = modalSvc.options;
                    mModalOptions.title = "Help: " + scope.header;
                    mModalOptions.content = message;
                    mModalOptions.btns = [];
                    modalSvc.showModal(mModalOptions).then(
                        /*** close ***/
                        function (result) {
                            GW.Common.debug('close data: ' + result)
                        },
                        /*** dismiss ***/
                        function (reason) {
                            GW.Common.debug('Modal dismissed, reason : ', reason);
                        }
                    );
                }
            };
        };

        return {
            restrict: 'E',
            scope: {
                availableItems: '=',
                allItemsText: '@',
                header: '@',
                id: '@',
                picklistTableHelp: '@',
                rows: '@',
                size: '@'
            },
            link: link,
            templateUrl: GW.Common.getBaseURL() + '/app/growthware/directives/DerivedRoles/DerivedRolesTemplate.html'
        };

    }]);
})();