(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'SearchController';
    var mDependencyInjection = ['AccountService', 'SearchService', 'MessageService', 'ModalService', '$route'];

    function mRetCntrl(acctSvc, searchSvc, msgSvc, modalSvc, $route) {
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
            msgSvc.brodcastClientMsg('');
            var editUrl = GW.Common.getBaseURL() + m_EditUrl + editKeyValue;
            var options = GW.Model.DefaultWebMethodOptions();
            options.url = editUrl;
            options.title = 'testing add edit';
            acctSvc.getSecurityInfo(thisCtrl.editAction).then(
                /*** success ***/
                function (securityInfo) {
                    if (securityInfo.MayView) {
                        var mController = '';

                        angular.forEach($route.routes, function (theRoute, key) {
                            if (theRoute.controller) {
                                if (theRoute.Action == thisCtrl.editAction) {
                                    mController = theRoute.controller;
                                }
                            }
                        });

                        var mModalOptions = modalSvc.options;
                        mModalOptions.controller = mController;
                        mModalOptions.data = {editId: editKeyValue};
                        mModalOptions.url = options.url;
                        mModalOptions.size = 'lg';

                        modalSvc.showModal(mModalOptions).then(
                            /*** close ***/
                            function (result) {
                                GW.Common.debug('close data: ' + result)
                            },
                            /*** dismiss ***/
                            function (reason) {
                                GW.Common.debug('Modal dismissed, reason : ', reason);
                            }
                        );
                    } else {
                    };
                },
                /*** error ***/
                function (result) {
                    console.log("Failed to getSecurityInfo, result is " + result);
                }
            );
        }

        function initCtrl() {
            thisCtrl.canSelectAll = false;
            acctSvc.getSecurityInfo(m_Action).then(
                /*** success ***/
                function (securityInfo) {
                    thisCtrl.securityInfo = securityInfo;
                },
                /*** error ***/
                function (result) {
                    console.log("Failed to getSecurityInfo, result is " + result);
                }
            );
            thisCtrl.selectedPage = { "value": "1", "text": "1" };
            thisCtrl.searchCriteria = new GW.Model.SearchCriteria()
            thisCtrl.sortText = '';
            acctSvc.getPreferences().then(
                /*** success ***/
                function (clientChoices) {
                    thisCtrl.clientChoices = clientChoices;
                    thisCtrl.searchCriteria.PageSize = thisCtrl.clientChoices.RecordsPerPage;
                    getSearchConfiguration();
                },
                /*** error ***/
                function (result) {
                    console.log("Failed to getPreferences, result is " + result);
                }
            );
            thisCtrl.vm = m_ViewModel;
        };

        function getSearchConfiguration() {
            searchSvc.getSearchConfiguration(m_Action).then(
                /* success function */
                function (searchInfo) {
                    thisCtrl.editAction = searchInfo.editAction;
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
            thisCtrl.searchCriteria.Columns = mColumns.substr(0, mColumns.length - 2);
            thisCtrl.searchCriteria.OrderByColumn = orderByColumn;
            thisCtrl.headerColumns = results
            m_EditUrl = editUrl;
            m_ApiUrl = apiURL;
        };

        function getData() {
            searchSvc.getSearchResults(m_ApiUrl, thisCtrl.searchCriteria)
            .then(
            /*** Success ***/
            function (response) {
                if (response && response.length > 0) {
                    setSelectPageDropData(response[0]['TotalRecords']);
                } else {
                    setSelectPageDropData(0);
                }
                thisCtrl.dataRows = response;
            },
            /*** Error ***/
            function (result) {
                console.log("Failed to get getSearchResults, result is " + result);
            });
        };

        function setSelectPageDropData(totalRecords) {
            totalRecords = parseInt(totalRecords);
            var mTotalPages = Math.ceil(totalRecords / thisCtrl.searchCriteria.PageSize);
            if (mTotalPages == 0) mTotalPages = 1;
            var mPageArray = new Array();
            for (var i = 1; i <= mTotalPages; i++) {
                mPageArray.push({ "value": i, "text": i })
            }
            thisCtrl.totalRecords = totalRecords;
            thisCtrl.totalPages = mPageArray;
        };

// Header
        thisCtrl.onAddNew = function () {
            addEdit(-1);
        }

        thisCtrl.onSearchChanged = function () {
            if (thisCtrl.sortText.length > 0) {
                thisCtrl.searchCriteria.WhereClause = '[' + thisCtrl.searchCriteria.OrderByColumn + ']' + ' LIKE \'%' + thisCtrl.sortText + '%\'';
            } else {
                thisCtrl.searchCriteria.WhereClause = '1 = 1';
            }
            getData();
        }

        thisCtrl.onRecordsChanged = function () {
            if (thisCtrl.searchCriteria.PageSize <= 0 || isNaN(thisCtrl.searchCriteria.PageSize)) {
                thisCtrl.searchCriteria.PageSize = 10;
            }
            getData();
        }

// Content/Table - TH
        thisCtrl.headerColumnFilter = function (header) {
            return header.visible;
        };

        thisCtrl.onChangeSort = function (columnName) {
            if (columnName != null && thisCtrl.searchCriteria.OrderByColumn != columnName) {
                thisCtrl.searchCriteria.OrderByColumn = columnName;
                thisCtrl.searchCriteria.WhereClause = thisCtrl.searchCriteria.WhereClause.replace(/\[[^\]]+\]/g, columnName)
                thisCtrl.searchCriteria.OrderByDirection = "desc";
            }
            if (thisCtrl.searchCriteria.OrderByDirection === "asc") {
                thisCtrl.searchCriteria.OrderByDirection = "desc";
            } else {
                thisCtrl.searchCriteria.OrderByDirection = "asc";
            }
            getData();
        }

// Content/Table - TD
        thisCtrl.onRowDoubleClick = function (item) {
            addEdit(item[m_EditKey]);
        };

// Footer (Pager)
        thisCtrl.onSelectPageChange = function (item) {
            thisCtrl.searchCriteria.SelectedPage = thisCtrl.selectedPage.value;;
            getData();
        }

        thisCtrl.onFirstPage = function () {
            thisCtrl.searchCriteria.SelectedPage = 1;
            thisCtrl.selectedPage.value = thisCtrl.searchCriteria.SelectedPage;
            getData();
        }

        thisCtrl.onPreviousPage = function () {
            if (thisCtrl.searchCriteria.SelectedPage > 0) {
                thisCtrl.searchCriteria.SelectedPage = thisCtrl.searchCriteria.SelectedPage - 1;
                thisCtrl.selectedPage.value = thisCtrl.searchCriteria.SelectedPage;
                getData();
            }
        }

        thisCtrl.onNextPage = function () {
            if (thisCtrl.searchCriteria.SelectedPage < thisCtrl.totalRecords) {
                thisCtrl.searchCriteria.SelectedPage = thisCtrl.searchCriteria.SelectedPage + 1;
                thisCtrl.selectedPage.value = thisCtrl.searchCriteria.SelectedPage;
                getData();
            }
        }

        thisCtrl.onLastPage = function () {
            thisCtrl.searchCriteria.SelectedPage = thisCtrl.totalPages.length;
            thisCtrl.selectedPage.value = thisCtrl.searchCriteria.SelectedPage;
            getData();
        }

        return thisCtrl;
    };

    mRetCntrl.$inject = mDependencyInjection;

    angular.module(mApplication).filter('formatData', ['SearchService', function (searchSvc) {
        return function (data, format) {
            if (format == 'date') {
                return searchSvc.formatDate(data);
            }
            return data;
        }
    }]);

    angular.module(mApplication).controller(mControllerName, mRetCntrl);

})();

