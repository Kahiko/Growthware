(function () {
    'use strict';

    function mRetSvc($http, $q) {
        var thisSvc = this;

        thisSvc.getClientMessage = function () {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/YourNamedController/Method";
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).success(function (response) {
                deferred.resolve(response);
            }).error(function (response) {
                GW.Common.debug(response);
                deferred.reject(response);
            });
            return deferred.promise;
        }

        return thisSvc;
    }

    mRetSvc.$inject = ['$http', '$q'];

    angular.module('growthwareApp').factory('YourNamedService', mRetSvc);
})();