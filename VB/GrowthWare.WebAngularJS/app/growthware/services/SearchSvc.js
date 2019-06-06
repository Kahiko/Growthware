(function () {
    'use strict';

    var mRetSvc = function ($http, $q) {
        var thisSvc = this;

        var m_SearchInfo = null;

        thisSvc.getSearchConfiguration = function (route) {
            var deferred = $q.defer();
            if (m_SearchInfo != null) {
                deferred.resolve(getSearchTableInfo(route));
            } else {
                var mDataUrl = GW.Common.getBaseURL() + '/app/growthware/data/Search.config.json';
                $http({ method: "GET", url: mDataUrl, dataType: 'json', headers: { 'Content-Type': 'application/json' } })
                    .then(
                        /*** success ***/
                        function (result) {
                            m_SearchInfo = result.data;
                            deferred.resolve(getSearchTableInfo(route));
                        },
                        /*** error ***/
                        function (response) {
                            deferred.reject(response);
                        }
                    );
            }
            return deferred.promise;
        };

        thisSvc.getSearchResults = function (url, criteria) {
            criteria = JSON.stringify(criteria);
            var deferred = $q.defer();
            $http({method: "POST", url: url, dataType: 'json', data: criteria, headers: { 'Content-Type': 'application/json' }})
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

        thisSvc.formatDate = function (sqlDate) {
            var dateTime = new Date(sqlDate);
            var mask = 'ddd, mmm d, yyyy h:MM TT';
            return dateTime.format(mask);
        };

        function getSearchTableInfo(route) {
            var searchInfo = [];
            for (var i = 0; i < m_SearchInfo.length; i++) {
                if (m_SearchInfo[i].action == route) {
                    searchInfo = m_SearchInfo[i];
                    break;
                }
            }
            return searchInfo;
        }
    };

    mRetSvc.$inject = ['$http', '$q', '$resource']

    angular.module('growthwareApp').service('SearchService', mRetSvc);
})();