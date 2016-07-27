(function () {
    'use strict';

    function mRetSvc($http, $q, $rootScope) {
        var thisSvc = this;
        var m_ClientChoices = null;
        var m_SecurityInfo = [];

        thisSvc.loadFunctions = function (callBackFunc) {
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/GetFunctionData";
            var mRetVal = new Array();
            $http({
                method: "GET",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' }
            }).success(function (response) {
                GW.Common.debug('loadFunctions: success');
                var list = response;
                for (i = 0; i < list.length; i++) {
                    var mNewNavigationObject = new GW.Model.NavigationObject();
                    mNewNavigationObject.Action = list[i].Action;
                    mNewNavigationObject.Location = list[i].Location;
                    mNewNavigationObject.Description = list[i].Description;
                    mNewNavigationObject.LinkBehavior = list[i].LinkBehavior;
                    mRetVal.push(mNewNavigationObject);
                }
                //GW.Common.debug(mRetVal);
                if (typeof (callBackFunc) == 'function') {
                    callBackFunc(mRetVal);
                }
            }).error(function (response) {
                GW.Common.debug(response);
                callBackFunc(response);
            });
            return mRetVal;
        };

        thisSvc.logon = function (logonInfo, callBackFunc) {
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Accounts/Logon?Action=Logon";
            var mRetVal = '';
            var options = {
                method: "POST",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' },
                data: logonInfo
            }
            $http(options).success(function (response) {
                GW.Common.debug('logon: success');
                m_SecurityInfo = [];
                $rootScope.$broadcast('accountChanged', []);
                if (typeof (callBackFunc) == 'function') {
                    callBackFunc(response);
                }
            }).error(function (response) {
                GW.Common.debug(response);
                callBackFunc(response);
            });
            return mRetVal;
        };

        thisSvc.logoff = function (callBackFunc) {
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Accounts/Logoff?Action=Logoff";
            var mRetVal = '';
            $http({
                method: "GET",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' }
            }).success(function (response) {
                GW.Common.debug('logoff: success');
                m_SecurityInfo = [];
                $rootScope.$broadcast('accountChanged',[]);
                mRetVal = response;
                if (typeof (callBackFunc) == 'function') {
                    callBackFunc(mRetVal);
                }
            }).error(function (response) {
                GW.Common.debug(response);
                callBackFunc(response);
            });
            return mRetVal;
        };

        thisSvc.getAccount = function (accountId, action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Accounts/GetProfile/?Action=" + action + "&accountSeqID=" + accountId;
            var options = {
                method: "GET",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' }
            }
            $http(options)
            .success(function (response) {
                deferred.resolve(response);
            }).error(function (response) {
                GW.Common.debug(response);
                deferred.reject(response);
            });
            return deferred.promise;
        };

        thisSvc.getHorizontalHierarchicalMenuData = function (callBackFunc) {
            var menuType = 3;  //MenuType.Hierarchical = 3
            getMenuData(callBackFunc, menuType);
        };

        thisSvc.getHorizontalMenuData = function (callBackFunc) {
            var menuType = 1;  //MenuType.Horizontal = 1
            getMenuData(callBackFunc, menuType);
        };

        thisSvc.getVerticallMenuData = function (callBackFunc) {
            var menuType = 2;  //MenuType.Vertical = 2
            getMenuData(callBackFunc, menuType);
        };

        thisSvc.getPreferences = function (force) {
            force = force || false;
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Accounts/GetPreferences";
            if (m_ClientChoices !== null && m_ClientChoices.AccountName.length == 0) force = true;
            if (m_ClientChoices !== null && force !== true) {
                //GW.Common.debug('m_ClientChoices from cache.')
                deferred.resolve(m_ClientChoices);
            } else {
                m_ClientChoices = GW.Model.ClientChoices();
                //GW.Common.debug('m_ClientChoices from server.')
                $http({
                    method: "GET",
                    url: mApiUrl,
                    dataType: 'json',
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (response) {
                    m_ClientChoices = response;
                    deferred.resolve(m_ClientChoices);
                }).error(function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                });
            }
            return deferred.promise;
        };

        thisSvc.getSecurityInfo = function (action) {
            var deferred = $q.defer();
            var securityDataIndex = -1;
            for (var i = 0; i < m_SecurityInfo.length; i++) {
                if (m_SecurityInfo[i]['action'] == action) {
                    securityDataIndex = i;
                    break;
                }
            }
            if (!m_SecurityInfo) securityDataIndex = -1;
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Accounts/GetSecurityInfo?action=" + action;
            if (securityDataIndex != -1) {
                deferred.resolve(m_SecurityInfo[securityDataIndex]['securityInfo']);
            } else {
                $http({
                    method: "POST",
                    url: mApiUrl,
                    dataType: 'json',
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (response) {
                    m_SecurityInfo.push({ "action": action, "securityInfo": response });
                    deferred.resolve(response);
                }).error(function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                });
            }
            return deferred.promise;
        };

        thisSvc.save = function (profile, action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Accounts/Save/?Action=" + action;
            var options = {
                method: "POST",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(profile)
            }
            $http(options)
            .success(function (response) {
                deferred.resolve(response);
            }).error(function (response) {
                GW.Common.debug(response);
                deferred.reject(response);
            });
            return deferred.promise;
        };

        function getMenuData(callBackFunc, menuType) {
            var mApiUrl = "/gw/api/Accounts/GetMenuData?menuType=" + menuType;
            var mRetVal = new Array();
            $http({
                method: "POST",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' }
            }).success(function (response) {
                //GW.Common.debug('getMenuData: success: Start');
                mRetVal = GW.Navigation.buildData(response);
                if (typeof (callBackFunc) == 'function') {
                    callBackFunc(mRetVal);
                }
                //GW.Common.debug('getMenuData: success: End');
            }).error(function (response) {
                GW.Common.debug(response);
                callBackFunc(response);
            });
            return mRetVal;
        };

        return thisSvc;

    };

    mRetSvc.$inject = ['$http', '$q', '$rootScope'];
    
    angular.module('growthwareApp').factory('AccountService', mRetSvc);

})();