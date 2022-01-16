(function () {
    'use strict';

    var gwApp = angular.module('growthwareApp', ['ngRoute', 'ngAnimate', 'ngSanitize', 'ngResource', 'ui.bootstrap']) // create an instance of a module

    gwApp.config(function ($provide, $routeProvider, $locationProvider) {
        GW.Common.debug("Start: growthwareApp.config");
        // register the reload event handeler
        GW.Navigation.createReloadUIEventHandler();

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

    gwApp.run(['$route', '$routeProvider', '$location', '$http', '$rootScope', '$templateCache', 'AccountService', 'MessageService', function ($route, $routeProvider, $location, $http, $rootScope, $templateCache, acctSvc, msgSvc) {
        GW.Common.debug("Start: growthwareApp.run");
        var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/GetFunctionData";
        var mNavObjects = new Array();

        $http({
            method: "GET",
            url: mApiUrl,
            headers: { 'Content-Type': 'application/json' }
        }).then(
            /*** success ***/
            function (response) {
                var list = response.data;
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
                    var routeInfo = {};
                    routeInfo["controller"] = route.Controller;
                    routeInfo["controllerAs"] = "vm";
                    routeInfo["templateUrl"] = route.Location;
                    routeInfo["title"] = route.Description;
                    routeInfo["Action"] = route.Action;
                    $routeProvider.when('/' + route.Action, routeInfo);
                });
                $routeProvider.when('/', { templateUrl: '/app/growthware/views/Home/GenericHome.html' })
                //$routeProvider.otherwise({ redirectTo: '/' }); // causes redirect when using FQDN in a popup ... should be able to put back if change from pop to page nav.
                $route.reload();
            },
            /*** error ***/
            function (response) {
                GW.Common.debug(response);
            }
        );
        $rootScope.$on('$routeChangeStart', function (event, next, current) {
            if (GW.Common.isDebug) {
                if (current) {
                    GW.Common.debug("Removing: " + current.templateUrl);
                    $templateCache.remove(current.templateUrl);
                }
            };
            if (next) {
                msgSvc.navigationHasHappened();
                var mRoute = next.$$route.originalPath.substr(1, next.$$route.originalPath.length - 1);
                if (mRoute.length > 0) {
                    acctSvc.getSecurityInfo(mRoute).then(
                        /* success */
                        function (securityInfo) {
                            if (!securityInfo.MayView) {
                                $location.path('/AccessDenied');
                            }
                        },
                        /* error */
                        function (result) {
                            console.log("Failed to get securityInfo, result is " + result);
                            $location.path('/UnknownAction');
                        });
                }
            }
        });

        $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
            var mTitle = current.$$route.title || "Welcome";
            window.document.title = mTitle;
            $('#pageMessage').html(mTitle);
        });

        GW.Common.debug("End: growthwareApp.run");
    }]);


})();