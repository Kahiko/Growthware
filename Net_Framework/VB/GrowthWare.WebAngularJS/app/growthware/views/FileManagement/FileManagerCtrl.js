(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'FileManagerController';
    var mDependencyInjection = ['AccountService', 'FileService', 'SearchService', 'MessageService', 'ModalService', '$scope', '$route', '$uibModal'];

    function mRetCntrl(acctSvc, fileSvc, searchSvc, msgSvc, modalSvc, $scope, $route, $uibModal) {
        // init
        var thisCtrl = this;
        var m_ApiUrl = {};
        var m_EditUrl = {};
        var m_EditKey = {};
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        var m_ViewModel = {};
        thisCtrl.currentDirectory = "/";
        thisCtrl.searchCriteria = new GW.Model.SearchCriteria()
        thisCtrl.securityInfo = {};
        thisCtrl.selectedPage = { "value": "1", "text": "1" };
        thisCtrl.sortText = '';

        var m_RequestDirectory = {
            Action: m_Action,
            CurrentDirectoryString: "/",
            SearchCriteria: thisCtrl.searchCriteria
        };

        initCtrl();

        function addEdit(editKeyValue) {
            msgSvc.brodcastClientMsg('');
            var editUrl = GW.Common.getBaseURL() + m_EditUrl + editKeyValue;
            var options = GW.Model.DefaultWebMethodOptions();
            options.url = editUrl;
            options.title = 'testing add edit';
            acctSvc.getSecurityInfo(m_Action).then(
                /*** success ***/
                function (securityInfo) {
                    if (securityInfo.MayView) {
                        var mModalOptions = modalSvc.options;
                        mModalOptions.controller = 'AddDirectoryController';
                        mModalOptions.data = {
                            currentDirectory: thisCtrl.currentDirectory,
                            newDirectory: '',
                            currentAction: m_Action
                        };
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

        function changeFolder(folderName) {
            thisCtrl.currentDirectory = thisCtrl.currentDirectory.toLocaleLowerCase().trim()
            folderName = folderName.toLocaleLowerCase().trim()
            if (!thisCtrl.currentDirectory.endsWith("/")) {
                thisCtrl.currentDirectory += "/";
            }
            thisCtrl.currentDirectory += folderName;
            m_RequestDirectory.CurrentDirectoryString = thisCtrl.currentDirectory;
            getData();
        };

        function getData() {
            fileSvc.getDirectory(m_RequestDirectory).then(
                /*** Success ***/
                function (response) {
                    if (response && response.length > 0) {
                        setSelectPageDropData(response[0]['TotalRows']);
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

        function initCtrl() {

            var uploadHandler = GW.Common.getBaseURL();
            uploadHandler = uploadHandler + '/UploadHandler.upload?Action=' + m_Action;
            try {
                GW.Upload.BtyesPerChunk = 4194304
                GW.Upload.BtyesPerChunk = 2097152;
                GW.Upload.BtyesPerChunk = 1048576
                GW.Upload.BtyesPerChunk = 3145728;
                //GW.Upload.BtyesPerChunk = 30720;
                GW.Upload.uploadHandler = uploadHandler;
                // GW.Upload.init();
            } catch (e) {
                alert("Error in FileManager.htms document.ready::\n" + e.Message);
            }


            msgSvc.brodcastClientMsg("Add Folder isn't working as of yet and upload broken");
            // https://www.encodedna.com/angularjs/tutorial/angularjs-multiple-file-upload-example-with-webapi.htm
            acctSvc.getSecurityInfo(m_Action).then(
                /*** success ***/
                function (securityInfo) {
                    thisCtrl.securityInfo = securityInfo;
                    // next two lines for testing ui
                    // thisCtrl.securityInfo.MayDelete = false;
                    // thisCtrl.securityInfo.MayAdd = false;
                    return acctSvc.getPreferences();
                },
                /*** error ***/
                function (result) {
                    console.log("Failed to getSecurityInfo, result is " + result);
                }
            ).then(
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
            $scope.vm = m_ViewModel;
        };

        thisCtrl.selectAll = function () {
            var btn = document.getElementById('cmdSelect');
            var checked = false;
            if (btn.value == "Select All") {
                btn.value = "De-Select All";
                checked = true;
            } else {
                btn.value = "Select All";
                checked = false;
            }
            try {
                var allHTMLInputs = document.getElementsByTagName('input');
                for (var i = 0; i < allHTMLInputs.length; i++) {
                    var htmlInput = allHTMLInputs[i];
                    if (htmlInput.type == 'checkbox') {
                        htmlInput.checked = checked;
                    }
                }
            } catch (e) {
                //alert(e.message);
            }
        }

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

        thisCtrl.onDelete = function () {
            var mSelectedFiles = [];
            var exampleMUIFileInfo = {
                Action: 'As string',
                CurrentDirectory: 'As String',
                FileName: 'As String',
                FileType: 'As String',
            }

            try {
                document.querySelectorAll('input').forEach(function (element) {
                    if (element.type == 'checkbox' && element.checked == true) {
                        var mData = element.value.split(",")
                        var mName = mData[0].split(":")[1].trim();
                        var mType = mData[1].split(":")[1].replace("}", "").trim();
                        var mFileInfo = {
                            Action: m_Action,
                            CurrentDirectory: thisCtrl.currentDirectory,
                            FileName: mName,
                            FileType: mType,
                        }
                        mSelectedFiles.push(mFileInfo);
                    }
                });
                // console.log(mSelectedFiles);
                fileSvc.deleteFiles(mSelectedFiles, m_Action).then(function (response) {
                    msgSvc.brodcastClientMsg(response)
                    getData();
                }).catch(function (error) {
                    console.log(error);
                });
            } catch (e) {
                alert(e.message);
            }
            // alert("not working atm");
        };

        thisCtrl.onRecordsChanged = function () {
            if (thisCtrl.searchCriteria.PageSize <= 0 || isNaN(thisCtrl.searchCriteria.PageSize)) {
                thisCtrl.searchCriteria.PageSize = 10;
            }
            getData();
        }

        thisCtrl.onSearchChanged = function () {
            if (thisCtrl.sortText.length > 0) {
                thisCtrl.searchCriteria.WhereClause = '[' + thisCtrl.searchCriteria.OrderByColumn + ']' + ' LIKE \'%' + thisCtrl.sortText + '%\'';
            } else {
                thisCtrl.searchCriteria.WhereClause = '1 = 1';
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
        thisCtrl.onDownLoadFile = function (fileName) {
            fileSvc.downLoadFile(m_RequestDirectory, fileName);
        };

        thisCtrl.onChangeFolder = function (folderName) {
            changeFolder(folderName);
        }

        thisCtrl.onRowDoubleClick = function (item) {
            addEdit(item[m_EditKey]);
        };

        // Footer (Pager)
        thisCtrl.onFirstPage = function () {
            thisCtrl.searchCriteria.SelectedPage = 1;
            thisCtrl.selectedPage.value = thisCtrl.searchCriteria.SelectedPage;
            getData();
        }

        thisCtrl.onLastPage = function () {
            thisCtrl.searchCriteria.SelectedPage = thisCtrl.totalPages.length;
            thisCtrl.selectedPage.value = thisCtrl.searchCriteria.SelectedPage;
            getData();
        }

        thisCtrl.onNextPage = function () {
            if (thisCtrl.searchCriteria.SelectedPage < thisCtrl.totalRecords) {
                thisCtrl.searchCriteria.SelectedPage = thisCtrl.searchCriteria.SelectedPage + 1;
                thisCtrl.selectedPage.value = thisCtrl.searchCriteria.SelectedPage;
                getData();
            }
        }

        thisCtrl.onPreviousPage = function () {
            if (thisCtrl.searchCriteria.SelectedPage > 0) {
                thisCtrl.searchCriteria.SelectedPage = thisCtrl.searchCriteria.SelectedPage - 1;
                thisCtrl.selectedPage.value = thisCtrl.searchCriteria.SelectedPage;
                getData();
            }
        }

        thisCtrl.onSelectPageChange = function (item) {
            thisCtrl.searchCriteria.SelectedPage = thisCtrl.selectedPage.value;;
            getData();
        }

        // GET THE FILE INFORMATION.
        thisCtrl.getFileDetails = function (e) {

            $scope.files = [];
            $scope.$apply(function () {

                // STORE THE FILE OBJECT IN AN ARRAY.
                for (var i = 0; i < e.files.length; i++) {
                    $scope.files.push(e.files[i])
                }

            });
        };

        // NOW UPLOAD THE FILES.
        // https://www.c-sharpcorner.com/UploadFile/2b481f/uploading-a-file-in-Asp-Net-web-api/
        $scope.uploadFiles = function () {

            //FILL FormData WITH FILE DETAILS.
            var mFormData = new FormData();

            for (var i in $scope.files) {
                mFormData.append("uploadedFile", $scope.files[i]);
            }

            mFormData.append('requestDirectory', JSON.stringify(m_RequestDirectory));

            fileSvc.uploadFile(mFormData, m_Action).then(function (response) {
                msgSvc.brodcastClientMsg(response);
                getData();
            });
        }

        // UPDATE PROGRESS BAR.
        function updateProgress(e) {
            if (e.lengthComputable) {
                document.getElementById('pro').setAttribute('value', e.loaded);
                document.getElementById('pro').setAttribute('max', e.total);
            }
        }

        // CONFIRMATION.
        function transferComplete(e) {
            alert("Files uploaded successfully.");
        }

        return thisCtrl;
    };

    mRetCntrl.$inject = mDependencyInjection;

    var app = angular.module(mApplication);

    app.filter('formatData', ['SearchService', function (searchSvc) {
        return function (data, format) {
            if (format == 'date') {
                return searchSvc.formatDate(data);
            }
            return data;
        }
    }]);

    app.controller(mControllerName, mRetCntrl);

})();
