(function () {
    'use strict';

    var gwApp = angular.module('growthwareApp', ['ngRoute', 'ngAnimate', 'ngResource', 'ui.bootstrap']) // create an instance of a module

    gwApp.config(function ($provide, $routeProvider, $locationProvider) {
            GW.Common.debug("Start: growthwareApp.config");
            /*** Expose the $routeProvider to .run ****/
            $provide.factory('$routeProvider', function () {
                return $routeProvider;
            });
            $locationProvider.html5Mode({
                enabled: true
                //, requireBase: false
            });
            GW.Common.debug("End: growthwareApp.config");
    })

    gwApp.run(['$route', '$routeProvider', '$location', '$http', '$rootScope', '$templateCache', 'AccountService', function ($route, $routeProvider, $location, $http, $rootScope, $templateCache, acctSvc) {
        GW.Common.debug("Start: growthwareApp.run");
        var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/GetFunctionData";
        var mNavObjects = new Array();

        $http({
            method: "GET",
            url: mApiUrl,
            headers: { 'Content-Type': 'application/json' }
        }).success(function (response) {
            var list = response;
            for (i = 0; i < list.length; i++) {
                var mPath = list[i].Location;
                if (mPath.indexOf("Functions/System/") >= 0) {
                    mPath = mPath.replace("Functions/System/", "app/growthware/views/");
                    mPath = mPath.replace(".aspx", ".html");
                }
                //mPath = GW.Common.getBaseURL() + "/" +mPath;
                var mNewNavObject = new GW.Model.NavigationObject();
                mNewNavObject.Action = list[i].Action;
                mNewNavObject.Controller = list[i].Controller;
                mNewNavObject.Description = list[i].Description;
                mNewNavObject.LinkBehavior = list[i].LinkBehavior;
                mNewNavObject.Location = mPath;
                mNavObjects.push(mNewNavObject);
            }
            //GW.Common.debug(mNavObjects);
            /*** register the routes ***/
            mNavObjects.forEach(function (route) {
                $routeProvider.when('/' + route.Action, { controller: route.Controller, templateUrl: route.Location + '?Action=' + route.Action, title: route.Description });
            });
            $routeProvider.when('/', { templateUrl: '/app/growthware/views/Home/GenericHome.html' })
            //$routeProvider.otherwise({ redirectTo: '/' }); // causes redirect when using FQDN in a popup ... should be able to put back if change from pop to page nav.
            $route.reload();
        }).error(function (response) {
            GW.Common.debug(response);
        });
        $rootScope.$on('$routeChangeStart', function (event, next, current) {
            if (GW.Common.isDebug) {
                if (current) {
                    GW.Common.debug("Removing: " + current.templateUrl);
                    $templateCache.remove(current.templateUrl);
                }
            };
        });

        $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
            window.document.title = current.$$route.title;
        });

        GW.Common.debug("End: growthwareApp.run");
    }]);

})();