(function () {
    'use strict';

    var searchSvc = function ($http, $q, $resource) {
        var thisSvc = this;

        var m_SearchInfo = null;

        thisSvc.Prototype = {
            get lastCriteria() {
                return this.m_LastCriteria;
            },
            set lastCriteria(value) {
                this.m_LastCriteria = value;
            }
        };

        thisSvc.Prototype = {
            get lastSearchRoute() {
                return this.m_LastSearchRoute;
            },
            set lastSearchRoute(value) {
                this.m_LastSearchRoute = value;
            }
        };

        thisSvc.Prototype = {
            get editId() {
                return this.m_EditId;
            },
            set editId(value) {
                this.m_EditId = value;
            }
        };
        thisSvc.getSearchConfiguration = function (route) {
            var deferred = $q.defer();
            if (m_SearchInfo != null) {
                deferred.resolve(getSearchTableInfo(route));
            } else {
                var mDataUrl = GW.Common.getBaseURL() + '/app/growthware/data/Search.config.json';
                $http({ method: "GET", url: mDataUrl, dataType: 'json', headers: { 'Content-Type': 'application/json' } })
                .success(function (result) {
                    m_SearchInfo = result;
                    deferred.resolve(getSearchTableInfo(route));
                })
                .error(function (response) {
                    deferred.reject(response);
                });
            }
            return deferred.promise;
        };

        thisSvc.getSearchResults = function (url, criteria) {
            this.lastCriteria = criteria;
            criteria = JSON.stringify(criteria);
            var deferred = $q.defer();
            $http({method: "POST", url: url, dataType: 'json', data: criteria, headers: { 'Content-Type': 'application/json' }})
            .success(function (result) {
                deferred.resolve(result);
            })
            .error(function (response) {
                deferred.reject(response);
            });
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

    searchSvc.$inject = ['$http', '$q', '$resource']

    angular.module('growthwareApp').service('SearchService', searchSvc);
})();