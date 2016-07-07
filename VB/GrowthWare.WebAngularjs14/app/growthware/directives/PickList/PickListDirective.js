(function () {
    'use strict';

    var app = angular.module('growthwareApp');
    angular.module('growthwareApp').directive('gwPicklist', ['$http', function ($http) {
        return {
            restrict: 'E',
            templateUrl: GW.Common.getBaseURL() + '/app/growthware/directives/PickList/PickListTemplate.html'
        };
    }]);
})();