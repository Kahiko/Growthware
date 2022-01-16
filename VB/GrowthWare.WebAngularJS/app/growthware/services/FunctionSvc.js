(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mServiceName = 'FunctionService';
    var mDependencyInjection = ['$http', '$q'];
    function mRetSvc($http, $q) {
        var thisSvc = this;

        var m_FunctionTypes = null;
        var m_LinkBehaviors = null;
        var m_NavigationTypes = null;

        thisSvc.changeMenuOrder = function (functionSeqID, direction) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/MoveMenu?Action=Search_Functions&functionSeqId=" + functionSeqID + "&direction=" + direction;
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

        thisSvc.delete = function (functionSeqID) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/Delete?Action=Search_Functions&functionSeqID=" + functionSeqID;
            $http({
                method: "POST",
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

        thisSvc.getAvalibleParents = function (functionSeqID) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/GetAvalibleParents?functionSeqID=" + functionSeqID;
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

        thisSvc.getFunction = function (functionSeqID, action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/GetFunctionForEdit?Action=" + action + "&functionSeqID=" + functionSeqID;
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

        thisSvc.getFunctionMenuOrder = function (functionSeqID) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/GetFunctionOrder?Action=Search_Accounts&functionSeqId=" + functionSeqID;
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

        thisSvc.getFunctionTypes = function (force) {
            var deferred = $q.defer();
            if (force == null) {
                force = false;
            }
            if (m_FunctionTypes != null || force) {
                deferred.resolve(m_FunctionTypes);
            }
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/GetFuncitonTypes";
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    m_FunctionTypes = [];
                    response.data.forEach(function (item) {
                        item["FUNCTION_TYPE_SEQ_ID"] = item["FUNCTION_TYPE_SEQ_ID"] * 1;
                        m_FunctionTypes.push(item);
                    });
                    // m_FunctionTypes = response.data;
                    deferred.resolve(m_FunctionTypes);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        }

        thisSvc.getLinkBehaviors = function (force) {
            var deferred = $q.defer();
            if (force == null) {
                force = false;
            }
            if (m_LinkBehaviors != null || force) {
                deferred.resolve(m_LinkBehaviors);
            }
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/GetLinkBehaviors";
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    m_LinkBehaviors = response.data;
                    deferred.resolve(m_LinkBehaviors);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        }

        thisSvc.getNavigationTypes = function (force) {
            var deferred = $q.defer();
            if (force == null) {
                force = false;
            }
            if (m_NavigationTypes != null || force) {
                deferred.resolve(m_NavigationTypes);
            }
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/GetNavigationTypes";
            $http({
                method: "GET",
                url: mApiUrl,
                dataType: 'json',
                headers: { 'Content-Type': 'application/json' }
            }).then(
                /*** success ***/
                function (response) {
                    //m_NavigationTypes = [];
                    //response.data.forEach(function (item) {
                    //    item["FUNCTION_TYPE_SEQ_ID"] = item["FUNCTION_TYPE_SEQ_ID"] * 1;
                    //    m_NavigationTypes.push(item);
                    //});
                    m_NavigationTypes = response.data;
                    deferred.resolve(m_NavigationTypes);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    deferred.reject(response);
                }
            );
            return deferred.promise;
        }

        thisSvc.move = function (functionSeqId, direction, action) {
            var deferred = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/MoveMenu?Action=" + action;
            mApiUrl += '&functionSeqId=' + functionSeqId;
            mApiUrl += '&direction=' + direction;
            $http({
                method: "POST",
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

        thisSvc.save = function (profile, action) {
            var deferred = $q.defer();
            console.log(profile);
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/Functions/Save?Action=" + action;
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