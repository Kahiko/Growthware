﻿(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'VerticalMenuController';
    var mDependencyInjection = ['AccountService', '$scope'];
    var mRetCtrl = function (acctSvc, $scope) {
        var thisCtrlr = this;
        $scope.$on('accountChanged', function (event) {
            GW.Common.debug('Loading VerticalMenu: from event');
            thisCtrlr.getMenuData();
        });

        thisCtrlr.getMenuData = function () {
            acctSvc.getVerticallMenuData(setMenuIems);
        };

        function setMenuIems(menuData) {
            GW.Common.debug('VerticalMenuController.setMenuIems: Start');
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
            GW.Common.debug('VerticallMenuController.setMenuIems: End');
        };

        thisCtrlr.getMenuData();

        return thisCtrlr;
    };

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);
})();