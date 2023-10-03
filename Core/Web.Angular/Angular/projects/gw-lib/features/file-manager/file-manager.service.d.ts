import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
import { IDirectoryTree } from './directory-tree.model';
import { IUploadStatus } from './upload-status.model';
import * as i0 from "@angular/core";
export declare class FileManagerService {
    private _DataSvc;
    private _GWCommon;
    private _HttpClient;
    private _LoggingSvc;
    private _Api;
    private _Api_GetDirectories;
    private _Api_GetFiles;
    private _Api_CreateDirectory;
    private _Api_DeleteDirectory;
    private _Api_DeleteFile;
    private _Api_RenameDirectory;
    private _Api_RenameFile;
    private _Api_GetTestNaturalSort;
    private _Api_UploadFile;
    private _CurrentDirectoryTree;
    private _SelectedPath;
    set CurrentDirectoryTree(value: IDirectoryTree);
    get CurrentDirectoryTree(): IDirectoryTree;
    get SelectedPath(): string;
    ModalId_Rename_Directory: string;
    ModalId_CreateDirectory: string;
    uploadStatusChanged: Subject<IUploadStatus>;
    selectedDirectoryChanged: Subject<IDirectoryTree>;
    constructor(_DataSvc: DataService, _GWCommon: GWCommon, _HttpClient: HttpClient, _LoggingSvc: LoggingService);
    /**
     * Creates a new directory in the currectly selected directory
     *
     * @param {string} action
     * @param {string} selectedPath
     * @param {string} newPath
     * @return {*}  {Promise<any>}
     * @memberof FileManagerService
     */
    createDirectory(action: string, newPath: string): Promise<any>;
    /**
     * Deletes a directory, subdirectory and all files
     *
     * @param {string} action
     * @param {string} selectedPath
     * @return {*}  {Promise<boolean>}
     * @memberof FileManagerService
     */
    deleteDirectory(action: string, selectedPath: string): Promise<boolean>;
    /**
     * @description Deletes a file from the currently selected path
     *
     * @param {string} action Used to determine the upload directory and enforce security on the server
     * @param {string} fileName The file name to delete
     * @return {*}  {Promise<boolean>}
     * @memberof FileManagerService
     */
    deleteFile(action: string, fileName: string): Promise<boolean>;
    /**
     * @description Retrieves an array of IDirectoryTree for the gien path
     *
     * @param {string} action Used to determine the upload directory and enforce security on the server
     * @param {string} path The relative of the directory path
     * @param {string} forControl - The control to notify.
     * @memberof FileManagerService
     * @returns {Promise<boolean>} A promise that resolves to a boolean indicating the success of the operation.
     */
    getDirectories(action: string, path: string, forControl: string): Promise<boolean>;
    /**
     * @description Retrives a array of IFileInfoLight for the given path
     *
     * @param {string} action Used to determine the upload directory and enforce security on the server
     * @param {string} controlId The id of the controler the files are for
     * @param {string} selectedPath The relative of the directory path
     * @memberof FileManagerService
     */
    getFiles(action: string, controlId: string, selectedPath: string): void;
    getTestNaturalSort(sortDirection: string): Promise<any>;
    /**
     * @description Calculates the total number of chuncks needed to upload a large file
     *
     * @param {number} fileSize  The size of the file
     * @param {number} chunkSize The size of the chunck that the server can accept
     * @return {*}  {number}
     * @memberof FileManagerService
     */
    getTotalNumberOfUploads(fileSize: number, chunkSize: number): number;
    /**
     * @description Uploads files that are too large to be sent in a single upload by using recursion
     * to call the API with a "slice" of the file.
     *
     * @private
     * @param {IMultiPartFileUploadParameters} parameters
     * @memberof FileManagerService
     */
    private multiPartFileUpload;
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
    private multiUploadComplete;
    /**
     * @description Refreshes the specified action.
     *
     * @param {string} action - Used to determine the upload directory and enforce security on the server.
     * @param {IDirectoryTree} directoryTree - Optional directory tree to use to determine the selected path.
     * @return {Promise<any>} A Promise that resolves when the refresh is complete.
     * @memberof FileManagerService
     */
    refresh(action: string, directoryTree?: IDirectoryTree): Promise<any>;
    /**
     * @description Renames a directory asynchronously.
     *
     * @param {string} action - Used to determine the upload directory and enforce security on the server.
     * @param {string} newName - The new name for the directory.
     * @return {Promise<boolean>} A promise that resolves to true if the directory was renamed successfully, or false otherwise.
     * @memberof FileManagerService
     */
    renameDirectory(action: string, newName: string, directoryControlName: string): Promise<boolean>;
    /**
     * @description Renames an existing file
     *
     * @param {string} action Used to determine the upload directory and enforce security on the server
     * @param {string} oldName The name of the file to rename
     * @param {string} newName The new name of the file
     * @return {*}  {Promise<boolean>}
     * @memberof FileManagerService
     */
    renameFile(action: string, oldName: string, newName: string): Promise<boolean>;
    /**
     * @description Performs a single file upload
     *
     * @private
     * @param {File} file the HTML "file" object
     * @param {string} action Used to determine the upload directory and enforce security on the server
     * @memberof FileManagerService
     */
    private singleFileUpload;
    /**
     * @description Sets the selected directory and triggers the selectedDirectoryChanged event.
     *
     * @param {IDirectoryTree} directoryTree - The directory tree used to set as the selected directory.
     * @return {void} This function does not return anything.
     * @memberof FileManagerService
     */
    setSelectedDirectory(directoryTree: IDirectoryTree): void;
    /**
     * @description Uploads file by calling either multiPartFileUpload or singleFileUpload depending on the file size.
     *
     * @param {string} action Used to determine the upload directory and enforce security on the server
     * @param {File} file An HTML "File" object
     * @param {number} [chunkSize=3072000] Used to break the "File" up if the file size is greater than the chunkSize.  The number is in direct relation to KestrelServerLimits.MaxRequestBodySize Property
     * @memberof FileManagerService
     */
    uploadFile(action: string, file: File, chunkSize?: number): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<FileManagerService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<FileManagerService>;
}
