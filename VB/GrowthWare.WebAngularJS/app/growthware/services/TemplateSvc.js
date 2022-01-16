(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mServiceName = 'YourNamedService';
    var mDependencyInjection = ['$http', '$q'];
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

        return thisSvc;
    }

    mRetSvc.$inject = mDependencyInjection;

    angular.module(mApplication).factory(mServiceName, mRetSvc);
})();