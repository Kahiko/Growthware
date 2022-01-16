(function () {
    'use strict';

    var app = angular.module('growthwareApp');

/** When included in in BundleConfig.vb this causes the following error:
 
 TypeError: $interpolate is not a function
    mRetDirective http://www.webangularjs14_vb_.com/app/growthware/directives/ClientChoicesStyle.js:8
    Angular 47
    jQuery 11
 <style client-choices-style="" class="ng-binding ng-scope"> angular.js:15567:41
    Angular 48
    jQuery 11
 
 This should be either be developed or deleted (guessing deleted b/c we have the clientchoices in styles already working)
 */

    function mRetDirective($interpolate) {
        return function (scope, elem) {
            var exp = $interpolate(elem.html()),
                watchFunc = function () { return exp(scope); };

            scope.$watch(watchFunc, function (html) {
                elem.html(html);
            });
        };
    }

    mRetDirective.$inject = ['ModalService'];

    app.directive('clientChoicesStyle', mRetDirective);
})();