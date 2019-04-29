(function () {
    'use strict';

    var mRetSvc = function ($http, $q, $rootScope) {
        var thisSvc = this;

        thisSvc.getGroups = function (action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Groups/GetGroups/?Action=" + action;
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

        return thisSvc;
    };

    mRetSvc.$inject = ['$http', '$q']

    angular.module('growthwareApp').service('GroupService', mRetSvc);
})();