(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mServiceName = 'FileService';
    var mDependencyInjection = ['$http', '$q'];
    function mRetSvc($http, $q) {
        var thisSvc = this;

        thisSvc.createDirectory = function (currentDirectory, newDirectory, currentAction) {
            var mDefered = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/FileManager/CreateDirectory/?currentDirectory=" + currentDirectory;
            mApiUrl += '&newDirectory=' + newDirectory;
            mApiUrl += '&currentAction=' + currentAction;
            mApiUrl += '&Action=' + currentAction;
            // mApiUrl = encodeURI(mApiUrl);
            mApiUrl = encodeURIComponent(mApiUrl);


            var options = {
                method: "POST",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' }
            }
            $http(options).then(
                /*** success ***/
                function (response) {
                    mDefered.resolve(response.data);
                },
                /*** error ***/
                function (error) {
                    mDefered.reject(error);
                    GW.Common.debug(error);
                }
            );
            return mDefered.promise;
        }

        thisSvc.deleteFiles = function (filesToDelete, action) {
            var mDefered = $q.defer();
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/FileManager/DeleteFiles/?Action=" + action;
            var options = {
                method: "POST",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(filesToDelete)
            }
            $http(options).then(
                /*** success ***/
                function (response) {
                    mDefered.resolve(response.data);
                },
                /*** error ***/
                function (error) {
                    mDefered.reject(error);
                    GW.Common.debug(error);
                }
            );
            return mDefered.promise;
        };

        thisSvc.downLoadFile = function (requestDirectory, fileName) {
            // Looks like single parameter primitive types should be sent as part of the URL
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/FileManager/DownloadFile/?Action=" + requestDirectory.Action;
            mApiUrl += "&fileName=" + fileName;
            var options = {
                method: "POST",
                url: mApiUrl,
                responseType: 'arraybuffer',
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(requestDirectory)
            }
            $http(options).then(
                /*** success ***/
                function (response) {
                    var mFile = new Blob([response.data]);
                    var mFauxURL = window.URL.createObjectURL(mFile);
                    var mAnchor = document.createElement("a");
                    document.body.appendChild(mAnchor);
                    mAnchor.style = "display: none";
                    mAnchor.href = mFauxURL;
                    mAnchor.download = fileName;
                    mAnchor.click();
                    document.removeChild(mAnchor);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                }
            );
        };

        thisSvc.getDirectory = function (requestDirectory) {
            var deferred = $q.defer();
            // Looks like single parameter primitive types should be sent as part of the URL
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/FileManager/GetDirectory/?Action=" + requestDirectory.Action;
            var options = {
                method: "POST",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(requestDirectory)
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

        thisSvc.getLineCount = function (countInfo, action) {
            var deferred = $q.defer();
            // Looks like single parameter primitive types should be sent as part of the URL
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/FileManager/GetLineCount/?Action=" + action;
            var options = {
                method: "POST",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(countInfo)
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

        thisSvc.getTestNaturalSort = function (sortDirection) {
            var deferred = $q.defer();
            // Looks like single parameter primitive types should be sent as part of the URL
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/FileManager/GetTestNaturalSort/?Action=NaturalSort";
            mApiUrl += "&sortDirection=" + sortDirection;
            var options = {
                method: "POST",
                url: mApiUrl,
                headers: { 'Content-Type': 'application/json' },
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

        thisSvc.uploadFile = function (formData, action) {
            var mApiUrl = GW.Common.getBaseURL() + "/gw/api/FileManager/Upload/?Action=" + action;
            var deferred = $q.defer();
            // Looks like single parameter primitive types should be sent as part of the URL
            var options = {
                method: "POST",
                url: mApiUrl,
                headers: { 'Content-Type': undefined, 'Accept': 'application/json' },
                data: formData,
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
        };

        return thisSvc;
    }

    mRetSvc.$inject = mDependencyInjection;

    angular.module(mApplication).factory(mServiceName, mRetSvc);
})();