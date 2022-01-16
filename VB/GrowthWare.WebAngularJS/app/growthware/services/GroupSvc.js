(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mServiceName = 'GroupService';
    var mDependencyInjection = ['$http', '$q'];

    var mRetSvc = function ($http, $q) {
        var thisSvc = this;

        thisSvc.delete = function (action, id) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Groups/Delete/?Action=" + action;
            mApiUrl += "&groupSeqId=" + id;
            var options = {
                method: "POST",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' }
            }
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
        }

        thisSvc.getGroups = function (action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Groups/GetGroups/?Action=" + action;
            var options = {
                method: "GET",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' }
            }
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

        thisSvc.getGroupForEdit = function (action, id) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Groups/GetGroupForEdit/?Action=" + action;
            mApiUrl += "&groupId=" + id;
            var options = {
                method: "GET",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' }
            }
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

        thisSvc.save = function (action, profile) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Groups/Save/?Action=" + action;
            var options = {
                method: "POST",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(profile)
            }
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

        return thisSvc;
    };

    mRetSvc.$inject = mDependencyInjection

    angular.module(mApplication).service(mServiceName, mRetSvc);
})();