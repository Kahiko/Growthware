(function () {
    'use strict';

    var app = angular.module('growthwareApp');
    angular.module('growthwareApp').directive('gwDerivedRoles', ['$http', function ($http) {

        var link = function (scope, element, attrs) {
            scope.showHelpMSG = function () {
                var message = scope.picklistTableHelp || "";
                if (message.length > 0) {
                    alert(message);
                }
            };
        };

        return {
            restrict: 'E',
            scope: {
                avalibleItems: '=',
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