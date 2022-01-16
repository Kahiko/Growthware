(function () {
    'use strict';

    var mApplication = 'growthwareApp';
    var mDirectiveNameName = 'gwClientMessage'; // <gw-client-message></gw-client-message>
    var mDependencyInjection = [];

    function mRetDirective() {
        var link = function (scope, element, attrs) {
            $(document).bind('~UIClientMsgEvent', function (event, data) {
                scope.clientMessage = data.message;
            });
        };

        return {
            restrict: 'E',
            scope: {},
            link: link,
            templateUrl: GW.Common.getBaseURL() + '/app/growthware/directives/ClientMessage/ClientMessageTemplate.html'
        };
    }

    mRetDirective.$inject = mDependencyInjection;

    angular.module(mApplication).directive(mDirectiveNameName, mRetDirective);
})();