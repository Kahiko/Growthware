(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mServiceName = 'RoleService';
    var mDependencyInjection = ['$http', '$q'];
    var mRetSvc = function ($http, $q, $rootScope) {
        var thisSvc = this;

        thisSvc.delete = function (roleSeqId, action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Roles/Delete/?Action=" + action;
            mApiUrl += '&roleSeqId=' + roleSeqId;
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
        };

        thisSvc.getRoleForEdit = function (roleSeqId) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Roles/GetRoleForEdit/?roleSeqId=" + roleSeqId;
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

        thisSvc.getRoles = function (action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Roles/GetRoles/?Action=" + action;
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

        thisSvc.save = function (profile, action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Roles/Save/?Action=" + action;
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
        }

        return thisSvc;
    };

    mRetSvc.$inject = mDependencyInjection;

    angular.module(mApplication).service(mServiceName, mRetSvc);
})();