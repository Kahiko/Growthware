(function () {
    'use strict';

    var app = angular.module('growthwareApp');

    app.filter('formatData', ['SearchService', function (searchSvc) {
        return function (data, format) {
            if (format == 'date') {
                return searchSvc.formatDate(data);
            }
            return data;
        }
    }]);

    function mRetCntrl(acctSvc, searchSvc, $scope, $route, $uibModal) {
        // init
        var thisCtrl = this;
        var m_ApiUrl = {};
        var m_EditUrl = {};
        var m_EditKey = {};
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        var m_ViewModel = {};

        initCtrl();

        function addEdit(editKeyValue) {
            var editUrl = GW.Common.getBaseURL() + m_EditUrl + editKeyValue;
            var options = GW.Model.DefaultWebMethodOptions();
            options.url = editUrl;
            options.title = 'testing add edit';
            acctSvc.getSecurityInfo(m_ViewModel.editAction).then(
                /*** success ***/
                function (securityInfo) {
                    var mController = '';

                    angular.forEach($route.routes, function (theRoute, key) {
                        if (theRoute.controller) {
                            if (theRoute.Action == m_ViewModel.editAction) {
                                mController = theRoute.controller;
                            }
                        }
                    });
                    searchSvc.editId = editKeyValue;

                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: options.url,
                        controller: mController,
                        size: 'lg',
                        resolve: {
                            modalData: function () { return ['item1', 'item2', 'item3']; }
                        }
                    });

                    modalInstance.result.then(
                        /*** close ***/
                        function (selectedItem) {
                            console.log('handeling close');
                            console.log(selectedItem);
                            $route.reload();
                        },
                        /*** dismiss ***/
                        function (cancelData) {
                            console.log(cancelData);
                            console.log('Modal dismissed at: ' + new Date());
                        });


                    //$location.path('/' + m_ViewModel.editAction);
                },
                /*** error ***/
                function (result) {
                    console.log("Failed to getSecurityInfo, result is " + result);
                }
            );
        }

        function initCtrl() {
            m_ViewModel.canSelectAll = false;
            acctSvc.getSecurityInfo(m_Action).then(
                /*** success ***/
                function (securityInfo) {
                    m_ViewModel.securityInfo = securityInfo;
                },
                /*** error ***/
                function (result) {
                    console.log("Failed to getSecurityInfo, result is " + result);
                }
            );
            var lastSearchRoute = searchSvc.lastSearchRoute || "";
            if (lastSearchRoute != m_Route) {
                searchSvc.lastSearchRoute = m_Route;
                m_ViewModel.selectedPage = { "value": "1", "text": "1" };
                m_ViewModel.searchCriteria = new GW.Model.SearchCriteria()
                m_ViewModel.sortText = '';
                acctSvc.getPreferences().then(
                    /*** success ***/
                    function (clientChoices) {
                        m_ViewModel.clientChoices = clientChoices;
                        m_ViewModel.searchCriteria.PageSize = m_ViewModel.clientChoices.RecordsPerPage;
                        getSearchConfiguration();
                    },
                    /*** error ***/
                    function (result) {
                        console.log("Failed to getPreferences, result is " + result);
                    }
               );
            } else {
                m_ViewModel.searchCriteria = searchSvc.lastCriteria;
                acctSvc.getPreferences().then(
                    /*** success ***/
                    function (clientChoices) {
                        m_ViewModel.clientChoices = clientChoices;
                    },
                    /*** error ***/
                    function (result) {
                        console.log("Failed to getPreferences, result is " + result);
                    }
               );
                var match = m_ViewModel.searchCriteria.WhereClause.match(new RegExp("%(.*)%"))
                if (m_ViewModel.searchCriteria.WhereClause != "1 = 1") { m_ViewModel.sortText = match[1]; }
                m_ViewModel.selectedPage = { "value": m_ViewModel.searchCriteria.SelectedPage, "text": m_ViewModel.searchCriteria.SelectedPage };
                m_ViewModel.searchCriteria.PageSize = m_ViewModel.searchCriteria.PageSize;
                getSearchConfiguration();
            };
            $scope.vm = m_ViewModel;
        };

        function getSearchConfiguration() {
            searchSvc.getSearchConfiguration(m_Action).then(
                /* success function */
                function (searchInfo) {
                    m_ViewModel.editAction = searchInfo.editAction;
                    setHeaders(searchInfo.columnInfo, GW.Common.getBaseURL() + searchInfo.apiURL, searchInfo.orderByColumn, searchInfo.editUrl);
                    getData();
                },
                /* error function */
                function (result) {
                    console.log("Failed to get getSearchConfiguration, result is " + result);
                }
           );
        };

        function setHeaders(results, apiURL, orderByColumn, editUrl) {
            var mColumns = '';
            for (var i = 0; i < results.length; i++) {
                mColumns += results[i].name + ', ';
                if (results[i].isEditKey === true) {
                    m_EditKey = results[i].name;
                } 
            }
            m_ViewModel.searchCriteria.Columns = mColumns.substr(0, mColumns.length - 2);
            m_ViewModel.searchCriteria.OrderByColumn = orderByColumn;
            m_ViewModel.headerColumns = results
            m_EditUrl = editUrl;
            m_ApiUrl = apiURL;
        };

        function getData() {
            searchSvc.getSearchResults(m_ApiUrl, m_ViewModel.searchCriteria)
            .then(
            /*** Success ***/
            function (response) {
                if (response && response.length > 0) {
                    setSelectPageDropData(response[0]['TotalRecords']);
                } else {
                    setSelectPageDropData(0);
                }
                m_ViewModel.dataRows = response;
            },
            /*** Error ***/
            function (result) {
                console.log("Failed to get getSearchResults, result is " + result);
            });
        };

        function setSelectPageDropData(totalRecords) {
            totalRecords = parseInt(totalRecords);
            var mTotalPages = Math.ceil(totalRecords / m_ViewModel.searchCriteria.PageSize);
            if (mTotalPages == 0) mTotalPages = 1;
            var mPageArray = new Array();
            for (var i = 1; i <= mTotalPages; i++) {
                mPageArray.push({ "value": i, "text": i })
            }
            m_ViewModel.totalRecords = totalRecords;
            m_ViewModel.totalPages = mPageArray;
        };

        $scope.headerColumnFilter = function (header) {
            return header.visible;
        };

        $scope.addNew = function () {
            addEdit(-1);
        }

        $scope.selectColumn = function (columnName) {
            m_ViewModel.sortDirection = 'asc';
            m_ViewModel.searchCriteria.OrderByColumn = columnName;
            getData();
        }

        $scope.onRowDoubleClick = function (item) {
            addEdit(item[m_EditKey]);
        };

        $scope.onRecordsChanged = function () {
            if (m_ViewModel.searchCriteria.PageSize <= 0 || isNaN(m_ViewModel.searchCriteria.PageSize)) {
                m_ViewModel.searchCriteria.PageSize = 10;
            }
            getData();
        }

        $scope.onSearchChanged = function () {
            if (m_ViewModel.sortText.length > 0) {
                m_ViewModel.searchCriteria.WhereClause = '[' + m_ViewModel.searchCriteria.OrderByColumn + ']' + ' LIKE \'%' + m_ViewModel.sortText + '%\'';
            } else {
                m_ViewModel.searchCriteria.WhereClause = '1 = 1';
            }
            getData();
        }

        $scope.changeSort = function (columnName) {
            if (columnName != null && m_ViewModel.searchCriteria.OrderByColumn != columnName) {
                m_ViewModel.searchCriteria.OrderByColumn = columnName;
                m_ViewModel.searchCriteria.WhereClause = m_ViewModel.searchCriteria.WhereClause.replace(/\[[^\]]+\]/g, columnName)
                m_ViewModel.searchCriteria.OrderByDirection = "desc";
            }
            if (m_ViewModel.searchCriteria.OrderByDirection === "asc") {
                m_ViewModel.searchCriteria.OrderByDirection = "desc";
            } else {
                m_ViewModel.searchCriteria.OrderByDirection = "asc";
            }
            getData();
        }

        $scope.firstPage = function () {
            m_ViewModel.searchCriteria.SelectedPage = 1;
            m_ViewModel.selectedPage.value = m_ViewModel.searchCriteria.SelectedPage;
            getData();
        }

        $scope.previousPage = function () {
            if (m_ViewModel.searchCriteria.SelectedPage > 0) {
                m_ViewModel.searchCriteria.SelectedPage = m_ViewModel.searchCriteria.SelectedPage - 1;
                m_ViewModel.selectedPage.value = m_ViewModel.searchCriteria.SelectedPage;
                getData();
            }
        }

        $scope.goToPage = function (item) {
            m_ViewModel.searchCriteria.SelectedPage = m_ViewModel.selectedPage.value;;
            getData();
        }

        $scope.nextPage = function () {
            if (m_ViewModel.searchCriteria.SelectedPage < m_ViewModel.totalRecords) {
                m_ViewModel.searchCriteria.SelectedPage = m_ViewModel.searchCriteria.SelectedPage + 1;
                m_ViewModel.selectedPage.value = m_ViewModel.searchCriteria.SelectedPage;
                getData();
            }
        }

        $scope.lastPage = function () {
            m_ViewModel.searchCriteria.SelectedPage = m_ViewModel.totalPages.length;
            m_ViewModel.selectedPage.value = m_ViewModel.searchCriteria.SelectedPage;
            getData();
        }

        return thisCtrl;
    };

    mRetCntrl.$inject = ['AccountService', 'SearchService', '$scope', '$route', '$uibModal'];

    app.controller('SearchController', mRetCntrl);

})();

