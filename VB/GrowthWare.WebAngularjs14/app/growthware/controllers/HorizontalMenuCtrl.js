(function () {
    'use strict';

    var mRetCtrl = function (acctSvc, $scope) {
        // init
        var thisCtrlr = this;

        $scope.$on('accountChanged', function (event) {
            GW.Common.debug('Loading HorizontalMenu: from event');
            thisCtrlr.getHorizontalMenuData();
        });

        thisCtrlr.getHorizontalMenuData = function () {
            acctSvc.getHorizontalMenuData(setMenuIems);
        };

        function setMenuIems(menuData) {
            GW.Common.debug('HorizontalMenuController.setMenuIems: Start');
            var menuItems = [];
            for (i = 0; i < menuData.length; i++) {
                var item = menuData[i];
                if (item) {
                    var menuItem = { Route: item["Action"], Action: item["Action"], Description: item['Description'], Title: item["item"]['Title'] };
                    menuItems.push(menuItem);
                }
            };
            thisCtrlr.menuItems = menuItems;  // Accounts for using a named contoler ng-controller="HorizontalMenuController as myCntrlr"
            $scope.menuItems = menuItems;
            GW.Common.debug('HorizontalMenuController.setMenuIems: End');
        };

        thisCtrlr.getHorizontalMenuData();

        return thisCtrlr;
    };

    mRetCtrl.$inject = ['AccountService', '$scope'];

    angular.module('growthwareApp').controller('HorizontalMenuController', mRetCtrl);
})();
