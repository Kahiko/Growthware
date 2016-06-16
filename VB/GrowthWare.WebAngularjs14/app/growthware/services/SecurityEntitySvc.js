(function () {
    'use strict';

    var securityEntitySvc = function ($http, $q, $rootScope) {
        var thisSvc = this;

        thisSvc.getValidSecurityEntities = function () {
            var deferred = $q.defer();
            var mDataUrl = GW.Common.getBaseURL() + '/gw/api/SecurityEntities/GetValidSecurityEntities';
            $http({ method: "GET", url: mDataUrl, dataType: 'json', headers: { 'Content-Type': 'application/json' } })
            .success(function (result) {
                deferred.resolve(result);
            }).error(function (response) {
                deferred.reject(response);
            });
            return deferred.promise;
        };

        thisSvc.selectSecurityEntity = function (selectedSecurityEntityId) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Accounts/SelectSecurityEntity/?Action=SelectASecurityEntity&selectedSecurityEntityId=" + selectedSecurityEntityId;
            $http({ method: "POST", url: mApiUrl, dataType: 'json', headers: { 'Content-Type': 'application/json' } })
            .success(function (response) {
                $rootScope.$broadcast('accountChanged', []);
                deferred.resolve(response);
            }).error(function (response) {
                GW.Common.debug(response);
                deferred.reject(response);
            });
            return deferred.promise;
        };

        return thisSvc;
    };

    securityEntitySvc.$inject = ['$http', '$q', '$rootScope']

    angular.module('growthwareApp').service('SecurityEntityService', securityEntitySvc);
})();