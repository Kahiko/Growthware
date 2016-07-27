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

    function mRetCntrl(acctSvc, searchSvc, $scope, $route, $location, $uibModal) {
        // init
        var thisCtrl = this;
        var m_ApiUrl = {};
        var m_EditUrl = {};
        var m_EditKey = {};
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        var viewModel = {};

        initCtrl();

        function addEdit(editKeyValue) {
            var editUrl = GW.Common.getBaseURL() + m_EditUrl + editKeyValue;
            var options = GW.Model.DefaultWebMethodOptions();
            options.url = editUrl;
            options.title = 'testing add edit';
            acctSvc.getSecurityInfo(viewModel.editAction).then(
                /*** success ***/
                function (securityInfo) {
                    searchSvc.editId = editKeyValue;
                    //var btns = [];
                    //var btnSave = {
                    //    id: 'btnSave',
                    //    label: 'Save',
                    //    cssClass: 'btn-primary',
                    //    icon: 'glyphicon glyphicon-save',
                    //    action: function (dialog) {
                    //        var $button = this; // 'this' here is a jQuery object that wrapping the <button> DOM element.
                    //        $button.disable();
                    //        $button.spin();
                    //    }
                    //};
                    //var btnCancel = {
                    //    id: 'btnCancel',
                    //    label: 'Cancel',
                    //    cssClass: 'btn-primary',
                    //    icon: 'glyphicon glyphicon-thumbs-down',
                    //    action: function (dialog) {
                    //        dialog.close();
                    //    }
                    //};
                    //var btnDelete = {
                    //    id: 'btnDelete',
                    //    label: 'Delete',
                    //    cssClass: 'btn-primary',
                    //    icon: 'glyphicon glyphicon-remove',
                    //    action: function (dialog) {
                    //        dialog.close();
                    //    }
                    //};
                    //btns.push(btnSave);
                    //btns.push(btnDelete);
                    //btns.push(btnCancel);
                    //var AddEditAccountController = angular.module('growthwareApp').controller('AddEditAccountController');
                    //BootstrapDialog.show({
                    //    title: options.title,
                    //    buttons: btns,
                    //    cssClass: 'add-edit-dialog',
                    //    onshow: function (dialog) {
                    //        if (!securityInfo.MayEdit) dialog.getButton('btnSave').disable();
                    //        if (!securityInfo.MayDelete) dialog.getButton('btnDelete').disable();
                    //    },
                    //    message: $('<div></div>').load(m_EditUrl + editKeyValue),
                    //    controller: AddEditAccountController
                    //});


                    //message: $('<div></div>').load(options.url)
                    //var modalInstance = $uibModal.open({
                    //    animation: $scope.animationsEnabled,
                    //    templateUrl: options.url,
                    //    controller: 'AddEditAccountController',
                    //    size: 'lg',
                    //    resolve: {
                    //        items: function () {
                    //            return $scope.items;
                    //        }
                    //    }
                    //});

                    //modalInstance.result.then(function (selectedItem) {
                    //    $scope.selected = selectedItem;
                    //}, function () {
                    //    $log.info('Modal dismissed at: ' + new Date());
                    //});


                    $location.path('/' + viewModel.editAction);
                },
                /*** error ***/
                function (result) {
                    console.log("Failed to getSecurityInfo, result is " + result);
                }
            );
        }

        function initCtrl() {
            viewModel.canSelectAll = false;
            acctSvc.getSecurityInfo(m_Action).then(
                /*** success ***/
                function (securityInfo) {
                    viewModel.securityInfo = securityInfo;
                },
                /*** error ***/
                function (result) {
                    console.log("Failed to getSecurityInfo, result is " + result);
                }
            );
            var lastSearchRoute = searchSvc.lastSearchRoute || "";
            if (lastSearchRoute != m_Route) {
                searchSvc.lastSearchRoute = m_Route;
                viewModel.selectedPage = { "value": "1", "text": "1" };
                viewModel.searchCriteria = new GW.Model.SearchCriteria()
                viewModel.sortText = '';
                acctSvc.getPreferences().then(
                    /*** success ***/
                    function (clientChoices) {
                        viewModel.clientChoices = clientChoices;
                        viewModel.searchCriteria.PageSize = viewModel.clientChoices.RecordsPerPage;
                        getSearchConfiguration();
                    },
                    /*** error ***/
                    function (result) {
                        console.log("Failed to getPreferences, result is " + result);
                    }
               );
            } else {
                viewModel.searchCriteria = searchSvc.lastCriteria;
                acctSvc.getPreferences().then(
                    /*** success ***/
                    function (clientChoices) {
                        viewModel.clientChoices = clientChoices;
                    },
                    /*** error ***/
                    function (result) {
                        console.log("Failed to getPreferences, result is " + result);
                    }
               );
                var match = viewModel.searchCriteria.WhereClause.match(new RegExp("%(.*)%"))
                if (viewModel.searchCriteria.WhereClause != "1 = 1") { viewModel.sortText = match[1]; }
                viewModel.selectedPage = { "value": viewModel.searchCriteria.SelectedPage, "text": viewModel.searchCriteria.SelectedPage };
                viewModel.searchCriteria.PageSize = viewModel.searchCriteria.PageSize;
                getSearchConfiguration();
            };
            $scope.vm = viewModel;
        };

        function getSearchConfiguration() {
            searchSvc.getSearchConfiguration(m_Action).then(
                /* success function */
                function (searchInfo) {
                    viewModel.editAction = searchInfo.editAction;
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
            viewModel.searchCriteria.Columns = mColumns.substr(0, mColumns.length - 2);
            viewModel.searchCriteria.OrderByColumn = orderByColumn;
            viewModel.headerColumns = results
            m_EditUrl = editUrl;
            m_ApiUrl = apiURL;
        };

        function getData() {
            searchSvc.getSearchResults(m_ApiUrl, viewModel.searchCriteria)
            .then(
            /*** Success ***/
            function (response) {
                if (response && response.length > 0) {
                    setSelectPageDropData(response[0]['TotalRecords']);
                } else {
                    setSelectPageDropData(0);
                }
                viewModel.dataRows = response;
            },
            /*** Error ***/
            function (result) {
                console.log("Failed to get getSearchResults, result is " + result);
            });
        };

        function setSelectPageDropData(totalRecords) {
            totalRecords = parseInt(totalRecords);
            var mTotalPages = Math.ceil(totalRecords / viewModel.searchCriteria.PageSize);
            if (mTotalPages == 0) mTotalPages = 1;
            var mPageArray = new Array();
            for (var i = 1; i <= mTotalPages; i++) {
                mPageArray.push({ "value": i, "text": i })
            }
            viewModel.totalRecords = totalRecords;
            viewModel.totalPages = mPageArray;
        };

        $scope.headerColumnFilter = function (header) {
            return header.visible;
        };

        $scope.addNew = function () {
            addEdit(-1);
        }

        $scope.selectColumn = function (columnName) {
            viewModel.sortDirection = 'asc';
            viewModel.searchCriteria.OrderByColumn = columnName;
            getData();
        }

        $scope.onRowDoubleClick = function (item) {
            addEdit(item[m_EditKey]);
        };

        $scope.onRecordsChanged = function () {
            if (viewModel.searchCriteria.PageSize <= 0 || isNaN(viewModel.searchCriteria.PageSize)) {
                viewModel.searchCriteria.PageSize = 10;
            }
            getData();
        }

        $scope.onSearchChanged = function () {
            if (viewModel.sortText.length > 0) {
                viewModel.searchCriteria.WhereClause = '[' + viewModel.searchCriteria.OrderByColumn + ']' + ' LIKE \'%' + viewModel.sortText + '%\'';
            } else {
                viewModel.searchCriteria.WhereClause = '1 = 1';
            }
            getData();
        }

        $scope.changeSort = function (columnName) {
            if (columnName != null && viewModel.searchCriteria.OrderByColumn != columnName) {
                viewModel.searchCriteria.OrderByColumn = columnName;
                viewModel.searchCriteria.WhereClause = viewModel.searchCriteria.WhereClause.replace(/\[[^\]]+\]/g, columnName)
                viewModel.searchCriteria.OrderByDirection = "desc";
            }
            if (viewModel.searchCriteria.OrderByDirection === "asc") {
                viewModel.searchCriteria.OrderByDirection = "desc";
            } else {
                viewModel.searchCriteria.OrderByDirection = "asc";
            }
            getData();
        }

        $scope.firstPage = function () {
            viewModel.searchCriteria.SelectedPage = 1;
            viewModel.selectedPage.value = viewModel.searchCriteria.SelectedPage;
            getData();
        }

        $scope.previousPage = function () {
            if (viewModel.searchCriteria.SelectedPage > 0) {
                viewModel.searchCriteria.SelectedPage = viewModel.searchCriteria.SelectedPage - 1;
                viewModel.selectedPage.value = viewModel.searchCriteria.SelectedPage;
                getData();
            }
        }

        $scope.goToPage = function (item) {
            viewModel.searchCriteria.SelectedPage = viewModel.selectedPage.value;;
            getData();
        }

        $scope.nextPage = function () {
            if (viewModel.searchCriteria.SelectedPage < viewModel.totalRecords) {
                viewModel.searchCriteria.SelectedPage = viewModel.searchCriteria.SelectedPage + 1;
                viewModel.selectedPage.value = viewModel.searchCriteria.SelectedPage;
                getData();
            }
        }

        $scope.lastPage = function () {
            viewModel.searchCriteria.SelectedPage = viewModel.totalPages.length;
            viewModel.selectedPage.value = viewModel.searchCriteria.SelectedPage;
            getData();
        }

        return thisCtrl;
    };

    mRetCntrl.$inject = ['AccountService', 'SearchService', '$scope', '$route', '$location', '$uibModal'];

    app.controller('SearchController', mRetCntrl);

})();
