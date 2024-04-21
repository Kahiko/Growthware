(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mServiceName = 'ConfigurationService';
    var mDependencyInjection = ['$http', '$q'];
    function mRetSvc($http, $q) {
        var thisSvc = this;
        var m_ConfigInfo = null;

        thisSvc.encrypt = function (textValue, encrypt) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/ConfigInfo/Decrypt?textValue=" + textValue;
            if (encrypt) {
                mApiUrl = GW.Common.getBaseURL() + "/gw/api/ConfigInfo/Encrypt?textValue=" + textValue;
            }
            $http({
                method: "GET",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    var mResponse = response.data;
                    deferred.resolve(mResponse);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        }

        thisSvc.getConfigInfo = function () {
            var deferred = $q.defer();
            if (m_ConfigInfo != null) {
                //GW.Common.debug('m_ConfigInfo from cache.')
                deferred.resolve(m_ConfigInfo);
            } else {
                //GW.Common.debug('m_ConfigInfo from server.')
                var mApiUrl = GW.Common.getBaseURL() + "/gw/api/ConfigInfo/GetConfigInfo";
                $http({
                    method: "GET",
                    url: mApiUrl,
                    dataType: 'json',
                    headers: { 'Content-Type': 'application/json' }
                }).then(
                    /*** success ***/
                    function (response) {
                        var mResponse = response.data;
                        var mRetVal = {
                            "ApplicationName": mResponse.ApplicationName,
                            "Environment": mResponse.Environment,
                            "FrameworkVersion": mResponse.FrameworkVersion,
                            "LogLevel": mResponse.LogLevel,
                            "SecurityEntityTranslation": mResponse.SecurityEntityTranslation,
                            "Version": mResponse.Version,
                        };
                        deferred.resolve(mRetVal);
                    },
                    /*** error ***/
                    function (response) {
                        GW.Common.debug(response);
                        deferred.reject(response);
                    }
                );
            }
            return deferred.promise;
        }

        thisSvc.getDBInformation = function () {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/ConfigInfo/GetDBInformation";
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    var mResponse = response.data;
                    var mRetVal = {
                        "informationSeqId": mResponse.m_InformationSeqId,
                        "version": mResponse.m_Version,
                        "enableInheritance": mResponse.m_EnableInheritance,
                    };
                    deferred.resolve(mRetVal);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        }

        thisSvc.getLogLevel = function () {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/ConfigInfo/GetLogLevel";
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    var mRetVal = response.data;
                    deferred.resolve(mRetVal);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        }

        thisSvc.getGUID = function () {
            var mRetVal = '';
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/ConfigInfo/GetGUID";
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    var mRetVal = response.data;
                    deferred.resolve(mRetVal);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        }

        thisSvc.getRandomNumbers = function (amountOfNumbers, maxNumber, minNumber) {
            var mRetVal = "";
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/ConfigInfo/GetRandomNumbers";
            mApiUrl += "?amountOfNumbers=" + amountOfNumbers;
            mApiUrl += "&maxNumber=" + maxNumber;
            mApiUrl += "&minNumber=" + minNumber
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    var mData = response.data;
                    deferred.resolve(mData);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        }

        thisSvc.saveDBInformation = function (enableInheritance) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/ConfigInfo/SaveDBInformation?enableInheritance=" + enableInheritance;
            $http({
                method: "POST",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    var mRetVal = response.data;
                    deferred.resolve(mRetVal);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        }

        thisSvc.setLogLevel = function (logLevel) {
            var mRetVal = '';
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/ConfigInfo/SetLogLevel/?logLevel=" + logLevel;
            $http({
                method: "POST",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    mRetVal = response.data;
                    deferred.resolve(mRetVal);
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