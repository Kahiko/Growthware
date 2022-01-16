(function () {
    'use strict';

    var app = angular.module('growthwareApp');
    app.directive('horizontalMenu', function (acctSvc, $scope, $route) {
        return {
            scope: {
                item: '=horizontalMenu'
            },
            restrict: 'EA',
            replace: true,
            template: "<a title='{{ item.Action }}' href='?Action={{ item.Route }}'>item.Action</a>;"
        };
    });

})();