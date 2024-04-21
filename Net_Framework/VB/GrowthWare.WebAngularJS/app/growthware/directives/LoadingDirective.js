(function () {
    'use strict';
    var app = angular.module('growthwareApp');

    function mRetDirective($http) {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                scope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };

                scope.$watch(scope.isLoading, function (v) {
                    if (v) {
                        elm.show();
                    } else {
                        elm.hide();
                    }
                });
            }
        };
    }

    mRetDirective.$inject = ['$http'];

    app.directive('loading', mRetDirective);

})();