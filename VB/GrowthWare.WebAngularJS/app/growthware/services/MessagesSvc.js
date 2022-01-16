(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mServiceName = 'MessageService';
    var mDependencyInjection = ['$http', '$q'];
    function mRetSvc($http, $q) {
        var thisSvc = this;
        var m_NavigationCount = 0;
        var m_LastMsg = '';

        thisSvc.brodcastClientMsg = function (msg) {
            // GW.Common.debug('Triggering event ~UIClientMsgEvent');
            if (m_LastMsg == '' && msg == '') {
                m_NavigationCount = 0;
            }
            jQuery.event.trigger('~UIClientMsgEvent', { message: msg });
        }

        thisSvc.getClientMessage = function () {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Messages/GetExceptionError";
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

        thisSvc.getMessageProfile = function (messageSeqID) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Messages/GetMessageProfile?messageSeqID=" + messageSeqID;
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

        thisSvc.getProfileForEdit = function (messageSeqID) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Messages/GetProfileForEdit?messageSeqID=" + messageSeqID;
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

        thisSvc.navigationHasHappened = function () {
            m_NavigationCount += 1;
            if (m_NavigationCount >= 2) {
                m_NavigationCount = 0;
                thisSvc.brodcastClientMsg('');
            }
        }

        thisSvc.save = function (profile, action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Messages/Save/?Action=" + action;
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
    }

    mRetSvc.$inject = mDependencyInjection;

    angular.module(mApplication).factory(mServiceName, mRetSvc);
})();