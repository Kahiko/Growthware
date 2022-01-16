(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mServiceName = 'SecurityEntityService';
    var mDependencyInjection = ['$http', '$q', '$rootScope'];
    var mRetSvc = function ($http, $q, $rootScope) {
        var thisSvc = this;

        thisSvc.getAvalibleSkins = function (action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/SecurityEntities/GetAvalibleSkins?Action=" + action;
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    var mRetVal = JSON.parse(JSON.stringify(response.data));
                    deferred.resolve(mRetVal);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        };

        thisSvc.getAvalibleStyles = function (action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/SecurityEntities/GetAvalibleStyles?Action=" + action;
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    var mRetVal = JSON.parse(JSON.stringify(response.data));
                    deferred.resolve(mRetVal);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        };

        thisSvc.getAvalibleParrents = function (id, action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/SecurityEntities/GetAvalibleParrents?Action=" + action + "&id=" + id;
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    var mRetVal = JSON.parse(JSON.stringify(response.data));
                    deferred.resolve(mRetVal);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        };

        thisSvc.getSecurityEntity = function (id, action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/SecurityEntities/GetSecurityEntity?Action=" + action + "&id=" + id;
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    deferred.resolve(response.data);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        }

        thisSvc.GetSecurityEntityForEdit = function (id, action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/SecurityEntities/GetSecurityEntityForEdit?Action=" + action + "&id=" + id;
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    deferred.resolve(response.data);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        }

        thisSvc.getValidSecurityEntities = function () {
            var deferred = $q.defer();
            var mDataUrl = GW.Common.getBaseURL() + '/gw/api/SecurityEntities/GetValidSecurityEntities';
            $http({ method: "GET", url: mDataUrl, dataType: 'json', headers: { 'Content-Type': 'application/json' } })
                .then(
                    /*** success ***/
                    function (result) {
                        deferred.resolve(result.data);
                    },
                    /*** error ***/
                    function (response) {
                        deferred.reject(response);
                    }
                );
            return deferred.promise;
        };

        thisSvc.save = function (uiProfile, action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/SecurityEntities/Save/?Action=" + action;
            var options = {
                method: "POST",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(uiProfile)
            };
            $http(options).then(
                /*** success ***/
                function (response) {
                    deferred.resolve(response.data);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        };

        thisSvc.selectSecurityEntity = function (selectedSecurityEntityId) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Accounts/SelectSecurityEntity/?Action=SelectASecurityEntity&selectedSecurityEntityId=" + selectedSecurityEntityId;
            $http({ method: "POST", url: mApiUrl, dataType: 'json', headers: { 'Content-Type': 'application/json' } }).then(
                /*** success ***/
                function (response) {
                    $rootScope.$broadcast('accountChanged', []);
                    deferred.resolve(response.data);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        };

        return thisSvc;
    };

    mRetSvc.$inject = mDependencyInjection

    angular.module(mApplication).service(mServiceName, mRetSvc);
})();