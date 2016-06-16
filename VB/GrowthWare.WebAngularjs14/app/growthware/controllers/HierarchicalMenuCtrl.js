(function () {
    'use strict';

    var horizontalHierarchicalMenuCntrl = function (acctSvc, $scope) {
        // init
        var thisCtrlr = this;

        $scope.$on('accountChanged', function (event) {
            GW.Common.debug('Loading HorizontalHierarchicalMenu: from event');
            thisCtrlr.getMenuData();
        });

        thisCtrlr.getMenuData = function () {
            acctSvc.getHorizontalHierarchicalMenuData(setMenuItems);
        };

        function setMenuItems(menuData) {
            GW.Common.debug('buildHorizontalHierarchicalMenu: start');
            thisCtrlr.menuItems = menuData;  // Accounts for using a named contoler ng-controller="HorizontalMenuController as myCntrlr"
            $scope.menuItems = menuData;
            GW.Common.debug('buildHorizontalHierarchicalMenu: end');
        };

        thisCtrlr.getMenuData();

        return thisCtrlr;
    };

    horizontalHierarchicalMenuCntrl.$inject = ['AccountService', '$scope'];

    angular.module('growthwareApp').controller('HHMenuController', horizontalHierarchicalMenuCntrl);

})();
