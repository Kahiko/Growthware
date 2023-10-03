import { Injectable } from '@angular/core';
import { HttpHeaders, HttpParams } from '@angular/common/http';
import { Subject } from 'rxjs';
import { MultiPartFileUploadParameters } from './multi-part-file-upload-parameters.model';
import { UploadStatus } from './upload-status.model';
import * as i0 from "@angular/core";
import * as i1 from "@Growthware/shared/services";
import * as i2 from "@Growthware/common-code";
import * as i3 from "@angular/common/http";
import * as i4 from "@Growthware/features/logging";
export class FileManagerService {
    set CurrentDirectoryTree(value) {
        this._CurrentDirectoryTree = JSON.parse(JSON.stringify(value));
    }
    get CurrentDirectoryTree() {
        return JSON.parse(JSON.stringify(this._CurrentDirectoryTree));
    }
    get SelectedPath() {
        return this._SelectedPath;
    }
    ;
    constructor(_DataSvc, _GWCommon, _HttpClient, _LoggingSvc) {
        this._DataSvc = _DataSvc;
        this._GWCommon = _GWCommon;
        this._HttpClient = _HttpClient;
        this._LoggingSvc = _LoggingSvc;
        this._Api = '';
        this._Api_GetDirectories = '';
        this._Api_GetFiles = '';
        this._Api_CreateDirectory = '';
        this._Api_DeleteDirectory = '';
        this._Api_DeleteFile = '';
        this._Api_RenameDirectory = '';
        this._Api_RenameFile = '';
        this._Api_GetTestNaturalSort = '';
        this._Api_UploadFile = '';
        this._SelectedPath = '\\';
        this.ModalId_Rename_Directory = 'DirectoryTreeComponent.onMenuRenameClick';
        this.ModalId_CreateDirectory = "CreateDirectoryForm";
        this.uploadStatusChanged = new Subject();
        this.selectedDirectoryChanged = new Subject();
        this._Api = this._GWCommon.baseURL + 'GrowthwareFile/';
        this._Api_GetDirectories = this._Api + 'GetDirectories';
        this._Api_GetFiles = this._Api + 'GetFiles';
        this._Api_CreateDirectory = this._Api + 'CreateDirectory';
        this._Api_DeleteDirectory = this._Api + 'DeleteDirectory';
        this._Api_DeleteFile = this._Api + 'DeleteFile';
        this._Api_RenameDirectory = this._Api + 'RenameDirectory';
        this._Api_RenameFile = this._Api + 'RenameFile';
        this._Api_GetTestNaturalSort = this._Api + 'GetTestNaturalSort';
        this._Api_UploadFile = this._Api + 'UploadFile';
    }
    /**
     * Creates a new directory in the currectly selected directory
     *
     * @param {string} action
     * @param {string} selectedPath
     * @param {string} newPath
     * @return {*}  {Promise<any>}
     * @memberof FileManagerService
     */
    async createDirectory(action, newPath) {
        if (this._GWCommon.isNullOrEmpty(action)) {
            throw new Error("action can not be blank!");
        }
        if (this._GWCommon.isNullOrEmpty(newPath)) {
            throw new Error("newPath can not be blank!");
        }
        ;
        let mQueryParameter = new HttpParams().append('action', action);
        mQueryParameter = mQueryParameter.append('selectedPath', this.SelectedPath);
        mQueryParameter = mQueryParameter.append('newPath', newPath);
        const mHttpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            }),
            params: mQueryParameter,
        };
        return new Promise((resolve, reject) => {
            this._HttpClient.post(this._Api_CreateDirectory, null, mHttpOptions).subscribe({
                next: (response) => {
                    resolve(response);
                },
                error: (error) => {
                    this._LoggingSvc.errorHandler(error, 'FileManagerService', 'createDirectory');
                    reject(false);
                },
                complete: () => { }
            });
        });
    }
    /**
     * Deletes a directory, subdirectory and all files
     *
     * @param {string} action
     * @param {string} selectedPath
     * @return {*}  {Promise<boolean>}
     * @memberof FileManagerService
     */
    async deleteDirectory(action, selectedPath) {
        return new Promise((resolve, reject) => {
            if (this._GWCommon.isNullOrEmpty(action)) {
                throw new Error("action can not be blank!");
            }
            if (this._GWCommon.isNullOrEmpty(selectedPath)) {
                throw new Error("selectedPath can not be blank!");
            }
            ;
            let mQueryParameter = new HttpParams().append('action', action);
            mQueryParameter = mQueryParameter.append('selectedPath', selectedPath);
            const mHttpOptions = {
                headers: new HttpHeaders({
                    'Content-Type': 'application/json',
                }),
                params: mQueryParameter,
            };
            this._HttpClient.delete(this._Api_DeleteDirectory, mHttpOptions).subscribe({
                next: (response) => {
                    resolve(response);
                },
                error: (error) => {
                    this._LoggingSvc.errorHandler(error, 'FileManagerService', 'deleteFile');
                    reject(false);
                },
                complete: () => { }
            });
        });
    }
    /**
     * @description Deletes a file from the currently selected path
     *
     * @param {string} action Used to determine the upload directory and enforce security on the server
     * @param {string} fileName The file name to delete
     * @return {*}  {Promise<boolean>}
     * @memberof FileManagerService
     */
    async deleteFile(action, fileName) {
        return new Promise((resolve, reject) => {
            if (this._GWCommon.isNullOrEmpty(action)) {
                throw new Error("action can not be blank!");
            }
            if (this._GWCommon.isNullOrEmpty(fileName)) {
                throw new Error("fileName can not be blank!");
            }
            let mQueryParameter = new HttpParams().append('action', action);
            mQueryParameter = mQueryParameter.append('selectedPath', this._SelectedPath);
            mQueryParameter = mQueryParameter.append('fileName', fileName);
            const mHttpOptions = {
                headers: new HttpHeaders({
                    'Content-Type': 'application/json',
                }),
                params: mQueryParameter,
            };
            this._HttpClient.delete(this._Api_DeleteFile, mHttpOptions).subscribe({
                next: (response) => {
                    resolve(response);
                },
                error: (error) => {
                    this._LoggingSvc.errorHandler(error, 'FileManagerService', 'deleteFile');
                    reject(false);
                },
                complete: () => { }
            });
        });
    }
    /**
     * @description Retrieves an array of IDirectoryTree for the gien path
     *
     * @param {string} action Used to determine the upload directory and enforce security on the server
     * @param {string} path The relative of the directory path
     * @param {string} forControl - The control to notify.
     * @memberof FileManagerService
     * @returns {Promise<boolean>} A promise that resolves to a boolean indicating the success of the operation.
     */
    async getDirectories(action, path, forControl) {
        let mQueryParameter = new HttpParams().append('action', action);
        mQueryParameter = mQueryParameter.append('selectedPath', path);
        const mHttpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            }),
            params: mQueryParameter,
        };
        return new Promise((resolve, reject) => {
            this._HttpClient.get(this._Api_GetDirectories, mHttpOptions).subscribe({
                next: (response) => {
                    const mDirectoryTree = [];
                    mDirectoryTree.push(response);
                    // console.log('getDirectories.mDirectoryTree', mDirectoryTree);
                    this._DataSvc.notifyDataChanged(forControl, mDirectoryTree);
                    resolve(true);
                },
                error: (error) => {
                    this._LoggingSvc.errorHandler(error, 'FunctionService', 'getFunction');
                    reject(false);
                },
                // complete: () => {}
            });
        });
    }
    /**
     * @description Retrives a array of IFileInfoLight for the given path
     *
     * @param {string} action Used to determine the upload directory and enforce security on the server
     * @param {string} controlId The id of the controler the files are for
     * @param {string} selectedPath The relative of the directory path
     * @memberof FileManagerService
     */
    getFiles(action, controlId, selectedPath) {
        let mQueryParameter = new HttpParams().append('action', action);
        mQueryParameter = mQueryParameter.append('selectedPath', selectedPath);
        const mHttpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            }),
            params: mQueryParameter,
        };
        this._SelectedPath = selectedPath;
        this._HttpClient.get(this._Api_GetFiles, mHttpOptions).subscribe({
            next: (response) => {
                this._DataSvc.notifyDataChanged(controlId, response);
            },
            error: (error) => {
                this._LoggingSvc.errorHandler(error, 'FileManagerService', 'getFiles');
            },
            // complete: () => {}
        });
    }
    async getTestNaturalSort(sortDirection) {
        return new Promise((resolve, reject) => {
            const mQueryParameter = new HttpParams().append('sortDirection', sortDirection);
            const mHttpOptions = {
                headers: new HttpHeaders({
                    'Content-Type': 'application/json',
                }),
                params: mQueryParameter,
            };
            this._HttpClient.get(this._Api_GetTestNaturalSort, mHttpOptions).subscribe({
                next: (response) => {
                    resolve(response);
                },
                error: (error) => {
                    this._LoggingSvc.errorHandler(error, 'FileManagerService', 'getTestNaturalSort');
                    reject(false);
                },
                // complete: () => {}
            });
        });
    }
    /**
     * @description Calculates the total number of chuncks needed to upload a large file
     *
     * @param {number} fileSize  The size of the file
     * @param {number} chunkSize The size of the chunck that the server can accept
     * @return {*}  {number}
     * @memberof FileManagerService
     */
    getTotalNumberOfUploads(fileSize, chunkSize) {
        const mRetVal = fileSize % chunkSize == 0 ? fileSize / chunkSize : Math.floor(fileSize / chunkSize) + 1;
        return mRetVal;
    }
    /**
     * @description Uploads files that are too large to be sent in a single upload by using recursion
     * to call the API with a "slice" of the file.
     *
     * @private
     * @param {IMultiPartFileUploadParameters} parameters
     * @memberof FileManagerService
     */
    multiPartFileUpload(parameters) {
        const mParams = { ...parameters }; // it's good practice to leave parameter values unchanged
        let mNextUploadNumber = mParams.uploadNumber + 1;
        // const mFileSize: number = mParams.file.size;
        const mMultiUploadFileName = mParams.file.name + "_UploadNumber_" + mNextUploadNumber;
        if (mParams.uploadNumber < mParams.totalNumberOfUploads) { // do we need to send any more pices of the file
            const mBlob = mParams.file.slice.call(mParams.file, mParams.startingByte, mParams.endingByte);
            const mFormData = new FormData();
            mFormData.append('action', mParams.action);
            mFormData.append('completed', 'false');
            mFormData.append('selectedPath', this._SelectedPath);
            mFormData.append(mMultiUploadFileName, mBlob);
            this._HttpClient.post(this._Api_UploadFile, mFormData).subscribe({
                next: (response) => {
                    const mUploadStatus = new UploadStatus(mParams.action, response.fileName, response.data, false, response.isSuccess, mParams.totalNumberOfUploads, mParams.uploadNumber);
                    mParams.uploadNumber = mNextUploadNumber;
                    mParams.startingByte = parameters.endingByte;
                    mParams.endingByte = parameters.endingByte + parameters.chunkSize;
                    this.uploadStatusChanged.next(mUploadStatus);
                    this.multiPartFileUpload(mParams);
                },
                error: (error) => {
                    if (parameters.retryNumber < 4) {
                        mParams.retryNumber = mParams.retryNumber + 1;
                        this.multiPartFileUpload(mParams);
                    }
                    else {
                        this._LoggingSvc.errorHandler(error, 'FileManagementService', 'upload');
                        // const mUploadStatus: IUploadStatus = new UploadStatus(mParams.action, mMultiUploadFileName, error, false, false, mParams.totalNumberOfUploads, mParams.uploadNumber);
                        mParams.uploadNumber = mNextUploadNumber;
                    }
                },
                // complete: () => {}
            });
        }
        ;
        if (mParams.uploadNumber == mParams.totalNumberOfUploads) { // make sure this is the last upload
            this.multiUploadComplete(mParams.action, mParams.file.name, this._Api_UploadFile).subscribe({
                next: (response) => {
                    const mUploadStatus = new UploadStatus(mParams.action, response.fileName, response.data, true, response.isSuccess, mParams.totalNumberOfUploads, mParams.uploadNumber);
                    this.uploadStatusChanged.next(mUploadStatus);
                },
                error: (error) => {
                    this._LoggingSvc.errorHandler(error, 'FileManagementService', 'upload');
                    const mUploadStatus = new UploadStatus(mParams.action, mParams.file.name, error, true, false, mParams.totalNumberOfUploads, mParams.uploadNumber);
                    this.uploadStatusChanged.next(mUploadStatus);
                },
                // complete: () => {}
            });
        }
    }
    /**
     * @description Helper method that calls the upload API so the merging of the file chunks (slices)
     * can be done
     *
     * @private
     * @param {string} action Used to determine the upload directory and enforce security on the server.
     * @param {string} fileName The file name for this Chunk.
     * @param {string} uri The Uniform Resource Identifier.
     * @return {*}  {Observable<IUploadResponse>}
     * @memberof FileManagerService
     */
    multiUploadComplete(action, fileName, uri) {
        var mFormData = new FormData();
        mFormData.append('action', action);
        mFormData.append('completed', 'true');
        mFormData.append('fileName', fileName);
        mFormData.append('selectedPath', this._SelectedPath);
        return this._HttpClient.post(uri, mFormData);
    }
    /**
     * @description Refreshes the specified action.
     *
     * @param {string} action - Used to determine the upload directory and enforce security on the server.
     * @param {IDirectoryTree} directoryTree - Optional directory tree to use to determine the selected path.
     * @return {Promise<any>} A Promise that resolves when the refresh is complete.
     * @memberof FileManagerService
     */
    async refresh(action, directoryTree) {
        const mAction = action.trim();
        let mSelectedNode = this._CurrentDirectoryTree;
        const mDirectoryControlName = mAction + '_Directories';
        const mFileControleName = mAction + '_Files';
        if (directoryTree != undefined) {
            mSelectedNode = directoryTree;
        }
        this.getDirectories(mAction, mDirectoryControlName, mSelectedNode.relitivePath).catch((error) => {
            this._LoggingSvc.errorHandler(error, 'FileManagerService', 'refresh/getDirectories');
        }).then((response) => {
            this.selectedDirectoryChanged.next(mSelectedNode);
            this._SelectedPath = mSelectedNode.relitivePath;
            this.getFiles(mAction, mFileControleName, this._SelectedPath);
        });
    }
    /**
     * @description Renames a directory asynchronously.
     *
     * @param {string} action - Used to determine the upload directory and enforce security on the server.
     * @param {string} newName - The new name for the directory.
     * @return {Promise<boolean>} A promise that resolves to true if the directory was renamed successfully, or false otherwise.
     * @memberof FileManagerService
     */
    async renameDirectory(action, newName, directoryControlName) {
        /**
         * 1.) Get the current directory
         * 2.) Calculate the new directory to select when rename is successful
         * 3.) Call FileManagerService to rename the directory
         * 4.) If successful, set the selected directory to the calculated new directory in step 2
         * 5.) Refresh the directory tree and file list
         * 6.) Close the modal
         *
         * 4.) If unsuccessful, log the error
         * 5.) Notify the client
         * 6.) Close the modal
         *
         *
         */
        if (this._GWCommon.isNullOrEmpty(action)) {
            throw new Error("action can not be blank!");
        }
        if (this._GWCommon.isNullOrEmpty(newName)) {
            throw new Error("newName can not be blank!");
        }
        const mDirectoryParts = this.SelectedPath.split('\\');
        mDirectoryParts[mDirectoryParts.length - 1] = newName;
        const mNewSelectedPath = mDirectoryParts.join('\\');
        const mCurrentDirectoryTree = this.CurrentDirectoryTree;
        mCurrentDirectoryTree.key = newName.replace(' ', '').toLowerCase();
        mCurrentDirectoryTree.name = newName;
        mCurrentDirectoryTree.relitivePath = mNewSelectedPath;
        return new Promise((resolve, reject) => {
            let mQueryParameter = new HttpParams().append('action', action);
            mQueryParameter = mQueryParameter.append('selectedPath', this._SelectedPath);
            mQueryParameter = mQueryParameter.append('newName', newName);
            const mHttpOptions = {
                headers: new HttpHeaders({
                    'Content-Type': 'application/json',
                }),
                params: mQueryParameter,
            };
            this._HttpClient.post(this._Api_RenameDirectory, null, mHttpOptions).subscribe({
                next: (response) => {
                    this.getDirectories(action, mCurrentDirectoryTree.relitivePath, directoryControlName).catch((error) => {
                        this._LoggingSvc.errorHandler(error, 'FileManagerService', 'renameDirectory/getDirectories');
                        reject(false);
                    }).then((_) => {
                        // console.log('mCurrentDirectoryTree', mCurrentDirectoryTree);
                        this.setSelectedDirectory(mCurrentDirectoryTree);
                        resolve(response);
                    });
                },
                error: (error) => {
                    this._LoggingSvc.errorHandler(error, 'FileManagerService', 'renameDirectory');
                    reject(false);
                },
                complete: () => { }
            });
        });
    }
    /**
     * @description Renames an existing file
     *
     * @param {string} action Used to determine the upload directory and enforce security on the server
     * @param {string} oldName The name of the file to rename
     * @param {string} newName The new name of the file
     * @return {*}  {Promise<boolean>}
     * @memberof FileManagerService
     */
    async renameFile(action, oldName, newName) {
        if (this._GWCommon.isNullOrEmpty(action)) {
            throw new Error("action can not be blank!");
        }
        if (this._GWCommon.isNullOrEmpty(oldName)) {
            throw new Error("oldName can not be blank!");
        }
        if (this._GWCommon.isNullOrEmpty(newName)) {
            throw new Error("newName can not be blank!");
        }
        return new Promise((resolve, reject) => {
            let mQueryParameter = new HttpParams().append('action', action);
            mQueryParameter = mQueryParameter.append('selectedPath', this._SelectedPath);
            mQueryParameter = mQueryParameter.append('oldName', oldName);
            mQueryParameter = mQueryParameter.append('newName', newName);
            const mHttpOptions = {
                headers: new HttpHeaders({
                    'Content-Type': 'application/json',
                }),
                params: mQueryParameter,
            };
            this._HttpClient.post(this._Api_RenameFile, null, mHttpOptions).subscribe({
                next: (response) => {
                    resolve(response);
                },
                error: (error) => {
                    this._LoggingSvc.errorHandler(error, 'FileManagerService', 'renameFile');
                    reject(false);
                },
                complete: () => { }
            });
        });
    }
    /**
     * @description Performs a single file upload
     *
     * @private
     * @param {File} file the HTML "file" object
     * @param {string} action Used to determine the upload directory and enforce security on the server
     * @memberof FileManagerService
     */
    singleFileUpload(file, action) {
        const mFormData = new FormData();
        mFormData.append('action', action);
        mFormData.append('selectedPath', this._SelectedPath);
        mFormData.append(file.name + '_UploadNumber_1', file);
        mFormData.append('fileName', file.name);
        mFormData.append('completed', 'true');
        this._HttpClient.post(this._Api_UploadFile, mFormData).subscribe({
            next: (response) => {
                const mUploadStatus = new UploadStatus(action, response.fileName, response.data, true, response.isSuccess, 1, 1);
                this.uploadStatusChanged.next(mUploadStatus);
            },
            error: (error) => {
                this._LoggingSvc.errorHandler(error, 'FileManagerService', 'singleFileUpload');
                // const mUploadStatus: IUploadStatus = new UploadStatus(action, file.name, error, true, false, 1, 1);
            },
            // complete: () => {}
        });
    }
    /**
     * @description Sets the selected directory and triggers the selectedDirectoryChanged event.
     *
     * @param {IDirectoryTree} directoryTree - The directory tree used to set as the selected directory.
     * @return {void} This function does not return anything.
     * @memberof FileManagerService
     */
    setSelectedDirectory(directoryTree) {
        // console.log('directoryTree.setSelectedDirectory', directoryTree);
        this._CurrentDirectoryTree = directoryTree;
        this.selectedDirectoryChanged.next(directoryTree);
    }
    /**
     * @description Uploads file by calling either multiPartFileUpload or singleFileUpload depending on the file size.
     *
     * @param {string} action Used to determine the upload directory and enforce security on the server
     * @param {File} file An HTML "File" object
     * @param {number} [chunkSize=3072000] Used to break the "File" up if the file size is greater than the chunkSize.  The number is in direct relation to KestrelServerLimits.MaxRequestBodySize Property
     * @memberof FileManagerService
     */
    uploadFile(action, file, chunkSize = 29696000) {
        const mTotalNumberOfUploads = this.getTotalNumberOfUploads(file.size, chunkSize);
        if (mTotalNumberOfUploads > 1) {
            const mMultiPartFileUpload = new MultiPartFileUploadParameters(action, file, mTotalNumberOfUploads, chunkSize);
            this.multiPartFileUpload(mMultiPartFileUpload);
        }
        else {
            this.singleFileUpload(file, action);
        }
    }
    static { this.ɵfac = i0.ɵɵngDeclareFactory({ minVersion: "12.0.0", version: "16.2.0", ngImport: i0, type: FileManagerService, deps: [{ token: i1.DataService }, { token: i2.GWCommon }, { token: i3.HttpClient }, { token: i4.LoggingService }], target: i0.ɵɵFactoryTarget.Injectable }); }
    static { this.ɵprov = i0.ɵɵngDeclareInjectable({ minVersion: "12.0.0", version: "16.2.0", ngImport: i0, type: FileManagerService, providedIn: 'root' }); }
}
i0.ɵɵngDeclareClassMetadata({ minVersion: "12.0.0", version: "16.2.0", ngImport: i0, type: FileManagerService, decorators: [{
            type: Injectable,
            args: [{
                    providedIn: 'root'
                }]
        }], ctorParameters: function () { return [{ type: i1.DataService }, { type: i2.GWCommon }, { type: i3.HttpClient }, { type: i4.LoggingService }]; } });
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmlsZS1tYW5hZ2VyLnNlcnZpY2UuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyJmaWxlLW1hbmFnZXIuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBYyxXQUFXLEVBQUUsVUFBVSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDM0UsT0FBTyxFQUFjLE9BQU8sRUFBRSxNQUFNLE1BQU0sQ0FBQztBQVEzQyxPQUFPLEVBQWtDLDZCQUE2QixFQUFFLE1BQU0sMkNBQTJDLENBQUM7QUFFMUgsT0FBTyxFQUFpQixZQUFZLEVBQUUsTUFBTSx1QkFBdUIsQ0FBQzs7Ozs7O0FBS3BFLE1BQU0sT0FBTyxrQkFBa0I7SUFjN0IsSUFBVyxvQkFBb0IsQ0FBQyxLQUFxQjtRQUNuRCxJQUFJLENBQUMscUJBQXFCLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUM7SUFDakUsQ0FBQztJQUVELElBQVcsb0JBQW9CO1FBQzdCLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLElBQUksQ0FBQyxxQkFBcUIsQ0FBQyxDQUFDLENBQUM7SUFDaEUsQ0FBQztJQUVELElBQVcsWUFBWTtRQUNyQixPQUFPLElBQUksQ0FBQyxhQUFhLENBQUM7SUFDNUIsQ0FBQztJQUFBLENBQUM7SUFRRixZQUNVLFFBQXFCLEVBQ3JCLFNBQW1CLEVBQ25CLFdBQXVCLEVBQ3ZCLFdBQTJCO1FBSDNCLGFBQVEsR0FBUixRQUFRLENBQWE7UUFDckIsY0FBUyxHQUFULFNBQVMsQ0FBVTtRQUNuQixnQkFBVyxHQUFYLFdBQVcsQ0FBWTtRQUN2QixnQkFBVyxHQUFYLFdBQVcsQ0FBZ0I7UUFuQzdCLFNBQUksR0FBVyxFQUFFLENBQUM7UUFDbEIsd0JBQW1CLEdBQVcsRUFBRSxDQUFDO1FBQ2pD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QUF1QixDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUUsU0FBUyxDQUFDLENBQUM7UUFDekYsSUFBSSxxQkFBcUIsR0FBRyxDQUFDLEVBQUU7WUFDN0IsTUFBTSxvQkFBb0IsR0FBbUMsSUFBSSw2QkFBNkIsQ0FDNUYsTUFBTSxFQUNOLElBQUksRUFDSixxQkFBcUIsRUFDckIsU0FBUyxDQUNWLENBQUE7WUFDRCxJQUFJLENBQUMsbUJBQW1CLENBQUMsb0JBQW9CLENBQUMsQ0FBQztTQUNoRDthQUFLO1lBQ0osSUFBSSxDQUFDLGdCQUFnQixDQUFDLElBQUksRUFBRSxNQUFNLENBQUMsQ0FBQztTQUNyQztJQUNILENBQUM7OEdBN2hCVSxrQkFBa0I7a0hBQWxCLGtCQUFrQixjQUZqQixNQUFNOzsyRkFFUCxrQkFBa0I7a0JBSDlCLFVBQVU7bUJBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBIdHRwQ2xpZW50LCBIdHRwSGVhZGVycywgSHR0cFBhcmFtcyB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbi9odHRwJztcclxuaW1wb3J0IHsgT2JzZXJ2YWJsZSwgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xyXG4vLyBMaWJyYXJ5XHJcbmltcG9ydCB7IERhdGFTZXJ2aWNlIH0gZnJvbSAnQEdyb3d0aHdhcmUvc2hhcmVkL3NlcnZpY2VzJztcclxuaW1wb3J0IHsgR1dDb21tb24gfSBmcm9tICdAR3Jvd3Rod2FyZS9jb21tb24tY29kZSc7XHJcbmltcG9ydCB7IExvZ2dpbmdTZXJ2aWNlIH0gZnJvbSAnQEdyb3d0aHdhcmUvZmVhdHVyZXMvbG9nZ2luZyc7XHJcbi8vIEZlYXR1cmVcclxuaW1wb3J0IHsgSURpcmVjdG9yeVRyZWUgfSBmcm9tICcuL2RpcmVjdG9yeS10cmVlLm1vZGVsJztcclxuaW1wb3J0IHsgSUZpbGVJbmZvTGlnaHQgfSBmcm9tICcuL2ZpbGUtaW5mby1saWdodC5tb2RlbCc7XHJcbmltcG9ydCB7IElNdWx0aVBhcnRGaWxlVXBsb2FkUGFyYW1ldGVycywgTXVsdGlQYXJ0RmlsZVVwbG9hZFBhcmFtZXRlcnMgfSBmcm9tICcuL211bHRpLXBhcnQtZmlsZS11cGxvYWQtcGFyYW1ldGVycy5tb2RlbCc7XHJcbmltcG9ydCB7IElVcGxvYWRSZXNwb25zZSB9IGZyb20gJy4vdXBsb2FkLXJlc3BvbnNlLm1vZGVsJztcclxuaW1wb3J0IHsgSVVwbG9hZFN0YXR1cywgVXBsb2FkU3RhdHVzIH0gZnJvbSAnLi91cGxvYWQtc3RhdHVzLm1vZGVsJztcclxuXHJcbkBJbmplY3RhYmxlKHtcclxuICBwcm92aWRlZEluOiAncm9vdCdcclxufSlcclxuZXhwb3J0IGNsYXNzIEZpbGVNYW5hZ2VyU2VydmljZSB7XHJcbiAgcHJpdmF0ZSBfQXBpOiBzdHJpbmcgPSAnJztcclxuICBwcml2YXRlIF9BcGlfR2V0RGlyZWN0b3JpZXM6IHN0cmluZyA9ICcnO1xyXG4gIHByaXZhdGUgX0FwaV9HZXRGaWxlczogc3RyaW5nID0gJyc7XHJcbiAgcHJpdmF0ZSBfQXBpX0NyZWF0ZURpcmVjdG9yeTogc3RyaW5nID0gJyc7XHJcbiAgcHJpdmF0ZSBfQXBpX0RlbGV0ZURpcmVjdG9yeTogc3RyaW5nID0gJyc7XHJcbiAgcHJpdmF0ZSBfQXBpX0RlbGV0ZUZpbGU6IHN0cmluZyA9ICcnO1xyXG4gIHByaXZhdGUgX0FwaV9SZW5hbWVEaXJlY3Rvcnk6IHN0cmluZyA9ICcnO1xyXG4gIHByaXZhdGUgX0FwaV9SZW5hbWVGaWxlOiBzdHJpbmcgPSAnJztcclxuICBwcml2YXRlIF9BcGlfR2V0VGVzdE5hdHVyYWxTb3J0OiBzdHJpbmcgPSAnJztcclxuICBwcml2YXRlIF9BcGlfVXBsb2FkRmlsZTogc3RyaW5nID0gJyc7XHJcbiAgcHJpdmF0ZSBfQ3VycmVudERpcmVjdG9yeVRyZWUhOiBJRGlyZWN0b3J5VHJlZTtcclxuICBwcml2YXRlIF9TZWxlY3RlZFBhdGg6IHN0cmluZyA9ICdcXFxcJztcclxuXHJcbiAgcHVibGljIHNldCBDdXJyZW50RGlyZWN0b3J5VHJlZSh2YWx1ZTogSURpcmVjdG9yeVRyZWUpIHtcclxuICAgIHRoaXMuX0N1cnJlbnREaXJlY3RvcnlUcmVlID0gSlNPTi5wYXJzZShKU09OLnN0cmluZ2lmeSh2YWx1ZSkpO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIGdldCBDdXJyZW50RGlyZWN0b3J5VHJlZSgpOiBJRGlyZWN0b3J5VHJlZSB7XHJcbiAgICByZXR1cm4gSlNPTi5wYXJzZShKU09OLnN0cmluZ2lmeSh0aGlzLl9DdXJyZW50RGlyZWN0b3J5VHJlZSkpO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIGdldCBTZWxlY3RlZFBhdGgoKTogc3RyaW5nIHtcclxuICAgIHJldHVybiB0aGlzLl9TZWxlY3RlZFBhdGg7XHJcbiAgfTtcclxuXHJcbiAgTW9kYWxJZF9SZW5hbWVfRGlyZWN0b3J5OiBzdHJpbmcgPSAnRGlyZWN0b3J5VHJlZUNvbXBvbmVudC5vbk1lbnVSZW5hbWVDbGljayc7XHJcbiAgTW9kYWxJZF9DcmVhdGVEaXJlY3Rvcnk6IHN0cmluZyA9IFwiQ3JlYXRlRGlyZWN0b3J5Rm9ybVwiO1xyXG5cclxuICB1cGxvYWRTdGF0dXNDaGFuZ2VkOiAgU3ViamVjdDxJVXBsb2FkU3RhdHVzPiA9IG5ldyBTdWJqZWN0PElVcGxvYWRTdGF0dXM+KCk7XHJcbiAgc2VsZWN0ZWREaXJlY3RvcnlDaGFuZ2VkOiAgU3ViamVjdDxJRGlyZWN0b3J5VHJlZT4gPSBuZXcgU3ViamVjdDxJRGlyZWN0b3J5VHJlZT4oKTtcclxuXHJcbiAgY29uc3RydWN0b3IoXHJcbiAgICBwcml2YXRlIF9EYXRhU3ZjOiBEYXRhU2VydmljZSxcclxuICAgIHByaXZhdGUgX0dXQ29tbW9uOiBHV0NvbW1vbixcclxuICAgIHByaXZhdGUgX0h0dHBDbGllbnQ6IEh0dHBDbGllbnQsXHJcbiAgICBwcml2YXRlIF9Mb2dnaW5nU3ZjOiBMb2dnaW5nU2VydmljZSwgICAgXHJcbiAgKSB7IFxyXG4gICAgdGhpcy5fQXBpID0gdGhpcy5fR1dDb21tb24uYmFzZVVSTCArICdHcm93dGh3YXJlRmlsZS8nO1xyXG4gICAgdGhpcy5fQXBpX0dldERpcmVjdG9yaWVzID0gdGhpcy5fQXBpICsgJ0dldERpcmVjdG9yaWVzJztcclxuICAgIHRoaXMuX0FwaV9HZXRGaWxlcyA9IHRoaXMuX0FwaSArICdHZXRGaWxlcyc7XHJcbiAgICB0aGlzLl9BcGlfQ3JlYXRlRGlyZWN0b3J5ID0gdGhpcy5fQXBpICsgJ0NyZWF0ZURpcmVjdG9yeSc7XHJcbiAgICB0aGlzLl9BcGlfRGVsZXRlRGlyZWN0b3J5ID0gdGhpcy5fQXBpICsgJ0RlbGV0ZURpcmVjdG9yeSc7XHJcbiAgICB0aGlzLl9BcGlfRGVsZXRlRmlsZSA9IHRoaXMuX0FwaSArICdEZWxldGVGaWxlJztcclxuICAgIHRoaXMuX0FwaV9SZW5hbWVEaXJlY3RvcnkgPSB0aGlzLl9BcGkgKyAnUmVuYW1lRGlyZWN0b3J5JztcclxuICAgIHRoaXMuX0FwaV9SZW5hbWVGaWxlID0gdGhpcy5fQXBpICsgJ1JlbmFtZUZpbGUnO1xyXG4gICAgdGhpcy5fQXBpX0dldFRlc3ROYXR1cmFsU29ydCA9IHRoaXMuX0FwaSArICdHZXRUZXN0TmF0dXJhbFNvcnQnO1xyXG4gICAgdGhpcy5fQXBpX1VwbG9hZEZpbGUgPSB0aGlzLl9BcGkgKyAnVXBsb2FkRmlsZSc7XHJcbiAgfVxyXG5cclxuICAvKipcclxuICAgKiBDcmVhdGVzIGEgbmV3IGRpcmVjdG9yeSBpbiB0aGUgY3VycmVjdGx5IHNlbGVjdGVkIGRpcmVjdG9yeVxyXG4gICAqXHJcbiAgICogQHBhcmFtIHtzdHJpbmd9IGFjdGlvblxyXG4gICAqIEBwYXJhbSB7c3RyaW5nfSBzZWxlY3RlZFBhdGhcclxuICAgKiBAcGFyYW0ge3N0cmluZ30gbmV3UGF0aFxyXG4gICAqIEByZXR1cm4geyp9ICB7UHJvbWlzZTxhbnk+fVxyXG4gICAqIEBtZW1iZXJvZiBGaWxlTWFuYWdlclNlcnZpY2VcclxuICAgKi9cclxuICBhc3luYyBjcmVhdGVEaXJlY3RvcnkoYWN0aW9uOiBzdHJpbmcsIG5ld1BhdGg6IHN0cmluZyk6IFByb21pc2U8YW55PlxyXG4gIHtcclxuICAgIGlmKHRoaXMuX0dXQ29tbW9uLmlzTnVsbE9yRW1wdHkoYWN0aW9uKSkge1xyXG4gICAgICB0aHJvdyBuZXcgRXJyb3IoXCJhY3Rpb24gY2FuIG5vdCBiZSBibGFuayFcIik7XHJcbiAgICB9XHJcbiAgICBpZih0aGlzLl9HV0NvbW1vbi5pc051bGxPckVtcHR5KG5ld1BhdGgpKSB7XHJcbiAgICAgIHRocm93IG5ldyBFcnJvcihcIm5ld1BhdGggY2FuIG5vdCBiZSBibGFuayFcIik7XHJcbiAgICB9O1xyXG4gICAgbGV0IG1RdWVyeVBhcmFtZXRlcjogSHR0cFBhcmFtcyA9IG5ldyBIdHRwUGFyYW1zKCkuYXBwZW5kKCdhY3Rpb24nLCBhY3Rpb24pO1xyXG4gICAgbVF1ZXJ5UGFyYW1ldGVyID0gbVF1ZXJ5UGFyYW1ldGVyLmFwcGVuZCgnc2VsZWN0ZWRQYXRoJywgdGhpcy5TZWxlY3RlZFBhdGgpO1xyXG4gICAgbVF1ZXJ5UGFyYW1ldGVyID0gbVF1ZXJ5UGFyYW1ldGVyLmFwcGVuZCgnbmV3UGF0aCcsIG5ld1BhdGgpO1xyXG4gICAgY29uc3QgbUh0dHBPcHRpb25zID0ge1xyXG4gICAgICBoZWFkZXJzOiBuZXcgSHR0cEhlYWRlcnMoe1xyXG4gICAgICAgICdDb250ZW50LVR5cGUnOiAnYXBwbGljYXRpb24vanNvbicsXHJcbiAgICAgIH0pLFxyXG4gICAgICBwYXJhbXM6IG1RdWVyeVBhcmFtZXRlcixcclxuICAgIH07XHJcbiAgICByZXR1cm4gbmV3IFByb21pc2U8Ym9vbGVhbj4oKHJlc29sdmUsIHJlamVjdCkgPT4ge1xyXG4gICAgICB0aGlzLl9IdHRwQ2xpZW50LnBvc3Q8YW55Pih0aGlzLl9BcGlfQ3JlYXRlRGlyZWN0b3J5LCBudWxsLCBtSHR0cE9wdGlvbnMpLnN1YnNjcmliZSh7XHJcbiAgICAgICAgbmV4dDooIHJlc3BvbnNlOiBib29sZWFuICkgPT4ge1xyXG4gICAgICAgICAgcmVzb2x2ZShyZXNwb25zZSlcclxuICAgICAgICB9LFxyXG4gICAgICAgIGVycm9yOiggZXJyb3I6IGFueSApID0+IHtcclxuICAgICAgICAgIHRoaXMuX0xvZ2dpbmdTdmMuZXJyb3JIYW5kbGVyKGVycm9yLCAnRmlsZU1hbmFnZXJTZXJ2aWNlJywgJ2NyZWF0ZURpcmVjdG9yeScpO1xyXG4gICAgICAgICAgcmVqZWN0KGZhbHNlKTtcclxuICAgICAgICB9LFxyXG4gICAgICAgIGNvbXBsZXRlOiggKSA9PiB7fVxyXG4gICAgICB9KTtcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgLyoqXHJcbiAgICogRGVsZXRlcyBhIGRpcmVjdG9yeSwgc3ViZGlyZWN0b3J5IGFuZCBhbGwgZmlsZXNcclxuICAgKlxyXG4gICAqIEBwYXJhbSB7c3RyaW5nfSBhY3Rpb25cclxuICAgKiBAcGFyYW0ge3N0cmluZ30gc2VsZWN0ZWRQYXRoXHJcbiAgICogQHJldHVybiB7Kn0gIHtQcm9taXNlPGJvb2xlYW4+fVxyXG4gICAqIEBtZW1iZXJvZiBGaWxlTWFuYWdlclNlcnZpY2VcclxuICAgKi9cclxuICBhc3luYyBkZWxldGVEaXJlY3RvcnkoYWN0aW9uOiBzdHJpbmcsIHNlbGVjdGVkUGF0aDogc3RyaW5nKTogUHJvbWlzZTxib29sZWFuPiB7XHJcbiAgICByZXR1cm4gbmV3IFByb21pc2U8Ym9vbGVhbj4oKHJlc29sdmUsIHJlamVjdCkgPT4ge1xyXG4gICAgICBpZih0aGlzLl9HV0NvbW1vbi5pc051bGxPckVtcHR5KGFjdGlvbikpIHtcclxuICAgICAgICB0aHJvdyBuZXcgRXJyb3IoXCJhY3Rpb24gY2FuIG5vdCBiZSBibGFuayFcIik7XHJcbiAgICAgIH1cclxuICAgICAgaWYodGhpcy5fR1dDb21tb24uaXNOdWxsT3JFbXB0eShzZWxlY3RlZFBhdGgpKSB7XHJcbiAgICAgICAgdGhyb3cgbmV3IEVycm9yKFwic2VsZWN0ZWRQYXRoIGNhbiBub3QgYmUgYmxhbmshXCIpO1xyXG4gICAgICB9O1xyXG4gICAgICBsZXQgbVF1ZXJ5UGFyYW1ldGVyOiBIdHRwUGFyYW1zID0gbmV3IEh0dHBQYXJhbXMoKS5hcHBlbmQoJ2FjdGlvbicsIGFjdGlvbik7XHJcbiAgICAgIG1RdWVyeVBhcmFtZXRlcj1tUXVlcnlQYXJhbWV0ZXIuYXBwZW5kKCdzZWxlY3RlZFBhdGgnLCBzZWxlY3RlZFBhdGgpO1xyXG4gICAgICBjb25zdCBtSHR0cE9wdGlvbnMgPSB7XHJcbiAgICAgICAgaGVhZGVyczogbmV3IEh0dHBIZWFkZXJzKHtcclxuICAgICAgICAgICdDb250ZW50LVR5cGUnOiAnYXBwbGljYXRpb24vanNvbicsXHJcbiAgICAgICAgfSksXHJcbiAgICAgICAgcGFyYW1zOiBtUXVlcnlQYXJhbWV0ZXIsXHJcbiAgICAgIH07XHJcbiAgICAgIHRoaXMuX0h0dHBDbGllbnQuZGVsZXRlPGJvb2xlYW4+KHRoaXMuX0FwaV9EZWxldGVEaXJlY3RvcnksIG1IdHRwT3B0aW9ucykuc3Vic2NyaWJlKHtcclxuICAgICAgICBuZXh0OiggcmVzcG9uc2U6IGJvb2xlYW4gKSA9PiB7XHJcbiAgICAgICAgICByZXNvbHZlKHJlc3BvbnNlKVxyXG4gICAgICAgIH0sXHJcbiAgICAgICAgZXJyb3I6KCBlcnJvcjogYW55ICkgPT4ge1xyXG4gICAgICAgICAgdGhpcy5fTG9nZ2luZ1N2Yy5lcnJvckhhbmRsZXIoZXJyb3IsICdGaWxlTWFuYWdlclNlcnZpY2UnLCAnZGVsZXRlRmlsZScpO1xyXG4gICAgICAgICAgcmVqZWN0KGZhbHNlKTtcclxuICAgICAgICB9LFxyXG4gICAgICAgIGNvbXBsZXRlOiggKSA9PiB7fVxyXG4gICAgICB9KTtcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgLyoqXHJcbiAgICogQGRlc2NyaXB0aW9uIERlbGV0ZXMgYSBmaWxlIGZyb20gdGhlIGN1cnJlbnRseSBzZWxlY3RlZCBwYXRoXHJcbiAgICpcclxuICAgKiBAcGFyYW0ge3N0cmluZ30gYWN0aW9uIFVzZWQgdG8gZGV0ZXJtaW5lIHRoZSB1cGxvYWQgZGlyZWN0b3J5IGFuZCBlbmZvcmNlIHNlY3VyaXR5IG9uIHRoZSBzZXJ2ZXJcclxuICAgKiBAcGFyYW0ge3N0cmluZ30gZmlsZU5hbWUgVGhlIGZpbGUgbmFtZSB0byBkZWxldGVcclxuICAgKiBAcmV0dXJuIHsqfSAge1Byb21pc2U8Ym9vbGVhbj59XHJcbiAgICogQG1lbWJlcm9mIEZpbGVNYW5hZ2VyU2VydmljZVxyXG4gICAqL1xyXG4gIGFzeW5jIGRlbGV0ZUZpbGUoYWN0aW9uOiBzdHJpbmcsIGZpbGVOYW1lOiBzdHJpbmcpOiBQcm9taXNlPGJvb2xlYW4+IHtcclxuICAgIHJldHVybiBuZXcgUHJvbWlzZTxib29sZWFuPigocmVzb2x2ZSwgcmVqZWN0KSA9PiB7XHJcbiAgICAgIGlmKHRoaXMuX0dXQ29tbW9uLmlzTnVsbE9yRW1wdHkoYWN0aW9uKSkge1xyXG4gICAgICAgIHRocm93IG5ldyBFcnJvcihcImFjdGlvbiBjYW4gbm90IGJlIGJsYW5rIVwiKTtcclxuICAgICAgfVxyXG4gICAgICBpZih0aGlzLl9HV0NvbW1vbi5pc051bGxPckVtcHR5KGZpbGVOYW1lKSkge1xyXG4gICAgICAgIHRocm93IG5ldyBFcnJvcihcImZpbGVOYW1lIGNhbiBub3QgYmUgYmxhbmshXCIpO1xyXG4gICAgICB9XHJcbiAgICAgIGxldCBtUXVlcnlQYXJhbWV0ZXI6IEh0dHBQYXJhbXMgPSBuZXcgSHR0cFBhcmFtcygpLmFwcGVuZCgnYWN0aW9uJywgYWN0aW9uKTtcclxuICAgICAgbVF1ZXJ5UGFyYW1ldGVyPW1RdWVyeVBhcmFtZXRlci5hcHBlbmQoJ3NlbGVjdGVkUGF0aCcsIHRoaXMuX1NlbGVjdGVkUGF0aCk7XHJcbiAgICAgIG1RdWVyeVBhcmFtZXRlcj1tUXVlcnlQYXJhbWV0ZXIuYXBwZW5kKCdmaWxlTmFtZScsIGZpbGVOYW1lKTtcclxuICAgICAgY29uc3QgbUh0dHBPcHRpb25zID0ge1xyXG4gICAgICAgIGhlYWRlcnM6IG5ldyBIdHRwSGVhZGVycyh7XHJcbiAgICAgICAgICAnQ29udGVudC1UeXBlJzogJ2FwcGxpY2F0aW9uL2pzb24nLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICAgIHBhcmFtczogbVF1ZXJ5UGFyYW1ldGVyLFxyXG4gICAgICB9O1xyXG4gICAgICB0aGlzLl9IdHRwQ2xpZW50LmRlbGV0ZTxib29sZWFuPih0aGlzLl9BcGlfRGVsZXRlRmlsZSwgbUh0dHBPcHRpb25zKS5zdWJzY3JpYmUoe1xyXG4gICAgICAgIG5leHQ6KCByZXNwb25zZTogYm9vbGVhbiApID0+IHtcclxuICAgICAgICAgIHJlc29sdmUocmVzcG9uc2UpXHJcbiAgICAgICAgfSxcclxuICAgICAgICBlcnJvcjooIGVycm9yOiBhbnkgKSA9PiB7XHJcbiAgICAgICAgICB0aGlzLl9Mb2dnaW5nU3ZjLmVycm9ySGFuZGxlcihlcnJvciwgJ0ZpbGVNYW5hZ2VyU2VydmljZScsICdkZWxldGVGaWxlJyk7XHJcbiAgICAgICAgICByZWplY3QoZmFsc2UpO1xyXG4gICAgICAgIH0sXHJcbiAgICAgICAgY29tcGxldGU6KCApID0+IHt9XHJcbiAgICAgIH0pO1xyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICAvKipcclxuICAgKiBAZGVzY3JpcHRpb24gUmV0cmlldmVzIGFuIGFycmF5IG9mIElEaXJlY3RvcnlUcmVlIGZvciB0aGUgZ2llbiBwYXRoXHJcbiAgICogIFxyXG4gICAqIEBwYXJhbSB7c3RyaW5nfSBhY3Rpb24gVXNlZCB0byBkZXRlcm1pbmUgdGhlIHVwbG9hZCBkaXJlY3RvcnkgYW5kIGVuZm9yY2Ugc2VjdXJpdHkgb24gdGhlIHNlcnZlclxyXG4gICAqIEBwYXJhbSB7c3RyaW5nfSBwYXRoIFRoZSByZWxhdGl2ZSBvZiB0aGUgZGlyZWN0b3J5IHBhdGggXHJcbiAgICogQHBhcmFtIHtzdHJpbmd9IGZvckNvbnRyb2wgLSBUaGUgY29udHJvbCB0byBub3RpZnkuXHJcbiAgICogQG1lbWJlcm9mIEZpbGVNYW5hZ2VyU2VydmljZVxyXG4gICAqIEByZXR1cm5zIHtQcm9taXNlPGJvb2xlYW4+fSBBIHByb21pc2UgdGhhdCByZXNvbHZlcyB0byBhIGJvb2xlYW4gaW5kaWNhdGluZyB0aGUgc3VjY2VzcyBvZiB0aGUgb3BlcmF0aW9uLlxyXG4gICAqL1xyXG4gIHB1YmxpYyBhc3luYyBnZXREaXJlY3RvcmllcyhhY3Rpb246IHN0cmluZywgcGF0aDogc3RyaW5nLCBmb3JDb250cm9sOiBzdHJpbmcpOiBQcm9taXNlPGJvb2xlYW4+IHtcclxuICAgIGxldCBtUXVlcnlQYXJhbWV0ZXI6IEh0dHBQYXJhbXMgPSBuZXcgSHR0cFBhcmFtcygpLmFwcGVuZCgnYWN0aW9uJywgYWN0aW9uKTtcclxuICAgIG1RdWVyeVBhcmFtZXRlciA9IG1RdWVyeVBhcmFtZXRlci5hcHBlbmQoJ3NlbGVjdGVkUGF0aCcsIHBhdGgpO1xyXG4gICAgY29uc3QgbUh0dHBPcHRpb25zID0ge1xyXG4gICAgICBoZWFkZXJzOiBuZXcgSHR0cEhlYWRlcnMoe1xyXG4gICAgICAgICdDb250ZW50LVR5cGUnOiAnYXBwbGljYXRpb24vanNvbicsXHJcbiAgICAgIH0pLFxyXG4gICAgICBwYXJhbXM6IG1RdWVyeVBhcmFtZXRlcixcclxuICAgIH07XHJcbiAgICByZXR1cm4gbmV3IFByb21pc2U8Ym9vbGVhbj4oKHJlc29sdmUsIHJlamVjdCkgPT4ge1xyXG4gICAgICB0aGlzLl9IdHRwQ2xpZW50LmdldDxJRGlyZWN0b3J5VHJlZT4odGhpcy5fQXBpX0dldERpcmVjdG9yaWVzLCBtSHR0cE9wdGlvbnMpLnN1YnNjcmliZSh7XHJcbiAgICAgICAgbmV4dDogKHJlc3BvbnNlOiBJRGlyZWN0b3J5VHJlZSkgPT4ge1xyXG4gICAgICAgICAgY29uc3QgbURpcmVjdG9yeVRyZWUgPSBbXTtcclxuICAgICAgICAgIG1EaXJlY3RvcnlUcmVlLnB1c2gocmVzcG9uc2UpO1xyXG4gICAgICAgICAgLy8gY29uc29sZS5sb2coJ2dldERpcmVjdG9yaWVzLm1EaXJlY3RvcnlUcmVlJywgbURpcmVjdG9yeVRyZWUpO1xyXG4gICAgICAgICAgdGhpcy5fRGF0YVN2Yy5ub3RpZnlEYXRhQ2hhbmdlZChmb3JDb250cm9sLCBtRGlyZWN0b3J5VHJlZSk7XHJcbiAgICAgICAgICByZXNvbHZlKHRydWUpO1xyXG4gICAgICAgIH0sXHJcbiAgICAgICAgZXJyb3I6IChlcnJvcjogYW55KSA9PiB7XHJcbiAgICAgICAgICB0aGlzLl9Mb2dnaW5nU3ZjLmVycm9ySGFuZGxlcihlcnJvciwgJ0Z1bmN0aW9uU2VydmljZScsICdnZXRGdW5jdGlvbicpO1xyXG4gICAgICAgICAgcmVqZWN0KGZhbHNlKTtcclxuICAgICAgICB9LFxyXG4gICAgICAgIC8vIGNvbXBsZXRlOiAoKSA9PiB7fVxyXG4gICAgICB9KTtcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgLyoqXHJcbiAgICogQGRlc2NyaXB0aW9uIFJldHJpdmVzIGEgYXJyYXkgb2YgSUZpbGVJbmZvTGlnaHQgZm9yIHRoZSBnaXZlbiBwYXRoXHJcbiAgICpcclxuICAgKiBAcGFyYW0ge3N0cmluZ30gYWN0aW9uIFVzZWQgdG8gZGV0ZXJtaW5lIHRoZSB1cGxvYWQgZGlyZWN0b3J5IGFuZCBlbmZvcmNlIHNlY3VyaXR5IG9uIHRoZSBzZXJ2ZXJcclxuICAgKiBAcGFyYW0ge3N0cmluZ30gY29udHJvbElkIFRoZSBpZCBvZiB0aGUgY29udHJvbGVyIHRoZSBmaWxlcyBhcmUgZm9yXHJcbiAgICogQHBhcmFtIHtzdHJpbmd9IHNlbGVjdGVkUGF0aCBUaGUgcmVsYXRpdmUgb2YgdGhlIGRpcmVjdG9yeSBwYXRoIFxyXG4gICAqIEBtZW1iZXJvZiBGaWxlTWFuYWdlclNlcnZpY2VcclxuICAgKi9cclxuICBwdWJsaWMgZ2V0RmlsZXMoYWN0aW9uOiBzdHJpbmcsIGNvbnRyb2xJZDogc3RyaW5nLCBzZWxlY3RlZFBhdGg6IHN0cmluZykge1xyXG4gICAgbGV0IG1RdWVyeVBhcmFtZXRlcjogSHR0cFBhcmFtcyA9IG5ldyBIdHRwUGFyYW1zKCkuYXBwZW5kKCdhY3Rpb24nLCBhY3Rpb24pO1xyXG4gICAgbVF1ZXJ5UGFyYW1ldGVyID0gbVF1ZXJ5UGFyYW1ldGVyLmFwcGVuZCgnc2VsZWN0ZWRQYXRoJywgc2VsZWN0ZWRQYXRoKTtcclxuICAgIGNvbnN0IG1IdHRwT3B0aW9ucyA9IHtcclxuICAgICAgaGVhZGVyczogbmV3IEh0dHBIZWFkZXJzKHtcclxuICAgICAgICAnQ29udGVudC1UeXBlJzogJ2FwcGxpY2F0aW9uL2pzb24nLFxyXG4gICAgICB9KSxcclxuICAgICAgcGFyYW1zOiBtUXVlcnlQYXJhbWV0ZXIsXHJcbiAgICB9O1xyXG4gICAgdGhpcy5fU2VsZWN0ZWRQYXRoID0gc2VsZWN0ZWRQYXRoO1xyXG4gICAgdGhpcy5fSHR0cENsaWVudC5nZXQ8SUZpbGVJbmZvTGlnaHRbXT4odGhpcy5fQXBpX0dldEZpbGVzLCBtSHR0cE9wdGlvbnMpLnN1YnNjcmliZSh7XHJcbiAgICAgIG5leHQ6IChyZXNwb25zZSkgPT4ge1xyXG4gICAgICAgIHRoaXMuX0RhdGFTdmMubm90aWZ5RGF0YUNoYW5nZWQoY29udHJvbElkLCByZXNwb25zZSk7XHJcbiAgICAgIH0sXHJcbiAgICAgIGVycm9yOiAoZXJyb3I6IGFueSkgPT4ge1xyXG4gICAgICAgIHRoaXMuX0xvZ2dpbmdTdmMuZXJyb3JIYW5kbGVyKGVycm9yLCAnRmlsZU1hbmFnZXJTZXJ2aWNlJywgJ2dldEZpbGVzJyk7XHJcbiAgICAgIH0sXHJcbiAgICAgIC8vIGNvbXBsZXRlOiAoKSA9PiB7fVxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgYXN5bmMgZ2V0VGVzdE5hdHVyYWxTb3J0KHNvcnREaXJlY3Rpb246IHN0cmluZyk6IFByb21pc2U8YW55PiB7XHJcbiAgICByZXR1cm4gbmV3IFByb21pc2U8YW55PigocmVzb2x2ZSwgcmVqZWN0KSA9PiB7XHJcbiAgICAgIGNvbnN0IG1RdWVyeVBhcmFtZXRlcjogSHR0cFBhcmFtcyA9IG5ldyBIdHRwUGFyYW1zKCkuYXBwZW5kKCdzb3J0RGlyZWN0aW9uJywgc29ydERpcmVjdGlvbik7XHJcbiAgICAgIGNvbnN0IG1IdHRwT3B0aW9ucyA9IHtcclxuICAgICAgICBoZWFkZXJzOiBuZXcgSHR0cEhlYWRlcnMoe1xyXG4gICAgICAgICAgJ0NvbnRlbnQtVHlwZSc6ICdhcHBsaWNhdGlvbi9qc29uJyxcclxuICAgICAgICB9KSxcclxuICAgICAgICBwYXJhbXM6IG1RdWVyeVBhcmFtZXRlcixcclxuICAgICAgfTtcclxuICAgICAgdGhpcy5fSHR0cENsaWVudC5nZXQ8YW55Pih0aGlzLl9BcGlfR2V0VGVzdE5hdHVyYWxTb3J0LCBtSHR0cE9wdGlvbnMpLnN1YnNjcmliZSh7XHJcbiAgICAgICAgbmV4dDogKHJlc3BvbnNlOiBhbnkpID0+IHtcclxuICAgICAgICAgIHJlc29sdmUocmVzcG9uc2UpO1xyXG4gICAgICAgIH0sXHJcbiAgICAgICAgZXJyb3I6IChlcnJvcjogYW55KSA9PiB7XHJcbiAgICAgICAgICB0aGlzLl9Mb2dnaW5nU3ZjLmVycm9ySGFuZGxlcihlcnJvciwgJ0ZpbGVNYW5hZ2VyU2VydmljZScsICdnZXRUZXN0TmF0dXJhbFNvcnQnKTtcclxuICAgICAgICAgIHJlamVjdChmYWxzZSk7XHJcbiAgICAgICAgfSxcclxuICAgICAgICAvLyBjb21wbGV0ZTogKCkgPT4ge31cclxuICAgICAgfSk7XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIC8qKlxyXG4gICAqIEBkZXNjcmlwdGlvbiBDYWxjdWxhdGVzIHRoZSB0b3RhbCBudW1iZXIgb2YgY2h1bmNrcyBuZWVkZWQgdG8gdXBsb2FkIGEgbGFyZ2UgZmlsZVxyXG4gICAqXHJcbiAgICogQHBhcmFtIHtudW1iZXJ9IGZpbGVTaXplICBUaGUgc2l6ZSBvZiB0aGUgZmlsZVxyXG4gICAqIEBwYXJhbSB7bnVtYmVyfSBjaHVua1NpemUgVGhlIHNpemUgb2YgdGhlIGNodW5jayB0aGF0IHRoZSBzZXJ2ZXIgY2FuIGFjY2VwdFxyXG4gICAqIEByZXR1cm4geyp9ICB7bnVtYmVyfVxyXG4gICAqIEBtZW1iZXJvZiBGaWxlTWFuYWdlclNlcnZpY2VcclxuICAgKi9cclxuICBwdWJsaWMgZ2V0VG90YWxOdW1iZXJPZlVwbG9hZHMoZmlsZVNpemU6IG51bWJlciwgY2h1bmtTaXplOiBudW1iZXIpOiBudW1iZXIge1xyXG4gICAgY29uc3QgbVJldFZhbCA9IGZpbGVTaXplICUgY2h1bmtTaXplID09IDAgPyBmaWxlU2l6ZSAvIGNodW5rU2l6ZSA6IE1hdGguZmxvb3IoZmlsZVNpemUgLyBjaHVua1NpemUpICsgMTtcclxuICAgIHJldHVybiBtUmV0VmFsOyAgICBcclxuICB9XHJcblxyXG4gIC8qKlxyXG4gICAqIEBkZXNjcmlwdGlvbiBVcGxvYWRzIGZpbGVzIHRoYXQgYXJlIHRvbyBsYXJnZSB0byBiZSBzZW50IGluIGEgc2luZ2xlIHVwbG9hZCBieSB1c2luZyByZWN1cnNpb25cclxuICAgKiB0byBjYWxsIHRoZSBBUEkgd2l0aCBhIFwic2xpY2VcIiBvZiB0aGUgZmlsZS5cclxuICAgKlxyXG4gICAqIEBwcml2YXRlXHJcbiAgICogQHBhcmFtIHtJTXVsdGlQYXJ0RmlsZVVwbG9hZFBhcmFtZXRlcnN9IHBhcmFtZXRlcnNcclxuICAgKiBAbWVtYmVyb2YgRmlsZU1hbmFnZXJTZXJ2aWNlXHJcbiAgICovXHJcbiAgcHJpdmF0ZSBtdWx0aVBhcnRGaWxlVXBsb2FkKHBhcmFtZXRlcnM6IElNdWx0aVBhcnRGaWxlVXBsb2FkUGFyYW1ldGVycykge1xyXG4gICAgY29uc3QgbVBhcmFtcyA9IHsuLi5wYXJhbWV0ZXJzfTsgLy8gaXQncyBnb29kIHByYWN0aWNlIHRvIGxlYXZlIHBhcmFtZXRlciB2YWx1ZXMgdW5jaGFuZ2VkXHJcbiAgICBsZXQgbU5leHRVcGxvYWROdW1iZXIgPSBtUGFyYW1zLnVwbG9hZE51bWJlciArIDE7XHJcbiAgICAvLyBjb25zdCBtRmlsZVNpemU6IG51bWJlciA9IG1QYXJhbXMuZmlsZS5zaXplO1xyXG4gICAgY29uc3QgbU11bHRpVXBsb2FkRmlsZU5hbWUgPSBtUGFyYW1zLmZpbGUubmFtZSArIFwiX1VwbG9hZE51bWJlcl9cIiArIG1OZXh0VXBsb2FkTnVtYmVyO1xyXG4gICAgaWYobVBhcmFtcy51cGxvYWROdW1iZXIgPCBtUGFyYW1zLnRvdGFsTnVtYmVyT2ZVcGxvYWRzKSB7IC8vIGRvIHdlIG5lZWQgdG8gc2VuZCBhbnkgbW9yZSBwaWNlcyBvZiB0aGUgZmlsZVxyXG4gICAgICBjb25zdCBtQmxvYjogQmxvYiA9IG1QYXJhbXMuZmlsZS5zbGljZS5jYWxsKG1QYXJhbXMuZmlsZSwgbVBhcmFtcy5zdGFydGluZ0J5dGUsIG1QYXJhbXMuZW5kaW5nQnl0ZSk7XHJcbiAgICAgIGNvbnN0IG1Gb3JtRGF0YTogRm9ybURhdGEgPSBuZXcgRm9ybURhdGEoKTtcclxuICAgICAgbUZvcm1EYXRhLmFwcGVuZCgnYWN0aW9uJywgbVBhcmFtcy5hY3Rpb24pO1xyXG4gICAgICBtRm9ybURhdGEuYXBwZW5kKCdjb21wbGV0ZWQnLCAnZmFsc2UnKTtcclxuICAgICAgbUZvcm1EYXRhLmFwcGVuZCgnc2VsZWN0ZWRQYXRoJywgdGhpcy5fU2VsZWN0ZWRQYXRoKTtcclxuICAgICAgbUZvcm1EYXRhLmFwcGVuZChtTXVsdGlVcGxvYWRGaWxlTmFtZSwgbUJsb2IpO1xyXG4gICAgICB0aGlzLl9IdHRwQ2xpZW50LnBvc3Q8SVVwbG9hZFJlc3BvbnNlPih0aGlzLl9BcGlfVXBsb2FkRmlsZSwgbUZvcm1EYXRhKS5zdWJzY3JpYmUoe1xyXG4gICAgICAgIG5leHQ6IChyZXNwb25zZTogSVVwbG9hZFJlc3BvbnNlKSA9PiB7IC8vIHVwZGF0ZSB0aGUgcGFyYW1ldGVycyBjYW4gY2FsbCB0aGlzIG1ldGhvZCBhZ2FpblxyXG4gICAgICAgICAgY29uc3QgbVVwbG9hZFN0YXR1czogSVVwbG9hZFN0YXR1cyA9IG5ldyBVcGxvYWRTdGF0dXMobVBhcmFtcy5hY3Rpb24sIHJlc3BvbnNlLmZpbGVOYW1lLCByZXNwb25zZS5kYXRhLCBmYWxzZSwgcmVzcG9uc2UuaXNTdWNjZXNzLCBtUGFyYW1zLnRvdGFsTnVtYmVyT2ZVcGxvYWRzLCBtUGFyYW1zLnVwbG9hZE51bWJlcik7XHJcbiAgICAgICAgICBtUGFyYW1zLnVwbG9hZE51bWJlciA9IG1OZXh0VXBsb2FkTnVtYmVyO1xyXG4gICAgICAgICAgbVBhcmFtcy5zdGFydGluZ0J5dGUgPSBwYXJhbWV0ZXJzLmVuZGluZ0J5dGU7XHJcbiAgICAgICAgICBtUGFyYW1zLmVuZGluZ0J5dGUgPSBwYXJhbWV0ZXJzLmVuZGluZ0J5dGUgKyBwYXJhbWV0ZXJzLmNodW5rU2l6ZVxyXG4gICAgICAgICAgdGhpcy51cGxvYWRTdGF0dXNDaGFuZ2VkLm5leHQobVVwbG9hZFN0YXR1cyk7XHJcbiAgICAgICAgICB0aGlzLm11bHRpUGFydEZpbGVVcGxvYWQobVBhcmFtcyk7XHJcbiAgICAgICAgfSxcclxuICAgICAgICBlcnJvcjogKGVycm9yOiBhbnkpID0+IHtcclxuICAgICAgICAgIGlmKHBhcmFtZXRlcnMucmV0cnlOdW1iZXIgPCA0KSB7XHJcbiAgICAgICAgICAgIG1QYXJhbXMucmV0cnlOdW1iZXIgPSBtUGFyYW1zLnJldHJ5TnVtYmVyICsgMTtcclxuICAgICAgICAgICAgdGhpcy5tdWx0aVBhcnRGaWxlVXBsb2FkKG1QYXJhbXMpO1xyXG4gICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgdGhpcy5fTG9nZ2luZ1N2Yy5lcnJvckhhbmRsZXIoZXJyb3IsICdGaWxlTWFuYWdlbWVudFNlcnZpY2UnLCAndXBsb2FkJyk7XHJcbiAgICAgICAgICAgIC8vIGNvbnN0IG1VcGxvYWRTdGF0dXM6IElVcGxvYWRTdGF0dXMgPSBuZXcgVXBsb2FkU3RhdHVzKG1QYXJhbXMuYWN0aW9uLCBtTXVsdGlVcGxvYWRGaWxlTmFtZSwgZXJyb3IsIGZhbHNlLCBmYWxzZSwgbVBhcmFtcy50b3RhbE51bWJlck9mVXBsb2FkcywgbVBhcmFtcy51cGxvYWROdW1iZXIpO1xyXG4gICAgICAgICAgICBtUGFyYW1zLnVwbG9hZE51bWJlciA9IG1OZXh0VXBsb2FkTnVtYmVyO1xyXG4gICAgICAgICAgfVxyXG4gICAgICAgIH0sXHJcbiAgICAgICAgLy8gY29tcGxldGU6ICgpID0+IHt9XHJcbiAgICAgIH0pO1xyXG4gICAgfTtcclxuICAgIGlmKG1QYXJhbXMudXBsb2FkTnVtYmVyID09IG1QYXJhbXMudG90YWxOdW1iZXJPZlVwbG9hZHMpIHsgLy8gbWFrZSBzdXJlIHRoaXMgaXMgdGhlIGxhc3QgdXBsb2FkXHJcbiAgICAgIHRoaXMubXVsdGlVcGxvYWRDb21wbGV0ZShtUGFyYW1zLmFjdGlvbiwgbVBhcmFtcy5maWxlLm5hbWUsIHRoaXMuX0FwaV9VcGxvYWRGaWxlKS5zdWJzY3JpYmUoe1xyXG4gICAgICAgIG5leHQ6IChyZXNwb25zZTogSVVwbG9hZFJlc3BvbnNlKSA9PiB7XHJcbiAgICAgICAgICBjb25zdCBtVXBsb2FkU3RhdHVzOiBJVXBsb2FkU3RhdHVzID0gbmV3IFVwbG9hZFN0YXR1cyhtUGFyYW1zLmFjdGlvbiwgcmVzcG9uc2UuZmlsZU5hbWUsIHJlc3BvbnNlLmRhdGEsIHRydWUsIHJlc3BvbnNlLmlzU3VjY2VzcywgbVBhcmFtcy50b3RhbE51bWJlck9mVXBsb2FkcywgbVBhcmFtcy51cGxvYWROdW1iZXIpO1xyXG4gICAgICAgICAgdGhpcy51cGxvYWRTdGF0dXNDaGFuZ2VkLm5leHQobVVwbG9hZFN0YXR1cyk7XHJcbiAgICAgICAgfSxcclxuICAgICAgICBlcnJvcjogKGVycm9yOiBhbnkpID0+IHtcclxuICAgICAgICAgIHRoaXMuX0xvZ2dpbmdTdmMuZXJyb3JIYW5kbGVyKGVycm9yLCAnRmlsZU1hbmFnZW1lbnRTZXJ2aWNlJywgJ3VwbG9hZCcpO1xyXG4gICAgICAgICAgY29uc3QgbVVwbG9hZFN0YXR1czogSVVwbG9hZFN0YXR1cyA9IG5ldyBVcGxvYWRTdGF0dXMobVBhcmFtcy5hY3Rpb24sIG1QYXJhbXMuZmlsZS5uYW1lLCBlcnJvciwgdHJ1ZSwgZmFsc2UsIG1QYXJhbXMudG90YWxOdW1iZXJPZlVwbG9hZHMsIG1QYXJhbXMudXBsb2FkTnVtYmVyKTtcclxuICAgICAgICAgIHRoaXMudXBsb2FkU3RhdHVzQ2hhbmdlZC5uZXh0KG1VcGxvYWRTdGF0dXMpO1xyXG4gICAgICAgIH0sXHJcbiAgICAgICAgLy8gY29tcGxldGU6ICgpID0+IHt9XHJcbiAgICAgIH0pO1xyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgLyoqXHJcbiAgICogQGRlc2NyaXB0aW9uIEhlbHBlciBtZXRob2QgdGhhdCBjYWxscyB0aGUgdXBsb2FkIEFQSSBzbyB0aGUgbWVyZ2luZyBvZiB0aGUgZmlsZSBjaHVua3MgKHNsaWNlcylcclxuICAgKiBjYW4gYmUgZG9uZVxyXG4gICAqXHJcbiAgICogQHByaXZhdGVcclxuICAgKiBAcGFyYW0ge3N0cmluZ30gYWN0aW9uIFVzZWQgdG8gZGV0ZXJtaW5lIHRoZSB1cGxvYWQgZGlyZWN0b3J5IGFuZCBlbmZvcmNlIHNlY3VyaXR5IG9uIHRoZSBzZXJ2ZXIuXHJcbiAgICogQHBhcmFtIHtzdHJpbmd9IGZpbGVOYW1lIFRoZSBmaWxlIG5hbWUgZm9yIHRoaXMgQ2h1bmsuXHJcbiAgICogQHBhcmFtIHtzdHJpbmd9IHVyaSBUaGUgVW5pZm9ybSBSZXNvdXJjZSBJZGVudGlmaWVyLlxyXG4gICAqIEByZXR1cm4geyp9ICB7T2JzZXJ2YWJsZTxJVXBsb2FkUmVzcG9uc2U+fVxyXG4gICAqIEBtZW1iZXJvZiBGaWxlTWFuYWdlclNlcnZpY2VcclxuICAgKi9cclxuICBwcml2YXRlIG11bHRpVXBsb2FkQ29tcGxldGUoYWN0aW9uOiBzdHJpbmcsIGZpbGVOYW1lOiBzdHJpbmcsIHVyaTogc3RyaW5nKTogT2JzZXJ2YWJsZTxJVXBsb2FkUmVzcG9uc2U+IHtcclxuICAgIHZhciBtRm9ybURhdGEgPSBuZXcgRm9ybURhdGEoKTtcclxuICAgIG1Gb3JtRGF0YS5hcHBlbmQoJ2FjdGlvbicsIGFjdGlvbik7XHJcbiAgICBtRm9ybURhdGEuYXBwZW5kKCdjb21wbGV0ZWQnLCAndHJ1ZScpO1xyXG4gICAgbUZvcm1EYXRhLmFwcGVuZCgnZmlsZU5hbWUnLCBmaWxlTmFtZSk7XHJcbiAgICBtRm9ybURhdGEuYXBwZW5kKCdzZWxlY3RlZFBhdGgnLCB0aGlzLl9TZWxlY3RlZFBhdGgpO1xyXG4gICAgcmV0dXJuIHRoaXMuX0h0dHBDbGllbnQucG9zdDxJVXBsb2FkUmVzcG9uc2U+KHVyaSwgbUZvcm1EYXRhKTsgICAgXHJcbiAgfVxyXG5cclxuLyoqXHJcbiAqIEBkZXNjcmlwdGlvbiBSZWZyZXNoZXMgdGhlIHNwZWNpZmllZCBhY3Rpb24uXHJcbiAqXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBhY3Rpb24gLSBVc2VkIHRvIGRldGVybWluZSB0aGUgdXBsb2FkIGRpcmVjdG9yeSBhbmQgZW5mb3JjZSBzZWN1cml0eSBvbiB0aGUgc2VydmVyLlxyXG4gKiBAcGFyYW0ge0lEaXJlY3RvcnlUcmVlfSBkaXJlY3RvcnlUcmVlIC0gT3B0aW9uYWwgZGlyZWN0b3J5IHRyZWUgdG8gdXNlIHRvIGRldGVybWluZSB0aGUgc2VsZWN0ZWQgcGF0aC5cclxuICogQHJldHVybiB7UHJvbWlzZTxhbnk+fSBBIFByb21pc2UgdGhhdCByZXNvbHZlcyB3aGVuIHRoZSByZWZyZXNoIGlzIGNvbXBsZXRlLlxyXG4gKiBAbWVtYmVyb2YgRmlsZU1hbmFnZXJTZXJ2aWNlXHJcbiAqL1xyXG4gIGFzeW5jIHJlZnJlc2goYWN0aW9uOiBzdHJpbmcsIGRpcmVjdG9yeVRyZWU/OiBJRGlyZWN0b3J5VHJlZSk6IFByb21pc2U8YW55PiB7XHJcbiAgICBjb25zdCBtQWN0aW9uID0gYWN0aW9uLnRyaW0oKTtcclxuICAgIGxldCBtU2VsZWN0ZWROb2RlID0gdGhpcy5fQ3VycmVudERpcmVjdG9yeVRyZWU7XHJcbiAgICBjb25zdCBtRGlyZWN0b3J5Q29udHJvbE5hbWUgPSBtQWN0aW9uICsgJ19EaXJlY3Rvcmllcyc7XHJcbiAgICBjb25zdCBtRmlsZUNvbnRyb2xlTmFtZSA9IG1BY3Rpb24gKyAnX0ZpbGVzJztcclxuICAgIGlmKGRpcmVjdG9yeVRyZWUgIT0gdW5kZWZpbmVkKSB7XHJcbiAgICAgIG1TZWxlY3RlZE5vZGUgPSBkaXJlY3RvcnlUcmVlO1xyXG4gICAgfVxyXG4gICAgdGhpcy5nZXREaXJlY3RvcmllcyhtQWN0aW9uLCBtRGlyZWN0b3J5Q29udHJvbE5hbWUsIG1TZWxlY3RlZE5vZGUucmVsaXRpdmVQYXRoKS5jYXRjaCgoZXJyb3IpID0+IHtcclxuICAgICAgdGhpcy5fTG9nZ2luZ1N2Yy5lcnJvckhhbmRsZXIoZXJyb3IsICdGaWxlTWFuYWdlclNlcnZpY2UnLCAncmVmcmVzaC9nZXREaXJlY3RvcmllcycpO1xyXG4gICAgfSkudGhlbigocmVzcG9uc2U6IGFueSkgPT4ge1xyXG4gICAgICB0aGlzLnNlbGVjdGVkRGlyZWN0b3J5Q2hhbmdlZC5uZXh0KG1TZWxlY3RlZE5vZGUpO1xyXG4gICAgICB0aGlzLl9TZWxlY3RlZFBhdGggPSBtU2VsZWN0ZWROb2RlLnJlbGl0aXZlUGF0aDtcclxuICAgICAgdGhpcy5nZXRGaWxlcyhtQWN0aW9uLCBtRmlsZUNvbnRyb2xlTmFtZSwgdGhpcy5fU2VsZWN0ZWRQYXRoKTtcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgLyoqXHJcbiAgICogQGRlc2NyaXB0aW9uIFJlbmFtZXMgYSBkaXJlY3RvcnkgYXN5bmNocm9ub3VzbHkuXHJcbiAgICpcclxuICAgKiBAcGFyYW0ge3N0cmluZ30gYWN0aW9uIC0gVXNlZCB0byBkZXRlcm1pbmUgdGhlIHVwbG9hZCBkaXJlY3RvcnkgYW5kIGVuZm9yY2Ugc2VjdXJpdHkgb24gdGhlIHNlcnZlci5cclxuICAgKiBAcGFyYW0ge3N0cmluZ30gbmV3TmFtZSAtIFRoZSBuZXcgbmFtZSBmb3IgdGhlIGRpcmVjdG9yeS5cclxuICAgKiBAcmV0dXJuIHtQcm9taXNlPGJvb2xlYW4+fSBBIHByb21pc2UgdGhhdCByZXNvbHZlcyB0byB0cnVlIGlmIHRoZSBkaXJlY3Rvcnkgd2FzIHJlbmFtZWQgc3VjY2Vzc2Z1bGx5LCBvciBmYWxzZSBvdGhlcndpc2UuXHJcbiAgICogQG1lbWJlcm9mIEZpbGVNYW5hZ2VyU2VydmljZVxyXG4gICAqL1xyXG4gIGFzeW5jIHJlbmFtZURpcmVjdG9yeShhY3Rpb246IHN0cmluZywgbmV3TmFtZTogc3RyaW5nLCBkaXJlY3RvcnlDb250cm9sTmFtZTogc3RyaW5nKTogUHJvbWlzZTxib29sZWFuPiB7XHJcbiAgICAvKipcclxuICAgICAqIDEuKSBHZXQgdGhlIGN1cnJlbnQgZGlyZWN0b3J5XHJcbiAgICAgKiAyLikgQ2FsY3VsYXRlIHRoZSBuZXcgZGlyZWN0b3J5IHRvIHNlbGVjdCB3aGVuIHJlbmFtZSBpcyBzdWNjZXNzZnVsXHJcbiAgICAgKiAzLikgQ2FsbCBGaWxlTWFuYWdlclNlcnZpY2UgdG8gcmVuYW1lIHRoZSBkaXJlY3RvcnlcclxuICAgICAqIDQuKSBJZiBzdWNjZXNzZnVsLCBzZXQgdGhlIHNlbGVjdGVkIGRpcmVjdG9yeSB0byB0aGUgY2FsY3VsYXRlZCBuZXcgZGlyZWN0b3J5IGluIHN0ZXAgMlxyXG4gICAgICogNS4pIFJlZnJlc2ggdGhlIGRpcmVjdG9yeSB0cmVlIGFuZCBmaWxlIGxpc3RcclxuICAgICAqIDYuKSBDbG9zZSB0aGUgbW9kYWxcclxuICAgICAqXHJcbiAgICAgKiA0LikgSWYgdW5zdWNjZXNzZnVsLCBsb2cgdGhlIGVycm9yXHJcbiAgICAgKiA1LikgTm90aWZ5IHRoZSBjbGllbnRcclxuICAgICAqIDYuKSBDbG9zZSB0aGUgbW9kYWxcclxuICAgICAqXHJcbiAgICAgKlxyXG4gICAgICovXHJcblxyXG4gICAgaWYodGhpcy5fR1dDb21tb24uaXNOdWxsT3JFbXB0eShhY3Rpb24pKSB7XHJcbiAgICAgIHRocm93IG5ldyBFcnJvcihcImFjdGlvbiBjYW4gbm90IGJlIGJsYW5rIVwiKTtcclxuICAgIH1cclxuICAgIGlmKHRoaXMuX0dXQ29tbW9uLmlzTnVsbE9yRW1wdHkobmV3TmFtZSkpIHtcclxuICAgICAgdGhyb3cgbmV3IEVycm9yKFwibmV3TmFtZSBjYW4gbm90IGJlIGJsYW5rIVwiKTtcclxuICAgIH1cclxuICAgIGNvbnN0IG1EaXJlY3RvcnlQYXJ0cyA9IHRoaXMuU2VsZWN0ZWRQYXRoLnNwbGl0KCdcXFxcJyk7XHJcbiAgICBtRGlyZWN0b3J5UGFydHNbbURpcmVjdG9yeVBhcnRzLmxlbmd0aCAtIDFdID0gbmV3TmFtZTtcclxuICAgIGNvbnN0IG1OZXdTZWxlY3RlZFBhdGggPSBtRGlyZWN0b3J5UGFydHMuam9pbignXFxcXCcpO1xyXG4gICAgY29uc3QgbUN1cnJlbnREaXJlY3RvcnlUcmVlID0gdGhpcy5DdXJyZW50RGlyZWN0b3J5VHJlZTtcclxuICAgIG1DdXJyZW50RGlyZWN0b3J5VHJlZS5rZXkgPSBuZXdOYW1lLnJlcGxhY2UoJyAnLCAnJykudG9Mb3dlckNhc2UoKTtcclxuICAgIG1DdXJyZW50RGlyZWN0b3J5VHJlZS5uYW1lID0gbmV3TmFtZTtcclxuICAgIG1DdXJyZW50RGlyZWN0b3J5VHJlZS5yZWxpdGl2ZVBhdGggPSBtTmV3U2VsZWN0ZWRQYXRoO1xyXG4gICAgcmV0dXJuIG5ldyBQcm9taXNlPGJvb2xlYW4+KChyZXNvbHZlLCByZWplY3QpID0+IHtcclxuICAgICAgbGV0IG1RdWVyeVBhcmFtZXRlcjogSHR0cFBhcmFtcyA9IG5ldyBIdHRwUGFyYW1zKCkuYXBwZW5kKCdhY3Rpb24nLCBhY3Rpb24pO1xyXG4gICAgICBtUXVlcnlQYXJhbWV0ZXI9bVF1ZXJ5UGFyYW1ldGVyLmFwcGVuZCgnc2VsZWN0ZWRQYXRoJywgdGhpcy5fU2VsZWN0ZWRQYXRoKTtcclxuICAgICAgbVF1ZXJ5UGFyYW1ldGVyPW1RdWVyeVBhcmFtZXRlci5hcHBlbmQoJ25ld05hbWUnLCBuZXdOYW1lKTtcclxuICAgICAgY29uc3QgbUh0dHBPcHRpb25zID0ge1xyXG4gICAgICAgIGhlYWRlcnM6IG5ldyBIdHRwSGVhZGVycyh7XHJcbiAgICAgICAgICAnQ29udGVudC1UeXBlJzogJ2FwcGxpY2F0aW9uL2pzb24nLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICAgIHBhcmFtczogbVF1ZXJ5UGFyYW1ldGVyLFxyXG4gICAgICB9O1xyXG4gICAgICB0aGlzLl9IdHRwQ2xpZW50LnBvc3Q8Ym9vbGVhbj4odGhpcy5fQXBpX1JlbmFtZURpcmVjdG9yeSwgbnVsbCwgbUh0dHBPcHRpb25zKS5zdWJzY3JpYmUoe1xyXG4gICAgICAgIG5leHQ6KCByZXNwb25zZTogYm9vbGVhbiApID0+IHtcclxuICAgICAgICAgIHRoaXMuZ2V0RGlyZWN0b3JpZXMoYWN0aW9uLCBtQ3VycmVudERpcmVjdG9yeVRyZWUucmVsaXRpdmVQYXRoLCBkaXJlY3RvcnlDb250cm9sTmFtZSkuY2F0Y2goKGVycm9yKSA9PiB7XHJcbiAgICAgICAgICAgIHRoaXMuX0xvZ2dpbmdTdmMuZXJyb3JIYW5kbGVyKGVycm9yLCAnRmlsZU1hbmFnZXJTZXJ2aWNlJywgJ3JlbmFtZURpcmVjdG9yeS9nZXREaXJlY3RvcmllcycpO1xyXG4gICAgICAgICAgICByZWplY3QoZmFsc2UpO1xyXG4gICAgICAgICAgfSkudGhlbigoXykgPT4ge1xyXG4gICAgICAgICAgICAvLyBjb25zb2xlLmxvZygnbUN1cnJlbnREaXJlY3RvcnlUcmVlJywgbUN1cnJlbnREaXJlY3RvcnlUcmVlKTtcclxuICAgICAgICAgICAgdGhpcy5zZXRTZWxlY3RlZERpcmVjdG9yeShtQ3VycmVudERpcmVjdG9yeVRyZWUpO1xyXG4gICAgICAgICAgICByZXNvbHZlKHJlc3BvbnNlKTtcclxuICAgICAgICAgIH0pXHJcbiAgICAgICAgfSxcclxuICAgICAgICBlcnJvcjooIGVycm9yOiBhbnkgKSA9PiB7XHJcbiAgICAgICAgICB0aGlzLl9Mb2dnaW5nU3ZjLmVycm9ySGFuZGxlcihlcnJvciwgJ0ZpbGVNYW5hZ2VyU2VydmljZScsICdyZW5hbWVEaXJlY3RvcnknKTtcclxuICAgICAgICAgIHJlamVjdChmYWxzZSk7XHJcbiAgICAgICAgfSxcclxuICAgICAgICBjb21wbGV0ZTooICkgPT4ge31cclxuICAgICAgfSk7XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIC8qKlxyXG4gICAqIEBkZXNjcmlwdGlvbiBSZW5hbWVzIGFuIGV4aXN0aW5nIGZpbGVcclxuICAgKlxyXG4gICAqIEBwYXJhbSB7c3RyaW5nfSBhY3Rpb24gVXNlZCB0byBkZXRlcm1pbmUgdGhlIHVwbG9hZCBkaXJlY3RvcnkgYW5kIGVuZm9yY2Ugc2VjdXJpdHkgb24gdGhlIHNlcnZlclxyXG4gICAqIEBwYXJhbSB7c3RyaW5nfSBvbGROYW1lIFRoZSBuYW1lIG9mIHRoZSBmaWxlIHRvIHJlbmFtZVxyXG4gICAqIEBwYXJhbSB7c3RyaW5nfSBuZXdOYW1lIFRoZSBuZXcgbmFtZSBvZiB0aGUgZmlsZVxyXG4gICAqIEByZXR1cm4geyp9ICB7UHJvbWlzZTxib29sZWFuPn1cclxuICAgKiBAbWVtYmVyb2YgRmlsZU1hbmFnZXJTZXJ2aWNlXHJcbiAgICovXHJcbiAgYXN5bmMgcmVuYW1lRmlsZShhY3Rpb246IHN0cmluZywgb2xkTmFtZTogc3RyaW5nLCBuZXdOYW1lOiBzdHJpbmcpOiBQcm9taXNlPGJvb2xlYW4+IHtcclxuICAgIGlmKHRoaXMuX0dXQ29tbW9uLmlzTnVsbE9yRW1wdHkoYWN0aW9uKSkge1xyXG4gICAgICB0aHJvdyBuZXcgRXJyb3IoXCJhY3Rpb24gY2FuIG5vdCBiZSBibGFuayFcIik7XHJcbiAgICB9XHJcbiAgICBpZih0aGlzLl9HV0NvbW1vbi5pc051bGxPckVtcHR5KG9sZE5hbWUpKSB7XHJcbiAgICAgIHRocm93IG5ldyBFcnJvcihcIm9sZE5hbWUgY2FuIG5vdCBiZSBibGFuayFcIik7XHJcbiAgICB9XHJcbiAgICBpZih0aGlzLl9HV0NvbW1vbi5pc051bGxPckVtcHR5KG5ld05hbWUpKSB7XHJcbiAgICAgIHRocm93IG5ldyBFcnJvcihcIm5ld05hbWUgY2FuIG5vdCBiZSBibGFuayFcIik7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gbmV3IFByb21pc2U8Ym9vbGVhbj4oKHJlc29sdmUsIHJlamVjdCkgPT4ge1xyXG4gICAgICBsZXQgbVF1ZXJ5UGFyYW1ldGVyOiBIdHRwUGFyYW1zID0gbmV3IEh0dHBQYXJhbXMoKS5hcHBlbmQoJ2FjdGlvbicsIGFjdGlvbik7XHJcbiAgICAgIG1RdWVyeVBhcmFtZXRlcj1tUXVlcnlQYXJhbWV0ZXIuYXBwZW5kKCdzZWxlY3RlZFBhdGgnLCB0aGlzLl9TZWxlY3RlZFBhdGgpO1xyXG4gICAgICBtUXVlcnlQYXJhbWV0ZXI9bVF1ZXJ5UGFyYW1ldGVyLmFwcGVuZCgnb2xkTmFtZScsIG9sZE5hbWUpO1xyXG4gICAgICBtUXVlcnlQYXJhbWV0ZXI9bVF1ZXJ5UGFyYW1ldGVyLmFwcGVuZCgnbmV3TmFtZScsIG5ld05hbWUpO1xyXG4gICAgICBjb25zdCBtSHR0cE9wdGlvbnMgPSB7XHJcbiAgICAgICAgaGVhZGVyczogbmV3IEh0dHBIZWFkZXJzKHtcclxuICAgICAgICAgICdDb250ZW50LVR5cGUnOiAnYXBwbGljYXRpb24vanNvbicsXHJcbiAgICAgICAgfSksXHJcbiAgICAgICAgcGFyYW1zOiBtUXVlcnlQYXJhbWV0ZXIsXHJcbiAgICAgIH07XHJcbiAgICAgIHRoaXMuX0h0dHBDbGllbnQucG9zdDxib29sZWFuPih0aGlzLl9BcGlfUmVuYW1lRmlsZSwgbnVsbCwgbUh0dHBPcHRpb25zKS5zdWJzY3JpYmUoe1xyXG4gICAgICAgIG5leHQ6KCByZXNwb25zZTogYm9vbGVhbiApID0+IHtcclxuICAgICAgICAgIHJlc29sdmUocmVzcG9uc2UpXHJcbiAgICAgICAgfSxcclxuICAgICAgICBlcnJvcjooIGVycm9yOiBhbnkgKSA9PiB7XHJcbiAgICAgICAgICB0aGlzLl9Mb2dnaW5nU3ZjLmVycm9ySGFuZGxlcihlcnJvciwgJ0ZpbGVNYW5hZ2VyU2VydmljZScsICdyZW5hbWVGaWxlJyk7XHJcbiAgICAgICAgICByZWplY3QoZmFsc2UpO1xyXG4gICAgICAgIH0sXHJcbiAgICAgICAgY29tcGxldGU6KCApID0+IHt9XHJcbiAgICAgIH0pO1xyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICAvKipcclxuICAgKiBAZGVzY3JpcHRpb24gUGVyZm9ybXMgYSBzaW5nbGUgZmlsZSB1cGxvYWRcclxuICAgKlxyXG4gICAqIEBwcml2YXRlXHJcbiAgICogQHBhcmFtIHtGaWxlfSBmaWxlIHRoZSBIVE1MIFwiZmlsZVwiIG9iamVjdFxyXG4gICAqIEBwYXJhbSB7c3RyaW5nfSBhY3Rpb24gVXNlZCB0byBkZXRlcm1pbmUgdGhlIHVwbG9hZCBkaXJlY3RvcnkgYW5kIGVuZm9yY2Ugc2VjdXJpdHkgb24gdGhlIHNlcnZlclxyXG4gICAqIEBtZW1iZXJvZiBGaWxlTWFuYWdlclNlcnZpY2VcclxuICAgKi9cclxuICBwcml2YXRlIHNpbmdsZUZpbGVVcGxvYWQoZmlsZTogRmlsZSwgYWN0aW9uOiBzdHJpbmcpIHtcclxuICAgIGNvbnN0IG1Gb3JtRGF0YSA9IG5ldyBGb3JtRGF0YSgpO1xyXG4gICAgbUZvcm1EYXRhLmFwcGVuZCgnYWN0aW9uJywgYWN0aW9uKTtcclxuICAgIG1Gb3JtRGF0YS5hcHBlbmQoJ3NlbGVjdGVkUGF0aCcsIHRoaXMuX1NlbGVjdGVkUGF0aCk7XHJcbiAgICBtRm9ybURhdGEuYXBwZW5kKGZpbGUubmFtZSArICdfVXBsb2FkTnVtYmVyXzEnLCBmaWxlKTtcclxuICAgIG1Gb3JtRGF0YS5hcHBlbmQoJ2ZpbGVOYW1lJywgZmlsZS5uYW1lKTtcclxuICAgIG1Gb3JtRGF0YS5hcHBlbmQoJ2NvbXBsZXRlZCcsICd0cnVlJyk7XHJcbiAgICB0aGlzLl9IdHRwQ2xpZW50LnBvc3Q8SVVwbG9hZFJlc3BvbnNlPih0aGlzLl9BcGlfVXBsb2FkRmlsZSwgbUZvcm1EYXRhKS5zdWJzY3JpYmUoe1xyXG4gICAgICBuZXh0OiAocmVzcG9uc2U6IElVcGxvYWRSZXNwb25zZSkgPT4ge1xyXG4gICAgICAgIGNvbnN0IG1VcGxvYWRTdGF0dXM6IElVcGxvYWRTdGF0dXMgPSBuZXcgVXBsb2FkU3RhdHVzKGFjdGlvbiwgcmVzcG9uc2UuZmlsZU5hbWUsIHJlc3BvbnNlLmRhdGEsIHRydWUsIHJlc3BvbnNlLmlzU3VjY2VzcywgMSwgMSk7XHJcbiAgICAgICAgdGhpcy51cGxvYWRTdGF0dXNDaGFuZ2VkLm5leHQobVVwbG9hZFN0YXR1cyk7XHJcbiAgICAgIH0sXHJcbiAgICAgIGVycm9yOiAoZXJyb3I6IGFueSkgPT4ge1xyXG4gICAgICAgIHRoaXMuX0xvZ2dpbmdTdmMuZXJyb3JIYW5kbGVyKGVycm9yLCAnRmlsZU1hbmFnZXJTZXJ2aWNlJywgJ3NpbmdsZUZpbGVVcGxvYWQnKTtcclxuICAgICAgICAvLyBjb25zdCBtVXBsb2FkU3RhdHVzOiBJVXBsb2FkU3RhdHVzID0gbmV3IFVwbG9hZFN0YXR1cyhhY3Rpb24sIGZpbGUubmFtZSwgZXJyb3IsIHRydWUsIGZhbHNlLCAxLCAxKTtcclxuICAgICAgfSxcclxuICAgICAgLy8gY29tcGxldGU6ICgpID0+IHt9XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIC8qKlxyXG4gICAqIEBkZXNjcmlwdGlvbiBTZXRzIHRoZSBzZWxlY3RlZCBkaXJlY3RvcnkgYW5kIHRyaWdnZXJzIHRoZSBzZWxlY3RlZERpcmVjdG9yeUNoYW5nZWQgZXZlbnQuXHJcbiAgICpcclxuICAgKiBAcGFyYW0ge0lEaXJlY3RvcnlUcmVlfSBkaXJlY3RvcnlUcmVlIC0gVGhlIGRpcmVjdG9yeSB0cmVlIHVzZWQgdG8gc2V0IGFzIHRoZSBzZWxlY3RlZCBkaXJlY3RvcnkuXHJcbiAgICogQHJldHVybiB7dm9pZH0gVGhpcyBmdW5jdGlvbiBkb2VzIG5vdCByZXR1cm4gYW55dGhpbmcuXHJcbiAgICogQG1lbWJlcm9mIEZpbGVNYW5hZ2VyU2VydmljZVxyXG4gICAqL1xyXG4gIHB1YmxpYyBzZXRTZWxlY3RlZERpcmVjdG9yeShkaXJlY3RvcnlUcmVlOiBJRGlyZWN0b3J5VHJlZSk6IHZvaWQge1xyXG4gICAgLy8gY29uc29sZS5sb2coJ2RpcmVjdG9yeVRyZWUuc2V0U2VsZWN0ZWREaXJlY3RvcnknLCBkaXJlY3RvcnlUcmVlKTtcclxuICAgIHRoaXMuX0N1cnJlbnREaXJlY3RvcnlUcmVlID0gZGlyZWN0b3J5VHJlZTtcclxuICAgIHRoaXMuc2VsZWN0ZWREaXJlY3RvcnlDaGFuZ2VkLm5leHQoZGlyZWN0b3J5VHJlZSk7XHJcbiAgfVxyXG4gIFxyXG4gIC8qKlxyXG4gICAqIEBkZXNjcmlwdGlvbiBVcGxvYWRzIGZpbGUgYnkgY2FsbGluZyBlaXRoZXIgbXVsdGlQYXJ0RmlsZVVwbG9hZCBvciBzaW5nbGVGaWxlVXBsb2FkIGRlcGVuZGluZyBvbiB0aGUgZmlsZSBzaXplLlxyXG4gICAqXHJcbiAgICogQHBhcmFtIHtzdHJpbmd9IGFjdGlvbiBVc2VkIHRvIGRldGVybWluZSB0aGUgdXBsb2FkIGRpcmVjdG9yeSBhbmQgZW5mb3JjZSBzZWN1cml0eSBvbiB0aGUgc2VydmVyXHJcbiAgICogQHBhcmFtIHtGaWxlfSBmaWxlIEFuIEhUTUwgXCJGaWxlXCIgb2JqZWN0XHJcbiAgICogQHBhcmFtIHtudW1iZXJ9IFtjaHVua1NpemU9MzA3MjAwMF0gVXNlZCB0byBicmVhayB0aGUgXCJGaWxlXCIgdXAgaWYgdGhlIGZpbGUgc2l6ZSBpcyBncmVhdGVyIHRoYW4gdGhlIGNodW5rU2l6ZS4gIFRoZSBudW1iZXIgaXMgaW4gZGlyZWN0IHJlbGF0aW9uIHRvIEtlc3RyZWxTZXJ2ZXJMaW1pdHMuTWF4UmVxdWVzdEJvZHlTaXplIFByb3BlcnR5XHJcbiAgICogQG1lbWJlcm9mIEZpbGVNYW5hZ2VyU2VydmljZVxyXG4gICAqL1xyXG4gIHVwbG9hZEZpbGUoYWN0aW9uOiBzdHJpbmcsIGZpbGU6IEZpbGUsIGNodW5rU2l6ZTogbnVtYmVyID0gMjk2OTYwMDApXHJcbiAge1xyXG4gICAgY29uc3QgbVRvdGFsTnVtYmVyT2ZVcGxvYWRzOiBudW1iZXIgPSB0aGlzLmdldFRvdGFsTnVtYmVyT2ZVcGxvYWRzKGZpbGUuc2l6ZSwgY2h1bmtTaXplKTtcclxuICAgIGlmIChtVG90YWxOdW1iZXJPZlVwbG9hZHMgPiAxKSB7XHJcbiAgICAgIGNvbnN0IG1NdWx0aVBhcnRGaWxlVXBsb2FkOiBJTXVsdGlQYXJ0RmlsZVVwbG9hZFBhcmFtZXRlcnMgPSBuZXcgTXVsdGlQYXJ0RmlsZVVwbG9hZFBhcmFtZXRlcnMoXHJcbiAgICAgICAgYWN0aW9uLFxyXG4gICAgICAgIGZpbGUsXHJcbiAgICAgICAgbVRvdGFsTnVtYmVyT2ZVcGxvYWRzLFxyXG4gICAgICAgIGNodW5rU2l6ZVxyXG4gICAgICApXHJcbiAgICAgIHRoaXMubXVsdGlQYXJ0RmlsZVVwbG9hZChtTXVsdGlQYXJ0RmlsZVVwbG9hZCk7XHJcbiAgICB9IGVsc2V7XHJcbiAgICAgIHRoaXMuc2luZ2xlRmlsZVVwbG9hZChmaWxlLCBhY3Rpb24pO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG4iXX0=