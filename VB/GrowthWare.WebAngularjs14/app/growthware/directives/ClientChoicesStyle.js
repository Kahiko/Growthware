(function () {
    'use strict';

    angular.module('growthwareApp').directive('clientChoicesStyle', function ($interpolate) {
        return function (scope, elem) {
            var exp = $interpolate(elem.html()),
                watchFunc = function () { return exp(scope); };

            scope.$watch(watchFunc, function (html) {
                elem.html(html);
            });
        };
    });
});